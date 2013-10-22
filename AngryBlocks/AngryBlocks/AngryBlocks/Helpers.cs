using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AngryBlocks
{
    public static class Helpers
    {
        public static GraphicsDevice GraphicsDevice { get; set; }

        public static void RestoreGraphicsDeviceTo3D()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }


        public static BoundingBox TransformBoundingBox(BoundingBox origBox, Matrix matrix)
        {
            Vector3 origCorner1 = origBox.Min;
            Vector3 origCorner2 = origBox.Max;

            Vector3 transCorner1 = Vector3.Transform(origCorner1, matrix);
            Vector3 transCorner2 = Vector3.Transform(origCorner2, matrix);

            return new BoundingBox(transCorner1, transCorner2);
        }

        public static List<Vector3> ExtractVector3FromMesh(ModelMesh mesh, Matrix[] boneTransforms)
        {
            //to store all the extracted vertice positions, wiull be returned
            List<Vector3> vertices = new List<Vector3>();

            //the possible transform of the current mesh bone
            Matrix _transform;
            //if it has a bone then get its matric transform
            if (mesh.ParentBone != null)
            {
                _transform = boneTransforms[mesh.ParentBone.Index];
            }
            else
            {
                //othjerwise set it an Identity matrix (Scale = 1)
                _transform = Matrix.Identity;
            }

            //loop through each mesh part
            foreach (ModelMeshPart part in mesh.MeshParts)
            {
                //create an array to match the number of vertices in the part
                var meshPartVertices = new Vector3[part.NumVertices];

                //use GetDat to extract the positions
                part.VertexBuffer.GetData(meshPartVertices);

                //transform, the vertices using the bone transofrm from above
                Vector3.Transform(meshPartVertices, ref _transform, meshPartVertices);

                //add to the List of Vector3, reapet
                vertices.AddRange(meshPartVertices);
            }
            return vertices;
        }

        public static List<Vector3> ExtractVector3FromModel(Model _model)
        {
            List<Vector3> vertices = new List<Vector3>();

            //an array to store the transforms of each bone in the model
            var boneTransforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            Matrix _transform;
            foreach (ModelMesh mesh in _model.Meshes)
            {
                if (mesh.ParentBone != null)
                {
                    _transform = boneTransforms[mesh.ParentBone.Index];
                }
                else
                {
                    _transform = Matrix.Identity;
                }

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    int startIndex = vertices.Count;

                    //how many vertices are the current mesh part
                    var meshPartVertices = new Vector3[part.NumVertices];

                    //stride is the size in bytes between each vertex
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    //using the above data we can extract the vertices from the mesh part
                    part.VertexBuffer.GetData(
                            part.VertexOffset * stride,
                            meshPartVertices,
                            0,
                            part.NumVertices,
                            stride);

                    //ensure our vertices have been transofmed using their parents transformation
                    Vector3.Transform(meshPartVertices, ref _transform, meshPartVertices);

                    //add them to the list
                    vertices.AddRange(meshPartVertices);
                }
            }

            return vertices;
        }

        public static void GetModelPoints(Model _model, out List<Vector3> vertices, out List<int> indices)
        {
            //will conatin teh data to be returned
            vertices = new List<Vector3>();
            indices = new List<int>();

            //an array to store the transforms of each bone in the model
            var boneTransforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            Matrix _transform;
            foreach (ModelMesh mesh in _model.Meshes)
            {
                if (mesh.ParentBone != null)
                {
                    _transform = boneTransforms[mesh.ParentBone.Index];
                }
                else
                {
                    _transform = Matrix.Identity;
                }
            }
        }

        private static void ExtractData(ModelMesh modelMesh, Matrix transform, List<Vector3> vertices, IList<int> indices)
        {
            foreach (ModelMeshPart meshPart in modelMesh.MeshParts)
            {
                //we want to continue adding our new vertices on to the existing list
                //get the count and start from there
                int startIndex = vertices.Count;

                //how many vertices are the current mesh part
                var meshPartVertices = new Vector3[meshPart.NumVertices];

                //stride is the size in bytes between each vertex
                int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                //using the above data we can extract the vertices from the mesh part
                meshPart.VertexBuffer.GetData(
                        meshPart.VertexOffset * stride,
                        meshPartVertices,
                        0,
                        meshPart.NumVertices,
                        stride);

                //ensure our vertices have been transofmed using their parents transformation
                Vector3.Transform(meshPartVertices, ref transform, meshPartVertices);

                //add them to the list
                vertices.AddRange(meshPartVertices);

                if (meshPart.IndexBuffer.IndexElementSize == IndexElementSize.ThirtyTwoBits)
                {
                    var meshIndices = new int[meshPart.PrimitiveCount * 3];

                    meshPart.IndexBuffer.GetData(meshPart.StartIndex * 4, meshIndices, 0, meshPart.PrimitiveCount * 3);

                    for (int k = 0; k < meshIndices.Length; k++)
                    {
                        indices.Add(startIndex + meshIndices[k]);
                    }
                }
                else
                {
                    var meshIndices = new ushort[meshPart.PrimitiveCount * 3];

                    meshPart.IndexBuffer.GetData(meshPart.StartIndex * 2, meshIndices, 0, meshPart.PrimitiveCount * 3);

                    for (int k = 0; k < meshIndices.Length; k++)
                    {
                        indices.Add(startIndex + meshIndices[k]);
                    }
                }
            }
        }

        public static void GetVerticesAndIndicesFromModel(Model collisionModel, out Vector3[] vertices, out int[] indices)
        {
            var verticesList = new List<Vector3>();
            var indicesList = new List<int>();
            var transforms = new Matrix[collisionModel.Bones.Count];
            collisionModel.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix transform;
            foreach (ModelMesh mesh in collisionModel.Meshes)
            {
                if (mesh.ParentBone != null)
                    transform = transforms[mesh.ParentBone.Index];
                else
                    transform = Matrix.Identity;
                AddMesh(mesh, transform, verticesList, indicesList);
            }

            vertices = verticesList.ToArray();
            indices = indicesList.ToArray();
        }

        /// <summary>
        /// Adds a mesh's vertices and indices to the given lists.
        /// </summary>
        /// <param name="collisionModelMesh">Model to use for the collision shape.</param>
        /// <param name="transform">Transform to apply to the mesh.</param>
        /// <param name="vertices">List to receive vertices from the mesh.</param>
        /// <param name="indices">List to receive indices from the mesh.</param>
        public static void AddMesh(ModelMesh collisionModelMesh, Matrix transform, List<Vector3> vertices, IList<int> indices)
        {
            foreach (ModelMeshPart meshPart in collisionModelMesh.MeshParts)
            {
                int startIndex = vertices.Count;
                var meshPartVertices = new Vector3[meshPart.NumVertices];

                //Grab position data from the mesh part.
                int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                meshPart.VertexBuffer.GetData(
                        meshPart.VertexOffset * stride,
                        meshPartVertices,
                        0,
                        meshPart.NumVertices,
                        stride);

                //Transform it so its vertices are located in the model's space as opposed to mesh part space.
                Vector3.Transform(meshPartVertices, ref transform, meshPartVertices);
                vertices.AddRange(meshPartVertices);

                if (meshPart.IndexBuffer.IndexElementSize == IndexElementSize.ThirtyTwoBits)
                {
                    var meshIndices = new int[meshPart.PrimitiveCount * 3];
                    meshPart.IndexBuffer.GetData(meshPart.StartIndex * 4, meshIndices, 0, meshPart.PrimitiveCount * 3);
                    for (int k = 0; k < meshIndices.Length; k++)
                    {
                        indices.Add(startIndex + meshIndices[k]);
                    }
                }
                else
                {
                    var meshIndices = new ushort[meshPart.PrimitiveCount * 3];
                    meshPart.IndexBuffer.GetData(meshPart.StartIndex * 2, meshIndices, 0, meshPart.PrimitiveCount * 3);
                    for (int k = 0; k < meshIndices.Length; k++)
                    {
                        indices.Add(startIndex + meshIndices[k]);
                    }
                }
            }
        }
    }
}
