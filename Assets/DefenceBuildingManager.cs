using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceBuildingManager : MonoBehaviour
{
    public Building buildingModel;
    public int hp;
    public int damage;
    private List<GameObject> enemiesInRange;
    public GameObject bulletPrefab;
    private float reloadTime = 0;
    public float reloadCooldown = 10;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Awake()
    {
        enemiesInRange = new List<GameObject>();
    }
    void Start()
    {
       buildingModel = new Building(this.name, hp, BuildingType.DEFENSE, damage);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesInRange.Count > 0 && !isReloading){
            // We Attack
            BulletHandler bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation).GetComponent<BulletHandler>();
            bullet.target = enemiesInRange[0];
            bullet.bulletDamage = this.damage;
            if(enemiesInRange[0].GetComponent<UnitManager>().getHP() - damage <= 0){
                enemiesInRange.RemoveAt(0);
            }

            isReloading = true;
        }

        if(isReloading){
            if(reloadTime < reloadCooldown){
                reloadTime += Time.deltaTime;
            }else{
                isReloading = false;
                reloadTime = 0;
            }
        }

    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player2")){
            enemiesInRange.Add(collisionInfo.gameObject);
        }
    }

        void OnTriggerExit(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player2")){
            enemiesInRange.Remove(collisionInfo.gameObject);
        }
    }
}
