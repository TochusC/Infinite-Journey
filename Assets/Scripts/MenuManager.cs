using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public void StartGame()
    {
       gameManager.isGameStarted = true;
    }
}
