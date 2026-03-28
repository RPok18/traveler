using System;
using System.IO;
using System.Text.Json;
using Traveler.Models;

namespace Traveler.Logic
{
    public static class JsonExporter
    {
        public static void ExportToPath(Itinerary itinerary, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(itinerary, options);

                string? directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, json);
                Console.WriteLine($"Itinerary exported to {filePath} successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting itinerary to {filePath}: {ex.Message}");
            }
        }
    }
}
