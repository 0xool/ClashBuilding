using UnityEngine;
using Unity.Netcode;
public class SingletonBehaviour<T> : MonoBehaviour where T: SingletonBehaviour<T>
{
    public static T instance { get; protected set; }
 
    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            instance = (T)this;
        }
    }
}


public class NetworkSingletonBehaviour<T> : NetworkBehaviour where T: NetworkSingletonBehaviour<T>
{
    public static T instance { get; protected set; }
 
    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            instance = (T)this;
        }
    }
}