using System;

namespace pokemonConsole
{
    class Rival
    {
        public string name { get; set; }
        public bool ia_difficult { get; set; }
        public List<Pokemon> pokemonParty = new List<Pokemon>();

        public Rival()
        {
             name = "Rival";
        }

        public void addPokemonToParty(Player player)
        {
            pokemonParty.Clear();
            ia_difficult = true;
            if (pokemonParty.Count <= 6)
            {
                pokemonParty.Add(new Pokemon(18,61,2));
                pokemonParty.Add(new Pokemon(65,59, 2));
                pokemonParty.Add(new Pokemon(112,61, 2));
                if (player.starterId==1)
                {
                    pokemonParty.Add(new Pokemon(103, 61, 2));
                    pokemonParty.Add(new Pokemon(130, 63, 2));
                    pokemonParty.Add(new Pokemon(6, 65, 2));
                }
                else if (player.starterId == 4)
                {
                    pokemonParty.Add(new Pokemon(103, 61, 2));
                    pokemonParty.Add(new Pokemon(59, 61, 2));
                    pokemonParty.Add(new Pokemon(9, 65, 2));
                }
                else if (player.starterId == 7)
                {
                    pokemonParty.Add(new Pokemon(59, 63, 2));
                    pokemonParty.Add(new Pokemon(130, 61, 2));
                    pokemonParty.Add(new Pokemon(3, 65, 2));
                }
            }
        }
    }
}

