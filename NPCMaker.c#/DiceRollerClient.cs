namespace DiceRollerClient;

using System;
using System.Net.Http;
using System.Threading.Tasks;

class Connector
{
  private static HttpClient DiceBag = new();
  private int Port = 8080;
  private string URI = $"http://localhost:8080/";
  public int diceRollTotal = -1;
  public string returnedTextString = "error";


public void SetPort(string whatToDo)
{
    whatToDo = whatToDo.ToLower();
    Port = whatToDo switch
    {
      "rolldice" => 9021,
      "scantext" => 9022,
      _ => 9020,
    };
    URI = $"http://localhost:{Port}/";
}

public void SetURI(string whatToDo, string withWhat, int colWidth = -1)
{
    URI += whatToDo switch
    {
      "rolldice" => "RollDice/" + withWhat,
      "scantext" => "SearchStringForRolls/" + withWhat + "/" + colWidth,
      _ => "",
    };
}

  public Connector(string whatToDo = "", string withWhat = "", int colWidth = -1)
  {
    SetPort(whatToDo);
    SetURI(whatToDo, withWhat, colWidth);
  }

  public async Task<int> GetRollAsync()
  {
    var diceRollTotal = await GetJsonHttpClient(URI, DiceBag);
    return Int32.Parse(diceRollTotal);
  }

  public async Task<string> GetTextAsync()
  {
    var updatedText = await GetJsonHttpClient(URI, DiceBag);
    return updatedText;
  }

  private static async Task<string> GetJsonHttpClient(string uri, HttpClient httpClient)
  {
    var queryPayload = new HttpRequestMessage(HttpMethod.Get, uri);
    queryPayload.Headers.Add("User-Agent", "C# program by michel@wolfstar.ca");
    var connection = await httpClient.SendAsync(queryPayload);

    var content = await connection.Content.ReadAsStringAsync();
    content ??= "-902";

    Console.WriteLine(">> DEBUG: " + content);
    return content;
  }
}
