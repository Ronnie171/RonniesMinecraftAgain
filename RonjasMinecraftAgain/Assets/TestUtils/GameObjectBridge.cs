using System.Linq;
using UnityEngine;

namespace TestUtils {
    public class GameObjectBridge {
        public Vector3 position {
            get => transform.position;
            set => transform.position = value;
        }
        public Vector3 scale {
            get => transform.localScale;
            set => transform.localScale = value;
        }
        public string tag {
            get => gameObject.tag;
            set => gameObject.tag = value;
        }
        public Transform transform {
            get;
            private set;
        }
        public readonly GameObject gameObject;
        public readonly Physics2DEvents physics2D;
        public readonly Physics3DEvents physics3D;
        public GameObjectBridge(GameObject gameObject, bool isInstance = false) {
            this.gameObject = gameObject;
            transform = FindComponent<Transform>();
            if (isInstance) {
                physics2D = gameObject.AddComponent<Physics2DEvents>();
                physics3D = gameObject.AddComponent<Physics3DEvents>();
            }
        }
        public Rigidbody rigidbody => FindComponent<Rigidbody>();
        public Rigidbody2D rigidbody2D => FindComponent<Rigidbody2D>();
        public Collider collider => FindComponent<Collider>();
        public Collider2D collider2D => FindComponent<Collider2D>();
        public Renderer renderer => FindComponent<Renderer>();

        protected FieldBridge<T> FindField<T>(string name) {
            return new FieldBridge<T>(gameObject, name);
        }
        protected MethodBridge<T> FindMethod<T>(string name, int parameterCount, string returnType) {
            return new MethodBridge<T>(gameObject, name, parameterCount, returnType);
        }
        protected MethodBridge FindMethod(string name, int parameterCount) {
            return new MethodBridge(gameObject, name, parameterCount);
        }
        protected T FindComponent<T>()
            where T : Component {
            return gameObject.GetComponent<T>();
        }
        protected T FindComponentInChildren<T>()
            where T : Component {
            return gameObject.GetComponentInChildren<T>();
        }
        protected T[] FindComponentsInChildren<T>()
            where T : Component {
            return gameObject.GetComponentsInChildren<T>();
        }
        protected T FindInterface<T>()
            where T : class {
            return gameObject
                .GetComponents<Component>()
                .OfType<T>()
                .FirstOrDefault();
        }
        public override string ToString() {
            return gameObject.name;
        }
    }
}