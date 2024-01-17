using System.Text.RegularExpressions;

namespace BadBank;

public class DepositTransaction : Transaction
{
    private static string PATTERN = @"^deposit (?<amount>[0-9]+(\.[0-9]+)?) to (?<account>[A-Za-z]+)$";

    public static Transaction Parse(string line)
    {
        var regex = new Regex(PATTERN);
        var matches = regex.Matches(line);
        if (matches.Count == 0 || matches[0].Groups.Count == 0)
        {
            throw new ArgumentException($"{line} does not match {PATTERN}");
        }
        var account = matches[0].Groups["account"].Value;
        var amount = double.Parse(matches[0].Groups["amount"].Value);
        if (amount < 0.0)
        {
            throw new ArgumentException($"cannot deposit negative amount");
        }
        return new DepositTransaction(line, account, amount);
    }

    private string accountName;
    private double depositAmount;

    public DepositTransaction(string line, string accountName, double depositAmount) : base(line)
    {
        this.accountName = accountName;
        this.depositAmount = depositAmount;
    }

    public override void Apply(IDictionary<string, double> accounts)
    {
        if (!accounts.ContainsKey(accountName))
        {
            throw new InvalidOperationException($"account {accountName} does not exist");
        }
        accounts[accountName] = accounts[accountName] + depositAmount;
    }
}