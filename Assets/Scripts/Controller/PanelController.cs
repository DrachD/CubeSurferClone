using System;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;

    [SerializeField] LevelPassedPanel _levelPassedPanel;

    [SerializeField] LevelFailedPanel _levelFailedPanel;

    /// <summary>
    /// activating the level passed panel and stopping the character
    /// </summary>
    public void LevelPassedActivate()
    {
        _playerController.StopMovement();
        _levelPassedPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// activating the level failed panel and stopping the character
    /// </summary>
    public void LevelFailedActivate()
    {
        _playerController.StopMovement();
        _levelFailedPanel.gameObject.SetActive(true);
    }
}
