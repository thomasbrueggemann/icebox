using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using ProtoBuf;

namespace Icebox
{
    public static class IceboxIO
    {
        public static void WriteToDisk(string assemblyName, IReadOnlyCollection<FrozenContract> frozenContracts)
        {
            var filePath = GenerateFilePath(assemblyName);
            using var file = File.Create(filePath);
            
            Serializer.Serialize(file, frozenContracts);
        }

        public static bool ExistsOnDisk(string assemblyName)
        {
            var filePath = GenerateFilePath(assemblyName);
            var fileExistsOnDisk = File.Exists(filePath);

            return fileExistsOnDisk;
        }

        public static IReadOnlyCollection<FrozenContract> ReadFromDisk(string assemblyName)
        {
            var filePath = GenerateFilePath(assemblyName);
            using var file = File.OpenRead(filePath);
            
            var frozenContracts = Serializer.Deserialize<IReadOnlyCollection<FrozenContract>>(file);
            return frozenContracts;
        }

        private static string GenerateFilePath(string assemblyName)
        {
            string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            string path = $"{basePath}{assemblyName}.icebox";
            
            return path;
        }
    }
}