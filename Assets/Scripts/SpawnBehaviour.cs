using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpawnBehaviour : MonoBehaviour, IUnit, IConstructable, ISelectable
{    
    RemoveFromTarget removeFromTarget;
    public Building buildingModel;
    private GameObject unit;
    public float offsetSpawn = 2;
    public int hp = 500;
    public float spawnInterval = 5.0f;
    public int constructionCost = 500;
    public GameObject[] units;
    private ConstructionComponent constructionComponent;
    public BuildingType buildingType = BuildingType.SPAWNER;
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

                case BuildingMode.SPAWNING:
                    constructionComponent.DisableConstructionMode();
                break;

                default:
                    break;
            }

            _buildingMode = value;
        }
    }

    private GameManager gameManager;

    void Awake() {
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
        constructionComponent.SetBuildingType(this.buildingType);
        gameManager = GameObject.FindWithTag("MainCamera").GetComponent<GameManager>();
        unit = units[0];
    }

    public void Spawn(){
        if(unit == null){
            this.buildingMode = BuildingMode.IDLE;
            return;
        }
        if(gameManager.GetPlayerResource(this.gameObject.tag) < GetUnitCost() || buildingMode != BuildingMode.SPAWNING) return;
        gameManager.UseResource(buildingModel.spawnCost, this.tag);
        var newUnit = Instantiate(unit, new Vector3(this.transform.position.x - offsetSpawn,this.transform.position.y - (this.transform.localScale.x/2 - unit.transform.localScale.x/2),this.transform.position.z), this.transform.rotation);
        newUnit.tag = this.gameObject.tag;
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
    public void RemoveEnemyAsTarget(GameObject enemy){

    }

    public void MoveBuildingByTouch(Vector3 touchLocation) {
        this.transform.position = touchLocation;
    }

    public void Build() {        
        if(constructionComponent.CanConstruct() && (gameManager.GetPlayerResource("Player2") > buildingModel.constructionCost))
        {
            if(gameManager.UseResource(buildingModel.constructionCost, "Player2")){
                this.buildingMode = BuildingMode.IDLE;
                this.tag = "Player2";
            }
            else
                Destroy(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    public void SelectUnit(GameObject unit) {
        this.unit = unit;
        UnSelectBuilding();
        this.buildingMode = BuildingMode.SPAWNING;
    }

    public void SelectBuilding() {
        GameObject.FindGameObjectWithTag("UnitMenuHandler").GetComponent<UnitMenuHandler>().CreateSelectUnitMenuItems(this.units);
    }

    public void UnSelectBuilding() {
        // hide 
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }
}