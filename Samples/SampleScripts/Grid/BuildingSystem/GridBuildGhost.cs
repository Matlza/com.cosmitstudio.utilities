using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CosmitStudio.GridSystem.BuildingSystem {
    public class GridBuildGhost : MonoBehaviour {
        Transform _visual;
        PlacedObjectTypeSO _placedObjectTypeSO;

        void Start() {
            RefreshVisual();

            GridBuildingSystem.Instance.OnSelectedChange += RefreshVisual;

            _visual.gameObject.SetActive(false);
        }

        void OnDisable() {
            GridBuildingSystem.Instance.OnSelectedChange -= RefreshVisual;
        }

        void LateUpdate() {
            Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldPositionSnappedPosition();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
            _visual.gameObject.SetActive(GridBuildingSystem.Instance.MouseInGrid());
        }

        void RefreshVisual() {
            if (_visual != null) {
                Destroy(_visual.gameObject);
                _visual = null;
            }

            PlacedObjectTypeSO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();

            if (placedObjectTypeSO != null) {
                _visual = Instantiate(placedObjectTypeSO.Visual, Vector3.zero, Quaternion.identity);
                _visual.parent = transform;
                _visual.localPosition = Vector3.zero;
                _visual.localEulerAngles = Vector3.zero;
            }
        }
    }
}
