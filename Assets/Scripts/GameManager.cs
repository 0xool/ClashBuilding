using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameModel;
using TMPro;

public class GameManager : SingletonBehaviour<GameManager>
{
    public string PlayerOneTag = "Player1";
    public string PlayerTwoTag = "Player2";
    private Player playerOne;
    private Player playerTwo;
    private string _currentPlayer;
    public string currentPlayer {
        get{
            return _currentPlayer;
        }set{
            _currentPlayer = value;
        }
    }
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
    void Start()
    {
        playerOne = new Player();
        playerTwo = new Player();
        SetCurrentPlayerOne();
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

        if(player == PlayerOneTag){
            GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().text = "Player Two Won";
        }else if (player == PlayerTwoTag){
            GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().text = "Player One Won";
        }
    }

    public int GetPlayerResource() {
        if(currentPlayer == PlayerOneTag){
            return playerOne.resourceValue;
        }
        if(currentPlayer == PlayerTwoTag){
            return playerTwo.resourceValue;
        }

        return 0;
    }
    // TODO: Remove all toghether player1 and player2.
    // Enemy Can't call resource network will handel it.
    public bool UseResource(int amount) {
        
        if(currentPlayer == PlayerOneTag){
            if(playerOne.resourceValue < amount) return false;
            playerOne.resourceValue -= amount;
            SetResourceText();
            return true;
        }

        if(currentPlayer == PlayerTwoTag){
            if(playerTwo.resourceValue < amount) return false;
            playerTwo.resourceValue -= amount;
            SetResourceText();
            return true;
        }
    
        return false;
    }

    public void IncreaseResourceIncome(int resource) {
        this.playerTwo.IncreaseResourcePower(resource);
    }

    public void DescreaseResourceIncome(int resource) {
        this.playerTwo.DecreaseResourcePower(resource);
    }

    public void SetCurrentPlayerOne() {
        currentPlayer = PlayerOneTag;
        this.transform.parent.transform.eulerAngles = new Vector3(30, -135, 0);
    }

    public void SetCurrentPlayerTwo() {
        currentPlayer = PlayerTwoTag;
    }

    public bool IsPlayerOne() {
        return currentPlayer == PlayerOneTag;
    }

    public bool IsPlayerTwo() {
        return currentPlayer == PlayerTwoTag;
    }

    public string GetEnemyTag() {
        return (currentPlayer == PlayerOneTag) ? PlayerTwoTag : PlayerOneTag;
    }
}
