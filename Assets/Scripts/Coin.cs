using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinsValue;  //An int signifying the value of the Coins 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SettingsManager.Instance.ChangeCoins(coinsValue);

            Destroy(this.gameObject);
        }
    }
}
