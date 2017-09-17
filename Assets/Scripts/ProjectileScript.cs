using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Tooltip("how much damage a single projectile does")]
    [Range(0f, 1000.0f)]
    public float damage;                //how much damage a single projectile does

    [Tooltip("projectile speed")]
    [Range(0f, 1000.0f)]
    public float speed = 1f;            //projectile speed

    [HideInInspector]
    public Vector3 direction;           //projectile heading

    [Tooltip ("time before the projectile self-destructs")]
    [Range(0f, 100.0f)]
    public float lifeDuration = 0.5f;     //time before the projectile self-destructs, to save cpu
    private float timer;

    //private reference to the projectile's rigidbody
    private Rigidbody2D rb2D;
    private GameObject parentObject;  //parent object for this script, used for self destruction


	
	void Start ()
    {
        rb2D = GetComponent<Rigidbody2D>(); //sets up the reference to the rigidbody

        timer = 0;

        //normalizes the direction
        direction = direction.normalized;  //normalize keeps the same orientation but changes the magnitude to 1.0

        //fixes the rotation
        float angle = (Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg) + 90f; //atan2 finds the angle in radians between 2 points
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //sets up selfdestruct
        Destroy(gameObject, lifeDuration);
	}
	
	
	void FixedUpdate ()
    {
        //updates the projectile's position based on starting position, heading, time and speed
        rb2D.MovePosition(transform.position + direction * Time.fixedDeltaTime * speed);
        
        /*if(rb2D.velocity.magnitude == 0)
        {
            Destroy(gameObject);
        }*/
	}

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeDuration)
        {
            Destroy(gameObject);
        }
    }
}
