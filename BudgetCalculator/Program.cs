using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BudgetCalculator
{
    class Program
    {
        // *************************************
        // App: The Budget Calculator
        // App Type: NET Framework - Console App
        // Author: Carlisle, Caleb
        // Date Created: 4/20/19
        // Date Revised: 4/28/19
        // *************************************

        static void Main(string[] args)
        {
            DisplayOpeningScreen();
            DisplayMainMenu();
            DisplayClosingScreen();
        }

        static int totalBudgetValue = 0;

        static void DisplayOpeningScreen()
        {
            Console.WriteLine();
            Console.WriteLine("\t\tBudget Calculator");
            Console.WriteLine("\tAn Easy Way to Make and Track your Budget");

            DisplayContinuePrompt();
        }

        static void DisplayMainMenu()
        {
            List<BudgetSector> budget = new List<BudgetSector>();
            BudgetInformation budgetInformation = new BudgetInformation();
            bool menuLoop = true;

            Console.Clear();
            Console.WriteLine("Would You Like to Load a Saved Budget? (Y/N)");
            if (Console.ReadLine().ToLower().Trim() == "y")
            {
                budgetInformation.DisplayLoadBudget();
                totalBudgetValue = LoadBudgetLimit();
                Console.WriteLine("Budget has been loaded.");
                DisplayContinuePrompt();
            }

            do
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Main Menu");
                Console.WriteLine();

                Console.WriteLine("\t1) View Budget");
                Console.WriteLine("\t2) View Budget Sector");
                Console.WriteLine("\t3) Create/Update Total Budget Limit");
                Console.WriteLine("\t4) Add Budget Sector");
                Console.WriteLine("\t5) Remove Budget Sector");
                Console.WriteLine("\tE) Exit");
                Console.WriteLine();
                Console.Write("Enter Menu Choice: ");

                switch (Console.ReadLine().ToLower())
                {
                    case "1":
                        DisplayBudget(budgetInformation, totalBudgetValue);
                        break;
                    case "2":
                        DisplayBudgetSector(budget, totalBudgetValue, budgetInformation);
                        break;
                    case "3":
                        totalBudgetValue = DisplayUpdateBudgetLimit();
                        break;
                    case "4":
                        DisplayAddBudgetSector(budget, budgetInformation);
                        break;
                    case "5":
                        DisplayRemoveBudgetSector(budgetInformation);
                        break;
                    case "e":
                        Console.Clear();
                        Console.WriteLine("Would You Like to Load a Saved Budget? (Y/N)");
                        if (Console.ReadLine().ToLower().Trim() == "y")
                        {
                            budgetInformation.DisplaySaveBudget();
                            SaveBudgetLimit(totalBudgetValue);
                            Console.WriteLine("Budget has been saved.");
                            DisplayContinuePrompt();
                        }
                        Console.Clear();
                        menuLoop = false;
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a valid menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (menuLoop);
            
        }

        static int LoadBudgetLimit()
        {
            string stringBudgetValue;
            string[] budgetValue = new string[0];
            budgetValue = File.ReadAllLines(@"Data\BudgetLimitSave.txt");
            stringBudgetValue = budgetValue[0];
            
            if (!int.TryParse(stringBudgetValue, out int totalBudgetValue))
            {
                totalBudgetValue = 0;
            }
            return totalBudgetValue;
        }

        static void SaveBudgetLimit(int totalBudgetValue)
        {
            string stringBudgetValue = totalBudgetValue.ToString();
            File.WriteAllText(@"Data\BudgetLimitSave.txt", stringBudgetValue);
        }

        static void DisplayBudget(BudgetInformation budgetInformation, int totalBudgetValue)
        {
            DisplayHeader("Budget");

            budgetInformation.DisplayBudget(totalBudgetValue);

            DisplayContinuePrompt();
        }

        static void DisplayBudgetItem(BudgetSector item)
        {
            Console.WriteLine(item.Name);
            Console.WriteLine("$" + item.SectorValue);
            Console.WriteLine();
        }

        static void DisplayBudgetSector(List<BudgetSector> budget, int totalBudgetValue, BudgetInformation budgetInformation)
        {
            DisplayHeader("View Budget Sector");
            
            budgetInformation.DisplayBudgetSector(totalBudgetValue);

            DisplayContinuePrompt();
        }

        static int DisplayUpdateBudgetLimit()
        {
            int budgetLimit;
            bool validResponse;
            
            do
            {
                DisplayHeader("Create/Update Budget Limit");

                validResponse = true;

                Console.Write("Enter Desired Budget Limit: ");
                if (int.TryParse(Console.ReadLine(), out budgetLimit))
                {
                    Console.WriteLine();
                    Console.WriteLine($"The Budget Limit has been set to {budgetLimit}.");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Please Enter a Valid Response.");
                    DisplayContinuePrompt();
                    validResponse = false;
                }
            } while (!validResponse);

            DisplayContinuePrompt();

            return budgetLimit;
        }

        static void DisplayAddBudgetSector(List<BudgetSector> budget, BudgetInformation budgetInformation)
        {
            DisplayHeader("Add a New Budget Sector");

            
            BudgetSector sector = new BudgetSector();
            
            Console.Write("Enter Name: ");
            sector.Name = Console.ReadLine();

            Console.Write("Enter Value: ");
            int.TryParse(Console.ReadLine(), out int sectorValue);
            sector.SectorValue = sectorValue;

            budget.Add(sector);

            budgetInformation.DisplayAddBudgetSector(sector.Name, sector.SectorValue);
            
            if (budget.Contains(sector))
            {
                Console.WriteLine();
                Console.WriteLine($"Item -- ");
                DisplayBudgetItem(sector);
            }

            DisplayContinuePrompt();
        }

        static void DisplayRemoveBudgetSector(BudgetInformation budgetInformation)
        {
            DisplayHeader("Remove Budget Sector");

            Console.WriteLine();
            Console.WriteLine("Enter the Sector to Remove: ");
            budgetInformation.DisplayRemoveBudgetSector();

            DisplayContinuePrompt();
        }

        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static void DisplayHeader(string headertext)
        {
            // display header
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t" + headertext);
            Console.WriteLine();
        }

        static void DisplayClosingScreen()
        {
            DisplayHeader("\t\tThank you for using this application.");

            DisplayContinuePrompt();
        }
    }

    class BudgetInformation
    {
        static BudgetInformation budgetInformation;
        Dictionary<string, BudgetSector> budgetDictionary;
        BinaryFormatter formatter;
        string savePath = @"Data\BudgetSave.txt";

        public static BudgetInformation Instance()
        {
            if (budgetInformation == null)
            {
                budgetInformation = new BudgetInformation();
            }

            return budgetInformation;
        }

        public BudgetInformation()
        {
            this.budgetDictionary = new Dictionary<string, BudgetSector>();
            this.formatter = new BinaryFormatter();
        }

        public void DisplayAddBudgetSector(string name, int sectorValue)
        {
            if (!this.budgetDictionary.ContainsKey(name))
            {
                this.budgetDictionary.Add(name, new BudgetSector(name, sectorValue));
            }
        }

        public void DisplayRemoveBudgetSector()
        {
            string name = Console.ReadLine();
            foreach (BudgetSector item in this.budgetDictionary.Values)
            {
                if (this.budgetDictionary.ContainsKey(name))
                {
                    this.budgetDictionary.Remove(name);
                    Console.WriteLine();
                    Console.WriteLine($"{name} has been removed.");
                    break;
                }
                else
                {
                    Console.WriteLine($"{name} has not been added before.");
                    break;
                }
            }
            
        }

        public void DisplaySaveBudget()
        {
            FileStream writerFileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            this.formatter.Serialize(writerFileStream, this.budgetDictionary);
            writerFileStream.Close();
        }

        public void DisplayLoadBudget()
        {
            FileStream readerFileStream = new FileStream(savePath, FileMode.Open, FileAccess.Read);
            this.budgetDictionary = (Dictionary<string, BudgetSector>)this.formatter.Deserialize(readerFileStream);
            readerFileStream.Close();
        }

        public void DisplayBudget(int totalBudgetValue)
        {
            int budgetLimit = 0;
            if (this.budgetDictionary.Count > 0)
            {
                Console.WriteLine($"Budget Limit: {totalBudgetValue}");
                Console.WriteLine();

                foreach (BudgetSector item in this.budgetDictionary.Values)
                {
                    Console.WriteLine($"Name: {item.Name}");
                    Console.WriteLine($"Value: ${item.SectorValue}");
                    if (totalBudgetValue > 0)
                    {
                        Console.WriteLine($"Percentage: {100 * item.SectorValue / totalBudgetValue}%");
                    }
                    Console.WriteLine();
                }

                foreach (BudgetSector item in this.budgetDictionary.Values)
                {
                    budgetLimit = item.SectorValue + budgetLimit;
                }

                if (budgetLimit > totalBudgetValue)
                {
                    Console.WriteLine();
                    Console.WriteLine($"The budget has been exceeded by ${budgetLimit - totalBudgetValue}.");
                }
            }
            else
            {
                Console.WriteLine("There is no budget information to display.");
            }
        }

        public void DisplayBudgetSector(int totalBudgetValue)
        {
            if (this.budgetDictionary.Count > 0)
            {
                Console.WriteLine("Enter the Sector to View: ");

                foreach (BudgetSector item in this.budgetDictionary.Values)
                {
                    if (item.Name == Console.ReadLine())
                    {
                        Console.WriteLine($"Name: {item.Name}");
                        Console.WriteLine($"Value: {item.SectorValue}");
                        Console.WriteLine($"Percentage: {100 * item.SectorValue / totalBudgetValue}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("This budget sector does not exist.");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Budget Sectors Have Been Added.");
            }
        }
    }

    [Serializable]
    public class BudgetSector
    {
        #region FIELDS

        private string _name;
        private int _sectorValue;

        #endregion

        #region PROPERTIES

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int SectorValue
        {
            get { return _sectorValue; }
            set { _sectorValue = value; }
        }

        #endregion

        #region METHODS

        

        #endregion

        #region CONSTRUCTORS

        public BudgetSector()
        {

        }

        public BudgetSector(string name, int sectorValue)
        {
            _name = name;
            _sectorValue = sectorValue;
        }

        #endregion
    }
}
