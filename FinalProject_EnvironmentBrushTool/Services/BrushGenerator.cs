using FinalProject_EnvironmentBrushTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EnvironmentBrushTool.Services
{
    public static class BrushGenerator
    {
        public static List<BrushPoint> GeneratePoints(BrushConfig config)
        {
            ArgumentNullException.ThrowIfNull(config);

            if (config.Radius <= 0)
                throw new ArgumentException("Radius must be greater than 0.");

            if (config.Density <= 0)
                throw new ArgumentException("Density must be greater than 0.");

            if (config.RandomScale && config.MinScale > config.MaxScale)
                throw new ArgumentException("MinScale cannot be greater than MaxScale.");

            return config.DistributionType switch
            {
                "Grid" => GenerateGrid(config),
                _ => GenerateRandom(config)
            };
        }

        private static List<BrushPoint> GenerateRandom(BrushConfig config)
        {
            var random = new Random(config.Seed);
            var points = new List<BrushPoint>();

            for (int i = 0; i < config.Density; i++)
            {
                double angle = random.NextDouble() * Math.PI * 2.0;
                double distance = Math.Sqrt(random.NextDouble()) * config.Radius;

                float x = (float)(Math.Cos(angle) * distance);
                float z = (float)(Math.Sin(angle) * distance);

                float rotationY = config.RandomRotation
                    ? (float)(random.NextDouble() * 360.0)
                    : 0f;

                float scale = config.RandomScale
                    ? Lerp(config.MinScale, config.MaxScale, (float)random.NextDouble())
                    : 1f;

                points.Add(new BrushPoint
                {
                    X = x,
                    Z = z,
                    RotationY = rotationY,
                    Scale = scale
                });
            }

            return points;
        }

        private static List<BrushPoint> GenerateGrid(BrushConfig config)
        {
            var random = new Random(config.Seed);
            var points = new List<BrushPoint>();

            int gridSize = (int)Math.Ceiling(Math.Sqrt(config.Density));
            float spacing = (config.Radius * 2f) / Math.Max(1, gridSize - 1);

            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (points.Count >= config.Density)
                        return points;

                    float x = -config.Radius + (col * spacing);
                    float z = -config.Radius + (row * spacing);

                    if ((x * x) + (z * z) > config.Radius * config.Radius)
                        continue;

                    float rotationY = config.RandomRotation
                        ? (float)(random.NextDouble() * 360.0)
                        : 0f;

                    float scale = config.RandomScale
                        ? Lerp(config.MinScale, config.MaxScale, (float)random.NextDouble())
                        : 1f;

                    points.Add(new BrushPoint
                    {
                        X = x,
                        Z = z,
                        RotationY = rotationY,
                        Scale = scale
                    });
                }
            }

            return points;
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + ((b - a) * t);
        }
    }
}