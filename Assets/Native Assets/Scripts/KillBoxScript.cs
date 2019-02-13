using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoxScript : MonoBehaviour
{
    public GameObject respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag( "Player" ) )
        {
            other.gameObject.transform.position = respawnPoint.transform.position;
            other.gameObject.GetComponent<Rigidbody>().velocity.Set( 0, 0, 0 );
        }
    }
}
