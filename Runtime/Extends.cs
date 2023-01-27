using UnityEngine;

namespace CosmitStudio.Utils {
    public static class Extends {
        public static float Sqr(this float value) {
            return value * value;
        }

        public static Vector3 RemoveY(this Vector3 value) {
            return new Vector3(value.x, 0f, value.z);
        }

        public static Vector3 RemoveXZ(this Vector3 value){
            return new Vector3(0f, value.y, 0f);
        }
    }
}
