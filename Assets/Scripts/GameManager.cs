using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameModel;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Player playerOne;
    private Player playerTwo;
    
    float resourceTimer = 0;
    public float addResourceInterval = 1;
    private TMP_Text playerResourceText;
    //============================================================================================================
    private void SetTestPlayers()
    { 
        playerOne.name = "Tom";
        playerTwo.name = "Jack";
    }
    //============================================================================================================
    // Start is called before the first frame update
    void Awake()
    {
        playerOne = new Player();
        playerTwo = new Player();
    }
    void Start()
    {
        SetTestPlayers();
        this.playerResourceText = GameObject.Find("ResourcePanel").GetComponentInChildren<TMP_Text>();
    }
    // Update is called once per frame
    void Update()
    {
        if (resourceTimer >= addResourceInterval) {
            resourceTimer = resourceTimer - addResourceInterval;
            ManageTheResources();
            SetResourceText();
        }

        resourceTimer += Time.deltaTime;
    }

    void ManageTheResources() {
        playerOne.AddResources();
        playerTwo.AddResources();
    }

    private void SetResourceText() {
        playerResourceText.text = playerTwo.resourceValue.ToString();
    }

    public void GameOver(string player){
        GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().enabled = true;
        GameObject.Find("GameOverText").GetComponent<TMP_Text>().enabled = true ;

        if(player == "Player1"){
            GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().text = "Player Two Won";
        }else if (player == "Player2"){
            GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().text = "Player One Won";
        }
    }

    public int GetPlayerResource(string playerTag) {
        if(playerTag == "Player1"){
            return playerOne.resourceValue;
        }
        if(playerTag == "Player2"){
            return playerTwo.resourceValue;
        }

        return 0;
    }

    public bool UseResource(int amount, string playerTag) {
        
        if(playerTag == "Player1"){
            if(playerOne.resourceValue < amount) return false;
            playerOne.resourceValue -= amount;
            SetResourceText();
        }

        if(playerTag == "Player2"){
            if(playerTwo.resourceValue < amount) return false;
            playerTwo.resourceValue -= amount;
            SetResourceText();
        }
    
        return false;
    }
}
