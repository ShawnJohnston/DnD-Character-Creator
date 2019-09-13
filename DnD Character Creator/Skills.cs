using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Skills
    {
        public List<string> SkillList = new List<string> { };

        public int Options;
        public int Athletics { get; set; }
        public int Acrobatics { get; set; }
        public int SleightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Arcana { get; set; }
        public int History { get; set; }
        public int Investigation { get; set; }
        public int Nature { get; set; }
        public int Religion { get; set; }
        public int AnimalHandling { get; set; }
        public int Insight { get; set; }
        public int Medicine { get; set; }
        public int Perception { get; set; }
        public int Survival { get; set; }
        public int Deception { get; set; }
        public int Intimidation { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }

        public bool Set = false;
        public bool AthleticsSet = false;
        public bool AcrobaticsSet = false;
        public bool SleightOfHandSet = false;
        public bool StealthSet = false;
        public bool ArcanaSet = false;
        public bool HistorySet = false;
        public bool InvestigationSet = false;
        public bool NatureSet = false;
        public bool ReligionSet = false;
        public bool AnimalHandlingSet = false;
        public bool InsightSet = false;
        public bool MedicineSet = false;
        public bool PerceptionSet = false;
        public bool SurvivalSet = false;
        public bool DeceptionSet = false;
        public bool IntimidationSet = false;
        public bool PerformanceSet = false;
        public bool PersuasionSet = false;

        public Skills(int athletics = 0, int acrobatics = 0, int sleightOfHand = 0, int stealth = 0, int arcana = 0, int history = 0, int investigation = 0, int nature = 0, int religion = 0,
                      int animalHandling = 0, int insight = 0, int medicine = 0, int perception = 0, int survival = 0, int deception = 0, int intimidation = 0, int performance = 0, int persuasion = 0)
        {
            Athletics = athletics;
            Acrobatics = acrobatics;
            SleightOfHand = sleightOfHand;
            Stealth = stealth;
            Arcana = arcana;
            History = history;
            Investigation = investigation;
            Nature = nature;
            Religion = religion;
            AnimalHandling = animalHandling;
            Insight = insight;
            Medicine = medicine;
            Perception = perception;
            Survival = survival;
            Deception = deception;
            Intimidation = intimidation;
            Performance = performance;
            Persuasion = persuasion;
        }

        public static Stats SavingThrowAssigner(Stats savingThrows, Stats mods)
        {
            int[] savingThrowsArray = { savingThrows.Constitution, savingThrows.Strength, savingThrows.Dexterity, savingThrows.Intelligence, savingThrows.Wisdom, savingThrows.Charisma };
            int[] modsArray = { mods.Constitution, mods.Strength, mods.Dexterity, mods.Intelligence, mods.Wisdom, mods.Charisma };
            for (int i = 0; i < savingThrowsArray.Length; i++)
            {
                savingThrowsArray[i] = modsArray[i];
            }
            Stats assignedSavingthrows = new Stats(savingThrowsArray[0], savingThrowsArray[1], savingThrowsArray[2], savingThrowsArray[3], savingThrowsArray[4], savingThrowsArray[5]);
            return assignedSavingthrows;
        }
    }
}
