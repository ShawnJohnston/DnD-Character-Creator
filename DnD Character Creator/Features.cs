using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Features
    {
        public List<string> Race;
        public List<string> Class;
        public List<string> Background;
        public Features(List<string> race, List<string> cClass, List<string> background)
        {
            Race = race;
            Class = cClass;
            Background = background;
        }
        public static void RaceFeatures()
        {
            //Dragonborn
            //Breath Weapon
        }

        public string ListFlatten(List<string> list)
        {
            int count = list.Count;
            string flatList = "";
            for (int i = 0; i < count; i++)
            {
                flatList = (flatList + ", " + list[i]);
            }
            return flatList;
        }
    }
    public class BackgroundFeatures
    {
        public static string Acolyte()
        {
            string acolyteFeature = "Shelter of the Faithful";
            return acolyteFeature;
        }
    }
    public class SpellBook
    {
        public List<string> Cantrips { get; set; }
        public List<string> PreparedSpells { get; set; }
        public List<string> Lvl1SpellSlots { get; set; }
        public int SpellSaveDC { get; set; }
        public int SpellAttackModifier { get; set; }
    }
    public class Cleric : SpellBook
    {
        //Spell Save DC = 8 + your proficiency bonus + your Wisdom modifier
        //Spell Attack Modifier = your proficiency bonus + your Wisdom modifier
        public List<string> DomainSpells { get; set; }

        public string[] SpellList(int lvl)
        {
            string[][] spellsLvls = new string[2][];
            spellsLvls[0] = new string[] { "Guidance", "Light", "Mending", "Resistance", "Sacred Flame", "Spare the Dying", "Thaumaturgy" };    //Cleric Cantrips

            spellsLvls[1] = new string[] { "Bane", "Bless", "Command", "Create or Destroy Water", "Cure Wounds", "Detect Evil and Good", "Detecct Magic", "Detect Poision and Disease", // Level 1 Cleric spells
                                               "Guiding Bolt", "Healing Word", "Inflict Wounds", "Purify Food and Drink", "Sanctuary", "Shield of Faith" };
            return spellsLvls[lvl];
        }
        public List<string> SelectSpells(Cleric cleric, int spellLvl, Stats abilityModifiers)
        {
            //Select prepared spells. Clerics can change their spells daily, however this is included for quick-start setup.
            int count;
            string prompt;
            if (spellLvl == 0)  //Clerics start with 3 cantrips at level 1
            {
                count = 3;
                prompt = "Cantrips are 0 level spells with unlimited uses. Unlike prepared spells, they are permanent choices once gaming starts. \r\n" +
                         "Select 3 cantrips: ";
            }
            else    //Prepared spells for Cleric = Wisdom modifier + Cleric level
             {
                count = abilityModifiers.Wisdom + 1;
                prompt = "Clerics prepare a list of spells daily. The amount of spells available is determined by their Wisdom modifier + Cleric level. \r\n" +
                         "Character level: 1 \r\n" +
                        $"Wisdom Modifier: {abilityModifiers.Wisdom} \r\n" +
                        $"Select {count} spells: \r\n";
            }
            List<string> spellList = new List<string>();
            string[] spellArray = cleric.SpellList(spellLvl);
            for (int i = 0; i < count; i++)
            {
                spellList.Add(Builder.Selection(prompt, spellArray, spellArray));
                spellArray = Proficiency.Configuration.ProficiencyListUpdate(spellList, spellArray);
            }
            return spellList;
        }

    }
    public class Wizard : SpellBook
    {
        public List<string> SpellBook { get; set; }
        //Spell Save DC = 8 + your proficiency bonus + your Intelligence modifier
        //Spell Attack Modifier = your proficiency bonus + your Intelligence modifier
        public string[] SpellList(int lvl)
        {
            string[][] spellsLvls = new string[2][];
            spellsLvls[0] = new string[]{"Acid Splash", "Chill Touch", "Dancing Lights", "Fire Bolt", "Light", "Mage Hand", "Mending",  //Wizard Cantrips
                                             "Message", "Minor Illusion", "Poison Spray", "Prestidigitation", "Ray of Frost", "Shocking Grasp", "True Strike" };

            spellsLvls[1] = new string[]{ "Alarm", "Burning Hands", "Charm Person", "Color Spray", "Comprehend Languages","Detect Magic","Disguise Self",   //Level 1 Wizard spells
                                              "Expeditious Retreat","False Life","Feather Fall", "Find Familiar", "Floating Disk", "Fog Cloud","Grease",
                                              "Hideous Laughter","Identify","Illusory Script","Jump","Longstrider","Mage Armor","Magic Missile",
                                              "Protection from Evil and Good","Shield","Silent Image","Sleep","Thunderwave","Unseen Servant" };
            return spellsLvls[lvl];
        }
        public List<string> FillSpellBook(Wizard wizard)
        {
            List<string> spellList = new List<string>();
            string[] spellArray = wizard.SpellList(1);
            for (int i = 0; i < 6; i++)
            {
                spellList.Add(Builder.Selection("Wizards start at level 1 with 6 spells in their spell book. \r\nSelect your spells: ", spellArray, spellArray));
                spellArray = Proficiency.Configuration.ProficiencyListUpdate(spellList, spellArray);
            }
            return spellList;
        }
        public  List<string> SelectSpells(Wizard wizard, int spellLvl, Stats abilityModifiers)  //Select prepared spells. Wizards can change their spells daily.
        {
            //Select prepared spells.
            int count;
            string prompt;
            if (spellLvl == 0)  //Wizards start with 3 cantrips at level 1
            {
                count = 3;
                prompt = "Cantrips are 0 level spells with unlimited uses. Unlike prepared spells, they are permanent choices once gaming starts. \r\n" +
                         "Select 3 cantrips: ";
            }
            else    //Prepared spells for Wizard = Intelligence Modifier + Wizard level
            {
                count = abilityModifiers.Intelligence + 1;
                prompt = "Wizards prepare a list of spells daily from their spell book. The amount of spells available is determined by their Intelligence modifier + Wizard level. \r\n" +
                         "Character level: 1 \r\n" +
                        $"Intelligence Modifier: {abilityModifiers.Intelligence} \r\n" +
                        $"Select {count} spells: \r\n";
            }
            List<string> spellList = new List<string>();
            string[] spellArray = wizard.SpellList(spellLvl);
            for (int i = 0; i < count; i++)
            {
                spellList.Add(Builder.Selection(prompt, spellArray, spellArray));
                spellArray = Proficiency.Configuration.ProficiencyListUpdate(spellList, spellArray);
            }
            return spellList;
        }
    }
}