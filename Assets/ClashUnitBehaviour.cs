using UnityEngine;
using Unity.Netcode;
using System.Collections;

public abstract class ClashUnitBehaviour : NetworkBehaviour{    
    private int timeToDestroy = 2;
    protected void RunDestructionAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionality(timeToDestroy));
    }

    protected void RunDestructionAnimationWithClientDestroy() {
        StartCoroutine(RunBeingDestroyedFunctionality(timeToDestroy, true));
    }

    protected void RunSellAnimationAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionality(timeToDestroy));
    }

    protected void RunDestroyForServer() {
        StartCoroutine(RunBeingDestroyedFunctionality(timeToDestroy + 2, true));
    }
 
    protected IEnumerator RunBeingDestroyedFunctionality(int secs, bool isNonServerDestroy = false)
    {
        yield return new WaitForSeconds(secs);
        if(IsServer || isNonServerDestroy)
            Destroy(this.gameObject);
    }
    
}