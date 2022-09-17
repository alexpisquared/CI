namespace OpenAILib;
public class OpenAI
{
  public static (TimeSpan ts, string finishReason, string answer) CallOpenAI(IConfigurationRoot cfg, int max_tokens, string prompt, string model = "text-davinci-002", double temperature = 0.7, int topP = 1, int frequencyPenalty = 0, int presencePenalty = 0)
  {
    var sw = Stopwatch.StartNew();
    var openAiKey = cfg?["OpenAiKey"] + "TpTu3Q";
    var apiCall = "https://api.openai.com/v1/completions";

    try
    {
      using var httpClient = new HttpClient();
      using var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall);

      request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAiKey);

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

      request.Content = new StringContent(jsonString);

      request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

      var response = httpClient.SendAsync(request).Result;
      var json = response.Content.ReadAsStringAsync().Result;

      dynamic dynObj = JsonConvert.DeserializeObject(json ?? "what?") ?? "No way!";          //dynamic dynObj = JsonSerializer.Deserialize(json);
      {
        try
        {
          return (sw.Elapsed, dynObj.choices[0].finish_reason.ToString(), dynObj.choices[0].text.ToString());
        }
        catch (Exception ex)
        {
          return (sw.Elapsed, ex.Message, ((Newtonsoft.Json.Linq.JToken)dynObj).Root.ToString());
        }
      }
    }
    catch (Exception ex)
    {
      return (sw.Elapsed, ex.Message, ex.ToString());
    }
  }
}