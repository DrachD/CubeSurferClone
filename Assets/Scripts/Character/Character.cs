using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator _anim;

    private Rigidbody _rb;

    private int HashDeath => Animator.StringToHash("Death");

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// if our character collides with an obstacle, 
    /// we end the game and emit the death of the character
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();

        if (obstacle)
        {
            Debug.Log("Game Over");
            obstacle?.OnLevelFailedEvent();
            transform.parent = null;
            _rb.constraints = RigidbodyConstraints.None;
            _anim.SetBool(HashDeath, true);
        }
    }

    /// <summary>
    /// if the first cube collided, 
    /// then we emit the death of the character
    /// </summary>
    public void FailedCharacter()
    {
        transform.parent = null;
        _rb.constraints = RigidbodyConstraints.None;
    }
}
