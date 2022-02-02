using NUnit.Framework;
using TestInterfaces;
using TestUtils;
using UnityEngine;

namespace Tests.EditMode {
    public class Test11_BlockLogic : TestSuite {
        #region Aufgabe 02
        [TestCase(Assets.sandBlockPrefab)]
        [TestCase(Assets.gravelBlockPrefab)]
        [TestCase(Assets.dirtBlockPrefab)]
        [TestCase(Assets.grassBlockPrefab)]
        public void A02a_BlockPrefabExists(string path) {
            Assert.IsTrue(LoadPrefab(path));
        }
        [TestCase(Assets.sandBlockPrefab, Assets.blockTag)]
        [TestCase(Assets.gravelBlockPrefab, Assets.blockTag)]
        [TestCase(Assets.dirtBlockPrefab, Assets.blockTag)]
        [TestCase(Assets.grassBlockPrefab, Assets.blockTag)]
        public void A02b_BlockPrefabHasTag(string path, string tag) {
            var prefab = LoadPrefab(path);
            Assert.AreEqual(tag, prefab.tag, $"Block must have a tag!");
        }
        [TestCase(Assets.sandBlockPrefab)]
        [TestCase(Assets.gravelBlockPrefab)]
        [TestCase(Assets.dirtBlockPrefab)]
        [TestCase(Assets.grassBlockPrefab)]
        public void A02c_BlockPrefabContainsTransform(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<Transform>(prefab, out var transform);
            CustomAssert.AreEqual(Vector3.zero, transform.localPosition, $"Block's Transform is at the wrong position!");
            CustomAssert.AreEqual(Quaternion.identity, transform.localRotation, $"Block's Transform has the wrong rotation!");
            CustomAssert.AreEqual(Vector3.one, transform.localScale, $"Block's Transform is of the wrong scale!");
        }
        [TestCase(Assets.sandBlockPrefab)]
        [TestCase(Assets.gravelBlockPrefab)]
        [TestCase(Assets.dirtBlockPrefab)]
        [TestCase(Assets.grassBlockPrefab)]
        public void A02d_BlockPrefabContainsBoxCollider(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<BoxCollider>(prefab, out var boxCollider);
            CustomAssert.AreEqual(Vector3.one, boxCollider.size, $"Block's Collider is of the wrong size!");
        }
        #endregion
    }
}
