using System;

namespace Assignment1
{
    public class Weapon
    {
        // Name,Type,Rarity,BaseAttack
        public string Name { get; set; }
        public string Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }

        public static int CompareByName(Weapon left, Weapon right)
        {
            if (left == null && right == null) return 0;
            if (left == null) return -1;
            if (right == null) return 1;

            return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase);
        }

        public static int CompareByType(Weapon left, Weapon right)
        {
            if (left == null && right == null) return 0;
            if (left == null) return -1;
            if (right == null) return 1;

            return string.Compare(left.Type, right.Type, StringComparison.OrdinalIgnoreCase);
        }

        public static int CompareByRarity(Weapon left, Weapon right)
        {
            if (left == null && right == null) return 0;
            if (left == null) return -1;
            if (right == null) return 1;

            return left.Rarity.CompareTo(right.Rarity);
        }

        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            if (left == null && right == null) return 0;
            if (left == null) return -1;
            if (right == null) return 1;

            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        public string ToCsv()
        {
            return $"{Name},{Type},{Rarity},{BaseAttack}";
        }

        public override string ToString()
        {
            // Comma-separated value string
            return ToCsv();
        }
    }
}
