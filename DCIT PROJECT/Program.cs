using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // =========================
    // a. Record for Transaction
    // =========================
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // =========================
    // b. Interface
    // =========================
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // =========================
    // c. Processors
    // =========================
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processed {transaction.Amount:C} for {transaction.Category}");
        }
    }

    // =========================
    // d. Base Account
    // =========================
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
        }
    }

    // =========================
    // e. Sealed SavingsAccount
    // =========================
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                Balance -= transaction.Amount;
                Console.WriteLine($"Transaction applied. New balance: {Balance:C}");
            }
        }
    }

    // =========================
    // f. FinanceApp
    // =========================
    public class FinanceApp
    {
        private List<Transaction> _transactions = new();

        public void Run()
        {
            // i. Create account
            var account = new SavingsAccount("ACC12345", 1000m);

            // ii. Create transactions
            var t1 = new Transaction(1, DateTime.Now, 200m, "Groceries");
            var t2 = new Transaction(2, DateTime.Now, 300m, "Utilities");
            var t3 = new Transaction(3, DateTime.Now, 150m, "Entertainment");

            // iii. Process transactions
            ITransactionProcessor mobileProcessor = new MobileMoneyProcessor();
            ITransactionProcessor bankProcessor = new BankTransferProcessor();
            ITransactionProcessor cryptoProcessor = new CryptoWalletProcessor();

            mobileProcessor.Process(t1);
            bankProcessor.Process(t2);
            cryptoProcessor.Process(t3);

            // iv. Apply transactions to account
            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
            account.ApplyTransaction(t3);

            // v. Add to list
            _transactions.Add(t1);
            _transactions.Add(t2);
            _transactions.Add(t3);

            Console.WriteLine("\n--- Transaction Log ---");
            foreach (var tx in _transactions)
            {
                Console.WriteLine($"{tx.Id}: {tx.Category} - {tx.Amount:C} on {tx.Date}");
            }
        }
    }

    // =========================
    // Main Entry Point
    // =========================
    class Program
    {
        static void Main(string[] args)
        {
            FinanceApp app = new FinanceApp();
            app.Run();
        }
    }
}
