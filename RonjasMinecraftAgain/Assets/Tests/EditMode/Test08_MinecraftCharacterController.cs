using NUnit.Framework;
using System;
using System.Linq;
using TestInterfaces;
using TestUtils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tests.EditMode {
    public class Test08_MinecraftCharacterController : TestSuite {
        #region Aufgabe 02
        [TestCase(Assets.playerControlsAsset)]
        public void A02a_PlayerControlsAssetExists(string path) {
            Assert.IsTrue(LoadAsset<InputActionAsset>(path));
        }
        [TestCase(Assets.playerControlsScript)]
        public void A02b_PlayerControlsScriptExists(string path) {
            Assert.IsTrue(LoadAsset<MonoScript>(path));
        }
        [TestCase(Assets.playerControlsAsset, Assets.playerActionMap)]
        public void A02c_PlayerControlsHaveActionMap(string path, string actionMap) {
            var asset = LoadAsset<InputActionAsset>(path);
            Assert.IsNotNull(asset.FindActionMap(actionMap, true));
        }
        [TestCase(Assets.playerControlsAsset, Assets.playerMoveAction)]
        [TestCase(Assets.playerControlsAsset, Assets.playerJumpAction)]
        public void A02d_PlayerControlsHaveAction(string path, string action) {
            var asset = LoadAsset<InputActionAsset>(path);
            Assert.IsNotNull(asset.FindAction(action, true));
        }
        #endregion

        #region Aufgabe 03
        [TestCase(Assets.floorPrefab)]
        public void A03a_FloorPrefabExists(string path) {
            Assert.IsTrue(LoadPrefab(path));
        }
        [TestCase(Assets.floorPrefab)]
        public void A03b_FloorPrefabContainsTransform(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<Transform>(prefab, out var transform);
            CustomAssert.AreEqual(new Vector3(127.5f, -1, 127.5f), transform.localPosition, $"Floor's Transform is at the wrong position!");
            CustomAssert.AreEqual(Quaternion.identity, transform.localRotation, $"Floor's Transform has the wrong rotation!");
            CustomAssert.AreEqual(new Vector3(1, 1, 1), transform.localScale, $"Floor's Transform is of the wrong scale!");
        }
        [TestCase(Assets.floorPrefab)]
        public void A03c_FloorPrefabContainsBoxCollider(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<BoxCollider>(prefab, out var boxCollider);
            CustomAssert.AreEqual(new Vector3(256, 1, 256), boxCollider.size, $"Floor's Collider is of the wrong size!");
        }
        [TestCase(Assets.floorPrefab)]
        public void A03d_FloorPrefabContainsMeshRenderer(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<MeshRenderer>(prefab, out var renderer);
            CustomAssert.AreEqual(new Vector3(256, 1, 256), renderer.bounds.size, $"Floor's Renderer is of the wrong size!");
        }
        [TestCase(Assets.floorPrefab)]
        public void A03e_FloorPrefabContainsMeshFilter(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<MeshFilter>(prefab, out var filter);
            Assert.IsTrue(filter.sharedMesh, $"Floor's MeshFilter is missing its mesh!");
        }
        #endregion

        #region Aufgabe 04
        [TestCase(Assets.avatarPrefab)]
        public void A04a_AvatarPrefabExists(string path) {
            Assert.IsTrue(LoadPrefab(path));
        }
        [TestCase(Assets.avatarPrefab)]
        public void A04b_AvatarPrefabContainsTransform(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<Transform>(prefab, out var transform);
            CustomAssert.AreEqual(new Vector3(0, 0, 0), transform.localPosition, $"Avatar's Transform is at the wrong position!");
            CustomAssert.AreEqual(Quaternion.identity, transform.localRotation, $"Avatar's Transform has the wrong rotation!");
            CustomAssert.AreEqual(new Vector3(1, 1, 1), transform.localScale, $"Avatar's Transform is of the wrong scale!");
        }
        [TestCase(Assets.avatarPrefab)]
        public void A04c_AvatarPrefabContainsIMovableAvatar(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<IMovableAvatar>(prefab);
        }
        [TestCase(Assets.avatarPrefab)]
        public void A04d_AvatarPrefabContainsCharacterController(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponent<CharacterController>(prefab, out var character);

            Assert.That(character.height, Is.EqualTo(Assets.avatarHeight), "Avatar's height is wrong!");
            Assert.That(character.radius, Is.EqualTo(Assets.avatarRadius), "Avatar's radius is wrong!");
            CustomAssert.AreEqual(new Vector3(0, Assets.avatarHeight / 2, 0), character.center, $"Avatar's center should be at its center!");
        }
        [TestCase(Assets.avatarPrefab)]
        public void A04e_AvatarPrefabContainsCamera(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Camera>(prefab, out var camera);

            CustomAssert.AreEqual(new Vector3(0, Assets.avatarEyeHeight, 0), camera.transform.localPosition, $"Avatar's camera should be at eye level!");
        }
        [TestCase(Assets.avatarPrefab)]
        public void A04f_AvatarPrefabCameraIsCentered(string path) {
            var prefab = LoadPrefab(path);
            CustomAssert.HasComponentInChildren<Camera>(prefab, out var camera);

            CustomAssert.AreEqual(Quaternion.identity, camera.transform.localRotation, $"Avatar's camera should not start off rotated!");
        }
        #endregion

        #region Aufgabe 05
        [TestCase(Assets.mainScenePath, Assets.avatarPrefab, 1)]
        [TestCase(Assets.mainScenePath, Assets.floorPrefab, 1)]
        public void A05a_SceneContainsPrefabInstance(string scenePath, string prefabPath, int count) {
            var scene = EditorSceneManager.OpenScene(scenePath);
            var prefab = LoadPrefab(prefabPath);
            var instances = PrefabUtility.FindAllInstancesOfPrefab(prefab);
            Assert.AreEqual(count, instances.Length, $"Expected {count} instance(s) of prefab '{prefab.name}' in scene '{scene.name}'!");
        }
        [TestCase(Assets.mainScenePath, typeof(Camera), 1)]
        [TestCase(Assets.mainScenePath, typeof(CharacterController), 1)]
        public void A05b_SceneContainsObjectsWithComponent(string scenePath, Type type, int count) {
            var scene = EditorSceneManager.OpenScene(scenePath);
            var components = scene.GetRootGameObjects()
                .SelectMany(obj => obj.GetComponentsInChildren(type))
                .ToArray();
            Assert.AreEqual(count, components.Length, $"Expected {count} object(s) with component '{type}' in scene '{scene.name}'!");
        }
        #endregion
    }
}
