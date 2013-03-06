using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;                                            
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Aether
{
    class Map
    {
        public int defaultTile = 0;

        public int tileSize = 24; // I seriously doubt we'll ever change the tile size dynamically but it's here anyway, and it certainly beats having magic numbers in the code.
        public int step = 2; // must be divisible by tile size or the player will get misaligned to the grid. should be in map class.

        public int mapWidth = 1;
        public int mapHeight = 1;

        public int mapNumber = -1;
        public int mapVersion = 1;
        public string mapName = "";
        public string mapDescription = "";

        public IList<MapLayer> layers = new List<MapLayer>();     

        public void GetMapDataFromFile(string fileName)
        {
            string fullPath = Path.Combine(StorageContainer.TitleLocation, fileName);
            
            StreamReader file = File.OpenText(fullPath);
            string fileString = file.ReadToEnd();

            string[] lines = fileString.Split(new char[] { ',' });
            string[] tileTypes;

            if (lines.GetLength(0) < 7)
            {
                throw new ArgumentException("Map file not valid. Data from file may be too small or incorrectly formatted");
            }

            else
            {
                    mapName = lines[0];
                    mapNumber = Convert.ToInt32(lines[1].Trim());
                    mapVersion = Convert.ToInt32(lines[2].Trim());
                    mapDescription = lines[3];

                    mapWidth = Convert.ToInt32(lines[4].Trim());
                    mapHeight = Convert.ToInt32(lines[5].Trim());

                    int mapSize = Math.Abs(mapWidth * mapHeight);

                    int[,] tileMap  = new int[mapHeight, mapWidth];

                    int max = 0;
                    int currentMaptile = 0;

                   
                     for (int y = 0; y < mapHeight; y++)
                     {
                            for (int x = 0; x < mapWidth; x++)
                            {

                            currentMaptile = Convert.ToInt32(lines[(y + x * mapHeight) + 7].Trim());

                            tileMap[y, x] = currentMaptile;

                            if (currentMaptile > max)
                            {
                                max = currentMaptile;
                            }

                        }
                    }

                    tileTypes = lines[6].Split(new char[] { '.' });
                    int[] passMap = new int[max+1];
                    string[] temp = new string[2];

                    for (int i = 0; i < tileTypes.GetLength(0); i++)
                    {
                        if (!tileTypes[i].Contains(":"))
                        {
                            throw new ArgumentException("Error parsing passmap for tile set for map. Could not find: \":\"");
                        }

                        else
                        {
                            temp = tileTypes[i].Split(new char[] {':'});
                            temp[0].Replace("\r\n","");
                            temp[1].Replace("\r\n","");
                            passMap[Convert.ToInt32(temp[0])] = Convert.ToInt32(temp[1]);
                        }
                    }

                  this.layers.Add(new MapLayer(passMap,tileMap,false));

            }

        }

        public void GetTilesetData(Texture2D mapTiles)
        {

            if (mapTiles.Width < tileSize || mapTiles.Height < tileSize)
            {
                throw new ArgumentOutOfRangeException("mapTiles", "Maptiles texture must be at least one tile in size.");
            }

            this.layers[layers.Count -1].mapTiles = mapTiles;
            this.layers[layers.Count - 1].tilesetX = mapTiles.Width / this.tileSize;
            this.layers[layers.Count - 1].tilesetY = mapTiles.Height / this.tileSize;

        }

        // this is just a temporary work around
        public void GetTilesetData(int mapLayerIndex, Texture2D mapTiles)
        {
            if (mapTiles.Width < tileSize || mapTiles.Height < tileSize)
            {
                throw new ArgumentOutOfRangeException("mapTiles", "Maptiles texture must be at least one tile in size.");
            }
            
            this.layers[mapLayerIndex].mapTiles = mapTiles;
            this.layers[mapLayerIndex].tilesetX = mapTiles.Width / this.tileSize;
            this.layers[mapLayerIndex].tilesetY = mapTiles.Height / this.tileSize;
        }

        public void WriteFile(string fileName)
        {
            string fullPath = Path.Combine(StorageContainer.TitleLocation, fileName);
            StreamWriter writer = new StreamWriter(fullPath);
            writer.Write(DateTime.Now.ToString());
            writer.Flush();
            writer.Close();
        }


    }
}
