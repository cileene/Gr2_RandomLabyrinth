namespace TurtleTileIINT
{
    public interface IMovementStrategy 
    {
        void Move(TurtleController turtle);
    }

    public class GrassMovement : IMovementStrategy 
    {
        public void Move(TurtleController turtle) 
        {
            turtle.MoveWithSpeed(1.0f);
            //will be another movement
        }
    }

    public class SandMovement : IMovementStrategy 
    {
        public void Move(TurtleController turtle) 
        {
            turtle.MoveWithSpeed(0.5f);
            //will be another movement
        }
    }

    public class WaterMovement : IMovementStrategy 
    {
        public void Move(TurtleController turtle) 
        {
            turtle.MoveWithSpeed(0.3f);
            //will be another movement
        }
    }

    public class FireMovement : IMovementStrategy 
    {
        public void Move(TurtleController turtle)
        {
            turtle.MoveWithSpeed(1.5f);
            // will be another movement
        }
    }
}