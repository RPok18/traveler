using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dapper;
using Microsoft.Data.Sqlite;
using Traveler.Models;

namespace Traveler.Logic
{
    public static class DatabaseHelper
    {
        public static string DbFile { get; set; } = "traveler.db";
        public static string ConnectionString => $"Data Source={DbFile}";

        public static void InitializeDatabase()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS SavedItineraries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Origin TEXT NOT NULL,
                    Destination TEXT NOT NULL,
                    SavedDate TEXT NOT NULL,
                    TotalPrice DECIMAL NOT NULL,
                    TotalCost REAL NOT NULL,
                    SegmentsJson TEXT NOT NULL
                )";

            connection.Execute(createTableQuery);
        }

        public static void SaveItinerary(TravelRequest request, Itinerary itinerary)
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            string insertQuery = @"
                INSERT INTO SavedItineraries (Origin, Destination, SavedDate, TotalPrice, TotalCost, SegmentsJson)
                VALUES (@Origin, @Destination, @SavedDate, @TotalPrice, @TotalCost, @SegmentsJson)";

            var jsonOptions = new JsonSerializerOptions { WriteIndented = false };
            var segmentsJson = JsonSerializer.Serialize(itinerary.Segments, jsonOptions);

            var parameters = new
            {
                Origin = itinerary.Origin,
                Destination = itinerary.Destination,
                SavedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                TotalPrice = itinerary.TotalPrice,
                TotalCost = itinerary.TotalCost,
                SegmentsJson = segmentsJson
            };

            connection.Execute(insertQuery, parameters);
            Console.WriteLine("Itinerary saved to database successfully.");
        }

        public static void GetHistory()
        {
            using var connection = new SqliteConnection(ConnectionString);
            if (!File.Exists(DbFile))
            {
                Console.WriteLine("No saved trips history found.");
                return;
            }
            connection.Open();

            var history = connection.Query<SavedItinerary>("SELECT * FROM SavedItineraries ORDER BY Id DESC");

            if (!history.Any())
            {
                Console.WriteLine("No saved trips history found.");
                return;
            }

            Console.WriteLine("--- Saved Trip History ---");
            foreach (var trip in history)
            {
                Console.WriteLine(trip.ToString());
            }
        }
    }
}
