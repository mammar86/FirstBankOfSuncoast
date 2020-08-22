using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;


namespace FirstBankOfSuncoast
{

    class Transaction

    {
        public string TransactionType { get; set; }
        public string AccountType { get; set; }
        public int Amount { get; set; }

        public DateTime Date { get; set; }


        class Program
        {
            static void Greeting()
            {

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("=============== Welcome to First Bank of Suncoast App ===============");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            static void Main(string[] args)
            {
                Greeting();
                TextReader reader;

                if (File.Exists("Bank-info.csv"))
                {
                    reader = new StreamReader("Bank-Info.csv");
                }
                else
                {
                    reader = new StringReader("");
                }

                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

                var transactions = csvReader.GetRecords<Transaction>().ToList();

                reader.Close();

                var checkingBalance = 0;
                var savingBalance = 0;

                var checkTrans = transactions.Where(transaction => transaction.AccountType == "checking").OrderBy(transaction => transaction.Date);
                Console.WriteLine("Here are your past checking transactions");
                Console.WriteLine();
                foreach (var tran in checkTrans)
                {
                    Console.WriteLine($"{tran.TransactionType} of {tran.Amount} on {tran.Date}");
                }
                Console.WriteLine("---------------------------------------");
                var savTrans = transactions.Where(transaction => transaction.AccountType == "saving");
                Console.WriteLine("Here are your past saving transactions");
                Console.WriteLine();
                foreach (var tran in savTrans)
                {
                    Console.WriteLine($"{tran.TransactionType} of {tran.Amount} on {tran.Date}");
                }
                Console.WriteLine("---------------------------------------");

                foreach (var transaction in transactions)
                {

                    if (transaction.AccountType == "checking")
                    {

                        checkingBalance += transaction.Amount;
                    }
                    else
                    {
                        savingBalance += transaction.Amount;
                    }
                }


                var quit = false;

                while (quit != true)
                {
                    Console.WriteLine("How can we help you today? please select a choice from the below");
                    Console.WriteLine();

                    Console.WriteLine("Deposit: deposit into your checking/saving account.");
                    Console.WriteLine("Withdrawal: withdraw from your checking/saving account.");
                    Console.WriteLine("Transfer: transfer between your checking and saving accounts.");
                    Console.WriteLine("Quit: quit the App.");
                    Console.WriteLine();
                    Console.Write("Choice: ");
                    var choice = Console.ReadLine().ToLower();

                    if (choice == "quit")
                    {
                        quit = true; // if the quit is set to true so why I still see the two lines below before it quits

                        Console.WriteLine();
                        Console.WriteLine("===== Thanks for being a customer of First bank of Suncoast =====");
                    }

                    else if (choice == "deposit")
                    {

                        Console.Write("Which account you wish to deposit into? Checking/Saving: ");
                        var account = Console.ReadLine().ToLower();

                        Console.WriteLine();
                        Console.Write("How much you are going to deposit today?: ");
                        var amount = int.Parse(Console.ReadLine());

                        if (account == "checking" || account == "saving")
                        {
                            var transaction = new Transaction()
                            {
                                TransactionType = choice,
                                AccountType = account,
                                Amount = amount,
                                Date = DateTime.Now

                            };

                            transactions.Add(transaction);

                            if (account == "checking")
                            {
                                checkingBalance += transaction.Amount;
                                Console.WriteLine();
                                Console.WriteLine($"Your transaction is successful and total checking balance now is ${checkingBalance}");
                                Console.WriteLine();
                                Console.Write("Do you want to make another transaction? Y|N Answer: ");
                                var answer = Console.ReadLine().ToLower();

                                if (answer == "n")
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Thanks for being our customer");

                                    quit = true;
                                }
                            }
                            else if (account == "saving")
                            {
                                savingBalance += transaction.Amount;
                                Console.WriteLine();
                                Console.WriteLine($"Your transaction is successful and total saving balance now is ${savingBalance}");
                                Console.WriteLine();

                                Console.Write("Do you want to make another transaction? Y|N Answer: ");
                                var answer = Console.ReadLine().ToLower();

                                if (answer == "n")
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("Thanks for being our customer");

                                    quit = true;
                                }
                            }


                        }

                    }
                    else if (choice == "withdrawal")

                    {
                        Console.Write("Which account you wish to withdraw from? Checking/Saving: ");
                        var account = Console.ReadLine();
                        Console.WriteLine();
                        Console.Write("How much you are going to withdraw today?: ");
                        var amount = int.Parse(Console.ReadLine());

                        if ((account == "checking" && checkingBalance < amount) || (account == "saving" && savingBalance < amount))
                        {

                            Console.WriteLine($"Insufficient balance to make the transaction please try lesser amount up to your account balance");

                        }
                        else
                        {

                            if (account == "Checking" || account == "saving")
                            {
                                var transaction = new Transaction()
                                {
                                    TransactionType = choice,
                                    AccountType = account,
                                    Amount = amount,
                                    Date = DateTime.Now
                                };

                                transactions.Add(transaction);

                                if (account == "checking")
                                {
                                    checkingBalance -= transaction.Amount;
                                    Console.WriteLine();
                                    Console.WriteLine($"Your transaction is successful and total checking balance now is ${checkingBalance}");
                                    Console.WriteLine();
                                    Console.Write("Do you want to make another transaction? Y|N Answer: ");
                                    var answer = Console.ReadLine().ToLower();

                                    if (answer == "n")
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine("Thanks for being our customer");

                                        quit = true;
                                    }
                                }
                                else if (account == "saving")
                                {
                                    savingBalance -= transaction.Amount;
                                    Console.WriteLine();
                                    Console.WriteLine($"Your transaction is successful and total saving balance now is ${savingBalance}");

                                }
                            }

                        }


                    }
                    else if (choice == "view")
                    {
                        Console.Write("Which account balance do you wish to view today? Saving or Checking? Answer: ");
                        var answer = Console.ReadLine().ToLower();

                        if (answer == "checking")
                        {
                            Console.WriteLine($"Your checking account total balance is ${checkingBalance}.");
                            Console.WriteLine();
                            Console.Write("Do you want to make another transaction? Y|N Answer: ");
                            var response = Console.ReadLine().ToLower();

                            if (response == "n")
                            {
                                Console.WriteLine();
                                Console.WriteLine("Thanks for being our customer");

                                quit = true;
                            }

                        }
                        else if (answer == "saving")
                        {
                            Console.WriteLine($"Your saving account balance is ${savingBalance}");
                            Console.WriteLine();
                            Console.Write("Do you want to make another transaction? Y|N Answer: ");
                            var response = Console.ReadLine().ToLower();

                            if (response == "n")
                            {
                                Console.WriteLine();
                                Console.WriteLine("Thanks for being our customer");

                                quit = true;
                            }


                        }

                    }
                }

                var fileWriter = new StreamWriter("Bank-Info.csv");
                var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);

                csvWriter.WriteRecords(transactions);

                fileWriter.Close();

            }


        }
    }
}
