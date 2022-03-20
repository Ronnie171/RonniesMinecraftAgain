using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestInterfaces;

public class BlockGravity : MonoBehaviour, IBlockUpdate {
    public void UpdateBlock(ILevel level, Vector3Int position) {
        throw new System.NotImplementedException();
    }
}
