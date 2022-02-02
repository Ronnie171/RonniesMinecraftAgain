using NUnit.Framework;
using System.Collections;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class Test10_Level : MinecraftTestSuite {
        private static readonly Quaternion[] bodyRotations = new[] {
            Quaternion.identity,
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, -90, 0),
            Quaternion.Euler(0, 180, 0),
            Quaternion.Euler(0, 45, 0),
        };
        private static readonly Quaternion[] headRotations = new[] {
            Quaternion.identity,
            Quaternion.Euler(-45, 0, 0),
        };

        private const int spawnDuration = 2;
        protected override float timeScale => 2;

        #region Aufgabe 02
        [UnityTest]
        public IEnumerator A02c_TestLevelSize() {
            yield return SpawnLevel();

            Assert.GreaterOrEqual(level.size.x, 16, "For these tests, the level must be at least 16 blocks wide!");
            Assert.GreaterOrEqual(level.size.y, 16, "For these tests, the level must be at least 16 blocks high!");
            Assert.GreaterOrEqual(level.size.z, 16, "For these tests, the level must be at least 16 blocks deep!");
        }
        [UnityTest]
        [TestCase(0f, 0f, 0f, 0, 0, 0, ExpectedResult = null)]
        [TestCase(0.4f, 0.6f, 1.9f, 0, 1, 2, ExpectedResult = null)]
        [TestCase(-0.4f, -0.6f, -1.9f, 0, -1, -2, ExpectedResult = null)]
        public IEnumerator A02d_TestWorldSpaceToBlockSpace(float inputX, float inputY, float inputZ, int expectedX, int expectedY, int expectedZ) {
            yield return SpawnLevel();

            Assert.AreEqual(
                new Vector3Int(expectedX, expectedY, expectedZ),
                level.WorldSpaceToBlockSpace(new Vector3(inputX, inputY, inputZ)),
                $"{nameof(level.WorldSpaceToBlockSpace)} should round all coordinates to their nearest integer!"
            );
        }
        [UnityTest]
        [TestCase(0, 0, 0, true, true, ExpectedResult = null)]
        [TestCase(5, 10, 15, true, true, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, true, ExpectedResult = null)]
        [TestCase(-1, 0, 0, true, false, ExpectedResult = null)]
        [TestCase(0, 0, -1, false, false, ExpectedResult = null)]
        public IEnumerator A02e_TestThatTrySetBlockSucceeds(int x, int y, int z, bool usePrefab, bool expectedResult) {
            yield return SpawnLevel();

            var position = new Vector3Int(x, y, z);
            var prefab = usePrefab
                ? CreateBlockPrefab()
                : default;

            yield return new WaitForFixedUpdate();

            bool success = level.TrySetBlock(position, prefab);
            if (expectedResult) {
                Assert.IsTrue(success, $"{nameof(level.TrySetBlock)} with position {position} and prefab '{prefab}' should have succeeded!");
            } else {
                Assert.IsFalse(success, $"{nameof(level.TrySetBlock)} with position {position} and prefab '{prefab}' should NOT have succeeded!");
            }
        }
        [UnityTest]
        [TestCase(0, 0, 0, true, ExpectedResult = null)]
        [TestCase(5, 10, 15, true, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, ExpectedResult = null)]
        public IEnumerator A02f_TestThatTrySetBlockInstantiatesThePrefab(int x, int y, int z, bool usePrefab) {
            yield return SpawnLevel();

            var position = new Vector3Int(x, y, z);
            var prefab = usePrefab
                ? CreateBlockPrefab()
                : default;

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(position, prefab);

            yield return new WaitForFixedUpdate();

            var colliders = Physics.OverlapSphere(position, 1);
            if (usePrefab) {
                Assert.AreEqual(1, colliders.Length, $"{nameof(level.TrySetBlock)} with position {position} and prefab '{prefab}' should have created ONE collider!");
                Assert.AreEqual(levelObj.transform, colliders[0].transform.parent, $"{nameof(level.TrySetBlock)} should instantiate its object as children of the level!");
            } else {
                Assert.AreEqual(0, colliders.Length, $"{nameof(level.TrySetBlock)} with position {position} and without prefab should have created NO colliders!");
            }
        }
        [UnityTest]
        [TestCase(0, 0, 0, true, true, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, true, ExpectedResult = null)]
        [TestCase(0, 0, 0, true, false, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, false, ExpectedResult = null)]
        public IEnumerator A02g_TestThatTrySetBlockDestroysThePreviousInstance(int x, int y, int z, bool usePrefabA, bool usePrefabB) {
            yield return SpawnLevel();

            var position = new Vector3Int(x, y, z);
            var prefabA = usePrefabA
                ? CreateBlockPrefab()
                : default;
            var prefabB = usePrefabB
                ? CreateBlockPrefab()
                : default;

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(position, prefabA);

            yield return new WaitForFixedUpdate();

            var collidersA = Physics.OverlapSphere(position, 1);
            var instanceA = collidersA.Length > 0
                ? collidersA[0]
                : default;

            level.TrySetBlock(position, prefabB);

            yield return new WaitForFixedUpdate();

            var collidersB = Physics.OverlapSphere(position, 2);
            var instanceB = collidersB.Length > 0
                ? collidersB[0]
                : default;

            if (usePrefabB) {
                Assert.AreEqual(1, collidersB.Length, $"{nameof(level.TrySetBlock)} with position {position} and prefab '{prefabB}' should have created ONE collider!");
            } else {
                Assert.AreEqual(0, collidersB.Length, $"{nameof(level.TrySetBlock)} with position {position} and without prefab should have created NO colliders!");
            }

            if (usePrefabA || usePrefabB) {
                Assert.AreNotEqual(instanceA, instanceB, $"{nameof(level.TrySetBlock)} should have destroyed the previous instance at position {position}!");
            } else {
                Assert.IsNull(instanceA, $"{nameof(level.TrySetBlock)} should not have instantiated any prefabs at position {position}!");
                Assert.IsNull(instanceB, $"{nameof(level.TrySetBlock)} should not have instantiated any prefabs at position {position}!");
            }
        }
        [UnityTest]
        [TestCase(0, 0, 0, true, true, ExpectedResult = null)]
        [TestCase(5, 10, 15, true, true, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, true, ExpectedResult = null)]
        [TestCase(-1, 0, 0, true, false, ExpectedResult = null)]
        [TestCase(0, 0, -1, false, false, ExpectedResult = null)]
        public IEnumerator A02h_TestThatTryGetBlockInstanceSucceeds(int x, int y, int z, bool usePrefab, bool expectedResult) {
            yield return SpawnLevel();

            var position = new Vector3Int(x, y, z);
            var prefab = usePrefab
                ? CreateBlockPrefab()
                : default;

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(position, prefab);

            bool success = level.TryGetBlockInstance(position, out var instance);
            if (expectedResult) {
                Assert.IsTrue(success, $"{nameof(level.TryGetBlockInstance)} with position {position} should have succeeded!");
                if (usePrefab) {
                    Assert.IsNotNull(instance, $"{nameof(level.TryGetBlockInstance)} with position {position} should have returned a non-air block!");
                    CustomAssert.AreEqual(position, instance.transform.position, $"Block instance should have been spawned at {position}!");
                } else {
                    Assert.IsNull(instance, $"{nameof(level.TryGetBlockInstance)} with position {position} should have returned the air block!");
                }
            } else {
                Assert.IsFalse(success, $"{nameof(level.TryGetBlockInstance)} with position {position} should NOT have succeeded!");
            }
        }
        [UnityTest]
        [TestCase(0, 0, 0, true, ExpectedResult = null)]
        [TestCase(5, 10, 15, true, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, ExpectedResult = null)]
        public IEnumerator A02i_TestThatTryGetBlockInstanceReturnsTheInstance(int x, int y, int z, bool usePrefab) {
            yield return SpawnLevel();

            var position = new Vector3Int(x, y, z);
            var prefab = usePrefab
                ? CreateBlockPrefab()
                : default;

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(position, prefab);

            yield return new WaitForFixedUpdate();

            var colliders = Physics.OverlapSphere(position, 1);
            var expectedInstance = colliders.Length > 0
                ? colliders[0].gameObject
                : default;

            level.TryGetBlockInstance(position, out var actualInstance);

            Assert.AreEqual(expectedInstance, actualInstance, $"{nameof(level.TryGetBlockInstance)} must return the instance spawned via {nameof(level.TrySetBlock)}, or null in case of air!");
        }
        [UnityTest]
        [TestCase(5, 10, 15, true, ExpectedResult = null)]
        [TestCase(-1, 0, 0, false, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, ExpectedResult = null)]
        public IEnumerator A02j_TestThatTrySwapBlocksSucceeds(int x, int y, int z, bool expectedResult) {
            yield return SpawnLevel();

            var positionA = Vector3Int.zero;
            var positionB = new Vector3Int(x, y, z);
            var prefabA = CreateBlockPrefab();

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(positionA, prefabA);

            bool successA = level.TrySwapBlocks(positionA, positionB);

            if (expectedResult) {
                Assert.IsTrue(successA, $"With positions {positionA} and {positionB}, {nameof(level.TrySwapBlocks)} should have succeeded!");
            } else {
                Assert.IsFalse(successA, $"With positions {positionA} and {positionB}, {nameof(level.TrySwapBlocks)} should NOT have succeeded!");
            }

            bool successB = level.TrySwapBlocks(positionB, positionA);

            if (expectedResult) {
                Assert.IsTrue(successB, $"With positions {positionB} and {positionA}, {nameof(level.TrySwapBlocks)} should have succeeded!");
            } else {
                Assert.IsFalse(successB, $"With positions {positionB} and {positionA}, {nameof(level.TrySwapBlocks)} should NOT have succeeded!");
            }
        }
        [UnityTest]
        [TestCase(true, true, ExpectedResult = null)]
        [TestCase(false, true, ExpectedResult = null)]
        [TestCase(true, false, ExpectedResult = null)]
        [TestCase(false, false, ExpectedResult = null)]
        public IEnumerator A02k_TestThatTrySwapBlocksMovesTheTransform(bool usePrefabA, bool usePrefabB) {
            yield return SpawnLevel();

            var positionA = Vector3Int.zero;
            var positionB = Vector3Int.one;
            var prefabA = usePrefabA
                ? CreateBlockPrefab()
                : null;
            var prefabB = usePrefabB
                ? CreateBlockPrefab()
                : null;

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(positionA, prefabA);
            level.TrySetBlock(positionB, prefabB);

            level.TryGetBlockInstance(positionA, out var instanceA1);
            level.TryGetBlockInstance(positionB, out var instanceB1);

            level.TrySwapBlocks(positionA, positionB);

            level.TryGetBlockInstance(positionB, out var instanceA2);
            level.TryGetBlockInstance(positionA, out var instanceB2);

            Assert.AreEqual(instanceA1, instanceA2, $"{nameof(level.TrySwapBlocks)} should NOT instantiate or destroy objects!");
            Assert.AreEqual(instanceB1, instanceB2, $"{nameof(level.TrySwapBlocks)} should NOT instantiate or destroy objects!");

            if (usePrefabA) {
                Assert.IsNotNull(instanceA2, $"{nameof(level.TryGetBlockInstance)} with position {positionB} should have returned a non-air block!");
                CustomAssert.AreEqual(positionB, instanceA2.transform.position, $"Block instance {instanceA2} should have been moved to position at {positionB}!");
            } else {
                Assert.IsNull(instanceA2, $"{nameof(level.TryGetBlockInstance)} with position {positionB} should have returned the air block!");
            }

            if (usePrefabB) {
                Assert.IsNotNull(instanceB2, $"{nameof(level.TryGetBlockInstance)} with position {positionA} should have returned a non-air block!");
                CustomAssert.AreEqual(positionA, instanceB2.transform.position, $"Block instance {instanceB2} should have been moved to position at {positionA}!");
            } else {
                Assert.IsNull(instanceB2, $"{nameof(level.TryGetBlockInstance)} with position {positionA} should have returned the air block!");
            }
        }

        [UnityTest]
        [TestCase(0, 0, 0, true, true, ExpectedResult = null)]
        [TestCase(5, 10, 15, true, true, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, true, ExpectedResult = null)]
        [TestCase(-1, 0, 0, true, false, ExpectedResult = null)]
        [TestCase(0, 0, -1, false, false, ExpectedResult = null)]
        public IEnumerator A02l_TestThatOnLevelChangeGetsInvokedByTrySetBlock(int x, int y, int z, bool usePrefab, bool expectedResult) {
            yield return SpawnLevel();

            var position = new Vector3Int(x, y, z);
            var prefab = usePrefab
                ? CreateBlockPrefab()
                : default;

            int levelChangeCount = 0;
            level.onLevelChange += (Vector3Int p, GameObject i) => {
                levelChangeCount++;

                Assert.AreEqual(position, p, $"{nameof(level.onLevelChange)} must be invoked with the effected position!");
                if (usePrefab) {
                    Assert.IsNotNull(i, $"In case of an actual block, {nameof(level.onLevelChange)} must be invoked with its instance!");
                    Assert.AreNotEqual(prefab, i, $"In case of an actual block, {nameof(level.onLevelChange)} must be invoked with its instance!");
                } else {
                    Assert.IsNull(i, $"In case of the air block, {nameof(level.onLevelChange)} must be invoked with null!");
                }
            };

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(position, prefab);

            if (expectedResult) {
                Assert.AreEqual(1, levelChangeCount, $"With valid coordinates, {nameof(level.TrySetBlock)} should invoke {nameof(level.onLevelChange)} exactly once!");
            } else {
                Assert.AreEqual(0, levelChangeCount, $"With out of bounds coordinates, {nameof(level.TrySetBlock)} should NOT invoke {nameof(level.onLevelChange)}!");
            }
        }

        [UnityTest]
        [TestCase(5, 10, 15, true, ExpectedResult = null)]
        [TestCase(-1, 0, 0, false, ExpectedResult = null)]
        [TestCase(0, 0, 0, false, ExpectedResult = null)]
        public IEnumerator A02m_TestThatOnLevelChangeGetsInvokedByTrySwapBlocks(int x, int y, int z, bool expectedResult) {
            yield return SpawnLevel();

            var positionA = Vector3Int.zero;
            var positionB = new Vector3Int(x, y, z);
            var prefabA = CreateBlockPrefab();

            yield return new WaitForFixedUpdate();

            level.TrySetBlock(positionA, prefabA);

            int levelChangeCount = 0;
            level.onLevelChange += (Vector3Int p, GameObject i) => {
                levelChangeCount++;
            };

            level.TrySwapBlocks(positionA, positionB);

            if (expectedResult) {
                Assert.AreEqual(2, levelChangeCount, $"With valid coordinates, {nameof(level.TrySwapBlocks)} should invoke {nameof(level.onLevelChange)} exactly twice!");
            } else {
                Assert.AreEqual(0, levelChangeCount, $"With invalid coordinates, {nameof(level.TrySwapBlocks)} should NOT invoke {nameof(level.onLevelChange)}!");
            }
        }
        #endregion

        #region Aufgabe 03
        [UnityTest]
        public IEnumerator A03b_TestSetLevel() {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var mockLevel = new LevelMock();
            levelAvatar.level = mockLevel;

            yield return new WaitForFixedUpdate();

            Assert.AreEqual(mockLevel, levelAvatar.level, $"{nameof(levelAvatar.level)} must return the value previously set via {nameof(levelAvatar.level)}!");
        }
        [UnityTest]
        public IEnumerator A03c_TestSetCurrentlySelectedBlockPrefab() {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var mockBlock = CreatePrimitive(PrimitiveType.Cube);
            levelAvatar.currentlySelectedBlockPrefab = mockBlock;

            yield return new WaitForFixedUpdate();

            Assert.AreEqual(mockBlock, levelAvatar.currentlySelectedBlockPrefab, $"{nameof(levelAvatar.currentlySelectedBlockPrefab)} must return the value previously set via {nameof(levelAvatar.currentlySelectedBlockPrefab)}!");
        }
        [UnityTest]
        public IEnumerator A03d_TestThatCurrentlySelectedBlockPrefabIsStone() {
            var prefab = LoadPrefab(Assets.stoneBlockPrefab);

            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            Assert.AreEqual(prefab, levelAvatar.currentlySelectedBlockPrefab, $"At level start, {nameof(levelAvatar.currentlySelectedBlockPrefab)} should return the stone block!");
        }
        [UnityTest]
        [TestCase(Assets.blockTag, 5, true, ExpectedResult = null)]
        [TestCase(Assets.blockTag, 15, false, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 5, false, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 15, false, ExpectedResult = null)]
        public IEnumerator A03e_TestThatDestroyBlockHonorsTagAndReach(string tag, int distance, bool expectedResult) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            body.rotation = Quaternion.identity;
            head.rotation = Quaternion.identity;

            var positionA = RoundToInt(head.position + head.forward * distance);

            var prefabA = CreatePrimitive(PrimitiveType.Cube);
            prefabA.transform.position = positionA;
            prefabA.tag = tag;

            yield return new WaitForFixedUpdate();

            int onTrySetBlockCount = 0;
            var level = new LevelMock {
                trySetBlock = (positionB, prefabB) => {
                    onTrySetBlockCount++;
                    Assert.IsNull(prefabB, $"{nameof(levelAvatar.DestroyBlockInLevel)} should attempt to place the air block!");
                    Assert.AreEqual(positionA, positionB, $"{nameof(levelAvatar.DestroyBlockInLevel)} should attempt to destroy the block at {positionA}!");
                    return true;
                }
            };

            levelAvatar.level = level;
            levelAvatar.currentlySelectedBlockPrefab = prefabA;
            levelAvatar.SetReach(10);

            levelAvatar.DestroyBlockInLevel();

            if (expectedResult) {
                Assert.AreEqual(1, onTrySetBlockCount, $"With a reach of {distance}, {nameof(levelAvatar.DestroyBlockInLevel)} should have attempted to destroy the block at position {positionA} tagged '{tag}' using its {levelAvatar.level} exactly once!");
            } else {
                Assert.AreEqual(0, onTrySetBlockCount, $"With a reach of {distance}, {nameof(levelAvatar.DestroyBlockInLevel)} should NOT have attempted to destroy the block at position {positionA} tagged '{tag}'!");
            }

        }
        [UnityTest]
        public IEnumerator A03f_TestThatDestroyBlockHonorsBodyAndHeadRotation(
            [ValueSource(nameof(bodyRotations))] Quaternion bodyRotation,
            [ValueSource(nameof(headRotations))] Quaternion headRotation) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            body.rotation = bodyRotation;
            head.rotation = headRotation;

            var positionA = RoundToInt(head.position + head.forward * 5);
            var prefabA = CreatePrimitive(PrimitiveType.Cube);
            prefabA.transform.position = positionA;
            prefabA.tag = Assets.blockTag;

            yield return new WaitForFixedUpdate();

            int onTrySetBlockCount = 0;
            var level = new LevelMock {
                trySetBlock = (positionB, prefabB) => {
                    onTrySetBlockCount++;
                    Assert.IsNull(prefabB, $"{nameof(levelAvatar.DestroyBlockInLevel)} should attempt to place the air block!");
                    Assert.AreEqual(positionA, positionB, $"{nameof(levelAvatar.DestroyBlockInLevel)} should attempt to destroy the block at {positionA}!");
                    return true;
                }
            };

            levelAvatar.level = level;
            levelAvatar.currentlySelectedBlockPrefab = prefabA;
            levelAvatar.SetReach(10);

            levelAvatar.DestroyBlockInLevel();

            Assert.AreEqual(1, onTrySetBlockCount, $"With body rotation {bodyRotation} and head rotation {headRotation}, {nameof(levelAvatar.DestroyBlockInLevel)} should have attempted to destroy the block at position {positionA} using its {levelAvatar.level} exactly once!");
        }
        [UnityTest]
        [TestCase(Assets.blockTag, 5, true, ExpectedResult = null)]
        [TestCase(Assets.blockTag, 15, false, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 5, true, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 15, false, ExpectedResult = null)]
        public IEnumerator A03g_TestThatBuildBlockHonorsTagAndReach(string tag, int distance, bool expectedResult) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var positionA = RoundToInt(head.position + head.forward * distance);
            var prefabA = CreatePrimitive(PrimitiveType.Cube);
            prefabA.transform.position = positionA;
            prefabA.tag = tag;

            yield return new WaitForFixedUpdate();

            int onTrySetBlockCount = 0;
            var level = new LevelMock {
                trySetBlock = (positionB, prefabB) => {
                    onTrySetBlockCount++;
                    Assert.AreEqual(prefabA, prefabB, $"{nameof(levelAvatar.BuildBlockInLevel)} should attempt to place the avatar's {nameof(levelAvatar.currentlySelectedBlockPrefab)}!");
                    Assert.AreEqual(positionA + Vector3Int.back, positionB, $"{nameof(levelAvatar.BuildBlockInLevel)} should attempt to place the block in front of {positionA}!");
                    return true;
                }
            };

            levelAvatar.level = level;
            levelAvatar.currentlySelectedBlockPrefab = prefabA;
            levelAvatar.SetReach(10);

            levelAvatar.BuildBlockInLevel();

            if (expectedResult) {
                Assert.AreEqual(1, onTrySetBlockCount, $"{nameof(levelAvatar.BuildBlockInLevel)} should have attempted to build in front of {positionA} using its {levelAvatar.level} exactly once!");
            } else {
                Assert.AreEqual(0, onTrySetBlockCount, $"{nameof(levelAvatar.BuildBlockInLevel)} should NOT have attempted to build in front of a block at position {positionA}!");
            }
        }
        [UnityTest]
        [TestCase(-90, ExpectedResult = null)]
        [TestCase(-85, ExpectedResult = null)]
        [TestCase(90, ExpectedResult = null)]
        [TestCase(85, ExpectedResult = null)]
        public IEnumerator A03h_TestThatBuildBlockDoesNotBuildWhereTheAvatarIsStanding(int headRotation) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var positionA = RoundToInt(ground.transform.position + Vector3.up * 3);
            var prefabA = CreatePrimitive(PrimitiveType.Cube);
            prefabA.transform.position = positionA;
            prefabA.tag = Assets.blockTag;

            body.rotation = Quaternion.identity;
            head.rotation = Quaternion.Euler(headRotation, 0, 0);

            yield return new WaitForFixedUpdate();

            int onTrySetBlockCount = 0;
            var level = new LevelMock {
                trySetBlock = (positionB, prefabB) => {
                    onTrySetBlockCount++;
                    return true;
                }
            };

            levelAvatar.level = level;
            levelAvatar.currentlySelectedBlockPrefab = prefabA;
            levelAvatar.SetReach(10);

            levelAvatar.BuildBlockInLevel();

            Assert.AreEqual(0, onTrySetBlockCount, $"{nameof(levelAvatar.BuildBlockInLevel)} should NOT spawn blocks when the avatar is looking at angle {headRotation}°!");
        }
        #endregion

        private GameObject blockPrefab;
        private GameObject CreateBlockPrefab() {
            if (!blockPrefab) {
                blockPrefab = CreatePrimitive(PrimitiveType.Cube);
                blockPrefab.transform.position = -100 * Vector3.one;
                blockPrefab.tag = Assets.blockTag;
            }
            return blockPrefab;
        }
    }
}