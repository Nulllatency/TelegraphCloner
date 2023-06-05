// See https://aka.ms/new-console-template for more information
using TelegraphCloner;

TelegraphManager telegraphCloner = new TelegraphManager();

string startMessage = "Must fit:\norigin_url pages_amount pages_name author_name";

try
{
    int count = int.Parse(args[1]);

    while (count > 0)
        count = await telegraphCloner.ClonePage(args[0], count, args[2], args[3]);

    Console.WriteLine("END");
} 
catch
{
    Console.WriteLine(startMessage);
}
Console.ReadKey();