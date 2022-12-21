using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceBuildingManager : MonoBehaviour, IUnit
{
    public Building buildingModel;
    public int hp;
    public int damage;
    private List<GameObject> enemiesInRange;
    public GameObject bulletPrefab;
    public int reloadTime = 0;
    RemoveFromTarget removeFromTarget;


    // Start is called before the first frame update
    void Awake()
    {
        enemiesInRange = new List<GameObject>();
    }
    void Start()
    {
       buildingModel = new Building(this.name, hp, BuildingType.DEFENSE, damage);
    }

    // Update is called once per frame
    void Update()
    {
            if(buildingModel.buildingState == BuildingState.DYING){
                return;
            }

            if(enemiesInRange.Count > 0){
                GameObject enemy = enemiesInRange[0];
                var gun = this.gameObject.GetComponentInChildren<GunHandler>();

                if(gun.CanShoot()){
                    gun.Shoot(enemy, bulletPrefab, this.buildingModel.damage);
                    if(enemy.GetComponent<IUnit>().GetHP() - damage <= 0 ){
                        RemoveEnemyAsTarget(enemy);
                    }
                }
            }
    }

    public int GetReloadTime() {
        return this.reloadTime;
    }

    void LateUpdate() {
        if(this.buildingModel.hp <= 0){
            IsBeingDestroyed();
        }    
    }

    public void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        removeFromTarget(this.gameObject);
        buildingModel.buildingState = BuildingState.DYING;
        StartCoroutine(RunBeingDestroyedFunctionality(3));
    }
 
    IEnumerator RunBeingDestroyedFunctionality(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(this.gameObject);
    }

    public int GetHP() {
        return this.buildingModel.hp;
    }

    public void AttackEnemy(GameObject enemy) {
        enemiesInRange.Add(enemy);
        enemy.GetComponent<IUnit>().AppendRemoveTargetDelegation(RemoveEnemyAsTarget);
        this.AppendRemoveTargetDelegation(enemy.GetComponent<IUnit>().RemoveEnemyAsTarget);
    }

    public void RemoveEnemyAsTarget(GameObject enemy) {
        enemiesInRange.Remove(enemy);
    }

    public void InflictDamage(int damage) {
        this.buildingModel.hp -= damage;
    }

    string GetEnemyTag() {
        if(this.gameObject.CompareTag("Player1")){
            return "Player2";
        }

        return "Player1";
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }
}
