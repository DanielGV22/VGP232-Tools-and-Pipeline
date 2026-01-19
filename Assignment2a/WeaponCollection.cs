using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment2a
{
    public class WeaponCollection : List<Weapon>, IPeristence
    {
        public int GetHighestBaseAttack()
        {
            if (Count == 0) return 0;
            return this.Max(w => w?.BaseAttack ?? 0);
        }

        public int GetLowestBaseAttack()
        {
            if (Count == 0) return 0;
            return this.Min(w => w?.BaseAttack ?? 0);
        }

        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType type)
        {
            return this.Where(w => w != null && w.Type == type).ToList();
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            return this.Where(w => w != null && w.Rarity == stars).ToList();
        }

        public void SortBy(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                Sort(Weapon.CompareByName);
                return;
            }

            string col = columnName.Trim();

            if (col.Equals("Name", StringComparison.OrdinalIgnoreCase))
                Sort(Weapon.CompareByName);
            else if (col.Equals("Type", StringComparison.OrdinalIgnoreCase))
                Sort(Weapon.CompareByType);
            else if (col.Equals("Rarity", StringComparison.OrdinalIgnoreCase))
                Sort(Weapon.CompareByRarity);
            else if (col.Equals("BaseAttack", StringComparison.OrdinalIgnoreCase))
                Sort(Weapon.CompareByBaseAttack);
            else
                Sort(Weapon.CompareByName);
        }

        public bool Load(string filename)
        {
            Clear();

            if (string.IsNullOrWhiteSpace(filename))
                return false;

            if (!File.Exists(filename))
                return false;

            try
            {
                using (var reader = new StreamReader(filename))
                {
                    // Header
                    string header = reader.ReadLine();
                    if (header == null)
                        return true; // empty file => valid load

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        if (Weapon.TryParse(line, out Weapon weapon) && weapon != null)
                        {
                            Add(weapon);
                        }
                    }
                }

                return true;
            }
            catch
            {
                // invalid path, permission, etc.
                Clear();
                return false;
            }
        }

        public bool Save(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return false;

            try
            {
                using (var writer = new StreamWriter(filename, append: false))
                {
                    writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");
                    foreach (var w in this)
                    {
                        if (w == null) continue;
                        writer.WriteLine(w.ToCsv());
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
