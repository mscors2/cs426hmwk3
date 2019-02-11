using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerMove : NetworkBehaviour
{
    public float baseSpeed = 25.0f;
    public float strafeSpeed = 15.0f;
    public float jumpForce = 700.0f;

    private float boostSecondsLeft = 0.0f;
    private float speedMult = 1.0f;
    private bool bAirborne = false;
    private float dragCache = 0.0f;
    private GameObject orb1, orb2, orb3, orb4;
    private int stage;

    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<MeshRenderer>().material.color = isLocalPlayer ? Color.cyan : Color.magenta;
        orb1 = GameObject.Find( "Speed Orbs/Orb1" );
        orb1.SetActive( false );
        orb2 = GameObject.Find( "Speed Orbs/Orb2" );
        orb2.SetActive( false );
        orb3 = GameObject.Find( "Speed Orbs/Orb3" );
        orb3.SetActive( false );
        orb4 = GameObject.Find( "Speed Orbs/Orb4" );
        orb4.SetActive( false );
        stage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ( isLocalPlayer )
            UpdateLocalPlayer();
    }

    void UpdateLocalPlayer()
    {
        // update speed boost
        if ( boostSecondsLeft > 0.0f )
        {
            boostSecondsLeft -= Time.deltaTime;
        }
        else // boost is over
        {
            boostSecondsLeft = 0.0f;
            speedMult = 1.0f;
        }

        // can't move while in air
        if ( !bAirborne )
        {
            // accelerate
            if ( Input.GetKey( KeyCode.W ) )
                rb.velocity += this.transform.forward * baseSpeed * speedMult * Time.deltaTime;
            else if ( Input.GetKey( KeyCode.S ) )
                rb.velocity -= this.transform.forward * baseSpeed * speedMult * Time.deltaTime;

            // strafe
            if ( Input.GetKey( KeyCode.D ) )
                rb.velocity += this.transform.right * strafeSpeed * Time.deltaTime;
            else if ( Input.GetKey( KeyCode.A ) )
                rb.velocity -= this.transform.right * strafeSpeed * Time.deltaTime;

        }

        // jump
        if ( Input.GetKeyDown( KeyCode.Space ) && !bAirborne )
        {
            rb.AddForce( this.transform.up * jumpForce );
            bAirborne = true;
            dragCache = rb.drag;
            rb.drag = 0.0f;
        }

        // display orb 1
        if ( stage == 0 && transform.position.x > -5.0f )
        {
            orb1.SetActive( true );
            stage++;
        }

        // display orbs 2 and 3
        if ( stage == 1 && transform.position.x > 26.0f )
        {
            orb2.SetActive( true );
            orb3.SetActive( true );
            stage++;
        }

        // display orb 4
        if (stage == 2 && transform.position.x > 78.0f)
        {
            orb4.SetActive(true);
            stage++;
        }
    }

    public void SpeedBoost( float speedMultiplier, float lengthInSeconds )
    {
        boostSecondsLeft = lengthInSeconds;
        speedMult = speedMultiplier;
    }

    public void OnCollisionEnter( Collision collision )
    {
        // any kind of collision at all will refresh the jump. Not ideal but don't feel like mapping out all the floor items
        bAirborne = false;
        rb.drag = dragCache;
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag("Orb") )
        {
            boostSecondsLeft = 2.0f; // boost for 2 seconds
            speedMult = 1.25f;       // by multiplier 1.25
        }
    }

    public override void OnStartLocalPlayer()
    {
        rb = GetComponent<Rigidbody>();
        bAirborne = false;
        speedMult = 1.0f;
        dragCache = rb.drag;
    }
}

