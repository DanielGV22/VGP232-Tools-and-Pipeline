using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace FinalProject_EnvironmentBrushTool.Models
{
    public class BrushConfig
    {
        public string BrushName { get; set; } = "NewBrush";
        public float Radius { get; set; } = 5.0f;
        public int Density { get; set; } = 25;
        public string DistributionType { get; set; } = "Random";
        public bool RandomRotation { get; set; } = true;
        public bool RandomScale { get; set; } = true;
        public float MinScale { get; set; } = 0.8f;
        public float MaxScale { get; set; } = 1.2f;
        public int Seed { get; set; } = 12345;

        public List<BrushPoint> Points { get; set; } = new();
    }
}