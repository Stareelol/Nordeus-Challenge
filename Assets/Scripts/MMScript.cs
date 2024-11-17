using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMScript : MonoBehaviour
{

    public GameObject tutorial;
    public GameObject woodenSign;
    void Start()
    {
        tutorial.SetActive(false);
    }
    public void TutorialStart()
    {
        tutorial.SetActive(true);
        woodenSign.SetActive(false);
    }

    public void GameStart() 
    {
        SceneManager.LoadScene("Game");
    }

    public void OnStats()
    {
        Debug.Log("Load Stats");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
