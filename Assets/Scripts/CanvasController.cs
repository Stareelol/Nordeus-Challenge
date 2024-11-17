using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public GameObject heartFull1;
    public GameObject heartFull2;
    public GameObject heartFull3;
    public GameObject heartEmpty1;
    public GameObject heartEmpty2;
    public GameObject heartEmpty3;
    public GameObject heightMap;
    public GameObject gameOver;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI retryText;

    int health;
    GameScript gs;
    GameObject controller;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        gs = controller.GetComponent<GameScript>();
        heartEmpty1.SetActive(false);
        heartEmpty2.SetActive(false);
        heartEmpty3.SetActive(false);
        gameOver.SetActive(false);
    }

    public void ChangeHealth(int health)
    {
        switch(health)
        {
            case 2:
                {
                    heartEmpty3.SetActive(true);
                    heartFull3.SetActive(false);
                } break;
            case 1:
                {
                    heartEmpty2.SetActive(true);
                    heartFull2.SetActive(false);
                } break;
            case 0:
                {
                    heartEmpty1.SetActive(true);
                    heartFull1.SetActive(false);
                }
                break;
        }
    }

    public void GameOver(string result)
    {

        if (heartEmpty1.activeInHierarchy)
        {
            heartEmpty1.GetComponent<Animator>().Rebind();
            heartEmpty1.GetComponent<Animator>().Update(0f);
            heartEmpty1.GetComponent<Animator>().enabled = false;
        }

        if (heartEmpty2.activeInHierarchy)
        {
            heartEmpty2.GetComponent<Animator>().Rebind();
            heartEmpty2.GetComponent<Animator>().Update(0f);
            heartEmpty2.GetComponent<Animator>().enabled = false;
        }

        if (heartEmpty3.activeInHierarchy)
        {
            heartEmpty3.GetComponent<Animator>().Rebind();
            heartEmpty3.GetComponent<Animator>().Update(0f);
            heartEmpty3.GetComponent<Animator>().enabled = false;
        }

        heightMap.SetActive(false);
        gameOver.SetActive(true);

        if (result == "win")
        {
            resultText.text = "V I C T O R Y";
            retryText.text = "You won! Play again?";
        }
        else
        {
            resultText.text = "D E F E A T";
            retryText.text = "You lost! Retry?";
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
