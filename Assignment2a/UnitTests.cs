using System;
using System.IO;
using NUnit.Framework;

namespace Assignment2a
{
    [TestFixture]
    public class UnitTests
    {
        private string inputPath;
        private string outputPath;

        private WeaponCollection weapons;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);

            weapons = new WeaponCollection();
            Assert.That(weapons.Load(inputPath), Is.True, "Expected valid input file to load.");
        }

        [TearDown]
        public void CleanUp()
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);
        }

        // WeaponCollection Unit Tests

        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            // Expected Value: 48
            int highest = weapons.GetHighestBaseAttack();
            Assert.That(highest, Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            // Expected Value: 23
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
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            var wc = new WeaponCollection();
            Assert.That(wc.Load(inputPath), Is.True);
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            var wc = new WeaponCollection();
            bool ok = wc.Load("this_file_should_not_exist_123.csv");
            Assert.That(ok, Is.False);
            Assert.That(wc.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            Assert.That(weapons.Save(outputPath), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(outputPath), Is.True);
            Assert.That(loaded.Count, Is.GreaterThan(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            var empty = new WeaponCollection();
            Assert.That(empty.Save(outputPath), Is.True);

            var loaded = new WeaponCollection();
            Assert.That(loaded.Load(outputPath), Is.True);
            Assert.That(loaded.Count, Is.EqualTo(0));
        }

        // Weapon Unit Tests

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
            // Invalid: wrong number of columns + wrong types
            string line = "1,Bulbasaur,A,B,C,65,65";
            bool ok = Weapon.TryParse(line, out Weapon actual);

            Assert.That(ok, Is.False);
            Assert.That(actual, Is.Null);
        }
    }
}
