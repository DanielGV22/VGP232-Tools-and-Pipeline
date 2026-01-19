using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment2a
{
    public class Weapon
    {
        // CSV: Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive

        public string Name { get; set; } = string.Empty;
        public WeaponType Type { get; set; } = WeaponType.None;
        public string Image { get; set; } = string.Empty;
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string SecondaryStat { get; set; } = string.Empty;
        public string Passive { get; set; } = string.Empty;

        public enum WeaponType
        {
            Sword,
            Polearm,
            Claymore,
            Catalyst,
            Bow,
            None
        }

        public static bool TryParse(string rawData, out Weapon weapon)
        {
            weapon = null;

            if (string.IsNullOrWhiteSpace(rawData))
                return false;

            string[] values = rawData.Split(',');

            // Expected: Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive
            if (values.Length != 7)
            {
                Console.WriteLine($"Invalid row: expected 7 columns, got {values.Length}");
                return false;
            }

            string name = values[0].Trim();
            string typeStr = values[1].Trim();
            string image = values[2].Trim();
            string rarityStr = values[3].Trim();
            string baseAttackStr = values[4].Trim();
            string secondary = values[5].Trim();
            string passive = values[6].Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name is empty/invalid.");
                return false;
            }

            if (!int.TryParse(rarityStr, out int rarity))
            {
                Console.WriteLine($"Rarity '{rarityStr}' is invalid format");
                return false;
            }

            if (!int.TryParse(baseAttackStr, out int baseAttack))
            {
                Console.WriteLine($"Base Attack '{baseAttackStr}' is invalid format");
                return false;
            }

            Weapon.WeaponType type;
            if (!Enum.TryParse(typeStr, ignoreCase: true, out type))
                type = Weapon.WeaponType.None;

            weapon = new Weapon
            {
                Name = name,
                Type = type,
                Image = image,
                Rarity = rarity,
                BaseAttack = baseAttack,
                SecondaryStat = secondary,
                Passive = passive
            };

            return true;
        }


        public string ToCsv()
        {
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public override string ToString() => ToCsv();

        // Comparers for SortBy(...)
        public static int CompareByName(Weapon left, Weapon right)
            => string.Compare(left?.Name, right?.Name, StringComparison.OrdinalIgnoreCase);

        public static int CompareByType(Weapon left, Weapon right)
            => (left?.Type ?? WeaponType.None).CompareTo(right?.Type ?? WeaponType.None);

        public static int CompareByRarity(Weapon left, Weapon right)
            => (left?.Rarity ?? 0).CompareTo(right?.Rarity ?? 0);

        public static int CompareByBaseAttack(Weapon left, Weapon right)
            => (left?.BaseAttack ?? 0).CompareTo(right?.BaseAttack ?? 0);
    }
}