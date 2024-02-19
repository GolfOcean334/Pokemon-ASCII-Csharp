using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usefull;
using static System.Net.Mime.MediaTypeNames;

namespace pokemonConsole
{
    internal class Combat
    {
        private static string FightButton = " ATTAQ";
        private static string PokemonButton = " PKMN";
        private static string ItemButton = " OBJET";
        private static string RunButton = " FUITE";

        private static int positionX;
        private static int positionY;

        private static int positionAttack;

        private static List<string> firstLine = new List<string>();
        private static List<string> secondLine = new List<string>();
        private static List<List<string>> bothLines = new List<List<string>>();

        private static List<string> listAttack = new List<string>();

        private static int cursorLeft;
        private static int cursorTop;

        private static int pokemonWidth;

        private static int tour = 0;


        private static List<Pokemon> pokemonPartyPlayer = null;



        public static void LoopCombat(Player player, List<Pokemon> pokemonPartyAdverse = null)
        {

            // Generer le pokemon adverse
            Random random = new Random();

            Pokemon pokemon = null;
            foreach (Pokemon poke in player.pokemonParty)
            {
                if (poke.pvLeft > 0) pokemon = poke; break;
            }
            pokemonPartyPlayer = player.pokemonParty;

            // Generer un pokemon sauvage
            int pokemonAdverseId = random.Next(1, 152);
            int pokemonAdverseLevel = 0;
            while (!(pokemonAdverseLevel > 0 && pokemonAdverseLevel <= 100))
            {
                pokemonAdverseLevel = random.Next(pokemon.level - 2, pokemon.level + 3);
            }
            Pokemon pokemonAdverse = new Pokemon(pokemonAdverseId, pokemonAdverseLevel);


            if (pokemonPartyAdverse == null)
            {
                pokemonPartyAdverse = new List<Pokemon>();
                pokemonAdverse = new Pokemon(pokemonAdverseId, pokemonAdverseLevel);
                pokemonPartyAdverse.Add(pokemonAdverse);
            }
            else
            {
                pokemonAdverse = pokemonPartyAdverse[0];
            }


            // Boutons Main
            firstLine.Add(FightButton);
            firstLine.Add(PokemonButton);
            secondLine.Add(ItemButton);
            secondLine.Add(RunButton);


            bool Selected = false;
            foreach (string item in firstLine)
            {
                if (item[0] == '>') Selected = true;
            }
            foreach (string item in secondLine)
            {
                if (item[0] == '>') Selected = true;
            }
            if (!Selected)
            {
                firstLine[0] = firstLine[0].Remove(0, 1);
                firstLine[0] = firstLine[0].Insert(0, ">");
                positionX = 0;
                positionY = 0;
            }

            bothLines.Add(firstLine);
            bothLines.Add(secondLine);

            // Boutons Atk
            ResetCapPokemon(pokemon);

            // Affichage 
            PrintPokemon(pokemon, pokemonAdverse);
            cursorLeft = Console.CursorLeft; cursorTop = Console.CursorTop;
            PrintMenuChoice();


            // Variables
            int nbFuite = 0;
            int pokemonEquipeAdverse = 0;
            Capacity capacityUsed = null;



            // Combat 
            ConsoleKeyInfo keyInfo;
            bool endFight = false;
            while (!endFight && !player.IsKO() && !player.caughtPokemon)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (positionY != bothLines[positionX].Count - 1)
                        {
                            SwitchSelectMain(ref positionX, ref positionY, 0, 1);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (positionY != 0)
                        {
                            SwitchSelectMain(ref positionX, ref positionY, 0, -1);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (positionX != bothLines.Count - 1)
                        {
                            SwitchSelectMain(ref positionX, ref positionY, +1, 0);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (positionX != 0)
                        {
                            SwitchSelectMain(ref positionX, ref positionY, -1, 0);
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (bothLines[positionY][positionX] == ">ATTAQ")
                        {
                            PrintMenuEmpty();
                            capacityUsed = LoopChoiceCap(pokemon);

                            if(capacityUsed != null)
                            {
                                PrintMenuEmpty();
                                Attaque(pokemon, pokemonAdverse, capacityUsed);
                                AfterAttack(ref pokemon, ref pokemonAdverse, ref pokemonEquipeAdverse, pokemonPartyAdverse, player);


                                if (player.IsKO())
                                {
                                    PrintInEmptyMenu("Vous avez perdu !");
                                    endFight = true;
                                }
                                else if (VerifAdverse(pokemonPartyAdverse))
                                {
                                    PrintInEmptyMenu("Vous avez gagné !");
                                    endFight = true;
                                }
                                else
                                {
                                    PrintMenuChoice();
                                }
                            }

                            
                        }
                        else if (bothLines[positionY][positionX] == ">OBJET")
                        {
                            MenuItems.Open(player, true, pokemonAdverse, pokemon);

                            PrintPokemon(pokemon, pokemonAdverse);
                            PrintMenuChoice();
                        }
                        else if (bothLines[positionY][positionX] == ">PKMN")
                        {
                            MenuPokemon.Open(player, true);
                            pokemon = player.pokemonParty[0];

                            ResetCapPokemon(pokemon);
                            PrintPokemon(pokemon, pokemonAdverse);
                            PrintMenuChoice();
                        }
                        else if (bothLines[positionY][positionX] == ">FUITE" && pokemonAdverse.appartenant != 2)
                        {
                            PrintMenuEmpty();

                            nbFuite++;
                            int spdQuart = (int)Math.Floor(pokemonAdverse.spdCombat / 4.0);
                            int fuiteEuclidienne = (spdQuart % 255 == 0) ? 0 : 1;
                            int randomFuiteValue = random.Next(0, 256);
                            int fuite = (pokemon.spd * 32 / (spdQuart % 255)) + (30 * nbFuite);

                            if (fuite > 255 || randomFuiteValue < fuite || fuiteEuclidienne == 0)
                            {
                                PrintInEmptyMenu("Vous avez reussi a fuir !");
                                endFight = true;
                            }
                            else
                            {
                                PrintInEmptyMenu("Vous n'avez pas reussi à fuir !");
                                capacityUsed = capacityUsedAdv(pokemonAdverse, pokemon);

                                PrintInEmptyMenu($"{pokemonAdverse.name} a utilisé {capacityUsed.name} !");

                                pokemon.pvLeft -= (int)Math.Round(CalculerDegatSubitPokemon(pokemonAdverse, pokemon, capacityUsed));
                                capacityUsed.Use(pokemonAdverse, pokemon);

                                PrintPvBar(pokemon);

                                PrintMenuChoice();
                            }
                        }
                        else if (bothLines[positionX][positionY] == ">FUITE" && pokemonAdverse.appartenant == 2)
                        {
                            PrintInEmptyMenu("Vous ne pouvez pas fuir d'un combat de dresseurs !");
                            PrintMenuChoice();
                        }
                        break;
                
                }
            }
            cursorLeft = 0;
            cursorTop = 0;
            firstLine.Clear();
            secondLine.Clear();
            bothLines.Clear();
            player.pokemonParty = pokemonPartyPlayer;
            foreach (Pokemon poke in player.pokemonParty)
            {
                if (poke.canEvolve == true) poke.Evolution();
            }
            player.caughtPokemon = false;
        }
        private static Capacity LoopChoiceCap(Pokemon pokemon)
        {
            PrintMenuAttack(pokemon);
            PrintPPAttack(pokemon);

            ConsoleKeyInfo keyInfo;
            bool endChoice = false;
            while (!endChoice)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        if(positionAttack < listAttack.Count - 1)
                        {
                            SwitchSelectAttack(ref positionAttack, 1);
                            PrintPPAttack(pokemon);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (positionAttack > 0)
                        {
                            SwitchSelectAttack(ref positionAttack, -1);
                            PrintPPAttack(pokemon);
                        }
                        break;
                    case ConsoleKey.Enter:
                        bool attackHavePP = false;
                        foreach (Capacity capacity in pokemon.listAttackActual)
                        {
                            if(capacity.ppLeft > 0) attackHavePP = true;
                        }

                        if (attackHavePP)
                        {
                            if (pokemon.listAttackActual[positionAttack].ppLeft > 0)
                            {
                                return pokemon.listAttackActual[positionAttack];
                            }
                            else
                            {
                                PrintMenuEmpty();
                                PrintInEmptyMenu($"{pokemon.listAttackActual[positionAttack]} n'a plus de PP ! ");
                                PrintMenuAttack(pokemon);
                                PrintPPAttack(pokemon);
                            }
                        }
                        else
                        {
                            PrintMenuEmpty();
                            PrintInEmptyMenu("Vous n'avez plus de PP !");
                            return new Capacity(165);
                        }
                        
                        break;
                    case ConsoleKey.Escape:
                        return null;
                        break;
                }
            }
            return new Capacity(165);
        }


        private static void Attaque(Pokemon pokemon, Pokemon pokemonAdverse, Capacity capacityUsed)
        {
            tour++; 

            Random random = new Random();
            bool playerAttackFirst = false;
            Capacity capacityAdv = null;

            if (pokemon.spd > pokemonAdverse.spd)
            {
                playerAttackFirst = true;
            }
            else if (pokemon.spd == pokemonAdverse.spd)
            {
                if(random.Next(0, 2) == 0)
                {
                    playerAttackFirst = true;
                }
            }

            if (playerAttackFirst)
            {

                PrintInEmptyMenu($"{pokemon.name} a utilisé {capacityUsed.name} !");

                pokemonAdverse.pvLeft -= (int)Math.Round(CalculerDegatSubitPokemon(pokemon, pokemonAdverse, capacityUsed));
                capacityUsed.Use(pokemon, pokemonAdverse);
                capacityUsed.ppLeft -= 1;

                PrintPvBar(pokemonAdverse, true);

                if (pokemonAdverse.pvLeft > 0)
                {
                    capacityAdv = capacityUsedAdv(pokemonAdverse, pokemon);

                    PrintInEmptyMenu($"{pokemonAdverse.name} a utilisé {capacityAdv.name} !");

                    pokemon.pvLeft -= (int)Math.Round(CalculerDegatSubitPokemon(pokemonAdverse, pokemon, capacityAdv));
                    capacityAdv.Use(pokemonAdverse, pokemon);

                    PrintPvBar(pokemon);
                }

            }
            else
            {
                capacityAdv = capacityUsedAdv(pokemonAdverse, pokemon);

                PrintInEmptyMenu($"{pokemonAdverse.name} a utilisé {capacityAdv.name} !");

                pokemon.pvLeft -= (int)Math.Round(CalculerDegatSubitPokemon(pokemonAdverse, pokemon, capacityAdv));
                capacityAdv.Use(pokemonAdverse, pokemon);

                PrintPvBar(pokemon);


                if (pokemon.pvLeft > 0)
                {
                    PrintInEmptyMenu($"{pokemon.name} a utilisé {capacityUsed.name} !");

                    pokemonAdverse.pvLeft -= (int)Math.Round(CalculerDegatSubitPokemon(pokemon, pokemonAdverse, capacityUsed));
                    capacityUsed.Use(pokemon, pokemonAdverse);
                    capacityUsed.ppLeft -= 1;

                    PrintPvBar(pokemonAdverse, true);
                }
            }


            
        }
        private static Capacity capacityUsedAdv(Pokemon pokemonAdv, Pokemon pokemon)
        {
            Capacity returnCap = null;
            Random random = new Random();

            if (pokemonAdv.appartenant == 2)
            {
                double degatsMax = double.MinValue;
                Capacity meilleureAttaque = null;
                int positionMeilleureAttaque = -1;
                for (int i = 0; i < pokemonAdv.listAttackActual.Count; i++)
                {
                    Capacity attaque = pokemonAdv.listAttackActual[i];
                    // Calcul des dégâts infligés par cette attaque
                    double degats = CalculerDegatSubitPokemon(pokemonAdv, pokemon, attaque, true);

                    // Mise à jour de l'attaque choisie si les dégâts sont inférieurs au minimum
                    if (degats > degatsMax)
                    {
                        degatsMax = degats;
                        meilleureAttaque = attaque;
                        positionMeilleureAttaque = i; // Mémorisation de la position de l'attaque choisie
                    }
                }

                // Affectation de capacityUsed à l'attaque choisie
                if (positionMeilleureAttaque != -1)
                {
                    returnCap = pokemonAdv.listAttackActual[positionMeilleureAttaque];
                }
            }
            else
            {
                returnCap = pokemonAdv.listAttackActual[random.Next(0, pokemonAdv.listAttackActual.Count)];
            }

            return returnCap;
        }
        private static void KillRewards(Pokemon pokemon, Pokemon pokemonAdverse)
        {
            float appartenant;
            float echange = 1; // x1.5 si c'est un pokemon echangé
            int nombrePokemon = 1; // Le nombre de pokemon qui ont combattu


            if (pokemonAdverse.appartenant == 0)
            {
                appartenant = 1;
            }
            else
            {
                appartenant = 1.5f;
            }

            float expWon = (appartenant * echange * pokemonAdverse.expDonne * pokemonAdverse.level) / 7 * nombrePokemon;

            if (pokemon.level < 100)
            {
                pokemon.GainExp((int)Math.Round(expWon));
            }
            pokemon.GainEV(pokemonAdverse.listPv[0], pokemonAdverse.listAtk[0], pokemonAdverse.listDef[0], pokemonAdverse.listSpe[0], pokemonAdverse.listSpd[0]);
        }
        private static void AfterAttack(ref Pokemon pokemon, ref Pokemon pokemonAdverse, ref int pokemonEquipeAdverse, List<Pokemon> pokemonPartyAdverse, Player player)
        {
            Capacity.ApplyStatusEffects(pokemon);
            Capacity.ApplyStatusEffects(pokemonAdverse);

            if (pokemon.pvLeft <= 0 && !player.IsKO())
            {
                MenuPokemon.Open(player, true);
                pokemon = player.pokemonParty[0];
                ResetCapPokemon(pokemon);
                PrintPokemon(pokemon, pokemonAdverse);
                PrintMenuAttack(pokemon);
            }

            if (pokemonAdverse.pvLeft <= 0)
            {
                KillRewards(pokemon, pokemonAdverse);

                if (pokemonEquipeAdverse + 1 < pokemonPartyAdverse.Count && !VerifAdverse(pokemonPartyAdverse))
                {
                    pokemonAdverse = pokemonPartyAdverse[pokemonEquipeAdverse + 1];
                    pokemonEquipeAdverse++;

                    PrintPokemon(pokemon, pokemonAdverse);
                    PrintMenuAttack(pokemon);
                }
            }
        }


        private static double CalculerDegatSubitPokemon(Pokemon pokemon, Pokemon pokemonAdverse, Capacity capacity, bool test = false)
        {
            // Degâts infliges = (((((((Niveau × 2 ÷ 5) +2) × Puissance × Att[Spe] ÷ 50) ÷ Def[Spe]) × Mod1) +2) × CC × Mod2 × R ÷ 100) × STAB × Type1 × Type2 × Mod3

            Random random = new Random();


            int hitOrNot = random.Next(1, 101);
            try
            {
                int.Parse(capacity.precision);
                if (hitOrNot > int.Parse(capacity.precision) && !test)
                {
                    PrintInEmptyMenu("Mais cela echoue.");
                    return 0;
                }
            }
            catch 
            {
            }

            if(capacity.puissance == 0)
            {
                return 0;
            }





            int atkSpeOrNot = 0;
            int defSpeOrNot = 0;
            float isBurn = 1;
            float critChance = 1;
            float critDamage = 1;
            float randomMod = (random.Next(217, 256) * 100) / 255;
            int stab = 1;

            float efficaciteType1 = TypeModifier.CalculerMultiplicateur(capacity.type, pokemonAdverse.listType[0]);
            float efficaciteType2 = 1;

            if (pokemonAdverse.listType.Count > 1)
            {
                efficaciteType2 = TypeModifier.CalculerMultiplicateur(capacity.type, pokemonAdverse.listType[1]);
            }


            // Determine si la capacite est physique ou special selon le type
            if (capacity.type == "DRAGON" || capacity.type == "EAU" || capacity.type == "ELECTRIK" || capacity.type == "FEU" || capacity.type == "GLACE" || capacity.type == "PLANTE" || capacity.type == "PSY")
            {
                atkSpeOrNot = pokemon.speCombat;
                defSpeOrNot = pokemonAdverse.speCombat;
            }
            else
            {
                atkSpeOrNot = pokemon.atkCombat;
                defSpeOrNot = pokemonAdverse.defCombat;
            }

            // Si le Pokemon est burn, l'attaque est divisee par deux
            if (pokemon.statusProblem == "BRN")
            {
                isBurn = isBurn * 0.5f;
            }

            if (efficaciteType1 * efficaciteType2 > 1 && !test)
            {
                PrintInEmptyMenu("C'est super efficace !");
            }
            else if (efficaciteType1 * efficaciteType2 < 1 && efficaciteType1 * efficaciteType2 != 0 && !test)
            {
                PrintInEmptyMenu("C'est pas tres efficace !");
            }
            else if (efficaciteType1 * efficaciteType2 == 0 && !test)
            {
                PrintInEmptyMenu("Ca n'a pas d'effet !");
            }

            if (!test)
            {
                // Critique
                critChance = ((int)Math.Round(pokemon.spdCombat / 2.0) * 2) / 256;
                if (critChance == 0)
                {
                    critDamage = 1;
                }
                else
                {
                    critDamage = 2;
                    PrintInEmptyMenu("Coup critique !");
                }
            }
            
            foreach (string typePokemon in pokemon.listType)
            {
                if (capacity.type == typePokemon)
                {
                    stab = 2;
                }
            }


            

            double damageDone = (((((((pokemon.level * 2 / 5) + 2) * capacity.puissance * atkSpeOrNot / 50) / defSpeOrNot) * isBurn) + 2) * critDamage * randomMod / 100) * stab * efficaciteType1 * efficaciteType2;



            return damageDone;



            
        }
        




        static public void PrintPokemon(Pokemon pokemon, Pokemon pokemonAdverse)
        {
            // Pokemon Adverse
            if (pokemonAdverse.width >= pokemon.widthBack + 11) pokemonWidth = pokemonAdverse.width;
            else pokemonWidth = pokemon.widthBack + 11;


            int offsetPokemon = pokemonWidth - 11;

            Console.Clear();
            Console.ForegroundColor = pokemonAdverse.color;
            Console.WriteLine(pokemonAdverse.asciiArt);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            Console.WriteLine(pokemonAdverse.name);
            Console.SetCursorPosition(Console.CursorLeft + 1 + 3, Console.CursorTop);
            if (pokemonAdverse.level.ToString().Length < 3)
            {
                Console.Write("L");
            }
            Console.WriteLine(pokemonAdverse.level);
            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            Console.Write("|");

            PrintPvBar(pokemonAdverse, true);
            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            Console.WriteLine("0----------");



            // Pokemon

            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 3);
            Console.ForegroundColor = pokemon.color;
            Console.WriteLine(pokemon.asciiArtBack);
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(Console.CursorLeft + offsetPokemon, Console.CursorTop - 5);
            for (int i = pokemon.name.Length; i < 10; i++)
            {
                Console.Write(' ');
            }
            Console.WriteLine(pokemon.name);
            Console.SetCursorPosition(Console.CursorLeft + offsetPokemon + 5, Console.CursorTop);
            if (pokemon.level.ToString().Length < 3)
            {
                Console.Write("L");
            }
            Console.WriteLine(pokemon.level);
            Console.SetCursorPosition(Console.CursorLeft + offsetPokemon + 2, Console.CursorTop);

            PrintPvBar(pokemon);
            Console.SetCursorPosition(Console.CursorLeft + offsetPokemon + 1, Console.CursorTop);
            Console.WriteLine("---------0");
        }
        static private void PrintPvBar(Pokemon pokemon, bool pokemonAdverse = false)
        {

            int offsetPokemon = pokemonWidth - 11;

            if (!pokemonAdverse && cursorTop != 0)
            {
                Console.SetCursorPosition(cursorLeft + offsetPokemon + 5, cursorTop -4);

                if (pokemon.statusProblem == "OK")
                {
                    if (pokemon.level.ToString().Length < 3)
                    {
                        Console.Write("L");
                    }
                    Console.Write(pokemon.level);
                }
                else
                {
                    Console.Write(pokemon.statusProblem);
                }
                Console.SetCursorPosition(cursorLeft + offsetPokemon + 2, cursorTop - 3);

            }
            else if (pokemonAdverse && cursorTop != 0) 
            {
                Console.SetCursorPosition(cursorLeft + 4, cursorTop - 11);

                if (pokemon.statusProblem == "OK")
                {
                    if (pokemon.level.ToString().Length < 3)
                    {
                        Console.Write("L");
                    }
                    Console.Write(pokemon.level);
                }
                else
                {
                    Console.Write(pokemon.statusProblem);
                }

                Console.SetCursorPosition(cursorLeft + 2, cursorTop - 10);
            }

            


            if (pokemon.pvLeft < 0) pokemon.pvLeft = 0;

            int pvPerSix = pokemon.pvLeft * 6 / pokemon.pv;
            string barPv = "PV";
            for (int i = 0; i < pvPerSix; i++)
            {
                barPv += "=";
            }
            for (int i = barPv.Length; i < 8; i++)
            {
                barPv += "*";
            }
            foreach (char c in barPv)
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

            if (!pokemonAdverse)
            {
                Console.WriteLine('|');
                Console.SetCursorPosition(Console.CursorLeft + offsetPokemon + 3, Console.CursorTop);
                for (int j = 3; j > pokemon.pvLeft.ToString().Length; j--)
                {
                    Console.Write(" ");
                }
                Console.Write(pokemon.pvLeft + "/");
                for (int j = 3; j > pokemon.pv.ToString().Length; j--)
                {
                    Console.Write(" ");
                }
                Console.Write(pokemon.pv + "|");
            }

            Console.WriteLine();
            
        }
        static private void PrintMenuChoice()
        {
            int offset = pokemonWidth - 16;

            // Clear
            for (int i = 4; i > 0; i--)
            {
                Console.SetCursorPosition(cursorLeft, cursorTop - i);
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);


            // Variables
            string topBox =    "0--------------0";
            string middleBox = "|              |";
            string topWholeBox = "0";
            for (int i = 1; i < offset; i++)
            {
                topWholeBox += "-";
            }
            string middleWholeBox = "|";
            for (int i = 1; i < offset; i++)
            {
                middleWholeBox += " ";
            }

            //Print
            Console.WriteLine(topWholeBox + topBox);
            for (int i = 0;i < 4; i++)
            {
                Console.WriteLine(middleWholeBox + middleBox);
            }
            Console.WriteLine(topWholeBox + topBox);

            

            // Print 2

            Console.SetCursorPosition(cursorLeft + offset + 2, cursorTop + 2);
            Console.Write(firstLine[0]);
            Console.SetCursorPosition(Console.CursorLeft +1, Console.CursorTop);
            Console.WriteLine(firstLine[1]);

            Console.WriteLine();

            Console.SetCursorPosition(Console.CursorLeft + offset + 2, Console.CursorTop);
            Console.Write(secondLine[0]);
            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            Console.Write(secondLine[1]);

        }
        static private void PrintMenuAttack(Pokemon pokemon)
        {
            int offset = pokemonWidth - 17;


            // Variables
            string topBox = "0---------------0";
            string middleBox = "|               |";


            string topWholeBox = "0";
            for (int i = 1; i < offset; i++)
            {
                topWholeBox += "-";
            }
            string middleWholeBox = "|";
            for (int i = 1; i < offset; i++)
            {
                middleWholeBox += " ";
            }

            //Print
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine(topWholeBox + topBox);
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(middleWholeBox + middleBox);
            }
            Console.WriteLine(topWholeBox + topBox);


            

