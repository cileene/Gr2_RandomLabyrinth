namespace ThisPCG
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
    
    public class DWLevelGenerator : MonoBehaviour
    {
        // ---------- Generator configuration ----------
        [SerializeField] private int width = 30;
        [SerializeField] private int height = 30;
        [SerializeField] private int steps = 63;
        [SerializeField] private int walkerCount = 10;
        [SerializeField] private float changeDirChance = 0.082f;
        [SerializeField] private float fireTileChance = 0f;

        // ---------- Prefab references ----------
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject fireTilePrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject exitPrefab;
        [SerializeField] private GameObject wallRemoverPrefab;

        // --- Runtime data containers ---
        private int[,] _map;                           // 0 = wall, 1 = floor, 2 = fire floor, 3 = exit
        private Vector2Int[] _walkerPositions;         // Current grid position of each walker
        private Vector2Int[] _walkerDirections;        // Direction each walker is moving
        
        private string _dataPath;
        
        [System.Serializable]
        public class MapData
        {
            public int width;
            public int height;
            public List<int> tiles; // Flattened 2D array into 1D list
        }
        
        void Awake()
        {
            _dataPath = Application.persistentDataPath + "/Player_Data/";
            Debug.Log(_dataPath);
        }
    
        private void Start()
        {
            // Initialize the map array
            _map = new int[width, height];
            // Clear the map (set all cells to 0 = wall)
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    _map[x, y] = 0;
            }

            // Create walkers at random positions with random directions
            _walkerPositions = new Vector2Int[walkerCount];
            _walkerDirections = new Vector2Int[walkerCount];

            for (int i = 0; i < walkerCount; i++)
            {
                _walkerPositions[i] = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
                _walkerDirections[i] = RandomDirection();
            }
            
            GenerateLevel();
            Debug.Log(_map);
            
            RenderLevel();
            
            SaveMapToJson();
        }
        
        private void GenerateLevel()
        {
            // --- PHASE 1: Move walkers and carve floors ---
            for (int i = 0; i < steps; i++)
            {
                for (int w = 0; w < walkerCount; w++)
                {
                    // Carve floor at current walker position
                    _map[_walkerPositions[w].x, _walkerPositions[w].y] = 1;

                    // Randomly change walker direction
                    if (Random.value < changeDirChance)
                    {
                        _walkerDirections[w] = RandomDirection();
                    }

                    _walkerPositions[w] += _walkerDirections[w];
                    
                    // Keep walker inside grid bounds
                    _walkerPositions[w].x = Mathf.Clamp(_walkerPositions[w].x, 1, width - 2);
                    _walkerPositions[w].y = Mathf.Clamp(_walkerPositions[w].y, 1, height - 2);
                }
            }

            // --- PHASE 2: Post‑process tiles (special tiles) ---
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Promote some floor tiles to special
                    if (_map[x, y] == 1 && Random.value < fireTileChance)
                    {
                        _map[x, y] = 2;
                    }
                }
            }

            // --- PHASE 3: Spawn the player on a random floor tile ---
            var floorTiles = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_map[x, y] == 1)
                        floorTiles.Add(new Vector2Int(x, y));
                }
            }

            if (floorTiles.Count <= 0) return;
            
            // Randomly select a floor tile to mark as the exit
            var exitSpawn = floorTiles[Random.Range(0, floorTiles.Count)];
            _map[exitSpawn.x, exitSpawn.y] = 3;
            
            // Randomly select a floor tile to spawn the player
            var playerSpawn = floorTiles[Random.Range(0, floorTiles.Count)];
            Instantiate(playerPrefab, new Vector3(playerSpawn.x, 2, playerSpawn.y), Quaternion.identity);
            
            // Instantiate the wall remover prefab
            Instantiate(wallRemoverPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                
        }
        
        private void RenderLevel()
        {
            // Iterate over each cell and instantiate the correct prefab
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (_map[x, y])
                    {
                        case 0:
                            // Spawn wall
                            Instantiate(wallPrefab, new Vector3(x, 0.5f, y), Quaternion.identity, transform);
                            break;
                        case 1:
                            // Spawn floor
                            Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        case 2:
                            // Spawn fire floor tile
                            Instantiate(fireTilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        case 3:
                            // Spawn exit
                            Instantiate(exitPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        default:
                            // Fallback if map[x, y] is something unexpected
                            Debug.LogWarning($"Unexpected value in map at {x}, {y}");
                            break;
                    }
                }
            }
        }
        
        private Vector2Int RandomDirection()
        {
            return Random.Range(0, 4) switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.down,
                2 => Vector2Int.left,
                _ => Vector2Int.right
            };
        }
      
        public void SaveMapToJson()
        {

            Directory.CreateDirectory(_dataPath);
            Debug.Log("New directory created!");

            MapData data = new MapData
            {
                width = width,
                height = height,
                tiles = new List<int>()
            };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    data.tiles.Add(_map[x, y]);
                }
            }

            string json = JsonUtility.ToJson(data, true);
            string filePath = Path.Combine(_dataPath, "map.json");
            File.WriteAllText(filePath, json);
            Debug.Log("Map saved to: " + filePath);
        }
    }
}
