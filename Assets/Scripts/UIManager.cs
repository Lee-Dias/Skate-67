using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        score.text = "" + gameManager.score;
    }
}
