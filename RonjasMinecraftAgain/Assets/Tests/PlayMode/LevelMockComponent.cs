using System;
using TestInterfaces;
using UnityEngine;

namespace Tests.PlayMode {
    [AddComponentMenu("")]
    public class LevelMockComponent : MonoBehaviour, ILevel {
        public LevelMock level = new LevelMock();

        public Vector3Int size => level.size;

        public event Action<Vector3Int, GameObject> onLevelChange;

        protected void Awake() {
            level.onLevelChange += (position, instance) => onLevelChange?.Invoke(position, instance);
        }

        public bool TryGetBlockInstance(Vector3Int position, out GameObject instance) {
            return level.TryGetBlockInstance(position, out instance);
        }

        public bool TrySetBlock(Vector3Int position, GameObject prefab) {
            return level.TrySetBlock(position, prefab);
        }

        public bool TrySwapBlocks(Vector3Int positionA, Vector3Int positionB) {
            return level.TrySwapBlocks(positionA, positionB);
        }

        public Vector3Int WorldSpaceToBlockSpace(Vector3 position) {
            return level.WorldSpaceToBlockSpace(position);
        }
    }
}
