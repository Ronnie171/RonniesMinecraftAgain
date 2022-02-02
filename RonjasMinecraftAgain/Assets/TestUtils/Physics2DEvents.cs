using System;
using UnityEngine;

namespace TestUtils {
    public class Physics2DEvents : MonoBehaviour {
        public event Action<Collision2D> onCollisionEnter;
        public event Action<Collision2D> onCollisionStay;
        public event Action<Collision2D> onCollisionExit;

        protected void OnCollisionEnter2D(Collision2D collision) {
            onCollisionEnter?.Invoke(collision);
        }

        protected void OnCollisionStay2D(Collision2D collision) {
            onCollisionStay?.Invoke(collision);
        }

        protected void OnCollisionExit2D(Collision2D collision) {
            onCollisionExit?.Invoke(collision);
        }
    }
}