using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfoPanel : MonoBehaviour
{
    public Action<int> OnUpdateCoinsEvent;

    [SerializeField] CoinPanel _coinPanel;

    private void OnEnable()
    {
        OnUpdateCoinsEvent += _coinPanel.UpdateCountCoins;
    }

    private void OnDisable()
    {
        OnUpdateCoinsEvent -= _coinPanel.UpdateCountCoins;
    }
}
