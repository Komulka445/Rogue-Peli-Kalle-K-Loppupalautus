using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using ZeroElectric.Vinculum;
using static System.Net.Mime.MediaTypeNames;
using Rectangle = ZeroElectric.Vinculum.Rectangle;
using TurboMapReader; //turbomapreader tiled mappi juttu

namespace Rogue
{
    class MapLayer
    {
        public string name;
        public int[] mapTiles;
    }
    internal class Game

    {
        Sound kavely;
        Texture temp;
        Texture seina;
        Texture lattia;
        Rectangle sPlayerRect;
        Rectangle dPlayerRect;
        Rectangle sWallRect;
        Rectangle dWallRect;
        Rectangle sFloorRect;
        Rectangle dFloorRect;
        public Font haloFontti;
        MapLoader loader = new MapLoader();
        int PosX = 1;
        int PosY = 1;
        int FormerX = 1;
        int FormerY = 1;
        int ScreensizeX;
        int ScreensizeY;
        int resoPlier = 1;
        public static readonly int tileSize = 16;
        Map level01;
        Map level02;
        TurboMapReader.TiledMap level03; //turbomapreader tiled mappi juttu
        Map mapproxy;
        MapLoader mapLoader;
        int pixelX;
        int pixelY;

        public void ScreenSettings()
        {
            Console.WriteLine("Select Resolution:\nnumpad1 -       480 x 270 (Native)\nnumpad2 - qHD   960 x 540 (Recommended)\nnnumpad3 - FHD 1920 x 1080 (Fullscreen!)\n\nEsitetyt vaatimukset tehtävän hyväksymistä varten:\n\nPelaajan liikkuminen ja törmääminen seiniin. DONE\nPelaajan liikkumisesta tulee ääntä. DONE\nPelaajan piirtäminen käyttämällä kuvia. DONE\nVihollisen olemassaolo. DONE\nVihollisen piirtäminen. DONE\nPelaaja törmää vihollisiin. DONE\nKentän piirtäminen käyttäen kuvia. DONE\nKentän lataaminen ja tallentaminen tiedostosta. DONE\nKenttien tekeminen kenttäeditorilla. DONE\n\nKaikki vaatimukset ovat täytetty noudatten niitä.\nPelissä liikutaan numpad  1 - 4, 6 - 9:llä.\nLadattuasi peliin paina numpad 2, 3 tai 6, jotta kenttä ilmestyy.\nPeli sisältää myös uniikin fontin.");
            
            while (true)
            {
                swf:
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.NumPad1:
                        ScreensizeX = 480;
                        ScreensizeY = 270;
                        resoPlier = 1;
                        break; //CONTINUE ohittaa loput silmukan koodista ja siirtyy silmukan alkuun
                               //BREAK lopettaa silmukan suorittamisen ennenaikaisesti ja jatkaa suoritusta ulkopuolelta
                               //RETURN palauttaa funkion tai metodin suorituksen keskeyttäen
                    case ConsoleKey.NumPad2:
                        ScreensizeX = 960;
                        ScreensizeY = 540;
                        resoPlier = 2;
                        break;
                    case ConsoleKey.NumPad3:
                        ScreensizeX = 1920;
                        ScreensizeY = 1080;
                        resoPlier = 4;
                        Raylib.ToggleFullscreen();
                        break;
                    default:
                        Console.WriteLine("Väärä arvo. Onkai numpad päällä?");
                        goto swf;
                }
                break;
            }
            
        }
        public void CharacterCreation()
        {
            PlayerCharacter player = new PlayerCharacter();
            Console.WriteLine("What is your name?");
            player.name = Console.ReadLine();
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Select race\n" + string.Join("\n", Enum.GetValues(typeof(Race)).Cast<Race>()));
                string ans = Console.ReadLine();
                if (ans == Race.Human.ToString() || ans == Race.Elf.ToString() || ans == Race.Orc.ToString() || ans == Race.Dwarf.ToString())
                {
                    //raceAnswer = (Race) player.rotu;
                    //player.rotu = raceAnswer;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Race");
                }
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Select class\n" + EnumHelper.ClassAlternatives);
                string ans = Console.ReadLine();

                if (EnumHelper.IsValidClass(ans))
                {
                    //Console.Clear();
                    //Console.WriteLine(ans, player);
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Class");
                }
            }
        }
        public void CheckTile()
        {
            if (mapproxy.GetLayer("ground").mapTiles[PosX + PosY * mapproxy.mapWidth] == 1 && mapproxy.GetLayer("enemies").mapTiles[PosX + PosY * mapproxy.mapWidth] == 0)
            {

                Console.SetCursorPosition(PosX, PosY);
                Raylib.PlaySound(kavely);
            }
            else
            {
                PosX = FormerX;
                PosY = FormerY;
            }
        }

        public void Run()
        {
            Console.CursorVisible = false;
            ScreenSettings();
            Console.Clear();
            level01 = loader.LoadMapFromFile("Maps/mapfile.json");  //loader.LoadTestMap();
            level02 = loader.LoadMapFromFile("Maps/mapfile_layers.json");
            level03 = TurboMapReader.MapReader.LoadMapFromFile("Maps/roguemappi.json"); //turbomapreader tiled mappi juttu
            //TileMap loadedTileMap = TurboMapReader.MapReader.LoadMapFromFile("Maps/roguemappi.json");
            mapproxy = level02;
            int indeksi = PosX + (PosY * 8 * resoPlier);
            Raylib.InitWindow(ScreensizeX, ScreensizeY, "test");
            Raylib.InitAudioDevice();
            kavely = Raylib.LoadSound("Audio/Kavely.wav");
            Texture pipsa = Raylib.LoadTexture("Textures/pipsa.png");
            seina = Raylib.LoadTexture("Textures/Seina.png");
            lattia = Raylib.LoadTexture("Textures/lattia.png");

            haloFontti = Raylib.LoadFont("Fonts/HALO____.TTF");
            // Define the source rectangle (entire texture)
            //Rectangle sourceRect = new Rectangle(0, 0, pipsa.width, pipsa.height);
            // Define the destination rectangle (where to draw and its size)
            //Rectangle destRect = new Rectangle(200, 0, pipsa.width / (6 * resoPlier), pipsa.height / (6 * resoPlier));

            sPlayerRect = new Rectangle(0, 0, pipsa.width, pipsa.height);
            dPlayerRect = new Rectangle(0, 0, 16 * resoPlier, 16 * resoPlier);

            sWallRect = new Rectangle(0, 0, seina.width, seina.height);
            dWallRect = new Rectangle(0, 0, 16 * resoPlier, 16 * resoPlier);

            sFloorRect = new Rectangle(0, 0, lattia.width, lattia.height);
            dFloorRect = new Rectangle(0, 0, 16 * resoPlier, 16 * resoPlier);

            Raylib.SetTargetFPS(30);
            bool game_running = true;
            while (game_running)
            {
                pixelX = (int)(PosX * Game.tileSize * resoPlier);
                pixelY = (int)(PosY * Game.tileSize * resoPlier);
                float baseSize = 16;
                int baseFixedSize = (int)Math.Round(baseSize, 0);
                Console.Clear();
                Raylib.ClearBackground(Raylib.BLACK);
                //Raylib.DrawTexturePro(pipsa, sourceRect, destRect, new Vector2(0, 0), 0, Raylib.WHITE);
                Raylib.DrawRectangle(pixelX, pixelY, 16 * resoPlier, 16 * resoPlier, Raylib.YELLOW);
                Raylib.DrawText("@", pixelX, pixelY, 16 * resoPlier, Raylib.BLACK);
                Raylib.DrawTexturePro(pipsa, sPlayerRect, dPlayerRect, new Vector2(-pixelX, -pixelY), 0, Raylib.WHITE);
                Draw();
                Console.SetCursorPosition(PosX, PosY);
                Console.Write("@");
                FormerX = PosX;
                FormerY = PosY;
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.NumPad9:
                        PosX = PosX + 1;
                        PosY = PosY - 1;
                        CheckTile();
                        //player.Move(1, -1);
                        break;
                    case ConsoleKey.NumPad8:
                        PosY = PosY - 1;
                        CheckTile();
                        //player.Move(0, -1);
                        break;
                    case ConsoleKey.NumPad7:
                        PosX = PosX - 1;
                        PosY = PosY - 1;
                        CheckTile();
                        //player.Move(-1, -1);
                        break;
                    case ConsoleKey.NumPad6:
                        PosX = PosX + 1;
                        CheckTile();
                        //player.Move(1, 0);
                        break;
                    case ConsoleKey.NumPad4:
                        PosX = PosX - 1;
                        CheckTile();
                        //player.Move(-1, 0);
                        break;
                    case ConsoleKey.NumPad3:
                        PosX = PosX + 1;
                        PosY = PosY + 1;
                        CheckTile();
                        //player.Move(1, 1);
                        break;
                    case ConsoleKey.NumPad2:
                        PosY = PosY + 1;
                        CheckTile();
                        //player.Move(0, 1);
                        break;
                    case ConsoleKey.NumPad1:
                        PosX = PosX - 1;
                        PosY = PosY + 1;
                        CheckTile();
                        //player.Move(-1, 1);
                        break;
                    case ConsoleKey.Escape:
                        Raylib.UnloadTexture(pipsa);
                        Raylib.UnloadSound(kavely);
                        Raylib.CloseWindow();
                        game_running = false;
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        //draw.GenerateMap();
                        //draw.DrawMap();
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        //draw.DrawMap();
                        break;
                    default:
                        break;
                }
            }
        }
        public void Draw()
        {
            /*TiledMap loadedTileMap = TurboMapReader.MapReader.LoadMapFromFile("Maps/roguekartta.json");
            TurboMapReader.MapReader.(loadedTileMap)
            TurboMapReader.MapReader.LoadMapFromFile("Maps/roguekartta.json");*/

            temp = Raylib.LoadTexture("Textures/pipsa.png");
            Raylib.BeginDrawing();
            Console.ForegroundColor = ConsoleColor.Gray; 
            Raylib.EndDrawing();
            MapLayer groundLayer = mapproxy.GetLayer("ground");
            MapLayer enemyLayer = mapproxy.GetLayer("enemies");//
            int[] mapTiles = groundLayer.mapTiles;
            int[] enemyTiles = enemyLayer.mapTiles;//
            int mapHeight = mapTiles.Length / mapproxy.mapWidth;
            int mapHeight2 = enemyTiles.Length / mapproxy.mapWidth;//
            for (int y = 0; y < mapHeight; y++) 
            {
                for (int x = 0; x < mapproxy.mapWidth; x++) 
                {
                    int pixelXw = (int)(x * Game.tileSize * resoPlier);
                    int pixelYw = (int)(y * Game.tileSize * resoPlier);
                    int index = x + y * mapproxy.mapWidth; 
                    int tileId = mapTiles[index];
                    Console.SetCursorPosition(x, y);
                    switch (tileId)
                    {
                        case 1:
                            Console.Write(".");
                            //Raylib.DrawRectangle(pixelXw, pixelYw,16 * resoPlier, 16 * resoPlier, Raylib.DARKGRAY);
                            dFloorRect = new Rectangle(pixelXw, pixelYw, 16 * resoPlier, 16 * resoPlier);
                            Raylib.DrawTexturePro(lattia, sFloorRect, dFloorRect, new Vector2(0, 0), 0, Raylib.WHITE);
                            break;
                        case 2:
                            Console.Write("#");
                            //Raylib.DrawRectangle(pixelXw, pixelYw, 16 * resoPlier, 16 * resoPlier, Raylib.BLUE);
                            dWallRect = new Rectangle(pixelXw, pixelYw, 16 * resoPlier, 16 * resoPlier);
                            Raylib.DrawTexturePro(seina, sWallRect, dWallRect, new Vector2(0, 0), 0, Raylib.WHITE);
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
            }
            for (int y = 0; y < mapHeight2; y++)
            {
                for (int x = 0; x < mapproxy.mapWidth; x++) 
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int pixelXw2 = (int)(x * Game.tileSize * resoPlier);
                    int pixelYw2 = (int)(y * Game.tileSize * resoPlier);
                    int index2 = x + y * mapproxy.mapWidth;
                    int tileId2 = enemyTiles[index2];
                    Console.SetCursorPosition(x, y);
                    switch (tileId2)
                    {
                        case 1:
                            // Tässä kohdassa kenttää ei ole vihollista
                            Raylib.DrawTextEx(haloFontti, "\n\nTama on kustom fontti.\n\n\n\n\nThis is a custom font.", new Vector2(pixelXw2, pixelYw2), 16 * resoPlier, 2, Raylib.PINK);
                            break;
                        case 2:
                            // Tässä kohdassa kenttää on örkki
                            // tileId on sama kuin drawIndex
                            Console.Write("Y");
                            Raylib.DrawRectangle(pixelXw2, pixelYw2, 16 * resoPlier, 16 * resoPlier, Raylib.RED);
                            Raylib.DrawTextEx(haloFontti ,"Y", new Vector2(pixelXw2, pixelYw2), 16 * resoPlier, 0, Raylib.BLACK);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
