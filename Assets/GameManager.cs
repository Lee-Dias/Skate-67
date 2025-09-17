using System.Collections;
using UnityEngine;
using  UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int score{ get; private set;}

    private bool one;
    private bool two;
    private bool three;
    
    [SerializeField]private int bad = 0 ;
    [SerializeField]private int ok = 100 ;
    [SerializeField]private int good = 250 ;
    [SerializeField]private int great = 500;
    [SerializeField]private int Perfect = 1000;

    public void AddPointsToScore(float mult)
    {
        if (one == true && two == false && three == false)
        {
            score += (int)(bad * mult);
        }
        else if (one == true && two == true && three == false)
        {
            score += (int)(ok * mult);
        }
        else if (one == false && two == true && three == false)
        {
            score += (int)(good * mult);
        }
        else if (one == false && two == true && three == true)
        {
            score += (int)(great * mult);
        }
        else if (one == false && two == false && three == true)
        {
            score += (int)(Perfect * mult);
        }
    }
    
    public void SetOneState(bool i)
    {
        one = i;
    }
    public void SetTwoState(bool i) {
        two = i;
    }
    public void SetThreeState(bool i) {
        three = i;
    }



    // Update is called once per frame
    public void EndGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
