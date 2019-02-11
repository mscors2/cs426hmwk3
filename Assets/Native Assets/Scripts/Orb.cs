using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate( Vector3.up );
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag("Player") )
        {
            Invoke( "Reappear", 4.0f );          // reappears in 4 seconds
            this.gameObject.SetActive( false );  // disappears
        }
    }

    public void Reappear()
    {
       this.gameObject.SetActive( true );        // reappears
    }
}
