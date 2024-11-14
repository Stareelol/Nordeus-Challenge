using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMScript : MonoBehaviour
{
    public void OnStart()
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
