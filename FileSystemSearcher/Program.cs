using System.Text.RegularExpressions;

namespace FileSystemSearcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("PATH: ");
                string path = Console.ReadLine();

                if (!Path.Exists(path))
                {
                    ShowError("Path doesn't exist.");
                    continue;
                }

                Console.Write("NAME: ");
                string name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    ShowError("Name can't be empty.");
                    continue;
                }

                if (!IsValidPattern(name))
                {
                    ShowError("Pattern is invalid.");
                    continue;
                }

                Console.WriteLine($"\nSearching in '{path}' any file or directory with name '{name}'...");

                List<string> files = SearchFilesByName(path, name);
                List<string> directories = SearchDirectoriesByName(path, name);

                // Files
                if (files.Count > 0)
                {
                    Console.WriteLine($"\nFiles found ({files.Count}):");
                    foreach (string file in files)
                    {
                        Console.WriteLine($" - {Path.GetFileName(file)} | {file}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo files found.");
                }

                // Directories
                if (directories.Count > 0)
                {
                    Console.WriteLine($"\nDirectories found ({directories.Count}):");
                    foreach (string directory in directories)
                    {
                        Console.WriteLine($" - {Path.GetFileName(directory)} | {directory}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo directories found.");
                }

                Console.Write("\nTo continue press any key... ");
                Console.ReadKey(true);
            }
        }

        static List<string> SearchFilesByName(string path, string name)
        {
            List<string> files = new List<string>();

            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            };

            foreach (var file in Directory.GetFiles(path, "*", options))
            {
                string fileName = Path.GetFileName(file);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                if (Regex.IsMatch(fileName, name, RegexOptions.IgnoreCase) || Regex.IsMatch(fileNameWithoutExtension, name, RegexOptions.IgnoreCase))
                    files.Add(file);
            }

            return files;
        }

        static List<string> SearchDirectoriesByName(string path, string name)
        {
            List<string> directories = new List<string>();

            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            };

            foreach (var directory in Directory.GetDirectories(path, "*", options))
            {
                string directoryName = Path.GetFileName(directory);

                if (Regex.IsMatch(directoryName, name, RegexOptions.IgnoreCase))
                    directories.Add(directory);
            }

            return directories;
        }

        // UTILS

        static bool IsValidPattern(string pattern)
        {
            try
            {
                new Regex(pattern);
                return true;
            }
            catch (RegexParseException)
            {
                return false;
            }
        }

        static void ShowError(string message)
        {
            Console.Clear();
            Console.WriteLine($"ERROR: {message}");
            Console.Write("To continue press any key... ");
            Console.ReadKey(true);
        }
    }
}