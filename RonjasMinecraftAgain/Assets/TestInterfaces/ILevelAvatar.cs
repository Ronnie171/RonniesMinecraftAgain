using UnityEngine;

namespace TestInterfaces {
    public interface ILevelAvatar {
        /// <summary>
        /// Updates the maximum distance within which the avatar can build or destroy blocks.
        /// </summary>
        void SetReach(float distance);

        /// <summary>
        /// Retrieves the maximum distance within which the avatar can build or destroy blocks.
        /// </summary>
        float GetReach();

        /// <summary>
        /// The level the avatar is currently in, to use with <see cref="BuildBlockInLevel"/> and <see cref="DestroyBlockInLevel"/>.
        /// </summary>
        ILevel level { get; set; }

        /// <summary>
        /// The prefab of the block that the avatar will place with <see cref="BuildBlockInLevel"/>.
        /// </summary>
        GameObject currentlySelectedBlockPrefab { get; set; }

        /// <summary>
        /// If a block can be placed in front of the avatar, spawn <see cref="currentlySelectedBlockPrefab"/> using <see cref="level"/>.
        /// </summary>
        void BuildBlockInLevel();

        /// <summary>
        /// If there is a block in front of the avatar, destroy it using <see cref="level"/>.
        /// </summary>
        void DestroyBlockInLevel();
    }
}