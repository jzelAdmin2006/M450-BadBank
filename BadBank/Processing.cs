using System.Text;

namespace BadBank;

public class Processing
{
    public static string Process(string bbtlFilePath, string outputFolderPath)
    {
        var accounts = new Dictionary<string, double>();
        var transactions = ParseFile(bbtlFilePath);
        foreach (var transaction in transactions)
        {
            // FIXME: surround with try/catch; report error to .bbrf file!
            transaction.Apply(accounts);
        }
        var result = new StringBuilder();
        foreach (var account in accounts.Keys)
        {
            var balance = accounts[account];
            result.AppendLine($"{account} {balance:0.00}");
        }
        var bbtlFile = new FileInfo(bbtlFilePath);
        var outputFilePath = Path.Combine(outputFolderPath, bbtlFile.Name.Replace(".bbtl", ".bbrf"));
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
        while ((line = reader.ReadLine()) != null)
        {
            var trimmed = line.Trim();
            transactions.Add(ParseLine(trimmed));
        }
        fileStream.Close();
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