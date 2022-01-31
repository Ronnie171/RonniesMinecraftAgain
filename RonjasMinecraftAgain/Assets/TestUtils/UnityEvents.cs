using System;
using UnityEngine;

namespace TestUtils {
    public class UnityEvents : MonoBehaviour {
        public event Action onDestroy;

        protected void OnDestroy() {
            onDestroy?.Invoke();
        }
    }
}
