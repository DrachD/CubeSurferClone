using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCheck : MonoBehaviour
{
    /// <summary>
    /// as soon as we reach the finish line, 
    /// activate the panel we need and stop the character
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        PlayerCheck playerCheck = other.GetComponent<PlayerCheck>();

        if (playerCheck != null)
        {
            playerCheck?.OnLevelPassedActivateEvent();
        }
    }
}
