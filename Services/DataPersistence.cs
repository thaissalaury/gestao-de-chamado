using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GestaoChamados.Services
{
    public static class DataPersistence
    {
        public static void Save<T>(string fileName, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        public static List<T> Load<T>(string fileName)
        {
            if (!File.Exists(fileName))
                return new List<T>();

            string json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }
}
