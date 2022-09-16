namespace OpenAILib;
public class OpenAI
{
  public static string? CallOpenAI(IConfigurationRoot cfg, int max_tokens, string prompt, string model = "text-davinci-002", double temperature = 0.7, int topP = 1, int frequencyPenalty = 0, int presencePenalty = 0)
  {
    var sw = Stopwatch.StartNew();
    var openAiKey = cfg?["OpenAiKey"];
    var apiCall = "https://api.openai.com/v1/completions";

    try
    {
      using var httpClient = new HttpClient();
      using var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall);

      request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAiKey);

      request.Content = new StringContent(
        @$"{{  
  ""model"": ""{model}"",
  ""prompt"": ""{prompt}"",  
  ""temperature"": {temperature},
  ""max_tokens"": {max_tokens},
  ""top_p"": {topP},
  ""frequency_penalty"": {frequencyPenalty},
  ""presence_penalty"": {presencePenalty}
}}");

      request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

      var response = httpClient.SendAsync(request).Result;
      var json = response.Content.ReadAsStringAsync().Result;

      dynamic dynObj = JsonConvert.DeserializeObject(json ?? "what?") ?? "No way!";          //dynamic dynObj = JsonSerializer.Deserialize(json);
      {
        try
        {
          return $" Took {sw.Elapsed.TotalSeconds,5:N1}s   finish_reason: {dynObj.choices[0].finish_reason} \n Prompt: {prompt}\n Text:\n{dynObj.choices[0].text}";
        }
        catch (Exception ex)
        {
          return $"Error: {ex.Message}\n\n         Json:  {((Newtonsoft.Json.Linq.JToken)dynObj).Root}";
        }
      }
    }
    catch (Exception ex)
    {
      return (ex.Message);
    }

    return null;
  }
}