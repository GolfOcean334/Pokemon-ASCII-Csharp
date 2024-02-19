using pokemonConsole;
using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Usefull;

public class Entity
{
    public string name { get; set; }

    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public char sprite { get; set; }

    public string map { get; set; }
    public char actuallPositionChar { get; set;  }



    public virtual void Function(Player player) { }

    protected Entity(string name, int positionX, int positionY, char sprite, string map, char actuallPositionChar)
    {
        this.name = name;
        PositionX = positionX;
        PositionY = positionY;
        this.sprite = sprite;
        this.map = map;
        this.actuallPositionChar = actuallPositionChar;
    }
}

class Pokeball : Entity
{
    public int id_pokemon;
    public bool taken;
    public int position;

    public Pokeball(int id_, string map_, int x, int y, char actualPosition, int position) : base(Pokemon.GetNom(id_), x, y, 'o', map_, actualPosition)
    {
        id_pokemon = id_;
        taken = false;
        this.position = position;
    }

    public override void Function(Player player) 
    {
        player.addPokemonToParty(new Pokemon(id_pokemon, 5, 1, player.id, player.id, player.name));
        player.starterId = id_pokemon;
        taken = true;
    }
}

