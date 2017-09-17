using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    private GameManagerScript gameManager;  //reference to the game manager script

	// Use this for initialization
	void Start ()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //gets the mouse position
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        //places the tower at mouse position, corrected for z-levels
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 7));      //the camera is at z = -10, so we need to adjust the tower's depth by +7

		//if the player clicks, checks if the tower is in a legal position
        if (Input.GetMouseButtonDown(0) && gameManager.isPointerOnAllowedArea())
        {
            //enables the tower's script (ensures it won't fire while it being moved)
            GetComponent<TowerScript>().enabled = true;

            gameObject.AddComponent<BoxCollider2D>(); //gives the tower a collider so another tower can't get on top of it

            Destroy(this); //removes this script, so the placed tower stops following the mouse
        }
	}
}
