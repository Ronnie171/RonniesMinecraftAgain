using NUnit.Framework;
using System.Collections;
using System.Linq;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode {
    public class Test08_MinecraftCharacterController : MinecraftTestSuite {
        private static readonly string[] avatarPaths = new[] {
            Assets.avatarPrefab,
        };
        private static readonly Vector3[] positions = new[] {
            new Vector3(50, 0, 0),
            new Vector3(100, 0, 0),
        };
        private static readonly (float height, bool canReach)[] jumps = new[] {
            (0.75f, true),
            (1f, true),
            (1.25f, true),
            (1.5f, true),
            (2f, false),
        };
        private static readonly (Vector2 input, Vector3 direction)[] moves = new[] {
            (Vector2.up, Vector3.forward),
            (Vector2.down, Vector3.back),
            (Vector2.left, Vector3.left),
            (Vector2.right, Vector3.right),
        };
        private const int fallDuration = 2;
        private const int jumpDuration = 1;
        protected override float timeScale => 2;

        #region Aufgabe 04
        [UnityTest]
        public IEnumerator A04g_AvatarFallsDown(
            [ValueSource(nameof(avatarPaths))] string path,
            [ValueSource(nameof(positions))] Vector3 position) {
            var prefab = LoadPrefab(path);
            yield return new WaitForFixedUpdate();
            var instance = InstantiateGameObject(prefab, position, Quaternion.identity);
            CustomAssert.AreEqual(position, instance.transform.position, $"Instantiating should not change position!");
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(instance, $"Avatar should not destroy itself?!");
            var newPosition = instance.transform.position;
            Assert.Less(newPosition.y, position.y, $"After waiting 1 FixedUpdate frame, avatar should have moved downwards!");
            newPosition.y = position.y;
            CustomAssert.AreEqual(position, newPosition, $"Falling downwards should not affect X or Z coordinates!");
            DestroyGameObject(instance);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A04h_AvatarFallUpdatesVelocity(
           [ValueSource(nameof(avatarPaths))] string path,
           [ValueSource(nameof(positions))] Vector3 position) {
            var prefab = LoadPrefab(path);
            yield return new WaitForFixedUpdate();
            var instance = InstantiateGameObject(prefab, position, Quaternion.identity);
            CustomAssert.HasComponent<CharacterController>(instance, out var character);
            CustomAssert.HasComponent<IMovableAvatar>(instance, out var movableAvatar);
            CustomAssert.AreEqual(position, instance.transform.position, $"Instantiating should not change position!");
            var oldVelocity = movableAvatar.GetVelocity();
            CustomAssert.AreEqual(Vector3.zero, oldVelocity, $"Instantiating should not change velocity!");

            yield return new WaitForFixedUpdate();

            var newVelocity = movableAvatar.GetVelocity();
            Assert.IsFalse(character.isGrounded, $"After waiting 1 FixedUpdate frame, Avatar no longer be grounded!");
            Assert.Less(newVelocity.y, oldVelocity.y, $"After waiting 1 FixedUpdate frame, Avatar's vertical velocity should have decreased!");
            newVelocity.y = oldVelocity.y;
            CustomAssert.AreEqual(oldVelocity, newVelocity, $"Falling downwards should not affect X or Z coordinates!");

            DestroyGameObject(instance);
            yield return new WaitForFixedUpdate();
        }
        #endregion

        #region Aufgabe 05
        [UnityTest]
        public IEnumerator A05c_AvatarLandsOnSomething() {
            yield return LoadScene(Assets.mainSceneName);
            yield return new WaitForFixedUpdate();
            var character = FindObjectsInScene<CharacterController>()
                .FirstOrDefault();
            Assert.IsTrue(character, $"Failed to find avatar instance in scene {Assets.mainSceneName}!");

            yield return WaitFor(fallDuration, () => character.isGrounded);

            Assert.IsTrue(character.isGrounded, $"After waiting {fallDuration}s, avatar should have become grounded!");

            yield return UnloadScene();
        }
        #endregion

        #region Aufgabe 06
        [TestCase(0, -30, 0)]
        public void A06a_VerifyGravity(float x, float y, float z) {
            CustomAssert.AreEqual(new Vector3(x, y, z), Physics.gravity, $"Gravity should be {y}m/s²!");
        }
        [UnityTest]
        public IEnumerator A06b_AvatarCanSpawnOnPlane([ValueSource(nameof(positions))] Vector3 position) {
            yield return SpawnAvatarOnGround(position, fallDuration);

            CustomAssert.AreEqual(Vector3.zero, movableAvatar.GetVelocity(), "After becoming grounded, Avatar's velocity should be zero!");

            DestroyGameObject(ground);
            DestroyGameObject(avatar);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A06c_AvatarCanJumpFromPlane([ValueSource(nameof(positions))] Vector3 position) {
            yield return SpawnAvatarOnGround(position, fallDuration);

            yield return WaitForJump(jumpDuration);

            yield return WaitForLandingOnPlane(ground, fallDuration);

            DestroyGameObject(ground);
            DestroyGameObject(avatar);

            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A06d_AvatarJumpSetsVelocity([ValueSource(nameof(positions))] Vector3 position) {
            yield return SpawnAvatarOnGround(position, fallDuration);

            movableAvatar.Jump();

            var velocity = movableAvatar.GetVelocity();
            Assert.Greater(movableAvatar.GetVelocity().y, 0, $"After invoking {nameof(IMovableAvatar.Jump)}, Avatar's vertical velocity should be positive!");

            yield return new WaitForFixedUpdate();

            for (int i = 0; i < 10; i++) {
                yield return new WaitForFixedUpdate();
                Assert.Less(movableAvatar.GetVelocity().y, velocity.y, $"After invoking {nameof(IMovableAvatar.Jump)}, Avatar's vertical velocity should continue to decrease!");
            }

            DestroyGameObject(ground);
            DestroyGameObject(avatar);

            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A06e_AvatarCannotJumpWhenAirborne([ValueSource(nameof(positions))] Vector3 position) {
            yield return SpawnAvatarOnGround(position, fallDuration);

            movableAvatar.Jump();
            var velocity = movableAvatar.GetVelocity();
            Assert.Greater(movableAvatar.GetVelocity().y, 0, $"After invoking {nameof(IMovableAvatar.Jump)}, Avatar's vertical velocity should be positive!");

            yield return new WaitForFixedUpdate();

            for (int i = 0; i < 10; i++) {
                yield return new WaitForFixedUpdate();
                movableAvatar.Jump();
                Assert.Less(movableAvatar.GetVelocity().y, velocity.y, $"After invoking {nameof(IMovableAvatar.Jump)}, Avatar's vertical velocity should continue to decrease even when spamming {nameof(IMovableAvatar.Jump)}!");
            }

            DestroyGameObject(ground);
            DestroyGameObject(avatar);

            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A06f_AvatarCanJumpAndLandOnPlatformOfACertainHeight(
            [ValueSource(nameof(jumps))] (float height, bool canReach) jump) {
            var position = jump.height * new Vector3(10, 0, 0);

            yield return SpawnAvatarOnGround(position, fallDuration);

            var platform = CreatePrimitive(PrimitiveType.Plane, "Platform");
            platform.transform.position = position + Vector3.up * jump.height;

            yield return WaitForJump(jumpDuration);

            yield return WaitForLandingOnPlane(jump.canReach ? platform : ground, fallDuration);

            DestroyGameObject(avatar);
            DestroyGameObject(ground);
            DestroyGameObject(platform);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A06g_AvatarCanMoveViaSetIntendedMovement(
            [ValueSource(nameof(moves))] (Vector2 input, Vector3 direction) move) {
            var position = 10 * move.direction;

            yield return SpawnAvatarOnGround(position, fallDuration);

            movableAvatar.SetIntendedMovement(move.input);

            for (int i = 0; i < 10; i++) {
                yield return new WaitForFixedUpdate();
                var direction = avatar.transform.position - position;
                direction.y = 0;
                CustomAssert.AreNotEqual(Vector3.zero, direction, $"With input {move.input}, Avatar should have moved in direction {move.direction}!");
                CustomAssert.AreEqual(move.direction, direction.normalized, $"With input {move.input}, Avatar should have moved in direction {move.direction}!");
                position = avatar.transform.position;
            }

            DestroyGameObject(avatar);
            DestroyGameObject(ground);
            yield return new WaitForFixedUpdate();
        }
        [UnityTest]
        public IEnumerator A06h_AvatarCanStopViaSetIntendedMovement(
            [ValueSource(nameof(moves))] (Vector2 input, Vector3 direction) move) {
            var position = 10 * move.direction;

            yield return SpawnAvatarOnGround(position, fallDuration);

            movableAvatar.SetIntendedMovement(move.input);

            for (int i = 0; i < 3; i++) {
                yield return new WaitForFixedUpdate();
            }

            position = avatar.transform.position;
            movableAvatar.SetIntendedMovement(Vector2.zero);

            for (int i = 0; i < 3; i++) {
                yield return new WaitForFixedUpdate();
                CustomAssert.AreEqual(position, avatar.transform.position, $"Without input, Avatar should stop immediately!");
            }

            DestroyGameObject(avatar);
            DestroyGameObject(ground);
            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator A06i_AvatarMovementUsesVelocity(
            [ValueSource(nameof(moves))] (Vector2 input, Vector3 direction) move) {
            var position = 10 * move.direction;

            yield return SpawnAvatarOnGround(position, fallDuration);

            movableAvatar.SetIntendedMovement(move.input);

            for (int i = 0; i < 3; i++) {
                yield return new WaitForFixedUpdate();
            }

            var velocity = new Vector2(movableAvatar.GetVelocity().x, movableAvatar.GetVelocity().z);
            CustomAssert.AreNotEqual(Vector2.zero, velocity, $"With input {move.input}, Avatar's velocity should have changed!");
            CustomAssert.AreEqual(velocity.normalized, move.input, $"With input {move.input}, Avatar's direction should have become {move.direction}!");

            DestroyGameObject(avatar);
            DestroyGameObject(ground);
            yield return new WaitForFixedUpdate();
        }

        [UnityTest]
        public IEnumerator A06j_AvatarMaximumMovementSpeed(
            [ValueSource(nameof(moves))] (Vector2 input, Vector3 direction) move) {
            var position = 10 * move.direction;

            yield return SpawnAvatarOnGround(position, fallDuration);

            movableAvatar.SetIntendedMovement(move.input);

            for (int i = 0; i < 3; i++) {
                yield return new WaitForFixedUpdate();
            }

            var velocity = new Vector2(movableAvatar.GetVelocity().x, movableAvatar.GetVelocity().z);
            Assert.AreEqual(Assets.avatarSpeed, velocity.magnitude, $"With input {move.input}, Avatar's reach their maximum speed of {Assets.avatarSpeed}m/s!");

            DestroyGameObject(avatar);
            DestroyGameObject(ground);
            yield return new WaitForFixedUpdate();
        }
        #endregion
    }
}