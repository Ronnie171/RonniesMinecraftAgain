using System;
using TestInterfaces;
using UnityEngine;

namespace Tests.PlayMode {
    public class AvatarMock : ILevelAvatar {
        public event Action onGetCurrentlySelectedBlockPrefab;
        public event Action<GameObject> onSetCurrentlySelectedBlockPrefab;
        public event Action onBuildBlockInLevel;
        public event Action onDestroyBlockInLevel;


        public ILevel level { get; set; }

        private GameObject m_currentlySelectedBlockPrefab;
        public GameObject currentlySelectedBlockPrefab {
            get {
                onGetCurrentlySelectedBlockPrefab?.Invoke();
                return m_currentlySelectedBlockPrefab;
            }
            set {
                onSetCurrentlySelectedBlockPrefab?.Invoke(value);
                m_currentlySelectedBlockPrefab = value;
            }
        }

        private float reach;
        public void SetReach(float distance) {
            reach = distance;
        }

        public float GetReach() {
            return reach;
        }

        public void BuildBlockInLevel() {
            onBuildBlockInLevel?.Invoke();
        }

        public void DestroyBlockInLevel() {
            onDestroyBlockInLevel?.Invoke();
        }
    }
}
