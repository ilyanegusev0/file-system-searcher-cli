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
                if (Path.GetFileName(file).Equals(name, StringComparison.OrdinalIgnoreCase) || Path.GetFileNameWithoutExtension(file).Equals(name, StringComparison.OrdinalIgnoreCase))
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
                if (Path.GetFileName(directory).Equals(name, StringComparison.OrdinalIgnoreCase))
                    directories.Add(directory);
            }

            return directories;
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