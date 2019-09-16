using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Builder
    {
        public static string Selection(string prompt, string[] option, string[] selection) //Constructs selection choices into a list, returns selection back to Main
        {
            bool check = true;
            int choice = 0;
            while (check == true)
            {
                Console.Clear();
                Console.WriteLine(prompt);
                for (int i = 0; i < option.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, option[i]);
                }
                try
                {
                    choice = int.Parse(Console.ReadLine()) - 1;
                    check = false;
                }
                catch (Exception)
                {
                    Console.Clear();
                }
            }
            check = false;

            try
            {
                return selection[choice];
            }
            catch (IndexOutOfRangeException)
            {
                while (check == false)
                {
                    Console.WriteLine("Improper selection. \r\n" +
                                      "Press enter to try again.");
                    Console.ReadLine();
                    return "";
                }
            }
            return selection[choice];

        }
        public static void DataLine(string prompt, List<string> names, List<int> data)   //Constructs a line of numbers, the line can be named.
        {
            List<int> dataSet = data;
            List<string> dataNames = names;
            while (dataNames.Count < dataSet.Count)
            {
                dataNames.Add("");
            }
            Console.Write(prompt);
            for (int i = 0; i < dataSet.Count(); i++)
            {
                Console.Write(dataNames[i] + " " + dataSet[i] + " ");
            }
        }
        public static string ItemToList(string prompt, string itemToList, string[] option, string[] selection)
        {
            while (itemToList == "")
            {
                itemToList = Builder.Selection(prompt, option, selection);
            }
            return itemToList;
        }
    }
}
