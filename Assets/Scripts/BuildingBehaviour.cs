using UnityEngine;
public abstract class BuildingBehaviour : ClashUnitBehaviour, IConstructable {
        public Building buildingModel;
        protected BuildingMode _buildingMode = BuildingMode.CONSTRUCTION;
        protected BuildingType buildingType = BuildingType.DEFENSE;
        protected BuildingMode buildingMode{
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
    protected ConstructionComponent constructionComponent;
    void Awake()
    {
        constructionComponent = this.GetComponentInChildren<ConstructionComponent>();
        constructionComponent.EnableConstructionMode();
        constructionComponent.SetBuildingType(this.buildingType);
        SetBuildingType();
        this.buildingMode = BuildingMode.CONSTRUCTION;
    }
    public void Build() {  
        if(!IsServer){
            ServerBuild(GameManager.instance.currentPlayer);
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
                
                this.tag = GameManager.instance.currentPlayer;
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
                GameManager.instance.BuildConstructionServerRpc(this.name, this.transform.position, playerTag);
                this.tag = playerTag;
            }
            else
                Destroy(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    private void SetResourceObjects() {
        this.buildingMode = BuildingMode.IDLE;
        var resourceLockOnPos = GetResourceLockOnPos();
        this.transform.position = resourceLockOnPos.position;
        Destroy(resourceLockOnPos.gameObject);
    }
    protected abstract void SetBuildingType();
    protected virtual int GetRefineryResourceValue(){
        throw new System.NotImplementedException();
    }
    protected virtual Transform GetResourceLockOnPos(){
        throw new System.NotImplementedException();
    }


}