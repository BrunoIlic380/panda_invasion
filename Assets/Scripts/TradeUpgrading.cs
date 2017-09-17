using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeUpgrading : TradeTower   //inherits from the parent TradeTower script instead of monobehavior
{

    //needs to be declared as override (and needs to be abstract in the parent class)
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (currentActiveTower != null && currentActiveTower.isUpgradable && currentActiveTower.upgradingCost <= moneyMeter.getMoneyAmount())
        {           //checks if there is a tower, if it is upgradeable and if the upgrade can be afforded
            moneyMeter.ChangeMoney(-currentActiveTower.upgradingCost);      //the payment is processed

            currentActiveTower.Upgrade();   //the tower is upgraded
        }
    }

}