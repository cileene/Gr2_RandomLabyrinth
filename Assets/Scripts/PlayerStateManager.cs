using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerStates.IPlayerStates currentState;

    public void SetState(PlayerStates.IPlayerStates newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + newState.GetType().Name);
    }

    private void Start()
    {
        SetState(new PlayerStates.NormalState());
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollision(this, collision);
    }
}