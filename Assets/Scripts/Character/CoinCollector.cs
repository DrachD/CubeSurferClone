using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] InfoPanel _infoPanel;

    public int _countCoins = 0;

    /// <summary>
    /// update the number of coins and update the number of coins in UI
    /// </summary>
    public void UpdateCountCoins(int value)
    {
        _countCoins += value;
        _infoPanel?.OnUpdateCoinsEvent(_countCoins);
    }
}
