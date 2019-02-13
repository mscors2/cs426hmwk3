using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WinText : NetworkBehaviour
{
    public GameObject winText;

    // Start is called before the first frame update
    void Start()
    {
        winText.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider Player)
    {
        if (Player.gameObject.tag == "Player")
        {
            if (Player.gameObject.GetComponent<PlayerMove>().isLocalPlayer)
            {
                //SHOW WIN TEXT
                winText.SetActive(true);
            }
        }
    }
}
