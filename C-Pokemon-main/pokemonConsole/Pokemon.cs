using Usefull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


using System.Buffers.Text;



namespace pokemonConsole
{
    public class Pokemon
    {
        // ------------------------------------- Infos Pokemon ------------------------------------- //
        public int id { get; private set; }
        public string name { get; private set; }
        private string actual_name;
        public List<string> listType = new List<string>();
        public string asciiArt { get; private set; }
        public string asciiArtBack { get; private set; }
        public ConsoleColor color { get; private set; }

        public int idOT { get;  set; }
        public string nameOT { get;  set; }


        // ------------------ Level ------------------ //
        public int level { get; private set; }
        public int expToLevelUp { get; private set; }
        public int expActuel { get; set; }

        // ------------------ Statistiques ------------------ //
        public int pv { get; private set; }
        public int pvLeft {  get; set; }
        public int atk { get; private set; }
        public int def { get; private set; }
        public int spe { get; private set; }
        public int spd { get; private set; }
        public int atkCombat { get; set; }
        public int defCombat { get; set; }
        public int speCombat { get; set; }
        public int spdCombat { get; set; }


        // Max EV = 65535 
        // [0] = Base
        // [1] = DV
        // [2] = EV
        public List<int> listPv = new List<int>();
        public List<int> listAtk = new List<int>();
        public List<int> listDef = new List<int>();
        public List<int> listSpe = new List<int>();
        public List<int> listSpd = new List<int>();


        // ------------------ Capacites ------------------ //
        public List<Capacity> listAttackActual = new List<Capacity>();


        // ------------------ Etat ------------------ //
        public string statusProblem { get; set; }
        public bool ko {  get; set; }

        // ------------------ Back ------------------ //
        private List<string> listEvo = new List<string>();

        private List<int> listAttackId = new List<int>();
        private List<int> listAttackLevel = new List<int>();

        private int maxEv = 65535;

        private string expCourbe;
        public int expDonne { get; private set; }
        private bool peutEvoluer;
        private int methodeEvolution;
        private int evolutionLevel;
        public List<int> evolutionItemId = new List<int>();

        public int tauxCapture {  get; private set; }

        private string filePokemonCSV = AdresseFile.FileDirection + "CSV\\pokemon.csv";
        private string colorCSV;

        public int appartenant {  get; set; }
        public bool echange {  get; set; }

        public int width { get; private set; }
public int widthBack {get; private set;}

        public bool canEvolve { get; private set; } = false;




