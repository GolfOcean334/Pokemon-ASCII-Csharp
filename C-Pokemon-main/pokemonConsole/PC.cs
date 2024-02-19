using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonConsole
{
    public class PC : Entity
    {
        static private List<Pokemon> box1 = new List<Pokemon>();


        public PC() : base("PC", 1, 1, 'P', "bedroom.txt", 'P')
        {

        }

        public override void Function(Player player)
        {
            Console.Clear();
            foreach (Pokemon pokemon in box1)
            {
                Console.WriteLine(pokemon.name + " | " + pokemon.level);
            }
            Map.PrintDialogue("En cours de construction");
        }






        static public void AddPokemonBox(Pokemon pokemon)
        {
            box1.Add(pokemon);
        }

        static public void RemovePokemonBox(Pokemon pokemon, Player player)
        {
            box1.Remove(pokemon);
            player.addPokemonToParty(pokemon);
        }
    }
}
