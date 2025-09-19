using UnityEngine;
using UnityEngine.Audio;

public class ApplyVolume : MonoBehaviour
{
    [SerializeField]private PlayerSettings playerSettings;  
    [SerializeField]private AudioMixer audioMixer;
    void Awake()
    {
        float db = Mathf.Lerp(-40f, 20f, playerSettings.volume / 10f);
        if (db == -40)
        {
            db = -80;            
        }

        audioMixer.SetFloat("Master", db);


    }

    public void UpdateThePreferences()
    {
        float db = Mathf.Lerp(-40f, 20f, playerSettings.volume / 10f);
        if (db == -40)
        {
            db = -80;            
        }

        audioMixer.SetFloat("Master", db);
    }
}
