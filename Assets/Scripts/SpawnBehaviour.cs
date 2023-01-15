using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;


public class SpawnBehaviour : BuildingBehaviour, IUnit, ISelectable, ISellable
{    
    RemoveFromTarget removeFromTarget;
    private GameObject unit;
    public float offsetSpawn = 2;
    public int hp = 500;
    public float spawnInterval = 5.0f;
    public int constructionCost = 500;
    public GameObject[] units;

    private UnitUIManager inGameMenuPrefab;
    void Start() {
        unit = units[0];
    }

    public void Spawn(){
        if(IsClient) return;
        if(unit == null){
            this.buildingMode = BuildingMode.IDLE;
            return;
        }
        if(buildingMode != BuildingMode.SPAWNING) return;
        if(GameManager.instance.GetPlayerResource() < GetUnitCost()) return;
        GameManager.instance.UseResource(buildingModel.spawnCost);
        var newUnit = Instantiate(unit, new Vector3(this.transform.position.x - offsetSpawn,this.transform.position.y - (this.transform.localScale.x/2 - unit.transform.localScale.x/2),this.transform.position.z), this.transform.rotation);
        newUnit.tag = this.gameObject.tag;  
        newUnit.GetComponent<NetworkObject>().Spawn();

    }
    public int GetHP() {
        return this.buildingModel.hp;
    }

    public void AttackEnemy(GameObject enemy){

    }
    public void InflictDamage(int bulletDamage){
        this.buildingModel.hp -= bulletDamage;
        if (this.buildingModel.hp <= 0){
            IsBeingDestroyed();
        }
    }

    public int GetUnitCost() {
        return this.unit.GetComponent<IUnit>().GetCost();
    }

    public int GetCost() {
        return this.constructionCost;
    }
    
    public void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        removeFromTarget(this.gameObject);        
        RunDestructionAnimation();
    }
    public int GetReloadTime(){
        return -1;
    }
    public void RemoveEnemyAsTarget(GameObject enemy){

    }

    public void MoveBuildingByTouch(Vector3 touchLocation) {
        this.transform.position = touchLocation;
    }

    public void SelectUnit(GameObject unit) {
        UnSelectBuilding();
        SelectUnitServerRpc((unit == null) ? "" : unit.name);
        this.buildingMode = BuildingMode.SPAWNING;
    }

    public bool SelectBuildingWithMenu() {
        GameObject.FindGameObjectWithTag("UnitMenuHandler").GetComponent<UnitMenuHandler>().CreateSelectUnitMenuItems(this.units);
        inGameMenuPrefab = Instantiate(Utilities.GetInGameMenuUIGameObject(),this.transform.position,  Quaternion.identity).GetComponentInChildren<UnitUIManager>(); 
        inGameMenuPrefab.SetUnit(this.gameObject);

        return true;
    }

    public void UnSelectBuilding() {
        inGameMenuPrefab.GetComponentInChildren<UnitUIManager>().RemoveUI();
    }
    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }

    public void Sell(){
        GameManager.instance.IncreaseResourceValue( constructionCost / Utilities.SellRatio);
        UnSelectBuilding();
        RunSellAnimationAnimation();
    }
    protected override void SetBuildingType()
    {
        this.buildingType = BuildingType.SPAWNER;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SelectUnitServerRpc(string unitName)
    {
        if(unitName == "")
            this.unit = null;
        else{
            this.unit = Array.Find(units, unit => unit.name == unitName);
            this.unit.tag = this.tag;
            this.buildingMode = BuildingMode.SPAWNING;
        }        
    }
}