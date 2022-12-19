using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour, IUnit
{
    private Unit unitModel;

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
        SetupMovmentDestination();

        navMeshAgent.speed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {   
        if(unitModel.unitState == UnitState.MOVING){
            float deltaPosToDestinationX = Mathf.Abs(this.transform.position.x - moveTransformPosition.position.x);
            float deltaPosToDestinationY =  Mathf.Abs(this.transform.position.y - moveTransformPosition.position.y);
            if(deltaPosToDestinationX + deltaPosToDestinationY < 3){
                SetupMovmentDestinationHQ();
            }
            navMeshAgent.destination = moveTransformPosition.position;
        }

        if(unitModel.unitState == UnitState.ATTACKING){  
            this.navMeshAgent.isStopped = true;          
            GameObject enemy = enemiesInRange[0];
            if(enemy == null){
                RemoveEnemy(enemy);
                return;
            }
            var gun = this.gameObject.GetComponentInChildren<GunHandler>();

            if(gun.CanShoot()){
                gun.Shoot(enemy, bulletPrefab, this.unitModel.damage);
                   if(enemy.GetComponent<IUnit>().GetHP() - damage <= 0){
                        RemoveEnemy(enemy);
                    }
            }

        }
    }

    void SetupMovmentDestination() {
        if(this.gameObject.CompareTag("Player1")){
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneEnemyLeft).transform;
        }else if (this.gameObject.CompareTag("Player2")) {
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneFriendlyLeft).transform;
        }
    }

        void SetupMovmentDestinationHQ() {
        if(this.gameObject.CompareTag("Player1")){
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneEnemyHQ).transform;
        }else if (this.gameObject.CompareTag("Player2")) {
            this.moveTransformPosition = GameObject.Find(UnitMovmentLocation.ZoneFriendlyHQ).transform;
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
