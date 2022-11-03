using NameOrigin.Code1stModelGen;

namespace NameOrigin;
public class GenderApi
{
  public static async Task<(TimeSpan ts, string finishReason, FirstnameRootObject? root)> CallOpenAI(IConfigurationRoot cfg, string firstName)
  {
    var sw = Stopwatch.StartNew();
    var key = cfg?["GenderApi"] + "6c7ef4601";
    var url = $"https://gender-api.com/get-country-of-origin?name={firstName}&key={key}";

    try
    {
      using var httpClient = new HttpClient();
      using var requestMsg = new HttpRequestMessage(new HttpMethod("POST"), url);

      var response = await httpClient.SendAsync(requestMsg);//.Result;
      var jsonPart = await response.Content.ReadAsStringAsync();//.Result;

      if(jsonPart is null) 
        return (sw.Elapsed, "jsonPart is null", null);

      await File.WriteAllTextAsync($@"C:\g\CI\Src\CI\NameOrigin\GenderApiResults\FirstName-{firstName}-{DateTime.Now.Second}.json", jsonPart);

      var dynOb2 = JsonConvert.DeserializeObject<FirstnameRootObject>(jsonPart);          //dynamic dynObj = JsonConvert.DeserializeObject(jsonPart) ?? "No way!";          //dynamic dynObj = JsonSerializer.Deserialize(json);

      try
      {
        return (sw.Elapsed, "", dynOb2);
      }
      catch (Exception ex)
      {
        return (sw.Elapsed, ex.Message, null);
      }
    }
    catch (Exception ex)
    {
      return (sw.Elapsed, ex.Message, null);
    }
  }
}