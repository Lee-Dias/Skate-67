using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            gameManager.EndGame();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("One"))
        {
            gameManager.SetOneState(true);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Two"))
        {
            gameManager.SetTwoState(true);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Three"))
        {
            gameManager.SetThreeState(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("One"))
        {
            gameManager.SetOneState(false);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Two"))
        {
            gameManager.SetTwoState(false);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Three"))
        {
            gameManager.SetThreeState(false);
        }
    }
}
