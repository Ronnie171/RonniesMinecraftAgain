namespace TestInterfaces {
    public interface IBuildingAvatar {
        /// <summary>
        /// Updates the maximum distance within which the avatar can build or destroy blocks.
        /// </summary>
        void SetReach(float distance);

        /// <summary>
        /// Retrieves the maximum distance within which the avatar can build or destroy blocks.
        /// </summary>
        float GetReach();

        /// <summary>
        /// If a block can be placed in front of the avatar, spawn it.
        /// </summary>
        void BuildBlock();

        /// <summary>
        /// If there is a block in front of the avatar, destroy it.
        /// </summary>
        void DestroyBlock();
    }
}