using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using GameModel;
using TMPro;
using System;

public enum PlayerTag
{
    PlayerOne,
    PlayerTwo
}

public class GameManager : NetworkSingletonBehaviour<GameManager>
{
    private Player playerOne;
    private Player playerTwo;
    private int playersConnected = 0;
    private bool gameStart = false;
    private PlayerTag _currentPlayer;
    private PlayerTag currentPlayer {
        get {
            return _currentPlayer;
        }
        set {
            _currentPlayer = value;
        }
    }
    float resourceTimer = 0;
    public float addResourceInterval = 1;
    private TMP_Text playerResourceText;
    private bool gameIsOver = false;
    private NetworkVariable<int> playerOneResource = new NetworkVariable<int>(0);
    private NetworkVariable<int> playerTwoResource = new NetworkVariable<int>(0);
    private int connectedPlayers = 0; 
    private bool connected = false;
    private Energy playerOneEnergy;
    private Energy playerTwoEnergy;
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
    }

    void Start()
    {    
        /// Mark: This is for testing
        // SetCurrentPlayerOne();
        
        if(IsServer){

        }
        
        this.playerResourceText = GameObject.Find("ResourcePanel").GetComponentInChildren<TMP_Text>();
    }
    // Update is called once per frame
    void Update()
    {
        if(IsClient && !connected){
            ConnectToServerRpc();
            connected = true;
        }

        if(!gameStart) return;

        if(IsClient){
            this.playerOne.resourceValue = playerOneResource.Value;
            this.playerTwo.resourceValue = playerTwoResource.Value;

            SetResourceText();
        }

        if(IsServer){
            if (resourceTimer >= addResourceInterval) {
                    resourceTimer = resourceTimer - addResourceInterval;
                    ManageTheResources();        
                    
                    playerOneResource.Value = this.playerOne.resourceValue;
                    playerTwoResource.Value = this.playerTwo.resourceValue;                
                }

            resourceTimer += Time.deltaTime;
        }
    }

    public void ConnectedPlayerSetup(){
        playersConnected++;
        gameStart = (playersConnected == 2);
    }

    void ManageTheResources() {
        playerOne.AddResources();
        playerTwo.AddResources();
    }

    private void SetResourceText() {
        playerResourceText.text = GetCurrentPlayer().resourceValue.ToString();
    }

    public void GameOver(string player){
        PlayerTag playerTag = GetPlayerTag(player);
        if(gameIsOver) return;
        GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().enabled = true;
        GameObject.Find("GameOverText").GetComponent<TMP_Text>().enabled = true ;
        GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().text = (playerTag == PlayerTag.PlayerOne) ? "Player One Won" : "Player Two Won";
    }


    public int GetPlayerResource() {
        return GetCurrentPlayer().resourceValue;
    }

    public int GetPlayerResourceWithTag(PlayerTag tag) {
        return GetPlayerWithTag(tag).resourceValue;
    }

    public int GetPlayerResourceWithTagString(string tag) {
        PlayerTag playerTag = (PlayerTag)Enum.Parse(typeof(PlayerTag), tag);
        return GetPlayerWithTag(playerTag).resourceValue;
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

    public bool UsePlayerResource(int amount, PlayerTag tag) {
        var player = GetPlayerWithTag(tag);
        return (player.resourceValue > amount);
    }

    public void IncreaseResourceIncome(int resource) {
        this.GetCurrentPlayer().IncreaseResourcePower(resource);
    }

    public void IncreaseResourceIncomeForPlayer(int resource, PlayerTag playerTag) {
        this.GetPlayerWithTag(playerTag).IncreaseResourcePower(resource);
    }
    

    public void IncreaseResourceValueForPlayer(int resource, PlayerTag playerTag) {
        this.GetPlayerWithTag(playerTag).IncreaseResourceValue(resource);
    }

    public void IncreaseResourceValue(int resource) {
        this.GetCurrentPlayer().IncreaseResourceValue(resource);
    }

    public void DescreaseResourceIncome(int resource) {
        this.GetCurrentPlayer().DecreaseResourcePower(resource);
    }

    private Player GetCurrentPlayer() {
        return (currentPlayer == PlayerTag.PlayerOne) ? playerOne : playerTwo;
    }

    private Player GetPlayerWithTag(PlayerTag tag) {
        return (tag == PlayerTag.PlayerOne) ? playerOne : playerTwo;
    }

    public void SetCurrentPlayerOne() {
        currentPlayer = PlayerTag.PlayerOne;
        this.transform.parent.transform.eulerAngles = new Vector3(30, -135, 0);
        TouchManager.instance.SetUpConstructionMovmentDirection();
    }

    public void SetCurrentPlayerTwo() {
        currentPlayer = PlayerTag.PlayerTwo;
        TouchManager.instance.SetUpConstructionMovmentDirection();
    }

    public bool IsPlayerOne() {
        return currentPlayer == PlayerTag.PlayerOne;
    }

    public bool IsPlayerTwo() {
        return currentPlayer == PlayerTag.PlayerTwo;
    }

    public string GetCurrentPlayerTagString(){
        return GetPlayerTagString(this.currentPlayer);
    }

    public PlayerTag GetCurrentPlayerTag(){
        return this.currentPlayer;
    }

    public static string GetPlayerTagString(PlayerTag tag){
        switch (tag) {
            case PlayerTag.PlayerOne:
                return "PlayerOne";
            case PlayerTag.PlayerTwo:
                return "PlayerTwo";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static PlayerTag GetPlayerTag(string tag){
        switch (tag) {
            case "PlayerOne":
                return PlayerTag.PlayerOne;
            case "PlayerTwo":
                return PlayerTag.PlayerTwo;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public PlayerTag GetEnemyTag() {
        return (currentPlayer == PlayerTag.PlayerOne) ? PlayerTag.PlayerTwo : PlayerTag.PlayerOne;
    }

    public string GetEnemyTagString() {
        return GetPlayerTagString(GetEnemyTag());
    }

    public PlayerTag GetEnemyTag(PlayerTag tag) {
        return (tag == PlayerTag.PlayerOne) ? PlayerTag.PlayerTwo : PlayerTag.PlayerOne;
    }

    public string GetEnemyTag(string tag) {
        if (tag == PlayerTag.PlayerOne.ToString())
        {
            return PlayerTag.PlayerTwo.ToString();
        }
        else if (tag == PlayerTag.PlayerTwo.ToString())
        {
            return PlayerTag.PlayerOne.ToString();
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void BuildConstructionServerRpc(string constructionName, Vector3 constructionPos, PlayerTag playerTag, int amount, ServerRpcParams serverRpcParams = default) 
    {
        //if(!gameStart) return;
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
            var player = GetPlayerWithTag(playerTag);
            if(player.resourceValue < amount) return;
            player.resourceValue -= amount; 
        
            playerOneResource.Value = this.playerOne.resourceValue;
            playerTwoResource.Value = this.playerTwo.resourceValue;
            GameObject construction = Instantiate(Utilities.GetConstructionGameObject(constructionName), constructionPos, Quaternion.identity);
            var constructionNetworkObject = construction.GetComponent<NetworkObject>();
            if(!constructionNetworkObject.IsSpawned) constructionNetworkObject.Spawn();
            construction.GetComponent<IConstructable>().SetupConstructionClientRpc(playerTag.ToString());
            construction.tag = playerTag.ToString();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnUnitServerRpc(string unitName, Vector3 spawnPos, PlayerTag playerTag, ServerRpcParams serverRpcParams = default) 
    {
        if(!gameStart) return;
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
            GameObject construction = Utilities.GetUnitGameObject(unitName);
        }
    }

    private void ConnectPlayer(ulong[] clientID){
        connectedPlayers++;
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = clientID
            }
        };

        if(connectedPlayers == 1){
            playerOne.cliendID = clientID;
            SetupClientRpc(PlayerTag.PlayerOne, playerOne.cliendID, clientRpcParams);
        }else if (connectedPlayers == 2){
            playerTwo.cliendID = clientID;
            SetupClientRpc(PlayerTag.PlayerTwo, playerTwo.cliendID, clientRpcParams);
            gameStart = true;
            
            GameObject.Find("LoadingImage").SetActive(false);
            RemoveLoadingScreenClientRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ConnectToServerRpc(ServerRpcParams serverRpcParams = default) {
        if(!IsServer) return;
        var clientId = new ulong[]{serverRpcParams.Receive.SenderClientId};
        ConnectPlayer(clientId);
    }

    [ClientRpc]
    public void SetupClientRpc(PlayerTag playerTag, ulong[] clientID, ClientRpcParams clientRpcParams = default) {
        if(playerTag == PlayerTag.PlayerTwo){
            SetCurrentPlayerTwo();
        }else if (playerTag == PlayerTag.PlayerOne){            
            SetCurrentPlayerOne();
        }
        connected = true;
    }

    [ClientRpc]
    public void RemoveLoadingScreenClientRpc(ClientRpcParams clientRpcParams = default){
        GameObject.Find("LoadingImage").SetActive(false);
        gameStart = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UseAbilityServerRpc(string abilityName, Vector3 abilityPos, PlayerTag playerTag, ServerRpcParams serverRpcParams = default) {
        if(!gameStart) return;
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
            GameObject ability = Instantiate(Utilities.GetAbilityGameObject(abilityName), abilityPos, Quaternion.identity);
            var abilityNetworkObject = ability.GetComponent<NetworkObject>();
            if(!abilityNetworkObject.IsSpawned) abilityNetworkObject.Spawn();
            ability.GetComponent<IAbility>().Use(playerTag.ToString());
            ability.tag = playerTag.ToString();
        }
    }

}