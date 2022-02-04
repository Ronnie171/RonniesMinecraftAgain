using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestInterfaces {
    public interface IGameManager {
        /// <summary>
        /// Whether the game is currently running (<see cref="false"/>) or paused (<see cref="true"/>).
        /// Changes <see cref="Cursor.lockState"/> (<see cref="CursorLockMode.Locked"/> when running, <see cref="CursorLockMode.None"/> when paused).
        /// Changes <see cref="Time.timeScale"/> (<see cref="1"/> when running, <see cref="0"/> when paused).
        /// Changes <see cref="pauseMenu"/> (inactive when running, active when paused).
        /// </summary>
        bool isPaused { get; set; }

        /// <summary>
        /// The pause menu
        /// </summary>
        GameObject pauseMenu { get; set; }

        /// <summary>
        /// Reloads the currently active <see cref="Scene"/>.
        /// </summary>
        void RestartLevel();

        /// <summary>
        /// Stops the <see cref="Application"/>.
        /// </summary>
        void QuitGame();
    }
}
