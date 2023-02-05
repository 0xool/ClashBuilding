using UnityEngine;
using Unity.Netcode;
using System.Collections;

using UnityEngine.UI;

public abstract class ClashUnitBehaviour : NetworkBehaviour{    
    private int timeToDestroy = 2;
    private GameObject hpBarPrefab = null;
    public int hp;
    public ClashUnit clashUnit;

    protected virtual void Awake() {
        this.clashUnit = new ClashUnit(this.name, hp);
    }

    private void SetHpBar() {
        if(IsServer) return;


        if (hpBarPrefab == null) AddHpBarObject();

        this.hpBarPrefab.GetComponentsInChildren<Image>()[1].fillAmount = (float)this.clashUnit.hp / (float)hp;        
    }

    public void DeacreaseHp(int hp){
        this.clashUnit.hp -= hp;
        if (this.clashUnit.hp <= 0){
            GameObject.Destroy(hpBarPrefab);
            IsBeingDestroyed();
            RunDestructionAnimation();
            return;
        }
        SetHpBar();
    }

    public void IncreaseHp(int hp){
        this.clashUnit.hp += hp;
        SetHpBar();
    }

    public abstract void IsBeingDestroyed();

    public int GetHp(){
        return this.clashUnit.hp;
    }

    private void AddHpBarObject(){    
        hpBarPrefab = Instantiate(Utilities.GetHpBarUIGameObject(), this.transform.position,  Quaternion.identity); 
        hpBarPrefab.GetComponent<HpBarManager>().SetUnit(this.gameObject);
    }

    protected void RunDestructionAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionalityForServer(timeToDestroy));
    }

    protected void RunDestructionAnimationWithClientDestroy() {
        StartCoroutine(RunBeingDestroyedFunctionalityForClient(timeToDestroy));
    }

    protected void RunSellAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionalityForServer(timeToDestroy));
    }

    protected void RunClientSellAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionalityForClient(timeToDestroy));
    }

    protected void RunDestroyForServer() {
        StartCoroutine(RunBeingDestroyedFunctionalityForServer(timeToDestroy + 2));
    }

    protected IEnumerator RunBeingDestroyedFunctionalityForClient(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(this.gameObject);
    }
 
    protected IEnumerator RunBeingDestroyedFunctionalityForServer(int secs)
    {
        yield return new WaitForSeconds(secs);
        if(IsServer)
            Destroy(this.gameObject);
    }
    
}