using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Proficiency
    {
        public int ProficiencyBonus { get; set; }
        public List<string> Arms { get; set; }
        public List<string> RaceArms { get; set; }
        public List<string> ClassArms { get; set; }
        public List<string> Skills { get; set; }
        public List<string> RaceSkills { get; set; }
        public List<string> ClassSkills { get; set; }
        public List<string> Tools { get; set; }
        public List<string> RaceTools { get; set; }
        public List<string> ClassTools { get; set; }
        public List<string> SavingThrows { get; set; }

        public int Options;

        public Proficiency(int proficiencyBonus, List<string> arms, List<string> skills, List<string> tools, List<string> savingThrows)
        {
            ProficiencyBonus = proficiencyBonus;
            Arms = arms;
            Skills = skills;
            Tools = tools;
            SavingThrows = savingThrows;
        }

        public class Configuration
        {
            public static List<string> ListHelper(List<string> main, List<string> race, List<string> cClass) //Imports to final list from support lists
            {
                if (race is null == false)
                {
                    foreach (string item in race)
                    {
                        main.Add(item);
                    }
                }
                if (cClass is null == false)
                {
                    foreach (string item in cClass)
                    {
                        main.Add(item);
                    }
                }
                return main;
            }
            public static List<string> SelectSkills(CharacterBasic character, CharacterClass characterClass)    //Uses objects from main to determine skill options.
            {

                int options = 2; //All current character classes except Rogue get 2 options.
                string raceSkill; //Some races get a free skill intrinsically. For others, this string will stay blank.
                List<string> knownProficiencies = new List<string>(); //Empty list to build and send back to Program Main.

                switch (character.Race) //This assignment comes first to prevent intrinsic skills from appearing in later selection.
                {
                    case "Elf":
                        raceSkill = "Perception";
                        break;
                    case "Half-Orc":
                        raceSkill = "Intimidation";
                        break;
                    default:
                        raceSkill = "";
                        break;
                }
                knownProficiencies.Add(raceSkill);
                if (character.CClass == "Rogue")    //Rogues get 4 skill choices. Other classes get 2.
                {
                    options += 2;
                }
                List<string> classOptions = characterClass.ClassSkillProficiencies.ToList<string>(); //Skills by character classare added to a list.
                List<string> allProficiencies = new List<string> { "Athletics", "Acrobatics", "SleightOfHand", "Stealth", "Arcana", "History", "Investigation", "Nature", "Religion",   //Full skill list
                                                                   "AnimalHandling", "Insight", "Medicine", "Perception", "Survival", "Deception", "Intimidation", "Performance", "Persuasion" };

                if (classOptions.Contains(raceSkill))   //Removes race skill so that it wont appear in future selection
                {
                    classOptions.Remove(raceSkill);
                }
                //Prompts the user based on background.
                string acolytePrompt = "The 'Acolyte' background is not enabled, which would enable proficiency in Insight and Religion. \r\n"; //Default prompt
                if (character.Background == "Acolyte")
                {
                    //'Acolyte' grants Insight and Religion. They are added to 'known' list. These skills are removed from options list so they dont appear in later selection.
                    knownProficiencies.Add("Insight");
                    knownProficiencies.Add("Religion");
                    if (classOptions.Contains("Insight"))
                    {
                        classOptions.Remove("Insight");
                    }
                    if (classOptions.Contains("Religion"))
                    {
                        classOptions.Remove("Religion");
                    }

                    string[] proficiencyOptions = classOptions.ToArray();

                    acolytePrompt = "The 'Acolyte' background grants proficiency in Insight and Religion. \r\n";
                    for (int i = 0; i < options; i++)   //Selection menu to choose unassigned skill proficiencies. Will remove skills from list as selected.
                    {
                        knownProficiencies.Add(Builder.ItemToList(acolytePrompt + "The " + character.CClass + " can choose proficiency in " + options + " of the following skills: \r\n\r\n" +
                                                                $"Selection {i + 1}:\r\n", "", proficiencyOptions, proficiencyOptions));
                        proficiencyOptions = ProficiencyListUpdate(knownProficiencies, proficiencyOptions);
                    }
                    options = 0; //Zero-ed out to prevent unexpected errors.
                }
                else
                {
                    for (int i = 0; i < options; i++)   //Selection menu to choose unassigned skill proficiencies. Will remove skills from list as selected.
                    {
                        knownProficiencies.Add(Builder.ItemToList(acolytePrompt + "The " + character.CClass + " can choose proficiency in " + options + " of the following skills: \r\n\r\n" +
                                                                  $"Selection {i + 1}:\r\n", "", characterClass.ClassSkillProficiencies, characterClass.ClassSkillProficiencies));

                        characterClass.ClassSkillProficiencies = ProficiencyListUpdate(knownProficiencies, characterClass.ClassSkillProficiencies);
                    }
                    options = 0;    //Zero-ed out to prevent unexpected errors.
                }


                if (character.Race == "Half-Elf")
                {
                    options += 2;   //Half-Elves get 2 additional skill proficiencies
                }
                if (options > 0)    //If the character has any more options left, they will select from the full list of remaining unassigned skills
                {
                    string[] proficiencyOptions = allProficiencies.ToArray();
                    proficiencyOptions = ProficiencyListUpdate(knownProficiencies, proficiencyOptions);
                    for (int i = 0; i < options; i++)
                    {
                        knownProficiencies.Add(Builder.ItemToList("Half-Elves gain 2 extra proficiencies: \r\n\r\n" +
                                                                 $"Selection { i + 1}:\r\n", "", proficiencyOptions, proficiencyOptions));
                        proficiencyOptions = ProficiencyListUpdate(knownProficiencies, proficiencyOptions);
                    }
                }
                return knownProficiencies;  //User selections are all added to the list and return to Program Main
            }
            public static string[] ProficiencyListUpdate(List<string> knownProficiencies, string[] proficiencyList) //After a proficiency is added to a list, this method removes that proficiency from the selection list
            {
                List<string> bufferList = new List<string>();

                //If any skill in the proficiency selection list is not in the 'known' list, it will be added to a buffer list.
                foreach (string skill in proficiencyList)
                {
                    if (!knownProficiencies.Contains(skill))
                    {
                        bufferList.Add(skill);
                    }
                }
                return bufferList.ToArray(); //Buffer list of skills not known sent back for selection
            }
        }
    }
}
