using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace CosmitStudio.Utils {
    public class StaticReferences : MonoBehaviour {
        public static StaticReferences Singleton;
        InputSystemUIInputModule _inputSystemUIInputModule;

        public static Camera MainCamera { get; private set; }
        public static Transform TextPopoutPrefab { get; private set; }

        [SerializeField] Transform _textPopoutPrefab;

        public static Vector2 MousePosition { get { return Singleton._inputSystemUIInputModule.point.action.ReadValue<Vector2>(); } }

        void Awake() {
            if (Singleton != null && Singleton != this) {
                Destroy(gameObject);
            }

            Singleton = this;

            _inputSystemUIInputModule = FindObjectOfType<InputSystemUIInputModule>();

            MainCamera = Camera.main;
            TextPopoutPrefab = _textPopoutPrefab;
        }
    }
}
