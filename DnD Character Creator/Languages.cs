using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Languages
    {
        public List<string> List = new List<string>();
        public string Native;
        public List<string> Background = new List<string>();
        public static List<string> SelectLanguages(string native, string race, string background)
        {
            //if the user set 'Acolyte as a background, int choices will be +2, if they picked Human, Elf, Half-Elf, they'll get +1
            Console.Clear();
            int choices = 0;
            if (background == "Acolyte")
            {
                choices += 2;
            }
            if (race == "Elf" || race == "Human" || race == "Half-Elf")
            {
                choices += 1;
            }

            List<string> knownLanguages = new List<string> { "Common" };   //Language list refreshes to Common
            knownLanguages.Add(native); //Every race except Human has a native language

            List<string> languageList = new List<string> { "Dwarven", "Elvish", "Giant", "Gnomish", "Goblin", "Halfling", "Orc", "Abyssal",     //All languages
                                                           "Celestial", "Draconic", "Deep-Speech", "Infernal", "Primordial", "Sylvan", "Undercommon" };

            if (languageList.Contains(native))  //Removes native language from list to prevent user from selecting it later.
            {
                languageList.Remove(native);
            }
            string[] languageArray = languageList.ToArray();    //Converts the list to an array to use in my selection method.

            for (int i = 0; i < choices; i++)
            {
                //User is told their extra language criteria, told how many they have, and given a list to choose from. The list removes languages as the user selects them.
                knownLanguages.Add(Builder.ItemToList($"Your character will gain an extra language if the selected race is Human or Half-Elf. \r\n" +
                                                     $"Your character will gain 2 extra languages if the selected background is Acolyte. \r\n" +
                                                     $"Your current settings grant {choices} extra languages. \r\n\r\n" +
                                                     $"Selection {i + 1}:\r\n", "", languageArray, languageArray));
                languageArray = Proficiency.Configuration.ProficiencyListUpdate(knownLanguages, languageArray);
            }
            //A final list is created. the loop will check each language. If the item is NOT in the new list, it will be added. This is to filter out repeats.
            List<string> knownLanguagesDistinct = new List<string>();
            foreach (string item in knownLanguages)
            {
                if (!knownLanguagesDistinct.Contains(item))
                {
                    knownLanguagesDistinct.Add(item);
                }
            }
            return knownLanguagesDistinct;
        }
    }

}
