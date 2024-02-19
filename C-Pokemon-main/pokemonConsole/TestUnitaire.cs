using NUnit.Framework;
using System;
using System.IO;

namespace pokemonConsole.Tests
{
    [TestFixture]
    public class CombatTests
    {
        [Test]
        public void TestLoopCombat_PokemonAdverseFaint()
        {
            using (StringReader stringReader = new StringReader("1\n1\n"))
            {
                Console.SetIn(stringReader);

                StringWriter stringWriter = new StringWriter();
                Console.SetOut(stringWriter);

                Player player = new Player(); 
                Combat.LoopCombat(player);

                string output = stringWriter.ToString();
                Assert.IsTrue(output.Contains("Le Pokemon de l'adversaire a perdu !"));
            }
        }

        [Test]
        public void TestLoopCombat_PlayerFaint()
        {
            using (StringReader stringReader = new StringReader("1\n4\n"))
            {
                Console.SetIn(stringReader);

                StringWriter stringWriter = new StringWriter();
                Console.SetOut(stringWriter);

                Player player = new Player(); 
                Combat.LoopCombat(player);

                string output = stringWriter.ToString();
                Assert.IsTrue(output.Contains("Le Pokemon du joueur a perdu !"));
            }
        }
    }
}
