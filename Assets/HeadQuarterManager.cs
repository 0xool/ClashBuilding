using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadQuarterManager : MonoBehaviour, IUnit
{
    private Building buidlingModel;
    public int hp = 500;
    RemoveFromTarget removeFromTarget;
    public int constructionCost = 500;

    // Start is called before the first frame update
    void Start()
    {
        buidlingModel = new Building(this.name, this.hp, BuildingType.HQ, 0, constructionCost);
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
        Debug.Log("Damage is being done.");
        if(this.buidlingModel.hp <= 0){
            //Game Over for this player
            GameObject.Find("Main Camera").GetComponent<GameManager>().GameOver(this.gameObject.tag);
        }
    }
    public int GetReloadTime(){
        return -1;
    }

    public void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        StartCoroutine(RunBeingDestroyedFunctionality(3));
    }
 
    IEnumerator RunBeingDestroyedFunctionality(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(this.gameObject);
    }

    public void RemoveEnemyAsTarget(GameObject enemy){
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }
}
