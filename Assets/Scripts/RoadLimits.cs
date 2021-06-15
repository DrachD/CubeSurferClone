using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLimits : MonoBehaviour
{
    private float _posX;
    private float _posZ;

    public int width = 2;

    public float minLimitPosX => _posX - width;
    public float maxLimitPosX => _posX + width;

    public float minLimitPosZ => _posZ - width;
    public float maxLimitPosZ => _posZ + width;

    private bool isTrigger = false;

    private void Start()
    {
        _posX = transform.position.x;
        _posZ = transform.position.z;
    }

    /// <summary>
    /// when switching to a straight line, 
    /// we find out the coordinates of the road in order to indicate the boundaries of the road
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null && !isTrigger)
        {
            playerController.newMinOffsetLimitX = minLimitPosX;
            playerController.newMaxOffsetLimitX = maxLimitPosX;
            playerController.newMinOffsetLimitZ = minLimitPosZ;
            playerController.newMaxOffsetLimitZ = maxLimitPosZ;

            isTrigger = true;
        }
    }
}
