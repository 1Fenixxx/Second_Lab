using System.Collections.Generic;
using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuFunctions.data = new List<City>();
            Menu menu = new Menu();
            ConsoleKeyInfo pressedKey;

            menu.AddElement(new Menu.MenuItem("Open cityes list", ConsoleColor.White, MenuFunctions.OpenCityesList));
            menu.AddElement(new Menu.MenuItem("Add new city", ConsoleColor.Green,MenuFunctions.CreateNewCity));
            menu.AddElement(new Menu.MenuItem("Delete city", ConsoleColor.Yellow, MenuFunctions.DeleteCity));

            while (true)
            {
                menu.Print();
                Thread.Sleep(16);
                pressedKey = Console.ReadKey();
                if (pressedKey.Key == ConsoleKey.DownArrow)
                    menu.ChangeChosedElement(false);
                if (pressedKey.Key == ConsoleKey.UpArrow)
                    menu.ChangeChosedElement(true);
                if (pressedKey.Key == ConsoleKey.Enter)
                    menu.ChoseMenuItem();
            }
        }
    }
    public static class MenuFunctions
    {
        public static List<City> data;
        public static void OpenCityesList()
        {
            int i = 0;
            foreach (City city in data)
            {
                Console.WriteLine(i + " city index");
                Console.Write(city.ToString());
                i++;
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        public static void CreateNewCity()
        {
            City.CityDesc desc = new City.CityDesc();

            paramInput("Region", ref desc.Region);
            paramInput("Name", ref desc.Name);
            paramInput("Mayor", ref desc.Mayor);
            paramInput("Pollution", ref desc.Pollution);
            paramInput("Crimes", ref desc.Crimes);
            paramInput("Zones Count", ref desc.ZonesNumber);
            paramInput("Population", ref desc.Population);

            data.Add(new City(desc));
        }
        private static void paramInput(string paramName, ref string variable)
        {
            try
            {
                Console.Write("Enter city " + paramName + ":");
                variable = Console.ReadLine();
            }
            catch { }
        }
        private static void paramInput(string paramName, ref double variable)
        {
            try
            {
                Console.Write("Enter device " + paramName + ":");
                variable = Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                ErrorLogger.Error(paramName);
                paramInput(paramName, ref variable);
            }
        }
        private static void paramInput(string paramName, ref int variable)
        {
            try
            {
                Console.Write("Enter city " + paramName + ":");
                variable = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                ErrorLogger.Error(paramName);
                paramInput(paramName, ref variable);
            }
        }
        public static void DeleteCity()
        {
            Console.Write("Enter index of city for delete: ");
            int index = Convert.ToInt32(Console.ReadLine());
            data.RemoveAt(index);
        }
    }
    public static class ErrorLogger
    {
        public static void Error(string paramName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Wrong Symbol or empty entered to " + paramName + ", please try again.\n For float numbers please enter , instead of .\n For int number enter only numbers");
            Console.ResetColor();
        }
    }
    public class City
    {
        public struct CityDesc
        {
            public string Region;
            public string Name;
            public string Mayor;
            public double Pollution;
            public int Crimes;
            public int ZonesNumber;
            public int Population;
        }
        public City(string Region, string Name, string Mayor, double Pollution, int Crimes, int ZonesNumber, int Population)
        {
            Description = new CityDesc
            {
                Region = Region,
                Name = Name,
                Mayor = Mayor,
                Pollution = Pollution,
                Crimes = Crimes,
                ZonesNumber = ZonesNumber,
                Population = Population
            };
        }
        public City(CityDesc CityDescription)
        {
            this.Description = CityDescription;
        }

        public override string ToString()
        {
            string output = string.Empty;

            output += "   Region: " + Description.Region + '\n';
            output += "     Name: " + Description.Name  + '\n';
            output += "    Mayor: " + Description.Mayor + '\n';
            output += "  Pollution: " + Description.Pollution + '\n';
            output += "     Crimes: " + Description.Crimes + '\n';
            output += "ZonesNumber: " + Description.ZonesNumber + '\n';
            output += " Population: " + Description.Population + '\n';

            return output;
        }

        public void SetCityDescription(CityDesc desc)
        {
            Description = desc;
        }

        public CityDesc Description { get; private set; }
    }
    
    //don`t using in primitive programming style (that`s primitive too)
    public class Menu
    {
        //inner MenuItem class for menu items in menu
        public class MenuItem
        {
            public MenuItem(string Title, WorkFunction function)
            {
                this.Title = Title;
                this.WorkMethod = function;
            }
            public MenuItem(string Title, ConsoleColor TextColor, WorkFunction function)
            {
                this.Title = Title;
                this.Color = TextColor;
                this.WorkMethod = function;
            }

            public delegate void WorkFunction();

            public void Print()
            {
                Console.ForegroundColor = this.Color;
                Console.WriteLine(this.Title);
                Console.ResetColor();
            }

            public string Title { get; set; }
            public WorkFunction WorkMethod { get; private set; }
            public ConsoleColor Color { get; private set; }
        }

        private static int counter = 0;

        public Menu()
        {
            this.AddElement(new MenuItem("Exit",ConsoleColor.Red,exitMenuItemDefaultFunction));
        }

        //new menuitem adding method
        public void AddElement(params MenuItem[] elements)
        {
            for(int i = 0; i < elements.Length; i++)
            {
                string newTitle = counter + " " + elements[i].Title;
                elements[i].Title = newTitle;
                menuItemsList.Add(elements[i]);
                counter++;
            }
        }

        //Menu printing method
        public void Print()
        {
            Console.Clear();
            foreach(MenuItem menuItem in menuItemsList)
            {
                menuItem.Print();
            }
            Console.WriteLine("Chosed menuitem: " + this.ChosedElementIndex);
        }

        //using for call chosed menu item work function
        public void ChoseMenuItem()
        {
            menuItemsList[ChosedElementIndex].WorkMethod();
        }

        //using for changing ChosedElementIndex prophety. True for index++ and False for index--
        public void ChangeChosedElement(bool up_down)
        {
            if (!up_down)
            {
                if (ChosedElementIndex + 1 < menuItemsList.Count)
                    ChosedElementIndex++;
            }
            else
            {
                if (ChosedElementIndex - 1 >= 0)
                    ChosedElementIndex--;
            }
        }

        //index of the chosed element in menu
        public int ChosedElementIndex { get; private set; }

        //MenuItems list, using for store menu elements
        private List<MenuItem> menuItemsList = new List<MenuItem>();

        //default function for "Exit" item in menu
        private void exitMenuItemDefaultFunction()
        {
            Console.Write("EXIT");
            for(int i = 0; i < 4; i++)
            {
                Console.Write('.');
            }
            Environment.Exit(0);
        }
    }
}
