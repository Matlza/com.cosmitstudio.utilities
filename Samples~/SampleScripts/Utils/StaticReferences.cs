using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace CosmitStudio.Utils {
    public class StaticReferences : MonoBehaviour {
        public static StaticReferences Singleton;

#if ENABLE_INPUT_SYSTEM
        InputSystemUIInputModule _inputSystemUIInputModule;
#endif

        public static Camera MainCamera { get; private set; }
        public static Transform TextPopoutPrefab { get; private set; }

        [SerializeField] Transform _textPopoutPrefab;

#if ENABLE_INPUT_SYSTEM
        public static Vector2 MousePosition { get { return Singleton._inputSystemUIInputModule.point.action.ReadValue<Vector2>(); } }
#endif

        void Awake() {
            if (Singleton != null && Singleton != this) {
                Destroy(gameObject);
            }

            Singleton = this;
            
#if ENABLE_INPUT_SYSTEM
            _inputSystemUIInputModule = FindObjectOfType<InputSystemUIInputModule>();
#endif

            MainCamera = Camera.main;
            TextPopoutPrefab = _textPopoutPrefab;
        }
    }
}
