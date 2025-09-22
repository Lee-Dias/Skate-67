using UnityEngine;
using  UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonsController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private PlayerSettings playerSettings; 
    void Start()
    {
        volumeSlider.value = playerSettings.volume;
    }
    public void ChangeScene(string nome)
    {
        SceneManager.LoadScene(nome);
    }
    public void ChangeVolume()
    {
        playerSettings.volume = volumeSlider.value;
    }
}
