using UnityEngine;
using UnityEngine.UI;   // Needed for Button
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject menu;
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button leaveGameButton;

    [Space(10)]
    [Header("Options")]
    [SerializeField] private GameObject options;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Button exitOptions;

    [Space(10)]
    [Header("Credits")]
    [SerializeField] private GameObject credits;
    [SerializeField] private Button creditsExit;
    private void Awake()
    {
        // Menu
        startButton.onClick.AddListener(LoadNextScene);
        creditsButton.onClick.AddListener(TurnCreditsOn);
        optionsButton.onClick.AddListener(TurnOptionsOn);


        //Options

        //Credits
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void TurnOptionsOn()
    {
        options.SetActive(true);
        TurnMenuOff();
    }
    private void TurnOptionsOff()
    {
        options.SetActive(false);
        TurnMenuOn();
    }
    private void TurnCreditsOn()
    {
        credits.SetActive(true);
        TurnMenuOff();
    }
    private void TurnCreditsOff()
    {
        credits.SetActive(false);
        TurnMenuOn();
    }
    private void TurnMenuOn()
    {
        menu.SetActive(true);
    }
    private void TurnMenuOff()
    {
        menu.SetActive(false);
    }

}
