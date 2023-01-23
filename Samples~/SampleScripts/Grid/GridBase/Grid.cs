using System;
using TMPro;
using UnityEngine;

namespace CosmitStudio.GridSystem {
    public class Grid<TGridObject> {

        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs {
            public int x;
            public int y;
        }

        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originPosition;
        private TGridObject[,] _gridArray;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _gridArray = new TGridObject[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++) {
                for (int y = 0; y < _gridArray.GetLength(1); y++) {
                    _gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            bool showDebug = false;
            if (showDebug) {
                TextMeshPro[,] debugTextArray = new TextMeshPro[width, height];

                for (int x = 0; x < _gridArray.GetLength(0); x++) {
                    for (int y = 0; y < _gridArray.GetLength(1); y++) {
                        debugTextArray[x, y] = Utils.Helpers.CreateWorldText(_gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + Vector3.up * _cellSize * 0.5f + Vector3.right * _cellSize * 0.5f, 80, Color.white, 10);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                    debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray[eventArgs.x, eventArgs.y]?.ToString();
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

        public Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y) {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value) {
            if (x >= 0 && y >= 0 && x < _width && y < _height) {
                _gridArray[x, y] = value;
                TriggerGridObjectChanged(x, y);
            }
        }

        public void TriggerGridObjectChanged(int x, int y) {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value) {
            GetXY(worldPosition, out int x, out int y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y) {
            if (x >= 0 && y >= 0 && x < _width && y < _height) {
                return _gridArray[x, y];
            }
            else {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

        public bool IsValidGridPosition(Vector2Int gridPosition) {
            int x = gridPosition.x;
            int y = gridPosition.y;

            if (x >= 0 && y >= 0 && x < _width && y < _height) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
