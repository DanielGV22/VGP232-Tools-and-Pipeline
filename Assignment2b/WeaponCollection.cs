using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace Assignment2b
{
    // WeaponCollection  CSV + JSON + XML serialization interfaces 
    public class WeaponCollection : List<Weapon>, ICsvSerializable, IJsonSerializable, IXmlSerializable
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

        // ============================
        // 2B: ROUTING (Load / Save)
        // ============================

        public bool Load(string path)
        {
            Clear();

            if (string.IsNullOrWhiteSpace(path)) return false;
            if (!File.Exists(path)) return false;

            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".csv" => LoadCSV(path),
                ".json" => LoadJSON(path),
                ".xml" => LoadXML(path),
                _ => false
            };
        }

        public bool Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".csv" => SaveAsCSV(path),
                ".json" => SaveAsJSON(path),
                ".xml" => SaveAsXML(path),
                _ => false
            };
        }

        // ============================
        // 2B: CSV
        // ============================

        public bool LoadCSV(string path)
        {
            Clear();

            if (string.IsNullOrWhiteSpace(path)) return false;
            if (!File.Exists(path)) return false;

            try
            {
                // If file is empty => valid load, just 0 entries
                if (IsEmptyFile(path))
                    return true;

                using var reader = new StreamReader(path);

                // Header (optional but expected)
                string header = reader.ReadLine();
                if (header == null)
                    return true;

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (Weapon.TryParse(line, out Weapon weapon) && weapon != null)
                        Add(weapon);
                }

                return true;
            }
            catch
            {
                Clear();
                return false;
            }
        }

        public bool SaveAsCSV(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                using var writer = new StreamWriter(path, append: false);
                writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                foreach (var w in this)
                {
                    if (w == null) continue;
                    writer.WriteLine(w.ToCsv());
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // ============================
        // 2B: JSON
        // ============================

        public bool LoadJSON(string path)
        {
            Clear();

            if (string.IsNullOrWhiteSpace(path)) return false;
            if (!File.Exists(path)) return false;

            try
            {
                if (IsEmptyFile(path))
                    return true;

                string json = File.ReadAllText(path);

                // Deserialize a List<Weapon> (Weapon must have public settable properties)
                var weapons = JsonSerializer.Deserialize<List<Weapon>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (weapons == null)
                    return false;

                foreach (var w in weapons)
                {
                    if (w != null) Add(w);
                }

                return true;
            }
            catch
            {
                Clear();
                return false;
            }
        }

        public bool SaveAsJSON(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                // Serialize collection as a JSON array
                string json = JsonSerializer.Serialize(this.ToList(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(path, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ============================
        // 2B: XML
        // ============================

        public bool LoadXML(string path)
        {
            Clear();

            if (string.IsNullOrWhiteSpace(path)) return false;
            if (!File.Exists(path)) return false;

            try
            {
                if (IsEmptyFile(path))
                    return true;

                var serializer = new XmlSerializer(typeof(List<Weapon>));

                using var fs = File.OpenRead(path);
                var weapons = serializer.Deserialize(fs) as List<Weapon>;

                if (weapons == null)
                    return false;

                foreach (var w in weapons)
                {
                    if (w != null) Add(w);
                }

                return true;
            }
            catch
            {
                Clear();
                return false;
            }
        }

        public bool SaveAsXML(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                var serializer = new XmlSerializer(typeof(List<Weapon>));

                using var fs = File.Create(path);
                serializer.Serialize(fs, this.ToList());

                return true;
            }
            catch
            {
                return false;
            }
        }

        // ============================
        // Helpers
        // ============================

        private static bool IsEmptyFile(string path)
        {
            try
            {
                var info = new FileInfo(path);
                return info.Length == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
