using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public enum GunType {
        UNIT,
        BUILDING
    }

    public GunType gunType;
    UnitManager unitManager;
    DefenceBuildingManager buildingManager;
    private float reloadTime = 0;
    public float reloadCooldown = 10;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        if(gunType == GunType.UNIT){
            unitManager = this.gameObject.GetComponentInParent<UnitManager>();
        }else{
            buildingManager = this.gameObject.GetComponentInParent<DefenceBuildingManager>();
        }

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
        bullet.enemyTag = GetEnemyTag();
        bullet.bulletDamage = damage;
        isReloading = true;
    }

    public bool CanShoot() {
        return !isReloading;
    }

    string GetEnemyTag() {
        if(this.transform.parent.gameObject.CompareTag("Player1")){
            return "Player2";
        }

        return "Player1";
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.CompareTag(GetEnemyTag())){
            if(this.gunType == GunType.UNIT){
                unitManager.AttackEnemy(collisionInfo.gameObject);
            }

            if(this.gunType == GunType.BUILDING){
                buildingManager.AttackEnemy(collisionInfo.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag(GetEnemyTag())){
            if(this.gunType == GunType.UNIT){
                unitManager.RemoveEnemy(collisionInfo.gameObject);
            }

            if(this.gunType == GunType.BUILDING){
                buildingManager.RemoveEnemy(collisionInfo.gameObject);
            }
        }
    }
}
