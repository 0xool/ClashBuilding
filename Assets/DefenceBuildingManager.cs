using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DefenceBuildingManager : BuildingBehaviour, IUnit, ISellable, ISelectable, IUpgradeable
{
    private GameObject inGameMenuPrefab;
    private int upgradeLevel{
        get{
            return _upgradeLevel;
        }set{
            // MARK : ADD DAMAGE UPGRADE INCREAMENT
            buildingModel.damage += 20;
            upgradeLevel = value;
        }
    }
    private int _upgradeLevel = 1;
    public int hp;
    public int damage;
    private List<GameObject> enemiesInRange;
    public GameObject bulletPrefab;
    public int reloadTime = 0;
    public int constructionCost = 500;
    RemoveFromTarget removeFromTarget;
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
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

    public int GetHP() {
        return this.buildingModel.hp;
    }

    public int GetCost() {
        return this.constructionCost;
    }

    public void AttackEnemy(GameObject enemy) {        
        if(enemy.GetComponent<DefenceBuildingManager>() == null){
            enemiesInRange.Add(enemy);
            enemy.GetComponent<IUnit>().AppendRemoveTargetDelegation(RemoveEnemyAsTarget);
            this.AppendRemoveTargetDelegation(enemy.GetComponent<IUnit>().RemoveEnemyAsTarget);
        }
    }

    public void RemoveEnemyAsTarget(GameObject enemy) {
        enemiesInRange.Remove(enemy);
    }

    public void InflictDamage(int damage) {
        this.buildingModel.hp -= damage;
    }

    public void ServerBuild() {           
        if(constructionComponent.CanConstruct() && (GameManager.instance.GetPlayerResource() > buildingModel.constructionCost))
        {
            if(GameManager.instance.UseResource(buildingModel.constructionCost)){
                this.buildingMode = BuildingMode.ATTACKING;
                this.tag = GameManager.instance.currentPlayer;
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

    public void Sell(){
        GameManager.instance.IncreaseResourceValue( constructionCost / Utilities.SellRatio);
        UnSelectBuilding();
        RunSellAnimationAnimation();
    }

    public void SelectUnit(GameObject unit) {
        return;
    }

    public bool SelectBuildingWithMenu() {
        inGameMenuPrefab = Instantiate(Utilities.GetInGameMenuUIGameObject(),this.transform.position, Quaternion.identity); 
        inGameMenuPrefab.GetComponentInChildren<UnitUIManager>().SetUnit(this.gameObject);

        return false;
    }

    public void UnSelectBuilding() {
        if (inGameMenuPrefab) inGameMenuPrefab.GetComponentInChildren<UnitUIManager>().RemoveUI();
    }

    public void Upgrade()
    {
        upgradeLevel++;
    }

    protected override void SetBuildingType()
    {
        this.buildingType = BuildingType.DEFENSE;
    }
    [ServerRpc(RequireOwnership = false)]
    public void SelectUnitServerRpc(string unitName)
    {
        throw new System.NotImplementedException();
    }
}
