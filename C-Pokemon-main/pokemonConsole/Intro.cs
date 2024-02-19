using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Usefull;

namespace pokemonConsole
{
    internal class Intro
    {
        private static string asciiFileOak = AdresseFile.FileDirection + "Assets\\Sprites\\ascii-art_chen.txt";
        private static string asciiFileBlue = AdresseFile.FileDirection + "Assets\\Sprites\\ascii-art_blue.txt";
        private static string asciiFileRed = AdresseFile.FileDirection + "Assets\\Sprites\\ascii-art_red.txt";
        private static string asciiFileMiddleBid = AdresseFile.FileDirection + "Assets\\Sprites\\ascii-art_middleBigRed.txt";
        private static string asciiFileMiddleLittle = AdresseFile.FileDirection + "Assets\\Sprites\\ascii-art_middleLittleRed.txt";
        private static string asciiFileLittleRed = AdresseFile.FileDirection + "Assets\\Sprites\\ascii-art_littleRed.txt";
        private static string scriptOak = AdresseFile.FileDirection + "Scripts\\ScriptIntroChen.txt";

        private static string asciiOak = "";
        private static string asciiBlue = "";
        private static string asciiRedHalf = "";
        private static string asciiRed = "";
        private static string asciiMiddleBigRed = "";
        private static string asciiMiddleLittleRed = "";
        private static string asciiLittleRed = "";


        private static int widthOak = 45;

        private static string hautTextZone = "0-------------------------------------------0";
        private static string middleTextZone = "|                                           |";



        static public void LaunchIntro(Player player, Rival rival)
        {
            LoadAllASCII();
            Functions.playSound("intro.wav");

            string line = "0";

            using (StreamReader sr = new StreamReader(scriptOak))
            {
                int lineVide = 0;
                bool firstLine = true;
                int lineReading = 1;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        if (!firstLine)
                        {
                            Console.ReadKey();
                        }
                        else
                        {
                            firstLine = false;
                        }

                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] == '*')
                            {
                                ReplaceCharacterWithPlayerName(ref line, '*', player.name);
                            }
                            else if (line[i] == '@')
                            {
                                ReplaceCharacterWithPlayerName(ref line, '@', rival.name);
                            }
                        }

                        Print(line, lineReading);
                        Functions.ClearInputBuffer();
                        lineReading++;
                        
                    }
                    else
                    {
                        lineVide++;
                        firstLine = true;

                        if (lineVide == 1)
                        {
                            Console.ReadKey();
                            player.name = Functions.ClavierName(7);
                        }
                        else if (lineVide == 2)
                        {
                            Console.ReadKey();
                            rival.name = Functions.ClavierName(7);
                        }
                    }
                }
                AnimationRedShrink();
            }
        }


        private static void Print(string line, int lineReading)
        {

            if (lineReading == 1 || lineReading == 9)
            {
                Console.Clear();
                Console.WriteLine(asciiOak);

                Console.WriteLine(hautTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(hautTextZone);
            }
            else if (lineReading == 10 || lineReading == 13)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(asciiBlue);
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(hautTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(hautTextZone);
            }
            else if (lineReading == 15) 
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(asciiRedHalf);
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(hautTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(middleTextZone);
                Console.WriteLine(hautTextZone);
            }


            
            int lineWriting = 1;
            int charRestant = widthOak - 4;

            Console.SetCursorPosition(2, 32);
            for (int i  = 2; i <= charRestant+1; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(2, 33);
            for (int i = 2; i <= charRestant +1; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(2, 34);
            for (int i = 2; i <= charRestant + 1; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(2, 32);

            string[] words = line.Split(" ");
            int wordIndex = 0;

            if (!string.IsNullOrEmpty(line))
            {
                for (int i = 0; i < line.Length; i++)
                {

                    if (charRestant > 0 && lineWriting == 1)
                    {
                        Console.Write(line[i]);
                        charRestant--;

                        // Si le caractère actuel est un espace, passe au mot suivant
                        if (line[i] == ' ')
                        {
                            wordIndex++;

                            // Vérifie si le mot suivant peut s'insérer dans la ligne actuelle
                            if (wordIndex < words.Length && words[wordIndex].Length > charRestant)
                            {
                                charRestant = widthOak - 4;
                                lineWriting++;
                                Console.SetCursorPosition(2, 33);
                            }
                        }
                    }
                    else if (charRestant > 0 && lineWriting == 2)
                    {
                        Console.Write(line[i]);
                        charRestant--;

                        // Si le caractère actuel est un espace, passe au mot suivant
                        if (line[i] == ' ')
                        {
                            wordIndex++;

                            // Vérifie si le mot suivant peut s'insérer dans la ligne actuelle
                            if (wordIndex < words.Length && words[wordIndex].Length > charRestant)
                            {
                                charRestant = widthOak - 4;
                                lineWriting++;
                                Console.SetCursorPosition(2, 34);
                            }
                        }
                    }
                    else if (charRestant > 0 && lineWriting == 3)
                    {
                        Console.Write(line[i]);
                        charRestant--;
                    }

                    Task.Delay(50).Wait();
                }


            }

        }

        private static void ReplaceCharacterWithPlayerName(ref string line, char characterToReplace, string playerName)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == characterToReplace)
                {
                    line = line.Remove(i, 1);

                    StringBuilder stringBuilder = new StringBuilder(line);
                    stringBuilder.Insert(i, playerName);

                    line = stringBuilder.ToString();
                }
            }
        }
        private static void LoadAllASCII()
        {
            asciiOak = File.ReadAllText(asciiFileOak);
            asciiBlue = File.ReadAllText(asciiFileBlue);
            asciiRed = File.ReadAllText(asciiFileRed);
            asciiMiddleBigRed = File.ReadAllText(asciiFileMiddleBid);
            asciiMiddleLittleRed = File.ReadAllText(asciiFileMiddleLittle);
            asciiLittleRed = File.ReadAllText(asciiFileLittleRed);
            using (StreamReader sr = new StreamReader(asciiFileRed))
            {
                for (int i = 0; i < 30; i++)
                {
                    asciiRedHalf += sr.ReadLine() + Environment.NewLine;
                }
            }
        }


        private static void AnimationRedShrink()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Clear();
            Console.WriteLine(asciiRed);
            Thread.Sleep(200);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine(asciiMiddleBigRed);
            Thread.Sleep(200);

            Console.Clear();
            Console.WriteLine(asciiMiddleLittleRed);
            Thread.Sleep(200);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine(asciiLittleRed);
            Thread.Sleep(200);


        }



    }
}