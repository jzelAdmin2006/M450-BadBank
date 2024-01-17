using System.Text.RegularExpressions;

namespace BadBank;

public class SendTransaction : Transaction
{
    private static string PATTERN = @"^send (?<amount>[0-9]+(\.[0-9]+)?) from (?<source>[a-zA-Z]+) to (?<target>[a-zA-Z]+)$";

    public static Transaction Parse(string line)
    {
        var regex = new Regex(PATTERN);
        var matches = regex.Matches(line);
        if (matches.Count == 0 || matches[0].Groups.Count == 0)
        {
            throw new ArgumentException($"{line} does not match {PATTERN}");
        }
        var amount = double.Parse(matches[0].Groups["amount"].Value);
        if (amount < 0.0)
        {
            throw new ArgumentException($"cannot send negative amount");
        }
        var fromAccount = matches[0].Groups["source"].Value;
        var toAccount = matches[0].Groups["target"].Value;
        return new SendTransaction(line, fromAccount, toAccount, amount);
    }

    private string fromAccount;
    private string toAccount;
    private double sendAmount;

    public SendTransaction(string line, string fromAccount, string toAccount, double sendAmount) : base(line)
    {
        this.fromAccount = fromAccount;
        this.toAccount = toAccount;
        this.sendAmount = sendAmount;
    }

    public override void Apply(IDictionary<string, double> accounts)
    {
        if (!accounts.ContainsKey(fromAccount))
        {
            throw new InvalidOperationException($"source account {toAccount} does not exist");
        }
        if (!accounts.ContainsKey(toAccount))
        {
            throw new InvalidOperationException($"destination account {toAccount} does not exist");
        }
        var sourceBalance = accounts[fromAccount];
        var destinationBalance = accounts[toAccount];
        if (sourceBalance < sendAmount)
        {
            throw new InvalidOperationException($"balance {sourceBalance} < {sendAmount}");
        }
        accounts[fromAccount] = sourceBalance - sendAmount;
        accounts[toAccount] = destinationBalance + sendAmount;
    }
}