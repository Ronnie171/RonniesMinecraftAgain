using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestInterfaces;
using TestUtils;
using UnityEngine;

namespace Tests.EditMode {
    public class Test12a_BlockRendering : TestSuite {
        private static readonly string[] cubeCreators = new[]{
            Assets.defaultCubeCreator,
            Assets.defaultCubeMesh,
            Assets.topSideBottomCubeCreator,
            Assets.topSideBottomCubeMesh,
        };
        public enum QuadChecks {
            CheckVertices,
            CheckNormals,
            CheckUVs,
        }
        private static readonly QuadChecks[] quadChecks = new[] {
            QuadChecks.CheckVertices,
            QuadChecks.CheckNormals,
            QuadChecks.CheckUVs,
        };

        #region Aufgabe 01
        [TestCase(Assets.defaultCubeCreator)]
        [TestCase(Assets.topSideBottomCubeCreator)]
        public void A01a_CubeCreatorExists(string path) {
            LoadAsset<ScriptableObject>(path);
        }
        [TestCase(Assets.defaultCubeCreator)]
        [TestCase(Assets.topSideBottomCubeCreator)]
        public void A01b_CubeCreatorImplementsIMeshCreator(string path) {
            LoadAsset<IMeshCreator>(path);
        }
        [TestCase(Assets.defaultCubeCreator)]
        [TestCase(Assets.topSideBottomCubeCreator)]
        public void A01c_CubeCreatorCreatesMesh(string path) {
            var creator = LoadAsset<IMeshCreator>(path);
            var mesh = new Mesh();
            creator.RecreateMesh(mesh);
            Assert.IsNotNull(mesh, $"{creator} should return a new mesh instance!");
        }
        [TestCase(Assets.defaultCubeMesh)]
        [TestCase(Assets.topSideBottomCubeMesh)]
        public void A01d_CubeMeshExists(string path) {
            LoadAsset<Mesh>(path);
        }
        [TestCase(Assets.defaultCubeCreator, 24)]
        [TestCase(Assets.defaultCubeMesh, 24)]
        [TestCase(Assets.topSideBottomCubeCreator, 24)]
        [TestCase(Assets.topSideBottomCubeMesh, 24)]
        public void A01e_CubeHasTotalVertices(string path, int vertexCount) {
            var (mesh, name) = LoadMesh(path);
            Assert.AreEqual(vertexCount, mesh.vertexCount, $"{name} should have exactly {vertexCount} vertices!");
        }
        [TestCase(Assets.defaultCubeCreator, 8)]
        [TestCase(Assets.defaultCubeMesh, 8)]
        [TestCase(Assets.topSideBottomCubeCreator, 8)]
        [TestCase(Assets.topSideBottomCubeCreator, 8)]
        public void A01f_CubeCreatorMeshHasDistinctVertices(string path, int vertexCount) {
            var (mesh, name) = LoadMesh(path);
            var distinct = new HashSet<string>(mesh.vertices.Select(vertex => vertex.ToString()));
            Assert.AreEqual(vertexCount, distinct.Count, $"{name} should have exactly {vertexCount} distinct vertices!");
        }
        [TestCase(Assets.defaultCubeCreator, 24)]
        [TestCase(Assets.defaultCubeMesh, 24)]
        [TestCase(Assets.topSideBottomCubeCreator, 24)]
        [TestCase(Assets.topSideBottomCubeMesh, 24)]
        public void A01g_CubeCreatorMeshHasTotalNormals(string path, int normalCount) {
            var (mesh, name) = LoadMesh(path);
            Assert.AreEqual(normalCount, mesh.normals.Length, $"{name} should have exactly {normalCount} normals!");
        }
        [TestCase(Assets.defaultCubeCreator, 6)]
        [TestCase(Assets.defaultCubeMesh, 6)]
        [TestCase(Assets.topSideBottomCubeCreator, 6)]
        [TestCase(Assets.topSideBottomCubeMesh, 6)]
        public void A01h_CubeCreatorMeshHasDistinctNormals(string path, int normalCount) {
            var (mesh, name) = LoadMesh(path);
            var distinct = new HashSet<string>(mesh.normals.Select(normal => normal.ToString()));
            Assert.AreEqual(normalCount, distinct.Count, $"{name} should have exactly {normalCount} distinct normals!");
        }
        [TestCase(Assets.defaultCubeCreator, 24)]
        [TestCase(Assets.defaultCubeMesh, 24)]
        [TestCase(Assets.topSideBottomCubeCreator, 24)]
        [TestCase(Assets.topSideBottomCubeMesh, 24)]
        public void A01i_CubeCreatorMeshHasTotalUVs(string path, int uvCount) {
            var (mesh, name) = LoadMesh(path);
            Assert.AreEqual(uvCount, mesh.uv.Length, $"{name} should have exactly {uvCount} uvs!");
        }
        [TestCase(Assets.defaultCubeCreator, 4)]
        [TestCase(Assets.defaultCubeMesh, 4)]
        [TestCase(Assets.topSideBottomCubeCreator, 4)]
        [TestCase(Assets.topSideBottomCubeMesh, 4)]
        public void A01j_CubeCreatorMeshHasDistinctUVs(string path, int uvCount) {
            var (mesh, name) = LoadMesh(path);
            var distinct = new HashSet<string>(mesh.uv.Select(uv => uv.ToString()));
            Assert.AreEqual(uvCount, distinct.Count, $"{name} should have exactly {uvCount} distinct uvs!");
        }
        [TestCase(Assets.defaultCubeCreator, MeshTopology.Quads)]
        [TestCase(Assets.defaultCubeMesh, MeshTopology.Quads)]
        [TestCase(Assets.topSideBottomCubeCreator, MeshTopology.Quads)]
        [TestCase(Assets.topSideBottomCubeMesh, MeshTopology.Quads)]
        public void A01k_CubeCreatorMeshUsesTopology(string path, MeshTopology topology) {
            var (mesh, name) = LoadMesh(path);
            for (int i = 0; i < mesh.subMeshCount; i++) {
                Assert.AreEqual(topology, mesh.GetTopology(i), $"{name}'s submesh #{i} should use the {topology} topology!");
            }
        }
        [TestCase(Assets.defaultCubeCreator, 1)]
        [TestCase(Assets.defaultCubeMesh, 1)]
        [TestCase(Assets.topSideBottomCubeCreator, 3)]
        [TestCase(Assets.topSideBottomCubeMesh, 3)]
        public void A01l_CubeCreatorMeshHasCorrectNumberOfSubmeshes(string path, int subMeshCount) {
            var (mesh, name) = LoadMesh(path);
            Assert.AreEqual(subMeshCount, mesh.subMeshCount, $"{name} should have exactly {subMeshCount} submeshes!");
        }
        [TestCase(Assets.defaultCubeCreator, 6)]
        [TestCase(Assets.defaultCubeMesh, 6)]
        [TestCase(Assets.topSideBottomCubeCreator, 1, 4, 1)]
        [TestCase(Assets.topSideBottomCubeMesh, 1, 4, 1)]
        public void A01m_CubeCreatorMeshHasCorrectNumberOfQuadsPerSubmesh(string path, params int[] quadsPerSubmesh) {
            var (mesh, name) = LoadMesh(path);
            Assert.AreEqual(quadsPerSubmesh.Length, mesh.subMeshCount, $"{name} should have exactly {quadsPerSubmesh.Length} submeshes!");
            for (int i = 0; i < mesh.subMeshCount; i++) {
                Assert.AreEqual(quadsPerSubmesh[i], mesh.GetIndexCount(i) / 4, $"{name} should have exactly {quadsPerSubmesh[i]} quads in submesh #{i}!");
            }
        }
        [Test]
        public void A01n_CubeCreatorMeshQuadsMakeSense(
            [ValueSource(nameof(cubeCreators))] string path,
            [ValueSource(nameof(quadChecks))] QuadChecks check) {

            var (mesh, name) = LoadMesh(path);
            var allVertices = mesh.vertices;
            var allNormals = mesh.normals;
            var allUV = mesh.uv;
            for (int i = 0; i < mesh.subMeshCount; i++) {
                var indices = mesh.GetIndices(i);
                for (int j = 0; j < indices.Length; j += 4) {
                    var vertices = new HashSet<Vector3>();
                    var normals = new HashSet<Vector3>();
                    var uvs = new HashSet<Vector2>();
                    for (int k = 0; k < 4; k++) {
                        vertices.Add(allVertices[indices[j + k]]);
                        normals.Add(allNormals[indices[j + k]]);
                        uvs.Add(allUV[indices[j + k]]);
                    }

                    switch (check) {
                        case QuadChecks.CheckVertices:
                            Assert.AreEqual(4, vertices.Count, $"All 4 vertices of the same quad should be at different positions! (checked submesh #{i} of {name})");
                            break;
                        case QuadChecks.CheckNormals:
                            Assert.AreEqual(1, normals.Count, $"All 4 normals of the same quad should point in the same direction! (checked submesh #{i} of {name})");
                            break;
                        case QuadChecks.CheckUVs:
                            CollectionAssert.Contains(uvs, Vector2.zero, $"One of the uvs of a quad should be {Vector2.zero}! (checked submesh #{i} of {name})");
                            CollectionAssert.Contains(uvs, Vector2.right, $"One of the uvs of a quad should be {Vector2.right}! (checked submesh #{i} of {name})");
                            CollectionAssert.Contains(uvs, Vector2.one, $"One of the uvs of a quad should be {Vector2.one}! (checked submesh #{i} of {name})");
                            CollectionAssert.Contains(uvs, Vector2.up, $"One of the uvs of a quad should be {Vector2.up}! (checked submesh #{i} of {name})");
                            break;
                    }
                }
            }
        }
        #endregion

