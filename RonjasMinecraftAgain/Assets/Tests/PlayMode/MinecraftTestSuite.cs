using NUnit.Framework;
using System.Collections;
using TestInterfaces;
using TestUtils;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Tests.PlayMode {
    public class MinecraftTestSuite : TestSuite {
        protected GameObject avatar;
        protected Transform body => avatar.transform;
        protected Transform head => camera.transform;

        protected CharacterController character {
            get {
                CustomAssert.HasComponent<CharacterController>(avatar, out var component);
                return component;
            }
        }
        protected Canvas canvas {
            get {
                CustomAssert.HasComponentInChildren<Canvas>(avatar, out var component);
                return component;
            }
        }
        protected Camera camera {
            get {
                CustomAssert.HasComponentInChildren<Camera>(avatar, out var component);
                return component;
            }
        }

        protected IMovableAvatar movableAvatar {
            get {
                CustomAssert.HasComponent<IMovableAvatar>(avatar, out var component);
                return component;
            }
        }
        protected IRotatableAvatar rotatableAvatar {
            get {
                CustomAssert.HasComponent<IRotatableAvatar>(avatar, out var component);
                return component;
            }
        }
        protected IBuildingAvatar buildingAvatar {
            get {
                CustomAssert.HasComponent<IBuildingAvatar>(avatar, out var component);
                return component;
            }
        }
        protected ILevelAvatar levelAvatar {
            get {
                CustomAssert.HasComponent<ILevelAvatar>(avatar, out var component);
                return component;
            }
        }

        protected GameObject ground;

        protected GameObject levelObj;
        protected ILevel level {
            get {
                CustomAssert.HasComponent<ILevel>(levelObj, out var component);
                return component;
            }
        }

        public IEnumerator SpawnAvatarOnGround(Vector3 position, float spawnDuration) {
            var prefab = LoadPrefab(Assets.avatarPrefab);
            yield return new WaitForFixedUpdate();

            avatar = InstantiateGameObject(prefab, position + new Vector3(0, 2, 0), Quaternion.identity);

            yield return new WaitForFixedUpdate();
            Assert.IsFalse(character.isGrounded, $"1 frame after spawning in the void, Avatar's CharacterController should no longer be grounded!");

            ground = CreatePrimitive(PrimitiveType.Plane, "Ground");
            ground.transform.position = position;

            yield return WaitForLandingOnPlane(ground, spawnDuration);
        }

        public IEnumerator SpawnLevel() {
            var prefab = LoadPrefab(Assets.levelPrefab);
            yield return new WaitForFixedUpdate();

            levelObj = InstantiateGameObject(prefab, Vector3.zero, Quaternion.identity);

            yield return new WaitForFixedUpdate();
        }

        public IEnumerator WaitForLandingOnPlane(GameObject plane, float fallDuration) {
            yield return WaitFor(fallDuration, () => character.isGrounded);
            Assert.IsTrue(character.isGrounded, $"After waiting {fallDuration}s, avatar should have become grounded!");
            //Debug.Log($"{character.transform.position} = {plane.transform.position}: {new FloatEqualityComparer(character.skinWidth / 2).Equals(character.transform.position.y, plane.transform.position.y)}");
            Assert.That(
                character.transform.position.y,
                Is.EqualTo(plane.transform.position.y).Using(new FloatEqualityComparer(character.skinWidth * 1.1f)),
                $"After landing on a plane, Avatar's Y position should be equal to it!"
            );
            for (int i = 0; i < 3; i++) {
                yield return new WaitForFixedUpdate();
                Assert.IsTrue(character.isGrounded, $"Avatar should stay grounded!");
            }
        }

        public IEnumerator WaitForJump(float jumpDuration) {
            movableAvatar.Jump();

            yield return WaitFor(jumpDuration, () => !character.isGrounded);

            Assert.IsFalse(character.isGrounded, $"After invoking {nameof(IMovableAvatar.Jump)}, Avatar's CharacterController should no longer be grounded!");
        }

        protected Vector3Int RoundToInt(Vector3 position) {
            return new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
        }
    }
}
