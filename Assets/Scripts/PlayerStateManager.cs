using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] public GameObject wetParticles;
    
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
        currentState.HandleCollision(this, collision);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        currentState.HandleTrigger(this, other);
    }
}