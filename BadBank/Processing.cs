using System.Text;
using System.Xml.XPath;

namespace BadBank;

public class Processing
{
    public static string Process(IDictionary<string, double> accounts, string bbtlFilePath, string outputFolderPath)
    {
        var result = new StringBuilder();
        foreach (var transaction in ParseFile(bbtlFilePath))
        {
            transaction.Apply(accounts);
        }
        foreach (var account in accounts.Keys)
        {
            var balance = accounts[account];
            result.AppendLine($"{account} {balance:0.00}");
        }
        return OutputResult(bbtlFilePath, result.ToString(), outputFolderPath);
    }

    private static string OutputResult(string inFile, string result, string outDir)
    {
        var bbtlFile = new FileInfo(inFile);
        var outputFilePath = Path.Combine(outDir, bbtlFile.Name.Replace(".bbtl", ".bbrf"));
        var outputFile = File.OpenWrite(outputFilePath);
        var writer = new StreamWriter(outputFile);
        writer.Write(result);
        bbtlFile.Delete();
        writer.Close();
        return outputFilePath;
    }

    private static List<Transaction> ParseFile(string bbtlFilePath)
    {
        var transactions = new List<Transaction>();
        var fileStream = File.OpenRead(bbtlFilePath);
        var reader = new StreamReader(fileStream, Encoding.UTF8);
        string? line;
        var lines = new List<string>();
        while ((line = reader.ReadLine()) != null)
        {
            var trimmed = line.Trim();
            lines.Add(trimmed);
        }
        fileStream.Close();
        foreach (var command in lines)
        {
            transactions.Add(ParseLine(command));
        }
        return transactions;
    }

    private static Transaction ParseLine(string line)
    {
        var command = line.Split(' ')[0];
        switch (command)
        {
            case "open":
                return OpenTransaction.Parse(line);
            case "close":
                return CloseTransaction.Parse(line);
            case "send":
                return SendTransaction.Parse(line);
            case "deposit":
                return DepositTransaction.Parse(line);
            case "withdraw":
                return WithdrawTransaction.Parse(line);
            default:
                throw new ArgumentException($"unknown command '{0}'", command);
        }
    }
}