using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IUnit, ISelectable, IConstructable
{
    private bool onResource = false;
    public int HP = 300;
    private ConstructionComponent constructionComponent;
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
    private Building buildingModel;
    RemoveFromTarget removeFromTarget;
    public BuildingType buildingType = BuildingType.REFINERY;
    public BuildingMode _buildingMode = BuildingMode.CONSTRUCTION;
    public BuildingMode buildingMode{
        get{
            return _buildingMode;
        }set{
            
            if(value != BuildingMode.CONSTRUCTION){
                
            }

            switch (value)
            {
                case BuildingMode.CONSTRUCTION:                    
                    constructionComponent.EnableConstructionMode();
                break;
                
                case BuildingMode.DESTRUCTION:
                break;

                case BuildingMode.IDLE:
                    constructionComponent.DisableConstructionMode();
                break;

                default:
                    constructionComponent.DisableConstructionMode();
                    break;
            }

            _buildingMode = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        this.buildingModel = new Building(this.name, HP, BuildingType.REFINERY, 0, 0, constructionCost);
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
        constructionComponent.SetBuildingType(this.buildingType);
    }

    private void Start() {
        this.buildingModel.constructionCost = constructionCost; 
        this.buildingModel.upgradeCost = upgradeCost;    
    }

    // Update is called once per frame
    void Update()
    {
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
        return -1;
    }

    public int GetCost() {
        return this.buildingModel.constructionCost;
    }
    
    public void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        removeFromTarget(this.gameObject);
        GameManager.instance.DescreaseResourceIncome(this.resourceValue);
        StartCoroutine(RunBeingDestroyedFunctionality(2));
    }

        IEnumerator RunBeingDestroyedFunctionality(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(this.gameObject);
    }   

    public int GetReloadTime(){
        return -1;
    }

    public void SelectUnit(GameObject unit) {
        UnSelectBuilding();
    }

    public bool SelectBuildingWithMenu() {
        return false;
    }

    public void UnSelectBuilding() {
        // hide 
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }

        public void Build() {        
        if(constructionComponent.CanConstruct() && (GameManager.instance.GetPlayerResource() > buildingModel.constructionCost) && onResource)
        {
            if(GameManager.instance.UseResource(buildingModel.constructionCost)){
                GameManager.instance.IncreaseResourceIncome(this.resourceValue);
                this.tag = GameManager.instance.currentPlayer;
                this.transform.position = resourceLockOnPos.position;
                Destroy(resourceLockOnPos.gameObject);
            }
            else
                Destroy(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
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
    
}
