using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usefull;

namespace pokemonConsole
{
    public class Capacity
    {

        public int id { get; private set; }
        public string name { get; private set; }
        public SecondaryEffect secondaryEffect { get; set; }
        public string type { get; private set; }
        public int categorie { get; private set; }

        public int puissance { get; private set; }
        public string precision { get; private set; }
        public int pp;
        public int ppLeft;

        public int ppOriginal { get; private set;}
        public int ppMax { get; private set; }

        private string sideEffect;
        private string sideEffectInfo;



        private string fileCSV = AdresseFile.FileDirection + "CSV\\capacites.csv";

        //lis le CSV
        public Capacity(int id_) 
        {
            using (StreamReader sr = new StreamReader(this.fileCSV))
            {
                string line;
                bool AttackFound = false;

                line = sr.ReadLine();
                line = sr.ReadLine();

                while ((line = sr.ReadLine()) != null && !AttackFound)
                {
                    string[] colonnes = line.Split(',');

                    int.TryParse(colonnes[0], out int id_search);

                    if (id_search == id_)
                    {
                        id = id_;
                        name = colonnes[1];
                        type = colonnes[2];
                        categorie = int.Parse(colonnes[3]);

                        puissance = int.Parse(colonnes[4]);
                        precision = colonnes[5];
                        pp = int.Parse(colonnes[6]);
                        ppOriginal = pp;
                        ppMax = int.Parse(colonnes[7]);

                        sideEffect = colonnes[8];
                        sideEffectInfo = colonnes[9];


                        ppLeft = pp;
                    }
                }
            }
        }
        //lis la save
        public Capacity(int id_, int pp_, int ppLeft_)
        {
            using (StreamReader sr = new StreamReader(this.fileCSV))
            {
                string line;
                bool AttackFound = false;

                line = sr.ReadLine();
                line = sr.ReadLine();

                while ((line = sr.ReadLine()) != null && !AttackFound)
                {
                    string[] colonnes = line.Split(',');

                    int.TryParse(colonnes[0], out int id_search);

                    if (id_search == id_)
                    {
                        id = id_;
                        name = colonnes[1];
                        type = colonnes[2];
                        categorie = int.Parse(colonnes[3]);

                        puissance = int.Parse(colonnes[4]);
                        precision = colonnes[5];
                        pp = pp_;
                        ppMax = int.Parse(colonnes[7]);

                        sideEffect = colonnes[8];
                        sideEffectInfo = colonnes[9];


                        ppLeft = ppLeft_;
                    }
                }
            }
        }
        public enum SecondaryEffect
        {
            None,
            AtkUp,
            DefUp,
            SpeedUp,
            SpeUp,
            EsquiveUp,
            CritUp,
            CritBoost,
            AtkDown,
            ChanceAtkDown,
            DefDown,
            ChanceDefDown,
            SpeedDown,
            ChanceSpeedDown,
            SpeDown,
            PrecisionDown,
            Para,
            Burn,
            Freeze,
            Sleep,
            SelfSleep,
            Poison,
            PoisonGrave,
            Confusion,
            randx,
            Moneyx2,
            OHKO,
            Charge,
            Damagex2,
            EndFight,
            Imun,
            Stuck,
            randt,
            Flinch,
            doublex,
            doublet,
            Fail,
            StuckSelf,
            Recoil,
            ConstDamage,
            BlockAttack,
            BlockStatusMoves,
            Pause,
            Counter,
            VarDamage,
            VarDamageLevel,
            StealHP,
            ConstantStealHP,
            StealHPIfSleep,
            Heal,
            AlwaysCrit,
            Priority,
            CopyLastAttack,
            cinqt,
            ResetStats,
            CounterPatience,
            RandomAttack,
            CopyLastAttackTarget,
            Death,
            TransformIntoEnemy,
            ChangeTypeToEnemy,
            Clone,
        }
        public void Use(Pokemon pokemon, Pokemon pokemonAdverse)
        {
            ApplyEffect(sideEffect, pokemon, pokemonAdverse);
        }

