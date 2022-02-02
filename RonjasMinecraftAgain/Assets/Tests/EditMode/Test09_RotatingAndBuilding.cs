using NUnit.Framework;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.EditMode {
    public class Test09_RotatingAndBuilding : TestSuite {
        #region Aufgabe 01
        [TestCase(Assets.avatarPrefab)]
        public void A01a_AvatarPrefabContainsCanvas(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Canvas>(prefab);
        }
        [TestCase(Assets.avatarPrefab, Assets.hudTag)]
        public void A01b_AvatarCanvasHasTag(string path, string tag) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Canvas>(prefab, out var canvas);

            Assert.AreEqual(tag, canvas.gameObject.tag);
        }
        [TestCase(Assets.avatarPrefab, Assets.userInterfaceLayer)]
        public void A01c_AvatarCanvasIsOnLayer(string path, int layer) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Canvas>(prefab, out var canvas);

            Assert.AreEqual(layer, canvas.gameObject.layer);
        }
        #endregion

        #region Aufgabe 02
        [TestCase(Assets.avatarPrefab)]
        public void A02a_AvatarPrefabContainsIRotatableAvatar(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<IRotatableAvatar>(prefab);
        }
        #endregion

        #region Aufgabe 03
        [TestCase(Assets.blockPrefab)]
        public void A03a_BlockPrefabExists(string path) {
            Assert.IsTrue(LoadPrefab(path));
        }
        [TestCase(Assets.blockPrefab, Assets.blockTag)]
        public void A03b_BlockPrefabHasTag(string path, string tag) {
            var prefab = LoadPrefab(path);
            Assert.AreEqual(tag, prefab.tag, $"Block must have a tag!");
        }
        [TestCase(Assets.blockPrefab)]
        public void A03c_BlockPrefabContainsTransform(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<Transform>(prefab, out var transform);
            CustomAssert.AreEqual(Vector3.zero, transform.localPosition, $"Block's Transform is at the wrong position!");
            CustomAssert.AreEqual(Quaternion.identity, transform.localRotation, $"Block's Transform has the wrong rotation!");
            CustomAssert.AreEqual(Vector3.one, transform.localScale, $"Block's Transform is of the wrong scale!");
        }
        [TestCase(Assets.blockPrefab)]
        public void A03d_BlockPrefabContainsBoxCollider(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<BoxCollider>(prefab, out var boxCollider);
            CustomAssert.AreEqual(Vector3.one, boxCollider.size, $"Block's Collider is of the wrong size!");
        }
        [TestCase(Assets.blockPrefab)]
        public void A03e_BlockPrefabContainsMeshRenderer(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<MeshRenderer>(prefab, out var renderer);
            CustomAssert.AreEqual(Vector3.one, renderer.bounds.size, $"Block's Renderer is of the wrong size!");
        }
        [TestCase(Assets.blockPrefab)]
        public void A03f_BlockPrefabContainsMeshFilter(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<MeshFilter>(prefab, out var filter);
            Assert.IsTrue(filter.sharedMesh, $"Block's MeshFilter is missing its mesh!");
        }
        #endregion

        #region Aufgabe 04
        [TestCase(Assets.avatarPrefab)]
        public void A04a_AvatarPrefabContainsIBuildingAvatar(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<IBuildingAvatar>(prefab);
        }
        #endregion

        #region Aufgabe 05
        [TestCase(Assets.playerControlsAsset, Assets.playerLookAction)]
        [TestCase(Assets.playerControlsAsset, Assets.playerBuildBlockAction)]
        [TestCase(Assets.playerControlsAsset, Assets.playerDestroyBlockAction)]
        public void A05a_PlayerControlsHaveAction(string path, string action) {
            var asset = LoadAsset<InputActionAsset>(path);
            Assert.IsNotNull(asset.FindAction(action, true));
        }
        #endregion
    }
}
