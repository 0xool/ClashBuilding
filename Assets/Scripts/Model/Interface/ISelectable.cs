using UnityEngine;
using Unity.Netcode;
public interface ISelectable {
    public void SelectUnit(GameObject unit);
    public void UnSelectBuilding();
    public bool SelectBuildingWithMenu();
    [ServerRpc(RequireOwnership = false)]
     public void SelectUnitServerRpc(string unitName);
}