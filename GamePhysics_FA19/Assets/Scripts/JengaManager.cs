using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JengaManager : MonoBehaviour
{
    [SerializeField]
    bool endGame = false;
    bool resetGame = false;

    [SerializeField]
    GameObject resetButton = null;

    void Awake()
    {
        resetButton.SetActive(false);
    }

    void Start()
    {
        endGame = false;
        resetGame = false;
    }

    void Update()
    {
        if(endGame && resetGame)
            SceneManager.LoadScene(0);
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
}
