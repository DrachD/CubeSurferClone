using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private delegate Vector3 GetVector3OffsetEvent();
    private GetVector3OffsetEvent GetVector3Offset; 

    private delegate Vector3 GetVector3RotateEvent(float swerveAmount);
    private GetVector3RotateEvent GetVector3Rotate;

    [SerializeField] FloatVariable _moveSpeed;

    [SerializeField] FloatVariable _speedRotationPC;

    [SerializeField] FloatVariable _speedRotationMOBILE;

    [SerializeField] FingerTutorial _fingerTutorial;

    public DirectionMovement DirectionMovement;

    public float minOffsetLimit = -2;
    public float maxOffsetLimit = 2;

    public int currentIndexDir = 0;
    private int maxIndexDir = 3;

    private Rigidbody _rb;

    private CharacterController controller;

    [SerializeField] float _offsetSpeed;
    public float OffsetSpeed => _offsetSpeed;

    [SerializeField] float _rotationSpeed;
    public float RotationSpeed => _rotationSpeed;

    private float _swerveAmount;

    private float _horizontal;

    public bool stopMovement = false;

    private int _wayOffset = 1;

    private int _wayRotate = 1;

    [HideInInspector]
    public float newMinOffsetLimitX;
    [HideInInspector]
    public float newMaxOffsetLimitX;
    [HideInInspector]
    public float newMinOffsetLimitZ;
    [HideInInspector]
    public float newMaxOffsetLimitZ;

    private Vector3 _target;
    private Vector2 _startPos;

    private void Start()
    {
        _offsetSpeed = _moveSpeed.value;
        _rotationSpeed = _speedRotationPC.value;
        //_rotationSpeed = _speedRotationMOBILE.value;

        TurnTowardWalking();
        Instantiate();
        
        _rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

    private Vector3 _dir;
    private Touch touch;

    float worldWidth         = 8f;  //serialized, example value
    float ratioScreenToWorld = 1f; //serialized, example value
    float screenWidth        = (float) Screen.width;

    public void StartMovement() => stopMovement = false;
    public void StopMovement() => stopMovement = true;

    private bool _fingerIsDisable = false;

    private bool _isStart = true;

    private void Update()
    {
        // START MOBILE

        // if (!_isStart)
        // {
        //     if (stopMovement) return;
        // }

        // if (Input.touchCount > 0) {
        //     touch = Input.GetTouch(0);

        //     switch (touch.phase)  {
        //         case TouchPhase.Began:
        //             _startPos = touch.position;
        //             break;
 
        //         case TouchPhase.Moved:
        //             stopMovement = false;
        //             _isStart = false;
        //             _fingerTutorial.DisableObject();
        //             _horizontal = touch.deltaPosition.x;
        //             break;

        //         case TouchPhase.Ended:
        //             _horizontal = 0;
        //             break;
        //     }
        // }
        // END MOBILE

        // START PC
        _horizontal = Input.GetAxis("Horizontal");
        _swerveAmount = _horizontal;
        if (_horizontal > 0 && !_fingerIsDisable)
        {
            stopMovement = false;
            _fingerTutorial.DisableObject();
            _fingerIsDisable = true;
        }
        // END PC
    }

    private void FixedUpdate()
    {
        if (stopMovement) return;

        Vector3 pos = GetVector3Rotate(_swerveAmount);
        transform.position = pos;
        transform.position += GetVector3Offset() * _wayOffset * _offsetSpeed * Time.fixedDeltaTime;
    }

    private void Instantiate()
    {
        UpdateWay();
    }

    /// <summary>
    /// our character will move along the x-axis
    /// </summary>
    private Vector3 GetVector3OffsetX()
    {
        return new Vector3(1, 0, 0);
    }

    /// <summary>
    /// our character will move along the z-axis
    /// </summary>
    private Vector3 GetVector3OffsetZ()
    {
        return new Vector3(0, 0, 1);
    }

    /// <summary>
    /// our character will rotate to the x-axis
    /// </summary>
    private Vector3 GetVector3RotateX(float swerveAmount)
    {
        // MOBILE
        //transform.position += new Vector3(_horizontal * (worldWidth / screenWidth) * _wayRotate * ratioScreenToWorld * _rotationSpeed * Time.fixedDeltaTime, 0f, 0f);
        //return new Vector3(Mathf.Clamp(transform.position.x, minOffsetLimit, maxOffsetLimit), transform.position.y, transform.position.z);
        // PC
        transform.position += new Vector3(1, 0, 0) * _wayRotate * swerveAmount * _rotationSpeed * Time.fixedDeltaTime;
        return new Vector3(Mathf.Clamp(transform.position.x, minOffsetLimit, maxOffsetLimit), transform.position.y, transform.position.z);
    }

    /// <summary>
    /// our character will rotate to the z-axis
    /// </summary>
    private Vector3 GetVector3RotateZ(float swerveAmount)
    {
        // MOBILE
        //transform.position += new Vector3(0f, 0f, _horizontal * (worldWidth / screenWidth) * _wayRotate * ratioScreenToWorld * _rotationSpeed * Time.fixedDeltaTime);
        //return new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, minOffsetLimit, maxOffsetLimit));
        // PC
        transform.position += new Vector3(0, 0, 1) * _wayRotate * swerveAmount * _rotationSpeed * Time.fixedDeltaTime;
        return new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, minOffsetLimit, maxOffsetLimit));
    }

    /// <summary>
    /// change the direction of movement after we have exited the circular movement
    /// </summary>
    public void ChangeDirectionMovement(Turn turn)
    {
        switch (turn)
        {
            case Turn.RIGHT:
            {
                if (currentIndexDir + 1 > maxIndexDir)
                    currentIndexDir = 0;
                else
                    currentIndexDir += 1;
                break;
            }
            case Turn.LEFT:
            {
                if (currentIndexDir - 1 < 0)
                    currentIndexDir = maxIndexDir;
                else
                    currentIndexDir -= 1;
                break;
            }
        }

        switch (currentIndexDir)
        {
            case 0:
                DirectionMovement = DirectionMovement.TOP;
                minOffsetLimit = newMinOffsetLimitX;
                maxOffsetLimit = newMaxOffsetLimitX;
                break;
            case 1:
                DirectionMovement = DirectionMovement.RIGHT;
                minOffsetLimit = newMinOffsetLimitZ;
                maxOffsetLimit = newMaxOffsetLimitZ;
                break;
            case 2:
                DirectionMovement = DirectionMovement.BOTTOM;
                minOffsetLimit = newMinOffsetLimitX;
                maxOffsetLimit = newMaxOffsetLimitX;
                break;
            case 3:
                DirectionMovement = DirectionMovement.LEFT;
                minOffsetLimit = newMinOffsetLimitZ;
                maxOffsetLimit = newMaxOffsetLimitZ;
                break;
        }
        

        UpdateWay();
    }

    /// <summary>
    /// update the path that our character will follow
    /// </summary>
    private void UpdateWay()
    {
        switch (DirectionMovement)
        {
            case DirectionMovement.TOP:
                _wayOffset = 1;
                _wayRotate = 1;
                currentIndexDir = 0;
                GetVector3Offset = GetVector3OffsetZ;
                GetVector3Rotate = GetVector3RotateX;
                break;
            case DirectionMovement.RIGHT:
                _wayOffset = 1;
                _wayRotate = -1;
                currentIndexDir = 1;
                GetVector3Offset = GetVector3OffsetX;
                GetVector3Rotate = GetVector3RotateZ;
                break;
            case DirectionMovement.BOTTOM:
                _wayOffset = -1;
                _wayRotate = - 1;
                currentIndexDir = 2;
                GetVector3Offset = GetVector3OffsetZ;
                GetVector3Rotate = GetVector3RotateX;
                break;
            case DirectionMovement.LEFT:
                _wayOffset = -1;
                _wayRotate = -1;
                currentIndexDir = 3;
                GetVector3Offset = GetVector3OffsetX;
                GetVector3Rotate = GetVector3RotateZ;
                break;
        }
    }

    private void TurnTowardWalking()
    {
        switch (DirectionMovement)
        {
            case DirectionMovement.TOP:
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case DirectionMovement.RIGHT:
                transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case DirectionMovement.BOTTOM:
                transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case DirectionMovement.LEFT:
                transform.localEulerAngles = new Vector3(0, 270, 0);
                break;
        }
    }
}

public enum DirectionMovement
{
    TOP,
    RIGHT,
    BOTTOM,
    LEFT
}

public enum Turn
{
    RIGHT,
    LEFT
}
