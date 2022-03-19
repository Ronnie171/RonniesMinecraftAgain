using NUnit.Framework;
using System.Linq;
using TestInterfaces;
using TestUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tests.EditMode {
    public class Test13b_HUDAndPauseMenu : TestSuite {
        #region Aufgabe 05
        [TestCase(Assets.avatarPrefab)]
        public void A05a_AvatarContainsIHotkeyBar(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<IHotkeyBar>(prefab);
        }
        [TestCase(Assets.avatarPrefab)]
        public void A05b_HotkeyBarHasNineBlocks(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<IHotkeyBar>(prefab, out var bar);
            Assert.AreEqual(9, bar.blockPrefabs.Length, $"Hotkey bar should have exactly 9 blocks!");
        }
        [TestCase(Assets.avatarPrefab)]
        public void A05c_HotkeyBarStartsAtZero(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<IHotkeyBar>(prefab, out var bar);
            Assert.AreEqual(0, bar.currentIndex, $"Hotkey bar should start with the first prefab selected!");
        }
        [TestCase(Assets.avatarPrefab, 0, Assets.stoneBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 1, Assets.dirtBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 2, Assets.woodenPlankBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 3, Assets.cobblestoneBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 4, Assets.sandBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 5, Assets.gravelBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 6, Assets.logBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 7, Assets.leavesBlockPrefab)]
        [TestCase(Assets.avatarPrefab, 8, Assets.glassBlockPrefab)]
        public void A05d_HotkeyBarHasBlock(string avatarPath, int index, string blockPath) {
            var avatarPrefab = LoadPrefab(avatarPath);
            var blockPrefab = LoadPrefab(blockPath);
            CustomAssert.HasComponentInChildren<IHotkeyBar>(avatarPrefab, out var bar);
            Assert.AreEqual(blockPrefab, bar.blockPrefabs[index], $"Starting block at index {index} should be {blockPrefab.name}!");
        }
        #endregion

        #region Aufgabe 06
        [TestCase(Assets.gameManagerPrefab)]
        public void A06a_GameManagerPrefabExists(string path) {
            LoadPrefab(path);
        }
        [TestCase(Assets.gameManagerPrefab)]
        public void A06b_GameManagerPrefabContainsIGameManager(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<IGameManager>(prefab);
        }
        [TestCase(Assets.gameManagerPrefab)]
        public void A06c_GameManagerPrefabContainsCanvas(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Canvas>(prefab);
        }
        [TestCase(Assets.gameManagerPrefab, 3)]
        public void A06d_GameManagerCanvasContainsButtons(string path, int buttonCount) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Canvas>(prefab, out var canvas);
            var buttons = canvas.GetComponentsInChildren<Button>();
            Assert.GreaterOrEqual(buttons.Length, buttonCount, $"The pause menu canvas should have at least 3 buttons!");
        }
        [TestCase(Assets.gameManagerPrefab, "Continue")]
        [TestCase(Assets.gameManagerPrefab, "Restart")]
        [TestCase(Assets.gameManagerPrefab, "Quit")]
        public void A06e_GameManagerCanvasContainsButtonWithText(string path, string label) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Canvas>(prefab, out var canvas);
            var labels = canvas
                .GetComponentsInChildren<Button>()
                .SelectMany(button => Enumerable.Empty<string>()
                    .Union(button.GetComponentsInChildren<TextMeshProUGUI>().Select(text => text.text))
                    .Union(button.GetComponentsInChildren<Text>().Select(text => text.text))
                );
            CollectionAssert.Contains(labels, label, $"The pause menu canvas should have a button with the label '{label}'!");
        }
        #endregion
    }
}
