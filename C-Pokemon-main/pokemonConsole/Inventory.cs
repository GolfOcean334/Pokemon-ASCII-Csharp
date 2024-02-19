using Microsoft.VisualBasic.FileIO;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Usefull;

namespace pokemonConsole
{
    public class Item
    {
        public int id;
        public string name;

        public effect effectAtk;
        private string effect2;


        private string usage;

        public int quantity;

        private string fileCSV = AdresseFile.FileDirection + "CSV\\item.csv";

        public Item(int item_id, int quantity = 1)
        {
            using (StreamReader sr = new StreamReader(fileCSV))
            {
                string line;
                bool itemFound = false;

                line = sr.ReadLine();
                line = sr.ReadLine();

                while ((line = sr.ReadLine()) != null && !itemFound)
                {
                    string[] colonnes = line.Split(',');
                    if (item_id == int.Parse(colonnes[0]))
                    {
                        id = item_id;
                        name = colonnes[1];
                        string effect1 = colonnes[2];
                        effect2 = colonnes[3];
                        usage = colonnes[4];

                        this.quantity = quantity;

                        effectAtk = GetEffect(effect1);
                    }
                }
            }
        }
        public enum effect
        { 
            none,
            CatchPokemon,
            HealPV,
            Guerison,
            Curestatut,
            Revive,
            TenPPAllCap,
            AllPPAllCap,
            TenPPOneCap,
            AllPPOneCap,
            EVPV,
            EVATK,
            EVDEF,
            EVSPE,
            EVSPD,
            PPPlus,
            LevelUp,
            Evolve,
            ADDAtk,
            ADDDef,
            ADDSpe,
            ADDSpd,
            ImmuneStats,
            ADDCrit,
            CantFail
        }
        private effect GetEffect(string effect_)
        {
            if (effect_ == "CATCHPOKEMON") return effect.CatchPokemon;
            else if (effect_ == "HEALPV") return effect.HealPV;
            else if (effect_ == "GUERISON") return effect.Guerison;
            else if (effect_ == "CURESTATUT") return effect.Curestatut;
            else if (effect_ == "REVIVE") return effect.Revive;
            else if (effect_ == "10PPALLCAP") return effect.TenPPAllCap;
            else if (effect_ == "ALLPPALLCAP") return effect.AllPPAllCap;
            else if (effect_ == "10PP1CAP") return effect.TenPPOneCap;
            else if (effect_ == "ALLPP1CAP") return effect.AllPPOneCap;
            else if (effect_ == "EVPV") return effect.EVPV;
            else if (effect_ == "EVATK") return effect.EVATK;
            else if (effect_ == "EVDEF") return effect.EVDEF;
            else if (effect_ == "EVSPE") return effect.EVSPE;
            else if (effect_ == "EVSPD") return effect.EVSPD;
            else if (effect_ == "PPPLUS") return effect.PPPlus;
            else if (effect_ == "LEVELUP") return effect.LevelUp;
            else if (effect_ == "EVOLVE") return effect.Evolve;
            else if (effect_ == "ADDATK") return effect.ADDAtk;
            else if (effect_ == "ADDDEF") return effect.ADDDef;
            else if (effect_ == "ADDSPE") return effect.ADDSpe;
            else if (effect_ == "ADDSPE") return effect.ADDSpd;
            else if (effect_ == "IMMUNE") return effect.ImmuneStats;
            else if (effect_ == "ADDCRIT") return effect.ADDCrit;
            else if (effect_ == "CANTFAIL") return effect.CantFail;

            return effect.none;
        }




