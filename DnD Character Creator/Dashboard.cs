using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Dashboard
    {
        public static string Controller()
        {
            //'check' is the factor keeping the loop active and 'choice' controls the loop. The loop continues until a proper 'choice' is made.
            bool check = true;
            int choice = 0;
            string[] menuOption = { "Name", "Gender", "Race", "Background & Class", "Alignment", "Stats", "Language/Proficiency", "Null" };

            while (check == true)
            {
                try     //'choice' aligns the user input with the array index for menuOptions. Legal choice entry breaks out of the loop and sends the user to a submenu.
                {
                    choice = int.Parse(Console.ReadLine()) - 1;
                    check = false;
                }
                catch (FormatException) //If the user enters an illegal data type, the catch prevents the application from crashing
                {
                    choice = 7;
                    check = false;
                }
            }
            if (choice < 0 || choice > 7) //If the user enters an incorect number, choice defaults to 7. The switch statement will use the "Null" case to take no action and restart the menu loop.
            {
                choice = 7;
            }
            return menuOption[choice];
        }
        public static void Menu(CharacterBasic character, Health health, Stats abilityScores, Stats abilityModifiers, Stats savingThrows, Equipment equipment, Languages languages, Proficiency proficiencies, Skills skills, CharacterClass characterClass, Features features, bool langProfFlag)   //Lists every important user choice. Information here will be used in future updates to launch to a webpage using HTML/CSS to allow easy porting of configs to character sheet
        {
            //Displays all the user's selections and everything they gain from said selections.
            //Red indicates unfinished, Green indicates completed
            Console.Clear();
            BasicInformation(character, abilityScores, langProfFlag);
            SavingThrows(character, proficiencies);
            HealthInformation(health, character, abilityModifiers, equipment);
            AbilityScores(abilityScores, abilityModifiers);

            Generic("\r\nRace Features: ", features.Race);
            ClassFeatures(character, features);
            Generic("\r\nLanguages: ", languages.List);
            Generic("\r\nEquipment: ", equipment.Bag);
            Generic("\r\nTools Proficiency: ", proficiencies.Tools);
            Generic("\r\nArms Proficiency: ", proficiencies.Arms);
            
            Generic("\r\nSkills: ", proficiencies.Skills);
            Skills(savingThrows, skills, abilityScores);
            Console.Write("\r\nEnter selection: ");
        }
        public static void BasicInformation(CharacterBasic character, Stats abilityScores, bool langProfFlag)
        {
            string[]dashboardItems = { $"1.  Name:          {character.Name}",
                                       $"2.  Gender:        {character.Gender}",
                                       $"3.  Race:          {character.SubRace}{character.Draconic} {character.Race}",
                                       $"4.  Background:    {character.Background}",
                                       $"    Class:         {character.SubClass} {character.CClass}",
                                       $"5.  Alignment:     {character.Alignment}",
                                        "6.  Stats          "};
            bool[] dashboardSets = { character.NameSet, character.GenderSet, character.RaceSet, character.BackgroundSet, character.CClassSet, character.AlignmentSet, abilityScores.Set };
            for (int i = 0; i < dashboardItems.Length; i++)
            {
                if (dashboardSets[i] == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (dashboardSets[i] == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(dashboardItems[i]);
            }
            if (langProfFlag == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("7.  Language/Proficiency");
            }
            else if (langProfFlag == false)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("7.  Language/Proficiency");
            }
            Console.ResetColor();
        }
        public static void HealthInformation(Health health, CharacterBasic character, Stats abilityModifiers, Equipment equipment)
        {
            int hillDwarf = 0;
            if (character.SubRace == "Hill" && character.Race == "Dwarf")   //Hill Dwarfs have a racial trait that they gain +1 HP per character level.
            {
                hillDwarf = (1 * character.Level);
            }
            health.HitPoints = health.HitPointsConstant + abilityModifiers.Constitution + hillDwarf; //HP is determined by a constant provided by character class. HP = constant + (Constitution ability modifier)
            health.ArmorClass = Health.SetArmorClass(abilityModifiers, equipment); //AC is determined by armor, shield, and/ or Dexterity modifier.
            health.Initiative = abilityModifiers.Dexterity; //Initiative directly equals Dexterity modifier. Other factors apply but are irrelevent within application scope.
            //If information in this display panel is not yet completed by user input, it should not display and remain red. Only finalized details should display.
            string[] valueList = { "HP: ", "   AC: ", "   Hit Dice: ", "   Initiative: ", "   Speed: " };
            string[] blankerList = { health.HitPoints.ToString(), health.ArmorClass.ToString(), health.HitDice, health.Initiative.ToString(), character.Speed.ToString() };
            bool [] boolList = { health.HitPointsSet, health.ArmorClassSet, health.HitDiceSet, health.InitiativeSet, character.RaceSet };

            for (int i = 0; i < boolList.Length; i++)   //All false booleans in the list will yield a blank string.
            {
                if (boolList[i] == false)
                {
                    blankerList[i] = "";
                }
            }
            for (int i = 0; i < valueList.Length; i++)  //All true booleans in the list will yield their value in string form.
            {
                if (blankerList[i] == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write("{0}{1}", valueList[i], blankerList[i]);
            }
            Console.ResetColor();
        }
        public static void AbilityScores(Stats abilityScores, Stats abilityModifiers)
        {
            string[] display = {"-", "-", "-", "-", "-", "-"};
            if (abilityScores.Set == false) //Ability scores are not assigned, so the display should be red.
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else    //Ability modifiers translate to strings, display becomes green.
            {
                string[] modDisplay = { abilityModifiers.Constitution.ToString(), abilityModifiers.Strength.ToString(), abilityModifiers.Dexterity.ToString(), 
                            abilityModifiers.Intelligence.ToString(), abilityModifiers.Wisdom.ToString(), abilityModifiers.Charisma.ToString()};
                display = modDisplay;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            //Writes the end results for ability scores and modifiers. Other factors affect these values, but use of the 'Stats' menu is most important.
            Console.WriteLine("\r\n" + "Con {0}({1})  Str {2}({3})  Dex {4}({5}) Int {6}({7})  Wis {8}({9})  Cha {10}({11})",
                                    abilityScores.Constitution, display[0],
                                    abilityScores.Strength, display[1],
                                    abilityScores.Dexterity, display[2],
                                    abilityScores.Intelligence, display[3],
                                    abilityScores.Wisdom, display[4],
                                    abilityScores.Charisma, display[5]);
            Console.ResetColor();
        }
        public static void Generic(string displayListing, List<string> generic)
        {
            //This method is effectively the same as the CommaHelper method. It is used as an intermediary method to keep the code looking consistent.
            CommaHelper(displayListing, generic);
        }
        public static void SavingThrows(CharacterBasic character, Proficiency proficiencies)
        {
            //The stats assigned to saving throw proficiency are determined by Character Class.
            if (character.CClassSet == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine("\r\nSaving Throws: {0} {1}", proficiencies.SavingThrows[0], proficiencies.SavingThrows[1]);
            Console.ResetColor();
        }
        public static void Skills(Stats savingThrows, Skills skills, Stats abilityScores)
        {
            //If user hasnt used the "Stats' menu, ability modifiers are incomplete. The skill proficiencies directly depend on these skills,
            //so the user doesn't need to be notified of their value.

            //All saving throws and skills
            int[] skillList = {savingThrows.Constitution, savingThrows.Strength, savingThrows.Dexterity, savingThrows.Intelligence,savingThrows.Wisdom, savingThrows.Charisma,
                               skills.Athletics, skills.Acrobatics, skills.SleightOfHand, skills.Stealth,
                               skills.Arcana, skills.History, skills.Investigation, skills.Nature,
                               skills.Religion, skills.AnimalHandling, skills.Insight, skills.Medicine,
                               skills.Perception, skills.Survival, skills.Deception, skills.Intimidation,
                               skills.Performance, skills.Persuasion};
            string[] skillListDash = new string[skillList.Length];
            if (abilityScores.Set == false) //If ability scores are not set, the skills will numerically be -5 + racial bonus. This will confuse the user. This code dashes out the number.
            {
                for (int i = 0; i < skillList.Length; i++)
                {
                    skillListDash[i] = "-";
                }
            }
            else    //This code runs if the ability scores have been assigned. Skill values translate to strings and are inserted into the displayed table.
            {
                for (int i = 0; i < skillList.Length; i++)
                {
                    skillListDash[i] = skillList[i].ToString();
                }
                for (int i = 0; i < skillListDash.Length; i++)  //Negative numbers misalign the table. This code adjusts the data so the table remains aligned.
                {
                    if (!skillListDash[i].Contains("-"))
                    {
                        string spacer = $" {skillListDash[i]}";
                        skillListDash[i] = spacer;
                    }
                }
            }
            //This code prints a table for the user to see their skill values. Incomplete values are replaced with a dash.
            Console.Write("\r\n" +
                              "        Constitution:    {0}    Athletics(Str):         {6}     Investigation(Int):     {12}     Perception(Wis):    {18}\r\n" +
                              "        Strength:        {1}    Acrobatics(Dex):        {7}     Nature(Int):            {13}     Survival(Wis):      {19}\r\n" +
                              "        Dexterity:       {2}    Sleight of Hand(Dex):   {8}     Religion(Int):          {14}     Deception(Cha):     {20}\r\n" +
                              "        Intelligence:    {3}    Stealth(Dex):           {9}     Animal Handling(Wis):   {15}     Intimidation(Cha):  {21}\r\n" +
                              "        Wisdom:          {4}    Arcana(Int):            {10}     Insight(Wis):           {16}     Performance(Cha):   {22}\r\n" +
                              "        Charisma:        {5}    History(Int)            {11}     Medicine(Wis):          {17}     Persuasion(Cha):    {23}\r\n",
                              skillListDash[0], skillListDash[1], skillListDash[2], skillListDash[3], skillListDash[4], skillListDash[5], skillListDash[6], skillListDash[7], skillListDash[8], skillListDash[9], skillListDash[10], skillListDash[11],
                              skillListDash[12], skillListDash[13], skillListDash[14], skillListDash[15], skillListDash[16], skillListDash[17], skillListDash[18], skillListDash[19], skillListDash[20], skillListDash[21], skillListDash[22], skillListDash[23]);
        }

        public static void ClassFeatures(CharacterBasic character, Features features)
        {
            Dashboard.Generic("\r\nClass Features: ", features.Class);
            //else if (character.CClass == "Cleric")
            //{
            //    Console.Write("Spell DC: {0} Spell ATK: {1} Cantrips: {2} Domain Spells: {3}");
            //}
            //else if (character.CClass == "Wizard")
            //{
            //    Console.Write("Spell DC: {0} Spell ATK: {1} Cantrips: {2} Spells: {3} ");
            //}
        }
        public static void CommaHelper(string prompt, List<string> list)
        {
            //For Displaying lists, commas should separate items.
            Console.Write(prompt);
            list.Remove("");

            foreach (string item in list) //This loop checks for the last item in the list and prevents an unnecessary comma appearing at the end of the list.
             {
                if (item != list.Last())
                {
                    Console.Write(item + ", ");
                }
                else
                {
                    Console.Write(item + " ");
                }
            }
        }
    }
}
