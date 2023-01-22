using UnityEngine;
using Unity.Netcode;
using System.Collections;

public abstract class ClashUnitBehaviour : NetworkBehaviour{    
    private int timeToDestroy = 2;
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