using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] public GameObject wetParticles;
    
    private PlayerStates.IPlayerStates currentState;

    public void SetState(PlayerStates.IPlayerStates newState) // Change the player state
    {
        currentState = newState;
        Debug.Log("State changed to: " + newState.GetType().Name);
    }

    private void Start() // Initialize the player state
    {
        SetState(new PlayerStates.NormalState());
    }

    private void OnCollisionEnter(Collision collision) // Handle collision events
    {
        currentState.HandleCollision(this, collision);
    }
    
    private void OnTriggerEnter(Collider other) // Handle trigger events
    {
        currentState.HandleTrigger(this, other);
    }
}