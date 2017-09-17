using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyMeterScript : MonoBehaviour
{
    private Text moneyMeter;    //reference to the text component the script will change

    private int money;          //amount of money (money) the player currently has

	// Use this for initialization
	void Start ()
    {
        //gets the reference to the money meter text
        moneyMeter = GetComponentInChildren<Text>();

        //calls method to update the money meter
        updateMoneyMeter();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //function to increase or decrease the amount of money
    public void ChangeMoney(int value)
    {
        money += value;

        //if the amount of money is negative, it will be set to 0
        if (money < 0)
        {
            money = 0;
        }

        //calls method to update the money meter
        updateMoneyMeter();
    }

    //function to return the current amount of money
    public int getMoneyAmount()
    {
        return money;
    }

    //updates the ui element based on the amount of money
    void updateMoneyMeter()
    {
        //parses the value to string
        moneyMeter.text = money.ToString();
    }
}
