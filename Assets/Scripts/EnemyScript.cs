using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Range (0, 100f)]
    [Tooltip ("the enemy's movement speed")]
    public float speed; //the enemy's movement speed
   
    [Tooltip("max health, default is 1000")]
    public float health; //the enemy's max health

    private Animator Animator;   //reference to the enemy's animator component

    //Hash representations of the Triggers on the Animator controller of the Panda
    //saves cpu by working with an int instead of a string
    private int AnimHitTriggerHash = Animator.StringToHash("HitTrigger");
    private int AnimEatTriggerHash = Animator.StringToHash("EatTrigger");
    private int AnimDieTriggerHash = Animator.StringToHash("DieTrigger");

    //use exact phrasing and capitalization for these three references to ensure it works correctly

    
    private Waypoint currentWaypoint;   //stores the current waypoint the enemy is moving towards

    //private constant under which a waypoint is considered reached
    private const float changeDist = 0.001f;    //needed because of numerical instability

    private Rigidbody2D rb2D;                   //the enemy's rigidbody and boxcollider
    private BoxCollider2D boxCollider2d;        //if making different enemies make sure there two are set up on the prefab

    private static GameManagerScript gameManager;  //static reference to the game manager

    private MoneyMeterScript moneyMeter;        //these are both attached to their UI elements
    private HealthBarScript healthBarScript;

    public int damageDoneAtGoal;    //how much damage does the enemy do when it reaches the finish line


    void Start ()
    {
        //at start, gets a reference to the attached object's components, the game manager, and the player's health and budget
        Animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();

        moneyMeter = FindObjectOfType<MoneyMeterScript>();  
        healthBarScript = FindObjectOfType<HealthBarScript>();
        gameManager = FindObjectOfType<GameManagerScript>();

        currentWaypoint = gameManager.firstWaypoint;

        if (gameManager.waveNumber != 0 && gameManager.waveNumber % 4 == 0) //every 4 waves, increases enemy health by a fixed percentage
        {
            health = health * 1.2f;
        }

        if (gameManager.waveNumber != 0 && gameManager.waveNumber % 6 == 0) //every 6 waves gain additional speed and a minor health bonus
        {
            health = health * 1.2f;
            speed = speed * 1.1f;
        }
    }
	

	void FixedUpdate ()
    {
		//if the enemy has reached the finish line, it will trigger the "eat" animation
        //this script's instance will be removed immediately
        //the enemy will selfdestruct
        //it will immediately selfdestruct if there is no waypoint or if it's been set up improperly
        if (currentWaypoint == null)
        {
            Animator.SetTrigger(AnimEatTriggerHash);
            Destroy(this);
            healthBarScript.health -= damageDoneAtGoal;
            gameManager.naturalCausesCounter.Increment();

            if (gameManager.waveNumber < 4)
            {
                moneyMeter.ChangeMoney(1);  //will gve the player 1$ for each enemy that dies of natural causes, but only in the first few waves
            }

            return;
        }

        //calculates the distance between the enemy and its current waypoint
        float dist = Vector2.Distance(transform.position, currentWaypoint.GetPosition());

        //if the waypoint is close enough the enemy moves towards the next one
        if (dist <= changeDist)
        {
            currentWaypoint = currentWaypoint.GetNextWaypoint();
        }
        //otherwise the enemy moves towards it
        else
        {
            MoveTowards(currentWaypoint.GetPosition());
        }
	}

    //function that is called whenever the panda gets hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")                                      //caution - this only works with projectiles that:
        {                                                                   //have been set up as prefabs AND
            //applies damage to the enemy using the hit function            //have been tagged as projectiles AND
            Hit(other.GetComponent<ProjectileScript>().damage);             //have an active collider

            //ensure that the prefab damage value is set to something other than the default of 0
            //a better version of this script would enable applying on-hit effects

            Destroy(other.gameObject);
        }                                   
    }

    //function that moves the enemy towards its goal
    private void MoveTowards(Vector3 destination)
    {
        //moves the enemy one step towards the goal
        float step = speed * Time.fixedDeltaTime;   //works for an enemy with a rigidbody
        rb2D.MovePosition(Vector3.MoveTowards(transform.position, destination, step));       
    }
    
    //function that gets called whenever the enemy suffers a hit
    //checks if the enemy still has health and handles the hit accordingly
    private void Hit(float damage)
    {
        //substracts damage from the enemy's health
        health -= damage;

        //triggers the die or hit animation as needed
        if (health <= 0)
        {
            Animator.SetTrigger(AnimDieTriggerHash);
            gameManager.enemiesKilled += 1;
            Destroy(boxCollider2d);     //needed to stop the turret from duping money by repeatedly shooting dying enemies
            gameManager.killCounter.Increment();
            speed = 0;

            if (gameManager.waveNumber < 7)     //enemies will give progressively less money as the game goes on
            {
                moneyMeter.ChangeMoney(2);
            }
            else if (gameManager.waveNumber >= 7 && gameManager.waveNumber <14)
            {
                moneyMeter.ChangeMoney(1);
            }
            else
            {
                float coin = Random.value;
                if(coin < (7f / gameManager.waveNumber))
                {
                    moneyMeter.ChangeMoney(1);
                }
            }

        }
        else
        {
            Animator.SetTrigger(AnimHitTriggerHash);
        }
    }
    //we do not need to destroy the enemy here, that is done by the state machine


    //method that triggers the eat animation
    private void Eat()
    {
        Animator.SetTrigger(AnimEatTriggerHash);
        gameManager.naturalCauses += 1;
    }

   

}
