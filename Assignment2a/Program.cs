using System;
using System.IO;

namespace Assignment2a
{
    internal class MainClass
    {
        public static int Main(string[] args)
        {
            string inputFile = string.Empty;
            string outputFile = string.Empty;

            bool appendToFile = false;      
            bool displayCount = false;
            bool sortEnabled = false;
            string sortColumnName = string.Empty;

            WeaponCollection results = new WeaponCollection();

            if (args == null || args.Length == 0)
            {
                PrintHelp();
                return 1;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg == "-h" || arg == "--help")
                {
                    PrintHelp();
                    return 0;
                }
                else if (arg == "-i" || arg == "--input")
                {
                    if (!TryGetNextArg(args, ref i, out inputFile) || string.IsNullOrWhiteSpace(inputFile))
                    {
                        Console.WriteLine("Error: Missing input path after {0}.", arg);
                        return 1;
                    }

                    if (!File.Exists(inputFile))
                    {
                        Console.WriteLine("Error: Invalid path. Input file does not exist: {0}", inputFile);
                        return 1;
                    }

                    if (!results.Load(inputFile))
                    {
                        Console.WriteLine("Error: Failed to load input file: {0}", inputFile);
                        return 1;
                    }
                }
                else if (arg == "-o" || arg == "--output")
                {
                    if (!TryGetNextArg(args, ref i, out outputFile) || string.IsNullOrWhiteSpace(outputFile))
                    {
                        Console.WriteLine("Error: Missing output path after {0}.", arg);
                        return 1;
                    }
                }
                else if (arg == "-a" || arg == "--append")
                {
                    appendToFile = true;
                }
                else if (arg == "-c" || arg == "--count")
                {
                    displayCount = true;
                }
                else if (arg == "-s" || arg == "--sort")
                {
                    sortEnabled = true;
                    if (!TryGetNextArg(args, ref i, out sortColumnName) || string.IsNullOrWhiteSpace(sortColumnName))
                    {
                        Console.WriteLine("Error: Missing sort column after {0}.", arg);
                        return 1;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Invalid argument Arg[{0}] = [{1}]", i, arg);
                    return 1;
                }
            }

            if (string.IsNullOrWhiteSpace(inputFile))
            {
                Console.WriteLine("Error: Missing required argument -i/--input <path>.");
                PrintHelp();
                return 1;
            }

            if (sortEnabled)
                results.SortBy(sortColumnName);

            if (displayCount)
                Console.WriteLine("There are {0} entries.", results.Count);

            if (results.Count == 0)
            {
                Console.WriteLine("Warning: No results to output.");
                Console.WriteLine("Done!");
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(outputFile))
            {
                if (!results.Save(outputFile))
                {
                    Console.WriteLine("Error: Failed to save output file: {0}", outputFile);
                    return 1;
                }
                Console.WriteLine("Output:");
                Console.WriteLine(outputFile);
            }
            else
            {
                foreach (var w in results)
                    Console.WriteLine(w);
            }

            Console.WriteLine("Done!");
            return 0;
        }

        private static bool TryGetNextArg(string[] args, ref int i, out string value)
        {
            value = string.Empty;
            if (args.Length <= i + 1) return false;
            i++;
            value = args[i];
            return true;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Assignment2a - CSV Parser");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  Assignment2a.exe -i <path> [-o <path>] [-a] [-c] [-s <column>]");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  -h, --help                 Output instructions on how to use it");
            Console.WriteLine("  -i, --input <path>         Loads the input file path specified (required)");
            Console.WriteLine("  -o, --output <path>        Saves result in the output file path specified (optional)");
            Console.WriteLine("  -c, --count                Displays the number of entries (optional)");
            Console.WriteLine("  -a, --append               (Legacy) flag retained; Save() overwrites");
            Console.WriteLine("  -s, --sort <column name>   Sorts by: Name, Type, Rarity, BaseAttack (optional)");
        }
    }
}
