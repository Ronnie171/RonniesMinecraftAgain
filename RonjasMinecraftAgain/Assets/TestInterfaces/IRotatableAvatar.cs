using UnityEngine;

namespace TestInterfaces {
    public interface IRotatableAvatar {
        /// <summary>
        /// Retrieves the current rotation of the avatar's body, in local space.
        /// </summary>
        /// <returns>The body's local rotation.</returns>
        Quaternion GetBodyRotation();

        /// <summary>
        /// Retrieves the current rotation of the avatar's head, in local space.
        /// </summary>
        /// <returns>The head's local rotation.</returns>
        Quaternion GetHeadRotation();

        /// <summary>
        /// Rotates the avatar's body by <paramref name="delta"/>.x and the avatar's head by <paramref name="delta"/>.y
        /// </summary>
        /// <param name="delta"></param>
        void RotateBy(Vector2 delta);
    }
}