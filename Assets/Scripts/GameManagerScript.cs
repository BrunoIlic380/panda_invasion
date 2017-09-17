using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private bool _isPointerOnAllowedArea = true;    //checks if the mouse is over an area where a tower can be placed

    [Tooltip ("use an empty game object with an attached Waypoint.cs to create waypoints, order is important")]
    public Waypoint firstWaypoint;  //reference to the first waypoint in the chain, should be on a straight horizontal or vertical line from the spawn point

    public GameObject winningScreen;    //winning, losting and pause screens should be disabled in the inspector
    public GameObject losingScreen;     //they are set active by a script when they are needed
    public GameObject pauseScreen;
    public GameObject howToScreen;

    private HealthBarScript playerHealth;   //reference to the player's health

    public int startingCash;            //the player should have enough starting budget to buy 2-3 towers depending on balance
    private MoneyMeterScript budget;    //reference to the UI element that keeps track of the player's money

    public CounterScript killCounter;      //ref to the enemy counter ui element
    public CounterScript naturalCausesCounter;  //same, but for natural causes

    private Transform spawner;      //the spawn point's location
    public GameObject enemyPrefab;  //reference to the enemy prefab
    public int numberOfWaves;           //how many waves of enemies will the game manager spawn
    public int numberOfEnemiesPerWave = 4;  //how many enemies will be spawned in the FIRST wave
    [HideInInspector]
    public int waveNumber = 0;
    private int originalNumberOfEnemies;

    public int numberNeededToWin = 420;

    [HideInInspector]
    public int enemiesKilled = 0;
    [HideInInspector]
    public int naturalCauses = 0;

    public float restTime = 3f;          //rest time after each wave


    private bool IsPlayerDead()     //i hada comment for this but i was unnecessary
    {
        if (playerHealth.health <= 0)  
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (IsPlayerDead())
        {
            GameOver(false);
        }

        if (Input.GetKey("escape"))     //activates the pause menu and stops time
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }

        if (Input.GetMouseButtonDown(1))    //deselects the tower if rmb is pressed
        {
            TradeTower.setActiveTower(null);

            GameObject[] selectionBoxes = Resources.FindObjectsOfTypeAll<GameObject>();

            for (int i = 0; i < selectionBoxes.Length; i++)
            {
                if (selectionBoxes[i].CompareTag("SelectionBox"))
                {
                    selectionBoxes[i].SetActive(false);
                }

            }
        }

        if (killCounter.value >= numberNeededToWin)
        {
            GameOver(true);
        }

    }
    //private function that is going to get called when one of the game over conditions is met
    private void GameOver (bool playerHasWon)
    {
        //has the player won?
        if (playerHasWon)
        {
            winningScreen.SetActive(true); //displays the win screen game object
        }
        else
        {
            losingScreen.SetActive(true);   //displays the lose screen in case the player hasn't won
        }

        Time.timeScale = 0;     //pauses the game after GameOver() is called
    }

    //function that returns true if the mouse is over an area where a tower can be placed
    public bool isPointerOnAllowedArea()
    {
        return _isPointerOnAllowedArea;
    }

    //method that is called when the mouse enters one of the colliders where the towers can be placed
    void OnMouseEnter()
    {
        _isPointerOnAllowedArea = true;
    }

    void OnMouseExit()
    {
        _isPointerOnAllowedArea = false;
    }

    private void Start()
    {
        playerHealth = FindObjectOfType<HealthBarScript>();     //gets the player's health at game start
        budget = FindObjectOfType<MoneyMeterScript>();
        originalNumberOfEnemies = numberOfEnemiesPerWave;
        Time.timeScale = 1;

        try
        {
            budget.ChangeMoney(startingCash);
        }
        catch
        {
            Debug.Log("MoneyMeterScript not present in scene.");
        }


        try
        {
            spawner = GameObject.Find("Spawn point").transform;
        }
        catch
        {
            Debug.Log("'Spawn point' not present or renamed.");
        }

        StartCoroutine(WavesSpawner());
    }


    private IEnumerator WavesSpawner()      //coroutine that spawns waves of enemies
    {
        for (int i = 0; i < numberOfWaves; i++) //runs once per each wave
        {
            Debug.Log("Spawning wave " + (i+1));
            waveNumber++;          
            yield return EnemySpawner();    //yields and lets another coroutine called EnemySpawner() handle the individual wave
            
            if (waveNumber < 12)
            {
                numberOfEnemiesPerWave++;    //increases the number of enemies in the next wave by 3
            }
            else if (waveNumber < 18)
            {
                numberOfEnemiesPerWave += 2;
            }
            else
            {
                numberOfEnemiesPerWave += 3;    //xexexexe
            }
        }

        GameOver(true);         //if the player has defeated all waves, GameOver is called and the player has won
    }

    private IEnumerator EnemySpawner()
    {
        for (int i = 0; i < numberOfEnemiesPerWave; i++)
        {
            Instantiate(enemyPrefab, spawner.position, Quaternion.identity);    //instantiates the enemy at the spawn point

            float swarmModifier = originalNumberOfEnemies / numberOfEnemiesPerWave;
            float timeToWait = 1f * (Mathf.Lerp(0f, 1.5f, swarmModifier) + Random.Range(0.1f, 0.3f));
            yield return new WaitForSeconds(timeToWait);    //will wait a random number of seconds based on the formula, less if there is more enemies
        }
        restTime = 6f + (waveNumber / 2f);
        yield return new WaitForSeconds(restTime);
    }

 

}
