using System.Text.RegularExpressions;

namespace BadBank;

public class WithdrawTransaction : Transaction
{
    private static string PATTERN = @"^withdraw (?<amount>[0-9]+(\.[0-9]+)?) from (?<account>[A-Za-z]+)$";

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
            throw new ArgumentException($"cannot withdraw negative amount");
        }
        return new WithdrawTransaction(line, account, amount);
    }

    private string accountName;
    private double withdrawAmount;

    public WithdrawTransaction(string line, string accountName, double withdrawAmount) : base(line)
    {
        this.accountName = accountName;
        this.withdrawAmount = withdrawAmount;
    }

    public override void Apply(IDictionary<string, double> accounts)
    {
        if (!accounts.ContainsKey(accountName))
        {
            throw new InvalidOperationException($"account {accountName} does not exist");
        }
        var balance = accounts[accountName];
        if (balance < withdrawAmount)
        {
            throw new InvalidOperationException($"balance {balance} < {withdrawAmount}");
        }
        accounts[accountName] = balance - withdrawAmount;
    }
}