
Console.ForegroundColor = ConsoleColor.Gray;

Console.WriteLine("Hello, World!");

Console.WriteLine("Ask your Question.");

var question = "history lesson on liberal progress";// Console.ReadLine();";

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var secretProvider = config.Providers.First();
if (secretProvider.TryGet("WhereAmI", out var secretPass))
  Console.WriteLine(secretPass);
else
  Console.WriteLine("Hello, World!");


var answer = OpenAILib.OpenAI.CallOpenAI(config, 1250, question, "text-davinci-002", 0.7, 1, 0, 0);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(answer);

Console.ForegroundColor = ConsoleColor.DarkGray;
