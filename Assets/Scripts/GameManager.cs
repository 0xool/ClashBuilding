using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameModel;
using TMPro;

public class GameManager : NetworkSingletonBehaviour<GameManager>
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
    private bool gameIsOver = false;
    //============================================================================================================
    private void SetTestPlayers()
    { 
        playerOne.name = "Tom";
        playerTwo.name = "Jack";
    }
    //============================================================================================================
    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
        playerOne = new Player();
        playerTwo = new Player();
        SetCurrentPlayerTwo();
        SetTestPlayers();
    }
    void Start()
    {        
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
        if(gameIsOver) return;
        GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().enabled = true;
        GameObject.Find("GameOverText").GetComponent<TMP_Text>().enabled = true ;
        GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().text = (player == PlayerOneTag) ? "Player One Won" : "Player Two Won";
    }

    public int GetPlayerResource() {
        return GetCurrentPlayer().resourceValue;
    }
    // TODO: Remove all toghether player1 and player2.
    // Enemy Can't call resource network will handel it.
    public bool UseResource(int amount) {
        var player = GetCurrentPlayer();
        if(player.resourceValue < amount) return false;
        
        player.resourceValue -= amount;
        SetResourceText();
        return true;
    }

    public void IncreaseResourceIncome(int resource) {
        this.GetCurrentPlayer().IncreaseResourcePower(resource);
    }

    public void IncreaseResourceValue(int resource) {
        this.GetCurrentPlayer().IncreaseResourceValue(resource);
    }

    public void DescreaseResourceIncome(int resource) {
        this.GetCurrentPlayer().DecreaseResourcePower(resource);
    }

    private Player GetCurrentPlayer() {
        return (currentPlayer == PlayerOneTag) ? playerOne : playerTwo;
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
