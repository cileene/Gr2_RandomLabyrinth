/*------------------------------------------------------------------------------
 DWLevelGenerator.cs
 Generates a grid‑based level at runtime using a multi‑agent Drunkard’s Walk
 algorithm and instantiates prefabs to visualise the result.

 Steps:
   1. Initialise N walkers at random positions.
   2. Move each walker for a defined number of steps, carving floor tiles.
   3. Optionally convert a subset of floor tiles to "special" tiles.
   4. Spawn the player on the first floor tile that passes a random check.
   5. Iterate over the grid and instantiate the appropriate prefab per cell.

 Attach this script to an empty GameObject in your scene.
 ------------------------------------------------------------------------------*/
namespace ThisPCG
{
    using UnityEngine;

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
        [SerializeField] private float specialTileChance = 0.1f; // Chance a carved floor becomes special

        // ---------- Prefab references ----------
        [SerializeField] private GameObject floorPrefab;     // Prefab for normal floor tiles
        [SerializeField] private GameObject wallPrefab;      // Prefab for wall tiles
        [SerializeField] private GameObject specialTilePrefab; // Prefab for special floor tiles
        [SerializeField] private GameObject playerPrefab;    // Player character prefab

        // --- Runtime data containers ---
        private int[,] _map;                           // 0 = wall, 1 = floor, 2 = special floor
        private Vector2Int[] _walkerPositions;         // Current grid position of each walker
        private Vector2Int[] _walkerDirections;        // Direction each walker is moving

        /// <summary>
        /// Unity entry point. Initialises data, generates the level, and renders it.
        /// </summary>
        private void Start()
        {
            // Initialise the map array
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

            // Instantiate prefabs in the scene
            RenderLevel();
        }

        /// <summary>
        /// Executes the Drunkard’s Walk algorithm to carve floors and assign special tiles.
        /// </summary>
        private void GenerateLevel()
        {
            // --- PHASE 1: Move walkers and carve floors ---
            for (int i = 0; i < steps; i++)
            {
                for (int w = 0; w < walkerCount; w++)
                {
                    // Carve floor at current walker position
                    _map[_walkerPositions[w].x, _walkerPositions[w].y] = 1;

                    // Randomly change walker direction
                    if (Random.value < changeDirChance)
                        _walkerDirections[w] = RandomDirection();

                    _walkerPositions[w] += _walkerDirections[w];
                    // Keep walker inside grid bounds
                    _walkerPositions[w].x = Mathf.Clamp(_walkerPositions[w].x, 0, width - 1);
                    _walkerPositions[w].y = Mathf.Clamp(_walkerPositions[w].y, 0, height - 1);
                }
            }

            // --- PHASE 2: Post‑process tiles (special tiles + player spawn) ---
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Promote some floor tiles to special
                    if (_map[x, y] == 1 && Random.value < specialTileChance)
                    {
                        _map[x, y] = 2;
                    }
                    
                    // Spawn the player on a random floor tile (temporary)
                    if (_map[x, y] == 1 && Random.value < 0.01f) // this should be moved
                    {
                        Instantiate(playerPrefab, new Vector3(x, 2, y), Quaternion.identity);
                        return;
                    }
                }
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
                            Instantiate(wallPrefab, new Vector3(x, 1, y), Quaternion.identity, transform);
                            break;
                        case 1:
                            // Spawn floor
                            Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                            break;
                        case 2:
                            // Spawn special floor tile (as a test)
                            Instantiate(specialTilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
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