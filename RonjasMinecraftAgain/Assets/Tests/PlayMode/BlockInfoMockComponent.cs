using TestInterfaces;
using UnityEngine;

namespace Tests.PlayMode {
    [AddComponentMenu("")]
    public class BlockInfoMockComponent : MonoBehaviour, IBlockInfo {
        public BlockId blockId => throw new System.NotImplementedException();

        public Sprite sprite { get; set; }
    }
}
