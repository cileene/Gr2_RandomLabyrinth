namespace ThisPCG
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
    
    // This class generates a level using a random walker / drunkards walk algorithm. 
    // Beware, baking metaphors ahead.
    public class DWLevelGenerator : MonoBehaviour
    {
        // --- The knobs and levers for the PCG oven ---
        [SerializeField] private int width = 35;
        [SerializeField] private int height = 35;
        [SerializeField] private int steps = 63;
        [SerializeField] private int walkerCount = 10;
        [SerializeField] private float changeDirChance = 0.08f;
        [SerializeField] private float fireTileChance = 0.03f;
        [SerializeField] private float waterTileChance = 0.1f;

        // --- The ingredients for the PCG oven ---
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject fireTilePrefab;
        [SerializeField] private GameObject waterTilePrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject exitPrefab;
        [SerializeField] private GameObject wallRemoverPrefab;

        // --- The internal workings of the PCG oven ---
        private int[,] _map;                           // 0 = wall, 1 = floor, 2 = fire, 3 = exit, 4 = water
        private Vector2Int[] _walkerPositions;         // Current grid position of each walker
        private Vector2Int[] _walkerDirections;        // Direction each walker is moving
        
    
        // And now the 6 (7, maybe 8) steps to make/bake a PCG labyrinth:
        private void Start()
        {
            InitializeMap();        // 1.
            InitializeWalkers();    // 2.
            CarveFloor();           // 3.
            PromoteSpecialTiles();  // 4.
            RenderLevel();          // 5.
            SpawnPlayerAndExit();   // 6.
            
                                    // (7.) Spawn the wall remover prefab to the scene
            Instantiate(wallRemoverPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            
            SaveMapToJson();        // (8.)
        }

        private void InitializeMap()
        {
            // Initialize the map array
            _map = new int[width, height];
            // Clear the map (set all cells to 0 = wall)
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    _map[x, y] = 0;
            }
        }
        
        private void InitializeWalkers()
        {
            // Create walkers at random positions with random directions
            _walkerPositions = new Vector2Int[walkerCount];
            _walkerDirections = new Vector2Int[walkerCount];

            for (int i = 0; i < walkerCount; i++)
            {
                _walkerPositions[i] = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
                _walkerDirections[i] = RandomDirection();
            }
        }
        
        // Do the handling of the player and exit spawn diverge from the rest of the logic? Yes, yes it does.
        // But it worked and we ran out of time. 
        private void SpawnPlayerAndExit()
        {
            // Go through the _map array and find all floor tiles (1)
            List<Vector2Int> floorTiles = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_map[x, y] == 1)
                        floorTiles.Add(new Vector2Int(x, y));
                }
            }
            
            // Randomly select a floor tile to mark as the exit
            Vector2Int exitSpawn = floorTiles[Random.Range(0, floorTiles.Count)];
            _map[exitSpawn.x, exitSpawn.y] = 3;
            
            // Randomly select a floor tile to spawn the player
            Vector2Int playerSpawn = floorTiles[Random.Range(0, floorTiles.Count)];
            Instantiate(playerPrefab, new Vector3(playerSpawn.x, 2, playerSpawn.y), Quaternion.identity);
        }

        private void PromoteSpecialTiles()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Promote some floor tiles to special tiles
                    if (_map[x, y] == 1 && Random.value < fireTileChance)
                    {
                        _map[x, y] = 2;
                    }
                    else if (_map[x, y] == 1 && Random.value < waterTileChance)
                    {
                        _map[x, y] = 4;
                    }
                }
            }
        }

        private void CarveFloor()
        {
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

                    // Move the walker in the new direction
                    _walkerPositions[w] += _walkerDirections[w];
                    
                    // Keep walker inside xy grid bounds
                    _walkerPositions[w].x = Mathf.Clamp(_walkerPositions[w].x, 1, width - 2);
                    _walkerPositions[w].y = Mathf.Clamp(_walkerPositions[w].y, 1, height - 2);
                }
            }
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
                            // Spawn exit hole
                            Instantiate(exitPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        case 4:
                            // Spawn water tile
                            Instantiate(waterTilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        default:
                            // Fallback if map[x, y] is something unexpected
                            Debug.LogWarning($"Unexpected value in map at {x}, {y}");
                            break;
                    }
                }
            }
        }
        
        // Helper method to get a random direction
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
      
        
        // And here is a bunch of code to save the map data to a JSON file
        // We never use it, but it's here for future me
        private string _dataPath; // Path to save the JSON file
        
        private void Awake()
        {
            // Set the location for the JSON file
            _dataPath = Application.persistentDataPath + "/Player_Data/";
            Debug.Log(_dataPath);
        }
        
        // Save the map data to a JSON file (and forget about it)
        private void SaveMapToJson()
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
        
        // TODO: Add a method to load the map from the JSON file
        
        // Class to hold the map data for JSON serialization
        [System.Serializable]
        private class MapData
        {
            public int width;
            public int height;
            public List<int> tiles; // Squash the 2D array into a 1D list
        }
    }
}
