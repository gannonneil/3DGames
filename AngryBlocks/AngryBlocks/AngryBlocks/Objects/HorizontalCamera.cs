using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using AngryBlocks.Base;
using AngryBlocks.Engines;

namespace AngryBlocks.Objects
{
    public class HorizontalCamera : Camera
    {
        public HorizontalCamera(string id, Vector3 position, Vector3 target, float aspectRatio)
            : base(id, position, target, aspectRatio)
        {
        }

        public override void Update(GameTime gametime)
        {
            if (InputEngine.IsKeyHeld(Keys.Left))
            {
                World *= Matrix.CreateTranslation(new Vector3(-0.1f, 0, 0));
            }
            else if (InputEngine.IsKeyHeld(Keys.Right))
            {
                World *= Matrix.CreateTranslation(new Vector3(0.1f, 0, 0));
            }

            if (InputEngine.IsKeyHeld(Keys.Up))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, 0, -0.1f));
            }
            else if (InputEngine.IsKeyHeld(Keys.Down))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, 0, 0.1f));
            }

            if (InputEngine.IsKeyHeld(Keys.Add))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, 0.1f, 0));
            }
            else if (InputEngine.IsKeyHeld(Keys.Subtract))
            {
                World *= Matrix.CreateTranslation(new Vector3(0, -0.1f, 0));
            }

            base.Update(gametime);
        }
    }
}
