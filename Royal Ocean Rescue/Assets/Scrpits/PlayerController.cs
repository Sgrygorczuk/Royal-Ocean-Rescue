using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float drift = 0.95f;
    public float acceleration = 30.0f;
    public float turnRate = 3.5f;
    public float speedNeededToTurn = 8;
    public float maxDrag = 3;
    public float maxSpeed = 20;
    
    private float _accelerationInput;
    private float _steeringInput;
    private float _rotationAngle;
    private float _velocityVsUp;

    private Rigidbody2D _rigidbody2D;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        MitigateOrthogonalVelocity();
        ApplySteering();
    }

    private void ApplyEngineForce()
    {
        _velocityVsUp = Vector2.Dot(_rigidbody2D.velocity, transform.up);

        if (_velocityVsUp > maxSpeed && _accelerationInput > 0)
        {
         return;
        }
        
        if (_velocityVsUp < -maxSpeed/10 && _accelerationInput < 0)
        {
            return;
        }

        if (_rigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && _accelerationInput > 0)
        {
            return;
        }
        
        //If Players is not holding the button start to drag till MAXDrag, else no drag 
        if (_accelerationInput == 0) { _rigidbody2D.drag = Mathf.Lerp(_rigidbody2D.drag, maxDrag, Time.fixedDeltaTime * maxDrag); }
        else { _rigidbody2D.drag = 0; }
        
        var engineForceVector = transform.up * (_accelerationInput * acceleration);
        _rigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void MitigateOrthogonalVelocity()
    {
        var forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = forwardVelocity + rightVelocity * drift;
    }

    private void ApplySteering()
    {
        //Checks if it's faster than required speed 
        var minSpeedBeforeTuring = _rigidbody2D.velocity.magnitude / speedNeededToTurn;
        //Rounds to either 0 or 1 
        minSpeedBeforeTuring = Mathf.Clamp01(minSpeedBeforeTuring);
        
        //If the speed is high enough then we can rotate 
        _rotationAngle -= _steeringInput * turnRate * minSpeedBeforeTuring;
        _rigidbody2D.MoveRotation(_rotationAngle);
    }

    public void SetInput(Vector2 driveVector)
    {
        _steeringInput = driveVector.x;
        _accelerationInput = driveVector.y;
    }
}
