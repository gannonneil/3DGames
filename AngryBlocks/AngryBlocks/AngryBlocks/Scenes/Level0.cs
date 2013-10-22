using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AngryBlocks.Base;
using AngryBlocks.Objects;
using AngryBlocks.Engines;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AngryBlocks.Scenes
{
    public class Level0 : Scene
    {
        Camera _simpleCamera;

        SimpleModel _cube0;

        public Level0(GameEngine _engine)
            : base("Level0", _engine){ }

        public override void Initialize()
        {
            _simpleCamera = new HorizontalCamera(
                                    "cam0",
                                    new Vector3(0, 2, 20),
                                    new Vector3(0, 5, 0),
                                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio);

            Engine.Cameras.AddCamera(_simpleCamera);

            _cube0 = new SimpleModel("cube0", "smallcube", new Vector3(0,0,0));

            //pass directly to the Scene, otherwise if we want to destory the object it would still exist in the Level0
            AddObject(new SimpleModel("cube1", "smallcube", new Vector3(6, 0, 0)));
            AddObject(new SimpleModel("cube2", "smallcube", new Vector3(-6, 0, 0)));
            AddObject(new SimpleModel("cube3", "smallcube", new Vector3(0, 6, 0)));
            AddObject(new SimpleModel("cube4", "smallcube", new Vector3(0, -6, 0)));

            AddObject(_cube0);

            //place in Audio\Effects folder
            //http://www.freesound.org/people/Sumanguru%20Gyra%20Jones/sounds/41329/
            //Engine.Audio.LoadEffect("ow");
            //uncomment the Engine.Audio.PlayEffect("ow"); in the SampleRayTests method
            base.Initialize();
        }

        public override void Update(GameTime gametime)
        {
            SampleRayTests();

            base.Update(gametime);
        }

        private void SampleRayTests()
        {
            //create a ray from cube 0 (centre) and point it in the right hand direction
            Ray _cubeRay = new Ray(_cube0.World.Translation, Vector3.Right);
            float? hitResult;
            if (GetObject("cube1") != null)
            {
                //does it hit the cube on teh right?
                hitResult = DoesRayIntersectWith(_cubeRay, (GetObject("cube1") as SimpleModel).AABB);
                if (hitResult != null)
                {
                    //if so draw a line to the hit object
                    DebugEngine.AddLine(_cube0.World.Translation, GetObject("cube1").World.Translation, Color.Red);
                }

                if (_cube0.AABB.Intersects((GetObject("cube1") as SimpleModel).AABB))
                {
                    GetObject("cube1").Destroy();
                    //Engine.Audio.PlayEffect("ow");
                }
            }

            //reset
            hitResult = null;
            //repeat but from the left
            _cubeRay = new Ray(_cube0.World.Translation, Vector3.Left);

            if (GetObject("cube2") != null)
            {
                hitResult = DoesRayIntersectWith(_cubeRay, (GetObject("cube2") as SimpleModel).AABB);
                if (hitResult != null)
                {
                    DebugEngine.AddLine(_cube0.World.Translation, GetObject("cube2").World.Translation, Color.LawnGreen);
                }

                if (_cube0.AABB.Intersects((GetObject("cube2") as SimpleModel).AABB))
                {
                    GetObject("cube2").Destroy();
                   // Engine.Audio.PlayEffect("ow");
                }
            }

            //reset
            hitResult = null;
            //repeat but from the left
            _cubeRay = new Ray(_cube0.World.Translation, Vector3.Up);

            if (GetObject("cube3") != null)
            {
                hitResult = DoesRayIntersectWith(_cubeRay, (GetObject("cube3") as SimpleModel).AABB);
                if (hitResult != null)
                {
                    DebugEngine.AddLine(_cube0.World.Translation, GetObject("cube3").World.Translation, Color.LawnGreen);
                }

                if (_cube0.AABB.Intersects((GetObject("cube3") as SimpleModel).AABB))
                {
                    GetObject("cube3").Destroy();
                   // Engine.Audio.PlayEffect("ow");
                }
            }

            //reset
            hitResult = null;
            //repeat but from the left
            _cubeRay = new Ray(_cube0.World.Translation, Vector3.Down);

            if (GetObject("cube4") != null)
            {
                hitResult = DoesRayIntersectWith(_cubeRay, (GetObject("cube4") as SimpleModel).AABB);
                if (hitResult != null)
                {
                    DebugEngine.AddLine(_cube0.World.Translation, GetObject("cube4").World.Translation, Color.Red);
                }


                if (_cube0.AABB.Intersects((GetObject("cube4") as SimpleModel).AABB))
                {
                    GetObject("cube4").Destroy();
                    //Engine.Audio.PlayEffect("ow");
                }
            }

        }

        public float? DoesRayIntersectWith(Ray _ray, BoundingBox _otherBox)
        {
            return _ray.Intersects(_otherBox);
        }

        protected override void HandlInput()
        {
            if (InputEngine.IsKeyHeld(Keys.A))
            {
                _cube0.World *= Matrix.CreateTranslation(-0.1f, 0, 0);
                _cube0.UpdateBoundingBox(Matrix.CreateTranslation(-0.1f, 0, 0));
            }
            else if (InputEngine.IsKeyHeld(Keys.D))
            {
                _cube0.World *= Matrix.CreateTranslation(0.1f, 0, 0);
                _cube0.UpdateBoundingBox(Matrix.CreateTranslation(0.1f, 0, 0));
            }

            if (InputEngine.IsKeyHeld(Keys.W))
            {
                _cube0.World *= Matrix.CreateTranslation(0, 0.1f, 0);
                _cube0.UpdateBoundingBox(Matrix.CreateTranslation(0, 0.1f, 0));
            }
            else if (InputEngine.IsKeyHeld(Keys.S))
            {
                _cube0.World *= Matrix.CreateTranslation(0, -0.1f, 0);
                _cube0.UpdateBoundingBox(Matrix.CreateTranslation(0, -0.1f, 0));
            }

            base.HandlInput();
        }
    }
}
