using NUnit.Framework;
using TestInterfaces;
using TestUtils;
using UnityEngine;

namespace Tests.EditMode {
    public class Test10_Level : TestSuite {
        #region Aufgabe 01
        [TestCase(Assets.stoneBlockPrefab)]
        public void A01a_BlockPrefabExists(string path) {
            Assert.IsTrue(LoadPrefab(path));
        }
        [TestCase(Assets.stoneBlockPrefab, Assets.blockTag)]
        public void A01b_BlockPrefabHasTag(string path, string tag) {
            var prefab = LoadPrefab(path);
            Assert.AreEqual(tag, prefab.tag, $"Block must have a tag!");
        }
        [TestCase(Assets.stoneBlockPrefab)]
        public void A01c_BlockPrefabContainsTransform(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<Transform>(prefab, out var transform);
            CustomAssert.AreEqual(Vector3.zero, transform.localPosition, $"Block's Transform is at the wrong position!");
            CustomAssert.AreEqual(Quaternion.identity, transform.localRotation, $"Block's Transform has the wrong rotation!");
            CustomAssert.AreEqual(Vector3.one, transform.localScale, $"Block's Transform is of the wrong scale!");
        }
        [TestCase(Assets.stoneBlockPrefab)]
        public void A01d_BlockPrefabContainsBoxCollider(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<BoxCollider>(prefab, out var boxCollider);
            CustomAssert.AreEqual(Vector3.one, boxCollider.size, $"Block's Collider is of the wrong size!");
        }
        #endregion

        #region Aufgabe 02
        [TestCase(Assets.levelPrefab)]
        public void A02a_LevelPrefabExists(string path) {
            LoadPrefab(path);
        }
        [TestCase(Assets.levelPrefab)]
        public void A02b_LevelPrefabContainsILevel(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<ILevel>(prefab);
        }
        #endregion

        #region Aufgabe 03
        [TestCase(Assets.avatarPrefab)]
        public void A03a_AvatarPrefabContainsILevelAvatar(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<ILevelAvatar>(prefab);
        }
        #endregion
    }
}
