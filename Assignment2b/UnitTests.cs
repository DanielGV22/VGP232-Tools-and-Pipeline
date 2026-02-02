using System;
using System.IO;
using NUnit.Framework;

namespace Assignment2b
{
    [TestFixture]
    public class UnitTests
    {
        private string inputPath;

        // Output files 
        private string outCsv;
        private string outJson;
        private string outXml;

        private string emptyCsv;
        private string emptyJson;
        private string emptyXml;

        private WeaponCollection weapons;

        private const string INPUT_FILE = "data2.csv";

        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);

            outCsv = CombineToAppPath("weapons.csv");
            outJson = CombineToAppPath("weapons.json");
            outXml = CombineToAppPath("weapons.xml");

            emptyCsv = CombineToAppPath("empty.csv");
            emptyJson = CombineToAppPath("empty.json");
            emptyXml = CombineToAppPath("empty.xml");

            weapons = new WeaponCollection();
            Assert.That(weapons.Load(inputPath), Is.True, "Expected valid input file to load.");
            Assert.That(weapons.Count, Is.EqualTo(95), "Expected data2.csv to contain 95 entries.");
        }

        [TearDown]
        public void TearDown()
        {
            TryDelete(outCsv);
            TryDelete(outJson);
            TryDelete(outXml);

            TryDelete(emptyCsv);
            TryDelete(emptyJson);
            TryDelete(emptyXml);
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                
            }
        }

        // =========================
        // Existing 2A tests
        // =========================

        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            int highest = weapons.GetHighestBaseAttack();
            Assert.That(highest, Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            int lowest = weapons.GetLowestBaseAttack();
            Assert.That(lowest, Is.EqualTo(23));
        }

        [Test]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons()
        {
            var swords = weapons.GetAllWeaponsOfType(Weapon.WeaponType.Sword);
            Assert.That(swords, Is.Not.Null);
            Assert.That(swords.Count, Is.GreaterThan(0));
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            var list = weapons.GetAllWeaponsOfRarity(stars);
            Assert.That(list.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            var wc = new WeaponCollection();
            bool ok = wc.Load("this_file_should_not_exist_123.csv");
            Assert.That(ok, Is.False);
            Assert.That(wc.Count, Is.EqualTo(0));
        }

        // =========================
        // 2B Required Tests
        // =========================

        // ---- Test LoadJson Valid (4 tests) ----

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            Assert.That(weapons.Save(outJson), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(outJson), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            Assert.That(weapons.SaveAsJSON(outJson), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(outJson), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            Assert.That(weapons.SaveAsJSON(outJson), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.LoadJSON(outJson), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            Assert.That(weapons.Save(outJson), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.LoadJSON(outJson), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        // ---- Test LoadCsv Valid (2 tests) ----

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            Assert.That(weapons.Save(outCsv), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(outCsv), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            Assert.That(weapons.SaveAsCSV(outCsv), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.LoadCSV(outCsv), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        // ---- Test LoadXml Valid (2 tests ) ----

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            Assert.That(weapons.Save(outXml), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(outXml), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            Assert.That(weapons.SaveAsXML(outXml), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.LoadXML(outXml), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(95));
        }

        // ---- Test SaveAs<format> Empty (3 tests) ----

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
            var empty = new WeaponCollection();
            Assert.That(empty.SaveAsJSON(emptyJson), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(emptyJson), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            var empty = new WeaponCollection();
            Assert.That(empty.SaveAsCSV(emptyCsv), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(emptyCsv), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            var empty = new WeaponCollection();
            Assert.That(empty.SaveAsXML(emptyXml), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(emptyXml), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        // ---- Test Load InvalidFormat (4 tests) ----

        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            Assert.That(weapons.SaveAsJSON(outJson), Is.True);

            var loaded = new WeaponCollection();
            bool ok = loaded.LoadXML(outJson);

            Assert.That(ok, Is.False);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            Assert.That(weapons.SaveAsXML(outXml), Is.True);

            var loaded = new WeaponCollection();
            bool ok = loaded.LoadJSON(outXml);

            Assert.That(ok, Is.False);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            var loaded = new WeaponCollection();
            bool ok = loaded.LoadXML(inputPath);

            Assert.That(ok, Is.False);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            var loaded = new WeaponCollection();
            bool ok = loaded.LoadJSON(inputPath);

            Assert.That(ok, Is.False);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        // =========================
        // Weapon parsing tests 
        // =========================

        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            string line =
                "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";

            bool ok = Weapon.TryParse(line, out Weapon actual);
            Assert.That(ok, Is.True);
            Assert.That(actual, Is.Not.Null);

            Assert.That(actual.Name, Is.EqualTo("Skyward Blade"));
            Assert.That(actual.Type, Is.EqualTo(Weapon.WeaponType.Sword));
            Assert.That(actual.Image, Is.EqualTo("https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png"));
            Assert.That(actual.Rarity, Is.EqualTo(5));
            Assert.That(actual.BaseAttack, Is.EqualTo(46));
            Assert.That(actual.SecondaryStat, Is.EqualTo("Energy Recharge"));
            Assert.That(actual.Passive, Is.EqualTo("Sky-Piercing Fang"));
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            string line = "1,Bulbasaur,A,B,C,65,65";
            bool ok = Weapon.TryParse(line, out Weapon actual);

            Assert.That(ok, Is.False);
            Assert.That(actual, Is.Null);
        }
    }
}
