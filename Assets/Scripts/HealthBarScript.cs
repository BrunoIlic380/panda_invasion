using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [Tooltip("Starting health, self-explanatory.")]
    public int maxHealth = 100;


    [Tooltip ("health bar filling image UI component")]
    public Image fillingImage; //reference to the health bar filling image component

    [HideInInspector]
    public int health; //current health

	void Start ()
    {

        //set starting health to max health
        health = maxHealth;

        //calls method to update the health bar ui component
        updateHealthBar();
	}
	
	
	void Update ()
    {
        updateHealthBar();
	}



    //recovers player health by the inputed amount 
    public void repair(int value)
    {
        if ((health > 0) && (health < maxHealth))
        {

            health += value;

            if (health > maxHealth)
            {
                health = maxHealth;     //ensures that it won't go over starting health
                updateHealthBar();      //calls method to update the health bar ui component
                Debug.Log("Recovered full health");
            }
            else
            {
                Debug.Log("Recovered " +value +" health");
            }

        }
        
    }

    void updateHealthBar()
    {
        //calculates the percentage (from 0 to 1f) of the current health compared to max health
        float percentage = health * 1f / maxHealth;

        //assigns the percentage to the filling amount of the health_bar_filling ui object
        fillingImage.fillAmount = percentage;
    }
}
