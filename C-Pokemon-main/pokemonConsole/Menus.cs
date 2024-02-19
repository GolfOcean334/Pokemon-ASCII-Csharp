using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pokemonConsole
{
    internal class Menu_principal
    {
        private static int size;

        private static string pokemonMenuIcon = "  POKEMON";
        private static string itemsMenuIcon = "  OBJETS";
        private static string playerMenuIcon = "  ";
        private static string saveMenuIcon = "  SAUVER";
        private static string optionMenuIcon = "  OPTIONS";
        private static string retourMenuIcon = "  RETOUR";

        private static List<string> icons = new List<string>();

        private static string hautMenu = "0-----------0";
        private static string middleMenu = "|           |";

        private static bool alreadySelected;

        private static int position;


        private static int endPositionXText;
        private static int endPositionYText;

        public static void Open(Player player, int mapWidth, Rival rival)
        {
            if (player.pokemonParty.Count != 0)
            {
                icons.Add(pokemonMenuIcon);
            }
            if (playerMenuIcon == "  ") playerMenuIcon += player.name;


            icons.Add(itemsMenuIcon);
            icons.Add(playerMenuIcon);
            icons.Add(saveMenuIcon);
            icons.Add(optionMenuIcon);
            icons.Add(retourMenuIcon);

            size = icons.Count;

            alreadySelected = false;
            foreach(string icon in icons)
            {
                if (icon[0] == '>')
                {
                    alreadySelected = true;
                }
            }

            if (!alreadySelected)
            {
                icons[0] = icons[0].Remove(0, 1);
                icons[0] = icons[0].Insert(0, ">");
                position = 0;
            }

            PrintMenu(mapWidth);

            ConsoleKeyInfo keyInfo;
            bool choiceDone = false;
            while (!choiceDone)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (position != size - 1)
                        {
                            ChangeSelected(position, position +1);
                            PrintMenu(mapWidth);
                            position++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (position != 0)
                        {
                            ChangeSelected(position, position - 1);
                            PrintMenu(mapWidth);
                            position--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (icons[position] == "> POKEMON")
                        {
                            MenuPokemon.Open(player);
                            Map.DrawMap();
                            PrintMenu(mapWidth);
                        }
                        else if (icons[position] == "> OBJETS")
                        {
                            MenuItems.Open(player);
                            Map.DrawMap();
                            PrintMenu(mapWidth);
                        }
                        else if (icons[position] == "> " + player.name)
                        {

                        }
                        else if (icons[position] == "> SAUVER")
                        {
                            Save.Saving(player, rival);
                            choiceDone = true;
                        }
                        else if (icons[position] == "> RETOUR")
                        {
                            choiceDone = true;
                        }

                        break;
                    case ConsoleKey.Escape:
                        choiceDone = true;
                        break;
                    default:
                        break;
                }
            }
            icons.Clear();
        }

        private static void ChangeSelected(int position, int nextPosition)
        {
            icons[position] = icons[position].Remove(0, 1);
            icons[position] = icons[position].Insert(0, " ");

            icons[nextPosition] = icons[nextPosition].Remove(0, 1);
            icons[nextPosition] = icons[nextPosition].Insert(0, ">");
        }

        private static void PrintMenu(int mapWidth)
        {
            Console.SetCursorPosition(mapWidth - 3, 1);
            Console.Write(hautMenu);
            for (int i = 2; i < size + 2; i++)
            {
                Console.SetCursorPosition(mapWidth - 3, i);
                Console.Write(middleMenu);

                Console.SetCursorPosition(mapWidth - 3 + 2, i);
                Console.Write(icons[i - 2]);
            }
            Console.SetCursorPosition(mapWidth - 3, size + 2);
            Console.Write(hautMenu);

            endPositionXText = Console.CursorLeft;
            endPositionYText = Console.CursorTop;
        }
    }

    class MenuPokemon
    {
        private static string pokemonLine1 = "  ";
        private static string pokemonLine2 = "   ";

        private static string littleMenuStats = "  STATS";
        private static string littleMenuItemUse = "  USE";
        private static string littleMenuOrdre = "  ORDRE";
        private static string littleMenuRetour = "  RETOUR";

        private static bool isChangingOnTop;

        

        private static List<string> pokemonLines1 = new List<string>();
        private static List<string> pokemonLines2 = new List<string>();
        private static List<string> littleMenuLines = new List<string>();
        private static List<string> littleAttackLine = new List<string>();


        private static bool alreadySelected;
        private static bool alreadySelectedLittle;
        private static int position;
        private static int positionLittle;
        private static int positionAttack;
        private static int endPositionXText;
        private static int endPositionYText;

        private static int PositionChangement;

        private static bool itemUsed = false;

        public static void Open(Player player, bool isCombat = false, int itemType = 0) // itemType = 0 : none, = 1 : heal & autres, = 2 : capacity
        {
            for (int i = 0; i < player.pokemonParty.Count; i++)
            {
                Pokemon pokemon = player.pokemonParty[i];


                pokemonLines1.Add(pokemonLine1);
                pokemonLines1[i] += pokemon.name;

                for (int j = pokemon.name.Length; j <= 10; j++)
                {
                    pokemonLines1[i] += " ";
                }
                pokemonLines1[i] += "N" + pokemon.level;


                pokemonLines2.Add(pokemonLine2);
                int pvPerSix = pokemon.pvLeft * 6 / pokemon.pv;
                string barPv = "PV";
                for (int j = 0; j < pvPerSix; j++)
                {
                    barPv += "=";
                }
                for (int j = barPv.Length; j < 8; j++)
                {
                    barPv += "*";
                }
                pokemonLines2[i] += barPv;

                pokemonLines2[i] += "  ";
                for (int j = 3; j > pokemon.pvLeft.ToString().Length ; j--)
                {
                    pokemonLines2[i] += " ";
                }
                pokemonLines2[i] += pokemon.pvLeft + "/";
                for (int j = 3; j > pokemon.pv.ToString().Length; j--)
                {
                    pokemonLines2[i] += " ";
                }
                pokemonLines2[i] += pokemon.pv;
            }
            
            

            alreadySelected = false;
            foreach (string pokemon in pokemonLines1)
            {
                if (pokemon[0] == '>')
                {
                    alreadySelected = true;
                }
            }

            if (!alreadySelected)
            {
                pokemonLines1[0] = pokemonLines1[0].Remove(0, 1);
                pokemonLines1[0] = pokemonLines1[0].Insert(0, ">");
                position = 0;
            }

            PrintMenu(false);


            bool isSwitching = false;
            bool retour = false;
            ConsoleKeyInfo keyInfo;

            while (!retour)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (position != pokemonLines1.Count - 1)
                        {
                            ChangeSelected(position, position + 1, pokemonLines1);
                            PrintMenu(isSwitching);
                            position++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (position != 0)
                        {
                            ChangeSelected(position, position - 1, pokemonLines1);
                            PrintMenu(isSwitching);
                            position--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (!isSwitching) OpenLittleMenu(ref isSwitching, player, isCombat, itemType);
                        if (itemUsed)
                        {
                            itemUsed = false;
                            retour = true;
                        }
                        
                        if (isCombat && isSwitching)
                        {
                            Pokemon temp = player.pokemonParty[position];
                            player.pokemonParty[position] = player.pokemonParty[0];
                            player.pokemonParty[0] = temp;


                            retour = true;
                        }

                        if(isSwitching && !isCombat)
                        {
                            if (PositionChangement != position && !isCombat)
                            {
                                Pokemon temp = player.pokemonParty[position];
                                player.pokemonParty[position] = player.pokemonParty[PositionChangement];
                                player.pokemonParty[PositionChangement] = temp;

                                string tempStr = pokemonLines1[position];
                                string tempStr2 = pokemonLines2[position];
                                pokemonLines1[position] = pokemonLines1[PositionChangement];
                                pokemonLines2[position] = pokemonLines2[PositionChangement];
                                pokemonLines1[PositionChangement] = tempStr;
                                pokemonLines2[PositionChangement] = tempStr2;

                                ChangeSelected(PositionChangement, position, pokemonLines1);


                                isChangingOnTop = false;
                                isSwitching = false;
                            }
                        }

                        PrintMenu(isSwitching);
                        break;
                    case ConsoleKey.Escape:
                        retour = true;
                        break;
                }
            }
            pokemonLines1.Clear();
            pokemonLines2.Clear();
        }
        private static void OpenLittleMenu(ref bool isSwitching, Player player, bool isCombat, int itemType)
        {
            if(itemType == 0)
            {
                littleMenuLines.Add(littleMenuStats);
                littleMenuLines.Add(littleMenuOrdre);
            }
            else if(itemType > 0)
            {
                littleMenuLines.Add(littleMenuItemUse);
            }
            littleMenuLines.Add(littleMenuRetour);

            alreadySelectedLittle = false;
            foreach (string button in littleMenuLines)
            {
                if (button[0] == '>')
                {
                    alreadySelectedLittle = true;
                }
            }

            if (!alreadySelectedLittle)
            {
                littleMenuLines[0] = littleMenuLines[0].Remove(0, 1);
                littleMenuLines[0] = littleMenuLines[0].Insert(0, ">");
                positionLittle = 0;
            }

            PrintLittleMenu();

            bool retour = false;
            ConsoleKeyInfo keyInfo;

            while (!retour)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (positionLittle != littleMenuLines.Count - 1)
                        {
                            ChangeSelected(positionLittle, positionLittle + 1, littleMenuLines);
                            PrintLittleMenu();
                            positionLittle++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (positionLittle != 0)
                        {
                            ChangeSelected(positionLittle, positionLittle - 1, littleMenuLines);
                            PrintLittleMenu();
                            positionLittle--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (positionLittle == 0)
                        {
                            if (itemType == 0)
                            {
                                player.pokemonParty[position].AfficherDetailsMenu();
                                retour = true;
                            }
                            else if (itemType == 1)
                            {
                                Item.UseItem(player.inventory[MenuItems.position], ref player, position, 0, isCombat);
                                itemUsed = true;
                                retour = true;
                            }
                            else if (itemType == 2)
                            {
                                OpenAttackMenu(player.pokemonParty[position], player, isCombat);
                                itemUsed = true;
                                retour = true;
                            }
                        }   
                        else if (positionLittle == 1)
                        {
                            if (!isCombat || (isCombat && player.pokemonParty[position].pvLeft > 0) && itemType == 0)
                            {

                                PositionChangement = position;
                                isChangingOnTop = true;
                                isSwitching = true;
                                retour = true;
                            }
                            else
                            {
                                retour = true;
                            }

                        }
                        else if (positionLittle == 2) retour = true;
                        break;
                    case ConsoleKey.Escape:
                        retour = true;
                        break;
                }
            }
            littleMenuLines.Clear();
        }

        private static void OpenAttackMenu(Pokemon pokemon, Player player, bool isCombat)
        {
            foreach (Capacity cap in pokemon.listAttackActual)
            {
                littleAttackLine.Add("  " + cap.name);
            }

            alreadySelectedLittle = false;
            foreach (string button in littleAttackLine)
            {
                if (button[0] == '>')
                {
                    alreadySelectedLittle = true;
                }
            }

            if (!alreadySelectedLittle)
            {
                littleAttackLine[0] = littleAttackLine[0].Remove(0, 1);
                littleAttackLine[0] = littleAttackLine[0].Insert(0, ">");
                positionAttack = 0;
            }

            PrintAttackMenu(pokemon);

            bool retour = false;
            ConsoleKeyInfo keyInfo;

            while (!retour)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (positionAttack != littleAttackLine.Count - 1)
                        {
                            ChangeSelected(positionAttack, positionAttack + 1, littleAttackLine);
                            PrintAttackMenu(pokemon);
                            positionAttack++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (positionAttack != 0)
                        {
                            ChangeSelected(positionAttack, positionAttack - 1, littleAttackLine);
                            PrintAttackMenu(pokemon);
                            positionAttack--;
                        }
                        break;
                    case ConsoleKey.Enter:
                        Item.UseItem(player.inventory[MenuItems.position], ref player, position, positionAttack, isCombat);
                        retour = true;
                        break;
                    case ConsoleKey.Escape:
                        retour = true;
                        break;
                }
            }
            littleAttackLine.Clear();
        }

        private static void ChangeSelected(int position, int nextPosition, List<string> list)
        {
            if (list[position][0] != '}')
            {
                list[position] = list[position].Remove(0, 1);
                list[position] = list[position].Insert(0, " ");
            }
            if (isChangingOnTop)
            {
                list[position] = list[position].Remove(0, 1);
                list[position] = list[position].Insert(0, "}");
            }

            if (list[nextPosition][0] == '}') isChangingOnTop = true;
            else isChangingOnTop = false;
            list[nextPosition] = list[nextPosition].Remove(0, 1);
            list[nextPosition] = list[nextPosition].Insert(0, ">");
        }

        private static void PrintMenu(bool isSwitching)
        {
            Console.Clear();
            for (int i = 0; i < pokemonLines1.Count; i++)
            {
                Console.WriteLine(pokemonLines1[i]);
                foreach (char c in pokemonLines2[i])
                {
                    if (c == '=')
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(c);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (c == '*')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write('=');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }

            for (int i = pokemonLines1.Count;i < 6; i++)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            string topBox =    "0-------------------0";
            string middleBox = "|                   |";

            Console.WriteLine(topBox);
            int tempPositionXText = Console.CursorLeft;
            int tempPositionYText = Console.CursorTop;
            Console.WriteLine(middleBox);
            Console.WriteLine(middleBox);
            Console.WriteLine(topBox);
            int tempPositionXText2 = Console.CursorLeft;
            int tempPositionYText2 = Console.CursorTop;

            Console.SetCursorPosition(tempPositionXText + 2, tempPositionYText);


            string text = "";
            if (!isSwitching) text = "Selectionnez un POKEMON.";
            else if (isSwitching) text = "Nouvelle position du POKEMON...";

            int charRestant = topBox.Length-4;
            int lineWriting = 1;
            string[] words = text.Split(" ");
            int wordIndex = 0;


            for (int i = 0; i < text.Length; i++)
            {

                if (charRestant > 0 && lineWriting == 1)
                {
                    Console.Write(text[i]);
                    charRestant--;

                    // Si le caractère actuel est un espace, passe au mot suivant
                    if (text[i] == ' ' || (charRestant == 0 && text[i +1] == ' '))
                    {
                        wordIndex++;

                        // Vérifie si le mot suivant peut s'insérer dans la ligne actuelle
                        if (wordIndex < words.Length && words[wordIndex].Length > charRestant)
                        {
                            charRestant = topBox.Length - 4;
                            lineWriting++;
                            Console.SetCursorPosition(tempPositionXText + 2, tempPositionYText+1);
                        }
                    }
                }
                else if (charRestant > 0 && lineWriting == 2)
                {
                    if (!(charRestant == topBox.Length - 4 && text[i] == ' '))
                    {
                        Console.Write(text[i]);
                    }

                    charRestant--;
                }
            }

            Console.SetCursorPosition(tempPositionXText2, tempPositionYText2);
            endPositionXText = Console.CursorLeft;
            endPositionYText = Console.CursorTop;
        }

        private static void PrintLittleMenu()
        {
            string hautBox = "0---------0";
            string middleBox = "|         |";

            Console.SetCursorPosition(endPositionXText + 10, endPositionYText - 5);
            Console.Write(hautBox);
            Console.SetCursorPosition(endPositionXText + 10, endPositionYText - 4);
            for (int i = littleMenuLines.Count; i >= 1; i--)
            {
                Console.Write(middleBox); 
                Console.SetCursorPosition(endPositionXText + 10, endPositionYText - i);
            }
            Console.Write(hautBox);

            for (int i = 0; i < littleMenuLines.Count; i++)
            {
                Console.SetCursorPosition(endPositionXText + 12, endPositionYText - 4 + i);
                Console.Write(littleMenuLines[i]);
            }

            Console.SetCursorPosition(endPositionXText, endPositionYText);
        }
        private static void PrintAttackMenu(Pokemon pokemon)
        {
            string hautBox =   "0-----------------0";
            string middleBox = "|                 |";

            Console.SetCursorPosition(endPositionXText + 2, endPositionYText - 5);
            Console.Write(hautBox);
            Console.SetCursorPosition(endPositionXText + 2, endPositionYText - 4);
            for (int i = 4; i >= 1; i--)
            {
                Console.Write(middleBox);
                Console.SetCursorPosition(endPositionXText + 2, endPositionYText - i);
            }
            Console.Write(hautBox);

            for (int i = 0; i < littleAttackLine.Count; i++)
            {
                Console.SetCursorPosition(endPositionXText + 4, endPositionYText - 4 + i);
                Console.Write(littleAttackLine[i]);
            }

            Console.SetCursorPosition(endPositionXText, endPositionYText);
        }
    }

    class MenuItems
    {
        private static string topBox = "0---------------0";
        private static string middleBox = "|               |";

        private static string topLittleBox =    "0----------0";
        private static string middleLittleBox = "|          |";

        private static string topNumberBox = "0----0";
        private static string middleNumberBox = "|    |";

        private static string retourButton = "  RETOUR";

        private static string tossButton = "  JETER";
        private static string useButton = "  UTILISER";

        public static int position;
        private static int visualPosition;
        private static int positionLittle;

        private static List<string> listItem = new List<string>();
        private static List<int> listItemQuantity = new List<int>();

        private static List<string> listLittleMenu = new List<string>();

        private static bool choiceMain;


        public static void Open(Player player, bool isCombat = false, Pokemon pokemonAdverse = null, Pokemon pokemon = null)
        {
            
            foreach (Item item in player.inventory)
            {
                listItem.Add("  " + item.name);
                listItemQuantity.Add(item.quantity);
            }
            listItem.Add(retourButton);

            bool alreadySelected = false;
            foreach (string item in listItem)
            {
                if (item[0] == '>')
                {
                    alreadySelected = true;
                }
            }

            if (!alreadySelected && listItem.Count != 0)
            {
                listItem[0] = listItem[0].Remove(0, 1);
                listItem[0] = listItem[0].Insert(0, ">");
                position = 0;
                visualPosition = 0;
            }

            PrintInventory();

            ConsoleKeyInfo keyInfo;
            choiceMain = false;
            while (!choiceMain)
            {
                keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (position < listItem.Count - 1)
                        {
                            if(visualPosition == 4)
                            {
                                ChangeSelected(1);
                                PrintItems(position - visualPosition);
                            }
                            else
                            {
                                ChangeSelected(1);
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if(position != 0)
                        {
                            if (visualPosition == 0)
                            {
                                ChangeSelected(-1);
                                PrintItems(position - visualPosition);
                            }
                            else
                            {
                                ChangeSelected(-1);
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (position == listItem.Count - 1)
                        {
                            choiceMain = true;
                        }
                        else
                        {
                            OpenLittleMenu(player, pokemonAdverse, pokemon, isCombat);

                            listItem.Clear();
                            listItemQuantity.Clear();

                            choiceMain = false;
                            foreach(Item item in player.inventory)
                            {
                                listItem.Add("  " + item.name);
                                listItemQuantity.Add(item.quantity);
                            }
                            listItem.Add(retourButton);

                            listItem[position] = listItem[position].Remove(0, 1);
                            listItem[position] = listItem[position].Insert(0, ">");

                            if (position > visualPosition)
                            {
                                PrintInventory(position - visualPosition);
                            }
                            else
                            {
                                PrintInventory();
                            }
                        }
                        break;
                }
            }
            listItem.Clear();
            listItemQuantity.Clear();
            choiceMain = false;
        }

        private static void OpenLittleMenu(Player player, Pokemon pokemonAdverse, Pokemon pokemon, bool isCombat)
        {
            listLittleMenu.Add(useButton);
            listLittleMenu.Add(tossButton);

            bool alreadySelected = false;
            foreach (string item in listLittleMenu)
            {
                if (item[0] == '>')
                {
                    alreadySelected = true;
                }
            }

            if (!alreadySelected)
            {
                listLittleMenu[0] = listLittleMenu[0].Remove(0, 1);
                listLittleMenu[0] = listLittleMenu[0].Insert(0, ">");
                positionLittle = 0;
            }

            PrintLittleMenu();

            ConsoleKeyInfo key;

            bool choiceDone = false;
            while (!choiceDone) 
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        if(positionLittle < listLittleMenu.Count - 1)
                        {
                            ChangeSelectedLittle(+1);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (positionLittle > 0)
                        {
                            ChangeSelectedLittle(-1);
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (listLittleMenu[positionLittle] == "> UTILISER")
                        {
                            if (player.inventory[position].effectAtk == Item.effect.CatchPokemon)
                            {
                                Item.UseItem(player.inventory[position], ref player, 0, 0, true, pokemonAdverse, pokemon);
                                choiceMain = true;
                            }
                            else if (player.inventory[position].effectAtk == Item.effect.TenPPOneCap || player.inventory[position].effectAtk == Item.effect.AllPPOneCap || player.inventory[position].effectAtk == Item.effect.PPPlus)
                            {
                                MenuPokemon.Open(player, isCombat, 2);
                                choiceMain = true;
                            }
                            else
                            {
                                MenuPokemon.Open(player, isCombat, 1);
                                choiceMain = true;
                            }
                        }
                        else if (listLittleMenu[positionLittle] == "> JETER")
                        {

                        }
                        choiceDone = true;
                        break;
                    case ConsoleKey.Escape:
                        choiceDone = true;
                        break;
                }
            }
            
        }
        private static void OpenNumberMenu(Player player)
        {
            if (position > visualPosition)
            {
                PrintInventory(position - visualPosition);
            }
            else
            {
                PrintInventory();
            }




        }


        private static void PrintInventory(int position = 0)
        {
            if (position < 0) position = 0;

            Console.Clear();
            Console.WriteLine(topBox);
            for (int i = 0; i < 10; i++) Console.WriteLine(middleBox);
            Console.WriteLine(topBox);

            PrintItems(position);
        }
        private static void PrintLittleMenu()
        {
            int offsetLeft = 11;
            int offsetTop = 8;

            Console.SetCursorPosition(offsetLeft, offsetTop);
            Console.Write(topLittleBox);
            for (int i = 1; i <= 2; i++)
            {
                Console.SetCursorPosition(offsetLeft, offsetTop + i);
                Console.Write(middleLittleBox);
            }
            Console.SetCursorPosition(offsetLeft, offsetTop + 3);
            Console.Write(topLittleBox);

            for (int i = 1; i <= 2; i++)
            {
                Console.SetCursorPosition(offsetLeft + 1, offsetTop + i);
                Console.Write(listLittleMenu[i-1]);
            }

        }
        private static void PrintNumberMenu()
        {

        }
        private static void ChangeSelected(int change)
        {
            listItem[position] = listItem[position].Remove(0, 1);
            listItem[position] = listItem[position].Insert(0, " ");

            Console.SetCursorPosition(2, 1 + visualPosition * 2);
            Console.Write(listItem[position][0]);

            position += change;
            visualPosition += change;

            if(visualPosition < 0)
            {
                visualPosition = 0;
            }
            else if (visualPosition > 4)
            {
                visualPosition = 4;
            }

            listItem[position] = listItem[position].Remove(0, 1);
            listItem[position] = listItem[position].Insert(0, ">");

            Console.SetCursorPosition(2, 1 + visualPosition * 2);
            Console.Write(listItem[position][0]);

            Console.SetCursorPosition(0, 12);
        }
        private static void ChangeSelectedLittle(int change)
        {

            int offsetLeft = 11;
            int offsetTop = 8;


            listLittleMenu[positionLittle] = listLittleMenu[positionLittle].Remove(0, 1);
            listLittleMenu[positionLittle] = listLittleMenu[positionLittle].Insert(0, " ");

            Console.SetCursorPosition(offsetLeft + 1, offsetTop + 1 + positionLittle);
            Console.Write(listLittleMenu[positionLittle][0]);

            positionLittle += change;

            listLittleMenu[positionLittle] = listLittleMenu[positionLittle].Remove(0, 1);
            listLittleMenu[positionLittle] = listLittleMenu[positionLittle].Insert(0, ">");

            Console.SetCursorPosition(offsetLeft + 1, offsetTop + 1 + positionLittle);
            Console.Write(listLittleMenu[positionLittle][0]);


            Console.SetCursorPosition(0, 12);
        }
        private static void PrintItems(int startList)
        {
            int offset = 12;
            string temp = "               ";

            Console.SetCursorPosition(0, 1);
            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                Console.WriteLine(temp);
            }


            for (int i = startList; i < 5 + startList && i < listItem.Count; i++)
            {
                Console.SetCursorPosition(2, 1 + (i - startList) * 2);
                Console.Write(listItem[i]);

                if (i != listItem.Count - 1)
                {
                    Console.SetCursorPosition(offset, 2 + (i - startList) * 2);
                    Console.Write("x");
                    if (listItemQuantity[i] < 10) Console.Write(" ");
                    Console.Write(listItemQuantity[i]);
                }
            }
            Console.SetCursorPosition(0, 12);
        }
    }
}
