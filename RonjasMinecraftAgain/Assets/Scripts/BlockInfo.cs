using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestInterfaces;

public class BlockInfo : MonoBehaviour, IBlockInfo {
    [SerializeField]

    private BlockId m_blockId = BlockId.Unknown;
    public BlockId blockId => m_blockId;


    [SerializeField]

    private Sprite m_sprite = default;
    public Sprite sprite => m_sprite;

} 
