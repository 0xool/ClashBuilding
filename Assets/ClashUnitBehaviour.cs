using UnityEngine;
using Unity.Netcode;
using System.Collections;

public abstract class ClashUnitBehaviour : NetworkBehaviour{    
    protected void RunDestructionAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionality(2));
    }

    protected void RunSellAnimationAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionality(2));
    }
 
    protected IEnumerator RunBeingDestroyedFunctionality(int secs)
    {
        yield return new WaitForSeconds(secs);
        if(IsServer)
            Destroy(this.gameObject);
    }
    
}