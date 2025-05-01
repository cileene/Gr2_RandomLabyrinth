using UnityEngine;
using System.Collections.Generic;


public class TearDownWalls : MonoBehaviour
{
    [SerializeField] private float timerLength = 0.5f;
    
    private float _removeWallTimer;
    private List<GameObject> _walls;

    
    private void Start()
    {
        _removeWallTimer = timerLength;
        
        // Set all wall prefabs to kinematic
        _walls = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wall"));
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
            if (_walls.Count > 0)
            {
                int randomIndex = Random.Range(0, _walls.Count);
                GameObject randomWall = _walls[randomIndex];
                Rigidbody rb = randomWall.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                Destroy(randomWall, 5f); // Destroy the wall after 2 seconds
                _walls.RemoveAt(randomIndex);
            }
            _removeWallTimer = timerLength; // Reset the timer
        }
        else
        {
            _removeWallTimer -= Time.deltaTime;
        }
    }
}
