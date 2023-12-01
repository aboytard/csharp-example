using CommandLine;
using JsonSchemaGenerator;

class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var options = Parser.Default
                .ParseArguments<CommandLineOptions>(args)
                .WithParsed(o => new Generator(o).Generate());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}