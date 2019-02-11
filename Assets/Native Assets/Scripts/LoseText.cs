using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoseText : NetworkBehaviour
{
    public GameObject loseText;

    // Start is called before the first frame update
    void Start()
    {
        loseText.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider Player)
    {
        if (Player.gameObject.tag == "Player")
        {
            if (!isLocalPlayer)
            {
                loseText.SetActive(true);
            }
        }
    }
}
