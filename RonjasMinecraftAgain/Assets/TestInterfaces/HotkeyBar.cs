using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestInterfaces;




    public class HotkeyBar : MonoBehaviour, TestInterfaces.IHotkeyBar  {

        [SerializeField]
        private RectTransform selector;
        [SerializeField]
        private RectTransform previewContainer;
        [SerializeField]
        private GameObject[] m_blockPrefabs = new GameObject[9];

        public GameObject[] blockPrefabs { 
            get => m_blockPrefabs; 
            set => m_blockPrefabs = value; 
        }

        private int m_currentIndex;
        
        public int currentIndex {
        get => m_currentIndex;
        set {
            value = (value+m_blockPrefabs.Length) % m_blockPrefabs.Length;
            m_currentIndex = value;

            UpdateSelection();
        }
    }

        private ILevelAvatar m_avatar;
        public ILevelAvatar avatar {
            get => m_avatar;
            set => m_avatar = value;
        }

        
        protected void Start() {
            avatar = (ILevelAvatar)FindObjectOfType<Avatar>();
        }
        private void UpdateSelection() {
            var selectedPreview = previewContainer.GetChild(currentIndex);
            selector.SetParent(selectedPreview, false);
    }
}

 