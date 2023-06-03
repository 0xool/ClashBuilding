using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using GameModel;
using TMPro;

public class GameManager : NetworkSingletonBehaviour<GameManager>
{
    public string PlayerOneTag = "Player1";
    public string PlayerTwoTag = "Player2";
    private Player playerOne;
    private Player playerTwo;
    private int playersConnected = 0;
    private bool gameStart = false;
    private string _currentPlayer;
    private string currentPlayer {
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
        if(gameIsOver) return;
        GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().enabled = true;
        GameObject.Find("GameOverText").GetComponent<TMP_Text>().enabled = true ;
        GameObject.Find("GameOverPlayerName").GetComponent<TMP_Text>().text = (player == PlayerOneTag) ? "Player One Won" : "Player Two Won";
    }


    public int GetPlayerResource() {
        return GetCurrentPlayer().resourceValue;
    }

    public int GetPlayerResourceWithTag(string tag) {
        return GetPlayerWithTag(tag).resourceValue;
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

    public bool UsePlayerResource(int amount, string tag) {
        var player = GetPlayerWithTag(tag);
        return (player.resourceValue > amount);
    }

    public void IncreaseResourceIncome(int resource) {
        this.GetCurrentPlayer().IncreaseResourcePower(resource);
    }

    public void IncreaseResourceIncomeForPlayer(int resource, string playerTag) {
        this.GetPlayerWithTag(playerTag).IncreaseResourcePower(resource);
    }
    

    public void IncreaseResourceValueForPlayer(int resource, string playerTag) {
        this.GetPlayerWithTag(playerTag).IncreaseResourceValue(resource);
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

    private Player GetPlayerWithTag(string tag) {
        return (tag == PlayerOneTag) ? playerOne : playerTwo;
    }

    public void SetCurrentPlayerOne() {
        currentPlayer = PlayerOneTag;
        this.transform.parent.transform.eulerAngles = new Vector3(30, -135, 0);
        TouchManager.instance.SetUpConstructionMovmentDirection();
    }

    public void SetCurrentPlayerTwo() {
        currentPlayer = PlayerTwoTag;
        TouchManager.instance.SetUpConstructionMovmentDirection();
    }

    public bool IsPlayerOne() {
        return currentPlayer == PlayerOneTag;
    }

    public bool IsPlayerTwo() {
        return currentPlayer == PlayerTwoTag;
    }

    public string GetCurrentPlayerTag(){
        return (currentPlayer == PlayerOneTag) ? PlayerOneTag : PlayerTwoTag;
    }

    public string GetEnemyTag() {
        return (currentPlayer == PlayerOneTag) ? PlayerTwoTag : PlayerOneTag;
    }

    public string GetEnemyTag(string tag) {
        return (tag == PlayerOneTag) ? PlayerTwoTag : PlayerOneTag;
    }

    [ServerRpc(RequireOwnership = false)]
    public void BuildConstructionServerRpc(string constructionName, Vector3 constructionPos, string playerTag, int amount, ServerRpcParams serverRpcParams = default) 
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
            construction.GetComponent<IConstructable>().SetupConstructionClientRpc(playerTag);
            construction.tag = playerTag;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnUnitServerRpc(string unitName, Vector3 spawnPos, string playerTag, ServerRpcParams serverRpcParams = default) 
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
            SetupClientRpc(PlayerOneTag, playerOne.cliendID, clientRpcParams);
        }else if (connectedPlayers == 2){
            playerTwo.cliendID = clientID;
            SetupClientRpc(PlayerTwoTag, playerTwo.cliendID, clientRpcParams);
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
    public void SetupClientRpc(string playerTag, ulong[] clientID, ClientRpcParams clientRpcParams = default) {
        if(playerTag == PlayerTwoTag){
            SetCurrentPlayerTwo();
        }else if (playerTag == PlayerOneTag){            
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
    public void UseAbilityServerRpc(string abilityName, Vector3 abilityPos, string playerTag, ServerRpcParams serverRpcParams = default) {
        if(!gameStart) return;
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
            GameObject ability = Instantiate(Utilities.GetAbilityGameObject(abilityName), abilityPos, Quaternion.identity);
            var abilityNetworkObject = ability.GetComponent<NetworkObject>();
            if(!abilityNetworkObject.IsSpawned) abilityNetworkObject.Spawn();
            ability.GetComponent<IAbility>().Use(playerTag);
            ability.tag = playerTag;
        }
    }

}