using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    public float baseSpeed = 20.0f;
    public float airSpeed = 5.0f;
    public float jumpForce = 700.0f;

    private float boostSecondsLeft = 0.0f;
    private float speedMult = 1.0f;
    private bool bAirborne = false;
    private float dragCache = 0.0f;

    Rigidbody rb;
    GameObject NormalTrail;
    GameObject BoostTrail;

    // Use this for initialization
    void Start()
    {
        if ( isLocalPlayer)
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;

            Camera camera = GetComponentInChildren<Camera>();
            camera.gameObject.SetActive( false );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( isLocalPlayer )
        {
            UpdateLocalPlayer();
        }
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
            NormalTrail.SetActive( true );
            BoostTrail.SetActive( false );
        }

        float speed = bAirborne ? airSpeed : baseSpeed;

        // accelerate
        if ( Input.GetKey( KeyCode.W ) )
            rb.velocity += this.transform.forward * speed * speedMult * Time.deltaTime;
        else if ( Input.GetKey( KeyCode.S ) )
            rb.velocity -= this.transform.forward * speed * speedMult * Time.deltaTime;

        // strafe
        if ( Input.GetKey( KeyCode.D ) )
            rb.velocity += this.transform.right * speed * Time.deltaTime;
        else if ( Input.GetKey( KeyCode.A ) )
            rb.velocity -= this.transform.right * speed * Time.deltaTime;

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
        NormalTrail.SetActive( false );
        BoostTrail.SetActive( true );
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
            SpeedBoost( 1.25f, 2.0f );
        }
    }

    public override void OnStartLocalPlayer()
    {
        foreach ( TrailRenderer trail in GetComponentsInChildren<TrailRenderer>() )
        {
            if (trail.gameObject.CompareTag("NormalTrail"))
            {
                NormalTrail = trail.gameObject;
            }
            else if (trail.gameObject.CompareTag("BoostTrail"))
            {
                BoostTrail = trail.gameObject;
                BoostTrail.SetActive( false );
            }
        }
        rb = GetComponent<Rigidbody>();
        bAirborne = false;
        speedMult = 1.0f;
        dragCache = rb.drag;
    }
}

