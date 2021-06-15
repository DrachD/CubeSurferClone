using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationStartCollider : MonoBehaviour
{
    [SerializeField] RotationDirection RotationDirection;

    public Transform center;

    private bool _isTrigger = true;

    /// <summary>
    /// we begin to rotate our character in the direction 
    /// we need when he collided with a certain obstacle
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Planet planet = other.GetComponent<Planet>();

        if (planet != null)
        {
            planet.StartRotating(center, RotationDirection);
        }
    }
}

public enum RotationDirection
{
    LEFT,
    RIGTH
}
