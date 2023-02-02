using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantrySpawnBuildingManager : SpawnBehaviour
{
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.buildingModel = new Building(BuildingType.SPAWNER, 0, 0, constructionCost);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(timer > spawnInterval){
            Spawn();
            timer = 0;
        }else{
            timer += Time.deltaTime;
        }
    }
}
