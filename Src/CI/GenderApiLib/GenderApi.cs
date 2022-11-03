using System.Diagnostics;
using System.Security.Policy;
using GenderApiLib.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GenderApiLib;
public class GenderApi
{
  public static async Task<(TimeSpan ts, string finishReason, FirstnameRootObject? root)> CallOpenAI(IConfigurationRoot cfg, string firstName)
  {
    var stopwatch = Stopwatch.StartNew();
    var filename = $@"C:\g\CI\Src\CI\NameOrigin\GenderApiResults\FirstName-{firstName}.json";

    try
    {
      var jsonPart = File.Exists(filename) ? await File.ReadAllTextAsync(filename) : await GetFromWeb(cfg, firstName);
      if (jsonPart is null)
        return (stopwatch.Elapsed, "jsonPart is null", null);

      await File.WriteAllTextAsync(filename, jsonPart);

      var dynOb2 = JsonConvert.DeserializeObject<FirstnameRootObject>(jsonPart);          //dynamic dynObj = JsonConvert.DeserializeObject(jsonPart) ?? "No way!";          //dynamic dynObj = JsonSerializer.Deserialize(json);

      try
      {
        return (stopwatch.Elapsed, "", dynOb2);
      }
      catch (Exception ex)
      {
        return (stopwatch.Elapsed, ex.Message, null);
      }
    }
    catch (Exception ex)
    {
      return (stopwatch.Elapsed, ex.Message, null);
    }
  }

  private static async Task<string> GetFromWeb(IConfigurationRoot cfg, string firstName)
  {
    var key = cfg?["GenderApi"] + "6c7ef4601";
    var url = $"https://gender-api.com/get-country-of-origin?name={firstName}&key={key}";

    using var httpClient = new HttpClient();
    using var requestMsg = new HttpRequestMessage(new HttpMethod("POST"), url);

    var response = await httpClient.SendAsync(requestMsg);//.Result;
    var jsonPart = await response.Content.ReadAsStringAsync();//.Result;
    return jsonPart;
  }
}