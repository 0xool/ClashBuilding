using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpawnBehaviour : MonoBehaviour, IUnit
{    
    public Building buildingModel;
    public GameObject unit;
    public float offsetSpawn = 2;
    public int cost = 200;
    public int hp = 500;
    public float spawnInterval = 5.0f;




    public void Spawn(){
        if(GameObject.FindWithTag("MainCamera").GetComponent<GameManager>().GetPlayerResource(this.gameObject.tag) < buildingModel.spawnCost) return;
        var newUnit = Instantiate(unit, new Vector3(this.transform.position.x - offsetSpawn,this.transform.position.y - (this.transform.localScale.x/2 - unit.transform.localScale.x/2),this.transform.position.z), this.transform.rotation);
        newUnit.tag = this.gameObject.tag;
        
    }
    public int GetHP() {
        return this.buildingModel.hp;
    }

    public void AttackEnemy(GameObject enemy){

    }
    public void InflictDamage(int bulletDamage){
        Debug.Log(this.buildingModel.hp);
        this.buildingModel.hp -= bulletDamage;
        if (this.buildingModel.hp <= 0){
            Destroy(this.gameObject);
        }
    }
    public int GetReloadTime(){
        return -1;
    }
    public void RemoveEnemyAsTarget(GameObject enemy){

    }
}