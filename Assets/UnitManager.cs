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
    public int movementSpeed;
    // Start is called before the first frame update
    private void Awake() {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.unitModel = new Unit(this.name, 10, movementSpeed, UnitState.MOVING, ZONE.RIGHTZONE);
        this.unitModel.hp = unitHP;
    }
    void Start()
    {
        this.moveTransformPosition = GameObject.Find("Pos1").transform;
    }

    // Update is called once per frame
    void Update()
    {    
        if(unitModel.unitState == UnitState.MOVING){
            navMeshAgent.destination = moveTransformPosition.position;
        }
    }

    void LateUpdate()
    {
        if(unitModel.hp < 0){
            Destroy(this);
        }
    }
}
