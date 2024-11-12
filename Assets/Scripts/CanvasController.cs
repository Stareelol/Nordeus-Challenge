using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject heartFull1;
    public GameObject heartFull2;
    public GameObject heartFull3;
    public GameObject heartEmpty1;
    public GameObject heartEmpty2;
    public GameObject heartEmpty3;

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
}
