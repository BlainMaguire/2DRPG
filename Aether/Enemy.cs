using System;
using System.Collections.Generic;
using System.Linq;
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

    class Enemy
    {
        public StatBar HP;
        public Texture2D battleGraphic;
        public Vector2 position;
        string name;
        public int expPoints;

        public Enemy(string name, Texture2D picture, StatBar hp, Vector2 position, int exp)
        {

            this.name = name;
            this.battleGraphic = picture;
            this.HP = hp;
            this.position = position;
            this.expPoints = exp;

        }

    }
}
