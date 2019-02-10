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

    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<MeshRenderer>().material.color = isLocalPlayer ? Color.cyan : Color.magenta;
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

    public override void OnStartLocalPlayer()
    {
        rb = GetComponent<Rigidbody>();
        bAirborne = false;
        speedMult = 1.0f;
        dragCache = rb.drag;
    }
}

