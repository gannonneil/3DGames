using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AngryBlocks.Base;

namespace AngryBlocks.Objects
{
    public class IndexingSampleVB : GameObject3D
    {
        VertexPositionColor[] _vertices;
        int[] _indices;

        VertexBuffer _vbuffer;
        IndexBuffer _ibuffer;

        BasicEffect _effect;
        RasterizerState _state = new RasterizerState() { FillMode = FillMode.WireFrame };

        public IndexingSampleVB(string id, Vector3 position)
            : base(id, position)
        {

        }

        public override void LoadContent(ContentManager _content)
        {
            _vertices = new VertexPositionColor[5];

            _vertices[0].Position = new Vector3(0f, 0f, 0f);
            _vertices[0].Color = Color.Black;

            _vertices[1].Position = new Vector3(5f, 0f, 0f);
            _vertices[1].Color = Color.Black;

            _vertices[2].Position = new Vector3(10f, 0f, 0f);
            _vertices[2].Color = Color.Black;

            _vertices[3].Position = new Vector3(5f, 0f, -5f);
            _vertices[3].Color = Color.Black;

            _vertices[4].Position = new Vector3(10f, 0f, -5f);
            _vertices[4].Color = Color.Black;

            _indices = new int[6];

            _indices[0] = 3;
            _indices[1] = 1;
            _indices[2] = 0;
            _indices[3] = 4;
            _indices[4] = 2;
            _indices[5] = 1;

            _vbuffer = new VertexBuffer(Helpers.GraphicsDevice, typeof(VertexPositionColor), _vertices.Length, BufferUsage.WriteOnly);
            _vbuffer.SetData<VertexPositionColor>(_vertices);

            _ibuffer = new IndexBuffer(Helpers.GraphicsDevice, typeof(int), _indices.Length, BufferUsage.WriteOnly);
            _ibuffer.SetData<int>(_indices);

            _effect = new BasicEffect(Helpers.GraphicsDevice);
            _effect.VertexColorEnabled = true;
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

            Helpers.GraphicsDevice.RasterizerState = _state;

            Helpers.GraphicsDevice.SetVertexBuffer(_vbuffer);
            Helpers.GraphicsDevice.Indices = _ibuffer;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Helpers.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    0,
                    _vertices.Length,
                    0,
                    _indices.Length / 3);
            }

            base.Draw(_camera);
        }
    }
}
