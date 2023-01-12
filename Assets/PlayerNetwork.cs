using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : NetworkSingletonBehaviour<PlayerNetwork>
{
    // [ServerRpc]
    // public void BuildConstructionServerRpc(string constructionName, Vector3 constructionPos, string playerTag, ServerRpcParams serverRpcParams = default) 
    // {
    //     Debug.Log("Omg this WORKS!!");
    //     if(!gameStart) return;
    //     var clientId = serverRpcParams.Receive.SenderClientId;
    //     if (NetworkManager.ConnectedClients.ContainsKey(clientId))
    //     {
    //         var client = NetworkManager.ConnectedClients[clientId];
    //         // Do things for this client
    //         GameObject construction = Utilities.GetConstructionGameObject(constructionName);


    //     }
    // }
}
