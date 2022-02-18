using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //===== Adjustable Speed Metrics 
    public float drift = 0.95f;             //How much drift player should accrue if not driving forward
    public float acceleration = 30.0f;      //Tells you how fast the ship picks up speed
    public float turnRate = 3.5f;           //How much we turn when it's possible to turn
    public float speedNeededToTurn = 8;     //How fast we have to go before the ship can start turing 
    public float maxDrag = 3;               //Max speed to slow down in
    public float maxSpeed = 20;             //Max of how fast you can go forwards 
    public float backwardDivider = 2;       //Divides Max Speed for backwards movement 

    //===== Internal speed controls 
    private float _accelerationInput;       //Listens to the player movement action (up/down)
    private float _steeringInput;           //Listens to the player turning action (lef/right)
    private float _rotationAngle;           //What angle the ship is at 
    private float _velocityVsUp;            //What is it's forward velocity 
    private Rigidbody2D _rigidbody2D;
    

    //==================================================================================================================
    // Functions 
    //==================================================================================================================

    //Sets up the rigidbody 
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Updates all the forces on the boat 
    private void FixedUpdate()
    {
        ApplyEngineForce();
        MitigateOrthogonalVelocity();
        ApplySteering();
    }

    //Updates the movement speed of the boat  
    private void ApplyEngineForce()
    {
        //Get the forward velocity of the boat 
        _velocityVsUp = Vector2.Dot(_rigidbody2D.velocity, transform.up);

        //If the boat is moving forward don't do anything 
        if (_velocityVsUp > maxSpeed && _accelerationInput > 0)
        {
         return;
        }
        
        //If moving backwards don't do anything 
        if (_velocityVsUp < -maxSpeed/backwardDivider && _accelerationInput < 0)
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
        
        //Update forces applied to the boat 
        var engineForceVector = transform.up * (_accelerationInput * acceleration);
        _rigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    //Makes the boat slower when turning 
    private void MitigateOrthogonalVelocity()
    {
        var forwardVelocity = transform.up * Vector2.Dot(_rigidbody2D.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rigidbody2D.velocity, transform.right);

        _rigidbody2D.velocity = forwardVelocity + rightVelocity * drift;
    }

    //Updates the steering left and right
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

    //Sets the speed of the turn and acceleration 
    public void SetInput(Vector2 driveVector)
    {
        _steeringInput = driveVector.x;
        _accelerationInput = driveVector.y;
    }
}
