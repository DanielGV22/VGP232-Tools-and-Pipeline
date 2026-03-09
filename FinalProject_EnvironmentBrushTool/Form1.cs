using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FinalProject_EnvironmentBrushTool.Models;
using FinalProject_EnvironmentBrushTool.Services;

namespace FinalProject_EnvironmentBrushTool
{
    public partial class Form1 : Form
    {
        private BrushConfig _currentConfig = new();
        private List<BrushPoint> _previewPoints = new();
        private bool _isInitializing;

        public Form1()
        {
            InitializeComponent();

            _isInitializing = true;
            InitializeUiDefaults();
            HookUiEvents();
            _isInitializing = false;

            RegeneratePreview();
        }

        private void InitializeUiDefaults()
        {
            cmbDistribution.Items.Clear();
            cmbDistribution.Items.Add("Random");
            cmbDistribution.Items.Add("Grid");
            cmbDistribution.SelectedIndex = 0;

            txtBrushName.Text = _currentConfig.BrushName;
            numRadius.Value = (decimal)_currentConfig.Radius;
            numDensity.Value = _currentConfig.Density;
            chkRandomRotation.Checked = _currentConfig.RandomRotation;
            chkRandomScale.Checked = _currentConfig.RandomScale;
            numMinScale.Value = (decimal)_currentConfig.MinScale;
            numMaxScale.Value = (decimal)_currentConfig.MaxScale;
            numSeed.Value = _currentConfig.Seed;

            pnlPreview.Paint += PnlPreview_Paint;
        }

        private void HookUiEvents()
        {
            txtBrushName.TextChanged += AnySettingChanged;
            cmbDistribution.SelectedIndexChanged += AnySettingChanged;
            numRadius.ValueChanged += AnySettingChanged;
            numDensity.ValueChanged += AnySettingChanged;
            numMinScale.ValueChanged += AnySettingChanged;
            numMaxScale.ValueChanged += AnySettingChanged;
            numSeed.ValueChanged += AnySettingChanged;
            chkRandomRotation.CheckedChanged += AnySettingChanged;
            chkRandomScale.CheckedChanged += AnySettingChanged;
        }

        private void AnySettingChanged(object? sender, EventArgs e)
        {
            if (_isInitializing)
                return;

            RegeneratePreview();
        }

        private BrushConfig ReadConfigFromUi()
        {
            return new BrushConfig
            {
                BrushName = string.IsNullOrWhiteSpace(txtBrushName.Text) ? "NewBrush" : txtBrushName.Text.Trim(),
                Radius = (float)numRadius.Value,
                Density = (int)numDensity.Value,
                DistributionType = cmbDistribution.SelectedItem?.ToString() ?? "Random",
                RandomRotation = chkRandomRotation.Checked,
                RandomScale = chkRandomScale.Checked,
                MinScale = (float)numMinScale.Value,
                MaxScale = (float)numMaxScale.Value,
                Seed = (int)numSeed.Value,
                Points = new List<BrushPoint>()
            };
        }

        private void ApplyConfigToUi(BrushConfig config)
        {
            _isInitializing = true;

            txtBrushName.Text = config.BrushName;
            numRadius.Value = (decimal)config.Radius;
            numDensity.Value = config.Density;

            if (cmbDistribution.Items.Contains(config.DistributionType))
                cmbDistribution.SelectedItem = config.DistributionType;
            else
                cmbDistribution.SelectedIndex = 0;

            chkRandomRotation.Checked = config.RandomRotation;
            chkRandomScale.Checked = config.RandomScale;
            numMinScale.Value = (decimal)config.MinScale;
            numMaxScale.Value = (decimal)config.MaxScale;
            numSeed.Value = config.Seed;

            _isInitializing = false;
        }

        private void SetStatus(string message)
        {
            if (lblStatus != null)
                lblStatus.Text = message;
        }

        private void RegeneratePreview()
        {
            try
            {
                _currentConfig = ReadConfigFromUi();
                _previewPoints = BrushGenerator.GeneratePoints(_currentConfig);
                _currentConfig.Points = _previewPoints;
                pnlPreview.Invalidate();

                SetStatus($"Preview updated: {_previewPoints.Count} points generated using {_currentConfig.DistributionType} distribution.");
            }
            catch (Exception ex)
            {
                _previewPoints.Clear();
                pnlPreview.Invalidate();
                SetStatus($"Preview error: {ex.Message}");
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                RegeneratePreview();
                SetStatus($"Generated {_previewPoints.Count} points.");

                MessageBox.Show("Brush pattern generated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Generate failed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _currentConfig = ReadConfigFromUi();
                _currentConfig.Points = _previewPoints;

                using SaveFileDialog dialog = new()
                {
                    Filter = "JSON Files (*.json)|*.json",
                    Title = "Save Brush Config",
                    FileName = $"{_currentConfig.BrushName}.json"
                };

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                JsonFileService.SaveConfig(dialog.FileName, _currentConfig);
                SetStatus($"Saved brush config: {_currentConfig.BrushName}");

                MessageBox.Show("Brush config saved successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog dialog = new()
                {
                    Filter = "JSON Files (*.json)|*.json",
                    Title = "Load Brush Config"
                };

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                _currentConfig = JsonFileService.LoadConfig(dialog.FileName);
                _previewPoints = _currentConfig.Points ?? new List<BrushPoint>();
                SetStatus($"Loaded brush config: {_currentConfig.BrushName}");

                ApplyConfigToUi(_currentConfig);

                if (_previewPoints.Count == 0)
                    RegeneratePreview();
                else
                    pnlPreview.Invalidate();

                MessageBox.Show("Brush config loaded successfully.", "Loaded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load failed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                _currentConfig = ReadConfigFromUi();

                if (_previewPoints.Count == 0)
                    _previewPoints = BrushGenerator.GeneratePoints(_currentConfig);

                _currentConfig.Points = _previewPoints;

                using SaveFileDialog dialog = new()
                {
                    Filter = "JSON Files (*.json)|*.json",
                    Title = "Export Brush Data for Unity",
                    FileName = $"{_currentConfig.BrushName}_Export.json"
                };

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                JsonFileService.SaveConfig(dialog.FileName, _currentConfig);
                SetStatus($"Exported brush JSON for Unity: {_currentConfig.BrushName}");

                MessageBox.Show("Brush JSON exported successfully.", "Exported",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PnlPreview_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int width = pnlPreview.ClientSize.Width;
            int height = pnlPreview.ClientSize.Height;
            int size = Math.Min(width, height) - 20;

            int centerX = width / 2;
            int centerY = height / 2;
            int radiusPixels = size / 2;

            using Pen brushPen = new(Color.DarkGray, 2f);
            e.Graphics.DrawEllipse(
                brushPen,
                centerX - radiusPixels,
                centerY - radiusPixels,
                radiusPixels * 2,
                radiusPixels * 2);

            if (_currentConfig.Radius <= 0f)
                return;

            foreach (BrushPoint point in _previewPoints)
            {
                float normalizedX = point.X / _currentConfig.Radius;
                float normalizedZ = point.Z / _currentConfig.Radius;

                float px = centerX + (normalizedX * radiusPixels);
                float py = centerY + (normalizedZ * radiusPixels);

                float dotSize = Math.Max(4f, point.Scale * 6f);

                e.Graphics.FillEllipse(
                    Brushes.ForestGreen,
                    px - (dotSize / 2f),
                    py - (dotSize / 2f),
                    dotSize,
                    dotSize);
            }
        }
    }
}