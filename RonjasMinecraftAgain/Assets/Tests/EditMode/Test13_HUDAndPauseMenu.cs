using NUnit.Framework;
using System.Linq;
using TestInterfaces;
using TestUtils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Tests.EditMode {
    public class Test13_HUDAndPauseMenu : TestSuite {
        #region Aufgabe 01
        [TestCase(Assets.guiSprites)]
        public void A01a_SpritesheetExists(string path) {
            LoadAsset<Texture>(path);
        }
        [TestCase(Assets.guiSprites)]
        public void A01b_SpritesheetIsTexture2D(string path) {
            LoadAsset<Texture2D>(path);
        }
        [TestCase(Assets.guiSprites, 6)]
        public void A01c_SpritesheetHasCorrectNumberOfTextures(string path, int spriteCount) {
            var sprites = LoadAssets<Sprite>(path);
            Assert.GreaterOrEqual(spriteCount, sprites.Length, $"Spritesheet at '{path}' should contain exactly {spriteCount} sprites!");
        }
        [TestCase(Assets.guiSprites, "hotkey_bar")]
        [TestCase(Assets.guiSprites, "hotkey_frame")]
        [TestCase(Assets.guiSprites, "crosshairs")]
        [TestCase(Assets.guiSprites, "button_disabled")]
        [TestCase(Assets.guiSprites, "button_active")]
        [TestCase(Assets.guiSprites, "button_selected")]
        public void A01d_SpritesheetContainsSprite(string path, string spriteName) {
            var sprites = LoadAssets<Sprite>(path).Select(sprite => sprite.name);
            CollectionAssert.Contains(sprites, spriteName, $"Spritesheet at '{path}' should contain a sprite called '{spriteName}'!");
        }
        [TestCase(Assets.guiSprites, "hotkey_bar", 182, 22)]
        [TestCase(Assets.guiSprites, "hotkey_frame", 24, 24)]
        [TestCase(Assets.guiSprites, "crosshairs", 9, 9)]
        [TestCase(Assets.guiSprites, "button_disabled", 200, 20)]
        [TestCase(Assets.guiSprites, "button_active", 200, 20)]
        [TestCase(Assets.guiSprites, "button_selected", 200, 20)]
        public void A01e_SpritesheetSpriteIsOfTheCorrectSize(string path, string spriteName, int width, int height) {
            var sprite = LoadAssets<Sprite>(path).FirstOrDefault(sprite => sprite.name == spriteName);
            Assert.IsTrue(sprite, $"Spritesheet at '{path}' should contain a sprite called '{spriteName}'!");
            Assert.AreEqual(new Vector2Int(width, height), Vector2Int.RoundToInt(sprite.rect.size), $"Sprite '{spriteName}' should have a size of {width}x{height}px!");
        }
        #endregion

        #region Aufgabe 02
        [TestCase(Assets.terrainSprites)]
        public void A02a_SpritesheetExists(string path) {
            LoadAsset<Texture>(path);
        }
        [TestCase(Assets.terrainSprites)]
        public void A02b_SpritesheetIsTexture2D(string path) {
            LoadAsset<Texture2D>(path);
        }
        [TestCase(Assets.terrainSprites, 256)]
        public void A02c_SpritesheetHasCorrectNumberOfTextures(string path, int spriteCount) {
            var sprites = LoadAssets<Sprite>(path);
            Assert.GreaterOrEqual(spriteCount, sprites.Length, $"Spritesheet at '{path}' should contain exactly {spriteCount} sprites!");
        }
        [TestCase(Assets.terrainSprites, 16, 16)]
        public void A02d_SpritesheetSpriteIsOfTheCorrectSize(string path, int width, int height) {
            var sprites = LoadAssets<Sprite>(path);
            foreach (var sprite in sprites) {
                Assert.AreEqual(new Vector2Int(width, height), Vector2Int.RoundToInt(sprite.rect.size), $"Sprite '{sprite.name}' should have a size of {width}x{height}px!");
            }
        }
        #endregion

        #region Aufgabe 03
        [TestCase(Assets.playerControlsAsset, Assets.uiActionMap)]
        public void A03a_PlayerControlsHaveActionMap(string path, string actionMap) {
            var asset = LoadAsset<InputActionAsset>(path);
            Assert.IsNotNull(asset.FindActionMap(actionMap, true));
        }
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey1Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey2Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey3Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey4Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey5Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey6Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey7Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey8Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkey9Action)]
        [TestCase(Assets.playerControlsAsset, Assets.uiHotkeyMouseAction)]
        [TestCase(Assets.playerControlsAsset, Assets.uiTogglePauseAction)]
        public void A03b_PlayerControlsHaveAction(string path, string action) {
            var asset = LoadAsset<InputActionAsset>(path);
            Assert.IsNotNull(asset.FindAction(action, true));
        }
        #endregion

        #region Aufgabe 04
        [TestCase(Assets.stoneBlockPrefab)]
        [TestCase(Assets.dirtBlockPrefab)]
        [TestCase(Assets.woodenPlankBlockPrefab)]
        [TestCase(Assets.cobblestoneBlockPrefab)]
        [TestCase(Assets.sandBlockPrefab)]
        [TestCase(Assets.gravelBlockPrefab)]
        [TestCase(Assets.logBlockPrefab)]
        [TestCase(Assets.leavesBlockPrefab)]
        [TestCase(Assets.glassBlockPrefab)]
        [TestCase(Assets.grassBlockPrefab)]
        public void A04a_BlockPrefabContainsIBlockInfo(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<IBlockInfo>(prefab);
        }
        [TestCase(Assets.stoneBlockPrefab, BlockId.Stone)]
        [TestCase(Assets.dirtBlockPrefab, BlockId.Dirt)]
        [TestCase(Assets.woodenPlankBlockPrefab, BlockId.WoodenPlank)]
        [TestCase(Assets.cobblestoneBlockPrefab, BlockId.Cobblestone)]
        [TestCase(Assets.sandBlockPrefab, BlockId.Sand)]
        [TestCase(Assets.gravelBlockPrefab, BlockId.Gravel)]
        [TestCase(Assets.logBlockPrefab, BlockId.Log)]
        [TestCase(Assets.leavesBlockPrefab, BlockId.Leaves)]
        [TestCase(Assets.glassBlockPrefab, BlockId.Glass)]
        [TestCase(Assets.grassBlockPrefab, BlockId.Grass)]
        public void A04b_BlockPrefabHasBlockId(string path, BlockId blockId) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<IBlockInfo>(prefab, out var info);
            Assert.AreEqual(blockId, info.blockId, $"Block {prefab} have an ID of {blockId}!");
        }
        [TestCase(Assets.stoneBlockPrefab)]
        [TestCase(Assets.dirtBlockPrefab)]
        [TestCase(Assets.woodenPlankBlockPrefab)]
        [TestCase(Assets.cobblestoneBlockPrefab)]
        [TestCase(Assets.sandBlockPrefab)]
        [TestCase(Assets.gravelBlockPrefab)]
        [TestCase(Assets.logBlockPrefab)]
        [TestCase(Assets.leavesBlockPrefab)]
        [TestCase(Assets.glassBlockPrefab)]
        [TestCase(Assets.grassBlockPrefab)]
        public void A04c_BlockPrefabHasSprite(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<IBlockInfo>(prefab, out var info);
            Assert.IsTrue(info.sprite, $"Block {prefab} should have a sprite assigned!");
        }
        #endregion

        #region Aufgabe 05
        [TestCase(Assets.avatarPrefab)]
        public void A05a_AvatarContainsIHotkeyBar(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<IHotkeyBar>(prefab);
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
