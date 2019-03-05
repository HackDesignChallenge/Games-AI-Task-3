using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LevelManager : MonoBehaviour
{
    public GameObject level;
    public ArrayList blocks;

    private readonly string url = "https://z1.data-qubit.com";
    private readonly string[] snakeColors = { "#6d9cff", "#2d851f", "#c46af6", "#4fce43", "#054158", "#bc0afb", "#054f7c", "#32b000", "#1f6aa3", "#410490" };
    private readonly string[] tunnelColors = { "#f0ff00", "#fdcc00", "#f95c5c", "#ffa951", "#ff9898", "#9f2e08", "#f24c14", "#ff0000", "#ff7878", "#ff3eed" };

    void Start()
    {
        blocks = new ArrayList();
        foreach (Transform block in level.transform)
        {
            blocks.Add(block);
        }
        PostInitVal();
        Invoke("PostTunnelsRequest", 0.5f);
        Invoke("PostSnakesRequest", 1f);
    }
    private async System.Threading.Tasks.Task PostInitVal()
    {
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"init_val\"}", Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                string init_val = await response.Content.ReadAsStringAsync();
            }
        }
    }

    private async System.Threading.Tasks.Task PostSnakesRequest()
    {
        int snakeAColorIndex = 0;
        int snakeBColorIndex = 0;
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"fill_snakes\"}", Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                string snakes = content.Substring(30);
                snakes = snakes.Remove(snakes.Length - 13);
                print("Snakes: " + snakes);
                string[] snakesTilesString = snakes.Split(',');
                int[] snakeTiles = new int[snakesTilesString.Length];
                for (int i = 0;  i < snakesTilesString.Length; i++)
                {
                    snakeTiles[i] = int.Parse(snakesTilesString[i].Trim());
                }
                for (int i = 0; i < snakeTiles.Length; i++)
                {
                    snakeTiles[i] = int.Parse(snakesTilesString[i].Trim());
                    Transform block = blocks[snakeTiles[i] - 1] as Transform;
                    block.GetChild(3).gameObject.SetActive(true);
                    block.GetChild(0).gameObject.SetActive(false);
                    block.GetChild(7).gameObject.SetActive(true);
                    if (ColorUtility.TryParseHtmlString(snakeColors[snakeAColorIndex], out Color color))
                    {
                        color.a = 0.25f;
                        block.GetChild(7).gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
                    }
                    snakeAColorIndex++;
                    i++;
                }
                for (int i = 1; i < snakeTiles.Length; i++)
                {
                    snakeTiles[i] = int.Parse(snakesTilesString[i].Trim());
                    Transform block = blocks[snakeTiles[i] - 1] as Transform;
                    block.GetChild(2).gameObject.SetActive(true);
                    block.GetChild(0).gameObject.SetActive(false);
                    block.GetChild(7).gameObject.SetActive(true);
                    if (ColorUtility.TryParseHtmlString(snakeColors[snakeBColorIndex], out Color color))
                    {
                        color.a = 0.25f;
                        block.GetChild(7).gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
                    }
                    snakeBColorIndex++;
                    i++;
                }
            }
        }
    }

    private async System.Threading.Tasks.Task PostTunnelsRequest()
    {
        int tunnelAColorIndex = 0;
        int tunnelBColorIndex = 0;
        using (var httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
            {
                request.Content = new StringContent("{\"jsonrpc\":\"2.0\",\"id\":\"0\",\"method\":\"fill_tunnels\"}", Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                string tunnels = content.Substring(30);
                tunnels = tunnels.Remove(tunnels.Length - 13);
                print("Tunnels: " + tunnels);
                string[] tunnelsTilesString = tunnels.Split(',');
                int[] tunnelsTiles = new int[tunnelsTilesString.Length];
                for (int i = 0; i < tunnelsTilesString.Length; i++)
                {
                    tunnelsTiles[i] = int.Parse(tunnelsTilesString[i].Trim());
                }
                for (int i = 0; i < tunnelsTilesString.Length; i++)
                {
                    tunnelsTiles[i] = int.Parse(tunnelsTilesString[i].Trim());
                    Transform block = blocks[tunnelsTiles[i] - 1] as Transform;
                    block.GetChild(6).gameObject.SetActive(true);
                    block.GetChild(0).gameObject.SetActive(false);
                    block.GetChild(7).gameObject.SetActive(true);
                    if (ColorUtility.TryParseHtmlString(tunnelColors[tunnelAColorIndex], out Color color))
                    {
                        color.a = 0.25f;
                        block.GetChild(7).gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
                    }
                    tunnelAColorIndex++;
                    i++;
                }
                for (int i = 1; i < tunnelsTilesString.Length; i++)
                {
                    tunnelsTiles[i] = int.Parse(tunnelsTilesString[i].Trim());
                    Transform block = blocks[tunnelsTiles[i] - 1] as Transform;
                    block.GetChild(5).gameObject.SetActive(true);
                    block.GetChild(0).gameObject.SetActive(false);
                    block.GetChild(7).gameObject.SetActive(true);
                    if (ColorUtility.TryParseHtmlString(tunnelColors[tunnelBColorIndex], out Color color))
                    {
                        color.a = 0.25f;
                        block.GetChild(7).gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
                    }
                    tunnelBColorIndex++;
                    i++;
                }

            }
        }
    }
}
