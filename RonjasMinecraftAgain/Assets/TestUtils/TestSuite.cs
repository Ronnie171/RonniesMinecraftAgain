using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TestUtils {
    public class TestSuite {
        protected virtual float timeScale => 1;

        protected InputTestFixture input;
        protected Keyboard keyboard;

        protected Scene currentScene => loadedScene.IsValid()
            ? loadedScene
            : testRunnerScene;

        private Scene testRunnerScene;
        private GameObject testRunnerObject;
        [UnitySetUp]
        public IEnumerator UnitySetUp() {
            if (!Application.isPlaying) {
                yield break;
            }

            yield return null;
            input = new InputTestFixture();
            input.Setup();
            yield return null;
            keyboard = InputSystem.AddDevice<Keyboard>();
            yield return null;
            testRunnerScene = SceneManager.GetActiveScene();
            testRunnerObject = testRunnerScene.GetRootGameObjects()[0];

            Time.timeScale = timeScale;
        }

        [UnityTearDown]
        public IEnumerator UnityTearDown() {
            if (!Application.isPlaying) {
                yield break;
            }

            Time.timeScale = 1;

            yield return UnloadScene();

            while (instantiatedObjects.Count > 0) {
                var obj = instantiatedObjects[0];
                if (obj) {
                    DestroyGameObject(obj);
                    yield return null;
                } else {
                    instantiatedObjects.RemoveAt(0);
                }
            }

            input.TearDown();

            do {
                foreach (var obj in currentScene.GetRootGameObjects()) {
                    if (obj && obj != testRunnerObject) {
                        UnityEngine.Object.Destroy(obj);
                        yield return null;
                    }
                }
                yield return null;
            } while (currentScene.GetRootGameObjects().Length > 1);
        }

        private Scene loadedScene;
        protected IEnumerator LoadScene(string name) {
            Assert.IsFalse(loadedScene.IsValid(), $"Must only load 1 scene from within tests!");
            var async = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            yield return async;
            loadedScene = SceneManager.GetSceneByName(name);
            Assert.IsTrue(loadedScene.IsValid(), $"Scene {name} could not be loaded, help!");
            SceneManager.SetActiveScene(loadedScene);
            yield return new WaitForFixedUpdate();
        }
        protected IEnumerator UnloadScene() {
            SceneManager.SetActiveScene(testRunnerScene);
            if (loadedScene.IsValid()) {
                var async = SceneManager.UnloadSceneAsync(loadedScene);
                yield return async;
                loadedScene = default;
            }
        }
        protected T LoadAsset<T>(string path) where T : class {
            FileAssert.Exists(new FileInfo(path));
            var asset = AssetDatabase.LoadMainAssetAtPath(path);
            Assert.IsTrue(asset, $"Could not load asset of type {typeof(T).Name} at path '{path}'!");
            Assert.IsInstanceOf(typeof(T), asset, $"");
            return asset as T;
        }
        protected T[] LoadAssets<T>(string path) where T : class {
            FileAssert.Exists(new FileInfo(path));
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            Assert.IsNotNull(assets, $"Could not load asset of type {typeof(T).Name} at path '{path}'!");
            return assets
                .OfType<T>()
                .ToArray();
        }
        protected GameObject LoadPrefab(string path) {
            FileAssert.Exists(new FileInfo(path));
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Assert.IsTrue(prefab, $"Could not load prefab at path '{path}'!");
            return prefab;
        }
        protected GameObject LoadPrefabInstance(string path) {
            var prefab = LoadPrefab(path);
            var instances = PrefabUtility.FindAllInstancesOfPrefab(prefab);
            CollectionAssert.IsNotEmpty(instances, $"Scene does not contain instance of prefab '{prefab.name}'!");
            return instances[0];
        }
        protected GameObject[] LoadPrefabInstances(string path) {
            var prefab = LoadPrefab(path);
            var instances = PrefabUtility.FindAllInstancesOfPrefab(prefab);
            return instances;
        }

        protected readonly List<GameObject> instantiatedObjects = new List<GameObject>();
        protected GameObject InstantiateGameObject(GameObject prefab) {
            return InstantiateGameObject(prefab, Vector3.zero);
        }
        protected GameObject InstantiateGameObject(GameObject prefab, Vector3 position) {
            return InstantiateGameObject(prefab, position, Quaternion.identity);
        }
        protected GameObject InstantiateGameObject(GameObject prefab, Vector3 position, Quaternion rotation) {
            var instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
            instantiatedObjects.Add(instance);
            return instance;
        }
        protected GameObject InstantiateGameObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) {
            var instance = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
            instantiatedObjects.Add(instance);
            return instance;
        }
        protected void DestroyGameObject(GameObject instance) {
            Assert.IsTrue(instantiatedObjects.Contains(instance));
            instantiatedObjects.Remove(instance);
            UnityEngine.Object.Destroy(instance);
        }
        protected GameObject CreateGameObject(string name = default) {
            var instance = new GameObject(name ?? "Temp");
            instantiatedObjects.Add(instance);
            return instance;
        }
        protected GameObject CreatePrimitive(PrimitiveType type, string name = default) {
            var instance = GameObject.CreatePrimitive(type);
            instance.name = name ?? type.ToString();
            instantiatedObjects.Add(instance);
            return instance;
        }

        protected T[] FindObjectsInScene<T>() where T : Component {
            return currentScene
                .GetRootGameObjects()
                .SelectMany(obj => obj.GetComponentsInChildren<T>())
                .ToArray();
        }
        protected T[] FindNewObjectsInScene<T>(ISet<T> storage) where T : Component {
            return FindObjectsInScene<T>()
                .Where(storage.Add)
                .ToArray();
        }
        protected IEnumerable<GameObject> FindGameObjectsInScene() {
            return currentScene
                .GetRootGameObjects()
                .SelectMany(obj => obj.GetComponentsInChildren<Transform>())
                .Select(transform => transform.gameObject);
        }
        protected IEnumerable<GameObject> FindNewGameObjectsInScene(ISet<GameObject> storage) {
            foreach (var obj in FindGameObjectsInScene()) {
                bool added = false;
                for (var transform = obj.transform; transform; transform = transform.parent) {
                    if (storage.Contains(transform.gameObject)) {
                        added = true;
                        break;
                    }
                }
                if (!added && storage.Add(obj)) {
                    yield return obj;
                }
            }
        }

        protected IEnumerator WaitFor(float duration, Func<bool> predicate) {
            float timeout = Time.realtimeSinceStartup + (duration / timeScale);
            yield return new WaitUntil(() => {
                return Time.realtimeSinceStartup > timeout || predicate();
            });
        }
    }
}