        static public void UseItem(Item item, ref Player player, int pokemonPosition, int capPosition, bool isCombat = false, Pokemon pokemonAdverse = null, Pokemon pokemon = null)
        {
            if ((item.usage == "NOTBATTLE" && !isCombat))
            {
                switch (item.effectAtk)
                {
                    case effect.TenPPAllCap:
                        foreach (Capacity cap in player.pokemonParty[pokemonPosition].listAttackActual)
                        {
                            if (cap.ppLeft + 10 > cap.pp) cap.ppLeft = cap.pp;
                            else cap.ppLeft += 10;
                        }
                        item.quantity--;
                        break;
                    case effect.AllPPAllCap:
                        foreach (Capacity cap in player.pokemonParty[pokemonPosition].listAttackActual)
                        {
                            cap.ppLeft = cap.pp;
                        }
                        item.quantity--;
                        break;
                    case effect.TenPPOneCap:
                        if (player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppLeft < player.pokemonParty[pokemonPosition].listAttackActual[capPosition].pp)
                        {
                            if (player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppLeft + 10 > player.pokemonParty[pokemonPosition].listAttackActual[capPosition].pp) player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppLeft = player.pokemonParty[pokemonPosition].listAttackActual[capPosition].pp;
                            else player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppLeft += 10;
                            item.quantity--;
                        }
                        break;
                    case effect.AllPPOneCap:
                        if (player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppLeft < player.pokemonParty[pokemonPosition].listAttackActual[capPosition].pp)
                        {
                            player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppLeft = player.pokemonParty[pokemonPosition].listAttackActual[capPosition].pp;
                            item.quantity--;
                        }
                        break;
                    case effect.EVPV:
                        if (player.pokemonParty[pokemonPosition].listPv[2] < 65535) { player.pokemonParty[pokemonPosition].GainEV(int.Parse(item.effect2), 0, 0, 0, 0); item.quantity--; }

                        break;
                    case effect.EVATK:
                        if (player.pokemonParty[pokemonPosition].listAtk[2] < 65535) { player.pokemonParty[pokemonPosition].GainEV(0, int.Parse(item.effect2), 0, 0, 0); item.quantity--; }
                        break;
                    case effect.EVDEF:
                        if (player.pokemonParty[pokemonPosition].listDef[2] < 65535) { player.pokemonParty[pokemonPosition].GainEV(0, 0, int.Parse(item.effect2), 0, 0); item.quantity--; }
                        break;
                    case effect.EVSPE:
                        if (player.pokemonParty[pokemonPosition].listSpe[2] < 65535) { player.pokemonParty[pokemonPosition].GainEV(0, 0, 0, int.Parse(item.effect2), 0); item.quantity--; }
                        break;
                    case effect.EVSPD:
                        if (player.pokemonParty[pokemonPosition].listSpd[2] < 65535) { player.pokemonParty[pokemonPosition].GainEV(0, 0, 0, 0, int.Parse(item.effect2)); item.quantity--; }
                        break;
                    case effect.PPPlus:
                        if (player.pokemonParty[pokemonPosition].listAttackActual[capPosition].pp < player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppMax)
                        {
                            int ppAdded = player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppOriginal * 20 / 100;

                            player.pokemonParty[pokemonPosition].listAttackActual[capPosition].pp += ppAdded;
                            player.pokemonParty[pokemonPosition].listAttackActual[capPosition].ppLeft += ppAdded;
                            item.quantity--;
                        }
                        break;
                    case effect.LevelUp:
                        if (player.pokemonParty[pokemonPosition].level < 100)
                        {
                            player.pokemonParty[pokemonPosition].LevelUp(true);
                            item.quantity--;
                        }
                        break;
                    case effect.Evolve:
                        bool evolved = false;
                        foreach (int id in player.pokemonParty[pokemonPosition].evolutionItemId)
                        {
                            if (item.id == id) { player.pokemonParty[pokemonPosition].Evolution(); evolved = true; }
                        }
                        if (evolved)
                        {

                            item.quantity--;
                        }
                        break;
                }
            }
            else if (item.usage == "NOTBATTLE" && isCombat)
            {
                Console.WriteLine("Unusable");
            }
            else if ((item.usage == "BATTLE" && isCombat))
            {
                switch (item.effectAtk)
                {
                    case effect.CatchPokemon:
                        if (pokemonAdverse.appartenant == 0)
                        {
                            player.caughtPokemon = Player.catchPokemon(pokemonAdverse, player, int.Parse(item.effect2), pokemon);
                            item.quantity--;
                        }
                        break;
                    case effect.ADDAtk:
                        Combat.PrintInEmptyMenu($"L'attaque de {player.pokemonParty[pokemonPosition].name} a augmenté !");
                        player.pokemonParty[pokemonPosition].atkCombat = (int)(player.pokemonParty[pokemonPosition].atkCombat * 1.5);
                        item.quantity--;
                        break;
                    case effect.ADDDef:
                        Combat.PrintInEmptyMenu($"La défense de {player.pokemonParty[pokemonPosition].name} a augmenté !");
                        player.pokemonParty[pokemonPosition].defCombat = (int)(player.pokemonParty[pokemonPosition].defCombat * 1.5);
                        item.quantity--;
                        break;
                    case effect.ADDSpe:
                        Combat.PrintInEmptyMenu($"La vitesse de {player.pokemonParty[pokemonPosition].name} a augmenté !");
                        player.pokemonParty[pokemonPosition].spdCombat = (int)(player.pokemonParty[pokemonPosition].spdCombat * 1.5);
                        item.quantity--;
                        break;
                    case effect.ADDSpd:
                        Combat.PrintInEmptyMenu($"Le special de {player.pokemonParty[pokemonPosition].name} a augmenté !");
                        player.pokemonParty[pokemonPosition].speCombat = (int)(player.pokemonParty[pokemonPosition].speCombat * 1.5);
                        item.quantity--;
                        break;
                    case effect.ImmuneStats:
                        item.quantity--;
                        break;
                    case effect.ADDCrit:
                        item.quantity--;
                        break;
                    case effect.CantFail:
                        item.quantity--;
                        break;
                }
            }
            else if (item.usage == "BATTLE" && !isCombat)
            {
                Console.WriteLine("Unusable");
            }
            else if (item.usage == "BOTH")
            {
                switch (item.effectAtk)
                {
                    case effect.HealPV:
                        if (player.pokemonParty[pokemonPosition].pvLeft < player.pokemonParty[pokemonPosition].pv && player.pokemonParty[pokemonPosition].pvLeft != 0)
                        {
                            try
                            {
                                int healInt = int.Parse(item.effect2);
                                if (player.pokemonParty[pokemonPosition].pvLeft + healInt > player.pokemonParty[pokemonPosition].pv)
                                {
                                    player.pokemonParty[pokemonPosition].pvLeft = player.pokemonParty[pokemonPosition].pv;
                                }
                                else
                                {
                                    player.pokemonParty[pokemonPosition].pvLeft += healInt;
                                }
                            }
                            catch
                            {
                                player.pokemonParty[pokemonPosition].pvLeft = player.pokemonParty[pokemonPosition].pv;
                            }
                            item.quantity--;
                        }
                        break;
                    case effect.Guerison:
                        if ((player.pokemonParty[pokemonPosition].pvLeft < player.pokemonParty[pokemonPosition].pv && player.pokemonParty[pokemonPosition].pvLeft != 0) || player.pokemonParty[pokemonPosition].statusProblem != "OK")
                        {
                            player.pokemonParty[pokemonPosition].pvLeft = player.pokemonParty[pokemonPosition].pv;
                            player.pokemonParty[pokemonPosition].statusProblem = "OK";


                            item.quantity--;
                        }
                        break;
                    case effect.Curestatut:
                        if (player.pokemonParty[pokemonPosition].statusProblem != "OK")
                        {
                            switch (item.effect2)
                            {
                                case "PSN":
                                    if (player.pokemonParty[pokemonPosition].statusProblem == "PSN" || player.pokemonParty[pokemonPosition].statusProblem == "PSNGRAVE") player.pokemonParty[pokemonPosition].statusProblem = "OK";
                                    break;
                                case "PAR":
                                    if (player.pokemonParty[pokemonPosition].statusProblem == "PAR") player.pokemonParty[pokemonPosition].statusProblem = "OK";
                                    break;
                                case "BRN":
                                    if (player.pokemonParty[pokemonPosition].statusProblem == "BRN") player.pokemonParty[pokemonPosition].statusProblem = "OK";
                                    break;
                                case "SLP":
                                    if (player.pokemonParty[pokemonPosition].statusProblem == "SLP") player.pokemonParty[pokemonPosition].statusProblem = "OK";
                                    break;
                                case "ALL":
                                    player.pokemonParty[pokemonPosition].statusProblem = "OK";
                                    break;
                            }

                            item.quantity--;
                        }
                        break;
                    case effect.Revive:
                        if (player.pokemonParty[pokemonPosition].pvLeft <= 0)
                        {
                            player.pokemonParty[pokemonPosition].pvLeft = player.pokemonParty[pokemonPosition].pv * int.Parse(item.effect2) / 100;
                            player.pokemonParty[pokemonPosition].ko = false;
                            item.quantity--;
                        }
                        break;
                }
            }

            if (item.quantity == 0)
            {
                player.inventory.Remove(item);
            }
        }

        public void JeterItem(Player player, int quantity)
        {
            if (this.quantity - quantity > 0)
            {
                this.quantity -= quantity;
            }
            else
            {
                player.inventory.Remove(this);
            }
        }
    }
}
