using UnityEngine;
using Unity.Netcode;
public abstract class BuildingBehaviour : ClashUnitBehaviour, IConstructable, ISellable {
        public Building buildingModel;
        protected BuildingMode _buildingMode = BuildingMode.CONSTRUCTION;
        protected BuildingType buildingType = BuildingType.DEFENSE;
        protected BuildingMode buildingMode{
            get{
                return _buildingMode;
            }set{
                if(value != BuildingMode.CONSTRUCTION) constructionComponent.DisableConstructionMode();
                switch (value)
                {
                    case BuildingMode.CONSTRUCTION:                    
                        constructionComponent.EnableConstructionMode();
                    break;

                    case BuildingMode.ATTACKING:
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
    protected ConstructionComponent constructionComponent;
    void Awake()
    {
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
        SetBuildingType();
        this.buildingMode = BuildingMode.CONSTRUCTION;
    }
    public void Build() {  
        if(!IsServer){
            ServerBuild(GameManager.instance.GetCurrentPlayerTag());
            return;
        }

        if(constructionComponent.CanConstruct() && (GameManager.instance.GetPlayerResource() > buildingModel.constructionCost))
        {
            if(!GameManager.instance.IsOwnedByServer) return;

            if(GameManager.instance.UseResource(buildingModel.constructionCost)){
                switch (this.buildingType)
                {
                    case BuildingType.SPAWNER:
                        this.buildingMode = BuildingMode.IDLE;
                        break;
                    
                    case BuildingType.DEFENSE:
                        this.buildingMode = BuildingMode.ATTACKING;
                        break;
                    
                    case BuildingType.REFINERY:                        
                        GameManager.instance.IncreaseResourceIncome(GetRefineryResourceValue());
                        SetResourceObjects();   
                        break;
                    
                    default:
                        this.buildingMode = BuildingMode.IDLE;
                        break;
                }
                
                this.tag = GameManager.instance.GetCurrentPlayerTag();
            }
            else
                Destroy(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }
    public void ServerBuild(string playerTag){
        if(constructionComponent.CanConstruct() && (GameManager.instance.GetPlayerResourceWithTag(playerTag) > buildingModel.constructionCost))
        {
            if(GameManager.instance.UsePlayerResource(buildingModel.constructionCost, playerTag)){
                switch (this.buildingType)
                {
                    case BuildingType.SPAWNER:
                        this.buildingMode = BuildingMode.IDLE;
                        break;
                    
                    case BuildingType.DEFENSE:
                        this.buildingMode = BuildingMode.ATTACKING;
                        break;
                    
                    case BuildingType.REFINERY:
                        GameManager.instance.IncreaseResourceIncomeForPlayer(GetRefineryResourceValue(), playerTag);
                        SetResourceObjects();
                        break;
                    
                    default:
                        this.buildingMode = BuildingMode.IDLE;
                        break;
                }
                GameManager.instance.BuildConstructionServerRpc(TouchManager.instance.currentDragedPrefabName, this.transform.position, playerTag, buildingModel.constructionCost);
                this.tag = playerTag;
                Destroy(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    public void Sell(){
        UnSelectBuilding();
        RunClientSellAnimation();
        SellServerRpc(GameManager.instance.GetCurrentPlayerTag());
    }

    public abstract void UnSelectBuilding();

    private void SetResourceObjects() {
        this.buildingMode = BuildingMode.IDLE;
        var resourceLockOnPos = GetResourceLockOnPos();
        this.transform.position = resourceLockOnPos.position;
        Destroy(resourceLockOnPos.gameObject);
    }
    protected virtual void SetBuildingType(){
        throw new System.NotImplementedException();
    }
    protected virtual int GetRefineryResourceValue(){
        throw new System.NotImplementedException();
    }
    protected virtual Transform GetResourceLockOnPos(){
        throw new System.NotImplementedException();
    }

    public BuildingType GetBuildingType()
    {
        return this.buildingType;
    }
    [ClientRpc]
    public void SetupConstructionClientRpc(string playerTag)
    {
        this.tag = playerTag;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SellServerRpc(string playerTag){
        if(IsServer) RunDestroyForServer();
        GameManager.instance.IncreaseResourceValueForPlayer( buildingModel.constructionCost / Utilities.SellRatio, playerTag);
    }
}