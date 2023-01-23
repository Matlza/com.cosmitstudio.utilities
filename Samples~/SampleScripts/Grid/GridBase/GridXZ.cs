using System;
using TMPro;
using UnityEngine;

namespace CosmitStudio.GridSystem {
    public class GridXZ<TGridObject> {

        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs {
            public int x;
            public int z;
        }

        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originPosition;
        private TGridObject[,] _gridArray;

        public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject) {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _gridArray = new TGridObject[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++) {
                for (int z = 0; z < _gridArray.GetLength(1); z++) {
                    _gridArray[x, z] = createGridObject(this, x, z);
                }
            }

            bool showDebug = false;
            if (showDebug) {
                TextMeshPro[,] debugTextArray = new TextMeshPro[width, height];

                for (int x = 0; x < _gridArray.GetLength(0); x++) {
                    for (int z = 0; z < _gridArray.GetLength(1); z++) {
                        debugTextArray[x, z] = Utils.Helpers.CreateWorldText(_gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + Vector3.up * 0.5f + Vector3.right * 0.5f, 2, Color.white, 10);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                    debugTextArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z]?.ToString();
                };
            }
        }

        public int GetWidth() {
            return _width;
        }

        public int GetHeight() {
            return _height;
        }

        public float GetCellSize() {
            return _cellSize;
        }

        public Vector3 GetWorldPosition(int x, int z) {
            return new Vector3(x, 0, z) * _cellSize + _originPosition;
        }

        public void GetXZ(Vector3 worldPosition, out int x, out int z) {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
        }

        public void SetGridObject(int x, int z, TGridObject value) {
            if (x >= 0 && z >= 0 && x < _width && z < _height) {
                _gridArray[x, z] = value;
                TriggerGridObjectChanged(x, z);
            }
        }

        public void TriggerGridObjectChanged(int x, int z) {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value) {
            GetXZ(worldPosition, out int x, out int z);
            SetGridObject(x, z, value);
        }

        public TGridObject GetGridObject(int x, int z) {
            if (x >= 0 && z >= 0 && x < _width && z < _height) {
                return _gridArray[x, z];
            }
            else {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition) {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetGridObject(x, z);
        }

        public Vector2Int ValidateGridPosition(Vector2Int gridPosition) {
            return new Vector2Int(
                Mathf.Clamp(gridPosition.x, 0, _width - 1),
                Mathf.Clamp(gridPosition.y, 0, _height - 1)
            );
        }

        public bool IsValidGridPosition(Vector2Int gridPosition) {
            int x = gridPosition.x;
            int z = gridPosition.y;

            if (x >= 0 && z >= 0 && x < _width && z < _height) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool IsValidGridPositionWithPadding(Vector2Int gridPosition) {
            Vector2Int padding = new Vector2Int(2, 2);
            int x = gridPosition.x;
            int z = gridPosition.y;

            if (x >= padding.x && z >= padding.y && x < _width - padding.x && z < _height - padding.y) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
