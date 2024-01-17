using System.Text.RegularExpressions;

namespace BadBank;

public class OpenTransaction : Transaction
{
    private static string PATTERN = "^open ([A-Za-z]+)$";

    public static Transaction Parse(string line)
    {
        var regex = new Regex(PATTERN);
        var match = regex.Match(line);
        if (!match.Success)
        {
            throw new ArgumentException($"{line} does not match {PATTERN}");
        }
        return new OpenTransaction(line, match.Groups[1].Value);
    }

    private string accountName;

    public OpenTransaction(string line, string accountName) : base(line)
    {
        this.accountName = accountName;
    }

    public override void Apply(IDictionary<string, double> accounts)
    {
        if (accounts.ContainsKey(accountName))
        {
            throw new InvalidOperationException($"account {accountName} was already opened");
        }
        accounts.Add(accountName, 0.0);
    }
}