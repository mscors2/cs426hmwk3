using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Trigger : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    //After crossing the finish line pause the game to be ended
    public bool gamePaused = false;
    public GameObject endMenu;
    // Update is called once per frame

    void OnTriggerEnter()
    {
        StartCoroutine(ProcessEndGame());
    }

    //Coroutine to wait and then End the Game
    IEnumerator ProcessEndGame()
    {
        yield return new WaitForSeconds(3);
        Application.Quit();
    }
}

