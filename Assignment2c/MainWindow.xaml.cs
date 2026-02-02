using Microsoft.Win32;
using System.Windows;
using System;
using Assignment2b;
using System.Windows.Controls;

namespace Assignment2c
{
    public partial class MainWindow : Window
    {
        private WeaponCollection mWeaponCollection;
        private bool mIsFiltered;
        private WeaponCollection mFilteredView;

        public MainWindow()
        {
            InitializeComponent();

            mWeaponCollection = new WeaponCollection();
            WeaponListBox.ItemsSource = mWeaponCollection;

            // Populate type filter dropdown with enum names
            FilterTypeOnlyComboBox.Items.Clear();
            FilterTypeOnlyComboBox.Items.Add("All");
            foreach (var name in Enum.GetNames(typeof(Weapon.WeaponType)))
                FilterTypeOnlyComboBox.Items.Add(name);

            FilterTypeOnlyComboBox.SelectedIndex = 0; // All
        }

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Weapon Files (*.csv;*.json;*.xml)|*.csv;*.json;*.xml|All files (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                bool ok = mWeaponCollection.Load(dlg.FileName);
                if (!ok)
                {
                    MessageBox.Show("Failed to load file (invalid path or invalid data).",
                        "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                ResetViewToMainCollection();
                WeaponListBox.Items.Refresh();
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv|JSON (*.json)|*.json|XML (*.xml)|*.xml|All files (*.*)|*.*",
                AddExtension = true
            };

            if (dlg.ShowDialog() == true)
            {
                var current = GetCurrentViewCollection();
                bool ok = current.Save(dlg.FileName);

                if (!ok)
                {
                    MessageBox.Show("Failed to save file (invalid path or unsupported extension).",
                        "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            var win = new EditWeaponWindow();
            win.SetupForAdd();

            if (win.ShowDialog() == true && win.TempWeapon != null)
            {
                mWeaponCollection.Add(win.TempWeapon);
                ApplySortIfAny();
                WeaponListBox.Items.Refresh();
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            if (WeaponListBox.SelectedItem is not Weapon selected)
            {
                MessageBox.Show("Select a weapon first.", "Edit", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var win = new EditWeaponWindow();
            win.SetupForEdit(selected);

            if (win.ShowDialog() == true && win.TempWeapon != null)
            {
                // Update the selected item in-place
                CopyWeapon(win.TempWeapon, selected);

                ApplySortIfAny();
                WeaponListBox.Items.Refresh();
            }
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            if (WeaponListBox.SelectedItem is not Weapon selected)
            {
                MessageBox.Show("Select a weapon first.", "Remove", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Always remove from the main source collection
            mWeaponCollection.Remove(selected);

            // If filtered, rebuild the filtered view so UI stays consistent
            if (mIsFiltered && mFilteredView != null)
            {
                // Re-apply the current filter selection
                FilterTypeOnlySelectionChanged(FilterTypeOnlyComboBox, null);
            }

            WeaponListBox.Items.Refresh();
        }



        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            ApplySortIfAny();
            WeaponListBox.Items.Refresh();
        }

        private void FilterTypeOnlySelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FilterTypeOnlyComboBox.SelectedItem == null)
                return;

            string selected = FilterTypeOnlyComboBox.SelectedItem.ToString();

            if (string.Equals(selected, "All", StringComparison.OrdinalIgnoreCase))
            {
                ResetViewToMainCollection();
                WeaponListBox.Items.Refresh();
                return;
            }

            if (!Enum.TryParse(selected, out Weapon.WeaponType type))
            {
                ResetViewToMainCollection();
                WeaponListBox.Items.Refresh();
                return;
            }

            var list = mWeaponCollection.GetAllWeaponsOfType(type);
            mFilteredView = new WeaponCollection();
            mFilteredView.AddRange(list);

            mIsFiltered = true;
            WeaponListBox.ItemsSource = mFilteredView;

            ApplySortIfAny();
            WeaponListBox.Items.Refresh();
        }

        // ---------- Helpers ----------

        private WeaponCollection GetCurrentViewCollection()
        {
            return mIsFiltered && mFilteredView != null ? mFilteredView : mWeaponCollection;
        }

        private void ResetViewToMainCollection()
        {
            mIsFiltered = false;
            mFilteredView = null;
            WeaponListBox.ItemsSource = mWeaponCollection;
        }

        private void ApplySortIfAny()
        {
            var current = GetCurrentViewCollection();

            string sortColumn = "Name";
            foreach (var rb in FindVisualChildren<System.Windows.Controls.RadioButton>(this))
            {
                if (rb.GroupName == "SortGroup" && rb.IsChecked == true)
                {
                    sortColumn = rb.Content?.ToString() ?? "Name";
                    break;
                }
            }

            if (current != null)
                current.SortBy(sortColumn);

        }

        private static void CopyWeapon(Weapon from, Weapon to)
        {
            to.Name = from.Name;
            to.Type = from.Type;
            to.Image = from.Image;
            to.Rarity = from.Rarity;
            to.BaseAttack = from.BaseAttack;
            to.SecondaryStat = from.SecondaryStat;
            to.Passive = from.Passive;
        }

        private static System.Collections.Generic.IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}
