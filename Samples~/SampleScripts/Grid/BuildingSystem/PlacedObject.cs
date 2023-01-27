using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CosmitStudio.GridSystem.BuildingSystem {
    public class PlacedObject : MonoBehaviour {
        public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO) {
            Transform placedObjectTransform = Instantiate(placedObjectTypeSO.Prefab, worldPosition, Quaternion.Euler(0, 0, placedObjectTypeSO.GetRotationAngle(dir)));

            PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

            placedObject._placedObjectTypeSO = placedObjectTypeSO;
            placedObject._origin = origin;
            placedObject._dir = dir;

            return placedObject;
        }

        PlacedObjectTypeSO _placedObjectTypeSO;
        Vector2Int _origin;
        PlacedObjectTypeSO.Dir _dir;

        public List<Vector2Int> GetGridPositionList() {
            return _placedObjectTypeSO.GetGridPositionList(_origin, _dir);
        }

        public void DestroSelf() {
            Destroy(gameObject);
        }
    }
}
