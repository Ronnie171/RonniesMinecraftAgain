using UnityEngine;

namespace TestInterfaces {
    public interface IBlockInfo {
        /// <summary>
        /// Returns the ID of this block as per <see href="https://minecraft.fandom.com/wiki/Java_Edition_data_values/Classic"/>.
        /// </summary>
        BlockId blockId { get; }

        /// <summary>
        /// Returns an image of this block for use in UI contexts.
        /// </summary>
        Sprite sprite { get; }
    }
}
