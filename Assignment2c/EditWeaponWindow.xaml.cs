using System;
using System.Windows;
using Assignment2b;

namespace Assignment2c
{
    public partial class EditWeaponWindow : Window
    {
        public Weapon TempWeapon { get; private set; }

        public EditWeaponWindow()
        {
            InitializeComponent();

            // Populate Type combo from enum names 
            TypeComboBox.Items.Clear();
            foreach (var name in Enum.GetNames(typeof(Weapon.WeaponType)))
                TypeComboBox.Items.Add(name);

            // Populate Rarity combo (1-5)
            RarityComboBox.Items.Clear();
            for (int i = 1; i <= 5; i++)
                RarityComboBox.Items.Add(i.ToString());

            // Defaults
            TypeComboBox.SelectedItem = Weapon.WeaponType.None.ToString();
            RarityComboBox.SelectedItem = "1";
            BaseAttackTextBox.Text = "0";

            ErrorTextBlock.Text = string.Empty;
        }

        // Called by MainWindow before ShowDialog() in ADD mode
        public void SetupForAdd()
        {
            Title = "Add Weapon";
            SubmitButton.Content = "Add";
            ErrorTextBlock.Text = string.Empty;

            TempWeapon = null;

            NameTextBox.Text = string.Empty;
            ImageTextBox.Text = string.Empty;
            SecondaryStatTextBox.Text = string.Empty;
            PassiveTextBox.Text = string.Empty;

            TypeComboBox.SelectedItem = Weapon.WeaponType.None.ToString();
            RarityComboBox.SelectedItem = "1";
            BaseAttackTextBox.Text = "0";
        }

        // Called by MainWindow before ShowDialog() in EDIT mode
        public void SetupForEdit(Weapon existing)
        {
            Title = "Edit Weapon";
            SubmitButton.Content = "Save";
            ErrorTextBlock.Text = string.Empty;

            if (existing == null)
                return;

            // Populate UI directly from selected weapon
            NameTextBox.Text = existing.Name;
            ImageTextBox.Text = existing.Image;
            SecondaryStatTextBox.Text = existing.SecondaryStat;
            PassiveTextBox.Text = existing.Passive;

            TypeComboBox.SelectedItem = existing.Type.ToString();
            RarityComboBox.SelectedItem = existing.Rarity.ToString();
            BaseAttackTextBox.Text = existing.BaseAttack.ToString();

            // TempWeapon will be created on Submit
            TempWeapon = null;
        }

        private void SubmitClicked(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = string.Empty;

            string name = NameTextBox.Text?.Trim() ?? string.Empty;
            string image = ImageTextBox.Text?.Trim() ?? string.Empty;
            string secondary = SecondaryStatTextBox.Text?.Trim() ?? string.Empty;
            string passive = PassiveTextBox.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorTextBlock.Text = "Name is required.";
                return;
            }

            if (TypeComboBox.SelectedItem == null ||
                !Enum.TryParse(TypeComboBox.SelectedItem.ToString(), out Weapon.WeaponType type))
            {
                ErrorTextBlock.Text = "Please select a valid Type.";
                return;
            }

            if (RarityComboBox.SelectedItem == null ||
                !int.TryParse(RarityComboBox.SelectedItem.ToString(), out int rarity) ||
                rarity < 1 || rarity > 5)
            {
                ErrorTextBlock.Text = "Please select a valid Rarity (1-5).";
                return;
            }

            if (!int.TryParse(BaseAttackTextBox.Text?.Trim(), out int baseAttack) || baseAttack < 0)
            {
                ErrorTextBlock.Text = "Base Attack must be a non-negative integer.";
                return;
            }

            TempWeapon = new Weapon
            {
                Name = name,
                Type = type,
                Image = image,
                Rarity = rarity,
                BaseAttack = baseAttack,
                SecondaryStat = secondary,
                Passive = passive
            };

            DialogResult = true;
            Close();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // BONUS: Generate random values 
        private void GenerateClicked(object sender, RoutedEventArgs e)
        {
            var rng = new Random();

            BaseAttackTextBox.Text = rng.Next(20, 51).ToString(); // 20..50
            RarityComboBox.SelectedItem = rng.Next(1, 6).ToString(); // 1..5

            // Random type excluding None
            var types = (Weapon.WeaponType[])Enum.GetValues(typeof(Weapon.WeaponType));
            Weapon.WeaponType picked;
            do
            {
                picked = types[rng.Next(types.Length)];
            } while (picked == Weapon.WeaponType.None);

            TypeComboBox.SelectedItem = picked.ToString();
        }
    }
}
