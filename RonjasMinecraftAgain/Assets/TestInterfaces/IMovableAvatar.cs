using UnityEngine;

namespace TestInterfaces {
    public interface IMovableAvatar {
        /// <summary>
        /// Sets the avatar's walk direction.
        /// </summary>
        /// <param name="intendedMovement">movement, clamped between -1 and 1</param>
        void SetIntendedMovement(Vector2 intendedMovement);

        /// <summary>
        /// If the avatar is grounded, set its vertical velocity to a positive value.
        /// </summary>
        void Jump();

        /// <summary>
        /// Retrieve the current velocity of the avatar in world space coordinates.
        /// </summary>
        /// <returns>current velocity, in m/s</returns>
        Vector3 GetVelocity();
    }
}