        public static void ApplyStatusEffects(Pokemon pokemon)
        {
            switch (pokemon.statusProblem)
            {
                case "PARA":
                    Combat.PrintInEmptyMenu($"{pokemon.name} est paralysé !");
                    // Appliquer l'effet de statut de la paralysie
                    break;

                case "BURN":
                    Combat.PrintInEmptyMenu($"{pokemon.name} est brûlé !");
                    pokemon.pvLeft -= pokemon.pv / 16;
                    // pokemon.atkCombat = pokemon.atkCombat * 88 / 100;
                    break;

                case "FREEZE":
                    Combat.PrintInEmptyMenu($"{pokemon.name} est gelé !");
                    // Appliquer l'effet de statut du gel
                    break;

                case "SLEEP":
                    Combat.PrintInEmptyMenu($"{pokemon.name} est endormi !");
                    // Appliquer l'effet de statut du sommeil
                    break;

                case "POISON":
                    Combat.PrintInEmptyMenu($"{pokemon.name} est empoisonné !");
                    pokemon.pvLeft -= pokemon.pv / 16;
                    break;

                case "POISONGRAVE":
                    Combat.PrintInEmptyMenu($"{pokemon.name} est gravement empoisonné !");
                    // Appliquer l'effet de statut de l'empoisonnement grave
                    break;

                default:
                    break;
            }
        }

