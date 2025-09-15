using UnityEngine;
using UnityEngine.UI;   // Needed for Button
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Button nextSceneButton; // assign in Inspector

    private void Awake()
    {
        // Add listener to button
        nextSceneButton.onClick.AddListener(LoadNextScene);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}
