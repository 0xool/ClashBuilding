using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConstructable {
    public void Build();
    public void ServerBuild(string playerTag);
    public BuildingType GetBuildingType();
}