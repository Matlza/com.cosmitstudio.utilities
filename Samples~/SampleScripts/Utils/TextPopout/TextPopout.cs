using UnityEngine;
using TMPro;

namespace CosmitStudio.Utils {
    public class TextPopout : MonoBehaviour {
        public static TextPopout Create(string text, Transform prefab, Vector3 position, Quaternion rotation, Color color, bool isStatic) {
            Transform damagePopoutTransform = Instantiate(prefab, position, rotation);

            TextPopout damagePopout = damagePopoutTransform.GetComponent<TextPopout>();

            damagePopout.Setup(text, color, isStatic);

            return damagePopout;
        }

        TextMeshPro _textMesh;
        bool _static;

        private void Awake() {
            _textMesh = transform.GetComponentInChildren<TextMeshPro>();
        }

        public void Setup(string text, Color color, bool isStatic) {
            _textMesh.SetText(text);
            _textMesh.color = color;
            _static = isStatic;

            if (!isStatic) Destroy(gameObject, 1.0f);
        }

        private void Update() {
            if (_static) return;

            float moveYSpeed = 3f;
            transform.position += new Vector3(0, moveYSpeed, 0) * Time.deltaTime;

            transform.LookAt(Camera.main.transform);

            Quaternion rotation = transform.rotation;
            rotation.x = 0.0f;
            rotation.z = 0.0f;

            transform.rotation = rotation;
        }
    }
}
