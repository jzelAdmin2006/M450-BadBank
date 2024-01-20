# Aufgabe 3

- **Positiver Testfall**:
	- Accounts "MyWallet" und "TheirWallet" erstellen
	- 30.50 in "TheirWallet" einzahlen
	- 30.00 von "TheirWallet" nach "MyWallet" senden
	- 29.75 von "MyWallet" abheben
	- Account "TheirWallet" schliessen
		- Nun sollte es nur noch "MyWallet" mit Stand 0.25 geben.
- **Negativer Testfall**:
	- Accounts "MyWallet" und "TheirWallet" erstellen
	- 1.5 × 10³⁰⁰ in "MyWallet" einzahlen
	- 1.50 in "MyWallet" einzahlen
	- 1.5 × 10³⁰⁰ von "MyWallet" nach "TheirWallet" senden
	- 1.50 von "MyWallet" abheben
		- Aufgrund der Ungenauigkeit von Double kommt die Fehlermeldung, dass man nicht unter 0 sein darf.
