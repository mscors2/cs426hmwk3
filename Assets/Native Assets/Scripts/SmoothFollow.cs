using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SmoothFollow : MonoBehaviour
{
    public new GameObject camera;
    public GameObject target;

    public float speed = 2.0f;

    void Update()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp( this.transform.position.y, camera.transform.position.y, interpolation );
        position.x = Mathf.Lerp( this.transform.position.x, camera.transform.position.x, interpolation );

        this.transform.position = position;
//         this.transform.LookAt( target.transform );
    }
}
