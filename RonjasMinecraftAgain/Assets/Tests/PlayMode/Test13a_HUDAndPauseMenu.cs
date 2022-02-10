using NUnit.Framework;
using System.Collections;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class Test13a_HUDAndPauseMenu : MinecraftTestSuite {

        #region Aufgabe 04
        [UnityTest]
        [TestCase(Assets.stoneBlockPrefab, BlockId.Stone, ExpectedResult = null)]
        [TestCase(Assets.dirtBlockPrefab, BlockId.Dirt, ExpectedResult = null)]
        [TestCase(Assets.woodenPlankBlockPrefab, BlockId.WoodenPlank, ExpectedResult = null)]
        [TestCase(Assets.cobblestoneBlockPrefab, BlockId.Cobblestone, ExpectedResult = null)]
        [TestCase(Assets.sandBlockPrefab, BlockId.Sand, ExpectedResult = null)]
        [TestCase(Assets.gravelBlockPrefab, BlockId.Gravel, ExpectedResult = null)]
        [TestCase(Assets.logBlockPrefab, BlockId.Log, ExpectedResult = null)]
        [TestCase(Assets.leavesBlockPrefab, BlockId.Leaves, ExpectedResult = null)]
        [TestCase(Assets.glassBlockPrefab, BlockId.Glass, ExpectedResult = null)]
        [TestCase(Assets.grassBlockPrefab, BlockId.Grass, ExpectedResult = null)]
        public IEnumerator A04b_BlockPrefabHasBlockId(string path, BlockId blockId) {
            var prefab = LoadPrefab(path);
            yield return new WaitForFixedUpdate();
            CustomAssert.HasComponent<IBlockInfo>(prefab, out var info);
            var actualId = info.blockId;
            Assert.AreEqual(blockId, actualId, $"Block {prefab} have an ID of {blockId}!");
        }
        [UnityTest]
        [TestCase(Assets.stoneBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.dirtBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.woodenPlankBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.cobblestoneBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.sandBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.gravelBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.logBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.leavesBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.glassBlockPrefab, ExpectedResult = null)]
        [TestCase(Assets.grassBlockPrefab, ExpectedResult = null)]
        public IEnumerator A04c_BlockPrefabHasSprite(string path) {
            var prefab = LoadPrefab(path);
            yield return new WaitForFixedUpdate();
            CustomAssert.HasComponent<IBlockInfo>(prefab, out var info);
            var actualSprite = info.sprite;
            Assert.IsNotNull(actualSprite, $"Block {prefab} should have a sprite assigned!");
        }
        #endregion
    }
}
