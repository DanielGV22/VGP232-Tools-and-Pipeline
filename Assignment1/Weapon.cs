using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment1
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            // The path to the input file to load.
            string inputFile = string.Empty;

            // The path of the output file to save.
            string outputFile = string.Empty;

            // The flag to determine if we overwrite the output file or append to it.
            bool appendToFile = false;

            // The flag to determine if we need to display the number of entries
            bool displayCount = false;

            // The flag to determine if we need to sort the results via a column name.
            bool sortEnabled = false;

            // The column name to be used to determine which sort comparison function to use.
            string sortColumnName = string.Empty;

            // The results to be output to a file or to the console
            List<Weapon> results = new List<Weapon>();

            if (args == null || args.Length == 0)
            {
                PrintHelp();
                return 1;
            }

            // Parse command line args
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                // -h / --help
                if (arg == "-h" || arg == "--help")
                {
                    PrintHelp();
                    return 0;
                }
                // -i / --input <path>  (required)
                else if (arg == "-i" || arg == "--input")
                {
                    if (!TryGetNextArg(args, ref i, out inputFile))
                    {
                        Console.WriteLine("Error: No input file specified after {0}.", arg);
                        return 1;
                    }

                    if (string.IsNullOrWhiteSpace(inputFile))
                    {
                        Console.WriteLine("Error: Input file path is empty.");
                        return 1;
                    }

                    if (!File.Exists(inputFile))
                    {
                        Console.WriteLine("Error: Invalid path. Input file does not exist: {0}", inputFile);
                        return 1;
                    }

                    // Parse immediately once we have a valid input
                    results = Parse(inputFile);
                }
                // -o / --output <path> (optional)
                else if (arg == "-o" || arg == "--output")
                {
                    if (!TryGetNextArg(args, ref i, out string filePath))
                    {
                        Console.WriteLine("Error: No output file specified after {0}.", arg);
                        return 1;
                    }

                    if (string.IsNullOrWhiteSpace(filePath))
                    {
                        Console.WriteLine("Error: Output file path is empty.");
                        return 1;
                    }

                    outputFile = filePath;
                }
                // -a / --append (optional)
                else if (arg == "-a" || arg == "--append")
                {
                    appendToFile = true;
                }
                // -c / --count (optional)
                else if (arg == "-c" || arg == "--count")
                {
                    displayCount = true;
                }
                // -s / --sort <column name> (optional)
                else if (arg == "-s" || arg == "--sort")
                {
                    sortEnabled = true;

                    if (!TryGetNextArg(args, ref i, out sortColumnName))
                    {
                        Console.WriteLine("Error: No column name specified after {0}.", arg);
                        return 1;
                    }

                    if (string.IsNullOrWhiteSpace(sortColumnName))
                    {
                        Console.WriteLine("Error: Sort column name is empty.");
                        return 1;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Invalid argument Arg[{0}] = [{1}]", i, arg);
                    return 1;
                }
            }

            // Validate required input
            if (string.IsNullOrWhiteSpace(inputFile))
            {
                Console.WriteLine("Error: Missing required argument -i/--input <path>.");
                PrintHelp();
                return 1;
            }

            // Sorting
            if (sortEnabled)
            {
                ApplySort(results, sortColumnName);
            }

            // Count
            if (displayCount)
            {
                Console.WriteLine("There are {0} entries.", results.Count);
            }

            // Output
            if (results.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(outputFile))
                {
                    try
                    {
                        WriteResults(outputFile, results, appendToFile);
                        Console.WriteLine("Output:");
                        Console.WriteLine(outputFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: Failed to write output file: {0}", ex.Message);
                        return 1;
                    }
                }
                else
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        Console.WriteLine(results[i]);
                    }
                }
            }
            else
            {
                Console.WriteLine("Warning: No results to output.");
            }

            Console.WriteLine("Done!");
            return 0;
        }

        private static bool TryGetNextArg(string[] args, ref int i, out string value)
        {
            value = string.Empty;
            if (args.Length <= i + 1)
                return false;

            i++;
            value = args[i];
            return true;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Assignment1 - CSV Parser");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  Assignment1.exe -i <path> [-o <path>] [-a] [-c] [-s <column>]");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  -h, --help                 Output instructions on how to use it");
            Console.WriteLine("  -i, --input <path>         Loads the input file path specified (required)");
            Console.WriteLine("  -o, --output <path>        Saves result in the output file path specified (optional)");
            Console.WriteLine("  -c, --count                Displays the number of entries (optional)");
            Console.WriteLine("  -a, --append               Appends to an existing output file (optional)");
            Console.WriteLine("  -s, --sort <column name>   Sorts by: Name, Type, Rarity, BaseAttack (optional)");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  Assignment1.exe -i data.csv -o output.csv -c -s BaseAttack");
        }

        private static void ApplySort(List<Weapon> results, string sortColumnName)
        {
            if (results == null) return;

            string col = sortColumnName.Trim();

            // Match Weapon properties (case-insensitive)
            if (col.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Sorting by Name.");
                results.Sort(Weapon.CompareByName);
            }
            else if (col.Equals("Type", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Sorting by Type.");
                results.Sort(Weapon.CompareByType);
            }
            else if (col.Equals("Rarity", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Sorting by Rarity.");
                results.Sort(Weapon.CompareByRarity);
            }
            else if (col.Equals("BaseAttack", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Sorting by BaseAttack.");
                results.Sort(Weapon.CompareByBaseAttack);
            }
            else
            {
                Console.WriteLine("Warning: Unknown sort column '{0}'. Defaulting to Name.", sortColumnName);
                Console.WriteLine("Sorting by Name.");
                results.Sort(Weapon.CompareByName);
            }
        }

        private static void WriteResults(string outputFile, List<Weapon> results, bool appendToFile)
        {
            // Decide file mode
            FileMode mode = (appendToFile && File.Exists(outputFile)) ? FileMode.Append : FileMode.Create;

            using (FileStream fs = File.Open(outputFile, mode))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                bool shouldWriteHeader = true;

                // If appending and file already has content, skip header
                if (mode == FileMode.Append)
                {
                    FileInfo fi = new FileInfo(outputFile);
                    if (fi.Length > 0)
                        shouldWriteHeader = false;
                }

                if (shouldWriteHeader)
                {
                    writer.WriteLine("Name,Type,Rarity,BaseAttack");
                }

                for (int i = 0; i < results.Count; i++)
                {
                    writer.WriteLine(results[i].ToCsv());
                }
            }
        }

        /// <summary>
        /// Reads the file and line by line parses the data into a List of Weapons
        /// </summary>
        public static List<Weapon> Parse(string fileName)
        {
            List<Weapon> output = new List<Weapon>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                // Skip header line
                string header = reader.ReadLine();

                int lineNumber = 1;

                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    lineNumber++;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] values = line.Split(',');

                    // Expected: Name,Type,Rarity,BaseAttack
                    if (values.Length != 4)
                    {
                        Console.WriteLine("Warning: Skipping line {0} (expected 4 columns, got {1}).", lineNumber, values.Length);
                        continue;
                    }

                    string name = values[0].Trim();
                    string type = values[1].Trim();
                    string rarityStr = values[2].Trim();
                    string baseAtkStr = values[3].Trim();

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
                    {
                        Console.WriteLine("Warning: Skipping line {0} (empty Name or Type).", lineNumber);
                        continue;
                    }

                    if (!int.TryParse(rarityStr, out int rarity))
                    {
                        Console.WriteLine("Warning: Skipping line {0} (invalid Rarity: '{1}').", lineNumber, rarityStr);
                        continue;
                    }

                    if (!int.TryParse(baseAtkStr, out int baseAttack))
                    {
                        Console.WriteLine("Warning: Skipping line {0} (invalid BaseAttack: '{1}').", lineNumber, baseAtkStr);
                        continue;
                    }

                    Weapon weapon = new Weapon
                    {
                        Name = name,
                        Type = type,
                        Rarity = rarity,
                        BaseAttack = baseAttack
                    };

                    output.Add(weapon);
                }
            }

            return output;
        }
    }
}
