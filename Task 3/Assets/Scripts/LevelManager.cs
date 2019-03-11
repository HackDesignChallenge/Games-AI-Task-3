using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject level;

    private const string URL = "https://z1.data-qubit.com";
    private readonly string[] dungeonColors = {
        "#e6194B", "#ffe119", "#3cb44b", "#4363d8", "#f032e6", 
        "#f58231", "#bfef45", "#42d4f4", "#911eb4", "#a9a9a9" 
    };

    private int[] tunnelsArray;
    private int[] snakesArray;
    private ArrayList blocks;

    private void Awake()
    {
        blocks = new ArrayList();
        foreach (Transform block in level.transform)
        {
            blocks.Add(block);
        }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        ResetGameLevel();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Invoke("GetTunnels", 0.1f);
        Invoke("GetSnakes", 0.2f);
    }

    private async System.Threading.Tasks.Task ResetGameLevel()
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"init_val\"}", Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                string init_val = await response.Content.ReadAsStringAsync();
            }
        }
    }

    private async System.Threading.Tasks.Task GetTunnels()
    {
        var colorIndex = 0;
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"fill_tunnels\"}", Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                var tunnels = content.Substring(30);
                tunnels = tunnels.Remove(tunnels.Length - 13);
                print("Tunnels: " + tunnels);

                var tunnelsStringArray = tunnels.Split(',');
                tunnelsArray = new int[tunnelsStringArray.Length];
                for (var i = 0; i < tunnelsStringArray.Length; i++)
                {
                    Color currentColor = new Color();
                    if (ColorUtility.TryParseHtmlString(dungeonColors[colorIndex], out Color color))
                    {
                        currentColor = color;
                    }

                    // Tunnel Entrance
                    tunnelsArray[i] = int.Parse(tunnelsStringArray[i].Trim());
                    Transform block = blocks[tunnelsArray[i] - 1] as Transform;
                    block.GetChild(3).gameObject.SetActive(true);
                    block.GetChild(3).GetComponent<SpriteRenderer>().color = currentColor;

                    // Tunnel Exit
                    i++;
                    tunnelsArray[i] = int.Parse(tunnelsStringArray[i].Trim());
                    block = blocks[tunnelsArray[i] - 1] as Transform;
                    block.GetChild(4).gameObject.SetActive(true);
                    block.GetChild(4).GetComponent<SpriteRenderer>().color = currentColor;

                    // End of Tunnel pair - increase Color Index
                    colorIndex++;
                }

            }
        }
    }

    private async System.Threading.Tasks.Task GetSnakes()
    {
        var colorIndex = 0;
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"fill_snakes\"}", Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                var snakes = content.Substring(30);
                snakes = snakes.Remove(snakes.Length - 13);
                print("Snakes: " + snakes);

                var snakesStringArray = snakes.Split(',');
                snakesArray = new int[snakesStringArray.Length];
                for (int i = 0; i < snakesStringArray.Length; i++)
                {
                    Color currentColor = new Color();
                    if (ColorUtility.TryParseHtmlString(dungeonColors[colorIndex], out Color color))
                    {
                        currentColor = color;
                    }

                    // Snake Entrance
                    snakesArray[i] = int.Parse(snakesStringArray[i].Trim());
                    Transform block = blocks[snakesArray[i] - 1] as Transform;
                    block.GetChild(1).gameObject.SetActive(true);
                    block.GetChild(1).GetComponent<SpriteRenderer>().color = currentColor;

                    // Snake Exit
                    i++;
                    snakesArray[i] = int.Parse(snakesStringArray[i].Trim());
                    block = blocks[snakesArray[i] - 1] as Transform;
                    block.GetChild(2).gameObject.SetActive(true);
                    block.GetChild(2).GetComponent<SpriteRenderer>().color = currentColor;

                    // End of Snake pair - increase Color Index
                    colorIndex++;
                }
            }
        }
    }

    public int[] GetLevelTunnels()
    {
        return tunnelsArray;
    }

    public int[] GetLevelSnakes()
    {
        return snakesArray;
    }
}
