namespace OpenAILib;
public class OpenAI
{
  public static (TimeSpan ts, string finishReason, string answer) CallOpenAI(IConfigurationRoot cfg, int max_tokens, string prompt, string model = "text-davinci-002", double temperature = 0.7, int topP = 1, int frequencyPenalty = 0, int presencePenalty = 0)
  {
    var sw = Stopwatch.StartNew();
    var openAiKey = cfg?["OpenAiKey"] + "TpTu3Q";
    var url = "https://api.openai.com/v1/completions";
    var jsonString = @$"{{  
  ""model"": ""{model}"",
  ""prompt"": ""{prompt}"",  
  ""temperature"": {temperature},
  ""max_tokens"": {max_tokens},
  ""top_p"": {topP},
  ""frequency_penalty"": {frequencyPenalty},
  ""presence_penalty"": {presencePenalty}
}}";

    WriteLine(jsonString);

    try
    {
      using var httpClient = new HttpClient();
      using var requestMsg = new HttpRequestMessage(new HttpMethod("POST"), url);

      requestMsg.Headers.TryAddWithoutValidation("Authorization", $"Bearer {openAiKey}");
      requestMsg.Content = new StringContent(jsonString);
      requestMsg.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

      var response = httpClient.SendAsync(requestMsg).Result;
      var json = response.Content.ReadAsStringAsync().Result;

      dynamic dynObj = JsonConvert.DeserializeObject(json ?? "what?") ?? "No way!";          //dynamic dynObj = JsonSerializer.Deserialize(json);

      try
      {
        return (sw.Elapsed, dynObj.choices[0].finish_reason.ToString(), dynObj.choices[0].text.ToString());
      }
      catch (Exception ex)
      {
        return (sw.Elapsed, ex.Message, $"{((Newtonsoft.Json.Linq.JToken)dynObj).Root}\n\n{jsonString}");
      }
    }
    catch (Exception ex)
    {
      return (sw.Elapsed, ex.Message, ex.ToString());
    }
  }
}