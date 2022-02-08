using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode {
    public class Test13_HUDAndPauseMenu : MinecraftTestSuite {
        protected override float timeScale => 2;

        #region Aufgabe 05
        [UnityTest]
        [TestCase(Assets.hotkeyBarSprite, ExpectedResult = null)]
        [TestCase(Assets.hotkeyFrameSprite, ExpectedResult = null)]
        public IEnumerator A05e_HotkeyBarHasImageWithGUISprite(string name) {
            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            var images = hotkeyBarObj
                .GetComponentsInChildren<Image>()
                .Select(image => image.sprite ? image.sprite.name : string.Empty);

            CollectionAssert.Contains(images, name, $"Hotkey bar should contain an image with a sprite called '{name}'!");
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
        public IEnumerator A05f_HotkeyBarHasImageWithBlockSprite(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<IBlockInfo>(prefab, out var info);

            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            var sprites = hotkeyBarObj
                .GetComponentsInChildren<Image>()
                .Select(image => image.sprite);

            CollectionAssert.Contains(sprites, info.sprite, $"Hotkey bar should contain an image with the sprite '{info.sprite}'!");
        }
        [UnityTest]
        [TestCase(8, false, ExpectedResult = null)]
        [TestCase(9, true, ExpectedResult = null)]
        [TestCase(10, false, ExpectedResult = null)]
        public IEnumerator A05g_HotkeyBarCanSetBlockPrefabs(int count, bool expectedResult) {
            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            var prefabs = Enumerable.Repeat(CreateBlockPrefab(), count)
                .ToArray();

            if (expectedResult) {
                hotkeyBar.blockPrefabs = prefabs;
                Assert.AreEqual(prefabs, hotkeyBar.blockPrefabs, $"Getting blockPrefabs should return previously set blockPrefabs!");
            } else {
                Assert.Throws<NotSupportedException>(() => hotkeyBar.blockPrefabs = prefabs, $"When attempting to set {count} block prefabs, hotkey bar should throw an exception!");
            }
        }
        [UnityTest]
        public IEnumerator A05h_HotkeyBarSetBlockPrefabsUpdatesSprites() {
            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            var images = hotkeyBarObj
               .GetComponentsInChildren<Image>();

            var prefab = CreateBlockPrefab();
            var expectedSprite = prefab.GetComponent<IBlockInfo>().sprite;

            hotkeyBar.blockPrefabs = Enumerable.Repeat(prefab, 9)
                .ToArray();

            var changedImages = images
                .Where(image => image.sprite == expectedSprite)
                .ToArray();

            Assert.AreEqual(9, changedImages.Length, $"Setting blockPrefabs should update the sprite of all 9 Image components!");
        }
        [UnityTest]
        [TestCase(-18, 0, ExpectedResult = null)]
        [TestCase(-8, 1, ExpectedResult = null)]
        [TestCase(-2, 7, ExpectedResult = null)]
        [TestCase(-1, 8, ExpectedResult = null)]
        [TestCase(0, 0, ExpectedResult = null)]
        [TestCase(1, 1, ExpectedResult = null)]
        [TestCase(8, 8, ExpectedResult = null)]
        [TestCase(9, 0, ExpectedResult = null)]
        [TestCase(10, 1, ExpectedResult = null)]
        [TestCase(17, 8, ExpectedResult = null)]
        public IEnumerator A05i_HotkeyBarCanSetIndex(int input, int expectedResult) {
            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            hotkeyBar.currentIndex = input;

            Assert.AreEqual(expectedResult, hotkeyBar.currentIndex, $"Setting currentIndex to {input} should result in a currentIndex of {expectedResult}!");
        }
        [UnityTest]
        public IEnumerator A05j_HotkeyBarCanSetAvatar() {
            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            var avatar = new AvatarMock();

            hotkeyBar.avatar = avatar;

            Assert.AreEqual(avatar, hotkeyBar.avatar, $"Getting avatar should return previously set avatar!");
        }
        [UnityTest]
        [TestCase(false, ExpectedResult = null)]
        [TestCase(true, ExpectedResult = null)]
        public IEnumerator A05k_HotkeyBarSetBlockPrefabsUpdatesAvatar(bool useMock) {
            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            var avatar = useMock
                ? new AvatarMock()
                : hotkeyBar.avatar;

            hotkeyBar.avatar = avatar;

            var prefab = CreateBlockPrefab();

            hotkeyBar.blockPrefabs = Enumerable.Repeat(prefab, 9)
                .ToArray();

            Assert.AreEqual(prefab, avatar.currentlySelectedBlockPrefab, $"Setting blockPrefabs should update the currentlySelectedBlockPrefab of the avatar!");
        }
        [UnityTest]
        [TestCase(0, false, ExpectedResult = null)]
        [TestCase(1, true, ExpectedResult = null)]
        [TestCase(2, true, ExpectedResult = null)]
        [TestCase(3, true, ExpectedResult = null)]
        public IEnumerator A05l_HotkeyBarSetCurrentIndexMovesFrame(int index, bool expectedMove) {
            yield return SpawnAvatar(Assets.avatarSpawnPoint);

            var position = hotkeyFrame.transform.position;

            hotkeyBar.currentIndex = index;

            if (expectedMove) {
                CustomAssert.AreNotEqual(position, hotkeyFrame.transform.position, $"Setting currentIndex to {index} should have caused the frame {hotkeyFrame} to move!");
            } else {
                CustomAssert.AreEqual(position, hotkeyFrame.transform.position, $"Setting currentIndex to {index} should have NOT caused the frame {hotkeyFrame} to move!");
            }
        }
        #endregion

        private GameObject CreateBlockPrefab() {
            var blockPrefab = CreatePrimitive(PrimitiveType.Cube);
            var info = blockPrefab.AddComponent<BlockInfoMockComponent>();
            info.sprite = Sprite.Create(default, new Rect(0, 0, 16, 16), Vector2.one / 2);
            blockPrefab.transform.position = -100 * Vector3.one;
            blockPrefab.tag = Assets.blockTag;
            return blockPrefab;
        }
    }
}
