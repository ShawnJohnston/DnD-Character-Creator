using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class CharacterRace
    {
        public bool Set = false;
        public string Race;
        public string SubRace;
        public string Draconic;

        public int Speed;
        public int ExtraProficiency;
        public string NativeLanguage;
        public Stats RacialBonus;
        public List<string> Features;
        public List<string> RaceArmsProficiencies;
        public List<string> RaceSkillsProficiencies;
        public List<string> RaceToolsProficiencies;
        public List<string> Cantrips;


        public CharacterRace(string race, string subRace, string draconic, int speed, int extraProficiency, string nativeLanguage, Stats racialBonus, List<string> features, List<string> raceArms, List<string> raceSkills, List<string> raceTools, List<string> cantrips)
        {
            Race = race;
            SubRace = subRace;
            Draconic = draconic;
            Speed = speed;
            ExtraProficiency = extraProficiency;
            NativeLanguage = nativeLanguage;
            RacialBonus = racialBonus;
            Features = features;
            RaceArmsProficiencies = raceArms;
            RaceSkillsProficiencies = raceSkills;
            RaceToolsProficiencies = raceTools;
            Cantrips = cantrips;
        }
        public string SetRace()
        {
            Console.Clear();
            int choice = 0;
            bool check = true;
            string[] races = { "Dwarf", "Elf", "Halfling", "Human", "Dragonborn", "Gnome", "Half-Elf", "Half-Orc", "Tiefling" };

            while (check == true)  //Loop keeps the user in the menu until a proper selection is made.
            {
                Console.WriteLine("1. Dwarf            +2 Constitution +1 Wisdom    +1 HP per lvl\r\n" +
                                  "                    Artisans Tools: Smith's, Brewer's, Mason's. Advantage & resistance against poison \r\n" +

                             "\r\n2. Elf              +2 Dexterity    +1 Intelligence \r\n" +
                                  "                    1 Wizard cantrip; Proficiency in Perception, 1 extra language \r\n" +
                                  "                    Advantage on saving throws against being charmed or put to sleep by magic \r\n" +

                             "\r\n3. Halfling         +2 Dexterity    +1 Charisma\r\n" +
                                  "                    1 re-roll on a d20 for attack roll, ability check, or saving throw \r\n" +

                             "\r\n4. Human            +1 All; 1 extra language \r\n" +

                             "\r\n5. Dragonborn       +2 Strength     +1 Charisma\r\n" +
                                 "                    Breath weapon/damage resistance based on draconic heritage \r\n" +

                             "\r\n6. Gnome            +2 Intelligence +1 Constitution\r\n" +
                                 "                    Advantage on all Int, Wis, or Cha saving throws against magic \r\n" +

                             "\r\n7. Half-Elf         +2 Charisma     +1 Any +1 Any \r\n" +
                                 "                    Proficiency in 2 skills; 1 extra language \r\n" +
                                 "                    Advantage on saving throws against being charmed or put to sleep by magic \r\n" +

                             "\r\n8. Half-Orc         +2 Strength     +1 Constitution \r\n" +
                                 "                    Proficiency in Intimidation \r\n" +
                                 "                    endure a strike and only drop to 1HP; roll an extra weapon damange dice on critical hits \r\n" +

                             "\r\n9. Tiefling         +2 Charisma     +1 Intelligence \r\n" +
                                 "                    Fire Resistance; Thaumaturgy cantrip");
                Console.Write("Select a race: ");
                try
                {
                    choice = int.Parse(Console.ReadLine()) - 1; //User enters a number corresponding to the race they want to play. The number is adjusted to match the array index.
                    check = false;
                }
                catch (SystemException)     //The user gives some input that isnt an integer. The console clears and the loop starts over.
                {
                    Console.Clear();
                }
            }
            try
            {
                return races[choice];
            }
            catch (IndexOutOfRangeException)    //If the user enters an illegal integer, a selection will be made at random. This is a temporary solution, as deciding on the user's behalf is inappropriate.
            {
                Console.Clear();
                Console.WriteLine("Improper selection. \r\n" +
                                  "Press enter to try again.");
                Console.ReadLine();
                return "";
            }
        }
        public static Stats Conditionals(CharacterBasic character, CharacterRace race, Stats racialBonus, Equipment equipment, Languages languages, Proficiency proficiencies, Features features)
        {
            //Controls details of character race choice. Subraces, ability scores, Languages based on race. Subrace functionality technically not necessary due to licensing restrictions. Included in case of software repurposing.
            race = Controller(character.Race);
            character.SubRace = race.SubRace;
            character.Draconic = race.Draconic;
            character.Speed = race.Speed;
            racialBonus = race.RacialBonus;
            languages.Native = race.NativeLanguage;
            proficiencies.RaceTools = race.RaceToolsProficiencies;
            proficiencies.RaceArms = race.RaceArmsProficiencies;
            proficiencies.RaceSkills = race.RaceSkillsProficiencies;
            features.Race = race.Features;
            proficiencies.Tools = Proficiency.Configuration.ListHelper(proficiencies.Tools, proficiencies.RaceTools, proficiencies.ClassTools);
            proficiencies.Arms = Proficiency.Configuration.ListHelper(proficiencies.Arms, proficiencies.RaceArms, proficiencies.ClassArms);
            proficiencies.Skills = Proficiency.Configuration.ListHelper(proficiencies.Skills, proficiencies.RaceSkills, proficiencies.ClassSkills);
            return racialBonus;
        }
        public static CharacterRace Controller(string race)     //Retrieves the settings corresponding to the set race. Sends back to Program Main.
        {
            CharacterRace raceSettings = new CharacterRace(
                          "", "", "", 0, 0,
                          nativeLanguage: "",
                          racialBonus: new Stats(0, 0, 0, 0, 0, 0),
                          features: new List<string> { "" },
                          raceArms: new List<string> { "" },
                          raceSkills: new List<string> { "" },
                          raceTools: new List<string> { "" },
                          cantrips: new List<string> { "" });

            Console.Clear();
            switch (race)
            {
                case "Dragonborn":
                    Dragonborn(raceSettings);
                    break;
                case "Dwarf":
                    Dwarf(raceSettings);
                    break;
                case "Elf":
                    Elf(raceSettings);
                    break;
                case "Halfling":
                    Halfling(raceSettings);
                    break;
                case "Human":
                    Human(raceSettings);
                    break;
                case "Gnome":
                    Gnome(raceSettings);
                    break;
                case "Half-Elf":
                    HalfElf(raceSettings);
                    break;
                case "Half-Orc":
                    HalfOrc(raceSettings);
                    break;
                case "Tiefling":
                    Tiefling(raceSettings);
                    break;
            }
            return raceSettings;
        }
        public static CharacterRace Dragonborn(CharacterRace raceSettings)
        {
            string[] draconicHeritage = { "Black", "Blue", "Brass", "Bronze", "Copper", "Gold", "Green", "Red", "Silver", "White" };

            raceSettings.Race = "Dragonborn";
            raceSettings.SubRace = "";
            raceSettings.Draconic = Builder.Selection("\r\nSelect a draconic heritage: \r\n", draconicHeritage, draconicHeritage);
            raceSettings.RacialBonus = new Stats(0, 2, 0, 0, 0, 1);
            raceSettings.Speed = 30;
            raceSettings.NativeLanguage = "Draconic";
            raceSettings.Features = new List<string> { "Draconic Ancestry", "Breath Weapon", "Damage Resistance" };
            raceSettings.RaceSkillsProficiencies = new List<string>();
            raceSettings.RaceToolsProficiencies = new List<string>();
            return raceSettings;
        }
        public static CharacterRace Dwarf(CharacterRace raceSettings)
        {
            string[] toolOptions = { "Smith's Tools", "Brewer's Tools", "Mason's Tools" };
            raceSettings.Race = "Dwarf";
            raceSettings.SubRace = "Hill";
            raceSettings.Draconic = "";
            raceSettings.RacialBonus = new Stats(2, 0, 0, 0, 0, 0);
            raceSettings.Speed = 25;
            raceSettings.NativeLanguage = "Dwarven";
            raceSettings.Features = new List<string> { "Darkvision", "Dwarven Resilience", "Dwarven Combat Training", "Stonecutting" };
            raceSettings.RaceArmsProficiencies = new List<string> { "Battleaxe", "Handaxe", "Light Hammer", "Warhammer" };
            raceSettings.RaceToolsProficiencies.Add(Builder.ItemToList("Select an artisan's tool to gain proficiency in: ", "", toolOptions, toolOptions));
            if (raceSettings.SubRace == "Hill")
            {
                raceSettings.RacialBonus.Wisdom = 1;
                raceSettings.Features.Add("Dwarven Toughness");

            }
            return raceSettings;
        }
        public static CharacterRace Elf(CharacterRace raceSettings)
        {
            raceSettings.Race = "Dwarf";
            raceSettings.SubRace = "High";
            raceSettings.Draconic = "";
            raceSettings.Speed = 30;
            raceSettings.RacialBonus = new Stats(0, 0, 2, 0, 0, 0);
            raceSettings.NativeLanguage = "Elvish";
            raceSettings.Features = new List<string> { "Darkvision", "Keen Senses", "Fey Ancestry", "Trance" };
            raceSettings.RaceSkillsProficiencies = new List<string> { "Perception" };
            if (raceSettings.SubRace == "High")
            {
                raceSettings.RacialBonus.Intelligence = 1;
                raceSettings.Features.Add("Elf Weapon Training");
                raceSettings.RaceArmsProficiencies = new List<string> { "Longsword", "Shortsword", "Shortbow", "Longbow" };
                Console.Clear();
                Wizard elf = new Wizard();
                string cantripToList = Builder.ItemToList("Select a wizard cantrip: ", "", elf.SpellList(0), elf.SpellList(0));
                raceSettings.Cantrips.Add(cantripToList);
                raceSettings.Features.Add(cantripToList);
                
            }
            return raceSettings;
        }
        public static CharacterRace Halfling(CharacterRace raceSettings)
        {
            raceSettings.Race = "Halfling";
            raceSettings.SubRace = "Lightfoot";
            raceSettings.Draconic = "";
            raceSettings.Speed = 25;
            raceSettings.RacialBonus = new Stats(0, 0, 2, 0, 0, 0);
            raceSettings.NativeLanguage = "Halfling";
            raceSettings.Features = new List<string> { "Lucky", "Brave", "Halfling Nimbleness" };
            if (raceSettings.SubRace == "Lightfoot")
            {
                raceSettings.RacialBonus.Charisma = 1;
                raceSettings.Features.Add("Naturally Stealthy");
            }
            return raceSettings;
        }
        public static CharacterRace Human(CharacterRace raceSettings)
        {
            string[] languageChoice = { "Dwarven", "Elvish", "Giant", "Gnomish", "Goblin", "Halfling", "Orc", "Abyssal", "Celestial", "Draconic", "Deep-Speech", "Infernal", "Primordial", "Sylvan", "Undercommon" };

            raceSettings.Race = "Human";
            raceSettings.SubRace = "";
            raceSettings.Draconic = "";
            raceSettings.Speed = 30;
            raceSettings.RacialBonus = new Stats(1, 1, 1, 1, 1, 1);
            raceSettings.Features = new List<string> { };

            return raceSettings;
        }
        public static CharacterRace Gnome(CharacterRace raceSettings)
        {
            raceSettings.Race = "Gnome";
            raceSettings.SubRace = "Rock";
            raceSettings.Draconic = "";
            raceSettings.Speed = 25;
            raceSettings.RacialBonus = new Stats(0, 0, 0, 2, 0, 0);

            raceSettings.NativeLanguage = "Gnomish";
            raceSettings.Features = new List<string> { "Darkvision", "Gnome Cunning" };
            if (raceSettings.SubRace == "Rock")
            {
                raceSettings.RacialBonus.Constitution = 1;
                raceSettings.Features.Add("Artificer's Lore");
                raceSettings.Features.Add("Tinker");
                raceSettings.RaceToolsProficiencies.Add("Tinker's Tools");
            }
            return raceSettings;
        }
        public static CharacterRace HalfElf(CharacterRace raceSettings)
        {
            Console.Clear();
            string[] statBonus = { "Constitution", "Strength", "Dexterity", "Intelligence", "Wisdom", "Charisma" };

            raceSettings.Race = "Half-Elf";
            raceSettings.SubRace = "";
            raceSettings.Draconic = "";
            raceSettings.Speed = 30;
            raceSettings.RacialBonus = new Stats(0, 0, 0, 0, 0, 2);
            raceSettings.NativeLanguage = "Elvish";
            raceSettings.Features = new List<string> { "Darkvision", "Fey Ancestry", "Skill Versatility" };

            Console.WriteLine("Half-Elves recieve 2 bonus values to apply to any desired Abillity Score");
            for (int i = 0; i < 2; i++)
            {
                string statToBonus = Builder.ItemToList("\r\nAssign 2 bonus values \r\n", "", statBonus, statBonus);
                switch (statToBonus)
                {
                    case "Constitution":
                        raceSettings.RacialBonus.Constitution += 1;
                        break;
                    case "Strength":
                        raceSettings.RacialBonus.Strength += 1;
                        break;
                    case "Dexterity":
                        raceSettings.RacialBonus.Dexterity += 1;
                        break;
                    case "Intelligence":
                        raceSettings.RacialBonus.Intelligence += 1;
                        break;
                    case "Wisdom":
                        raceSettings.RacialBonus.Wisdom += 1;
                        break;
                    case "Charisma":
                        raceSettings.RacialBonus.Charisma += 1;
                        break;
                }

            }
            return raceSettings;
        }
        public static CharacterRace HalfOrc(CharacterRace raceSettings)
        {
            raceSettings.Race = "Half-Orc";
            raceSettings.SubRace = "";
            raceSettings.Draconic = "";
            raceSettings.Speed = 30;
            raceSettings.RacialBonus = new Stats(1, 2, 0, 0, 0, 0);
            raceSettings.NativeLanguage = "Orc";
            raceSettings.Features = new List<string> { "Darkvision", "Menacing", "Relentless Endurance", "Savage Attacks" };
            raceSettings.RaceSkillsProficiencies = new List<string> { "Intimidation" };
            return raceSettings;
        }
        public static CharacterRace Tiefling(CharacterRace raceSettings)
        {
            raceSettings.Race = "Tiefling";
            raceSettings.SubRace = "";
            raceSettings.Draconic = "";
            raceSettings.Speed = 30;
            raceSettings.RacialBonus = new Stats(0, 0, 0, 1, 0, 2);
            raceSettings.NativeLanguage = "Infernal";
            raceSettings.Features = new List<string> { "Darkvision", "Hellish Resistance", "Infernal Legacy" };
            raceSettings.Cantrips = new List<string> { "Thaumaturgy" };
            return raceSettings;
        }
    }
}
