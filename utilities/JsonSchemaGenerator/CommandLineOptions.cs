using CommandLine;

namespace JsonSchemaGenerator
{
    public class CommandLineOptions
    {
        [Option('c', "class", Required = false, HelpText = "Class name")]
        public string ClassName { get; set; }

        [Option('a', "assembly", Required = true, HelpText = "Path to assembly with type")]
        public string AssemblyPath { get; set; }

        [Option('d', "destination", Required = false, HelpText = "Destination directory")]
        public string Destination { get; set; }

        [Option('o', "destination", Required = false, HelpText = "Whether to overwrite existing file")]
        public bool Overwrite{ get; set; }
    }
}
