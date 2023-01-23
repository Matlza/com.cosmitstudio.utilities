using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace CosmitStudio.Utils {
    public class Helpers {
        public static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public static bool IsPointerOverUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return true;
            }
            else {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }

        public static Color GetRandomColor() {
            return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        }

        public static void DestroyChildren(Transform parent) {
            foreach (Transform transform in parent)
                GameObject.Destroy(transform.gameObject);
        }

        public static void SetAllChildrenLayer(Transform parent, int layer) {
            parent.gameObject.layer = layer;
            foreach (Transform trans in parent) {
                SetAllChildrenLayer(trans, layer);
            }
        }

        public static void FindClosestPositionArrayIndex(ref int tracedColliderCount, RaycastHit[] tempBuffer, out int closestIndex, Collider self, float traceBias = 0.001f) {
            int tempIndex = tracedColliderCount - 1;
            float closestTrace = Mathf.Infinity;
            closestIndex = -1;

            while (tempIndex >= 0) {
                tempBuffer[tempIndex].distance -= traceBias;
                RaycastHit tempHit = tempBuffer[tempIndex];
                float traceLength = tempHit.distance;

                if (traceLength > 0.0f && tempHit.collider != self) {
                    if (traceLength < closestTrace) {
                        closestIndex = tempIndex;
                        closestTrace = traceLength;
                    }
                }
                else {
                    if (tempIndex < --tracedColliderCount) {
                        tempBuffer[tempIndex] = tempBuffer[tracedColliderCount];
                    }
                }

                tempIndex--;
            }
        }

        public static Quaternion ShortestRotation(Quaternion a, Quaternion b) {
            if (Quaternion.Dot(a, b) < 0) {
                return a * Quaternion.Inverse(Multiply(b, -1));
            }
            else return a * Quaternion.Inverse(b);
        }

        static Quaternion Multiply(Quaternion input, float scalar) {
            return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
        }

        public static TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, int sortingOrder = 0) {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, sortingOrder);
        }

        public static TextMeshPro CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, int sortingOrder) {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMeshPro));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            //textMeshPro.autoSizeTextContainer = true;
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color;
            textMeshPro.alignment = TextAlignmentOptions.Midline;
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMeshPro;
        }

        public static Vector3 GetMouseWorldPosition() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f)) {
                return hit.point;
            }
            else {
                return Vector3.zero;
            }
        }
    }
}
