namespace ThisPCG
{
    using UnityEngine;

    // Test script made with o1 to play with PCG based on Drunkard's Walk
    
    public class DWLevelGenerator : MonoBehaviour
    {
        [Header("Level Generation Settings")] 
        [SerializeField] private int width = 50;
        [SerializeField] private int height = 50;
        [SerializeField] private int steps = 500;
        [SerializeField] private int walkerCount = 3;
        [SerializeField] private float changeDirChance = 0.2f;
        [SerializeField] private float specialTileChance = 0.1f;

        [Header("Prefabs")] 
        [SerializeField] private GameObject floorPrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject specialTilePrefab; // For testing
        [SerializeField] private GameObject playerPrefab;

        private int[,] _map;
        private Vector2Int[] _walkerPositions;
        private Vector2Int[] _walkerDirections;

        private void Start()
        {
            _map = new int[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    _map[x, y] = 0;
            }

            _walkerPositions = new Vector2Int[walkerCount];
            _walkerDirections = new Vector2Int[walkerCount];

            for (int i = 0; i < walkerCount; i++)
            {
                _walkerPositions[i] = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
                _walkerDirections[i] = RandomDirection();
            }

            // Generate the map data
            GenerateLevel();

            // Instantiate prefabs in scene
            RenderLevel();
        }

        private void GenerateLevel()
        {
            for (int i = 0; i < steps; i++)
            {
                for (int w = 0; w < walkerCount; w++)
                {
                    _map[_walkerPositions[w].x, _walkerPositions[w].y] = 1;

                    if (Random.value < changeDirChance)
                        _walkerDirections[w] = RandomDirection();

                    _walkerPositions[w] += _walkerDirections[w];
                    _walkerPositions[w].x = Mathf.Clamp(_walkerPositions[w].x, 0, width - 1);
                    _walkerPositions[w].y = Mathf.Clamp(_walkerPositions[w].y, 0, height - 1);
                }
            }

            // OPTIONAL: Convert some floors into "special" tiles
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_map[x, y] == 1 && Random.value < specialTileChance)
                    {
                        _map[x, y] = 2;
                    }
                    
                    // Spawn player at random floor tile
                    if (_map[x, y] == 1 && Random.value < 0.01f) // this should be moved
                    {
                        Instantiate(playerPrefab, new Vector3(x, 2, y), Quaternion.identity);
                        return;
                    }
                }
            }
        }

        private void RenderLevel()
        {
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