using System;
using System.Collections.Generic;
using System.Text.Json;
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
            Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(Run);
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
                    Console.Error.WriteLine("⚠️ Assembly name could not be determined");
                }

                var iceboxExistsOnDisk = ExistsOnDisk(opts.OutputPath, iceboxName);
                if (!iceboxExistsOnDisk)
                {
                    var writeContracts = icebox.Freeze();
                    
                    WriteToDisk(opts.OutputPath, iceboxName, writeContracts);
                    Console.WriteLine("✅ Done! Icebox has been written. To checks were carried out, " +
                                      "since Icebox file did not exist prior to this run");
                }
                else
                {
                    var readContracts = ReadFromDisk(opts.OutputPath, iceboxName);
                    var results = icebox.FindMatchingTypesToIceboxedContracts(readContracts);

                    PrintResults(results);
                }
            }
        }

        private static void PrintResults(IceboxMatchResults results)
        {
            if (results.Succeeded)
            {
                PrintResultsSuccess();
            }
            else
            {
                PrintResultsFailure(results.BreakingChanges);
            }
        }

        private static void PrintResultsSuccess()
        {
            Console.WriteLine("✅ All good, no breaking changes detected!");
        }

        private static void PrintResultsFailure(IReadOnlyCollection<IceboxBreakingChange> breakingChanges)
        {
            Console.WriteLine($"⚠️ {breakingChanges.Count} breaking changes detected:");

            foreach (var breakingChange in breakingChanges)
            {
                Console.WriteLine($"  - {breakingChange?.Contract?.Name}");
            }
        }
    }
}