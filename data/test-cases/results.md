# Resultate

- **Positiver Testfall**:
	- Der Output ist "MyWallet 0.25", was dem erwarteten Ergebnis entspricht.
		- Der andere Account wurde gelöscht, es gibt nur noch MyWallet.
		- Es verbleibt noch 0.25 auf diesem Account.
- **Negativer Testfall**:
	- Da aufgrund der Ungenauigkeit von Double "1.5 × 10³⁰⁰ + 1.5" zu "1.5 × 10³⁰⁰" gerundet wurde, entsteht die folgende Fehlermeldung:

```
Unhandled exception. System.InvalidOperationException: balance 0 < 1.5
   at BadBank.WithdrawTransaction.Apply(IDictionary`2 accounts) in C:\Users\jzelAdmin\source\repos\BadBank\BadBank\WithdrawTransaction.cs:line 44
   at BadBank.Processing.Process(IDictionary`2 accounts, String bbtlFilePath, String outputFolderPath) in C:\Users\jzelAdmin\source\repos\BadBank\BadBank\Processing.cs:line 13
   at BadBank.Program.Main(String[] args) in C:\Users\jzelAdmin\source\repos\BadBank\BadBank.Demo\Program.cs:line 36
```
