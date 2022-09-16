Console.ForegroundColor = ConsoleColor.Gray;
Console.WriteLine("Ask your Question.");

/// https://beta.openai.com/playground
/// also try https://beta.openai.com/examples
/// 

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build(); //var secretProvider = config.Providers.First(); if (secretProvider.TryGet("WhereAmI", out var secretPass))  Console.WriteLine(secretPass);else  Console.WriteLine("Hello, World!");

var question = ".NET Core 6.0 is amazing! But why?";// Console.ReadLine();";

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(question);

var answer = OpenAILib.OpenAI.CallOpenAI(config, 1250, question);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine(answer);

Console.ForegroundColor = ConsoleColor.DarkGray;
