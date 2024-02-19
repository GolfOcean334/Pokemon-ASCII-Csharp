
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usefull;

namespace pokemonConsole
{
    class NPC : Entity
    {
        public string dialogue { get; set; }
        public bool updated;

        public NPC(string name_, string dialogue_, char sprite_, string map_, int positionX_, int positionY_, char actualTile) : base (name_, positionX_, positionY_, sprite_, map_, actualTile)
        {
            dialogue = dialogue_;
            updated = false;
        }

        public virtual void Update(DateTime deltatime, Player player)
        {

        }
    }

    class Maman : NPC
    {
        public Maman() : base("Maman", "Bonjour mon fils ! Bien dormi ?", 'M', "mom.txt", 7, 4, ' ')
        {

        }

        public override void Function(Player player)
        {
            Pokemon.Heal(player);
        }
    }

    class Nina : NPC
    {
        public Nina() : base("Nina", "Va botter les fesses de mon frère au labo !", 'N', "blue.txt", 7, 4, ' ')
        {

        }

        public override void Function(Player player)
        {
            Pokemon.Heal(player);
        }
    }
    class RivalNPC :NPC
    {
        public Rival rival { get; private set; } = new Rival();
        public RivalNPC (Rival rival) : base(rival.name, "Yo minable !", 'R', "chen.txt", 4, 4, ' ')
        {
            rival = this.rival;
        }
        public override void Function(Player player)
        {
            if(player.starterId != null)
            {
                Functions.playSound("combat_trainer.wav");
                Combat.LoopCombat(player, rival.pokemonParty);
                Functions.playSound("chen.wav");
            }
            else
            {
                Map.PrintDialogue("Va chercher un pokemon minable");
            }
        }
    }
    class PotionMan : NPC
    {
        private bool alreadyGave = false;
        public PotionMan() : base("PotionMan", "Tiens ! Une nouvelle Potion !", 'E', "route_1.txt", 3, 24, ' ') { }

        public override void Update(DateTime deltatime, Player player)
        {
            Random random = new Random();

            DateTime endTime = DateTime.Now;

            if ((endTime - deltatime).TotalMilliseconds > 2000)
            {
                bool movedChose = false;
                while (!movedChose)
                {
                    int direction = random.Next(1, 5);
                    switch(direction)
                    {
                        case 1: // haut
                            if (PositionY > 24 && !(PositionX == player.PositionX && PositionY-1 == player.PositionY))
                            {
                                PositionY--;
                                movedChose = true;
                            }
                            break;
                        case 2: // bas
                            if (PositionY < 21 && !(PositionX == player.PositionX && PositionY + 1 == player.PositionY))
                            {
                                PositionY++;
                                movedChose = true;
                            }
                            break;
                        case 3: // gauche
                            if (PositionX > 2 && !(PositionX - 1 == player.PositionX && PositionY == player.PositionY))
                            {
                                PositionX--;
                                movedChose = true;
                            }
                            break;
                        case 4: // droite
                            if (PositionX < 5 && !(PositionX + 1 == player.PositionX && PositionY == player.PositionY))
                            {
                                PositionX++;
                                movedChose = true;
                            }
                            break;
                    }
                }
                updated = true;
            }
        }

        public override void Function(Player player)
        {
            player.addItemToInventory(5);
            alreadyGave = true;
        }
    }

    
}
