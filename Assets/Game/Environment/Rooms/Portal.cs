using UnityEngine;

public enum PortalType
{
    Spawn,
    Exit,
}

public class Portal : MonoBehaviour
{
    public PortalType PortalType = PortalType.Spawn;
    public LevelManager LevelManager;

    public void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && this.PortalType == PortalType.Exit)
        {
            this.LevelManager.LoadNextState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
        }
    }
}