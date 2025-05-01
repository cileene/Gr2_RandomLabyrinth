using UnityEngine;


public class TearDownWalls : MonoBehaviour
{
    [SerializeField] private float timerLength = 0.5f;
    
    private float _removeWallTimer;
    private GameObject[] _walls;

    
    private void Start()
    {
        _removeWallTimer = timerLength;
        
        // Set all wall prefabs to kinematic
        _walls = GameObject.FindGameObjectsWithTag("Wall");
    }
    
    private void Update()
    {
        //when removeWallTimer reaches 0, find a random wallPrefab in scene and set it to non-kinematic
        RemoveWall();
    }

    private void RemoveWall()
    {
        if (_removeWallTimer <= 0)
        {
            if (_walls.Length > 0)
            {
                int randomIndex = Random.Range(0, _walls.Length);
                GameObject randomWall = _walls[randomIndex];
                randomWall.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(randomWall, 5f); // Destroy the wall after 2 seconds
            }
            _removeWallTimer = 0.5f; // Reset the timer
        }
        else
        {
            _removeWallTimer -= Time.deltaTime;
        }
    }

}
