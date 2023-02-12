using System;
using System.Collections;
using UnityEngine;

class Energy
{
    public float Current { get; private set; }
    public float Max { get; private set; }
    public float Regen { get; private set; }

    public Energy(float max, float regen)
    {
        Max = max;
        Regen = regen;
        Current = max;
    }

    public void Update(float deltaTime)
    {
        Current = Mathf.Min(Current + Regen * deltaTime, Max);
    }

    public void Use(float amount)
    {
        Current = Mathf.Max(Current - amount, 0);
    }
}