using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public interface IConstructable {
    public void Build();
    public void ServerBuild(string playerTag);
    public BuildingType GetBuildingType();
    [ClientRpc]
    public void SetupConstructionClientRpc(string playerTag);
}