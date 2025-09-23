using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]private GameObject pause;

    // Update is called once per frame
    void Awake()
    {
        Time.timeScale = 1.0f;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SetPause(true);
        }
    }

    public void SetPause(bool paused)
    {
        if (paused == true)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        pause.SetActive(paused);
    }
}
