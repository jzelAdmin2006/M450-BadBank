# BadBank

BadBank simulates the internal settlement system of a (bad) bank. The system periodically processes input files and generates reports.

## Usage

Syntax:

    $ dotnet run --project BadBank.Demo INPUT OUTPUT FREQUENCY

Example (Bash):

    $ mkdir -p BadBank/{input,output}
    $ dotnet run --project BadBank.Demo BadBank/input BadBank/output 30

Arguments:

- `INPUT` is a folder path that will be watched for new files
- `OUTPUT` is a folder path to which reports will be written
- `FREQUENCY` is the processing frequency in seconds (integer)

The input files are written in the _BadBank Transaction Language_ (see below).

Example file (`input.bbtl`):

    open Alice
    deposit Alice 100
    open Bob
    send 25 from Alice to Bob
    withdraw 15 from Bob
    withdraw 20 from Alice

If the file `alice-bob.bbtl` is put into the `INPUT` folder, its processing will start after `FREQUENCY` seconds the latest. If multiple files are put into the `INPUT` folder, they will be processed in alphabetical order (case-sensitive).

The result will be published as `alice-bob.bbrf` (_BadBank Reporting Format_) in the `OUTPUT` folder:

    Alice 55.00
    Bob 10.00

## BadBank Transaction Language

- `open ACCOUNT`
    - **semantics**: open the account with name `ACCOUNT`
    - **pre-condition**: no account of name `ACCOUNT` exists yet
    - **post-condition**: an account of name `ACCOUNT` with balance 0.0 exists
- `close ACCOUNT`
    - **semantics**: close the account with the name `ACCOUNT`
    - **pre-condition**: an account of name `ACCOUNT` exists
    - **post-condition**: the account of name `ACCOUNT` no longer exists
- `send AMOUNT from ACCOUNT_A to ACCOUNT_B`
    - **semantics**: send the amount `AMOUNT` from `ACCOUNT_A` to `ACCOUNT_B`
    - **pre-condition**: both `ACCOUNT_A` and `ACCOUNT_B` exist; `AMOUNT` must be positive (`> 0.0`)
    - **post-condition**: balance of `ACCOUNT_A` decreased, balance of `ACCOUNT_B` increased by `AMOUNT`
    - **Note**: _no_ balance check is performed (balance can become negative)
- `deposit AMOUNT to ACCOUNT`
    - **semantics**: deposit the amount `AMOUNT` for `ACCOUNT`
    - **pre-condition**: an account of name `ACCOUNT` exists
    - **post-condition**: balance of `ACCOUNT` increased by `AMOUNT`
- `withdraw AMOUNT from ACCOUNT`
    - **semantics**: withdraw the amount `AMOUNT` from `ACCOUNT`
    - **pre-condition**: an account of name `ACCOUNT` exists
    - **post-condition**: balance of `ACCOUNT` decreased by `AMOUNT`; balance of `ACCOUNT` must be `>= AMOUNT`

## BadBank Reporting Format

- `ACCOUNT AMOUNT`
    - the account `ACCOUNT` has the given `AMOUNT` (formatted as a floating-point number with two fraction digits, e.g. `165.29`)
- `error "COMMAND": MESSAGE`
    - the given `COMMAND` could not be processed (invalid format; violation of a condition) for the reason denoted in `MESSAGE`