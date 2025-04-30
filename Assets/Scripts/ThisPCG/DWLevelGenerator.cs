/*------------------------------------------------------------------------------
 DWLevelGenerator.cs
 Generates a grid‑based level at runtime using a multi‑agent Drunkard’s Walk
 algorithm and instantiates prefabs to visualize the result.

 Steps:
   1. Initialise N walkers at random positions.
   2. Move each walker for a defined number of steps, carving floor tiles.
   3. Optionally convert a subset of floor tiles to "special" tiles.
   4. Spawn the player on the first-floor tile that passes a random check.
   5. Iterate over the grid and instantiate the appropriate prefab per cell.

 ------------------------------------------------------------------------------*/
namespace ThisPCG
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Runtime procedural level generator based on the Drunkard's Walk algorithm.
    /// </summary>
    public class DWLevelGenerator : MonoBehaviour
    {
        // ---------- Generator configuration ----------
        [SerializeField] private int width = 50;   // Grid width (x‑axis tiles)
        [SerializeField] private int height = 50;  // Grid height (y‑axis tiles)
        [SerializeField] private int steps = 500;  // Total walker moves
        [SerializeField] private int walkerCount = 3; // Number of simultaneous walkers
        [SerializeField] private float changeDirChance = 0.2f; // Chance a walker turns at each step
        [SerializeField] private float fireTileChance = 0.1f; // Chance a carved floor becomes special

        // ---------- Prefab references ----------
        [SerializeField] private GameObject floorPrefab;     // Prefab for normal floor tiles
        [SerializeField] private GameObject wallPrefab;      // Prefab for wall tiles
        [SerializeField] private GameObject fireTilePrefab; // Prefab for special floor tiles
        [SerializeField] private GameObject playerPrefab;    // Player character prefab

        // --- Runtime data containers ---
        private int[,] _map;                           // 0 = wall, 1 = floor, 2 = special floor
        private Vector2Int[] _walkerPositions;         // Current grid position of each walker
        private Vector2Int[] _walkerDirections;        // Direction each walker is moving

        /// <summary>
        /// Unity entry point. Initializes data, generates the level, and renders it.
        /// </summary>
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

            // Carve the level layout
            GenerateLevel();
            Debug.Log(_map);

            // Instantiate prefabs in the scene
            RenderLevel();
            
            SaveMapToJson();
        }

        /// <summary>
        /// Executes the Drunkard’s Walk algorithm to carve floors and assign special tiles.
        /// </summary>
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
                    _walkerPositions[w].x = Mathf.Clamp(_walkerPositions[w].x, 0, width - 1);
                    _walkerPositions[w].y = Mathf.Clamp(_walkerPositions[w].y, 0, height - 1);
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

            if (floorTiles.Count > 0)
            {
                var spawn = floorTiles[Random.Range(0, floorTiles.Count)];
                Instantiate(playerPrefab, new Vector3(spawn.x, 2, spawn.y), Quaternion.identity);
            }
        }

        /// <summary>
        /// Converts the numeric map into actual GameObjects in the scene.
        /// </summary>
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
                        default:
                            // Fallback if map[x, y] is something unexpected
                            Debug.LogWarning($"Unexpected value in map at {x}, {y}");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a random cardinal direction as a Vector2Int.
        /// </summary>
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
      
    }
}