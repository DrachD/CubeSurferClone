using UnityEngine;
using System;

public class Obstacle : MonoBehaviour
{
    public Action OnLevelFailedEvent;

    [SerializeField] PanelController _panelController;

    private BoxCollider _boxCollider;

    public CollectedCubes collectedCubes;

    private CubeCollector cubeCollector;

    private void Awake()
    {
        _panelController = GameObject.Find("Panel Controller").GetComponent<PanelController>();
        OnLevelFailedEvent += _panelController.LevelFailedActivate;
    }

    private void Start()
    {
        //heightPositionY = (int) transform.position.y;
        _boxCollider = GetComponent<BoxCollider>();
        collectedCubes = GameObject.Find("Collected Cubes").GetComponent<CollectedCubes>();
        cubeCollector = GameObject.Find("Collector").GetComponent<CubeCollector>();
    }

    /// <summary>
    /// deactivate our obstacle
    /// </summary>
    public void ObstacleDeactivation()
    {
        cubeCollector.OnDestroyCube();
        _boxCollider.isTrigger = false;
    }
}
