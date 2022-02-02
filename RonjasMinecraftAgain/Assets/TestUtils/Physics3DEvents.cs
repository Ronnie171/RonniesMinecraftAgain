using System;
using UnityEngine;

namespace TestUtils {
    public class Physics3DEvents : MonoBehaviour {
        public event Action<Collision> onCollisionEnter;
        public event Action<Collision> onCollisionStay;
        public event Action<Collision> onCollisionExit;

        protected void OnCollisionEnter(Collision collision) {
            onCollisionEnter?.Invoke(collision);
        }

        protected void OnCollisionStay(Collision collision) {
            onCollisionStay?.Invoke(collision);
        }

        protected void OnCollisionExit(Collision collision) {
            onCollisionExit?.Invoke(collision);
        }
    }
}