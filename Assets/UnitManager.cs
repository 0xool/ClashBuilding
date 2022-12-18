using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour, IUnit
{
    private Unit _unitModel;
    private Unit unitModel{
        get{
            return _unitModel;
        }set{
            if(_unitModel != null){
                if(value.unitState == UnitState.MOVING && _unitModel.unitState == UnitState.ATTACKING){
                    navMeshAgent.isStopped = true;
                    Debug.Log("ATTACIKNG");
                }

                if(value.unitState == UnitState.ATTACKING && _unitModel.unitState == UnitState.MOVING){
                    navMeshAgent.isStopped = false;
                    Debug.Log("MOVING");
                }
            }

            

            _unitModel = value;
        }
    }

    [SerializeField] private Transform moveTransformPosition;
    private NavMeshAgent navMeshAgent;
    public int unitHP;
    public float movementSpeed;
    public int damage;
    private List<GameObject> enemiesInRange;
    public GameObject bulletPrefab;
    public int ReloadTime;


    // Start is called before the first frame update
    private void Awake() {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.unitModel = new Unit(this.name, movementSpeed, unitHP, UnitState.MOVING, ZONE.RIGHTZONE, damage);
        this.unitModel.hp = unitHP;

        enemiesInRange = new List<GameObject>();
    }
    void Start()
    {
        if(moveTransformPosition == null)
            this.moveTransformPosition = GameObject.Find("Pos1").transform;

        navMeshAgent.speed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {    
        if(unitModel.unitState == UnitState.MOVING){
            navMeshAgent.destination = moveTransformPosition.position;
        }

        if(unitModel.unitState == UnitState.ATTACKING){  
            this.navMeshAgent.isStopped = true;          
            GameObject enemy = enemiesInRange[0];
            var gun = this.gameObject.GetComponentInChildren<GunHandler>();

            if(gun.CanShoot()){
                gun.Shoot(enemy, bulletPrefab, this.unitModel.damage);
                if(enemy.GetComponent<IUnit>().GetHP() - damage <= 0 ){
                    RemoveEnemy(enemy);
                }
            }

        }
    }

    public int GetReloadTime() {
        return this.ReloadTime;
    }

    string GetEnemyTag() {
        if(this.gameObject.CompareTag("Player1")){
            return "Player2";
        }

        return "Player1";
    }

    public void InflictDamage(int damage) {
        this.unitModel.hp -= damage;
    }

    public int GetHP() {
        return unitModel.hp;
    }

    void LateUpdate()
    {
        if(unitModel.hp <= 0){
            Destroy(this.gameObject);
        }
    }

    public void AttackEnemy(GameObject enemy) {
        this.unitModel.unitState = UnitState.ATTACKING;
        enemiesInRange.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy){
        this.unitModel.unitState = UnitState.MOVING;
        enemiesInRange.Remove(enemy);
        if(enemiesInRange.Count == 0){
            this.navMeshAgent.isStopped = false;
            this.unitModel.unitState = UnitState.MOVING;
        }
    }

}
