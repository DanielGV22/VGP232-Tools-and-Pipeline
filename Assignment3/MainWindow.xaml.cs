using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Assignment3
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<ImageItem> _images = new();

        private string? _projectXmlPath;

        public MainWindow()
        {
            InitializeComponent();

            lbImages.ItemsSource = _images;

            txtStatus.Text = "Ready.";
        }

        // ========= File Menu =========

        private void MiNew_Click(object sender, RoutedEventArgs e)
        {
            if (HasUnsavedWork())
            {
                var res = MessageBox.Show(
                    "You have an existing project. Would you like to save first?",
                    "Save?",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (res == MessageBoxResult.Cancel) return;

                if (res == MessageBoxResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(_projectXmlPath))
                        MiSaveAs_Click(sender, e);
                    else
                        MiSave_Click(sender, e);
                }
            }

            ClearAll();
            txtStatus.Text = "New project (cleared).";
        }

        private void MiOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Open SpriteSheet Project (.xml)",
                Filter = "SpriteSheet Project (*.xml)|*.xml|All Files (*.*)|*.*",
                Multiselect = false
            };

            if (dlg.ShowDialog() == true)
            {
                _projectXmlPath = dlg.FileName;
                tbProjectXml.Text = System.IO.Path.GetFileName(_projectXmlPath);
                miSave.IsEnabled = true;

                MessageBox.Show("Open is stubbed for now. Next step: load the XML project.",
                    "Not implemented yet",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                txtStatus.Text = $"Selected project: {tbProjectXml.Text}";
            }
        }

        private void MiSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_projectXmlPath))
            {
                MiSaveAs_Click(sender, e);
                return;
            }

            MessageBox.Show("Save is stubbed for now. Next step: write project XML.",
                "Not implemented yet",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            txtStatus.Text = $"Saved: {System.IO.Path.GetFileName(_projectXmlPath)}";
        }

        private void MiSaveAs_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Title = "Save SpriteSheet Project (.xml)",
                Filter = "SpriteSheet Project (*.xml)|*.xml|All Files (*.*)|*.*",
                AddExtension = true,
                DefaultExt = ".xml",
                FileName = "SpriteSheet.xml"
            };

            if (dlg.ShowDialog() == true)
            {
                _projectXmlPath = dlg.FileName;
                tbProjectXml.Text = System.IO.Path.GetFileName(_projectXmlPath);
                miSave.IsEnabled = true;

                MessageBox.Show("Save As is stubbed for now. Next step: write project XML.",
                    "Not implemented yet",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                txtStatus.Text = $"Save As selected: {tbProjectXml.Text}";
            }
        }

        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            if (HasUnsavedWork())
            {
                var res = MessageBox.Show(
                    "Would you like to save the project before exiting?",
                    "Exit",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (res == MessageBoxResult.Cancel) return;
                if (res == MessageBoxResult.Yes)
                {
                    if (string.IsNullOrWhiteSpace(_projectXmlPath))
                        MiSaveAs_Click(sender, e);
                    else
                        MiSave_Click(sender, e);
                }
            }

            Close();
        }

        // ========= Buttons =========

        // Browse… (SaveFileDialog) -> fill tbOutputDir + tbOutputFile
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Title = "Choose Output SpriteSheet (.png)",
                Filter = "PNG Image (*.png)|*.png|All Files (*.*)|*.*",
                AddExtension = true,
                DefaultExt = ".png",
                FileName = string.IsNullOrWhiteSpace(tbOutputFile.Text) ? "SpriteSheet.png" : tbOutputFile.Text
            };

            if (dlg.ShowDialog() == true)
            {
                tbOutputDir.Text = System.IO.Path.GetDirectoryName(dlg.FileName) ?? "";
                tbOutputFile.Text = System.IO.Path.GetFileName(dlg.FileName);

                txtStatus.Text = $"Output set: {tbOutputDir.Text}\\{tbOutputFile.Text}";
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Add PNG Images",
                Filter = "PNG Image (*.png)|*.png|All Files (*.*)|*.*",
                Multiselect = true
            };

            if (dlg.ShowDialog() != true) return;

            int added = 0;
            foreach (var path in dlg.FileNames)
            {
                if (!File.Exists(path)) continue;

                // Avoid duplicates
                if (_images.Any(i => string.Equals(i.Path, path, StringComparison.OrdinalIgnoreCase)))
                    continue;

                _images.Add(new ImageItem(path));
                added++;
            }

            txtStatus.Text = added > 0 ? $"Added {added} image(s)." : "No new images added (maybe duplicates).";
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbImages.SelectedItem is ImageItem selected)
            {
                _images.Remove(selected);
                txtStatus.Text = "Removed selected image.";
            }
            else
            {
                txtStatus.Text = "Nothing selected to remove.";
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out int columns))
                return;

            if (_images.Count == 0)
            {
                MessageBox.Show("Please add at least one PNG image before generating.",
                    "Missing images",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(
                "Generate is wired and validated.\n.",
                "Generate (stub)",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            txtStatus.Text = $"Generate requested ({columns} columns, {_images.Count} images).";

            TextureAtlasLib.Spritesheet sheet = new TextureAtlasLib.Spritesheet
            {
                Columns = columns,
                OutputDirectory = tbOutputDir.Text,
                OutputFile = tbOutputFile.Text,
                IncludeMetaData = cbIncludeMetaData.IsChecked == true,
                InputPaths = _images.Select(i => i.Path).ToList()
            };

        }

        // ========= Helpers =========

        private bool ValidateInputs(out int columns)
        {
            columns = 0;

            // Output directory
            if (string.IsNullOrWhiteSpace(tbOutputDir.Text))
            {
                MessageBox.Show("Please choose an Output Directory (use Browse...).",
                    "Missing Output Directory",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            // Output filename
            if (string.IsNullOrWhiteSpace(tbOutputFile.Text))
            {
                MessageBox.Show("Please enter an output filename (e.g., SpriteSheet.png).",
                    "Missing Filename",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            // Columns
            if (!int.TryParse(tbColumns.Text, out columns) || columns <= 0)
            {
                MessageBox.Show("Columns must be a positive integer (e.g., 6).",
                    "Invalid Columns",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            if (!Directory.Exists(tbOutputDir.Text))
            {
                MessageBox.Show("Output directory does not exist.",
                    "Invalid Output Directory",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            if (!tbOutputFile.Text.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                tbOutputFile.Text += ".png";

            return true;
        }

        private bool HasUnsavedWork()
        {
            return !string.IsNullOrWhiteSpace(tbOutputDir.Text)
                || !string.IsNullOrWhiteSpace(tbOutputFile.Text)
                || !string.IsNullOrWhiteSpace(tbColumns.Text)
                || _images.Count > 0;
        }

        private void ClearAll()
        {
            tbOutputDir.Text = "";
            tbOutputFile.Text = "";
            tbColumns.Text = "";
            cbIncludeMetaData.IsChecked = false;
            _images.Clear();

            _projectXmlPath = null;
            tbProjectXml.Text = "(not saved yet)";
            miSave.IsEnabled = false;
        }
        private void OpenExplorerToOutput()
        {
            var dir = tbOutputDir.Text;
            if (Directory.Exists(dir))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = dir,
                    UseShellExecute = true
                });
            }
        }
    }

    public sealed class ImageItem
    {
        public string Path { get; }
        public string FileName => System.IO.Path.GetFileName(Path);
        public BitmapImage Thumbnail { get; }

        public ImageItem(string path)
        {
            Path = path;

            Thumbnail = new BitmapImage();
            Thumbnail.BeginInit();
            Thumbnail.CacheOption = BitmapCacheOption.OnLoad;
            Thumbnail.UriSource = new Uri(path, UriKind.Absolute);
            Thumbnail.DecodePixelWidth = 220; // keeps memory low
            Thumbnail.EndInit();
            Thumbnail.Freeze();
        }
    }
}