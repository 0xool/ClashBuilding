using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public GameObject target;
    public float bulletSpeed = 10;
    public int bulletDamage = 10;
    public string enemyTag;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null){
            Destroy(this.gameObject);
        }else{
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
            this.transform.LookAt(target.transform);
        }
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.CompareTag(enemyTag) && collisionInfo.gameObject == target){
            IUnit unit = collisionInfo.gameObject.GetComponent<IUnit>();
            unit.InflictDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }
}
