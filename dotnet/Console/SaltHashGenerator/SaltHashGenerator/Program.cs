using SaltHashGenerator;

Console.WriteLine("Enter text:");
var text = Console.ReadLine();

if (!string.IsNullOrEmpty(text))
{
    Console.WriteLine("Generating SaltHash...");
    var textSaltHash = SaltHashHelper.GeneratePasswordHashString(text);
    Console.WriteLine("Result:");
    Console.WriteLine(textSaltHash);
    Console.WriteLine("Press any key to close...");
    Console.ReadKey();
} else
{
    Console.WriteLine("Invalid text");
}