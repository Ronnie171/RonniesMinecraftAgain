using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestInterfaces;
using System;

public class Level : MonoBehaviour, ILevel
   {

    [SerializeField]
    private Vector3Int m_size = new Vector3Int(256, 256, 256); // Maybe 32

    private GameObject[][][] map;


    protected void Awake() {

        map = new GameObject[size.x][][];
        for (int x = 0; x < size.x; x++) {
            map[x] = new GameObject[size.y][];
            for (int y = 0; y < size.y; y++) {
                map[x][y] = new GameObject[size.z];
            }
        }
       
    }

    protected void FixedUpdate() {

        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                for (int z = 0; z < size.y; z++) {
                    var instance = map[x][y][z];
                    if (instance) {
                    }
                }
            }
        }
    }




    //ILevel Anfang
    public Vector3Int size => m_size;

    public event Action<Vector3Int, GameObject> onLevelChange;

    public bool TryGetBlockInstance(Vector3Int position, out GameObject instance) {
      if (!IsWithinBounds(position)) {
            instance = null;
            return false;
        }

        instance = map[position.x][position.y][position.z];
        return true;
    }

    public bool TrySetBlock(Vector3Int position, GameObject prefab) {
        if (!IsWithinBounds(position)) {
            return false;
        }
        if (map[position.x][position.y][position.z]) {
            Destroy(map[position.x][position.y][position.z]);
            map[position.x][position.y][position.z] = null;
        }
        if (prefab) {
            map[position.x][position.y][position.z] = Instantiate(prefab, position, Quaternion.identity, transform);
        }

        onLevelChange?.Invoke(position, map[position.x][position.y][position.z]);
        return true;
    }

    public bool TrySwapBlocks(Vector3Int positionA, Vector3Int positionB) {
        if (!TryGetBlockInstance(positionA, out var blockA) || !TryGetBlockInstance(positionB, out var blockB)) {
            return false;
        }
        if (blockA) {
            blockA.transform.position = positionB;

        }
        if (blockB) {
            blockB.transform.position = positionA;
        }
        map[positionA.x][positionA.y][positionA.z] = blockB;
        map[positionB.x][positionB.y][positionB.z] = blockA;
        onLevelChange?.Invoke(positionA, blockA);
        onLevelChange?.Invoke(positionB, blockB);
        return true;
    }

    private bool IsWithinBounds(Vector3Int position) { //nicht in ILevel
        if (position.x < 0 || position.x >= size.x) {
            return false;
        }
        if (position.y < 0 || position.y >= size.y) {
            return false;
        }
        if (position.z < 0 || position.z >= size.z) {
            return false;
        }
        return true;
    }
        public Vector3Int WorldSpaceToBlockSpace(Vector3 position) {
        return new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
    }
//ILevel Ende

}
