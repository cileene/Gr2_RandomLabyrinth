namespace ThisPCG
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
    
    // This class generates a level using a random walker / drunkards walk algorithm. 
    // Beware, baking metaphors incoming.
    
    public class LabyrinthGenerator : MonoBehaviour
    {
        // --- The knobs for the PCG oven ---
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
        private List<Vector2Int> _floorTiles;          // List of all floor tiles (1)
        
    
        // And now the 8 (9, maybe 10) steps to make/bake a PCG labyrinth:
        private void Start()
        {
            InitializeMap();        // 1.
            InitializeWalkers();    // 2.
            CarveFloor();           // 3.
            PromoteSpecialTiles();  // 4.
            ListFloorTiles();       // 5.
            MarkExit();             // 6.
            RenderLevel();          // 7.
            SpawnPlayer();          // 8.
            
                                    // (9.) Spawn the wall remover prefab to the scene
            Instantiate(wallRemoverPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            
            SaveMapToJson();        // (10.) Cakes done now cram it into the fridge and forget about it
        }

        private void InitializeMap() // 1. Initialize the map array
        {
            // Create a new 2D array for the map (think X/Y coordinate system)
            _map = new int[width, height];
            
            // Clear the map (set all cells to 0 = wall)
            System.Array.Clear(_map, 0, _map.Length);
        }
        
        private void InitializeWalkers() // 2. Create walkers at random positions with random directions
        {
            _walkerPositions = new Vector2Int[walkerCount];
            _walkerDirections = new Vector2Int[walkerCount];

            for (int i = 0; i < walkerCount; i++)
            {
                _walkerPositions[i] = new Vector2Int(Random.Range(0, width - 1), Random.Range(0, height - 1));
                _walkerDirections[i] = RandomDirection();
            }
        }
        
        private void CarveFloor() // 3. Move the walkers around the map, carving out a path
        {
            for (int i = 0; i < steps; i++)
            {
                for (int w = 0; w < walkerCount; w++)
                {
                    if (Random.value < changeDirChance) _walkerDirections[w] = RandomDirection();

                    _walkerPositions[w] += _walkerDirections[w];

                    // Clamp the walker positions to the map boundaries
                    _walkerPositions[w].x = Mathf.Clamp(_walkerPositions[w].x, 1, width - 2);
                    _walkerPositions[w].y = Mathf.Clamp(_walkerPositions[w].y, 1, height - 2);

                    _map[_walkerPositions[w].x, _walkerPositions[w].y] = 1;
                }
            }
        }
        
        private void PromoteSpecialTiles() // 4. Promote some floor tiles to special tiles
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_map[x, y] != 1) continue; // If not floor, move on
                    if (Random.value < fireTileChance) _map[x, y] = 2;
                    else if (Random.value < waterTileChance) _map[x, y] = 4;
                }
            }
        }
        
        private void ListFloorTiles() // 5. Create a list of all floor tiles (1) in the map
        {
            _floorTiles = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_map[x, y] == 1) _floorTiles.Add(new Vector2Int(x, y));
                }
            }
        }
        
        private void MarkExit() // 6. Randomly select a floor tile to mark as the exit
        {
            Vector2Int exitSpawn = _floorTiles[Random.Range(0, _floorTiles.Count)];
            _map[exitSpawn.x, exitSpawn.y] = 3;
            Debug.Log($"Exit spawned at: {exitSpawn}");
        }
        
        private void RenderLevel() // 7. Iterate over each cell and instantiate the correct prefab
        {
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
                            // Spawn fire 
                            Instantiate(fireTilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        case 3:
                            // Spawn exit
                            Instantiate(exitPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        case 4:
                            // Spawn water 
                            Instantiate(waterTilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        default:
                            // Fallback if there's an unexpected value in _map
                            Debug.LogWarning($"Unexpected value in map at {x}, {y}");
                            break;
                    }
                }
            }
        }
        
        private void SpawnPlayer() // 8. Randomly select a floor tile to spawn the player
        {
            Vector2Int playerSpawn = _floorTiles[Random.Range(0, _floorTiles.Count)];
            Instantiate(playerPrefab, new Vector3(playerSpawn.x, 2, playerSpawn.y), Quaternion.identity);
            Debug.Log($"Player spawned at: {playerSpawn}");
        }
        
        private Vector2Int RandomDirection() // Helper method to get a random direction
        {
            return Random.Range(0, 4) switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.down,
                2 => Vector2Int.left,
                _ => Vector2Int.right
            };
        }
      
        
        // And here is a bunch of logic to save the map data to a JSON file
        // We never use it, but it's here for future me
        private string _dataPath; // Path to save the JSON file
        
        private void Awake() // Set the location for the JSON file
        {
            _dataPath = Application.persistentDataPath + "/Player_Data/";
            Debug.Log(_dataPath);
        }
        
        private void SaveMapToJson() // Save the map data to a JSON file (and promptly forget about it)
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
        
        [System.Serializable]
        private class MapData // Class to hold the map data for JSON serialization
        {
            public int width;
            public int height;
            public List<int> tiles; // Squash the 2D array into a 1D list
        }
    }
}