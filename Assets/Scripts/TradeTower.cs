using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//this is going to be a parent class that has three children for buying, selling or upgrading
public abstract class TradeTower : MonoBehaviour, IPointerClickHandler
{
    //static reference to the player's funds
    protected static MoneyMeterScript moneyMeter;

    //reference to the current selected tower
    protected static TowerScript currentActiveTower;


	// Use this for initialization
	void Start ()
    {
		//finds a reference to the player's funds
        //this script could be improved to handle an exception
        if (moneyMeter == null)
        {
            moneyMeter = FindObjectOfType<MoneyMeterScript>();
        }
	}
	
	//static function that allows other scripts to assign the new or current selected tower
    public static void setActiveTower (TowerScript thisTower)
    {
        currentActiveTower = thisTower;
    }

    //abstract function triggered when one of the trading buttons is pressed
    public abstract void OnPointerClick(PointerEventData eventData);
}
