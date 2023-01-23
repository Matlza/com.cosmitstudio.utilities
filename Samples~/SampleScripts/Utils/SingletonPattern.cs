using UnityEngine;

namespace CosmitStudio {
    public class SingletonPattern : MonoBehaviour {
        public static SingletonPattern Instance;

        void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
