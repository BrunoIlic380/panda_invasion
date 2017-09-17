using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    [HideInInspector]
    public int value;

    public Text valueText;


	// Use this for initialization
	void Start ()
    {
        value = 0;
        valueText.text = value.ToString();
	}
	
	public void Increment()
    {
        value += 1;
        valueText.text = value.ToString();
    }
}
