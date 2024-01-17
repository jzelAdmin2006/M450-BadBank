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

        while (true)
        {
            Thread.Sleep(1000 * frequency);
            var dir = new DirectoryInfo(inputFolder);
            var files = dir.GetFiles("*.bbtl");
            foreach (var file in files)
            {
                Console.WriteLine($"processed file {file}");
                var result = Processing.Process(file.ToString(), outputFolder);
                Console.WriteLine($"processed file {file}, reported to {result}");
            }
        }
    }
}