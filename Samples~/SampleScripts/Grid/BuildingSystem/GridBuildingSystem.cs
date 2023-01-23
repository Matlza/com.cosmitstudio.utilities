using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CosmitStudio.GridSystem.BuildingSystem {
    public class GridBuildingSystem : MonoBehaviour {
        public static GridBuildingSystem Instance;

        Grid<GridObject> _grid;
        PlacedObjectTypeSO.Dir _dir = PlacedObjectTypeSO.Dir.Down;

        public Action OnSelectedChange;

        [SerializeField] Transform _gridCellVisual;
        [SerializeField] List<PlacedObjectTypeSO> _placedObjectTypeSOList = new List<PlacedObjectTypeSO>();
        PlacedObjectTypeSO _placedObjectTypeSO;


        [Header("Grid Size")]
        [SerializeField] int _gridWidth = 10;
        [SerializeField] int _gridHeight = 10;
        [SerializeField] float _cellSize = 1;

        void Awake() {
            Instance = this;

            _grid = new Grid<GridObject>(_gridWidth, _gridHeight, _cellSize, Vector3.zero, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

            for (int x = 0; x < _gridWidth; x++) {
                for (int y = 0; y < _gridHeight; y++) {
                    Instantiate(_gridCellVisual, _grid.GetWorldPosition(x, y), Quaternion.identity);
                }
            }

            _placedObjectTypeSO = _placedObjectTypeSOList[0];
        }

        public class GridObject {
            Grid<GridObject> _grid;
            int _x;
            int _y;
            PlacedObject _placedObject;

            public GridObject(Grid<GridObject> g, int x, int y) {
                _grid = g;
                _x = x;
                _y = y;
            }

            public void SetPlacedObject(PlacedObject placedObject) {
                _placedObject = placedObject;
                _grid.TriggerGridObjectChanged(_x, _y);
            }

            public PlacedObject GetPlacedObject() {
                return _placedObject;
            }

            public void ClearPlacedObject() {
                _placedObject = null;
                _grid.TriggerGridObjectChanged(_x, _y);
            }

            public bool CanBuild() {
                return _placedObject == null;
            }

            public override string ToString() {
                return _x + " , " + _y + "\n" + (_placedObject != null);
            }
        }

        void Update() {
            if (Mouse.current.leftButton.wasPressedThisFrame && Utils.Helpers.GetMouseWorldPosition() != Vector3.zero) {
                _grid.GetXY(Utils.Helpers.GetMouseWorldPosition(), out int x, out int y);

                List<Vector2Int> gridPositionList = _placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, y), _dir);

                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList) {
                    GridObject gridObject = _grid.GetGridObject(gridPosition.x, gridPosition.y);

                    if (gridObject == null || !gridObject.CanBuild()) {
                        canBuild = false;
                        break;
                    }
                }

                if (canBuild) {
                    Vector2Int rotationOffset = _placedObjectTypeSO.GetRotationOffset(_dir);
                    Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * _grid.GetCellSize();

                    PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, y), _dir, _placedObjectTypeSO);

                    foreach (Vector2Int gridPosition in gridPositionList) {
                        _grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }
                }
                else {
                    Utils.TextPopout.Create("Can't build here!", Utils.StaticReferences.TextPopoutPrefab, _grid.GetWorldPosition(x, y) + transform.forward * -2, Quaternion.identity, Color.cyan, false);
                }
            }

            if (Keyboard.current.rKey.wasReleasedThisFrame) {
                _dir = PlacedObjectTypeSO.GetNextDir(_dir);
            }

            if (Mouse.current.rightButton.wasPressedThisFrame && Utils.Helpers.GetMouseWorldPosition() != Vector3.zero) {
                GridObject gridObject = _grid.GetGridObject(Utils.Helpers.GetMouseWorldPosition());
                PlacedObject placedObject = gridObject.GetPlacedObject();
                if (placedObject != null) {
                    placedObject.DestroSelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();

                    foreach (Vector2Int gridPosition in gridPositionList) {
                        _grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }

            if (Keyboard.current.numpad1Key.wasReleasedThisFrame) { _placedObjectTypeSO = _placedObjectTypeSOList[0]; OnSelectedChange?.Invoke(); }
            if (Keyboard.current.numpad2Key.wasReleasedThisFrame) { _placedObjectTypeSO = _placedObjectTypeSOList[1]; OnSelectedChange?.Invoke(); }
            if (Keyboard.current.numpad3Key.wasReleasedThisFrame) { _placedObjectTypeSO = _placedObjectTypeSOList[2]; OnSelectedChange?.Invoke(); }
            if (Keyboard.current.numpad4Key.wasReleasedThisFrame) { _placedObjectTypeSO = _placedObjectTypeSOList[3]; OnSelectedChange?.Invoke(); }
            if (Keyboard.current.numpad5Key.wasReleasedThisFrame) { _placedObjectTypeSO = _placedObjectTypeSOList[4]; OnSelectedChange?.Invoke(); }
            if (Keyboard.current.numpad6Key.wasReleasedThisFrame) { _placedObjectTypeSO = _placedObjectTypeSOList[5]; OnSelectedChange?.Invoke(); }
            if (Keyboard.current.numpad7Key.wasReleasedThisFrame) { _placedObjectTypeSO = _placedObjectTypeSOList[6]; OnSelectedChange?.Invoke(); }
        }

        public Vector3 GetMouseWorldPositionSnappedPosition() {
            _grid.GetXY(Utils.Helpers.GetMouseWorldPosition(), out int x, out int y);
            Vector2Int rotationOffset = _placedObjectTypeSO.GetRotationOffset(_dir);
            Vector3 placedObjectWorldPosition = _grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, 0) * _grid.GetCellSize();
            return placedObjectWorldPosition;
        }

        public bool MouseInGrid() {
            _grid.GetXY(Utils.Helpers.GetMouseWorldPosition(), out int x, out int y);

            return (x >= 0 && x < _gridWidth) && (y >= 0 && y < _gridHeight);
        }

        public Quaternion GetPlacedObjectRotation() {
            return Quaternion.Euler(0, 0, _placedObjectTypeSO.GetRotationAngle(_dir));
        }

        public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
            return _placedObjectTypeSO;
        }
    }
}
