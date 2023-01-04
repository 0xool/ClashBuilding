using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    private float reloadTime = 0;
    public float reloadCooldown = 10;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        reloadCooldown = this.transform.parent.GetComponent<IUnit>().GetReloadTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading){
            if(reloadTime < reloadCooldown){
                reloadTime += Time.deltaTime;
            }else{
                isReloading = false;
                reloadTime = 0;
            }
        }
    }

    public void Shoot(GameObject target, GameObject bulletPrefab, int damage) {
        BulletHandler bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation).GetComponent<BulletHandler>();
        bullet.target = target;
        bullet.enemyTag = GameManager.instance.GetEnemyTag();
        bullet.bulletDamage = damage;
        isReloading = true;
    }

    public bool CanShoot() {
        return !isReloading;
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.CompareTag(GameManager.instance.GetEnemyTag())){
            this.transform.parent.GetComponent<IUnit>().AttackEnemy(collisionInfo.gameObject);            
        }
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag(GameManager.instance.GetEnemyTag())){
            this.transform.parent.GetComponent<IUnit>().RemoveEnemyAsTarget(collisionInfo.gameObject);
        }
    }
}