            // Print 2
            Console.SetCursorPosition(cursorLeft, cursorTop + 1);
            foreach (string str in listAttack)
            {
                Console.SetCursorPosition(Console.CursorLeft + offset + 2, Console.CursorTop);
                Console.WriteLine(str);
            }
            for (int i = listAttack.Count;i <= 4; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft + offset + 2, Console.CursorTop);
                Console.WriteLine("-");
            }
        }
        static private void PrintPPAttack(Pokemon pokemon)
        {
            string topBox = "0---------0";
            string MiddleBox = "|         |";

            Console.SetCursorPosition(cursorLeft, cursorTop - 4);
            Console.WriteLine(topBox);
            for (int i = 0; i < 3; i++) 
            {
                Console.WriteLine(MiddleBox);
            }
            Console.WriteLine(topBox);

            Console.SetCursorPosition(cursorLeft+1, cursorTop - 3);
            Console.WriteLine("TYPE/");

            Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop);
            Console.WriteLine(pokemon.listAttackActual[positionAttack].type);

            Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
            if (pokemon.listAttackActual[positionAttack].ppLeft < 10) Console.Write(" ");
            Console.Write(pokemon.listAttackActual[positionAttack].ppLeft + "/");
            if (pokemon.listAttackActual[positionAttack].pp < 10) Console.Write(" ");
            Console.Write(pokemon.listAttackActual[positionAttack].pp);

            Console.SetCursorPosition(cursorLeft, cursorTop + 6);
        }
        static public void PrintMenuEmpty()
        {
            // Clear
            string clear = "           ";
            for (int i = 4; i > 0; i--)
            {
                Console.SetCursorPosition(cursorLeft, cursorTop - i);
                Console.Write(clear);
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);



            string topBox = "0";
            for (int i = 1; i < pokemonWidth-1; i++)
            {
                topBox += "-";
            }
            topBox += "0";
            string middleBox = "|";
            for (int i = 1; i < pokemonWidth-1; i++)
            {
                middleBox += " ";
            }
            middleBox += "|";

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine(topBox);
            for(int i = 0;i < 4;i++)
            {
                Console.WriteLine(middleBox);
            }
            Console.WriteLine(topBox);

        }
        static public void PrintInEmptyMenu(string text)
        {
            // Clear
            string clear = "";
            for (int  i = 0;  i < pokemonWidth - 4;  i++)
            {
                clear += " ";
            }
            for (int i = 0; i < 4; i++)
            {
                Console.SetCursorPosition(cursorLeft + 2, cursorTop + i + 1);
                Console.WriteLine(clear);
            }


            Console.SetCursorPosition(cursorLeft+2, cursorTop+1);
            int charRestant = pokemonWidth - 4;
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
                    if (text[i] == ' ' || (charRestant == 0 && text[i + 1] == ' '))
                    {
                        wordIndex++;

                        // Vérifie si le mot suivant peut s'insérer dans la ligne actuelle
                        if (wordIndex < words.Length && words[wordIndex].Length > charRestant)
                        {
                            charRestant = pokemonWidth - 4;
                            lineWriting++;
                            Console.SetCursorPosition(cursorLeft + 2, cursorTop + 3);
                        }
                    }
                }
                else if (charRestant > 0 && lineWriting == 2)
                {
                    if (!(charRestant == pokemonWidth - 4 && text[i] == ' '))
                    {
                        Console.Write(text[i]);
                    }

                    charRestant--;

                    // Si le caractère actuel est un espace, passe au mot suivant
                    if (text[i] == ' ' || (charRestant == 0 && text[i + 1] == ' '))
                    {
                        wordIndex++;

                        // Vérifie si le mot suivant peut s'insérer dans la ligne actuelle
                        if (wordIndex < words.Length && words[wordIndex].Length > charRestant)
                        {
                            charRestant = pokemonWidth - 4;
                            lineWriting++;
                            Console.SetCursorPosition(cursorLeft + 2, cursorTop + 5);
                        }
                    }
                }
                else if (charRestant > 0 && lineWriting == 3)
                {
                    if (!(charRestant == pokemonWidth - 4 && text[i] == ' '))
                    {
                        Console.Write(text[i]);
                    }

                    charRestant--;

                }
            }
            Console.ReadKey(true);
        }


        static private void SwitchSelectMain(ref int x, ref int y, int nextX, int nextY)
        {
            int offset = pokemonWidth - 16;

            bothLines[y][x] = bothLines[y][x].Remove(0, 1);
            bothLines[y][x] = bothLines[y][x].Insert(0, " ");

            Console.SetCursorPosition(cursorLeft + offset + 2 + x * 7, cursorTop + 2 + y * 2);
            Console.Write(bothLines[y][x][0]);

            x += nextX;
            y += nextY;

            bothLines[y][x] = bothLines[y][x].Remove(0, 1);
            bothLines[y][x] = bothLines[y][x].Insert(0, ">");

            Console.SetCursorPosition(cursorLeft + offset + 2 + x * 7, cursorTop + 2 + y * 2);
            Console.Write(bothLines[y][x][0]);
            Console.SetCursorPosition(cursorLeft, cursorTop + 6);
        }
        static private void SwitchSelectAttack(ref int x, int nextX)
        {
            int offset = pokemonWidth - 17;

            listAttack[x] = listAttack[x].Remove(0, 1);
            listAttack[x] = listAttack[x].Insert(0, " ");

            Console.SetCursorPosition(cursorLeft + offset + 2, cursorTop + 1 + x);
            Console.Write(listAttack[x][0]);

            x += nextX;

            listAttack[x] = listAttack[x].Remove(0, 1);
            listAttack[x] = listAttack[x].Insert(0, ">");

            Console.SetCursorPosition(cursorLeft + offset + 2, cursorTop + 1 + x);
            Console.Write(listAttack[x][0]);
            Console.SetCursorPosition(cursorLeft, cursorTop + 6);
        }

        static private void ResetCapPokemon(Pokemon pokemon)
        {
            listAttack.Clear();
            string buttonaa = " ";
            foreach (Capacity atk in pokemon.listAttackActual)
            {
                listAttack.Add(buttonaa + atk.name);
            }

            bool Selected = false;
            foreach (string attack in listAttack)
            {
                if (attack[0] == '>') Selected = true;
            }

            if (!Selected)
            {
                listAttack[0] = listAttack[0].Remove(0, 1);
                listAttack[0] = listAttack[0].Insert(0, ">");
                positionAttack = 0;
            }
        }

        

        public class TypeModifier
        {
            public static float CalculerMultiplicateur(string typePokemon, string typeAdverse)
            {
                Dictionary<string, Dictionary<string, float>> multiplicateurs = new Dictionary<string, Dictionary<string, float>>()
                {
                    {"NORMAL", new Dictionary<string, float>() {{"ROCHE", 0.5f}, {"SPECTRE", 0f}, {"default", 1f}}},
                    {"FEU", new Dictionary<string, float>() {{"FEU", 0.5f}, {"EAU", 0.5f}, {"ROCHE", 0.5f}, {"DRAGON", 0.5f}, {"PLANTE", 2f}, {"GLACE", 2f}, {"INSECTE", 2f}, {"default", 1f}}},
                    {"EAU", new Dictionary<string, float>() {{"EAU", 0.5f}, {"PLANTE", 0.5f}, {"DRAGON", 0.5f}, {"FEU", 2f}, {"SOL", 2f}, {"ROCHE", 2f}, {"default", 1f}}},
                    {"PLANTE", new Dictionary<string, float>() {{"FEU", 0.5f}, {"PLANTE", 0.5f}, {"POISON", 0.5f}, { "VOL", 0.5f }, { "INSECTE", 0.5f }, { "DRAGON", 0.5f }, { "EAU", 2f}, {"SOL", 2f}, {"ROCHE", 2f}, {"default", 1f}}},
                    {"ELECTRIK", new Dictionary<string, float>() {{"PLANTE", 0.5f}, {"ELECTRIK", 0.5f}, {"DRAGON", 0.5f}, {"EAU", 2f}, {"VOL", 2f}, {"SOL", 0f}, {"default", 1f}}},
                    {"GLACE", new Dictionary<string, float>() {{"EAU", 0.5f}, {"GLACE", 0.5f}, {"PLANTE", 2f}, {"SOL", 2f}, {"VOL", 2f}, {"DRAGON", 2f}, {"default", 1f}}},
                    {"COMBAT", new Dictionary<string, float>() {{"POISON", 0.5f}, {"VOL", 0.5f}, {"PSY", 0.5f}, {"INSECTE", 0.5f}, {"NORMAL", 2f}, {"GLACE", 2f}, { "ROCHE", 2f }, { "SPECTRE", 0f }, { "default", 1f}}},
                    {"POISON", new Dictionary<string, float>() {{"POISON", 0.5f}, {"SOL", 0.5f}, {"ROCHE", 0.5f}, {"SPECTRE", 0.5f}, {"PLANTE", 2f}, {"INSECTE", 2f}, {"default", 1f}}},
                    {"SOL", new Dictionary<string, float>() {{"PLANTE", 0.5f}, {"INSECTE", 0.5f}, {"FEU", 2f}, {"ELECTRIK", 2f}, {"POISON", 2f}, {"ROCHE", 2f}, { "VOL", 0f }, { "default", 1f}}},
                    {"VOL", new Dictionary<string, float>() {{"ELECTRIK", 0.5f}, {"ROCHE", 0.5f}, {"PLANTE", 2f}, {"COMBAT", 2f}, {"INSECTE", 2f}, {"default", 1f}}},
                    {"PSY", new Dictionary<string, float>() {{"PSY", 0.5f}, {"COMBAT", 2f}, {"POISON", 2f}, {"default", 1f}}},
                    {"INSECTE", new Dictionary<string, float>() {{"FEU", 0.5f}, { "COMBAT", 0.5f }, { "VOL", 0.5f }, { "SPECTRE", 0.5f }, { "PLANTE", 2f}, {"POISON", 2f}, { "PSY", 2f }, { "default", 1f}}},
                    {"ROCHE", new Dictionary<string, float>() {{"COMBAT", 0.5f}, {"SOL", 0.5f}, {"FEU", 2f}, { "GLACE", 2f }, { "VOL", 2f }, { "INSECTE", 2f }, { "default", 1f}}},
                    {"SPECTRE", new Dictionary<string, float>() {{"SPECTRE", 2f}, {"NORMAL", 0f}, { "PSY", 0f }, { "default", 1f}}},
                    {"DRAGON", new Dictionary<string, float>() {{"DRAGON", 2f}, { "default", 1f}}},
                };

                if (multiplicateurs.ContainsKey(typePokemon))
                {
                    if (multiplicateurs[typePokemon].ContainsKey(typeAdverse))
                    {
                        return multiplicateurs[typePokemon][typeAdverse];
                    }
                    else
                    {
                        return multiplicateurs[typePokemon]["default"];
                    }
                }
                else
                {
                    return 1f;
                }

            }
        }
        public static bool VerifAdverse(List<Pokemon>pokemonPartyAdverse) 
        {
            foreach (Pokemon p in pokemonPartyAdverse)
            {
                if (p.pvLeft > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }


}

