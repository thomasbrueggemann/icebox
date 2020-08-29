using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommandLine;
using static Icebox.IceboxIO;

namespace Icebox.CLI
{
    public sealed class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('a', "assemblies", Required = true, HelpText = "Comma-separated assembly-paths to inspect")]
        public IEnumerable<string> Assemblies { get; set; }
        
        [Option('o', "output", Required = true, HelpText = "The output path to write the icebox files to")]
        public string OutputPath { get; set; }
    }
    
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);
        }

        private static void Run(Options opts)
        {
            foreach (string assemblyPath in opts.Assemblies)
            {
                var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                
                var icebox = new Icebox(assembly);
                var iceboxName = icebox.Name;
                if (iceboxName == null)
                {
                    Console.Error.WriteLine("Assembly name could not be determined");
                }

                var iceboxExistsOnDisk = ExistsOnDisk(opts.OutputPath, iceboxName);
                if (!iceboxExistsOnDisk)
                {
                    var writeContracts = icebox.Freeze();
                    
                    WriteToDisk(opts.OutputPath, iceboxName, writeContracts);
                    return;
                }

                var readContracts = ReadFromDisk(opts.OutputPath, iceboxName);
                var results = icebox.FindMatchingTypesToIceboxedContracts(readContracts);

                Console.WriteLine(JsonSerializer.Serialize(results));
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            var errors = JsonSerializer.Serialize(errs);
            Console.Error.WriteLine(errors);
        }
    }
}