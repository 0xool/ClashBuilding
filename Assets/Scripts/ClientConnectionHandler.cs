using Unity.Netcode;
using UnityEngine;
using System;
using System.Collections.Generic;

    public class ClientConnectionHandler : NetworkBehaviour
    {
        public List<uint> AlternatePlayerPrefabs;

        public void SetClientPlayerPrefab(int index)
        {
            if (index > AlternatePlayerPrefabs.Count)
            {
                Debug.LogError($"Trying to assign player prefab index of {index} when there are onlky {AlternatePlayerPrefabs.Count} entries!");
                return;
            }
            if (NetworkManager.IsListening || IsSpawned)
            {
                Debug.LogError("This needs to be set this prior to connecting!");
                return;
            }
            NetworkManager.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(index);
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.ConnectionApprovalCallback = ConnectionApprovalCallback;
            }
        }

        private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.LogError("Player Has Connected");
            GameManager.instance.ConnectedPlayerSetup();
            
            var playerPrefabIndex = System.BitConverter.ToInt32(request.Payload);
            if (AlternatePlayerPrefabs.Count < playerPrefabIndex)
            {
                response.PlayerPrefabHash = AlternatePlayerPrefabs[playerPrefabIndex];
            }
            else
            {
                Debug.LogError($"Client provided player prefab index of {playerPrefabIndex} when there are onlky {AlternatePlayerPrefabs.Count} entries!");
                return;
            }
            // Continue filling out the response
        }
    }