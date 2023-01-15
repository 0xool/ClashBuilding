using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : ClashUnitBehaviour, IUnit
{
    private Unit unitModel;
    private Transform moveTransformPosition;
    private NavMeshAgent navMeshAgent;
    public int unitHP;
    public float movementSpeed;
    public int damage;
    public int cost;
    private List<GameObject> enemiesInRange;
    public GameObject bulletPrefab;
    public int reloadTime;
    RemoveFromTarget removeFromTarget;

    void Start()
    {
        
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.unitModel = new Unit(this.name, movementSpeed, unitHP, UnitState.MOVING, ZONE.RIGHTZONE, damage, cost);
        this.unitModel.hp = unitHP;
        this.unitModel.cost = cost;

        enemiesInRange = new List<GameObject>();
                
        if(IsServer){
            SetupMovmentDestination();
            navMeshAgent.speed = movementSpeed;
        }

        if(IsClient){
            this.tag = GameManager.instance.currentPlayer;
        }
    }

    // Update is called once per frame
    void Update()
    {   

        if(unitModel.unitState == UnitState.DYING){
            return;
        }

        if(enemiesInRange.Count == 0){
            this.navMeshAgent.isStopped = false;
            this.unitModel.unitState = UnitState.MOVING;
        }else{
            this.navMeshAgent.isStopped = true;
            this.unitModel.unitState = UnitState.ATTACKING;
        }

        MovmentHandling();

        if(unitModel.unitState == UnitState.ATTACKING){  
            this.navMeshAgent.isStopped = true;          
            GameObject enemy = enemiesInRange[0];
            if(enemy == null){
                RemoveEnemyAsTarget(enemy);
                return;
            }
            var gun = this.gameObject.GetComponentInChildren<GunHandler>();

            if(gun.CanShoot()){
                gun.Shoot(enemy, bulletPrefab, this.unitModel.damage);
            }

        }
    }

    private void MovmentHandling() {
        if(IsClient) return;
        if(unitModel.unitState == UnitState.MOVING){
            float deltaPosToDestinationX = Mathf.Abs(this.transform.position.x - moveTransformPosition.position.x);
            float deltaPosToDestinationY =  Mathf.Abs(this.transform.position.y - moveTransformPosition.position.y);
            if(deltaPosToDestinationX + deltaPosToDestinationY < 3){
                SetupMovmentDestinationHQ();
            }
            navMeshAgent.destination = moveTransformPosition.position;
        }
    }

    void SetupMovmentDestination() {
        if(this.gameObject.CompareTag(GameManager.instance.PlayerOneTag)){
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneEnemyLeft).transform;
        }else if (this.gameObject.CompareTag(GameManager.instance.PlayerTwoTag)) {
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneFriendlyLeft).transform;
        }
    }

    void SetupMovmentDestinationHQ() {
        if(this.gameObject.CompareTag(GameManager.instance.PlayerOneTag)){
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneEnemyHQ).transform;
        }else if (this.gameObject.CompareTag(GameManager.instance.PlayerTwoTag)) {
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneFriendlyHQ).transform;
        }
    }

    public int GetReloadTime() {
        return this.reloadTime;
    }

    public void InflictDamage(int damage) {
        this.unitModel.hp -= damage;
    }

    public int GetHP() {
        return unitModel.hp;
    }

    public int GetCost() {
        return cost;
    }

    void LateUpdate()
    {
        if(unitModel.hp <= 0){
            IsBeingDestroyed();
        }
    }

    public void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        removeFromTarget(this.gameObject);
        unitModel.unitState = UnitState.DYING;
        StartCoroutine(RunBeingDestroyedFunctionality(2));
    }

    public void AttackEnemy(GameObject enemy) {
        enemiesInRange.Add(enemy);
        enemy.GetComponent<IUnit>().AppendRemoveTargetDelegation(RemoveEnemyAsTarget);
        this.AppendRemoveTargetDelegation(enemy.GetComponent<IUnit>().RemoveEnemyAsTarget);
    }

    public void RemoveEnemyAsTarget(GameObject enemy){
        this.unitModel.unitState = UnitState.MOVING;
        enemiesInRange.Remove(enemy);
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }

}
