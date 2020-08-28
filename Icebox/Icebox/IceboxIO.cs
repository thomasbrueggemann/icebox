using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProtoBuf;

namespace Icebox
{
    public static class IceboxIO
    {
        public static void WriteToDisk(string assemblyName, IReadOnlyCollection<IceboxedContract> iceboxedContracts)
        {
            var filePath = GenerateFilePath(assemblyName);
            using var file = File.Create(filePath);
            
            Serializer.Serialize(file, iceboxedContracts);
        }

        public static bool ExistsOnDisk(string assemblyName)
        {
            var filePath = GenerateFilePath(assemblyName);
            var fileExistsOnDisk = File.Exists(filePath);

            return fileExistsOnDisk;
        }

        public static IReadOnlyCollection<IceboxedContract> ReadFromDisk(string assemblyName)
        {
            var filePath = GenerateFilePath(assemblyName);
            using var file = File.OpenRead(filePath);
            
            var frozenContracts = Serializer.Deserialize<IReadOnlyCollection<IceboxedContract>>(file);
            return frozenContracts;
        }

        private static string GenerateFilePath(string assemblyName)
        {
            string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string projectPath = GetParentDirectoryWithProjectFile(basePath);
            string iceboxPath = $"{projectPath}/{assemblyName}.icebox";
            
            return iceboxPath;
        }

        private static string GetParentDirectoryWithProjectFile(string path)
        {
            var parentDirectory = Directory.GetParent(path);

            if (parentDirectory == null)
            {
                return path;
            }
            
            var filesInFolder = Directory.GetFiles(parentDirectory.FullName);

            if (filesInFolder.Any(f => f.EndsWith(".csproj") || f.EndsWith(".vbproj")))
            {
                return parentDirectory.FullName;
            }

            return GetParentDirectoryWithProjectFile(parentDirectory.FullName);
        }
    }
}