namespace BadBank;

public class Program
{
    public static void Main(string[] args)
    {
        var inputFolder = args[0];
        var outputFolder = args[1];
        var frequency = int.Parse(args[2]);

        if (!Directory.Exists(inputFolder))
        {
            Console.WriteLine($"input folder '{0}' does not exist", inputFolder);
            return;
        }
        if (!Directory.Exists(outputFolder))
        {
            Console.WriteLine($"output folder '{0}' does not exist", outputFolder);
            return;
        }
        if (frequency < 0)
        {
            Console.WriteLine($"frequency must be positive, was {0}", frequency);
            return;
        }

        var accounts = new Dictionary<string, double>();
        while (true)
        {
            Thread.Sleep(1000 * frequency);
            var dir = new DirectoryInfo(inputFolder);
            var files = dir.GetFiles("*.bbtl");
            Array.Sort(files, (FileInfo left, FileInfo right) => left.Name.CompareTo(right.Name));
            foreach (var file in files)
            {
                var result = Processing.Process(accounts, file.ToString(), outputFolder);
                Console.WriteLine($"processed file {file}, reported to {result}");
            }
        }
    }
}