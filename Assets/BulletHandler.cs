using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public GameObject target;
    public float bulletSpeed = 10;
    public int bulletDamage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            transform.position = Vector3.MoveTowards(transform.position, target.transform
            .position, bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.CompareTag("Player2")){
            UnitManager unitManager = collisionInfo.gameObject.GetComponent<UnitManager>();
            unitManager.InflictDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }
}
