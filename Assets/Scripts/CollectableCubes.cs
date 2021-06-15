using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCubes : MonoBehaviour
{
    public int countCollectableCubes;

    private void Start()
    {
        countCollectableCubes = GetComponentsInChildren<CollectableCube>().Length;
    }

    /// <summary>
    /// We destroy the selected cube 
    /// and create a cube under the character
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        CubeCollector cubeCollector = other.GetComponent<CubeCollector>();

        if (cubeCollector)
        {
            cubeCollector.InstantiateCubes(countCollectableCubes);
            Destroy(gameObject);
        }
    }
}