        public SecondaryEffect ApplyEffect(string sideEffect, Pokemon pokemon, Pokemon pokemonAdverse)
        {

            Random random = new Random();
            int randomNumber = random.Next(100);
            // Ajoutez des cas pour chaque effet possible
            switch (sideEffect.ToLower())
            {
                case "atkup":
                    Combat.PrintInEmptyMenu($"L'attaque de {pokemon.name} a augmenté !");
                    pokemon.atkCombat = (int)(pokemon.atkCombat * 1.5);
                    return SecondaryEffect.AtkUp;

                case "defup":
                    Combat.PrintInEmptyMenu($"La défense de {pokemon.name} a augmenté !");
                    pokemon.defCombat = (int)(pokemon.defCombat * 1.5);
                    return SecondaryEffect.DefUp;

                case "speedup":
                    Combat.PrintInEmptyMenu($"La vitesse de {pokemon.name} a augmenté !");
                    pokemon.spdCombat = (int)(pokemon.spdCombat * 1.5);
                    return SecondaryEffect.SpeedUp;

                case "speup":
                    Combat.PrintInEmptyMenu($"Le special de {pokemon.name} a augmenté !");
                    pokemon.speCombat = (int)(pokemon.speCombat * 1.5);
                    return SecondaryEffect.SpeUp;

                case "esquiveup":
                    Combat.PrintInEmptyMenu("esquiveup");
                    //==============================//
                    return SecondaryEffect.EsquiveUp;

                case "critup":
                    Combat.PrintInEmptyMenu("critup");
                    //==============================//
                    return SecondaryEffect.CritUp;

                case "critboost":
                    Combat.PrintInEmptyMenu("critboost");
                    //==============================//
                    return SecondaryEffect.CritBoost;

                case "atkdown":
                    Combat.PrintInEmptyMenu($"L'attaque de {pokemonAdverse.name} a baissé !");
                    pokemonAdverse.atkCombat = (int)(pokemonAdverse.atkCombat * 0.88);
                    return SecondaryEffect.AtkDown;

                case "chanceatkdown":
                    if (randomNumber <= 10)
                    {
                        Combat.PrintInEmptyMenu($"L'attaque de {pokemonAdverse.name} a baissé !");
                        pokemonAdverse.atkCombat = (int)(pokemonAdverse.atkCombat * 0.88);
                    }
                        return SecondaryEffect.ChanceAtkDown;

                case "defdown":
                    Combat.PrintInEmptyMenu($"La défense de {pokemonAdverse.name} a baissé !");
                    pokemonAdverse.defCombat = (int)(pokemonAdverse.defCombat * 0.88);
                    return SecondaryEffect.DefDown;

                case "chancedefdown":
                    Combat.PrintInEmptyMenu("chancedefdown");
                    if (randomNumber <= 10)
                    {
                        pokemonAdverse.defCombat = (int)(pokemonAdverse.defCombat * 0.88);
                    }
                    return SecondaryEffect.ChanceDefDown;

                case "speeddown":
                    Combat.PrintInEmptyMenu($"La vitesse de {pokemonAdverse.name} a baissé !");
                    pokemonAdverse.spdCombat = (int)(pokemonAdverse.spdCombat * 0.88);
                    return SecondaryEffect.SpeedDown;

                case "chancespeeddown":
                    if (randomNumber <= 10)
                    {
                        Combat.PrintInEmptyMenu($"La vitesse de {pokemonAdverse.name} a baissé !");
                        pokemonAdverse.spdCombat = (int)(pokemonAdverse.spdCombat * 0.88);
                    }
                        return SecondaryEffect.ChanceSpeedDown;

                case "spedown":
                    Combat.PrintInEmptyMenu($"Le special de {pokemonAdverse.name} a baissé !");
                    pokemonAdverse.speCombat = (int)(pokemonAdverse.speCombat * 0.88);
                    return SecondaryEffect.SpeDown;

                case "precisiondown":
                    Combat.PrintInEmptyMenu("precisiondown");
                    //==============================//
                    return SecondaryEffect.PrecisionDown;

                case "para":
                    Combat.PrintInEmptyMenu("para");
                    pokemonAdverse.statusProblem = "PARA";
                    return SecondaryEffect.Para;

                case "burn":
                    Combat.PrintInEmptyMenu("burn");
                    pokemonAdverse.statusProblem = "BURN";
                    return SecondaryEffect.Burn;

                case "freeze":
                    Combat.PrintInEmptyMenu("freeze");
                    pokemonAdverse.statusProblem = "FREEZE";
                    return SecondaryEffect.Freeze;

                case "sleep":
                    Combat.PrintInEmptyMenu("sleep");
                    pokemonAdverse.statusProblem = "SLEEP";
                    return SecondaryEffect.Sleep;

                case "selfSleep":
                    Combat.PrintInEmptyMenu("selfsleep");
                    pokemon.statusProblem = "SLEEP";
                    return SecondaryEffect.SelfSleep;

                case "poison":
                    Combat.PrintInEmptyMenu("Poison");
                    pokemonAdverse.statusProblem = "POISON";
                    return SecondaryEffect.Poison;

                case "poisongrave":
                    Combat.PrintInEmptyMenu("PoisonGrave");
                    pokemonAdverse.statusProblem = "POISONGRAVE";
                    return SecondaryEffect.PoisonGrave;

                case "confusion":
                    Combat.PrintInEmptyMenu("Confusion");
                    //==============================//
                    return SecondaryEffect.Confusion;

                case "randx":
                    Combat.PrintInEmptyMenu("randx");
                    //==============================//
                    return SecondaryEffect.randx;

                case "moneyx2":
                    Combat.PrintInEmptyMenu("Moneyx2");
                    //==============================//
                    return SecondaryEffect.Moneyx2;

                case "ohko":
                    Combat.PrintInEmptyMenu("OHKO");
                    pokemonAdverse.pvLeft = 0;
                    return SecondaryEffect.OHKO;

                case "charge":
                    Combat.PrintInEmptyMenu("Charge");
                    //==============================//
                    return SecondaryEffect.Charge;

                case "damagex2":
                    Combat.PrintInEmptyMenu("Damagex2");
                    //==============================//
                    return SecondaryEffect.Damagex2;

                case "endfight":
                    Combat.PrintInEmptyMenu("EndFight");
                    //==============================//
                    return SecondaryEffect.EndFight;

                case "imun":
                    Combat.PrintInEmptyMenu("Imun");
                    //==============================//
                    return SecondaryEffect.Imun;

                case "stuck":
                    Combat.PrintInEmptyMenu("Stuck");
                    //==============================//
                    return SecondaryEffect.Stuck;

                case "randt":
                    Combat.PrintInEmptyMenu("randt");
                    //==============================//
                    return SecondaryEffect.randt;

                case "flinch":
                    Combat.PrintInEmptyMenu("Flinch");
                    //==============================//
                    return SecondaryEffect.Flinch;

                case "doublex":
                    Combat.PrintInEmptyMenu("doublex");
                    //==============================//
                    return SecondaryEffect.doublex;

                case "doublet":
                    Combat.PrintInEmptyMenu("doublet");
                    //==============================//
                    return SecondaryEffect.doublet;

                case "fail":
                    Combat.PrintInEmptyMenu("Fail");
                    //==============================//
                    return SecondaryEffect.Fail;

                case "stuckself":
                    Combat.PrintInEmptyMenu("StuckSelf");
                    //==============================//
                    return SecondaryEffect.StuckSelf;

                case "recoil":
                    Combat.PrintInEmptyMenu("Recoil");
                    //==============================//
                    return SecondaryEffect.Recoil;

                case "constdamage":
                    Combat.PrintInEmptyMenu("ConstDamage");
                    //==============================//
                    return SecondaryEffect.ConstDamage;

                case "blockAttack":
                    Combat.PrintInEmptyMenu("BlockAttack");
                    //==============================//
                    return SecondaryEffect.BlockAttack;

                case "blockstatusmoves":
                    Combat.PrintInEmptyMenu("Effect: BlockStatusMoves");
                    //==============================//
                    return SecondaryEffect.BlockStatusMoves;

                case "pause":
                    Combat.PrintInEmptyMenu("Effect: Pause");
                    //==============================//
                    return SecondaryEffect.Pause;

                case "counter":
                    Combat.PrintInEmptyMenu("Effect: Counter");
                    //==============================//
                    return SecondaryEffect.Counter;

                case "vardamage":
                    Combat.PrintInEmptyMenu("Effect: VarDamage");
                    //==============================//
                    return SecondaryEffect.VarDamage;

                case "vardamagelevel":
                    Combat.PrintInEmptyMenu("Effect: VarDamageLevel");
                    //==============================//
                    return SecondaryEffect.VarDamageLevel;

                case "stealhp":
                    Combat.PrintInEmptyMenu("Effect: StealHP");
                    //==============================//
                    return SecondaryEffect.StealHP;

                case "constantstealhp":
                    Combat.PrintInEmptyMenu("Effect: ConstantStealHP");
                    //==============================//
                    return SecondaryEffect.ConstantStealHP;

                case "stealhpifsleep":
                    Combat.PrintInEmptyMenu("Effect: StealHPIfSleep");
                    //==============================//
                    return SecondaryEffect.StealHPIfSleep;

                case "heal":
                    Combat.PrintInEmptyMenu("Effect: Heal");
                    pokemon.pvLeft = pokemon.pv / 2 + pokemon.pvLeft;
                    if (pokemon.pvLeft > pokemon.pv)
                    {
                        pokemon.pvLeft = pokemon.pv;
                    }
                    return SecondaryEffect.Heal;

                case "alwayscrit":
                    Combat.PrintInEmptyMenu("Effect: AlwaysCrit");
                    //==============================//
                    return SecondaryEffect.AlwaysCrit;

                case "priority":
                    Combat.PrintInEmptyMenu("Effect: Priority");
                    //==============================//
                    return SecondaryEffect.Priority;

                case "copylastattack":
                    Combat.PrintInEmptyMenu("Effect: CopyLastAttack");
                    //==============================//
                    return SecondaryEffect.CopyLastAttack;

                case "cinqt":
                    Combat.PrintInEmptyMenu("Effect: Cinqt");
                    //==============================//
                    return SecondaryEffect.cinqt;

                case "resetstats":
                    Combat.PrintInEmptyMenu("Effect: ResetStats");
                    //==============================//
                    return SecondaryEffect.ResetStats;

                case "counterpatience":
                    Combat.PrintInEmptyMenu("Effect: CounterPatience");
                    //==============================//
                    return SecondaryEffect.CounterPatience;

                case "randomattack":
                    Combat.PrintInEmptyMenu("Effect: RandomAttack");
                    //==============================//
                    return SecondaryEffect.RandomAttack;

                case "copylastattacktarget":
                    Combat.PrintInEmptyMenu("Effect: CopyLastAttackTarget");
                    //==============================//
                    return SecondaryEffect.CopyLastAttackTarget;

                case "death":
                    Combat.PrintInEmptyMenu("Effect: Death");
                    //==============================//
                    return SecondaryEffect.Death;

                case "transformintoenemy":
                    Combat.PrintInEmptyMenu("Effect: TransformIntoEnemy");
                    //==============================//
                    return SecondaryEffect.TransformIntoEnemy;

                case "changetypetoenemy":
                    Combat.PrintInEmptyMenu("Effect: ChangeTypeToEnemy");
                    pokemon.listType = pokemonAdverse.listType;
                    return SecondaryEffect.ChangeTypeToEnemy;

                case "clone":
                    Combat.PrintInEmptyMenu("Effect: Clone");
                    //==============================//
                    return SecondaryEffect.Clone;

                default:
                    return SecondaryEffect.None;
            }
        }
        
    }
}
