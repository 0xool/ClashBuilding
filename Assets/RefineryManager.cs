using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RefineryManager : BuildingBehaviour, IUnit, ISelectable, IUpgradeable
{
    private GameObject inGameMenuPrefab;
    private bool onResource = false;
    public int HP = 300;
    private enum RefineryState
    {
        NotBuilt,
        Built,
    }
    public int constructionCost;
    private Transform resourceLockOnPos;
    public int upgradeCost;
    private int upgradeLevel = 1;
    private int _resourceValue = 50;
    public  int resourceValue{
        get{
            if(upgradeLevel == 1) return _resourceValue;
            if(upgradeLevel == 2) return Convert.ToInt32(_resourceValue * 1.5);
            if(upgradeLevel == 3) return _resourceValue * 2;

            throw new Exception();
        }set{
            _resourceValue = value;
        }

        
    }
    RemoveFromTarget removeFromTarget;
    private void Start() {
        this.buildingModel = new Building(BuildingType.REFINERY, 0, 0, constructionCost);
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
        
        this.buildingModel.constructionCost = constructionCost; 
        this.buildingModel.upgradeCost = upgradeCost;    
    }

    public int GetHP() {
        return this.clashUnit.hp;
    }

    public void AttackEnemy(GameObject enemy){

    }
    public void InflictDamage(int bulletDamage){
        this.DeacreaseHp(bulletDamage);
    }

    public int GetUnitCost() {
        return -1;
    }

    public int GetCost() {
        return this.buildingModel.constructionCost;
    }
    
    public override void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        removeFromTarget(this.gameObject);
        GameManager.instance.DescreaseResourceIncome(this.resourceValue);
    } 

    public int GetReloadTime(){
        
        return -1;
    }

    public void SelectUnit(GameObject unit) {
        UnSelectBuilding();
    }

    public bool SelectBuildingWithMenu() {
        inGameMenuPrefab = Instantiate(Utilities.GetInGameMenuUIGameObject(),this.transform.position, Quaternion.identity); 
        inGameMenuPrefab.GetComponentInChildren<UnitUIManager>().SetUnit(this.gameObject);

        return false;
    }

    public override void UnSelectBuilding() {
        if (inGameMenuPrefab) inGameMenuPrefab.GetComponentInChildren<UnitUIManager>().RemoveUI();
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }
    public void RemoveEnemyAsTarget(GameObject enemy){
        return;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Resource")){
            this.resourceLockOnPos = other.transform;
            this.onResource = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Resource")){
            this.onResource = true;
        }
    }

    public void Upgrade(){
        this.upgradeLevel += 1;
    }

    protected override void SetBuildingType()
    {
        this.buildingType = BuildingType.REFINERY;
    }

    protected override int GetRefineryResourceValue(){
        return this.resourceValue;
    }

    protected override Transform GetResourceLockOnPos(){
        return this.resourceLockOnPos;
    }
    
    [ServerRpc]
    public void SelectUnitServerRpc(string unitName)
    {
        throw new NotImplementedException();
    }
}
