using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using ProtoBuf;

namespace Icebox
{
    public static class IceboxIO
    {
        public static void WriteFrozenContract(FrozenContract frozenContract, bool storeIceboxesAlongsideSourceFiles)
        {
            var filePath = GenerateFilePath(frozenContract, storeIceboxesAlongsideSourceFiles);
            using var file = File.Create(filePath);
            
            Serializer.Serialize(file, frozenContract);
        }

        public static FrozenContract ReadFrozenContract(Type type, string basePath)
        {
            var filePath = "";//GenerateFilePath(basePath, "");
            using var file = File.OpenRead(filePath);
            
            var frozenContracts = Serializer.Deserialize<FrozenContract>(file);
            return frozenContracts;
        }

        private static string GenerateFilePath(FrozenContract frozenContract, bool storeIceboxesAlongsideSourceFiles)
        {
            string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

            if (storeIceboxesAlongsideSourceFiles)
            {
                basePath = "";
            }

            string path = $"{basePath}{frozenContract.Name}.icebox";
            return path;
        }
    }
}