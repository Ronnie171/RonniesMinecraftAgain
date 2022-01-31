using System;
using TestInterfaces;
using UnityEngine;

namespace Tests.PlayMode {
    public class LevelMock : ILevel {
        public Vector3Int size => new Vector3Int(32, 32, 32);

        public event Action<Vector3Int, GameObject> onLevelChange;

        public TryGetBlockInstanceDelegate tryGetBlockInstance;
        public delegate bool TryGetBlockInstanceDelegate(Vector3Int position, out GameObject instance);
        public bool TryGetBlockInstance(Vector3Int position, out GameObject instance) {
            if (tryGetBlockInstance == null) {
                throw new NotImplementedException($"{nameof(TryGetBlockInstance)} is not supported by this implementation (it should not be necessary for this test).");
            }
            return tryGetBlockInstance(position, out instance);
        }

        public TrySetBlockDelegate trySetBlock;
        public delegate bool TrySetBlockDelegate(Vector3Int position, GameObject prefab);
        public bool TrySetBlock(Vector3Int position, GameObject prefab) {
            if (trySetBlock == null) {
                throw new NotImplementedException($"{nameof(TrySetBlock)} is not supported by this implementation (it should not be necessary for this test).");
            }
            return trySetBlock(position, prefab);
        }

        public TrySwapBlocksDelegate trySwapBlocks;
        public delegate bool TrySwapBlocksDelegate(Vector3Int positionA, Vector3Int positionB);
        public bool TrySwapBlocks(Vector3Int positionA, Vector3Int positionB) {
            if (trySwapBlocks == null) {
                throw new NotImplementedException($"{nameof(TrySwapBlocks)} is not supported by this implementation (it should not be necessary for this test).");
            }
            return trySwapBlocks(positionA, positionB);
        }

        public Vector3Int WorldSpaceToBlockSpace(Vector3 position) {
            return new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
        }
    }
}
