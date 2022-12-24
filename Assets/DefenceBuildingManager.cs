using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceBuildingManager : MonoBehaviour, IUnit, IConstructable
{
    public Building buildingModel;
    public int hp;
    public int damage;
    private List<GameObject> enemiesInRange;
    public GameObject bulletPrefab;
    public int reloadTime = 0;
    public int constructionCost = 500;
    RemoveFromTarget removeFromTarget;
    private ConstructionComponent constructionComponent;
    private GameManager gameManager;
    public BuildingType buildingType = BuildingType.DEFENSE;
    private BuildingMode _buildingMode = BuildingMode.CONSTRUCTION;
    private BuildingMode buildingMode{
        get{
            return _buildingMode;
        }set{
            switch (value)
            {
                case BuildingMode.CONSTRUCTION:                    
                    constructionComponent.EnableConstructionMode();
                break;

                case BuildingMode.ATTACKING:
                    constructionComponent.DisableConstructionMode();
                    break;
                
                case BuildingMode.DESTRUCTION:
                    // Animate
                break;

                default:
                    break;
            }

            _buildingMode = value;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        enemiesInRange = new List<GameObject>();
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
        constructionComponent.SetBuildingType(this.buildingType);
        gameManager = GameObject.FindWithTag("MainCamera").GetComponent<GameManager>();
    }
    void Start()
    {
       buildingModel = new Building(this.name, hp, BuildingType.DEFENSE, damage, constructionCost);
    }

    // Update is called once per frame
    void Update()
    {
            if(buildingModel.buildingState == BuildingState.DYING){
                return;
            }

            if(enemiesInRange.Count > 0){
                GameObject enemy = enemiesInRange[0];
                var gun = this.gameObject.GetComponentInChildren<GunHandler>();

                if(gun.CanShoot()){
                    gun.Shoot(enemy, bulletPrefab, this.buildingModel.damage);
                    if(enemy.GetComponent<IUnit>().GetHP() - damage <= 0 ){
                        RemoveEnemyAsTarget(enemy);
                    }
                }
            }
    }

    public int GetReloadTime() {
        return this.reloadTime;
    }

    void LateUpdate() {
        if(this.buildingModel.hp <= 0){
            IsBeingDestroyed();
        }    
    }

    public void IsBeingDestroyed() {
        this.gameObject.tag = "BeingDestroyed";
        removeFromTarget(this.gameObject);
        buildingModel.buildingState = BuildingState.DYING;
        StartCoroutine(RunBeingDestroyedFunctionality(3));
    }
 
    IEnumerator RunBeingDestroyedFunctionality(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(this.gameObject);
    }

    public int GetHP() {
        return this.buildingModel.hp;
    }

    public void AttackEnemy(GameObject enemy) {
        enemiesInRange.Add(enemy);
        enemy.GetComponent<IUnit>().AppendRemoveTargetDelegation(RemoveEnemyAsTarget);
        this.AppendRemoveTargetDelegation(enemy.GetComponent<IUnit>().RemoveEnemyAsTarget);
    }

    public void RemoveEnemyAsTarget(GameObject enemy) {
        enemiesInRange.Remove(enemy);
    }

    public void InflictDamage(int damage) {
        this.buildingModel.hp -= damage;
    }

    string GetEnemyTag() {
        if(this.gameObject.CompareTag("Player1")){
            return "Player2";
        }

        return "Player1";
    }

    public void Build() {           
        if(constructionComponent.CanConstruct() && (gameManager.GetPlayerResource("Player2") > buildingModel.constructionCost))
        {
            Debug.Log("WTF");
            if(gameManager.UseResource(buildingModel.constructionCost, "Player2")){
                Debug.Log("WTF1");
                this.buildingMode = BuildingMode.ATTACKING;
                this.tag = "Player2";
            }
            else
                Destroy(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget) {
        this.removeFromTarget += removeFromTarget;
    }
}
