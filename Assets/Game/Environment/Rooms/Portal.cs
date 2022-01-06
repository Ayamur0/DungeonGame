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

    private TransitionManager transitionManager;

    public void Start()
    {
        this.transitionManager = FindObjectOfType<TransitionManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && this.PortalType == PortalType.Exit)
        {
            if (this.transitionManager != null)
            {
                StartCoroutine(this.transitionManager.FadeToBlack(1f, delegate (int i) { this.LevelManager.LoadNextState(); }));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
        }
    }
}