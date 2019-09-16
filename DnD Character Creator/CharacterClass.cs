using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class CharacterClass
    {
        public bool Set = false;
        public string SubClass { get; set; }
        public int HitPointsConstant { get; set; }
        public string HitDice { get; set; }
        public List<string> ClassArmsProficiencies { get; set; }
        public List<string> ClassToolsProficiencies { get; set; }
        public List<string> SavingThrows { get; set; }
        public string[] ClassSkillProficiencies { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Features { get; set; }
        public string SetBackground()
        {
            string[] background = { "None", "Acolyte" };
            string backgroundOption = "";
            while (backgroundOption == "")
            {
                backgroundOption = Builder.Selection("Choose your character background: \r\n*Note: The only background available in D&D SRD is Acolyte. \r\n\r\n" +
                                                        "Acolyte: \r\n" +
                                                        "Skill Proficiencies: Insight, Religion \r\n" +
                                                        "Language Proficiencies: +2 Any\r\n" +
                                                        "Equipment: Holy Symbol\r\n" +
                                                        "Feature: Shelter of the Faithful \r\n", background, background);
            }
            return backgroundOption;
        }
        public string SetClass()
        {
            string CClass = "";
            string[] basicClass = { "Cleric", "Fighter", "Rogue", "Wizard" };
            while (CClass == "")
            {
                CClass = Builder.Selection("Choose your character class: ", basicClass, basicClass);
            }
            return CClass;
        }
        public CharacterClass(string subClass = "", int hitPointsConstant = 0, string hitDice = "", List<string> classArms = null, List<string> classTools = null, List<string> savingThrows = null, string[] classSkills = null, List<string> features = null)
        {
            SubClass = subClass;
            HitPointsConstant = hitPointsConstant;
            HitDice = hitDice;
            ClassArmsProficiencies = classArms;
            ClassToolsProficiencies = classTools;
            SavingThrows = savingThrows;
            ClassSkillProficiencies = classSkills;
            Features = features;
        }
        public static CharacterClass Controller(string CClass)  //Gets settings corresponding to set character class. Sends back to Program Main.
        {
            List<string> classFeatures = new List<string>();
            CharacterClass characterClass = new CharacterClass();   //Empty object, will be overwritten by information based on set character class.
            switch (CClass) 
            {
                case "Cleric":
                    characterClass = Cleric(characterClass);
                    break;
                case "Fighter":
                    characterClass = Fighter(characterClass);
                    break;
                case "Rogue":
                    characterClass = Rogue(characterClass);
                    break;
                case "Wizard":
                    characterClass = Wizard(characterClass);
                    break;
            }
            return characterClass;
        }
        public static CharacterClass Cleric(CharacterClass classObject)
        {
            classObject = new CharacterClass(
                          "Life", 8, "1d8", //Subclass, HP Constant, Hit Dice
                          new List<string> { "Light Armor", "Medium Armor", "Simple Weapons" }, //Class Arms
                          new List<string> { "" }, //Class Tools
                          new List<string> { "Wisdom", "Charisma" }, //Saving Throws
                          new string[] { "History", "Insight", "Medicine", "Persuasion", "Religion" },   //Class Skills
                          new List<string> ());

            return classObject;
        }
        public static CharacterClass Fighter(CharacterClass classObject)
        {
            classObject = new CharacterClass(
                          "Champion", 10, "1d10", //Subclass, HP Constant, Hit Dice
                          new List<string> { "Light Armor", "Medium Armor", "Heavy Armor", "Shields", "Simple Weapons", "Martial Weapons" }, //Class Arms
                          new List<string> { "" }, //Class Tools
                          new List<string> { "Strength", "Constitution" }, //Saving Throws
                          new string[] { "Acrobatics", "Animal Handling", "Athletics", "History", "Insight", "Intimidation", "Perception", "Survival" },
                          new List<string> { "Fighting Style: ", "Second Wind" });   //Class Features
            return classObject;
        }
        public static CharacterClass Rogue(CharacterClass classObject)
        {
            classObject = new CharacterClass(
                          "Thief", 8, "1d8", //Subclass, HP Constant, Hit Dice
                          new List<string> { "Light Armor", "Medium Armor", "Simple Weapons", "Hand-Crossbows", "Longswords", "Shortswords", "Rapiers" }, //Class Arms
                          new List<string> { "Thieves Tools" }, //Class Tools
                          new List<string> { "Dexterity", "Intelligence" }, //Saving Throws
                          new string[] { "Acrobatics", "Athletics", "Deception", "Insight", "Intimidation", "Investigation", //Class Skills
                                         "Perception", "Performance", "Persuasion", "Sleight of Hand", "Stealth" },
                          new List<string> { "Expertise", "Sneak Attack", "Thieves' Cant" });    //Class Features
            return classObject;
        }
        public static CharacterClass Wizard(CharacterClass classObject)
        {
            classObject = new CharacterClass(
                          "Evocation", 6, "1d6", //Subclass, HP Constant, Hit Dice
                          new List<string> { "Daggers", "Darts", "Slings", "Quarterstaffs", "Light-Crossbows" }, //Class Arms
                          new List<string> { "" }, //Class Tools
                          new List<string> { "Intelligence", "Wisdom" }, //Saving Throws
                          new string[] { "Arcana", "History", "Insight", "Investigation", "Medicine", "Religion" },  //Class Skills
                          new List<string>());
            return classObject;
        }

        
        public static void Conditionals(CharacterBasic character, Health health, Stats abilityModifiers, Equipment equipment, Languages languages, Proficiency proficiencies, CharacterClass characterClass, Features features, SpellBook spellCasting)
        {
            //Collects all relevant objects from Program Main and overwrites their properties from corresponding objects from a 'Character Class' class determined by the controller.

            Console.Clear();
            characterClass = Controller(character.CClass);  //Gets character class properties
            character.SubClass = characterClass.SubClass;
            health.HitPointsConstant = characterClass.HitPointsConstant;
            health.HitDice = characterClass.HitDice;
            proficiencies.ClassArms = characterClass.ClassArmsProficiencies;
            proficiencies.SavingThrows = characterClass.SavingThrows;
            features.Class = characterClass.Features;

            equipment.ClassBag = Equipment.Controller(character.CClass, character, abilityModifiers, features);     //Gets the equipment setting/options. Determined by character class.
            equipment.Bag = Proficiency.Configuration.ListHelper(equipment.Bag, equipment.RaceBag, equipment.ClassBag);                                 //Consolidates equipment into a main list.
            proficiencies.Tools = Proficiency.Configuration.ListHelper(proficiencies.Tools, proficiencies.RaceTools, proficiencies.ClassTools);         //Consolidates tools into a main list.
            proficiencies.Arms = Proficiency.Configuration.ListHelper(proficiencies.Arms, proficiencies.RaceArms, proficiencies.ClassArms);             //Consolidates armaments into a main list.
            proficiencies.Skills = Proficiency.Configuration.ListHelper(proficiencies.Skills, proficiencies.RaceSkills, proficiencies.ClassSkills);     //Consolidates skills into a main list.


            if (characterClass.SubClass == "Life") //Heavy armor proficiency is granted by Life Domain.
            {
                Cleric cleric = new Cleric();
                cleric.DomainSpells = new List<string> { "Bless", "Cure Wounds" };
                cleric.Cantrips = cleric.SelectSpells(cleric, 0, abilityModifiers);
                foreach (string cantrip in cleric.Cantrips)
                {
                    features.Class.Add(cantrip);
                }
                features.Class.Add("Bless");
                features.Class.Add("Cure Wounds");
                features.Class.Add("Disciple of Life");

                characterClass.ClassArmsProficiencies.Add("Heavy Armor");
            }
            if (characterClass.SubClass == "Evocation") //Heavy armor proficiency is granted by Life Domain.
            {
                Wizard wizard = new Wizard();
                wizard.Cantrips = wizard.SelectSpells(wizard, 0, abilityModifiers);
                wizard.SpellBook = wizard.FillSpellBook(wizard);
                foreach (string cantrip in wizard.Cantrips)
                {
                    features.Class.Add(cantrip);
                }
                foreach (string spell in wizard.SpellBook)
                {
                    features.Class.Add(spell);
                }
                features.Class.Add("Arcane Recovery");
            }
            Console.Clear();
            if (character.Background == "Acolyte")
            {
                equipment.Bag.Add("Holy Symbol");
                features.Background.Add(BackgroundFeatures.Acolyte());
            }
        }
    }
}
