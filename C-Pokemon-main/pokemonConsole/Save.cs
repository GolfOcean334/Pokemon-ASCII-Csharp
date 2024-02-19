using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Usefull;

namespace pokemonConsole
{
    internal class Save
    {
        private static string filePath = AdresseFile.FileDirection + "save.txt";


        public static void Saving(Player player, Rival rival)
        {
            if (!File.Exists(filePath))
            {
                // Crée le fichier s'il n'existe pas
                using (StreamWriter createFileWriter = File.CreateText(filePath))
                {
                    Console.WriteLine($"Le fichier {filePath} a été créé.");
                }
            }
            File.WriteAllText(filePath, string.Empty);

            using (StreamWriter writer = File.AppendText(filePath))
            {
                // ------------- Player ------------- //
                writer.WriteLine("Player :");
                writer.WriteLine($"{player.name},{player.id},{player.PositionX},{player.PositionY},{player.map},{player.actuallPositionChar},{player.starterId}");

                // ------------- Pokemon ------------- //
                writer.WriteLine(player.pokemonParty.Count);
                foreach (Pokemon pokemon in player.pokemonParty)
                {
                    writer.WriteLine(pokemon.listAttackActual.Count);
                    foreach (Capacity capacity in pokemon.listAttackActual)
                    {
                        writer.WriteLine($"{capacity.id},{capacity.pp},{capacity.ppLeft}");
                    }
                    writer.WriteLine($"{pokemon.id},{pokemon.name},{pokemon.idOT},{pokemon.nameOT},{pokemon.level},{pokemon.expActuel},{pokemon.pvLeft}");
                    writer.WriteLine($"{pokemon.listPv[1]},{pokemon.listPv[2]},{pokemon.listAtk[1]},{pokemon.listAtk[2]},{pokemon.listDef[1]},{pokemon.listDef[2]},{pokemon.listSpe[1]},{pokemon.listSpe[2]},{pokemon.listSpd[1]},{pokemon.listSpd[2]}");
                    writer.WriteLine($"{pokemon.statusProblem},{pokemon.ko},{pokemon.echange}");
                }

                // ------------- Inventaire ------------- //
                writer.WriteLine(player.inventory.Count);
                foreach (Item item in player.inventory)
                {
                    writer.WriteLine($"{item.id},{item.quantity}");
                }

                // ------------- Rival ------------- //
                writer.WriteLine("Rival :");
                writer.WriteLine($"{rival.name}");
            }
        }


        public static void Loading(Player player, Rival rival)
        {
            int pokemonListCount;
            int pokemonAttackCount;
            int itemCount;
            List<Capacity> pokemonCapacity = new List<Capacity>();


            int pokemonId;
            string pokemonName;
            int idOT;
            string nameOT;
            int levelPokemon;
            int expActuel;
            int pvLeft_;
            int dvHp;
            int evHp;
            int dvAtk;
            int evAtk;
            int dvDef;
            int evDef;
            int dvSpe;
            int evSpe;
            int dvSpd;
            int evSpd;
            string statusProblemPokemon;
            bool ko;
            bool echange;



            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                sr.ReadLine();

                line = sr.ReadLine();
                string[] colonnes = line.Split(',');


                // ------------- Player ------------- //
                player.name = colonnes[0];
                player.id = int.Parse(colonnes[1]);
                player.PositionX = int.Parse(colonnes[2]);
                player.PositionY = int.Parse(colonnes[3]);
                player.map = colonnes[4];
                player.actuallPositionChar = colonnes[5][0];

                try
                {
                    player.starterId = int.Parse(colonnes[6]);
                }
                catch
                {
                    player.starterId = null;
                }

                line = sr.ReadLine();

                // ------------- Pokemon ------------- //
                pokemonListCount = int.Parse(line);

                for (int i = 0; i < pokemonListCount; i++)
                {
                    pokemonCapacity.Clear();


                    line = sr.ReadLine();
                    pokemonAttackCount = int.Parse(line);

                    for(int j = 0; j < pokemonAttackCount; j++)
                    {
                        line = sr.ReadLine();
                        colonnes = line.Split(',');

                        Capacity capacity = new Capacity(int.Parse(colonnes[0]), int.Parse(colonnes[1]), int.Parse(colonnes[2]));
                        pokemonCapacity.Add(capacity);
                    }
                    line = sr.ReadLine();
                    colonnes = line.Split(',');

                    pokemonId = int.Parse(colonnes[0]);
                    pokemonName = colonnes[1];
                    idOT = int.Parse(colonnes[2]);
                    nameOT = colonnes[3];
                    levelPokemon = int.Parse(colonnes[4]);
                    expActuel = int.Parse(colonnes[5]);
                    pvLeft_ = int.Parse(colonnes[6]);

                    line = sr.ReadLine();
                    colonnes = line.Split(',');

                    dvHp = int.Parse(colonnes[0]);
                    evHp = int.Parse(colonnes[1]);
                    dvAtk = int.Parse(colonnes[2]);
                    evAtk = int.Parse(colonnes[3]);
                    dvDef = int.Parse(colonnes[4]);
                    evDef = int.Parse(colonnes[5]);
                    dvSpe = int.Parse(colonnes[6]);
                    evSpe = int.Parse(colonnes[7]);
                    dvSpd = int.Parse(colonnes[8]);
                    evSpd = int.Parse(colonnes[9]);

                    line = sr.ReadLine();
                    colonnes = line.Split(',');

                    statusProblemPokemon = colonnes[0];
                    ko = bool.Parse(colonnes[1]);
                    echange = bool.Parse(colonnes[2]);

                    Pokemon pokemon = new Pokemon(pokemonId, pokemonName, idOT, nameOT, levelPokemon, expActuel, pvLeft_, dvHp, evHp, dvAtk, evAtk, dvDef, evDef, dvSpe, evSpe, dvSpd, evSpd, statusProblemPokemon, ko, echange, pokemonCapacity, player.id);
                    player.addPokemonToParty(pokemon);
                }

                // ------------- Inventaire ------------- //
                itemCount = int.Parse(sr.ReadLine());
                for (int i = 0; i < itemCount; i++)
                {
                    line = sr.ReadLine();
                    colonnes = line.Split(',');

                    player.addItemToInventory(int.Parse(colonnes[0]), int.Parse(colonnes[1]));
                }


                // ------------- Rival ------------- //
                sr.ReadLine();
                rival.name = sr.ReadLine();
            }
        }
    }
}
