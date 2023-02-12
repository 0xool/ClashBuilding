using UnityEngine;
using System;
using System.Collections;


public class AbilityManager : MonoBehaviour, IAbility {

    private Ability ability;
    public void Use() {
        if (ability.isReady) {
            ability.Cast();
        }
    }

}