        // ------------------------------------- Fonctions ------------------------------------- //
        public Pokemon(int id_generate, int level_generate, int appartenant_ = 0, int playerId = 0, int idOT_ = 0, string nameOT_ = "OT") // appartenant -> 0 : sauvage, 1 : player, 2 : dresseur
        {

            Random random = new Random();

            // Variables
            int basePv = 0;
            int baseAtk = 0;
            int baseDef = 0;
            int baseSpe = 0;
            int baseSpd = 0;

            int dvPv = random.Next(0, 16);
            int dvAtk = random.Next(0, 16);
            int dvDef = random.Next(0, 16);
            int dvSpe = random.Next(0, 16);
            int dvSpd = random.Next(0, 16);

            List<int> listAttackStart = new List<int>();


            using (StreamReader sr = new StreamReader(filePokemonCSV))
            {
                string line;
                bool pokemonFound = false;
                bool pokemonFinishReading = false;

                line = sr.ReadLine();
                line = sr.ReadLine();

                while ((line = sr.ReadLine()) != null && !pokemonFinishReading)
                {
                    if (!pokemonFound)
                    {
                        string[] colonnes = line.Split(',');

                        int.TryParse(colonnes[0], out int id_search);

                        if (id_search == id_generate)
                        {
                            name = colonnes[1];
                            actual_name = name;
                            listType.Add(colonnes[2]);
                            if (colonnes[3] != "NONE")
                            {
                                listType.Add(colonnes[3]);
                            }
                            basePv = int.Parse(colonnes[4]);
                            baseAtk = int.Parse(colonnes[5]);
                            baseDef = int.Parse(colonnes[6]);
                            baseSpe = int.Parse(colonnes[7]);
                            baseSpd = int.Parse(colonnes[8]);
                            atkCombat = baseAtk;
                            defCombat = baseDef;
                            speCombat = baseSpe;
                            spdCombat = baseSpd;
                            if (colonnes[9] == "FALSE")
                            {
                                peutEvoluer = false;
                                pokemonFinishReading = true;
                            }
                            else
                            {
                                peutEvoluer = true;
                            }
                            if (peutEvoluer)
                            {
                                methodeEvolution = int.Parse(colonnes[10]);
                                if (methodeEvolution == 0)
                                {
                                    evolutionLevel = int.Parse(colonnes[11]);
                                }
                                else if (methodeEvolution == 2)
                                {
                                    //evolutionItemId.Add(int.Parse(colonnes[11]));
                                }
                                else if ((methodeEvolution == 3))
                                {
                                    //string[] items = colonnes[11].Split('/');
                                    //evolutionItemId = items.Select(int.Parse).ToList();
                                }
                            }
                            
                            expCourbe = colonnes[12];
                            expDonne = int.Parse(colonnes[13]);

                            tauxCapture = int.Parse(colonnes[14]);

                            string[] temp = colonnes[15].Split("/");
                            listAttackStart = temp.Select(int.Parse).ToList();


                            if (colonnes[16] != "")
                            {
                                temp = colonnes[16].Split("/");
                                listAttackId = temp.Select(int.Parse).ToList();

                                temp = colonnes[17].Split("/");
                                listAttackLevel = temp.Select(int.Parse).ToList();
                            }
                            colorCSV = colonnes[18];

                            pokemonFound = true;
                        }
                    }

                    else
                    {
                        listEvo.Add(line);

                        string[] colonnes = line.Split(',');
                        if (colonnes[9] == "FALSE")
                        {
                            pokemonFinishReading = true;
                        }
                    }
                }
            }

            listPv.Add(basePv); listPv.Add(dvPv); listPv.Add(0);
            listAtk.Add(baseAtk); listAtk.Add(dvAtk); listAtk.Add(0);
            listDef.Add(baseDef); listDef.Add(dvDef); listDef.Add(0);
            listSpe.Add(baseSpe); listSpe.Add(dvSpe); listSpe.Add(0);
            listSpd.Add(baseSpd); listSpd.Add(dvSpd); listSpd.Add(0);


            id = id_generate;
            level = level_generate;

            pv = FormulaStatsPv(this.level, this.listPv);
            atk = FormulaStatsNotPv(this.level, this.listAtk);
            def = FormulaStatsNotPv(this.level, this.listDef);
            spe = FormulaStatsNotPv(this.level, this.listSpe);
            spd = FormulaStatsNotPv(this.level, this.listSpd);
            pvLeft = pv;
            ko = false;

            if (expCourbe == "rapide")
            {
                expActuel = FormulaCourbeRapide(level);
                expToLevelUp = FormulaCourbeRapide(level+1);
            }
            else if (expCourbe == "moyenne")
            {
                expActuel = FormulaCourbeMoyenne(level);
                expToLevelUp = FormulaCourbeMoyenne(level+1);
            }
            else if (expCourbe == "parabolique")
            {
                expActuel = FormulaCourbePara(level);
                expToLevelUp = FormulaCourbePara(level+1);
            }
            else if (expCourbe == "lente")
            {
                expActuel = FormulaCourbeLente(level);
                expToLevelUp = FormulaCourbeLente(level+1);
            }

            statusProblem = "OK";
            appartenant = appartenant_;
            echange = false;
            if (appartenant == 1)
            {
                idOT = idOT_;
                nameOT = nameOT_;

                if (idOT != playerId)
                {
                    echange = true;
                }
                else
                {
                    name = Functions.ClavierName(10);
                }
            }

            // Attaques 
            int numberOfAttacksAvailaible = listAttackStart.Count;
            int numberOfAllAttacksAvailaible = listAttackStart.Count;
            int numberOfAttackLevel = 0;


            // Compter le nombre d'attaques disponibles au moment du level
            for (int i = listAttackLevel.Count - 1; i >= 0; i--)
            {
                if (listAttackLevel[i] <= level_generate)
                {
                    numberOfAttacksAvailaible++;
                    numberOfAllAttacksAvailaible++;
                    numberOfAttackLevel++;
                }
            }

            // Vérifier le nombre de doublons
            for (int i = 0; i < listAttackStart.Count; i++)
            {
                for (int j = 0; j < listAttackId.Count; j++)
                {
                    if (listAttackId[j] == listAttackStart[i])
                    {
                        numberOfAttacksAvailaible--;
                    }
                }
            }


            int numberAssigned = 0;

            // si il y a pile le nombre qu'il faut pour faire les attaques
            if (numberOfAttacksAvailaible <= 4)
            {
                foreach (int AttackStart in listAttackStart)
                {
                    listAttackActual.Add(new Capacity(AttackStart));
                    numberAssigned++;
                }

                while (numberAssigned < numberOfAttacksAvailaible)
                {
                    foreach (int AttackId in listAttackId)
                    {
                        bool AttackTaken = false;
                        foreach (Capacity AttackActual in listAttackActual)
                        {
                            if (AttackActual.id == AttackId)
                            {
                                AttackTaken = true;
                            }
                        }
                        if (!AttackTaken)
                        {
                            listAttackActual.Add(new Capacity(AttackId));
                            numberAssigned++;
                        }
                        if (numberAssigned == numberOfAttacksAvailaible)
                        {
                            break;
                        }
                    }
                }
            }

            // S'il n'y a pas assez pour faire les attaques
            else if (numberOfAttacksAvailaible > 4)
            {

                // s'il faut des attaques de départ 
                if (numberOfAttackLevel < 4)
                {
                    List<Capacity> temp = new List<Capacity>();

                    // mettre les capacités de level up
                    for (int i = 0; i < numberOfAttackLevel; i++)
                    {
                        temp.Add(new Capacity(listAttackId[i]));
                    }

                    // mettre les capacités de départ
                    for (int i = (numberOfAttacksAvailaible - numberOfAttackLevel) - 1; i < listAttackStart.Count; i++)
                    {
                        bool AttackAlreadyTaken = false;
                        foreach (Capacity AttackActual in temp)
                        {
                            if (listAttackStart[i] == AttackActual.id)
                            {
                                AttackAlreadyTaken = true;
                            }
                        }
                        if (!AttackAlreadyTaken)
                        {
                            listAttackActual.Add(new Capacity(listAttackStart[i]));
                        }
                    }

                    foreach (Capacity AttackActual in temp)
                    {
                        listAttackActual.Add(AttackActual);
                    }
                }



                if (numberOfAttackLevel >= 4)
                {
                    int AttackAEviter = numberOfAttackLevel - 4;
                    for (int i = AttackAEviter; i < AttackAEviter + 4; i++)
                    {
                        listAttackActual.Add(new Capacity(listAttackId[i]));
                    }
                }
            }


            // Sprite
            string asciiArtFileName = $"ascii-art ({id_generate}).txt";
            string asciiArtBackFileName = $"back({id_generate}).txt";

            asciiArt = GetSprite(asciiArtFileName, true);
            asciiArtBack = GetSprite(asciiArtBackFileName, false);


            ColorForegroundCheck();


        }
        public Pokemon(int id_, string name_, int idot_, string nameot_, int level_, int expActuel_, int pvLeft_, int dvPv, int evPv, int dvAtk, int evAtk, int dvDef, int evDef, int dvSpe, int evSpe, int dvSpd, int evSpd, string statusProblem_, bool ko_, bool echange_, List<Capacity> listCapacity, int playerId)
        {
            int basePv = 0;
            int baseAtk = 0;
            int baseDef = 0;
            int baseSpe = 0; 
            int baseSpd = 0;

            // Récupère les données des pokemon
            using (StreamReader sr = new StreamReader(filePokemonCSV))
            {
                sr.ReadLine();
                sr.ReadLine();
                bool pokemonFinishReading = false;
                bool pokemonFound = false;

                string line;

                while ((line = sr.ReadLine()) != null && !pokemonFinishReading)
                {
                    if (!pokemonFound)
                    {
                        string[] colonnes = line.Split(',');

                        int.TryParse(colonnes[0], out int id_search);

                        if (id_search == id_)
                        {
                            actual_name = colonnes[1];
                            name = name_;
                            listType.Add(colonnes[2]);
                            if (colonnes[3] != "NONE")
                            {
                                listType.Add(colonnes[3]);
                            }
                            basePv = int.Parse(colonnes[4]);
                            baseAtk = int.Parse(colonnes[5]);
                            baseDef = int.Parse(colonnes[6]);
                            baseSpe = int.Parse(colonnes[7]);
                            baseSpd = int.Parse(colonnes[8]);
                            atkCombat = baseAtk;
                            defCombat = baseDef;
                            speCombat = baseSpe;
                            spdCombat = baseSpd;
                            if (colonnes[9] == "FALSE")
                            {
                                peutEvoluer = false;
                                pokemonFinishReading = true;
                            }
                            else
                            {
                                peutEvoluer = true;
                            }
                            if (peutEvoluer)
                            {
                                methodeEvolution = int.Parse(colonnes[10]);
                                if (methodeEvolution == 0)
                                {
                                    evolutionLevel = int.Parse(colonnes[11]);
                                }
                                else if (methodeEvolution == 2)
                                {
                                    //evolutionItemId.Add(int.Parse(colonnes[11]));
                                }
                                else if ((methodeEvolution == 3))
                                {
                                    //string[] items = colonnes[11].Split('/');
                                    //evolutionItemId = items.Select(int.Parse).ToList();
                                }
                            }

                            expCourbe = colonnes[12];
                            expDonne = int.Parse(colonnes[13]);

                            tauxCapture = int.Parse(colonnes[14]);



                            if (colonnes[16] != "")
                            {
                                string[] temp2 = colonnes[16].Split("/");
                                listAttackId = temp2.Select(int.Parse).ToList();

                                temp2 = colonnes[17].Split("/");
                                listAttackLevel = temp2.Select(int.Parse).ToList();
                            }
                            colorCSV = colonnes[18];

                            pokemonFound = true;
                        }
                    }

                    else
                    {
                        listEvo.Add(line);

                        string[] colonnes = line.Split(',');
                        if (colonnes[9] == "FALSE")
                        {
                            pokemonFinishReading = true;
                        }
                    }
                }
            }
            listPv.Add(basePv); listPv.Add(dvPv); listPv.Add(evPv);
            listAtk.Add(baseAtk); listAtk.Add(dvAtk); listAtk.Add(evAtk);
            listDef.Add(baseDef); listDef.Add(dvDef); listDef.Add(evDef);
            listSpe.Add(baseSpe); listSpe.Add(dvSpe); listSpe.Add(evSpe);
            listSpd.Add(baseSpd); listSpd.Add(dvSpd); listSpd.Add(evSpd);


            id = id_;
            level = level_;

            pv = FormulaStatsPv(this.level, this.listPv);
            atk = FormulaStatsNotPv(this.level, this.listAtk);
            def = FormulaStatsNotPv(this.level, this.listDef);
            spe = FormulaStatsNotPv(this.level, this.listSpe);
            spd = FormulaStatsNotPv(this.level, this.listSpd);
            pvLeft = pvLeft_;
            ko = ko_;


            expActuel = expActuel_;
            int temp = 0;
            if (expCourbe == "rapide")
            {
                expToLevelUp = FormulaCourbeRapide(level + 1);
                temp = FormulaCourbeRapide(level);
            }
            else if (expCourbe == "moyenne")
            {
                expToLevelUp = FormulaCourbeMoyenne(level + 1);
                temp = FormulaCourbeRapide(level);
            }
            else if (expCourbe == "parabolique")
            {
                expToLevelUp = FormulaCourbePara(level + 1);
                temp = FormulaCourbePara(level);
            }
            else if (expCourbe == "lente")
            {
                expToLevelUp = FormulaCourbeLente(level + 1);
                temp = FormulaCourbeLente(level);
            }

            statusProblem = statusProblem_;
            appartenant = 1;
            echange = echange_;
            if (appartenant == 1)
            {
                idOT = idot_;
                nameOT = nameot_;

                if (idOT != playerId)
                {
                    echange = true;
                }
            }


            foreach (Capacity cap in listCapacity)
            {
                listAttackActual.Add(cap);
            }

            // Sprite
                        string asciiArtFileName = $"ascii-art ({id}).txt";
            string asciiArtBackFileName = $"back({id}).txt";

            asciiArt = GetSprite(asciiArtFileName, true);
            asciiArtBack = GetSprite(asciiArtBackFileName, false);



            ColorForegroundCheck();
        }





