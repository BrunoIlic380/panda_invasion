using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeBuying : TradeTower   //inherits from the parent TradeTower script instead of monobehavior
{
    //used to identify what kind of tower is being bought, since we have several
    public GameObject TowerPrefab;

    //needs to be declared as override (and needs to be abstract in the parent class)
    public override void OnPointerClick(PointerEventData eventData)
    {
        int price = TowerPrefab.GetComponent<TowerScript>().initialCost; //gets the price tag from the prefab

        if (price <= moneyMeter.getMoneyAmount())       //checks if the player can afford the tower
        {
            moneyMeter.ChangeMoney(-price);             //payment succeeds and funds are removed from the player's wallet

            GameObject newTower = Instantiate(TowerPrefab); //after payment the tower is instantiated

            currentActiveTower = newTower.GetComponent<TowerScript>();   //the new tower becomes the current selection
        }
    }

}
