using UnityEngine;
using System;

public class PlayerCheck : MonoBehaviour
{
    public Action OnLevelPassedActivateEvent;

    [SerializeField] PanelController _panelController;

    private void Awake()
    {
        OnLevelPassedActivateEvent += _panelController.LevelPassedActivate;
    }
}