        public void AfficherDetailsMenu()
        {

            string hautBoxPage1 = "0--------0";
            string middleBoxPage1 = "|        |";

            string hautBoxPage2 =   "0------------------0";
            string middleBoxPage2 = "|                  |";

            int spaceStartInt = width / 2 - 10;
            int pokemonOffset = 10;

            int cursorXTop;
            int cursorYTop;

            int cursorXBox;
            int cursorYBox;

            Console.Clear();

            // Sprite
            Console.WriteLine();
            Console.ForegroundColor = color;
            Console.WriteLine(asciiArt);
            Console.ForegroundColor = ConsoleColor.White;


            Console.WriteLine();

            cursorXTop = Console.CursorLeft;
            cursorYTop = Console.CursorTop;

            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset, Console.CursorTop); // Afficher nom
            Console.WriteLine(name);


            // ---------------- Premiere fenetre ---------------- //
            //Premiere partie
            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset + 5, Console.CursorTop); // Afficher Level
            if (level >= 100)
            {
                Console.WriteLine(level);
            }
            else
            {
                Console.WriteLine("N" + level);
            }

            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset + 2, Console.CursorTop); // Afficher Bar PV
            int pvPerSix = pvLeft * 6 / pv;
            string barPv = "PV";
            for (int j = 0; j < pvPerSix; j++)
            {
                barPv += "=";
            }
            for (int j = barPv.Length; j < 8; j++)
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
            Console.WriteLine();

            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset + 3, Console.CursorTop); // Afficher pv/pv
            for (int j = 3; j > pvLeft.ToString().Length; j--)
            {
                Console.Write( " ");
            }
            Console.Write(pvLeft + "/");
            for (int j = 3; j > pv.ToString().Length; j--)
            {
                Console.Write(" ");
            }
            Console.WriteLine(pv);
            Console.WriteLine();

            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset, Console.CursorTop); //Afficher Statut
            Console.WriteLine("STATUT/" + statusProblem);

            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + 1, Console.CursorTop); // Afficher No pokedex
            Console.Write("No"); 
            for (int i = id.ToString().Length; i < 3; i++)
            {
                Console.Write("0");
            }
            Console.Write(id);

            Console.SetCursorPosition(Console.CursorLeft + 2, Console.CursorTop); // Afficher Séparation
            Console.WriteLine("____________");



            //Deuxieme partie
            int offsetStats = 0;
            int offsetBox = 1;
            int offsetAfterBox = 10;

            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt, Console.CursorTop); // Aficher la box
            Console.WriteLine(hautBoxPage1);
            cursorXBox = Console.CursorLeft;
            cursorYBox = Console.CursorTop;

            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft + spaceStartInt, Console.CursorTop);
                Console.WriteLine(middleBoxPage1);
            }
            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt, Console.CursorTop); // Aficher la box
            Console.WriteLine(hautBoxPage1);

            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox, cursorYBox); // Afficher les noms des stats
            Console.Write("FOR");
            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox, cursorYBox + 2);
            Console.Write("DEF");
            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox, cursorYBox + 4);
            Console.Write("VIT");
            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox, cursorYBox + 6);
            Console.Write("SPE");

            
            if (atk.ToString().Length == 1) offsetStats = 2;
            else if (atk.ToString().Length == 2) offsetStats = 1;
            else if (atk.ToString().Length == 3) offsetStats = 0;
            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox + 5 + offsetStats, cursorYBox + 1); // Afficher ATK
            Console.Write(atk);

            if (def.ToString().Length == 1) offsetStats = 2;
            else if (def.ToString().Length == 2) offsetStats = 1;
            else if (def.ToString().Length == 3) offsetStats = 0;
            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox + 5 + offsetStats, cursorYBox + 3); // Afficher Def
            Console.Write(def);

            if (spd.ToString().Length == 1) offsetStats = 2;
            else if (spd.ToString().Length == 2) offsetStats = 1;
            else if (spd.ToString().Length == 3) offsetStats = 0;
            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox + 5 + offsetStats, cursorYBox + 5); // Afficher Spd
            Console.Write(spd);

            if (spe.ToString().Length == 1) offsetStats = 2;
            else if (spe.ToString().Length == 2) offsetStats = 1;
            else if (spe.ToString().Length == 3) offsetStats = 0;
            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox + 5 + offsetStats, cursorYBox + 7); // Afficher Dpe
            Console.Write(spe);

            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetAfterBox, cursorYBox); // Afficher Type1
            Console.Write("TYPE1/");

            Console.SetCursorPosition(cursorXBox + spaceStartInt +1  + offsetAfterBox, cursorYBox + 1);// Afficher Type1
            Console.Write(listType[0]);

            if (listType.Count == 2)
            {
                Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetAfterBox, cursorYBox + 2);// Afficher Type2
                Console.Write("TYPE2/");

                Console.SetCursorPosition(cursorXBox + spaceStartInt + 1 + offsetAfterBox, cursorYBox + 3);// Afficher Type2
                Console.Write(listType[1]);
            }

            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetAfterBox, cursorYBox + 4);// Afficher Id Player
            Console.Write("NoID/");

            Console.SetCursorPosition(cursorXBox + spaceStartInt + 2 + offsetAfterBox, cursorYBox + 5);// Afficher Id Player
            Console.Write(idOT);

            Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetAfterBox, cursorYBox + 6);// Afficher name Player
            Console.Write("OT/");

            Console.SetCursorPosition(cursorXBox + spaceStartInt + 2 + offsetAfterBox, cursorYBox + 7);// Afficher name Player
            Console.Write(nameOT);

            Console.SetCursorPosition(cursorXBox + spaceStartInt + 2 + offsetAfterBox, cursorYBox + 8);// Afficher Separation
            Console.WriteLine("_______");



            Console.ReadKey(true);

            // ---------------- Deuxieme fenetre ---------------- //
            //Premiere partie
            Console.SetCursorPosition(cursorXTop + spaceStartInt + pokemonOffset, cursorYTop + 1);
            Console.WriteLine("          ");
            Console.SetCursorPosition(cursorXTop + spaceStartInt + pokemonOffset, cursorYTop + 2);
            Console.WriteLine("          ");
            Console.SetCursorPosition(cursorXTop + spaceStartInt + pokemonOffset, cursorYTop + 3);
            Console.WriteLine("          ");
            Console.SetCursorPosition(cursorXTop + spaceStartInt + pokemonOffset, cursorYTop + 4);
            Console.WriteLine("          ");
            Console.SetCursorPosition(cursorXTop + spaceStartInt + pokemonOffset, cursorYTop + 5);
            Console.WriteLine("          ");


            Console.SetCursorPosition(cursorXTop + spaceStartInt + pokemonOffset + 2, cursorYTop+2);
            Console.WriteLine("PTS EXP.");

            int offSetxp = 10 - expActuel.ToString().Length;
            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset + offSetxp, Console.CursorTop);
            Console.WriteLine(expActuel);

            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset + 2, Console.CursorTop);
            Console.WriteLine("PTS EXP.");

            offSetxp = 5 - (expToLevelUp - expActuel).ToString().Length;
            string exp = "";
            if (level >= 100)
            {
                exp += level;
            }
            else
            {
                exp += "N" + (level +1);
            }
            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt + pokemonOffset + offSetxp, Console.CursorTop);
            Console.WriteLine((expToLevelUp - expActuel) + "> " + exp);


            offsetBox = 2;

            Console.SetCursorPosition(cursorXBox + spaceStartInt, cursorYBox - 1);
            Console.WriteLine(hautBoxPage2);
            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft + spaceStartInt, Console.CursorTop);
                Console.WriteLine(middleBoxPage2);
            }
            Console.SetCursorPosition(Console.CursorLeft + spaceStartInt, Console.CursorTop);
            Console.WriteLine(hautBoxPage2);

            for (int i = 0; i < listAttackActual.Count; i++)
            {
                Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox, cursorYBox + i *2);
                Console.Write(listAttackActual[i].name);

                Console.SetCursorPosition(cursorXBox + spaceStartInt + offsetBox + 9, cursorYBox + i*2 + 1);
                Console.Write("PP ");
                if (listAttackActual[i].ppLeft.ToString().Length == 1)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(listAttackActual[i].ppLeft + "/" + listAttackActual[i].pp);
            }


            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop+5);
            Console.ReadKey(true);
        }


        public void AfficherSprite()
        {
            AfficherSprite(color, asciiArt);
        }
        public void AfficherSprite(ConsoleColor color_, string asciiArt_)
        {
            Console.ForegroundColor = color_;
            Console.WriteLine(asciiArt_);
            Console.ForegroundColor = ConsoleColor.White;
        } // Utilisé dans l'évo
        

        public static string GetNom(int id)
        {
            string name = "";
            using (StreamReader sr = new StreamReader(AdresseFile.FileDirection + "CSV\\pokemon.csv"))
            {
                string line;
                bool pokemonFound = false;

                line = sr.ReadLine();
                line = sr.ReadLine();

                while ((line = sr.ReadLine()) != null && !pokemonFound)
                {
                    string[] colonnes = line.Split(',');

                    int.TryParse(colonnes[0], out int id_search);

                    if (id_search == id)
                    {
                        name = colonnes[1];
                        pokemonFound = true;
                    }
                }
            }
            return name;
        }




        public void Evolution()
        {
            if (this.listEvo.Count > 0)
            {
                string nextEvo = this.listEvo[0];
                string[] colonnes = nextEvo.Split(',');


                string old_name = name;
                string old_specie = actual_name;
                string old_sprite = asciiArt;
                ConsoleColor old_color = color;

                this.id = int.Parse(colonnes[0]);

                this.actual_name = colonnes[1];

                if (old_specie == old_name)
                {
                    name = actual_name;
                }

                // Sprite
                
            string asciiArtFileName = $"ascii-art ({id}).txt";
            string asciiArtBackFileName = $"back({id}).txt";

            asciiArt = GetSprite(asciiArtFileName, true);
            asciiArtBack = GetSprite(asciiArtBackFileName, false);



                this.listType[0] = colonnes[2];
                if (colonnes[3] != "NONE")
                {
                    if (listType.Count == 1)
                    {
                        this.listType.Add(colonnes[3]);
                    }
                    else
                    {
                        this.listType[1] = colonnes[3];
                    }
                }

                colorCSV = colonnes[18];
                ColorForegroundCheck();


                EvolutionAnimation(old_sprite, asciiArt, old_name, actual_name, old_color, color);

                this.listPv[0] = int.Parse(colonnes[4]);
                this.listAtk[0] = int.Parse(colonnes[5]);
                this.listDef[0] = int.Parse(colonnes[6]);
                this.listSpe[0] = int.Parse(colonnes[7]);
                this.listSpd[0] = int.Parse(colonnes[8]);

                int tempOldPv = pv;
                this.pv = FormulaStatsPv(this.level, this.listPv);
                this.atk = FormulaStatsNotPv(this.level, this.listAtk);
                this.def = FormulaStatsNotPv(this.level, this.listDef);
                this.spe = FormulaStatsNotPv(this.level, this.listSpe);
                this.spd = FormulaStatsNotPv(this.level, this.listSpd);

                pvLeft = pvLeft + (pv - tempOldPv);

                if (colonnes[9] == "FALSE")
                {
                    peutEvoluer = false;
                }
                else
                {
                    peutEvoluer = true;
                }

                if (peutEvoluer)
                {
                    methodeEvolution = int.Parse(colonnes[10]);

                    if (methodeEvolution == 0)
                    {
                        evolutionLevel = int.Parse(colonnes[11]);
                    }
                    else if (methodeEvolution == 2)
                    {

                        evolutionItemId.Clear();
                        evolutionItemId.Add(int.Parse(colonnes[11]));
                    }
                }

                expDonne = int.Parse(colonnes[13]);

                tauxCapture = int.Parse(colonnes[14]);

                listAttackId.Clear();
                listAttackLevel.Clear();

                string[] temp = colonnes[16].Split("/");
                listAttackId = temp.Select(int.Parse).ToList();

                temp = colonnes[17].Split("/");
                listAttackLevel = temp.Select(int.Parse).ToList();




                if (this.listEvo.Count > 1)
                {
                    for (int i = 0; i < this.listEvo.Count - 1; i++)
                    {
                        string temp2 = this.listEvo[i + 1];
                        this.listEvo[i] = temp2;
                    }
                }

                this.listEvo.RemoveAt(listEvo.Count - 1);
            }

            CheckNewCapacity();

            canEvolve = false;
        }
        public void LevelUp(bool expReset = false)
        {
            level++;
            
            int temp = 0;
            if (expCourbe == "rapide")
            {
                expToLevelUp = FormulaCourbeRapide(level + 1);
                temp = FormulaCourbeRapide(level);
            }
            else if (expCourbe == "moyenne")
            {
                expToLevelUp = FormulaCourbeMoyenne(level + 1);
                temp = FormulaCourbeRapide(level);
            }
            else if (expCourbe == "parabolique")
            {
                expToLevelUp = FormulaCourbePara(level+1);
                temp = FormulaCourbePara(level);
            }
            else if (expCourbe == "lente")
            {
                expToLevelUp = FormulaCourbeLente(level + 1);
                temp = FormulaCourbeLente(level);
            }

            if (level == 100 || expReset)
            {
                expActuel = temp;
            }


            int tempOldPv = pv;
            this.pv = FormulaStatsPv(this.level, this.listPv);
            this.atk = FormulaStatsNotPv(this.level, this.listAtk);
            this.def = FormulaStatsNotPv(this.level, this.listDef);
            this.spe = FormulaStatsNotPv(this.level, this.listSpe);
            this.spd = FormulaStatsNotPv(this.level, this.listSpd);

            pvLeft = pvLeft + (pv - tempOldPv);
            CheckNewCapacity();
            
            if (level >= evolutionLevel)
            {
                canEvolve = true;
            }
        }
        private void CheckNewCapacity()
        {
            for (int i = 0; i < listAttackLevel.Count; i++)
            {
                
                if (level == listAttackLevel[i])
                {
                    Capacity cap = new Capacity(listAttackId[i]);

                    bool AttackAlreadyLearned = false;
                    foreach(Capacity capacity in listAttackActual)
                    {
                        if (capacity.id == cap.id)
                        {
                            AttackAlreadyLearned = true;
                        }
                    }

                    if (!AttackAlreadyLearned)
                    {
                        if (listAttackActual.Count < 4)
                        {
                            listAttackActual.Add(cap);
                            Console.WriteLine($"{name} a appris {cap.name}.");
                        }
                        else
                        {
                            bool choiceFinished = false;
                            bool loop2Question = true;
                            while (loop2Question)
                            {
                                bool loop1Question = true;
                                while (loop1Question)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($"Votre POKEMON souhaite apprendre l'attaque {cap.name}.");
                                    Console.WriteLine("Mais votre POKEMON ne peut plus rien apprendre");
                                    Console.WriteLine($"Voulez vous remplacer une attaque pour apprendre l'attaque {cap.name} ?");
                                    Console.WriteLine("[OUI] [NON]");
                                    string userInput2 = Console.ReadLine();

                                    if (string.Equals(userInput2, "oui", StringComparison.OrdinalIgnoreCase))
                                    {
                                        bool loopQuestion3 = true;
                                        Console.Clear();
                                        Console.WriteLine("Quelle capacite souhaitez vous remplacer ?");
                                        while (loop1Question)
                                        {
                                            foreach (Capacity capacity in listAttackActual)
                                            {
                                                Console.WriteLine(capacity.name);
                                            }
                                            string userInput3 = Console.ReadLine();

                                            if (string.Equals(userInput3, listAttackActual[0].name, StringComparison.OrdinalIgnoreCase))
                                            {
                                                Console.WriteLine("1... 2... 3... Tada !");
                                                Console.WriteLine($"{name} a oublie {listAttackActual[0].name}...");
                                                Console.WriteLine($"Et il a appris {cap.name}.");

                                                listAttackActual[0] = cap;
                                                loopQuestion3 = false;
                                                loop1Question = false;
                                                loop2Question = false;
                                                choiceFinished = true;
                                            }
                                            else if (string.Equals(userInput3, listAttackActual[1].name, StringComparison.OrdinalIgnoreCase))
                                            {
                                                Console.WriteLine("1... 2... 3... Tada !");
                                                Console.WriteLine($"{name} a oublie {listAttackActual[1].name}...");
                                                Console.WriteLine($"Et il a appris {cap.name}.");

                                                listAttackActual[1] = cap;
                                                loopQuestion3 = false;
                                                loop1Question = false;
                                                loop2Question = false;
                                                choiceFinished = true;
                                            }
                                            else if (string.Equals(userInput3, listAttackActual[2].name, StringComparison.OrdinalIgnoreCase))
                                            {
                                                Console.WriteLine("1... 2... 3... Tada !");
                                                Console.WriteLine($"{name} a oublie {listAttackActual[2].name}...");
                                                Console.WriteLine($"Et il a appris {cap.name}.");

                                                listAttackActual[2] = cap;
                                                loopQuestion3 = false;
                                                loop1Question = false;
                                                loop2Question = false;
                                                choiceFinished = true;
                                            }
                                            else if (string.Equals(userInput3, listAttackActual[3].name, StringComparison.OrdinalIgnoreCase))
                                            {
                                                Console.WriteLine("1... 2... 3... Tada !");
                                                Console.WriteLine($"{name} a oublie {listAttackActual[3].name}...");
                                                Console.WriteLine($"Et il a appris {cap.name}.");

                                                listAttackActual[3] = cap;
                                                loopQuestion3 = false;
                                                loop1Question = false;
                                                loop2Question = false;
                                                choiceFinished = true;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Reponse invalide. Veuillez repondre par 'oui' ou 'non'.");
                                            }


                                        }

                                    }
                                    else if (string.Equals(userInput2, "non", StringComparison.OrdinalIgnoreCase))
                                    {
                                        loop1Question = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Reponse invalide. Veuillez repondre par 'oui' ou 'non'.");
                                    }
                                }

                                if (!choiceFinished)
                                {
                                    Console.Clear();
                                    Console.WriteLine($"Voulez vous vraiment renoncer a apprendre {cap.name} ?");
                                    Console.WriteLine("[OUI] [NON]");
                                    string userInput = Console.ReadLine();

                                    if (string.Equals(userInput, "oui", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Console.WriteLine($"Vous n'avez pas appris {cap.name}");
                                        loop2Question = false;
                                    }
                                    else if (string.Equals(userInput, "non", StringComparison.OrdinalIgnoreCase))
                                    {
                                        loop1Question = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Reponse invalide. Veuillez repondre par 'oui' ou 'non'.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine();
        }
        public void GainExp(int expGained)
        {
            expActuel += expGained;
            
            if (expActuel >= expToLevelUp)
            {
                LevelUp();
            }


        }
        public void GainEV(int evHp, int evAtk, int evDef, int evSpe, int evSpd)
        {
            if(listPv[2] == maxEv)
            {
                listPv[2] += evHp;
                if (listPv[2] > maxEv)
                {
                    listPv[2] = maxEv;
                }
            }
            else if (listAtk[2] == maxEv)
            {
                listAtk[2] += evAtk;
                if (listAtk[2] > maxEv)
                {
                    listAtk[2] = maxEv;
                }
            }
            else if (listDef[2] == maxEv)
            {
                listDef[2] += evDef;
                if (listDef[2] > maxEv)
                {
                    listDef[2] = maxEv;
                }
            }
            else if (listSpe[2] == maxEv)
            {
                listSpe[2] += evSpe;
                if (listSpe[2] > maxEv)
                {
                    listSpe[2] = maxEv;
                }
            }
            else if (listSpd[2] == maxEv)
            {
                listSpd[2] += evSpd;
                if (listSpd[2] > maxEv)
                {
                    listSpd[2] = maxEv;
                }
            }
        }




        private string GetSprite(string filedirection, bool isnotBack) {

string returnValue = "";

filedirection =  Path.Combine(AdresseFile.FileDirection,"Assets\\Sprites\\Pokemon", filedirection);

            if (File.Exists(filedirection))
            {
                returnValue = File.ReadAllText(filedirection);
                string[] temp = returnValue.Split('\n');
                if(isnotBack)
                {

                    this.width = temp[0].Length;
                }
                else
                {
                    this.widthBack = temp[0].Length;
                }
            }
            return returnValue;            }


        private void ColorForegroundCheck()
        {
            if (colorCSV == "White")
            {
                color = ConsoleColor.White;
            }
            else if (colorCSV == "DarkBlue")
            {
                color = ConsoleColor.DarkBlue;
            }
            else if (colorCSV == "DarkGreen")
            {
                color = ConsoleColor.DarkGreen;
            }
            else if (colorCSV == "DarkCyan")
            {
                color = ConsoleColor.DarkCyan;
            }
            else if (colorCSV == "DarkRed")
            {
                color = ConsoleColor.DarkRed;
            }
            else if (colorCSV == "DarkMagenta")
            {
                color = ConsoleColor.DarkMagenta;
            }
            else if (colorCSV == "DarkYellow")
            {
                color = ConsoleColor.DarkYellow;
            }
            else if (colorCSV == "Gray")
            {
                color = ConsoleColor.Gray;
            }
            else if (colorCSV == "DarkGray")
            {
                color = ConsoleColor.DarkGray;
            }
            else if (colorCSV == "Blue")
            {
                color = ConsoleColor.Blue;
            }
            else if (colorCSV == "Green")
            {
                color = ConsoleColor.Green;
            }
            else if (colorCSV == "Cyan")
            {
                color = ConsoleColor.Cyan;
            }
            else if (colorCSV == "Red")
            {
                color = ConsoleColor.Red;
            }
            else if (colorCSV == "Magenta")
            {
                color = ConsoleColor.Magenta;
            }
            else if (colorCSV == "Yellow")
            {
                color = ConsoleColor.Yellow;
            }
        }
        private void EvolutionAnimation(string sprite_oldPokemon, string sprite_newPokemon, string name_oldPokemon, string name_newPokemon, ConsoleColor color_oldPokemon, ConsoleColor color_newPokemon)
        {
            Functions.playSound("evolution.wav");
            int first_time = 750;
            int second_time = 250;

            int next_pokemon = 125;

            bool evolved = false;
            int timesSwitch = 0;

            while(!evolved)
            {
                while (timesSwitch < 3) 
                {
                    Console.Clear();
                    Console.WriteLine("Quoi ?");
                    Console.WriteLine($"{name_oldPokemon} evolue !");

                    AfficherSprite(color_oldPokemon, sprite_oldPokemon);
                    Thread.Sleep((first_time));
                    Functions.ClearInputBuffer();

                    Console.Clear();
                    Console.WriteLine("Quoi ?");
                    Console.WriteLine($"{name_oldPokemon} evolue !");

                    AfficherSprite(color_newPokemon, sprite_newPokemon);   
                    Thread.Sleep((next_pokemon));
                    Functions.ClearInputBuffer();

                    timesSwitch++;
                }

                while (timesSwitch < 6) 
                {
                    Console.Clear();
                    Console.WriteLine("Quoi ?");
                    Console.WriteLine($"{name_oldPokemon} evolue !");

                    AfficherSprite(color_oldPokemon, sprite_oldPokemon);
                    Thread.Sleep((second_time));
                    Functions.ClearInputBuffer();

                    Console.Clear();
                    Console.WriteLine("Quoi ?");
                    Console.WriteLine($"{name_oldPokemon} evolue !");

                    AfficherSprite(color_newPokemon, sprite_newPokemon);
                    Thread.Sleep((next_pokemon));
                    Functions.ClearInputBuffer();

                    timesSwitch++;
                }



                while (timesSwitch < 6) 
                {
                    Console.Clear();
                    Console.WriteLine("Quoi ?");
                    Console.WriteLine($"{name_oldPokemon} evolue !");

                    AfficherSprite(color_oldPokemon, sprite_oldPokemon);
                    Thread.Sleep((second_time));
                    Functions.ClearInputBuffer();

                    Console.Clear();
                    Console.WriteLine("Quoi ?");
                    Console.WriteLine($"{name_oldPokemon} evolue !");

                    AfficherSprite(color_newPokemon, sprite_newPokemon);
                    Thread.Sleep((next_pokemon));
                    Functions.ClearInputBuffer();

                    timesSwitch++;
                }
                Functions.playSound("tada.wav");
                Console.WriteLine($"Felicitations ! Votre {name_oldPokemon} a evolue en {name_newPokemon} ! ");
                Thread.Sleep(3000);
                Functions.playSound("victoire_sauvage.wav");
                Console.ReadLine();
                Console.Clear();
                evolved = true;
            }

        }


        static public void Heal(Player player)
        {
            foreach (Pokemon pokemon in player.pokemonParty)
            {
                pokemon.pvLeft = pokemon.pv;
                foreach (Capacity cap in pokemon.listAttackActual)
                {
                    cap.ppLeft = cap.pp;
                }
            }
        }

        



        // ------------------ Formules ------------------ //
        private int FormulaStatsPv(int level, List<int> listPv)
        {

            return (int)(((((listPv[0] + listPv[1]) * 2 + Math.Sqrt(listPv[2]) / 4) * level) / 100) + level + 10);
        }
        private int FormulaStatsNotPv(int level, List<int> listStat)
        {
            return (int)(((((listStat[0] + listStat[1]) * 2 + Math.Sqrt(listStat[2]) / 4) * level) / 100) + 5);
        }

        private int FormulaCourbeRapide(int level)
        {
            double result = 0.8 * (level * level * level);
            return (int)Math.Round(result);
        }
        private int FormulaCourbeMoyenne(int level) 
        {
            return (level * level * level);
        }
        private int FormulaCourbePara(int level)
        {
            double result = (1.2 * (level * level * level)) - (15 * (level * level)) + (100 * level) - 140;
            return (int)Math.Round(result);
        }
        private int FormulaCourbeLente(int level)
        {
            double result = 1.25 * (level * level * level);
            return (int)Math.Round(result);
        }
    }
}