        #region Aufgabe 02
        [TestCase(Assets.terrainTexture)]
        public void A02a_TerrainTextureExists(string path) {
            LoadAsset<Texture>(path);
        }
        [TestCase(Assets.terrainTexture)]
        public void A02b_TerrainIsTexture2DArray(string path) {
            LoadAsset<Texture2DArray>(path);
        }
        [TestCase(Assets.terrainTexture, 256)]
        public void A02c_TerrainHasCorrectNumberOfTextures(string path, int textureCount) {
            var texture = LoadAsset<Texture2DArray>(path);
            for (int i = 0; i < textureCount; i++) {
                Color[] pixels;
                try {
                    pixels = texture.GetPixels(i);
                } catch {
                    pixels = null;
                }
                Assert.IsNotNull(pixels, $"Terrain {texture} should contain {textureCount} textures!");
            }
        }
        #endregion

        #region Aufgabe 03
        [TestCase(Assets.terrainShader)]
        public void A03a_BlockShaderExists(string path) {
            LoadAsset<Shader>(path);
        }
        [TestCase(Assets.terrainShader, "_Texture_ID")]
        [TestCase(Assets.terrainShader, "_AlphaClip")]
        public void A03b_BlockShaderHasProperty(string path, string property) {
            var shader = LoadAsset<Shader>(path);
            var index = shader.FindPropertyIndex(property);
            Assert.GreaterOrEqual(index, 0, $"Shader {shader} does not expose a property '{property}'");
        }
        #endregion

        private (Mesh mesh, string name) LoadMesh(string path) {
            var asset = LoadAsset<Object>(path);
            if (asset is Mesh mesh) {
                return (mesh, asset.name);
            }
            if (asset is IMeshCreator creator) {
                mesh = new Mesh();
                creator.RecreateMesh(mesh);
                return (mesh, $"{asset.name}'s name");
            }
            throw new AssertionException($"Object at path '{path}' is neither a {typeof(Mesh)} nor a {typeof(IMeshCreator)}!");
        }
    }
}
