using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using ProtoBuf;

namespace Icebox
{
    public static class IceboxIO
    {
        public static IReadOnlyCollection<string> FindAllIceboxFiles()
        {
            string currentAssemblyDirectory = Directory.GetCurrentDirectory();
            
            string[] files = Directory.GetFiles(
                currentAssemblyDirectory, 
                "*.icebox", 
                SearchOption.AllDirectories);
            
            return files
                .ToList()
                .AsReadOnly();
        }
        
        public static void WriteToDisk(
            string outputPath, 
            string? assemblyName, 
            IReadOnlyCollection<IceboxedContract> iceboxedContracts)
        {
            if (assemblyName == null)
            {
                return;
            }
            
            var filePath = GenerateFilePath(outputPath, assemblyName);
            using var file = File.Create(filePath);
            
            Serializer.Serialize(file, iceboxedContracts);
        }

        public static bool ExistsOnDisk(string outputPath, string? assemblyName)
        {
            if (assemblyName == null)
            {
                return false;
            }
            
            var filePath = GenerateFilePath(outputPath, assemblyName);
            var fileExistsOnDisk = File.Exists(filePath);

            return fileExistsOnDisk;
        }

        public static IReadOnlyCollection<IceboxedContract> ReadFromDisk(string outputPath, string? assemblyName)
        {
            if (assemblyName == null)
            {
                return new List<IceboxedContract>();
            }
            
            var filePath = GenerateFilePath(outputPath, assemblyName);
            using var file = File.OpenRead(filePath);
            
            var frozenContracts = Serializer.Deserialize<IReadOnlyCollection<IceboxedContract>>(file);
            return frozenContracts;
        }

        private static string GenerateFilePath(string outputPath, string assemblyName)
        {
            string iceboxPath = $"{outputPath}/{assemblyName}.icebox";
            return iceboxPath;
        }
    }
}