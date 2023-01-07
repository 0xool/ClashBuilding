using UnityEngine;
public interface ISelectable {
    public void SelectUnit(GameObject unit);
    public void UnSelectBuilding();
    public bool SelectBuildingWithMenu();
}