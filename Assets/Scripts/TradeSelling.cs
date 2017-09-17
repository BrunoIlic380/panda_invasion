using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeSelling : TradeTower   //inherits from the parent TradeTower script instead of monobehavior
{
    //used to identify what kind of tower is being bought, since we have several

    //needs to be declared as override (and needs to be abstract in the parent class)
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (currentActiveTower == null)         //checks if there is a tower selected
        {
            return;
        }

        moneyMeter.ChangeMoney(currentActiveTower.sellingValue);    //adds the selected tower's resale value to the player's funds

        Destroy(currentActiveTower.gameObject);        //removes the tower being sold from the game world
    }

}