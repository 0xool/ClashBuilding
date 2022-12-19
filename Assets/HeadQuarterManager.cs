using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadQuarterManager : MonoBehaviour, IUnit
{
    private Building buidlingModel;
    public int hp = 500;
    // Start is called before the first frame update
    void Start()
    {
        buidlingModel = new Building(this.name, this.hp, BuildingType.HQ, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int GetHP(){
        return buidlingModel.hp;
    }
    public void AttackEnemy(GameObject enemy){
        return;
    }
    public void InflictDamage(int bulletDamage){
        this.buidlingModel.hp -= bulletDamage;
        if(this.buidlingModel.hp <= 0){
            //Game Over for this player
            GameObject.Find("Main Camera").GetComponent<GameManager>().GameOver(this.gameObject.tag);
        }
    }
    public int GetReloadTime(){
        return -1;
    }

    public void RemoveEnemyAsTarget(GameObject enemy){
    }
}
