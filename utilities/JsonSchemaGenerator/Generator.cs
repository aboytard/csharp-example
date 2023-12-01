using Newtonsoft.Json.Schema.Generation;

using System.Reflection;

namespace JsonSchemaGenerator
{
    internal class Generator
    {
        private const string outputFolder = "Schemas";

        private readonly string className;
        private readonly Assembly assembly;
        private readonly string destination;
        private readonly bool overwrite;
        private readonly JSchemaGenerator generator = new();


        public Generator(CommandLineOptions options)
        {
            className = options.ClassName;
            assembly = Assembly.LoadFrom(options.AssemblyPath);
            destination = options.Destination;
            overwrite = options.Overwrite;
            generator.GenerationProviders.Add(new StringEnumGenerationProvider());
        }

        private string GetPath(string typeName) => Path.Combine(string.IsNullOrEmpty(destination) ? Path.Combine(AppContext.BaseDirectory, outputFolder) : destination,
                    $"{assembly.GetName().Name ?? "empty"}_{assembly.GetName().Version?.ToString() ?? "empty"}",
                    $"{typeName}.json");
        public void Generate()
        {
            var type = assembly.GetTypes()
                .FirstOrDefault(t => string.Equals(t.Name, className, StringComparison.CurrentCultureIgnoreCase));

            if (type == default)
            {
                foreach (var t in assembly.GetTypes())
                {
                    Generate(t);
                }
            }
            else
            {
                Generate(type);
            }
        }

        private void Generate(Type type) 
        {
            var path = GetPath(type.Name);

            if (overwrite || !File.Exists(path)) 
            {
                var schema = generator.Generate(type);
                
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllText(path, schema.ToString());
            }
        }
    }
}
