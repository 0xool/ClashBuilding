using UnityEngine;
using System.Collections;

public abstract class ClashUnitBehaviour : MonoBehaviour{    
    protected void RunDestructionAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionality(2));
    }

    protected void RunSellAnimationAnimation() {
        StartCoroutine(RunBeingDestroyedFunctionality(2));
    }
 
    protected IEnumerator RunBeingDestroyedFunctionality(int secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(this.gameObject);
    }
    
}