using System;
using UnityEngine;

namespace TestInterfaces {
    public interface ILevel {
        /// <summary>
        /// An event that gets invoked whenever a block gets placed in the level (including <see cref="BlockId.Air"/>).
        /// </summary>
        event Action<Vector3Int, GameObject> onLevelChange;

        /// <summary>
        /// The width, height and depth of this level.
        /// </summary>
        Vector3Int size { get; }

        /// <summary>
        /// Looks up a block via its position.
        /// Fails if the position is out of bounds.
        /// The air block is returned as <see cref="null"/>.
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <param name="instance">The block at <paramref name="position"/>. If the call is unsuccessful, the value of this parameter is undefined.</param>
        /// <returns><see cref="true"/> if <paramref name="position"/> is in bounds, <see cref="false"/> otherwise.</returns>
        bool TryGetBlockInstance(Vector3Int position, out GameObject instance);

        /// <summary>
        /// Places a block in the level using <see cref="UnityEngine.Object.Instantiate"/>.
        /// Before placing, removes whatever block is currently at <paramref name="position"/> using <see cref="UnityEngine.Object.Destroy"/>.
        /// <see cref="null"/> is treated is as the air block.
        /// Fails if the position is out of bounds.
        /// If successful, invokes <see cref="onLevelChange"/>.
        /// </summary>
        /// <param name="position">The position to update</param>
        /// <param name="prefab">The block to spawn at <paramref name="position"/></param>
        /// <returns><see cref="true"/> if <paramref name="position"/> is in bounds, <see cref="false"/> otherwise</returns>
        bool TrySetBlock(Vector3Int position, GameObject prefab);

        /// <summary>
        /// Swaps the blocks between two locations.
        /// Does not use <see cref="UnityEngine.Object.Instantiate"/> or <see cref="UnityEngine.Object.Destroy"/>.
        /// Instead, moves the blocks via <see cref="Transform.position"/>
        /// Fails if either of the positions are out of bounds.
        /// Fails if both positions are identical.
        /// If successful, invokes <see cref="onLevelChange"/> on both locations.
        /// </summary>
        /// <param name="positionA">One of the positions to swap</param>
        /// <param name="positionB">One of the positions to swap</param>
        /// <returns><see cref="true"/> if the swap was successful</returns>
        bool TrySwapBlocks(Vector3Int positionA, Vector3Int positionB);

        /// <summary>
        /// Converts a world space coordinate to a coordinate inside this level by rounding all 3 components to their nearest integer.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Vector3Int WorldSpaceToBlockSpace(Vector3 position);
    }
}
