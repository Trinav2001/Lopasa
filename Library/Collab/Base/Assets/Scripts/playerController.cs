using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class playerController : MonoBehaviour
{
    public Rigidbody2D rd;

    private float speed = 5.0f;
    private float power = 6.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rd.transform.localScale= new Vector3(-1,1,1);
            rd.velocity = new Vector2(-3*speed, rd.velocity.y);
        } 
        if (Input.GetKey(KeyCode.D))
        {
            rd.transform.localScale= new Vector3(1,1,1);
            rd.velocity = new Vector2(3*speed, rd.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rd.velocity = new Vector2(rd.velocity.x, power * 4.9f);
        }
    }
}
