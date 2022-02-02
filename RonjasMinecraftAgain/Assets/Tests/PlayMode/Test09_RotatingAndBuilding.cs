using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class Test09_RotatingAndBuilding : MinecraftTestSuite {
        private static readonly string[] avatarPaths = new[] {
            Assets.avatarPrefab,
        };
        private static readonly int[] rotations = new[] {
            0,
            45,
            -45,
        };
        private static readonly int[] movements = new[] {
            1,
            -1,
        };
        private static readonly Quaternion[] bodyRotations = new[] {
            Quaternion.identity,
            Quaternion.Euler(0, 90, 0),
            Quaternion.Euler(0, -90, 0),
            Quaternion.Euler(0, 180, 0),
            Quaternion.Euler(0, 45, 0),
        };
        private static readonly Quaternion[] headRotations = new[] {
            Quaternion.identity,
            Quaternion.Euler(-45, 0, 0),
        };

        private const int spawnDuration = 2;
        protected override float timeScale => 2;

        #region Aufgabe 01
        [UnityTest]
        public IEnumerator A01d_AvatarCanvasHasCrosshairs(
            [ValueSource(nameof(avatarPaths))] string path) {
            var prefab = LoadPrefab(path);
            yield return new WaitForFixedUpdate();
            var instance = InstantiateGameObject(prefab, Vector3.zero, Quaternion.identity);
            yield return new WaitForFixedUpdate();

            CustomAssert.HasComponentInChildren<Canvas>(instance, out var canvas);

            var eventSystem = EventSystem.current
                ? EventSystem.current
                : canvas.gameObject.AddComponent<InputSystemUIInputModule>().GetComponent<EventSystem>();

            yield return new WaitForFixedUpdate();
            yield return null;

            var data = new PointerEventData(eventSystem) {
                position = new Vector2(Screen.width, Screen.height) / 2,
            };
            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(data, results);

            Assert.IsNotEmpty(results, $"Tried to find crosshairs in the center of the screen, but didn't.");

            DestroyGameObject(instance);
            yield return new WaitForFixedUpdate();
        }
        #endregion

        #region Aufgabe 02
        [UnityTest]
        public IEnumerator A02b_TestGetBodyRotation(
            [ValueSource(nameof(rotations))] int x) {
            int y = 90;
            int z = 180;
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            body.rotation = Quaternion.Euler(x, y, z);

            CustomAssert.AreEqual(body.localRotation, rotatableAvatar.GetBodyRotation(), $"{nameof(rotatableAvatar.GetBodyRotation)} should always return the avatar's body's current local rotation!");

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A02c_TestGetHeadRotation(
            [ValueSource(nameof(rotations))] int x) {
            int y = 90;
            int z = 180;
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            head.rotation = Quaternion.Euler(x, y, z);

            CustomAssert.AreEqual(head.localRotation, rotatableAvatar.GetHeadRotation(), $"{nameof(rotatableAvatar.GetHeadRotation)} should always return the avatar's head's current local rotation!");

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A02d_TestHeadRotationIsLocal(
            [ValueSource(nameof(rotations))] int x) {
            int y = 90;
            int z = 180;
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            body.rotation = Quaternion.Euler(x + 180, y + 180, z);
            head.rotation = Quaternion.Euler(x, y, z);

            CustomAssert.AreEqual(head.localRotation, rotatableAvatar.GetHeadRotation(), $"{nameof(rotatableAvatar.GetHeadRotation)} should always return the avatar's head's current local rotation!");

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A02e_TestThatRotateByRotatesTheBody(
            [ValueSource(nameof(rotations))] int x,
            [ValueSource(nameof(rotations))] int y) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var previousBody = rotatableAvatar.GetBodyRotation();

            rotatableAvatar.RotateBy(new Vector2(x, y));

            var newBody = rotatableAvatar.GetBodyRotation();

            var deltaAngle = (newBody * Quaternion.Inverse(previousBody)).eulerAngles;

            Assert.AreEqual(0, Math.Sign(deltaAngle.x), $"{nameof(rotatableAvatar.RotateBy)} should NOT rotate the avatar's body around the X axis!");
            Assert.AreEqual(0, Math.Sign(deltaAngle.z), $"{nameof(rotatableAvatar.RotateBy)} should NOT rotate the avatar's body around the Z axis!");

            if (x == 0) {
                CustomAssert.AreEqual(previousBody, newBody, $"When delta.x is zero, {nameof(rotatableAvatar.RotateBy)} should not rotate the avatar's body!");
            } else {
                CustomAssert.AreEqual(previousBody * Quaternion.Euler(0, x, 0), newBody, $"{nameof(rotatableAvatar.RotateBy)} should rotate the avatar's body around the Y axis in the direction of delta.x ('10' means 'turn 10° to the right', '-10' means 'turn 10° to the left')!");
            }

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator A02f_TestThatRotateByRotatesTheHead(
            [ValueSource(nameof(rotations))] int x,
            [ValueSource(nameof(rotations))] int y) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var previousHead = rotatableAvatar.GetHeadRotation();

            rotatableAvatar.RotateBy(new Vector2(x, y));

            var newHead = rotatableAvatar.GetHeadRotation();

            var deltaAngle = (newHead * Quaternion.Inverse(previousHead)).eulerAngles;

            Assert.AreEqual(0, Math.Sign(deltaAngle.y), $"{nameof(rotatableAvatar.RotateBy)} should NOT rotate the avatar's head around the Y axis!");
            Assert.AreEqual(0, Math.Sign(deltaAngle.z), $"{nameof(rotatableAvatar.RotateBy)} should NOT rotate the avatar's head around the Z axis!");

            if (y == 0) {
                CustomAssert.AreEqual(previousHead, newHead, $"When delta.y is zero, {nameof(rotatableAvatar.RotateBy)} should not rotate the avatar's head!");
            } else {
                CustomAssert.AreEqual(previousHead * Quaternion.Euler(y, 0, 0), newHead, $"{nameof(rotatableAvatar.RotateBy)} should rotate the avatar's head around the Y axis in the direction of delta.x ('10' means 'look 10° up', '-10' means 'look 10° down')!");
            }

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator A02g_TestThatSetIntendedMovementHonorsAvatarRotation(
            [ValueSource(nameof(rotations))] int rotateX,
            [ValueSource(nameof(movements))] int movementX,
            [ValueSource(nameof(movements))] int movementY) {
            int rotateY = 45;
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var rotation = new Vector2(rotateX, rotateY);
            var input = new Vector2(movementX, movementY);
            var targetDirection = (Quaternion.Euler(0, rotateX, 0) * avatar.transform.rotation * new Vector3(movementX, 0, movementY)).normalized;

            rotatableAvatar.RotateBy(rotation);

            movableAvatar.SetIntendedMovement(input);

            var position = avatar.transform.position;

            for (int i = 0; i < 10; i++) {
                yield return new WaitForFixedUpdate();
                var direction = avatar.transform.position - position;
                direction.y = 0;
                CustomAssert.AreNotEqual(Vector3.zero, direction, $"With rotation {rotation} and intended movement {input}, Avatar should have moved in direction {targetDirection}!");
                CustomAssert.AreEqual(targetDirection, direction.normalized, $"With rotation {rotation} and intended movement {input}, Avatar should have moved in direction {targetDirection}!");
                position = avatar.transform.position;
            }

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        [TestCase(120, 90, ExpectedResult = null)]
        [TestCase(-120, -90, ExpectedResult = null)]
        public IEnumerator A02h_TestThatHeadRotationDoesNotOvershoot(int inputY, int expectedX) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var rotation = Quaternion.Euler(expectedX, 0, 0);
            var input = new Vector2(0, inputY);


            for (int i = 0; i < 3; i++) {
                rotatableAvatar.RotateBy(input);
                CustomAssert.AreEqual(rotation, rotatableAvatar.GetHeadRotation(), $"With input {input}, Avatar's head rotation should be at exactly {expectedX}°!");
            }

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }
        #endregion

        #region Aufgabe 04
        [UnityTest]
        [TestCase(1, ExpectedResult = null)]
        [TestCase(10, ExpectedResult = null)]
        public IEnumerator A04a_TestReach(float reach) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            buildingAvatar.SetReach(reach);

            Assert.AreEqual(reach, buildingAvatar.GetReach(), $"{nameof(buildingAvatar.GetReach)} must return the value previously set via {nameof(buildingAvatar.SetReach)}!");

            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        [TestCase(Assets.blockTag, 5, true, ExpectedResult = null)]
        [TestCase(Assets.blockTag, 15, false, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 5, false, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 15, false, ExpectedResult = null)]
        public IEnumerator A04b_TestThatDestroyBlockHonorsTagAndReach(string tag, int distance, bool expectedResult) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            body.rotation = Quaternion.identity;
            head.rotation = Quaternion.identity;

            var obj = CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = RoundToInt(head.position + head.forward * distance);
            obj.tag = tag;

            yield return new WaitForFixedUpdate();


            buildingAvatar.SetReach(10);
            buildingAvatar.DestroyBlock();

            yield return new WaitForFixedUpdate();

            if (expectedResult) {
                Assert.IsFalse(obj, $"{nameof(buildingAvatar.DestroyBlock)} should destroy blocks tagged '{tag}' with distance {distance}!");
            } else {
                Assert.IsTrue(obj, $"{nameof(buildingAvatar.DestroyBlock)} should NOT destroy blocks tagged '{tag}' with distance {distance}!");
            }

            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A04c_TestThatDestroyBlockHonorsBodyAndHeadRotation(
            [ValueSource(nameof(bodyRotations))] Quaternion bodyRotation,
            [ValueSource(nameof(headRotations))] Quaternion headRotation) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            body.rotation = bodyRotation;
            head.rotation = headRotation;

            var obj = CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = RoundToInt(head.position + head.forward * 5);
            obj.tag = Assets.blockTag;

            yield return new WaitForFixedUpdate();


            buildingAvatar.SetReach(10);
            buildingAvatar.DestroyBlock();

            yield return new WaitForFixedUpdate();

            Assert.IsFalse(obj, $"{nameof(buildingAvatar.DestroyBlock)} should destroy blocks even when body rotation is {bodyRotation.eulerAngles} and head rotation is {headRotation.eulerAngles}!");

            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        [TestCase(Assets.blockTag, 5, true, ExpectedResult = null)]
        [TestCase(Assets.blockTag, 15, false, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 5, true, ExpectedResult = null)]
        [TestCase(Assets.emptyTag, 15, false, ExpectedResult = null)]
        public IEnumerator A04d_TestThatBuildBlockHonorsTagAndReach(string tag, int distance, bool expectedResult) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var obj = CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = RoundToInt(head.position + head.forward * distance);
            obj.tag = tag;

            yield return new WaitForFixedUpdate();


            buildingAvatar.SetReach(10);
            buildingAvatar.BuildBlock();

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(obj, "Block must not get destroyed!");
            Assert.IsTrue(Physics.Raycast(head.position, head.forward, out var info));

            var newObj = info.collider.gameObject;
            instantiatedObjects.Add(newObj);

            if (expectedResult) {
                Assert.AreNotEqual(obj, newObj, $"{nameof(buildingAvatar.BuildBlock)} should attach a new GameObject in front of blocks tagged '{tag}' with distance {distance}!");
                AssertBuiltBlock(newObj);
            } else {
                Assert.AreEqual(obj, newObj, $"{nameof(buildingAvatar.BuildBlock)} should NOT attach a new GameObject in front of blocks tagged '{tag}' with distance {distance}!");
            }

            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        [TestCase(-90, ExpectedResult = null)]
        [TestCase(-85, ExpectedResult = null)]
        [TestCase(90, ExpectedResult = null)]
        [TestCase(85, ExpectedResult = null)]
        public IEnumerator A04e_TestThatBuildBlockDoesNotBuildWhereTheAvatarIsStanding(int headRotation) {
            yield return SpawnAvatarOnGround(Assets.avatarSpawnPoint, spawnDuration);

            var obj = CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = RoundToInt(ground.transform.position + Vector3.up * 3);
            obj.tag = Assets.blockTag;

            body.rotation = Quaternion.identity;
            head.rotation = Quaternion.Euler(headRotation, 0, 0);

            yield return new WaitForFixedUpdate();

            buildingAvatar.SetReach(10);
            buildingAvatar.BuildBlock();

            yield return new WaitForFixedUpdate();

            Assert.IsTrue(Physics.Raycast(head.position, head.forward, out var info));

            var newObj = info.collider.gameObject;
            instantiatedObjects.Add(newObj);

            var targetObj = headRotation > 0
                ? ground
                : obj;
            Assert.AreEqual(targetObj, newObj, $"{nameof(buildingAvatar.BuildBlock)} should NOT spawn blocks when the avatar is looking at angle {headRotation}°!");

            yield return new WaitForFixedUpdate();
        }

        private void AssertBuiltBlock(GameObject obj) {
            Assert.AreEqual((Vector3)RoundToInt(obj.transform.position), obj.transform.position, $"{nameof(buildingAvatar.BuildBlock)} should spawn new objects on an integer grid!");
            Assert.AreEqual(Assets.blockTag, obj.tag, $"{nameof(buildingAvatar.BuildBlock)} should spawn new objects with the block tag!");
        }
        #endregion
    }
}