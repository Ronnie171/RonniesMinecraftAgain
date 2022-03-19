using System;
using UnityEngine;
using UnityEngine.UI;

namespace TestInterfaces {
    public interface IHotkeyBar {
        /// <summary>
        /// The avatar controlled by this bar.
        /// </summary>
        ILevelAvatar avatar { get; set; }

        /// <summary>
        /// The blocks currently available in this hotkey bar.
        /// Upon setting, updates all referenced <see cref="Image"/> components using <see cref="IBlockInfo"/>.
        /// Upon setting, updates <see cref="ILevelAvatar.currentlySelectedBlockPrefab"/>.
        /// Throws a <see cref="NotSupportedException"/> if the new value of <see cref="blockPrefabs"/> has a different number of elements from the previous value.
        /// </summary>
        GameObject[] blockPrefabs { get; set; }

        /// <summary>
        /// The index for <see cref="blockPrefabs"/> that determines which block is currently selected by the avatar.
        /// Setting <see cref="currentIndex"/> to a value outside the valid indices of <see cref="blockPrefabs"/> wraps it around to a valid value (e.g., "-1" becomes "the index for the last block in <see cref="blockPrefabs"/>").
        /// Upon setting, updates <see cref="ILevelAvatar.currentlySelectedBlockPrefab"/>.
        /// </summary>
        int currentIndex { get; set; }
    }
}
