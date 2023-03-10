using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : ClashUnitBehaviour, IUnit
{
    private Unit unitModel;
    private Transform moveTransformPosition;
    private NavMeshAgent navMeshAgent;
    public float movementSpeed;
    public int damage;
    public int cost;
    private List<GameObject> enemiesInRange;
    public GameObject bulletPrefab;
    public int reloadTime;
    RemoveFromTarget removeFromTarget;
    private const int middleSectionX = -20;
    private Animator animator;
    void Start()
    {
        this.animator = GetComponentInChildren<Animator>();
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.unitModel = new Unit(movementSpeed, UnitState.MOVING, ZONE.RIGHTZONE, damage, cost);
        SetUnitState(UnitState.MOVING);

        this.unitModel.cost = cost;
        enemiesInRange = new List<GameObject>();
                
        if(IsServer){
            SetupMovmentDestination();
            navMeshAgent.speed = movementSpeed;
        }

        if(IsClient){
            if(this.transform.position.x < middleSectionX)
                this.tag = GameManager.instance.PlayerTwoTag;
            else
                this.tag = GameManager.instance.PlayerOneTag;
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
            SetUnitState(UnitState.MOVING);
        }else{
            this.navMeshAgent.isStopped = true;
            SetUnitState(UnitState.ATTACKING);
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
                Shoot(gun, enemy);
            }
        }
    }

    private void Shoot(GunHandler gun, GameObject enemy) {
        animator.Play("Recoil_Arm_R");
        gun.Shoot(enemy, bulletPrefab, this.unitModel.damage);
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
            if(this.transform.position.z > -7)
                this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZonePlayerTwoLeft).transform;
            else
                this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZonePlayerTwoRight).transform;
        }else if (this.gameObject.CompareTag(GameManager.instance.PlayerTwoTag)) {
            if(this.transform.position.z > -7)
                this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZonePlayerOneLeft).transform;
            else
                this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZonePlayerOneRight).transform;
        }
    }

    void SetupMovmentDestinationHQ() {
        if(this.gameObject.CompareTag(GameManager.instance.PlayerOneTag)){
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZonePlayerTwoHQ).transform;
        }else if (this.gameObject.CompareTag(GameManager.instance.PlayerTwoTag)) {
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZonePlayerOneHQ).transform;
        }
    }

    public int GetReloadTime() {
        return this.reloadTime;
    }

    public void InflictDamage(int damage) {
        DeacreaseHp(damage);
    }

    public int GetHP() {
        return GetHP();
    }

    public int GetCost() {
        return cost;
    }
    
    public override void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        removeFromTarget(this.gameObject);
        SetUnitState(UnitState.DYING);
        //StartCoroutine(RunBeingDestroyedFunctionalityForClient(2));
    }

    public void AttackEnemy(GameObject enemy) {
        enemiesInRange.Add(enemy);
        enemy.GetComponent<IUnit>().AppendRemoveTargetDelegation(RemoveEnemyAsTarget);
        this.AppendRemoveTargetDelegation(enemy.GetComponent<IUnit>().RemoveEnemyAsTarget);
    }

    public void RemoveEnemyAsTarget(GameObject enemy){        
        SetUnitState(UnitState.MOVING);
        enemiesInRange.Remove(enemy);
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }

    public void SetUnitState(UnitState unitState) {
        if (this == null || animator == null) return;
        switch(unitState){
            case UnitState.MOVING:
                animator.Play("Walk_Slow");
                this.navMeshAgent.isStopped = false;
                break;
            case UnitState.ATTACKING:
                this.navMeshAgent.isStopped = true;
                animator.Play("Idle");
                break;
            case UnitState.DYING:
                this.navMeshAgent.isStopped = true;
                animator.Play("Die");
                break;
        }
        this.unitModel.unitState = unitState;
    }
}
