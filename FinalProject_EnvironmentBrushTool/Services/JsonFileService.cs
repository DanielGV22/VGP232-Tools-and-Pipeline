using System;
using System.IO;
using System.Text.Json;
using FinalProject_EnvironmentBrushTool.Models;

namespace FinalProject_EnvironmentBrushTool.Services
{
    public static class JsonFileService
    {
        private static readonly JsonSerializerOptions WriteOptions = new()
        {
            WriteIndented = true
        };

        public static void SaveConfig(string filePath, BrushConfig config)
        {
            ArgumentNullException.ThrowIfNull(filePath);
            ArgumentNullException.ThrowIfNull(config);

            string json = JsonSerializer.Serialize(config, WriteOptions);
            File.WriteAllText(filePath, json);
        }

        public static BrushConfig LoadConfig(string filePath)
        {
            ArgumentNullException.ThrowIfNull(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Config file not found.", filePath);

            string json = File.ReadAllText(filePath);
            BrushConfig? config = JsonSerializer.Deserialize<BrushConfig>(json);

            if (config == null)
                throw new InvalidOperationException("Failed to deserialize brush config.");

            return config;
        }
    }
}