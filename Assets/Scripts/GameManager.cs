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

    [SerializeField]private Spawner spawner;
    [SerializeField]private float multiplier = 1.5f;

    private float gameTimePassed;

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
    public void AddGrindPointsToScore(int GrindPoints)
    {
        score += GrindPoints;
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

    private void Update()
    {
        gameTimePassed += Time.deltaTime;

        if (gameTimePassed >= 60)
        {
            spawner.levelUp(multiplier);
        }
    }

    public void EndGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
