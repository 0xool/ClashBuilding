using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantrySpawnBuildingManager : MonoBehaviour, IUnit
{
    public GameObject unit;
    public Building buildingModel;
    public float spawnInterval = 5.0f;
    private float timer = 0;
    public float offsetSpawn = 2;
    public int hp = 500;
    // Start is called before the first frame update
    void Start()
    {
        this.buildingModel = new Building(this.name, hp, BuildingType.SPAWNER, 0);
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

    // Update is called once per frame
    void Update()
    {
        if(timer > spawnInterval){
            var newUnit = Instantiate(unit, new Vector3(this.transform.position.x - offsetSpawn,this.transform.position.y - (this.transform.localScale.x/2 - unit.transform.localScale.x/2),this.transform.position.z), this.transform.rotation);
            newUnit.tag = this.gameObject.tag;
            timer = 0;
        }else{
            timer += Time.deltaTime;
        }
    }
}
