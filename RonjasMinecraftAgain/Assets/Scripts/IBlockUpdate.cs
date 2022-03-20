
using UnityEngine;

namespace TestInterfaces {
    public interface IBlockUpdate {
        void UpdateBlock(ILevel level, Vector3Int position);
    }

}