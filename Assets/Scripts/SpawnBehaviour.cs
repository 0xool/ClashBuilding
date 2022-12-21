using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpawnBehaviour : MonoBehaviour, IUnit
{    
    RemoveFromTarget removeFromTarget;
    public Building buildingModel;
    public GameObject unit;
    public float offsetSpawn = 2;
    public int cost = 200;
    public int hp = 500;
    public float spawnInterval = 5.0f;
    private BuildingMode _buildingMode = BuildingMode.CONSTRUCTION;
    private ConstructionComponent constructionComponent;
    private BuildingMode buildingMode{
        get{
            return _buildingMode;
        }set{
            switch (value)
            {
                case BuildingMode.CONSTRUCTION:                    
                    constructionComponent.EnableConstructionMode();
                break;
                
                case BuildingMode.DESTRUCTION:

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

    void Awake() {
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
    }

    public void Spawn(){
        GameManager gameManager = GameObject.FindWithTag("MainCamera").GetComponent<GameManager>();
        if(gameManager.GetPlayerResource(this.gameObject.tag) < buildingModel.spawnCost) return;
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
        if(constructionComponent.CanConstruct()){
            this.buildingMode = BuildingMode.SPAWNING;
        }else{
            Destroy(this.gameObject);
        }
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }
}