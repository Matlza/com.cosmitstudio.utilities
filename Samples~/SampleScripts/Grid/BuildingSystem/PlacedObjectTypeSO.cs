using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CosmitStudio.GridSystem.BuildingSystem {
    [CreateAssetMenu(menuName = "CosmitStudio/PlacedObjectTypeSO")]
    public class PlacedObjectTypeSO : ScriptableObject {
        public static Dir GetNextDir(Dir dir) {
            switch (dir) {
                default:
                case Dir.Down: return Dir.Left;
                case Dir.Left: return Dir.Up;
                case Dir.Up: return Dir.Right;
                case Dir.Right: return Dir.Down;
            }
        }

        public enum Dir {
            Down,
            Left,
            Up,
            Right,
        }

        [SerializeField] string _nameString;
        [SerializeField] Transform _prefab;
        [SerializeField] Transform _visual;
        [SerializeField] int _width;
        [SerializeField] int _height;

        public string NameString => _nameString;
        public Transform Prefab => _prefab;
        public Transform Visual => _visual;
        public int Width => _width;
        public int Height => _height;

        public int GetRotationAngle(Dir dir) {
            switch (dir) {
                default:
                case Dir.Down: return 0;
                case Dir.Left: return -90;
                case Dir.Up: return -180;
                case Dir.Right: return -270;
            }
        }

        public Vector2Int GetRotationOffset(Dir dir) {
            switch (dir) {
                default:
                case Dir.Down: return new Vector2Int(0, 0);
                case Dir.Left: return new Vector2Int(0, _width);
                case Dir.Up: return new Vector2Int(_width, _height);
                case Dir.Right: return new Vector2Int(_height, 0);
            }
        }

        public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir) {
            List<Vector2Int> gridPositionList = new List<Vector2Int>();
            switch (dir) {
                case Dir.Down:
                case Dir.Up:
                for (int x = 0; x < _width; x++) {
                    for (int y = 0; y < _height; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
                case Dir.Left:
                case Dir.Right:
                for (int x = 0; x < _height; x++) {
                    for (int y = 0; y < _width; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            }
            return gridPositionList;
        }
    }
}
