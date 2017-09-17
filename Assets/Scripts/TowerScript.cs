using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
   
    [Range(0f, 10000.0f)]
    public float rangeRadius;   //maximum range that the tower can fire at

    [Range(0f, 10.0f)]
    public float reloadTime;    //the reload time eg cooldown

    public GameObject projectilePrefab;     //type of projectile that is fired from the tower after cooldown

    private float elapsedTime;  //time elapsed from the last time a shot was fired

    [Range (0, 100)]
    public int initialCost;     //initial cost to buy this tower

    [Range(0, 100)]
    public int upgradingCost;   //cost to upgrade this tower

    private int upgradeLevel;   //this tower's upgrade level

    [Range(0, 100)]
    public int sellingValue;    //the resale value of this tower, must be lower than initialCost




    [Tooltip("array of sprites for different upgrade levels")]
    public Sprite[] upgradeSprites; //array of sprites for different upgrade levels

    [Tooltip("can the tower be upgraded")]
    public bool isUpgradable = true;    //can the tower be upgraded
	
	void Start ()
    {
        elapsedTime = 0;  //change elapsedtime from 0 to reloadTime if you want to fire in the first frame
        upgradeLevel = 0;
        GetComponent<SpriteRenderer>().sprite = upgradeSprites[upgradeLevel];
    }

    void OnMouseDown()           //if a tower is clicked, it will be set as the active tower and can be sold/upgraded
    {
        TradeTower.setActiveTower(this);
        GameObject[] selectionBoxes = Resources.FindObjectsOfTypeAll<GameObject>();

        for (int i = 0; i < selectionBoxes.Length; i++)
        {
            if (selectionBoxes[i].CompareTag("SelectionBox") && selectionBoxes[i].transform.IsChildOf(transform))
            {
                selectionBoxes[i].SetActive(true);
            }
            else if (selectionBoxes[i].CompareTag("SelectionBox"))
            {
                selectionBoxes[i].SetActive(false);
            }

        }

    }

    public void Upgrade()
    {
        //returns if tower is not upgradeable
        if (!isUpgradable)
        {
            return;
        }

        upgradeLevel++;
  
        if (upgradeLevel < upgradeSprites.Length)  //checks if tower is already at max level
        {
            isUpgradable = true;
        }
        else
        {
            isUpgradable = false;
        }

        rangeRadius += 2f;
        reloadTime = reloadTime * 0.83f;                 //increases the tower's range and rate of fire
        if (reloadTime <= 0.5f) { reloadTime = 0.5f; }   //prevents excessive fire rates

        GetComponent<SpriteRenderer>().sprite = upgradeSprites[upgradeLevel];           //changes the tower's sprite based on level

        sellingValue += 5;          //increments the resale value every time the tower is upgraded
        upgradingCost += 5;        //increments the cost to make further upgrades
    }

	void Update ()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= reloadTime)
        {
            //resets elapsedTime
            elapsedTime = 0;

            //creates an array of all gameobjects with colliders within this tower's range
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, rangeRadius);

            //will only execute if there is a collider in range
            if (hitColliders.Length != 0)
            {
                //sets default values for index and distance
                float min = int.MaxValue;
                int index = -1;

                //loops over all gameobjects to find the closest enemy
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].tag == "Enemy") //runs the code if the object at index is really an enemy
                    {
                        float distance = Vector2.Distance(hitColliders[i].transform.position, transform.position);
                        if (distance < min) //cycles through all found enemies to determine the closest
                        {
                            index = i;
                            min = distance;
                        }
                    }
                }
                /*if (index == -1) //code commented out in favor of presumably safer code
                {
                    return;
                }*/

                if (index != -1) //will only fire if there is at least one enemy in range
                {
                    Transform target = hitColliders[index].transform;
                    Vector2 direction = (target.position - transform.position).normalized; //calculates a magnitude 1 vector towards the target position
                    GameObject projectile = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject; //instantiates a projectile at the turret's position
                    projectile.GetComponent<ProjectileScript>().direction = direction; //assigns a direction to the instantiated projectile
                    //instantiating drains cpu and is often replaced or worked around
                }
            }

        }
        elapsedTime += Time.deltaTime;
    }
}
