using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AngryBlocks.Engines;
using AngryBlocks.Base;
using AngryBlocks.Objects;
using AngryBlocks.Scenes;

namespace AngryBlocks
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        GameEngine engine;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            
            Content.RootDirectory = "Content";

            engine = new GameEngine(this);
        }

        protected override void Initialize()
        {
            Helpers.GraphicsDevice = GraphicsDevice;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            engine.LoadScene(new Level0(engine));
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputEngine.IsKeyPressed(Keys.Escape) || InputEngine.IsButtonPressed(Buttons.Back))
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            Helpers.RestoreGraphicsDeviceTo3D();
        }

    }
}
