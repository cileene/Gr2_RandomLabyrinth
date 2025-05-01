using UnityEngine;

namespace TurtleTileIINT
{
    public class TurtleTileCollision : MonoBehaviour 
    {
        private TurtleController _turtleController;

        private void Start() 
        {
            _turtleController = GetComponent<TurtleController>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            switch (other.tag) {
                case "Grass":
                    _turtleController.SetStrategy(new GrassMovement(), "Grass");
                    break;
                case "Sand":
                    _turtleController.SetStrategy(new SandMovement(), "Sand");
                    break;
                case "Water":
                    _turtleController.SetStrategy(new WaterMovement(), "Water");
                    break;
                case "Fire":
                    _turtleController.SetStrategy(new FireMovement(), "Fire");
                    break;
            }
        }
    }
}