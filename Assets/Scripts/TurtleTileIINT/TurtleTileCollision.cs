using UnityEngine;

namespace TurtleTileIINT
{
    public class TurtleTileCollision : MonoBehaviour 
    {
        private TurtleController turtleController;

        private void Start() 
        {
            turtleController = GetComponent<TurtleController>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            switch (other.tag) {
                case "Grass":
                    turtleController.SetStrategy(new GrassMovement(), "Grass");
                    break;
                case "Sand":
                    turtleController.SetStrategy(new SandMovement(), "Sand");
                    break;
                case "Water":
                    turtleController.SetStrategy(new WaterMovement(), "Water");
                    break;
                case "Fire":
                    turtleController.SetStrategy(new FireMovement(), "Fire");
                    break;
            }
        }
    }
}