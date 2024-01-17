using System.Data;

namespace BadBank;

public interface Transaction
{
    public void Apply(IDictionary<string, double> accounts);
}