using NUnit.Framework;
using TestInterfaces;
using TestUtils;
using UnityEngine;

namespace Tests.EditMode {
    public class Test12b_BlockRendering : TestSuite {
        #region Aufgabe 04
        [TestCase(Assets.stoneBlockPrefab)]
        [TestCase(Assets.sandBlockPrefab)]
        [TestCase(Assets.dirtBlockPrefab)]
        [TestCase(Assets.grassBlockPrefab)]
        [TestCase(Assets.glassBlockPrefab)]
        [TestCase(Assets.leavesBlockPrefab)]
        [TestCase(Assets.gravelBlockPrefab)]
        [TestCase(Assets.logBlockPrefab)]
        [TestCase(Assets.woodenPlankBlockPrefab)]
        [TestCase(Assets.cobblestoneBlockPrefab)]
        public void A04a_BlockPrefabContainsMeshRenderer(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshRenderer>(prefab);
        }
        [TestCase(Assets.stoneBlockPrefab, false)]
        [TestCase(Assets.sandBlockPrefab, false)]
        [TestCase(Assets.dirtBlockPrefab, false)]
        [TestCase(Assets.grassBlockPrefab, false)]
        [TestCase(Assets.glassBlockPrefab, true)]
        [TestCase(Assets.leavesBlockPrefab, true)]
        [TestCase(Assets.gravelBlockPrefab, false)]
        [TestCase(Assets.logBlockPrefab, false)]
        [TestCase(Assets.woodenPlankBlockPrefab, false)]
        [TestCase(Assets.cobblestoneBlockPrefab, false)]
        public void A04b_BlockHasCorrectTransparency(string path, bool isTransparent) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshRenderer>(prefab, out var renderer);
            foreach (var material in renderer.sharedMaterials) {
                if (isTransparent) {
                    Assert.AreEqual(1, material.GetInt(Assets.alphaClipProperty), $"Material {material} of Block {prefab} should use alpha clipping!");
                } else {
                    Assert.AreEqual(0, material.GetInt(Assets.alphaClipProperty), $"Material {material} of {prefab} should NOT use alpha clipping!");
                }
            }
        }
        [TestCase(Assets.stoneBlockPrefab, 1)]
        [TestCase(Assets.sandBlockPrefab, 1)]
        [TestCase(Assets.dirtBlockPrefab, 1)]
        [TestCase(Assets.grassBlockPrefab, 3)]
        [TestCase(Assets.glassBlockPrefab, 1)]
        [TestCase(Assets.leavesBlockPrefab, 1)]
        [TestCase(Assets.gravelBlockPrefab, 1)]
        [TestCase(Assets.logBlockPrefab, 3)]
        [TestCase(Assets.woodenPlankBlockPrefab, 1)]
        [TestCase(Assets.cobblestoneBlockPrefab, 1)]
        public void A04c_BlockHasCorrectNumberOfMaterials(string path, int materialCount) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshRenderer>(prefab, out var renderer);
            Assert.AreEqual(materialCount, renderer.sharedMaterials.Length, $"Block {prefab} should use exactly {materialCount} materials!!");
        }
        [TestCase(Assets.stoneBlockPrefab, 1)]
        [TestCase(Assets.sandBlockPrefab, 18)]
        [TestCase(Assets.dirtBlockPrefab, 2)]
        [TestCase(Assets.grassBlockPrefab, 0)]
        [TestCase(Assets.glassBlockPrefab, 49)]
        [TestCase(Assets.leavesBlockPrefab, 22)]
        [TestCase(Assets.gravelBlockPrefab, 19)]
        [TestCase(Assets.logBlockPrefab, 21)]
        [TestCase(Assets.woodenPlankBlockPrefab, 4)]
        [TestCase(Assets.cobblestoneBlockPrefab, 16)]
        public void A04d_BlockHasCorrectFirstTexture(string path, int topTextureId) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshRenderer>(prefab, out var renderer);
            var material = renderer.sharedMaterials[0];
            Assert.IsTrue(material.HasProperty(Assets.textureIdProperty), $"First material of block {prefab} should support the property '{Assets.textureIdProperty}'!");
            Assert.AreEqual(topTextureId, material.GetInt(Assets.textureIdProperty), $"First material of block {prefab} should use a different texture!");
        }
        [TestCase(Assets.grassBlockPrefab, 3)]
        [TestCase(Assets.logBlockPrefab, 20)]
        public void A04e_BlockHasCorrectSecondTexture(string path, int sideTextureId) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshRenderer>(prefab, out var renderer);
            var material = renderer.sharedMaterials[1];
            Assert.IsTrue(material.HasProperty(Assets.textureIdProperty), $"Second material of block {prefab} should support the property '{Assets.textureIdProperty}'!");
            Assert.AreEqual(sideTextureId, material.GetInt(Assets.textureIdProperty), $"Second material of block {prefab} should use a different texture!");
        }
        [TestCase(Assets.grassBlockPrefab, 2)]
        [TestCase(Assets.logBlockPrefab, 21)]
        public void A04f_BlockHasCorrectThirdTexture(string path, int bottomTextureId) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshRenderer>(prefab, out var renderer);
            var material = renderer.sharedMaterials[2];
            Assert.IsTrue(material.HasProperty(Assets.textureIdProperty), $"Third material of block {prefab} should support the property '{Assets.textureIdProperty}'!");
            Assert.AreEqual(bottomTextureId, material.GetInt(Assets.textureIdProperty), $"Third material of block {prefab} should use a different texture!");
        }
        [TestCase(Assets.stoneBlockPrefab)]
        [TestCase(Assets.sandBlockPrefab)]
        [TestCase(Assets.dirtBlockPrefab)]
        [TestCase(Assets.grassBlockPrefab)]
        [TestCase(Assets.glassBlockPrefab)]
        [TestCase(Assets.leavesBlockPrefab)]
        [TestCase(Assets.gravelBlockPrefab)]
        [TestCase(Assets.logBlockPrefab)]
        [TestCase(Assets.woodenPlankBlockPrefab)]
        [TestCase(Assets.cobblestoneBlockPrefab)]
        public void A04g_BlockPrefabContainsMeshFilter(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshFilter>(prefab);
        }
        [TestCase(Assets.stoneBlockPrefab, Assets.defaultCubeMesh)]
        [TestCase(Assets.sandBlockPrefab, Assets.defaultCubeMesh)]
        [TestCase(Assets.dirtBlockPrefab, Assets.defaultCubeMesh)]
        [TestCase(Assets.grassBlockPrefab, Assets.topSideBottomCubeMesh)]
        [TestCase(Assets.glassBlockPrefab, Assets.defaultCubeMesh)]
        [TestCase(Assets.leavesBlockPrefab, Assets.defaultCubeMesh)]
        [TestCase(Assets.gravelBlockPrefab, Assets.defaultCubeMesh)]
        [TestCase(Assets.logBlockPrefab, Assets.topSideBottomCubeMesh)]
        [TestCase(Assets.woodenPlankBlockPrefab, Assets.defaultCubeMesh)]
        [TestCase(Assets.cobblestoneBlockPrefab, Assets.defaultCubeMesh)]
        public void A04h_BlockPrefabUsesTheCorrectMesh(string path, string mesh) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<MeshFilter>(prefab, out var filter);
            Assert.AreEqual(LoadAsset<Mesh>(mesh), filter.sharedMesh, $"The mesh assigned to block {prefab} should be {mesh}!");
        }
        #endregion
    }
}
