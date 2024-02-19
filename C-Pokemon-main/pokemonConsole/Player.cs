using System;

namespace pokemonConsole
{
    public class Player : Entity
    {
        public int id { get; set; }

        public List<Pokemon> pokemonParty = new List<Pokemon>();
        public List<Item> inventory = new List<Item>();

        public int? starterId;

        public bool caughtPokemon { get; set; } = false;
        public bool aVaincuBlue { get; set; } = false;


        public Player() : base("Player", 8, 8, 'P', "bedroom.txt", ' ')
        {
            Random random = new Random();
            id = random.Next(1, 65536);
            starterId = null;
        }

        public bool IsKO()
        {
            foreach (Pokemon p in pokemonParty)
            {
                if (p.pvLeft > 0)
                {
                    return false; // If any Pokemon is not knocked out, return false
                }
            }
            return true; // If all Pokemon are knocked out, return true
        }

        public void addPokemonToParty(Pokemon pokemon)
        {
            if (pokemonParty.Count <= 6)
            {
                pokemonParty.Add(pokemon);
            }
            else
            {
                PC.AddPokemonBox(pokemon);
            }
        }
        
        public void addItemToInventory(int item_id, int quantity = 1)
        {
            bool itemInInv = false;

            foreach (Item item in inventory)
            {
                if(item.id == item_id)
                {
                    item.quantity += quantity;
                    itemInInv = true;
                }
            }
            if (!itemInInv) 
            {
                inventory.Add(new Item(item_id, quantity));
            }
        }


        public static bool catchPokemon(Pokemon pokemon, Player player, int pokeballCatchRate, Pokemon pokemonMine)
        {
            Random random = new Random();

            float chanceCapture;
            float randomChanceCapture;
            float statusProbleme;
            float tauxCapture;

            if (pokemon.statusProblem == "BRN" || pokemon.statusProblem == "PAR" || pokemon.statusProblem == "PSN" || pokemon.statusProblem == "PSNGRAVE")
            {
                statusProbleme = 1.5f;
            }
            else if (pokemon.statusProblem == "FRZ" || pokemon.statusProblem == "SLP")
            {
                statusProbleme = 2f;
            }
            else
            {
                statusProbleme = 1f;
            }

            tauxCapture = 0.4f * pokemon.tauxCapture * statusProbleme * pokeballCatchRate;
            if (tauxCapture > 255) tauxCapture = 255;

            chanceCapture = tauxCapture / 255 * 100;
            randomChanceCapture = random.Next(0, 101);
            if (randomChanceCapture <= chanceCapture)
            {
                Combat.PrintPokemon(pokemonMine, pokemon);
                Combat.PrintMenuEmpty();

                Combat.PrintInEmptyMenu($"Vous avez capturé {pokemon.name} !");
                pokemon.idOT = player.id;
                pokemon.nameOT = player.name;
                pokemon.appartenant = 1;
                player.addPokemonToParty(pokemon);
                return true;
            }
            else
            {
                Combat.PrintInEmptyMenu($"{pokemon.name} s'est libéré !");
            }
            return false;
        }
    }
}

