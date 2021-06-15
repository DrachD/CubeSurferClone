using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPanel : MonoBehaviour
{
    [SerializeField] Text _text;

    [SerializeField] string _baseText = "";

    public void UpdateCountCoins(int value)
    {
        _text.text = _baseText + value.ToString();
    }
}
