using UnityEngine;

public class TurtleEffectReceiver : MonoBehaviour
{

    public GameObject turtle;
    public GameObject smokeObject;
    public GameObject soundManager;

    private bool isLifted = false;

    public void EnableSmoke()
    {
        smokeObject.SetActive(true);
        smokeObject.transform.SetParent(turtle.transform);
        smokeObject.transform.localPosition = new Vector3(0, 1f, 0);
    }

    public void DisableSmoke()
    {
        if(smokeObject != null)
            smokeObject.SetActive(false);
    }

    public void EnableSneezing()
    {
        if(soundManager != null)
            soundManager.SetActive(true);
    }

    public void DisableSneezing()
    {
        if(soundManager != null)
            soundManager.SetActive(false);
    }

    public void StartBubblePickup()
    {
        if (isLifted) return;

        isLifted = true;
        transform.position += Vector3.up * 2;
        GetComponent<Rigidbody>().isKinematic = true;
        Invoke(nameof(EndBubblePickup), 5f);
    }

    private void EndBubblePickup()
    {
        transform.position += Vector3.down * 2;
        GetComponent<Rigidbody>().isKinematic = false;
        isLifted = false;
    }
}

