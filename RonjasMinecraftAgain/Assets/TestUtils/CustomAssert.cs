using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace TestUtils {
    public static class CustomAssert {
        public static void HasComponent<T>(GameObject obj) {
            HasComponent<T>(obj, out _);
        }
        public static void HasComponent<T>(GameObject obj, out T component) {
            Assert.IsTrue(obj, $"GameObject with copmonent {typeof(T)} is missing!");
            Assert.IsTrue(obj.TryGetComponent<T>(out component), $"GameObject {obj} is missing a {typeof(T)} component!");
        }
        public static void HasComponentInChildren<T>(GameObject obj) {
            HasComponentInChildren<T>(obj, out _);
        }
        public static void HasComponentInChildren<T>(GameObject obj, out T component) {
            Assert.IsTrue(obj, $"GameObject with copmonent {typeof(T)} is missing!");
            component = obj.GetComponentInChildren<T>();
            Assert.IsNotNull(component, $"GameObject {obj} is missing a {typeof(T)} component!");
        }
        public static void InBounds(float actual, float expectedMinimum, float expectedMaximum, string message) {
            Assert.GreaterOrEqual(actual, expectedMinimum, message);
            Assert.LessOrEqual(actual, expectedMaximum, message);
        }

        public static void AreEqual(Color expected, Color actual, string message) {
            Assert.That(actual, Is.EqualTo(expected).Using(ColorEqualityComparer.Instance), message);
        }
        public static void AreEqual(float expected, float actual, string message) {
            Assert.That(actual, Is.EqualTo(expected).Using(FloatEqualityComparer.Instance), message);
        }
        public static void AreEqual(Vector2 expected, Vector2 actual, string message) {
            Assert.That(actual, Is.EqualTo(expected).Using(Vector2EqualityComparer.Instance), message);
        }
        public static void AreEqual(Vector3 expected, Vector3 actual, string message) {
            Assert.That(actual, Is.EqualTo(expected).Using(Vector3EqualityComparer.Instance), message);
        }
        public static void AreEqual(Quaternion expected, Quaternion actual, string message) {
            Assert.That(actual, Is.EqualTo(expected).Using(QuaternionEqualityComparer.Instance), message);
        }

        public static void AreNotEqual(Color expected, Color actual, string message) {
            Assert.That(actual, Is.Not.EqualTo(expected).Using(ColorEqualityComparer.Instance), message);
        }
        public static void AreNotEqual(float expected, float actual, string message) {
            Assert.That(actual, Is.Not.EqualTo(expected).Using(FloatEqualityComparer.Instance), message);
        }
        public static void AreNotEqual(Vector2 expected, Vector2 actual, string message) {
            Assert.That(actual, Is.Not.EqualTo(expected).Using(Vector2EqualityComparer.Instance), message);
        }
        public static void AreNotEqual(Vector3 expected, Vector3 actual, string message) {
            Assert.That(actual, Is.Not.EqualTo(expected).Using(Vector3EqualityComparer.Instance), message);
        }
        public static void AreNotEqual(Quaternion expected, Quaternion actual, string message) {
            Assert.That(actual, Is.Not.EqualTo(expected).Using(QuaternionEqualityComparer.Instance), message);
        }
    }
}