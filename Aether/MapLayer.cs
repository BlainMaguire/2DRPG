using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Aether
{

    class MapLayer
    {

    public bool isAlphaLayer = false;
    public bool isEnabled = true;

    public Texture2D mapTiles;
    public int tilesetX;
    public int tilesetY;

    public int[] passMap;
    public int[,] tileMap;

    public MapLayer(int[] passMap, int[,] tileMap) {

        this.passMap = passMap;
        this.tileMap = tileMap;
    
    }

    public MapLayer(int[] passMap, int[,] tileMap, bool isAlphaLayer)
    {

        this.passMap = passMap;
        this.tileMap = tileMap;
        this.isAlphaLayer = isAlphaLayer;
    }

    }
}
