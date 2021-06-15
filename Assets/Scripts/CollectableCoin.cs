using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
    // count of points for selecting a coin
    [SerializeField] IntVariable _countCoin;
    
    /// <summary>
    /// when colliding with a coin, 
    /// update the number of coins in UI and destroy the coin itself
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        CoinCollector coinCollector = other.GetComponent<CoinCollector>();

        if (coinCollector)
        {
            coinCollector.UpdateCountCoins(_countCoin.value);
            Destroy(gameObject);
        }
    }
}
