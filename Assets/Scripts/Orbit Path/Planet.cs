using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Planet : MonoBehaviour
{
    private delegate bool Condition(float degree);
    private Condition condition;

    public GameObject player;

    public PlayerController playerController;

    public Transform objectToOrbit;

    public Vector3 orbitAxis = Vector3.up;

    public float orbitRadius = 75.0f;

    public float orbitRadiusCorrectionSpeed = 0.5f;

    public float orbitRoationSpeed = 10.0f;

    public float orbitAlignToDirectionSpeed = 0.5f;

    private Vector3 _orbitDesiredPosition;
    private Vector3 _previousPosition;
    private Vector3 _relativePos;
    private Quaternion _rotation;
    private Transform _thisTransform;
    private float _currentRotateY;
    private float _maxRotateY;

    private float _leftLimitY;
    private float _rightLimitY;

    private float _distance;

    private float _speed;

    private int _minDistance = 0;
    private int _maxDistance = 10;

    private int _direction = 1;

    private Turn Turn;

    private Touch touch;

    private Vector3 _startPos;

    private float _horizontal;

    float worldWidth         = 8f;  //serialized, example value
    float ratioScreenToWorld = 0.5f; //serialized, example value
    float screenWidth        = (float) Screen.width;

    public void StartRotating(Transform center, RotationDirection rotationDirection)
    {
        _speed = playerController.RotationSpeed;
        playerController.StopMovement();
        objectToOrbit = center;
        _thisTransform = player.transform;
        _distance = Vector3.Distance(center.position, transform.position);
        orbitRadius = _distance;
        StartCoroutine(Rotating(center, rotationDirection));
    }

    /// <summary>
    /// we rotate the character to a certain angle 
    /// and go to the usual rectilinear movement
    /// </summary>
    IEnumerator Rotating(Transform center, RotationDirection rotationDirection)
    {
        _thisTransform = transform;
        _currentRotateY = _thisTransform.rotation.eulerAngles.y;

        RotationDataInitialization(rotationDirection);
        WhereIsLooking(rotationDirection);

        while (true)
        {
            // START PC
            float swerveAmount = Input.GetAxis("Horizontal") * _speed * 5 * Time.deltaTime;
            // END PC

            // START MOBILE
            // if (Input.touchCount > 0) {
            //     touch = Input.GetTouch(0);

            //     switch (touch.phase)  {
            //         case TouchPhase.Began:
            //             _startPos = touch.position;
            //             break;
    
            //         case TouchPhase.Moved:
            //             _horizontal = touch.deltaPosition.x;
            //             break;
            //         case TouchPhase.Ended:
            //             _horizontal = 0;
            //             break;
            //     }
            // }

            // float x = _horizontal * (worldWidth / screenWidth) * ratioScreenToWorld * _speed * 5 * Time.deltaTime;
            // END MOBILE
        
            _thisTransform.RotateAround (objectToOrbit.position, orbitAxis, orbitRoationSpeed * -_direction * Time.deltaTime);

            // START PC
            orbitRadius += (swerveAmount * _direction);
            // END PC

            // START MOBILE
            //orbitRadius += (x * _direction);
            // END MOBILE

            orbitRadius = Mathf.Clamp(orbitRadius, _minDistance + 5.5f, _maxDistance + 0.5f);
            
            _orbitDesiredPosition = (_thisTransform.position - objectToOrbit.position).normalized * orbitRadius + objectToOrbit.position;
            _thisTransform.position = Vector3.Slerp(_thisTransform.position, _orbitDesiredPosition, Time.deltaTime * orbitRadiusCorrectionSpeed); 

            _relativePos = _thisTransform.position - _previousPosition;
            _rotation = Quaternion.LookRotation(_relativePos);
            _thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, _rotation, orbitAlignToDirectionSpeed * Time.deltaTime);
            _previousPosition = _thisTransform.position;
            

            Debug.Log(_thisTransform.rotation.eulerAngles.y);

            float currentEulerAngleY = _thisTransform.rotation.eulerAngles.y;

            if (condition(currentEulerAngleY))
            {
                transform.rotation = Quaternion.Euler(0, Mathf.RoundToInt(_maxRotateY), 0);
                playerController.ChangeDirectionMovement(Turn);
                playerController.StartMovement();
                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// select the current position of the y axis 
    /// and rotate in the direction we need by 90 degrees
    /// </summary>
    private void RotationDataInitialization(RotationDirection rotationDirection)
    {
        switch (rotationDirection)
        {
            case RotationDirection.RIGTH:
                if (_currentRotateY == 270)
                {
                    _maxRotateY = 0;
                    _leftLimitY = 0;
                    _rightLimitY = 270;
                    condition = ConditionRight;
                }
                else if (_currentRotateY == 0)
                {
                    _maxRotateY = 90;
                    _leftLimitY = 90;
                    _rightLimitY = 360;
                    condition = ConditionRight;
                }
                else
                {
                    _leftLimitY = _currentRotateY + 90f;
                    _maxRotateY = _leftLimitY;
                    condition = ConditionUsuallyRight;
                }
                _direction = -1;
                break;
            case RotationDirection.LEFT:
                if (_currentRotateY == 0)
                {
                    _maxRotateY = 270;
                    _leftLimitY = 270;
                    _rightLimitY = 0;
                    condition = ConditionLeft;
                }
                else if (_currentRotateY == 90)
                {
                    _maxRotateY = 0;
                    _leftLimitY = 360;
                    _rightLimitY = 90;
                    condition = ConditionLeft;
                }
                else
                {
                    _leftLimitY = _currentRotateY - 90f;
                    _maxRotateY = _leftLimitY;
                    condition = ConditionUsuallyLeft;
                }
                
                _direction = 1;
                break;
        }
    }

    /// <summary>
    /// we assign the side in which our character will rotate
    /// </summary>
    private void WhereIsLooking(RotationDirection rotationDirection)
    {
        switch (rotationDirection)
        {
            case RotationDirection.RIGTH:
                Turn = Turn.RIGHT;
                break;
            case RotationDirection.LEFT:
                Turn = Turn.LEFT;
                break;
        }
    }

    /// <summary>
    /// conditions under which we can go into rectilinear motion
    /// </summary>
    #region All conditions

    private bool ConditionLeft(float degree)
    {
        return (degree <= _leftLimitY && degree > _rightLimitY);
    }

    private bool ConditionRight(float degree)
    {
        return (degree >= _leftLimitY && degree < _rightLimitY);
    }

    private bool ConditionUsuallyLeft(float degree)
    {
        return (degree <= _leftLimitY);
    }

    private bool ConditionUsuallyRight(float degree)
    {
        return (degree >= _leftLimitY);
    }

    #endregion
}
