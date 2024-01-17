using System.Data;

namespace BadBank;

public abstract class Transaction
{
    private string line;

    public Transaction(string line)
    {
        this.line = line;
    }

    public abstract void Apply(IDictionary<string, double> accounts);

    public override string ToString()
    {
        return line;
    }
}