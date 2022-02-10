using NUnit.Framework;
using System.Linq;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.EditMode {
    public class Test13a_HUDAndPauseMenu : TestSuite {
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
        [TestCase(Assets.guiSprites, Assets.hotkeyBarSprite)]
        [TestCase(Assets.guiSprites, Assets.hotkeyFrameSprite)]
        [TestCase(Assets.guiSprites, Assets.crosshairsSprite)]
        [TestCase(Assets.guiSprites, Assets.buttonDisabledSprite)]
        [TestCase(Assets.guiSprites, Assets.buttonActiveSprite)]
        [TestCase(Assets.guiSprites, Assets.buttonSelectedSprite)]
        public void A01d_SpritesheetContainsSprite(string path, string spriteName) {
            var sprites = LoadAssets<Sprite>(path).Select(sprite => sprite.name);
            CollectionAssert.Contains(sprites, spriteName, $"Spritesheet at '{path}' should contain a sprite called '{spriteName}'!");
        }
        [TestCase(Assets.guiSprites, Assets.hotkeyBarSprite, 182, 22)]
        [TestCase(Assets.guiSprites, Assets.hotkeyFrameSprite, 24, 24)]
        [TestCase(Assets.guiSprites, Assets.crosshairsSprite, 9, 9)]
        [TestCase(Assets.guiSprites, Assets.buttonDisabledSprite, 200, 20)]
        [TestCase(Assets.guiSprites, Assets.buttonActiveSprite, 200, 20)]
        [TestCase(Assets.guiSprites, Assets.buttonSelectedSprite, 200, 20)]
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
        #endregion
    }
}
