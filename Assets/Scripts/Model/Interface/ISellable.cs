using UnityEngine;
using Unity.Netcode;
public interface ISellable {
    public void Sell();
    [ServerRpc]
    public void SellServerRpc(string playerTag);
}