﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AngryBlocks.Base;
using AngryBlocks.Engines;

namespace AngryBlocks.Objects
{
    public class SimpleLitTexturedTriangle : GameObject3D
    {
        VertexPositionNormalTexture[] _vertices;
        BasicEffect _effect;
        Texture2D _texture;

        string _asset;

        public SimpleLitTexturedTriangle(string id, string asset, Vector3 position)
            : base(id, position)
        {
            _asset = asset;
        }

        public override void LoadContent(ContentManager _content)
        {
            //local coordinates, World will transform these to global coords
            _vertices = new VertexPositionNormalTexture[3];

            _vertices[0].Position = new Vector3(-1, -1, 0);
            _vertices[0].TextureCoordinate = new Vector2(0, 1);
            _vertices[0].Normal = Vector3.Forward;

            _vertices[1].Position = new Vector3(0, 1, 0);
            _vertices[1].TextureCoordinate = new Vector2(0, 0);
            _vertices[0].Normal = Vector3.Forward;

            _vertices[2].Position = new Vector3(1, -1, 0);
            _vertices[2].TextureCoordinate = new Vector2(1, 1);
            _vertices[0].Normal = Vector3.Forward;

            _effect = new BasicEffect(Helpers.GraphicsDevice);
            _effect.TextureEnabled = true;
            
            _texture = _content.Load<Texture2D>("Textures\\" + _asset);
            _effect.Texture = _texture;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }

        public override void Draw(Camera _camera)
        {
            _effect.View = _camera.View;
            _effect.Projection = _camera.Projection;
            _effect.World = World;
            
            _effect.EnableDefaultLighting();

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Helpers.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList, 
                    _vertices, 
                    0,
                    1,
                    VertexPositionNormalTexture.VertexDeclaration);
            }

            base.Draw(_camera);
        }
    }
}
