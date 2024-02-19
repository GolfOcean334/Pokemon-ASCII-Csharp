using pokemonConsole;
using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Media;
using Usefull;
using NUnit.Framework.Interfaces;

internal class Map
{
    private static char[,] map;
    private static Random random = new Random();
    private static string currentMapFileName="";
    private static int mapHeight;
    private static int mapWidth;

    private static Player player;
    private static Rival rival;

    private static List<Entity> entityList = new List<Entity>();
    private static List<Entity> entityToRemove = new List<Entity>();

    private static string topBox;
    private static string middleBox;
    private static int DialogueX;
    private static int DialogueY;


    public static void MapPlayer(Player player_, Rival rival_)
    {
        Functions.playSound("bourg_palette.wav");
        player = player_;
        rival = rival_;

        LoadMap(player.map);
        ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();


        player.actuallPositionChar = map[player.PositionX, player.PositionY];

        DrawMap();
        DateTime time = DateTime.Now;

        do
        {
            foreach (Entity entity in entityList)
            {
                if (entity is NPC npc)
                {
                    npc.Update(time, player);
                    if (npc.updated)
                    {
                        npc.updated = false;
                        DrawMap();
                        time = DateTime.Now;
                    }
                }
            }


            bool moved = false;
            int deltaX = 0, deltaY = 0;
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);
                // Deplacer le joueur en fonction de la touche pressee
                if (player.name != "BLUE" && rival.name != "RED")
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            deltaY = -1;
                            moved = true;
                            break;
                        case ConsoleKey.DownArrow:
                            deltaY = 1;
                            moved = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            deltaX = -1;
                            moved = true;
                            break;
                        case ConsoleKey.RightArrow:
                            deltaX = 1;
                            moved = true;
                            break;
                        case ConsoleKey.X:
                            Menu_principal.Open(player, mapWidth, rival);

                            DrawMap();

                            break;
                    }
                }
                else
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.DownArrow:
                            deltaY = -1;
                            moved = true;
                            break;
                        case ConsoleKey.UpArrow:
                            deltaY = 1;
                            moved = true;
                            break;
                        case ConsoleKey.RightArrow:
                            deltaX = -1;
                            moved = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            deltaX = 1;
                            moved = true;
                            break;
                        case ConsoleKey.X:
                            Menu_principal.Open(player, mapWidth, rival);

                            DrawMap();

                            break;
                    }
                }
                

            if (MovePlayer(deltaX, deltaY))
            {
                DrawPlayer();


                // Transition et npc
                if (IsCurrentMap("bedroom.txt"))
                {
                    if (moved)
                    {
                        ChangeMap(15, 1, "mom.txt", 8, 1, "\nMaman...");

                    }

                    foreach (Entity entity in entityList)
                    {
                        if (entity is PC pc) UsePC(pc, keyInfo);
                    }
                }

                else if (IsCurrentMap("bourg_palette.txt"))
                {
                    if (moved)
                    {
                        ChangeMap(0, "route_1.txt", player.PositionX - 3, 35, "\nVers la route 1 !");
                        ChangeMap(13, 10, "chen.txt", 5, 8, "\nVers le labo du Pr.Chen...");
                        ChangeMap(6, 5, "mom.txt", 3, 8, "\nMaman...");
                        ChangeMap(14, 5, "blue.txt", 3, 8, $"\nMaison de {rival.name}...");
                    }
                }

                else if (IsCurrentMap("chen.txt"))
                {
                    if (moved) ChangeMap(8, "bourg_palette.txt", 13, 11, "\nVers Bourg-Palette...");

                    foreach (Entity entity in entityList)
                    {
                        if (entity is NPC npc)
                        {
                            CanTalk(npc, keyInfo);
                        }
                        else if (entity is Pokeball pokeball)
                        {
                            Open(pokeball, keyInfo);
                        }
                    }
                }

                else if (IsCurrentMap("mom.txt"))
                {
                    if (moved)
                    {
                        ChangeMap(8, "bourg_palette.txt", 6, 6, "\nVers Bourg-Palette...");
                        ChangeMap(8, 1, "bedroom.txt", 15, 1, "\nChambre...");
                    }
                    foreach (Entity entity in entityList)
                    {
                        if(entity is NPC npc) CanTalk(npc, keyInfo);
                    }
                }

                else if (IsCurrentMap("route_1.txt"))
                {
                    if (moved) ChangeMap(35, "bourg_palette.txt", player.PositionX + 3, 0, "Vous arrivez à Bourg Palette !");
                    foreach (NPC npc in entityList)
                    {
                        CanTalk(npc, keyInfo);
                    }
                }
                else if (IsCurrentMap("blue.txt"))
                 {
                     if (moved)
                     {
                         ChangeMap(8, "bourg_palette.txt", 14, 6, "\nVers Bourg-Palette...");
                     }

                     foreach (NPC npc in entityList)
                     {
                         CanTalk(npc, keyInfo);
                     }
                 }

                    // Hautes herbes
                if (map[player.PositionX, player.PositionY] == '#' && moved)
                {
                    if (random.Next(1, 101) <= 10) // chance de rencontrer un Pokemon dans les hautes herbes
                    {
                        Functions.playSound("combat_sauvage.wav");
                        Console.WriteLine($"\nCombat lancé !");
                        Thread.Sleep(1000);
                        Functions.ClearInputBuffer();
                        Combat.LoopCombat(player);
                        if (player.IsKO())
                        {
                            Console.WriteLine("Tous vos Pokemon sont KO !");
                            ChangeMap("mom.txt", 7, 5, "Retour chez maman...");
                            Functions.playSound("bourg_palette.wav");
                            Pokemon.Heal(player);
                        }
                        Console.Clear();
                        DrawMap();
                        if (IsCurrentMap("route_1.txt"))
                            {
                                Functions.playSound("route_1.wav");
                            }
                        else if (IsCurrentMap("bourg_palette.txt"))
                            {
                                Functions.playSound("bourg_palette.wav");
                            }
                        else if (IsCurrentMap("chen.txt"))
                            {
                                Functions.playSound("chen.wav");
                            }
                        }
                }

                if (entityToRemove.Count != 0)
                {
                    RemoveEntity();
                }
            }
        }


        } while (keyInfo.Key != ConsoleKey.Escape);

        

        player.pokemonParty.Clear();
    }


    private static void ChangeMap(string nextMapFileName, int nextX, int nextY, string loadingText)
    {
            Console.WriteLine($"\n{loadingText}");
            Thread.Sleep(500);
            Functions.ClearInputBuffer();
            LoadMap(nextMapFileName);
            player.PositionX = nextX;
            player.PositionY = nextY;
            player.map = nextMapFileName;

            player.actuallPositionChar = map[nextX, nextY];

            Console.Clear();
            DrawMap();
    }
    private static void ChangeMap(int x, int y, string nextMapFileName, int nextX, int nextY, string loadingText)
    {

        if (player.PositionX == x && player.PositionY == y)
        {
            Console.WriteLine($"\n{loadingText}");
            Thread.Sleep(500);
            Functions.ClearInputBuffer();
            LoadMap(nextMapFileName);
            player.PositionX = nextX;
            player.PositionY = nextY;
            player.map = nextMapFileName;

            player.actuallPositionChar = map[nextX, nextY];

            DrawMap();
        }
    }
    private static void ChangeMap(int y, string nextMapFileName, int nextX, int nextY, string loadingText)
    {

        if (player.PositionY == y)
        {
            Console.WriteLine($"\n{loadingText}");
            Thread.Sleep(500);
            Functions.ClearInputBuffer();
            LoadMap(nextMapFileName);
            player.PositionX = nextX;
            player.PositionY = nextY;
            player.map = nextMapFileName;

            player.actuallPositionChar = map[nextX, nextY];

            DrawMap();
        }
    }
    private static void CanTalk(NPC npc, ConsoleKeyInfo keyInfo)
    {

        if (npc.PositionX != -1 && npc.PositionY != -1)
        {
            if ((player.PositionX + 1 == npc.PositionX && player.PositionY == npc.PositionY) || (player.PositionX - 1 == npc.PositionX && player.PositionY == npc.PositionY) || (player.PositionX == npc.PositionX && player.PositionY - 1 == npc.PositionY) || (player.PositionX == npc.PositionX && player.PositionY + 1 == npc.PositionY))
            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    PrintDialogue(npc.dialogue);
                    if (npc is RivalNPC rivalNPC)
                    {
                        rivalNPC.rival.addPokemonToParty(player);
                    }

                    npc.Function(player);


                    DrawMap();
                }
                
            }
        }


    }
    private static void Open(Pokeball pokeball, ConsoleKeyInfo keyInfo)
    {

        if (pokeball.PositionX != -1 && pokeball.PositionY != -1 && !pokeball.taken && player.starterId == null)
        {
            if ((player.PositionX + 1 == pokeball.PositionX && player.PositionY == pokeball.PositionY) || (player.PositionX - 1 == pokeball.PositionX && player.PositionY == pokeball.PositionY) || (player.PositionX == pokeball.PositionX && player.PositionY - 1 == pokeball.PositionY) || (player.PositionX == pokeball.PositionX && player.PositionY + 1 == pokeball.PositionY))
            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    PrintDialogue($"Voulez vous {pokeball.name} ?");
                    int position = 0;
                    Console.SetCursorPosition(Console.CursorLeft + mapWidth * 2- 7, Console.CursorTop-5);
                    int cursorLeft = Console.CursorLeft;
                    int cursorTop = Console.CursorTop;

                    Console.WriteLine("0-----0");
                    Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    Console.WriteLine("|> OUI|");
                    Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    Console.WriteLine("|  NON|");
                    Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                    Console.WriteLine("0-----0");

                    bool choicePokemon = false;
                    while (!choicePokemon)
                    {
                        keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.UpArrow:
                                if (position == 1)
                                {
                                    Console.SetCursorPosition(cursorLeft + 1, cursorTop + 2);
                                    Console.Write(" ");
                                    Console.SetCursorPosition(cursorLeft + 1, Console.CursorTop - 1);
                                    Console.Write(">");
                                    position -= 1;
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (position == 0)
                                {
                                    Console.SetCursorPosition(cursorLeft + 1, cursorTop + 1);
                                    Console.WriteLine(" ");
                                    Console.SetCursorPosition(cursorLeft + 1, Console.CursorTop);
                                    Console.Write(">");
                                    position += 1;
                                }
                                break;
                            case ConsoleKey.Enter:
                                if (position == 0)
                                {
                                    pokeball.Function(player);
                                    entityToRemove.Add(pokeball);
                                    Functions.playSound("receive_pokemon.wav");
                                    PrintDialogue($"Vous avez obtenu {pokeball.name} !");

                                    if (pokeball.position == 1)
                                    {
                                        foreach (Entity entity in entityList)
                                        {
                                            if (entity is Pokeball pokeball2 && pokeball2.position == 2)
                                            {
                                                entityToRemove.Add(pokeball2);
                                            }
                                        }
                                    }
                                    else if (pokeball.position == 2)
                                    {
                                        foreach (Entity entity in entityList)
                                        {
                                            if (entity is Pokeball pokeball2 && pokeball2.position == 3)
                                            {
                                                entityToRemove.Add(pokeball2);
                                            }
                                        }
                                    }
                                    else if (pokeball.position == 3)
                                    {
                                        foreach (Entity entity in entityList)
                                        {
                                            if (entity is Pokeball pokeball2 && pokeball2.position == 1)
                                            {
                                                entityToRemove.Add(pokeball2);
                                            }
                                        }
                                    }

                                    Functions.ClearInputBuffer();

                                    Functions.playSound("chen.wav");
                                }

                                choicePokemon = true;
                                break;
                        }
                    }
                    

                    
                    DrawMap();
                }

            }
        }

    }
    
    private static void UsePC(PC pc, ConsoleKeyInfo keyInfo)
    {
        if (player.PositionX == pc.PositionX && player.PositionY == pc.PositionY + 1)
        {
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                pc.Function(player);
                DrawMap();
            }
        }
    }

    
    
    
    
    private static void LoadMap(string filename)
    {

        currentMapFileName = AdresseFile.FileDirection + "Assets\\Maps\\" + filename;

        string[] lines = File.ReadAllLines(currentMapFileName);

        mapWidth = lines[0].Length;
        mapHeight = lines.Length;

        map = new char[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            string line = lines[y];

            for (int x = 0; x < mapWidth; x++)
            {
                map[x, y] = line[x];
            }
        }

        topBox = "0";
        middleBox = "|";
        for (int y = 1;y < (mapWidth - 1) * 2; y++)
        {
            topBox += "-";
            middleBox += " ";
        }
        topBox += "0";
        middleBox+= "|";
        DialogueX = 0;
        DialogueY = mapHeight - 2;


        entityList.Clear();
        if (filename == "chen.txt")
        {
            Functions.playSound("chen.wav");
            NPC chen = new NPC("Prof.Chen", "Voici 3 Pokémon! Mais... Ils sont dans des Poké Balls. Plus jeune, j'étais un sacré Dresseur de Pokémon! Et oui! Mais avec l'âge, il ne m'en reste plus que 3! Choisis-en un!", 'C', filename, 6, 2, map[6, 2]);
            
            RivalNPC rivalNPC = new RivalNPC(rival);
            
            entityList.Add(chen);
            entityList.Add(rivalNPC);

            Pokeball pokeball1;
            Pokeball pokeball2;
            Pokeball pokeball3;

            if (player.starterId == null)
            {
                pokeball1 = new Pokeball(1, filename, 8, 3, map[8, 3], 1);
                pokeball2 = new Pokeball(4, filename, 9, 3, map[9, 3], 2);
                pokeball3 = new Pokeball(7, filename, 10, 3, map[10, 3], 3);

                entityList.Add(pokeball1);
                entityList.Add(pokeball2);
                entityList.Add(pokeball3);
            }
            else if (player.starterId == 1)
            {
                pokeball3 = new Pokeball(7, filename, 10, 3, map[10, 3], 3);

                entityList.Add(pokeball3);
            }
            else if (player.starterId == 4)
            {
                pokeball1 = new Pokeball(1, filename, 8, 3, map[8, 3], 1);

                entityList.Add(pokeball1);
            }
            else if (player.starterId == 7)
            {
                pokeball2 = new Pokeball(4, filename, 9, 3, map[9, 3], 2);

                entityList.Add(pokeball2);
            }

            

        }
        else if (filename == "mom.txt")
        {
            Maman maman = new Maman();
            entityList.Add(maman);
        }
        else if (filename == "route_1.txt")
        {
            Functions.playSound("route_1.wav");
            PotionMan potionMan = new PotionMan();
            entityList.Add(potionMan);
        }
        else if (filename == "bourg_palette.txt")
        {
            if (!Functions.MusicCurrent("bourg_palette.wav"))
            {
                Functions.playSound("bourg_palette.wav");
            }
        }
        else if (filename == "blue.txt")
        {
            Nina nina = new Nina();
            entityList.Add(nina);
        }
        else if (filename == "bedroom.txt")
        {
            PC pc = new PC();
            entityList.Add(pc);
        }
    }


    public static void DrawMap()
    {
        Console.Clear();

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                
                if (map[x, y] == '~')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (map[x, y] == '#')
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write(map[x, y]);
            }
            Console.WriteLine();
        }

        DrawEntity();
        DrawPlayer();
    }
    private static bool MovePlayer(int deltaX, int deltaY)
    {
        // limites du deplacement pour eviter de sortir de la carte

        int newX = player.PositionX + deltaX;
        int newY = player.PositionY + deltaY;

        if (IsInsideMap(newX, newY) && IsWalkable(newX, newY))
        {
            int x = player.PositionX;
            int y = player.PositionY;
            // Effacer l'ancienne position du joueur
            Console.SetCursorPosition(x, y);
            
            if (map[x, y] == '~')
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (map[x, y] == '#')
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }

            Console.Write(player.actuallPositionChar);
            Console.ForegroundColor = ConsoleColor.White;



            // Mettre à jour la position du joueur
            player.PositionX = newX;
            player.PositionY = newY;

            player.actuallPositionChar = map[newX, newY];

            return true;
        }
        else
        {
            return false;
        }
    }
    private static void DrawPlayer()
    {
        Console.SetCursorPosition(player.PositionX, player.PositionY);
        Console.Write(player.sprite);
    }
    private static void DrawEntity()
    {
        foreach (Entity entity in entityList)
        {
            Console.SetCursorPosition(entity.PositionX, entity.PositionY);
            if (entity.sprite == 'o')
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(entity.sprite);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Write(entity.sprite);
            }
        }
        
        
    }


    private static void printBoxDialogue()
    {
        Console.SetCursorPosition(DialogueX, DialogueY);
        Console.WriteLine(topBox);
        for (int i = 0; i < 3; i++) 
        {
            Console.WriteLine(middleBox);        
        }
        Console.WriteLine(topBox);
    }
    public static void PrintDialogue(string text)
    {
        printBoxDialogue();

        Console.SetCursorPosition(DialogueX + 2, DialogueY + 1);
        if (!string.IsNullOrEmpty(text))
        {
            int charRestant = topBox.Length - 4;
            int lineWriting = 1;
            string[] words = text.Split(" ");
            int wordIndex = 0;


            for (int i = 0; i < text.Length; i++)
            {

                if (charRestant > 0 && lineWriting == 1)
                {
                    Console.Write(text[i]);
                    charRestant--;

                    // Si le caractère actuel est un espace, passe au mot suivant
                    if (text[i] == ' ' || (charRestant == 0 && text[i + 1] == ' '))
                    {
                        wordIndex++;

                        // Vérifie si le mot suivant peut s'insérer dans la ligne actuelle
                        if (wordIndex < words.Length && words[wordIndex].Length > charRestant)
                        {
                            charRestant = topBox.Length - 4;
                            lineWriting++;
                            Console.SetCursorPosition(DialogueX + 2, DialogueY + 2);
                        }
                    }
                }
                else if (charRestant > 0 && lineWriting == 2)
                {
                    if (!(charRestant == topBox.Length - 4 && text[i] == ' '))
                    {
                        Console.Write(text[i]);
                    }

                    charRestant--;

                    // Si le caractère actuel est un espace, passe au mot suivant
                    if (text[i] == ' ' || (charRestant == 0 && text[i + 1] == ' '))
                    {
                        wordIndex++;

                        // Vérifie si le mot suivant peut s'insérer dans la ligne actuelle
                        if (wordIndex < words.Length && words[wordIndex].Length > charRestant)
                        {
                            charRestant = topBox.Length - 4;
                            lineWriting++;
                            Console.SetCursorPosition(DialogueX + 2, DialogueY + 3);
                        }
                    }
                }
                else if (charRestant > 0 && lineWriting == 3)
                {
                    if (!(charRestant == topBox.Length - 4 && text[i] == ' '))
                    {
                        Console.Write(text[i]);
                    }

                    charRestant--;

                }
                Thread.Sleep(20);
            }

            Console.WriteLine();
            Functions.ClearInputBuffer();
            Console.ReadKey(true);
            Functions.ClearInputBuffer();
        }
    }
    




    private static bool IsCurrentMap(string mapToCheck)
    {
        string fullPathToCheck = AdresseFile.FileDirection + "Assets\\Maps\\" + mapToCheck;
        return currentMapFileName.Equals(fullPathToCheck, StringComparison.OrdinalIgnoreCase);
    }
    private static bool IsInsideMap(int x, int y)
    {
        return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
    }
    private static bool IsWalkable(int x, int y)
    {
        if (map[x, y] != ' ' && map[x, y] != '#' && map[x, y] != 'D')
        {
            return false;
        }

        if (map[x, y] == '#' && player.pokemonParty.Count == 0)
        {
            return false;
        }

        foreach(Entity entity in entityList)
        {
            if (x == entity.PositionX && y == entity.PositionY)
            {
                return false;
            }
        }

        return true;
    }


    private static void RemoveEntity()
    {
        foreach(Entity entity in entityToRemove) 
        {
            entityList.Remove(entity);
        }
        entityToRemove.Clear();

        DrawMap();
    }
}
