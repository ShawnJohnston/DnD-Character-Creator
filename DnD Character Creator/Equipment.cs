using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_Character_Creator
{
    public class Equipment
    {
        public List<string> Bag { get; set; }
        public List<string> RaceBag { get; set; }
        public List<string> ClassBag { get; set; }

        public Equipment(List<string> bag, List<string> raceBag, List<string> classBag)
        {
            Bag = bag;
            RaceBag = raceBag;
            ClassBag = classBag;
        }

        public static List<String> Controller(string CClass, CharacterBasic character, Stats abilityModifiers, Features features)
        {
            List<string> equipment = new List<string>();
            switch (CClass)
            {
                case "Cleric":
                    equipment = Cleric(equipment, character, abilityModifiers, features);
                    break;
                case "Fighter":
                    equipment = Fighter(equipment, character, abilityModifiers, features);
                    break;
                case "Rogue":
                    equipment = Rogue(equipment, character, abilityModifiers, features);
                    break;
                case "Wizard":
                    equipment = Wizard(equipment, character, abilityModifiers, features);
                    break;
            }
            return equipment;
        }
        public static List<string> Cleric(List<string> equipment, CharacterBasic character, Stats abilityModifiers, Features features)
        {
            //For all of this code, a new equipment list is created, and the user will select items from a series of prompts. 
            //First, the user selects weapons and armor.
            //Second, the user selects a bag containing adventuring gear.
            equipment = new List<string> { "Shield", "Holy Symbol" };   //All Clerics will start with these items.
            if (character.Race == "Dwarf")  //Dwarfs have weapon proficiency with warhammers.
            {
                string[] startingInitialWeapon = { "Mace", "Warhammer" };
                equipment.Add(Builder.ItemToList("Select a starting weapon: ", "", startingInitialWeapon, startingInitialWeapon));
            }
            else    //Mace will be selected by default if the character doesn't have warhammer proficiency. There's no reason to let the user pick a first weapon they can't use.
            {
                equipment.Add("Mace");
            }
            string firstWeapon = equipment.Last();
            string[] startingArmor = { "Scale Mail       AC: 13 + Dex modifier (max 2)", //Chain Mail is only an option if the character is proficient with heavy armor. Because SRD only allows the Cleric to choose Life Domain anyway (which grants Heavy Armor Proficiency, the option is included here for simplicity.
                                       "Leather Armor    AC: 11 + Dex modifier",
                                       "Chain Mail       AC: 16"};
            string[] startingArmorSelection = { "Scale Mail", "Leather Armor", "Chain Mail"};

            equipment.Add(Builder.ItemToList("Select a starting armor: ", "", startingArmor, startingArmorSelection));

            string[] startingWeapons = { "Club                    1d4 bludgeoning, light",    //User can choose a crossbow or any simple weapon
                                         "Dagger                  1d4 piercing, finesse, light, thrown (range 20/60)",
                                         "Greatclub               1d8 bludgeoning, Two-handed",
                                         "Handaxe                 1d6 slashing, Light, thrown (range 20/60)",
                                         "Javelin                 1d6 piercing, Thrown (range 30/120)",
                                         "Light Hammer            1d4 bludgeoning, Light, thrown (range 20/60)",
                                         "Mace                    1d6 bludgeoning",
                                         "Quarterstaff            1d6 bludgeoning, Versatile (1d8)",
                                         "Sickle                  1d4 slashing, Light",
                                         "Spear                  1d6 piercing, Thrown (range 20/60), versatile (1d8)",
                                         "Crossbow, 20 bolts     1d8 piercing, Ammunition (range 80/320), loading, two-handed",
                                         "Dart                   1d4 piercing, Finesse, thrown (range 20/60)",
                                         "Shortbow               1d6 piercing, Ammunition (range 80/320),two-handed",
                                         "Sling                  1d4 bludgeoning, Ammunition (30/120)" };
            string[] startingWeaponsSelection = { "Club", "Dagger", "Greatclub", "Handaxe", "Javelin", "Light Hammer", "Mace", "Quarterstaff", "Sickle", "Spear", "Crossbow w/ 20 bolts", "Dart", "Shortbow", "Sling" }; //User can choose a crossbow or any simple weapon

            equipment.Add(Builder.ItemToList($"Your Cleric has a {firstWeapon}. \r\nSelect a second weapon: ", "", startingWeapons, startingWeaponsSelection));

            string[] equipmentPack = { "Priest’s Pack: \r\n" +
                                       "   backpack, blanket, 10 candles, tinderbox, alms box, 2 blocks of incense, censer, vestments, 2 days rations, waterskin",
                                       "Explorer’s Pack: \r\n" +
                                       "   backpack, bedroll, mess kit, tinderbox, 10 torches, 10 days rations, waterskin, 50 ft of hempen rope" };
            string[] equipmentPackSelection = { "Priest's Pack", "Explorer's Pack" };
            equipment.Add(Builder.ItemToList("Select an equipment pack: ", "", equipmentPack, equipmentPackSelection));
            return equipment;
        }
        public static List<string> Fighter(List<string> equipment, CharacterBasic character, Stats abilityModifiers, Features features)
        {
            //For all of this code, a new equipment list is created, and the user will select items from a series of prompts. 
            //First, the user chooses a fighting style, which adds certain numerical benefits or proficiencies.
            //Second, the user selects weapons and armor.
            //Third, the user selects a bag containing adventuring gear.
            equipment = new List<string> { };
            string[] fightingStylesOption = { "Archery", "Defense", "Dueling", "Great Weapon Fighting", "Protection", "Two Weapon Fighting" };
            for (int i = 0; i < 1; i++)
            {
                string fightingStyle = $"Fighting Style: {Builder.ItemToList("Select a fighting style: ", "", fightingStylesOption, fightingStylesOption)}";
                features.Class[0] = fightingStyle;
            }
            string[] startingArmor = {"Leather Armor, Longbow w/ 20 arrows         AC: 11 + Dex modifier",
                                      "Chain Mail                                  AC: 16" };
            string[] startingArmorSelection = { "Leather Armor  Longbow w/ 20 arrows", "Chain Mail" };
            switch (Builder.ItemToList("Select starting armor: ", "", startingArmor, startingArmorSelection))
            {
                case "Leather Armor  Longbow w/ 20 arrows":
                    equipment.Add("Leather Armor");
                    equipment.Add("Longbow w/ 20 arrows");
                    break;
                case "Chain Mail":
                    equipment.Add("Chain Mail");
                    break;
            }
            Console.Clear();
            string[] martialWeapons = { " Battleaxe     1d8 slashing, Versatile (1d10)",
                                        " Flail         1d8 bludgeoning",
                                        " Glaive        1d10 slashing, Heavy, reach, two-handed",
                                        " Greataxe      1d12 slashing, Heavy, two-handed",
                                        " Greatsword    2d6 slashing, Heavy, two-handed",
                                        " Halberd       1d10 slashing, Heavy, reach, two-handed",
                                        " Longsword     1d8 slashing, Verrsatile",
                                        " Lance         1d12 piercing, Reach, special",
                                        " Maul          2d6 bludgeoning, Heavy, two-handed",
                                        "Morningstar   1d8 piercing",
                                        "Pike          1d6 piercing,	Heavy, reach, two-handed",
                                        "Rapier        1d6 piercing, Finesse",
                                        "Scimitar      1d6 slashing, Finesse, light",
                                        "Shortsword    1d4 piercing, Finesse, light",
                                        "Trident       1d8 piercing, Thrown (range 20/60), versatile (1d8)",
                                        "War pick      1d8 piercing",
                                        "Warhammer     1d8 bludgeoning, Versatile (1d10)",
                                        "Whip          1d4 slashing, Finesse, reach" };
            string[] martialWeaponsSelected = { "Battleaxe", "Flail", "Glaive", "Greataxe", "Greatsword", "Halberd", "Lance", "Longsword", "Maul", "Morningstar", "Pike", "Rapier", "Scimitar", "Shortsword", "Trident", "War pick", "Warhammer", "Whip" };
            string[] armsChoice = { "Weapon and Shield", "2 Weapons" };
            switch (Builder.ItemToList("You can choose between a martial weapon w/ shield or 2 martial weapons. A shield adds +2 to AC when equipped.", "", armsChoice, armsChoice))
            {
                case "Weapon and Shield":
                    equipment.Add("Shield");
                    equipment.Add(Builder.ItemToList("Select a martial weapon: ", "", martialWeapons, martialWeaponsSelected));
                    break;
                case "2 Weapons":
                    for (int i = 0; i < 2; i++)
                    {
                        Console.Clear();
                        equipment.Add(Builder.ItemToList($"Select 2 martial weapons. \r\nSelection {i + 1}:", "", martialWeapons, martialWeaponsSelected));
                    }
                    break;
            }
            string[] armsChoice2 = { "Light Crossbow w/ 20 bolts", "2 Handaxe" };
            equipment.Add(Builder.ItemToList("Select between a Crossbow or 2 Handaxes: ", "", armsChoice2, armsChoice2));
            string[] equipmentPack = { "Dungeoneer’s Pack: \r\n" +
                                       "   backpack, crowbar, hammer, 10 pitons, 10 torches, tinderbox, 10 day's rations, waterskin. 50 feet of hempen rope",
                                       "Explorer’s Pack: \r\n" +
                                       "   backpack, bedroll, mess kit, tinderbox, 10 torches, 10 day's rations, waterskin, 50 ft of hempen rope" };
            string[] equipmentPackSelection = { "Dungeoneer's Pack", "Explorer's Pack" };
            equipment.Add(Builder.ItemToList("Select an equipment pack: ", "", equipmentPack, equipmentPackSelection));
            Console.Clear();
            return equipment;
        }
        public static List<string> Rogue(List<string> equipment, CharacterBasic character, Stats abilityModifiers, Features features)
        {
            //For all of this code, a new equipment list is created, and the user will select items from a series of prompts. 
            //First, the user selects weapons and armor.
            //Second, the user selects a bag containing adventuring gear.

            equipment = new List<string> { "Leather Armor", "2 Daggers", "Thieves' Tools" };

            string[] armsChoice = { "Rapier         1d6 piercing, Finesse",
                                    "Shortsword     1d4 piercing, Finesse, light" };
            string[] armsChoiceSelection = { "Rapier", "Shortsword" };
            equipment.Add(Builder.ItemToList("Selection one of the 2: ", "", armsChoice, armsChoiceSelection));
            string[] armsChoice2 = { "Shortbow & Quiver  1d6 piercing, Ammunition (range 80/320),two-handed\r\n" +
                                     "  w/ 20 arrows",
                                     "Shortsword         1d4 piercing, Finesse, light" };
            string[] armsChoiceSelection2 = { "Shortbow & Quiver w/ 20 arrows", "Shortsword" };
            switch (Builder.ItemToList("Selection one of the 2: ", "", armsChoice2, armsChoiceSelection2))
            {
                case "Shortbow & Quiver w/ 20 arrows":
                    equipment.Add("Shortbow & Quiver");
                    equipment.Add("20 arrows");
                    break;
                case "Shortsword":
                    equipment.Add("Shortsword");
                    break;
            }

            string[] equipmentPack = { "Dungeoneer’s Pack: \r\n" +
                                       "   backpack, crowbar, hammer, 10 pitons, 10 torches, tinderbox, 10 day's rations, waterskin. 50 feet of hempen rope",
                                       "Explorer’s Pack: \r\n" +
                                       "   backpack, bedroll, mess kit, tinderbox, 10 torches, 10 day's rations, waterskin, 50 ft of hempen rope" };
            string[] equipmentPackSelection = { "Dungeoneer's Pack", "Explorer's Pack" }; //Website error, verify elsewhere
            equipment.Add(Builder.ItemToList("Select an equipment pack: ", "", equipmentPack, equipmentPackSelection));
            return equipment;
        }
        public static List<string> Wizard(List<string> equipment, CharacterBasic character, Stats abilityModifiers, Features features)
        {
            //For all of this code, a new equipment list is created, and the user will select items from a series of prompts. 
            //First, the user chooses a medium to channel magic through. 
            //Second, the user selects a weapon.
            //Third, the user selects a bag containing adventuring gear.

            equipment = new List<string> { "Spellbook" };   //All Wizards have a spell book.
            string[] magicMedium = { "Component Pouch\r\n" +
                                     "      A small, watertight leather belt pouch that has compartments to hold all the material components and other \r\n" +
                                     "      special items you need to cast your spells, except for those components that have a specific cost \r\n" +
                                     "      (as indicated in a spell's description).",
                                     "Arcane Focus\r\n" +
                                     "      A special item— an orb, a crystal, a rod, a specially constructed staff, a wand-­‐like length of wood, or \r\n" +
                                     "      some similar item— designed to channel the power of arcane spells.\r\n" };
            string[] magicMediumSelection = { "Component Pouch", "Arcane Focus" };
            string medium = Builder.ItemToList("Select a magic item: \r\n", "", magicMedium, magicMediumSelection);
            if (medium == "Arcane Focus")
            {
                string[] focusOptions = { "Orb", "Crystal", "Rod", "Staff", "Wooden Wand", "Other" };
                string[] focusSelection = { "Orb", "Crystal", "Rod", "Staff", "Wooden Wand", "" };
                medium = Builder.ItemToList("Select a magic item: \r\n", "", focusOptions, focusSelection);
            }
            equipment.Add(medium);

            string[] armsChoice = { "Quarterstaff  1d6 bludgeoning, Versatile (1d8)",
                                    "Dagger        1d4 piercing, finesse, light, thrown (range 20/60)" };
            string[] armsChoiceSelection = { "Quarterstaff", "Dagger" };
            equipment.Add(Builder.ItemToList("Select a weapon: \r\n", "", armsChoice, armsChoiceSelection));

            string[] equipmentPack = { "Scholar’s Pack: \r\n" +
                                       "   backpack, book of lore, bottle of ink, ink pen, 10 sheets of parchment, little bag of sand, small knife.",
                                       "Explorer’s Pack: \r\n" +
                                       "   backpack, bedroll, mess kit, tinderbox, 10 torches, 10 day's rations, waterskin, 50 ft of hempen rope" };
            string[] equipmentPackSelection = { "Scholar's Pack", "Explorer's Pack" };
            equipment.Add(Builder.ItemToList("Select an equipment pack: ", "", equipmentPack, equipmentPackSelection));
            return equipment;
        }
        public static List<string> DuplicateItemStacker(List<string> list)
        {
            //This method counts the times any item 'x' appears in a list. The items are stacked into a single index and replaced with "{count} x" to indicate multiples of the same item.
            List<string> compiledList = new List<string>();
            foreach (string item in list)
            {
                int itemCount = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (item == list[i])
                    {
                        itemCount++;
                    }
                }
                if (itemCount == 1)
                {
                    compiledList.Add(item);
                }
                else
                {
                    compiledList.Add($"{itemCount} {item}");
                }
            }
            list = new List<string>(compiledList.Distinct());
            return list;
        }
    }
}
