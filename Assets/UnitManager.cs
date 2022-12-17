using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    private Unit unitModel;
    [SerializeField] private Transform moveTransformPosition;
    private NavMeshAgent navMeshAgent;
    public int unitHP;
    public float movementSpeed;
    public int damage;

    // Start is called before the first frame update
    private void Awake() {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.unitModel = new Unit(this.name, movementSpeed, unitHP, UnitState.MOVING, ZONE.RIGHTZONE, damage);
        this.unitModel.hp = unitHP;
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
    }

    public void InflictDamage(int damage) {
        this.unitModel.hp -= damage;
    }

    public int getHP() {
        return unitModel.hp;
    }

    void LateUpdate()
    {
        if(unitModel.hp <= 0){
            Destroy(this.gameObject);
        }
    }
}
