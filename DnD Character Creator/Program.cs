using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    //Features to add:
    //High Priority:
    //1.    When the user finishes, there should be an option to launch a static webpage the contains all of their setting. 
    //      The page should have a fillable character sheet auto-filled with their entries
    //2.    Selecting prepared spell is not implemented. There's no distinction between cantrips and spells. The dashboard needs to display Spell DC, Spell ATK, Spell slots
    //3.    Point-Buy is not implemented for stat assignment.
    //4.    Only the standard 4 character classes are included in this build. Adding the rest will be a major effort.
    //Low priority
    //5.    Refactor RefreshValues() method. It is written poorly.
    //6.    Character background needs to include personality trait, ideal, bond, flaw.
    //7.    Use of the DiceRoll.Roll method if the user wants to pick something randomly.
    //8.    Include more descriptions for specific setings (ie. spells, etc.)
    //9.    Race selections that don't include a subrace are misaligned to the right by 1 space.
    class Program
    {
        static void Main(string[] args)
        {
            //Initializing objects, variables, etc.
            bool displayMenu = true;
            bool langProfFlag = true;

            //Name, Gender, Race, Subrace, Draconic Heritage, Class, Subclass, Background, Alignment, Level, Speed
            CharacterBasic character = new CharacterBasic();

            //HP, HP constant, AC, Hit Dice, Initiative
            Health health = new Health();

            //Constitution, Strength, Dexterity, Intelligence, Wisdom, Charisma
            Stats stats = new Stats();              //Generic object to hold numbers.
            Stats racialBonus = new Stats();        //These values come from race selection.
            Stats abilityScores = new Stats();      //These values are the end destination. Stat values and Racial values are added and stored here.
            Stats abilityModifiers = new Stats();   //These values are used for various skills and actions. Each number is determined directly by it's corresponding ability score from a table.
            Stats savingThrows = new Stats();       //These values are primarily used for specific proficiencies. Each number is determined by it's corresponding ability modifier + 2 if proficient.

            Languages languages = new Languages();
            Skills skills = new Skills();

            //Empty equipment list
            Equipment equipment = new Equipment(
                      bag: new List<string>(),
                      raceBag: new List<string>(),
                      classBag: new List<string>());
            //Empty features list
            Features features = new Features(
                     race: new List<string> { "" },
                     cClass: new List<string> { "" },
                     background: new List<string> { "" });

            //Empty proficiencies list
            Proficiency proficiencies = new Proficiency(
                        2,  //Proficiency Bonus
                        arms: new List<string>(),
                        skills: new List<string>(),
                        tools: new List<string>(),
                        savingThrows: new List<string> { "", "" }); //Invisible string values hard-coded in to prevent error.

            //Secondary object for collecting additional details from race decision.
            CharacterRace race = new CharacterRace(
                          "", "", "", 0, 0, "", racialBonus,   //Race, Subrace, Draconic Heritage, Speed, Extra Language, Extra Proficiency, native Language, Racial Bonus
                          features: new List<string>(),
                          raceArms: new List<string>(),
                          raceSkills: new List<string>(),
                          raceTools: new List<string>(),
                          cantrips: new List<string>());

            //Secondary object for collecting additional details from class decision.
            CharacterClass characterClass = new CharacterClass();
            SpellBook spellCasting = new SpellBook();

            StartUp();  //Display text
            while (displayMenu) //Encloses entire program
            {
                displayMenu = MainMenu();
            }
            bool MainMenu()
            {
                //This is the primary menu for selecting and editing aspects of the character.
                //When a character property is set, the display listing turns green.

                RefreshValues(); //Assigns the end results of stat configuration. Readjusts every time user returns to Main Menu
                Dashboard.Menu(character, health, abilityScores, abilityModifiers, savingThrows, equipment, languages, proficiencies, skills, characterClass, features, langProfFlag); //Display panel
                switch (Dashboard.Controller()) //Primary configuration menu
                {
                    case "Name":
                        Console.Clear();
                        Console.WriteLine("Name your character: ");
                        character.Name = Console.ReadLine();
                        if (character.Name is "")
                        {
                            character.NameSet = false;
                        }
                        else
                        {
                            character.NameSet = true;
                        }
                        return true;
                    case "Gender":
                        Console.Clear();
                        character.Gender = "";
                        character.Gender = character.SetGender();
                        character.GenderSet = true;
                        return true;
                    case "Race": //Races in D&D are better described as humanoid subspecies. Dwarfs differ from Elves like Pitbulls differ from Greyhounds.
                        Console.Clear();
                        character.Race = "";
                        RaceListsReset();   //Empties relevent lists used when character race is changed.
                        while (character.Race == "")
                        {
                            character.Race = race.SetRace();
                        }
                        racialBonus = CharacterRace.Conditionals(character, race, racialBonus, equipment, languages, proficiencies, features); //Secondary menu for character race
                        languages.List = new List<string> { "Common", languages.Native };   //The language list is cleared. Common and native language from race are set. Other languages known will be set under "Language/Proficiency" menu.

                        //Lists used in RaceConditionals are emptied. Preventative measure against unexpected errors.
                        equipment.RaceBag = new List<string>();         //Reset         
                        proficiencies.RaceTools = new List<string>();   //Reset
                        proficiencies.RaceArms = new List<string>();    //Reset
                        character.RaceSet = true;
                        langProfFlag = true;
                        return true;
                    case "Background & Class": //User can choose Acolyte background or None. Only the 4 classic D&D classes are included for now. The rest will be included in later updates.
                        Console.Clear();
                        ClassListsReset();  //Empties relevent lists used when character race is changed.
                        character.Background = "";
                        character.CClass = "";
                        string backgroundPrevious = character.Background;
                        character.Background = characterClass.SetBackground();
                        if (!character.Background.Equals(backgroundPrevious))   //If the user removes the 'Acolyte' background, the language list should reset so that they don't get the extra language benefits.
                        {
                            languages.List = new List<string>();
                        }
                        character.CClass = characterClass.SetClass();
                        characterClass = CharacterClass.Controller(character.CClass);   //Gets class features for selected class
                        CharacterClass.Conditionals(character, health, abilityModifiers, equipment, languages, proficiencies, characterClass, features, spellCasting); //Secondary menu for character class options. Includes integration of background benefits.

                        //Lists used in RaceConditionals are emptied. Preventative measure against unexpected errors.
                        equipment.ClassBag = new List<string>();        //Reset
                        languages.Background = new List<string>();      //Reset
                        proficiencies.ClassTools = new List<string>();  //Reset
                        proficiencies.ClassArms = new List<string>();   //Reset
                        proficiencies.ClassSkills = new List<string>(); //Reset
                        character.BackgroundSet = true;
                        character.CClassSet = true;
                        langProfFlag = true;
                        return true;
                    case "Alignment":   //Character alignment describes broad-stroked trajectory of character's moral/social tendencies. Mostly for role-play, but matters for Cleric/Paladin classes.
                        Console.Clear();
                        character.Alignment = "";
                        character.Alignment = character.SetAlignment();
                        character.AlignmentSet = true;
                        return true;
                    case "Stats":   //Series of menus that will ultimately facilitate assignment of character ability scores.
                        Console.Clear();
                        stats = Stats.Menu(character, stats, racialBonus);
                        stats.Set = true;
                        return true;
                    case "Language/Proficiency":    //Extra languages and proficiency choices will be determined here
                        if (character.Race == "" || character.CClass == "")     //If race or class are not set, returns to main menu
                        {
                            Console.Clear();
                            Console.WriteLine("This menu will become available after you've selected both a race and class. \r\n" +
                                              "Press Enter to return to the main menu.");
                            Console.ReadLine();
                            return true;
                        }
                        else      //Continues with selection
                        {
                            langProfFlag = false;
                            languages.List = Languages.SelectLanguages(languages.Native, character.Race, character.Background); //Adjusts for extra languages
                            proficiencies.Skills = Proficiency.Configuration.SelectSkills(character, characterClass);           //Adjusts for skill options
                        }
                        return true;
                    case "Null":
                        return true;
                    case "Exit":
                        return false;
                }
                return false;
            }
            void RefreshValues()    //This function updates all settings, especially numerical, every time the user returns to the main menu.
            {
                //Equipment
                equipment.Bag = Proficiency.Configuration.ListHelper(equipment.Bag, equipment.RaceBag, equipment.ClassBag);  //equipment is consolidated into main list.
                equipment.Bag = Equipment.DuplicateItemStacker(equipment.Bag);     //Any duplicates are stacked. If 2 of 'x' exists, it becomes '2 x'.
                
                //Stats
                abilityScores = Stats.AbilityScoreAssigner(stats, racialBonus);     //Ability Score x = Stat x + Racial Bonus x. 
                abilityModifiers = Stats.AbilityModifierAssigner(abilityScores);    //Ability Modifier x assigned by charting system from AbilityModAssigner method, given Ability Score x.

                //For dashboard
                if (stats.Set == true)
                {
                    //If the user has used the 'Stats' menu, the ability scores should be assigned. So long as this is the case, the following conditions are also true.
                    //The dashboard class will use these booleans for it's processes.
                    abilityScores.Set = true;
                    abilityModifiers.Set = true;
                    health.InitiativeSet = true;
                }
                if (health.HitPointsConstantSet == true && abilityModifiers.Set == true && character.RaceSet == true)
                {
                    health.HitPointsSet = true;
                }
                if (character.CClassSet)
                {
                    //If the user has used the 'Background & Class' menu, the following conditions are true.
                    //The dashboard class will use these booleans for it's processes.
                    health.HitPointsConstantSet = true;
                    health.HitDiceSet = true;
                    if (abilityModifiers.Set == true)
                    {
                        health.ArmorClassSet = true;
                    }
                }


                //The following is poorly optimized code, due to being hard-coded. Methods will be written to streamline these in the future.

                //Saving Throws
                savingThrows = Skills.SavingThrowAssigner(savingThrows, abilityModifiers);  //Saving Throw x = Ability Modifier x, +2 if proficient

                if (proficiencies.SavingThrows.Contains("Constitution"))
                {
                    savingThrows.Constitution += proficiencies.ProficiencyBonus;
                }
                if (proficiencies.SavingThrows.Contains("Strength"))
                {
                    savingThrows.Strength += proficiencies.ProficiencyBonus;
                }
                if (proficiencies.SavingThrows.Contains("Dexterity"))
                {
                    savingThrows.Dexterity += proficiencies.ProficiencyBonus;
                }
                if (proficiencies.SavingThrows.Contains("Intelligence"))
                {
                    savingThrows.Intelligence += proficiencies.ProficiencyBonus;
                }
                if (proficiencies.SavingThrows.Contains("Wisdom"))
                {
                    savingThrows.Wisdom += proficiencies.ProficiencyBonus;
                }
                if (proficiencies.SavingThrows.Contains("Charisma"))
                {
                    savingThrows.Charisma += proficiencies.ProficiencyBonus;
                }

                //Groups each skill together by their related ability modifier. Foreach loop will assign each group the value of it's modifier.
                int[][] skillsBuilder = new int[5][];
                skillsBuilder[0] = new int[] { abilityModifiers.Strength, skills.Athletics }; //Strength
                skillsBuilder[1] = new int[] { abilityModifiers.Dexterity, skills.Acrobatics, skills.SleightOfHand, skills.Stealth }; //Dexterity
                skillsBuilder[2] = new int[] { abilityModifiers.Intelligence, skills.Arcana, skills.History, skills.Investigation, skills.Nature, skills.Religion }; //Intelligence
                skillsBuilder[3] = new int[] { abilityModifiers.Wisdom, skills.AnimalHandling, skills.Insight, skills.Medicine, skills.Perception, skills.Survival }; //Wisdom
                skillsBuilder[4] = new int[] { abilityModifiers.Charisma, skills.Deception, skills.Intimidation, skills.Performance, skills.Persuasion }; //Charisma
                foreach (int[] item in skillsBuilder)
                {
                    for (int i = 1; i < item.Length; i++)
                    {
                        item[i] = item[0];
                    }
                }

                //skill x = related modifier + 2 (if proficient)
                skills.Athletics = skillsBuilder[0][0];
                if (proficiencies.Skills.Contains(nameof(skills.Athletics)))
                {
                    skills.Athletics += proficiencies.ProficiencyBonus;
                }
                skills.Acrobatics = skillsBuilder[1][0];
                if (proficiencies.Skills.Contains(nameof(skills.Acrobatics)))
                {
                    skills.Acrobatics += proficiencies.ProficiencyBonus;
                }
                skills.SleightOfHand = skillsBuilder[1][1];
                if (proficiencies.Skills.Contains(nameof(skills.SleightOfHand)))
                {
                    skills.SleightOfHand += proficiencies.ProficiencyBonus;
                }
                skills.Stealth = skillsBuilder[1][2];
                if (proficiencies.Skills.Contains(nameof(skills.Stealth)))
                {
                    skills.Stealth += proficiencies.ProficiencyBonus;
                }
                skills.Arcana = skillsBuilder[2][0];
                if (proficiencies.Skills.Contains(nameof(skills.Arcana)))
                {
                    skills.Arcana += proficiencies.ProficiencyBonus;
                }
                skills.History = skillsBuilder[2][1];
                if (proficiencies.Skills.Contains(nameof(skills.History)))
                {
                    skills.History += proficiencies.ProficiencyBonus;
                }
                skills.Investigation = skillsBuilder[2][2];
                if (proficiencies.Skills.Contains(nameof(skills.Investigation)))
                {
                    skills.Investigation += proficiencies.ProficiencyBonus;
                }
                skills.Nature = skillsBuilder[2][3];
                if (proficiencies.Skills.Contains(nameof(skills.Nature)))
                {
                    skills.Nature += proficiencies.ProficiencyBonus;
                }
                skills.Religion = skillsBuilder[2][4];
                if (proficiencies.Skills.Contains(nameof(skills.Religion)))
                {
                    skills.Religion += proficiencies.ProficiencyBonus;
                }
                skills.AnimalHandling = skillsBuilder[3][0];
                if (proficiencies.Skills.Contains(nameof(skills.AnimalHandling)))
                {
                    skills.AnimalHandling += proficiencies.ProficiencyBonus;
                }
                skills.Insight = skillsBuilder[3][1];
                if (proficiencies.Skills.Contains(nameof(skills.Insight)))
                {
                    skills.Insight += proficiencies.ProficiencyBonus;
                }
                skills.Medicine = skillsBuilder[3][2];
                if (proficiencies.Skills.Contains(nameof(skills.Medicine)))
                {
                    skills.Medicine += proficiencies.ProficiencyBonus;
                }
                skills.Perception = skillsBuilder[3][3];
                if (proficiencies.Skills.Contains(nameof(skills.Perception)))
                {
                    skills.Perception += proficiencies.ProficiencyBonus;
                }
                skills.Survival = skillsBuilder[3][4];
                if (proficiencies.Skills.Contains(nameof(skills.Survival)))
                {
                    skills.Survival += proficiencies.ProficiencyBonus;
                }
                skills.Deception = skillsBuilder[4][0];
                if (proficiencies.Skills.Contains(nameof(skills.Deception)))
                {
                    skills.Deception += proficiencies.ProficiencyBonus;
                }
                skills.Intimidation = skillsBuilder[4][1];
                if (proficiencies.Skills.Contains(nameof(skills.Intimidation)))
                {
                    skills.Intimidation += proficiencies.ProficiencyBonus;
                }
                skills.Performance = skillsBuilder[4][2];
                if (proficiencies.Skills.Contains(nameof(skills.Performance)))
                {
                    skills.Performance += proficiencies.ProficiencyBonus;
                }
                skills.Persuasion = skillsBuilder[4][3];
                if (proficiencies.Skills.Contains(nameof(skills.Persuasion)))
                {
                    skills.Persuasion += proficiencies.ProficiencyBonus;
                }

                //These disabled lists are part of potential strategies for replacing manually-written code with a loop. 
                //Each skill is grouped by the ability modifier it depends on. By default, the skill literally equals the modifier.
                //If the character is proficient in the skill, it's value = ability modifier + proficiency bonus. The bonus is always 2 so long as the application only manipulates level 1 characters.
                //Intended strategy to replace hard-coded instructons:
                //First: Use a loop to compare known proficiencies with list of all proficiencies. If a match occurs, switch the corresponding skills boolean to true, else boolean is false.
                //Second: Use another loop to compare list of all skills with skills boolean. If false, skill = moifier. If true, skill = modifier + proficiency bonus;

                //List<string> allSkillNames = new List<string> { "Athletics",
                //                                                "Acrobatics", "SleightOfHand", "Stealth",
                //                                                "Arcana", "History", "Investigation", "Nature", "Religion",
                //                                                "AnimalHandling", "Insight", "Medicine", "Perception", "Survival",
                //                                                "Deception", "Intimidation", "Performance", "Persuasion" };
                //List<int> allSkills = new List<int> { skills.Athletics,
                //                                      skills.Acrobatics, skills.SleightOfHand, skills.Stealth,
                //                                      skills.Arcana, skills.History, skills.Investigation, skills.Nature, skills.Religion,
                //                                      skills.AnimalHandling, skills.Insight, skills.Medicine, skills.Perception, skills.Survival,
                //                                      skills.Deception, skills.Intimidation, skills.Performance, skills.Persuasion };

                //List<bool> allProficiencies = new List<bool> { skills.AthleticsSet,
                //                                               skills.AcrobaticsSet, skills.SleightOfHandSet, skills.StealthSet,
                //                                               skills.ArcanaSet, skills.HistorySet, skills.InvestigationSet, skills.NatureSet, skills.ReligionSet,
                //                                               skills.AnimalHandlingSet, skills.InsightSet, skills.MedicineSet, skills.PerceptionSet, skills.SurvivalSet,
                //                                               skills.DeceptionSet, skills.IntimidationSet, skills.PerformanceSet, skills.PersuasionSet };

            }
            void ClassListsReset()
            {
                equipment.Bag = new List<string>();             //Reset
                equipment.ClassBag = new List<string>();        //Reset

                languages.Background = new List<string>();      //Reset

                proficiencies.Tools = new List<string>();       //Reset
                proficiencies.ClassTools = new List<string>();  //Reset

                proficiencies.Arms = new List<string>();        //Reset
                proficiencies.ClassArms = new List<string>();   //Reset

                proficiencies.Skills = new List<string>();      //Reset
                proficiencies.ClassSkills = new List<string>(); //Reset
            }
            void RaceListsReset()
            {
                equipment.Bag = new List<string>();                 //Reset
                equipment.RaceBag = new List<string>();             //Reset
                languages.List = new List<string> { "Common" };     //Reset
                languages.Native = "";                              //Reset
                proficiencies.Tools = new List<string>();           //Reset
                proficiencies.RaceTools = new List<string>();       //Reset
                proficiencies.Arms = new List<string>();            //Reset
                proficiencies.RaceArms = new List<string>();        //Reset
                proficiencies.Skills = new List<string>();          //Reset
                proficiencies.RaceSkills = new List<string>();      //Reset
            }
        }
        public static void StartUp() //Introductory text
        {
            //Purpose & scope of application
            //Legal information

            Console.WriteLine("Welcome to Shawn Johnston's Dungeons & Dragons Character Creation Machine. \r\n\r\n" +
                              "This program is intended to facilitate quick-start character creation. \r\n" +
                              "The ruleset being used is for the 5th edition of Dungeons and Dragons. \r\n" +
                              "Playing a different version of the game requires another tool. \r\n\r\n" +

                              "This program uses only open source SRD5.1 content under the Wizard's of the Coast Open Gaming License. \r\n" +
                              "Dungeons and Dragons is owned entirely by Wizards of the Coast. This program is not for sale or profit.\r\n" +
                              "Details can be found here: https://dnd.wizards.com/articles/features/systems-reference-document-srd \r\n\r\n" +

                              "This program is not designed to give comprehensive explanation on how to create DnD characters or play the game. \r\n" +
                              "For any material that is ambiguous, please refer to the basic rules pdf available for free from the creators here: \r\n" +
                              "https://media.wizards.com/2018/dnd/downloads/DnD_BasicRules_2018.pdf \r\n" +
                              "For more character options, please use the official Dungeons & Dragons character creator at http://dndbeyond.com. \r\n\r\n" +

                              "Press enter to continue...");
            Console.ReadLine();
        }
    }
}
