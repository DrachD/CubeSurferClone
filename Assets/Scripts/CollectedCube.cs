using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCube : MonoBehaviour
{
    public bool isFirstCube = false;


    /// <summary>
    /// We deactivate obstacles when colliding with cubes, 
    /// if the first cube collided with an obstacle - end the game
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();


        if (obstacle)
        {
            obstacle.ObstacleDeactivation();

            if (1 >= obstacle.collectedCubes.cubes.Count || isFirstCube)
            {
                Debug.Log("Game Over");
                obstacle?.OnLevelFailedEvent();
                obstacle.collectedCubes.character.FailedCharacter();
                return;
            }
            
            obstacle.collectedCubes.cubes.Remove(gameObject);
            transform.parent = null;
        }
    }
}
