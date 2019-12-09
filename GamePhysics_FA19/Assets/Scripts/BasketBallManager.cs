using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasketBallManager : MonoBehaviour
{
    [SerializeField]
    bool endGame = false;
    bool resetGame = false;

    int shotsMade = 0;

    [SerializeField]
    GameObject resetButton = null;

    [SerializeField]
    SpawnBall spawnBall;

    [SerializeField]
    RandomX basketBallHoop;

    void Awake()
    {
        resetButton.SetActive(false);
    }

    void Start()
    {
        endGame = false;
        resetGame = false;
        spawnBall.CreateBall();

    }

    void Update()
    {
        if(endGame && resetGame)
            SceneManager.LoadScene(0);
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(SpawnBall());

        }
    }

    public void ShotMade()
    {
        basketBallHoop.randomX();
        shotsMade++;
    }

    public void EndGame()
    {
        endGame = true;
        resetButton.SetActive(true);
    }

    public void ResetGame()
    {
        resetGame = true;
    }


    IEnumerator SpawnBall()
    {

        yield return new WaitForSeconds(1);
        spawnBall.CreateBall();

    }
}
