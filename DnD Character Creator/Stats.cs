using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Stats
    {
        public bool Set = false;
        public int Constitution { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int Initiative { get; set; }

        public Stats(int constitution = 0, int strength = 0, int dexterity = 0, int intelligence = 0, int wisdom = 0, int charisma = 0)
        {
            Constitution = constitution;
            Strength = strength;
            Dexterity = dexterity;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;
        }

        public static Stats Menu(CharacterBasic character, Stats stats, Stats racialBonus)
        {
            Console.Clear();
            stats = new Stats(); //Reset
            List<string> emptyList = new List<string>();    //Buffer list
            List<string> statNames = new List<string> { "Constitution", "Strength", "Dexterity", "Intelligence", "Wisdom", "Charisma" };
            string statSelection = "";
            List<int> unassignedScores = new List<int>();

            unassignedScores = StatProcessor(unassignedScores); //Gets numbers for user to assign
            Builder.DataLine("\r\nStat results: ", emptyList, unassignedScores);
            Console.Write("\r\nPress enter to continue...");
            Console.ReadLine();

            for (int i = 0; i < 6; i++) //This loop displays user stat choices
            {
                    switch (Builder.Selection($"Stat Pool: {unassignedScores[0]} {unassignedScores[1]} {unassignedScores[2]} {unassignedScores[3]} {unassignedScores[4]} {unassignedScores[5]}\r\n\r\n" +
                                          "Optimal stat arrangement: \r\n" +
                                          "Cleric: Wisdom     Fighter: Strength     Rogue: Dexterity     Wizard: Intelligence \r\n\r\n" +
                                          "Ability Modifier Table: \r\n" +
                                          "Ability Score:      0    2    4    6    8    10    12    14    16    18    20    22    24    26    28    30\r\n" +
                                          "Ability Modifier:  -5   -4   -3   -2   -1    0      1     2     3     4     5     6     7     8     9    10\r\n\r\n" +
                                          $"Racial Bonus ({character.Race}):   Con: {racialBonus.Constitution} Str: {racialBonus.Strength} Dex: {racialBonus.Dexterity} Int: {racialBonus.Intelligence} Wis: {racialBonus.Wisdom} Cha: {racialBonus.Charisma} \r\n\r\n" +
                                          $"Ability Scores: Con: {stats.Constitution} Str: {stats.Strength} Dex: {stats.Dexterity} Int: {stats.Intelligence} Wis: {stats.Wisdom} Cha: {stats.Charisma} \r\n" +
                                          "\r\nAssign: " + unassignedScores[i], statNames.ToArray(), statNames.ToArray()))
                    {
                        case "Constitution":
                            stats.Constitution = unassignedScores[i];
                            statNames.Remove("Constitution");
                            break;
                        case "Strength":
                            stats.Strength = unassignedScores[i];
                            statNames.Remove("Strength");
                            break;
                        case "Dexterity":
                            stats.Dexterity = unassignedScores[i];
                            statNames.Remove("Dexterity");
                            break;
                        case "Intelligence":
                            stats.Intelligence = unassignedScores[i];
                            statNames.Remove("Intelligence");
                            break;
                        case "Wisdom":
                            stats.Wisdom = unassignedScores[i];
                            statNames.Remove("Wisdom");
                            break;
                        case "Charisma":
                            stats.Charisma = unassignedScores[i];
                            statNames.Remove("Charisma");
                            break;
                        case "":
                            i--;
                            break;
                }
            }
            return stats;
        }
        public static List<int> StatProcessor(List<int> processedValues) //User decides how stat values will be determined. Standard Array is most balanced, Rolling is risky and statistically subpar. Point buy allows optimization. Point buy not yet implemented.
        {
            processedValues.Clear();
            string[] statMode = { "Standard Array", "Roll" };
            string statSelection = "";
            while (statSelection == "")
            {
                switch (statSelection = Builder.Selection("How is your character Ability scores being determined? \r\n\r\n" +
                                                      "Standard array gives you fixed numbers to assign wherever you choose. Best for balance. \r\n" +
                                                      "15, 14, 13, 12, 10, 9\r\n\r\n" +
                                                      "Rolling for stats gives you 6 random numbers, each determined by rolling 4 D6 and dropping the lowest. Risky but fun.\r\n\r\n", statMode, statMode))
                {
                    case "Standard Array":
                        processedValues = new List<int> { 15, 14, 13, 12, 10, 9 };
                        break;

                    case "Roll":    //4 random numbers are rolled and sorted. Top 3 numbers added together, then added to the return array. This all occurs 6 times.
                        processedValues = DiceRoll.RollForStats();
                        break;
                    //case "Point Buy":
                    //    break;
                    case "":
                        break;
                }
            }
            return processedValues;
        }
        public static Stats AbilityScoreAssigner(Stats stats, Stats racialBonus)
        {
            int[] statsArray = { stats.Constitution, stats.Strength, stats.Dexterity, stats.Intelligence, stats.Wisdom, stats.Charisma };
            int[] racialBonusArray = { racialBonus.Constitution, racialBonus.Strength, racialBonus.Dexterity, racialBonus.Intelligence, racialBonus.Wisdom, racialBonus.Charisma };
            int[] compilerArray = new int[6];
            for (int i = 0; i < statsArray.Length; i++)
            {
                compilerArray[i] = statsArray[i] + racialBonusArray[i];
            }
            Stats assignedScores = new Stats(compilerArray[0], compilerArray[1], compilerArray[2], compilerArray[3], compilerArray[4], compilerArray[5]);
            return assignedScores;
        }
        public static Stats AbilityModifierAssigner(Stats scores)
        {
            //This is a temporary solution. A math formula will replace this eventually. There are several possible solutions to reduce this method to a uniform formula within Algebra, Trigonometry, or Calculus.
            //Line x1 and x2 have the same slope and therefore parallel. y increases by 1 every even number. A line in between x1 and x2 can be estableshed, x3. f(x1) = (x-10)/2. f(x2) = (x-11)/2. f(x3) = x-10.5)/2.
            int[] scoresArray = { scores.Constitution, scores.Strength, scores.Dexterity, scores.Intelligence, scores.Wisdom, scores.Charisma };
            int[] compilerArray = new int[6];
            int[] tableX1 = { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 };  //Even scores
            int[] tableX2 = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31 };  //Odd scores
            int[] tableY = { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };        //Modifier values

            for (int i = 0; i < scoresArray.Length; i++)    //For loop spans across each scores value
            {
                compilerArray[i] = ConfigureByTable(scoresArray[i], tableX1, tableX2, tableY); //Each score is sent to the table method to retrieve it's respective modifier
            }
            Stats assignedModifiers = new Stats(compilerArray[0], compilerArray[1], compilerArray[2], compilerArray[3], compilerArray[4], compilerArray[5]); //Modifiers are added to a new object
            return assignedModifiers;
        }
        public static int ConfigureByTable(int score, int[] tableX1, int[] tableX2, int[] tableY)
        {
            //Ability modifiers are assigned directly from a table.
            int result = 0;
            for (int i = 0; i < tableY.Length; i++)
            {
                if (score == tableX1[i])
                {
                    result = tableY[i];
                }
                else if (score == tableX2[i])
                {
                    result = tableY[i];
                }
            }
            score = result;
            return score;
        }
    }
}
