using NUnit.Framework;
using System.Collections;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class Test11_BlockLogic : MinecraftTestSuite {
        #region Aufgabe 02
        [UnityTest]
        [TestCase(0, 0, 0, Assets.sandBlockPrefab, 0, ExpectedResult = null)]
        [TestCase(0, 0, 1, Assets.gravelBlockPrefab, 0, ExpectedResult = null)]
        [TestCase(0, 2, 0, Assets.sandBlockPrefab, 2, ExpectedResult = null)]
        [TestCase(1, 2, 0, Assets.gravelBlockPrefab, 2, ExpectedResult = null)]
        [TestCase(0, 5, 0, Assets.sandBlockPrefab, 5, ExpectedResult = null)]
        [TestCase(1, 5, 1, Assets.gravelBlockPrefab, 5, ExpectedResult = null)]
        public IEnumerator A02a_TestBlockFallsDown(int x, int y, int z, string path, int swapCount) {
            var prefab = LoadPrefab(path);

            var position = new Vector3Int(x, y, z);

            var (obj, level) = CreateLevelMock();

            yield return new WaitForFixedUpdate();

            var instance = InstantiateGameObject(prefab, position, Quaternion.identity, obj.transform);

            level.tryGetBlockInstance = (Vector3Int testPosition, out GameObject testInstance) => {
                Assert.AreEqual(position + Vector3Int.down, testPosition, $"With block at position {position}, the only relevant block should be at position {position + Vector3Int.down}!");
                testInstance = null;
                return testPosition.x >= 0 && testPosition.y >= 0 && testPosition.z >= 0;
            };

            int swapPositionCount = 0;
            level.trySwapBlocks = (Vector3Int positionA, Vector3Int positionB) => {
                swapPositionCount++;
                if (positionA == position) {
                    position = positionB;
                    instance.transform.position = positionB;
                    return true;
                }
                if (positionB == position) {
                    position = positionA;
                    instance.transform.position = positionA;
                    return true;
                }
                return false;
            };

            for (int i = 1; i <= swapCount; i++) {
                yield return new WaitForFixedUpdate();
                Assert.AreEqual(i, swapPositionCount, $"With {prefab} at position {position} and after waiting {i} FixedUpdate(s), {level.trySwapBlocks} should have been called exactly {i} times!");
                CustomAssert.AreEqual(position, instance.transform.position, $"Block {prefab} must never change its own transform.position!");
            }

            for (int i = 0; i < 3; i++) {
                yield return new WaitForFixedUpdate();
                Assert.AreEqual(swapCount, swapPositionCount, $"Block {prefab} should have landed on floor by now, {level.trySwapBlocks} should not have been called!");
                CustomAssert.AreEqual(position, instance.transform.position, $"Block {prefab} must never change its own transform.position!");
            }
        }
        [UnityTest]
        [TestCase(0, 0, 0, Assets.sandBlockPrefab, ExpectedResult = null)]
        [TestCase(0, 0, 1, Assets.gravelBlockPrefab, ExpectedResult = null)]
        [TestCase(0, 2, 0, Assets.sandBlockPrefab, ExpectedResult = null)]
        [TestCase(1, 5, 0, Assets.gravelBlockPrefab, ExpectedResult = null)]
        public IEnumerator A02b_TestBlockDoesNotFallDown(int x, int y, int z, string path) {
            var prefab = LoadPrefab(path);

            var position = new Vector3Int(x, y, z);

            var (obj, level) = CreateLevelMock();

            yield return new WaitForFixedUpdate();

            var instance = InstantiateGameObject(prefab, position, Quaternion.identity, obj.transform);

            level.tryGetBlockInstance = (Vector3Int testPosition, out GameObject testInstance) => {
                Assert.AreEqual(position + Vector3Int.down, testPosition, $"With block at position {position}, the only relevant block should be at position {position + Vector3Int.down}!");
                testInstance = obj;
                return testPosition.x >= 0 && testPosition.y >= 0 && testPosition.z >= 0;
            };

            int swapPositionCount = 0;
            level.trySwapBlocks = (Vector3Int positionA, Vector3Int positionB) => {
                swapPositionCount++;
                return false;
            };

            for (int i = 0; i < 3; i++) {
                yield return new WaitForFixedUpdate();
                Assert.AreEqual(0, swapPositionCount, $"Block {prefab} was spawned directly on a non-air block and therefor should not have attempted to fall down!");
                CustomAssert.AreEqual(position, instance.transform.position, $"Block {prefab} must never change its own transform.position!");
            }
        }
        #endregion


        #region Aufgabe 02
        [UnityTest]
        [TestCase(0, 0, 0, Assets.dirtBlockPrefab, false, Assets.dirtBlockPrefab, ExpectedResult = null)]
        [TestCase(1, 1, 1, Assets.dirtBlockPrefab, false, Assets.dirtBlockPrefab, ExpectedResult = null)]
        [TestCase(0, 0, 0, Assets.dirtBlockPrefab, true, Assets.grassBlockPrefab, ExpectedResult = null)]
        [TestCase(1, 1, 1, Assets.dirtBlockPrefab, true, Assets.grassBlockPrefab, ExpectedResult = null)]
        [TestCase(0, 0, 0, Assets.grassBlockPrefab, false, Assets.dirtBlockPrefab, ExpectedResult = null)]
        [TestCase(1, 1, 1, Assets.grassBlockPrefab, false, Assets.dirtBlockPrefab, ExpectedResult = null)]
        [TestCase(0, 0, 0, Assets.grassBlockPrefab, true, Assets.grassBlockPrefab, ExpectedResult = null)]
        [TestCase(1, 1, 1, Assets.grassBlockPrefab, true, Assets.grassBlockPrefab, ExpectedResult = null)]
        public IEnumerator A03a_TestDirtTurnsToGrassAndBack(int x, int y, int z, string startingPath, bool hasAirAbove, string expectedPath) {
            var startingPrefab = LoadPrefab(startingPath);
            var expectedPrefab = LoadPrefab(expectedPath);

            var position = new Vector3Int(x, y, z);

            var (obj, level) = CreateLevelMock();

            yield return new WaitForFixedUpdate();

            var instance = InstantiateGameObject(startingPrefab, position, Quaternion.identity, obj.transform);

            level.tryGetBlockInstance = (Vector3Int testPosition, out GameObject testInstance) => {
                Assert.AreEqual(position + Vector3Int.up, testPosition, $"With block at position {position}, the only relevant block should be at position {position + Vector3Int.up}!");
                testInstance = hasAirAbove
                    ? null
                    : obj;
                return testPosition.x >= 0 && testPosition.y >= 0 && testPosition.z >= 0;
            };

            int setBlockCount = 0;
            level.trySetBlock = (Vector3Int actualPosition, GameObject actualPrefab) => {
                setBlockCount++;
                Assert.AreEqual(position, actualPosition, $"Block is at position {position}, so that's where the new block should be set!");

                if (hasAirAbove) {
                    Assert.AreEqual(expectedPrefab, actualPrefab, $"Block {startingPrefab} has air above, so it should have turned to {expectedPrefab}!");
                } else {
                    Assert.AreEqual(expectedPrefab, actualPrefab, $"Block {startingPrefab} does NOT have air above, so it should have turned to {expectedPrefab}!");
                }

                DestroyGameObject(instance);
                instance = InstantiateGameObject(actualPrefab, actualPosition, Quaternion.identity, obj.transform);

                return true;
            };

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            if (startingPrefab == expectedPrefab) {
                Assert.AreEqual(0, setBlockCount, $"Block {startingPrefab} should not have changed, so {nameof(ILevel.TrySetBlock)} should NOT have been called!");
            } else {
                Assert.AreEqual(1, setBlockCount, $"Block {startingPrefab} should have changed to {expectedPrefab} via {nameof(ILevel.TrySetBlock)} once!");
            }
        }
        #endregion

        private (GameObject, LevelMock) CreateLevelMock() {
            var obj = CreateGameObject("Level Mock");
            var level = obj.AddComponent<LevelMockComponent>();
            return (obj, level.level);
        }
    }
}