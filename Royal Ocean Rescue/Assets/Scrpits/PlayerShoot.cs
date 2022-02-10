using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    //======= Spawn Positions  
    private Transform _spawnPointRight;     //The transform to the right of the boat 
    private Transform _spawnPointLeft;      //The transform to the left of the boat 

    //======== Instantiation 
    private GameObject _cannonPreFabGameObject;  //The PreFab cannon game object
    private GameObject _cannonHolderGameObject;  //A empty game object that holds all the instantiated cannons 
    
    //====== Shooting Direction 
    private Image _leftPlayerIndicatorImage;      //Image showing player if they're shooting from left if highlighted 
    private Image _rightPlayerIndicatorImage;     //Image showing player if they're shooting from right if highlighted 
    private bool _shootingDirection;              //Keeps track of which direction player is shooting 

    //===== Cannon Firing 
    public int cannonsAvailable = 1;
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    private void Awake()
    {
        //Loads the cannon game object from the Resources/PreFabs/Cannon prefab 
        _cannonPreFabGameObject =  Resources.Load($"PreFabs/Cannon") as GameObject;
        
        //Loads the data from the child of the Ship Game Object 
        _spawnPointLeft = transform.Find($"Cannon_Left").transform;
        _spawnPointRight = transform.Find($"Cannon_Right").transform;
        
        //Loads from Parent of the Ship Game Object 
        _cannonHolderGameObject = GameObject.Find($"Ship_With_Respawn").transform.Find($"Cannons").gameObject;
        
        //Loads from the UI_Canvas game object 
        _leftPlayerIndicatorImage =
            GameObject.Find($"UI_Canvas").transform.Find("Panel").Find($"Left").GetComponent<Image>();
        _rightPlayerIndicatorImage =
            GameObject.Find($"UI_Canvas").transform.Find("Panel").Find($"Right").GetComponent<Image>();

        //Sets the colors of the image indicators 
        SetColor();
    }

    //Spawns a cannon ball 
    public void SpawnCannon()
    {
        //If the amount of cannons that exit in _cannonHolderGameObject is less than or equal to cannons available fire 
        if (_cannonHolderGameObject.transform.childCount >= cannonsAvailable) return;
        
        //Creates cannon object using _cannonPreFabGameObject and sets it's parent to be _cannonHolderGameObject
        var instantiate = Instantiate(_cannonPreFabGameObject.GetComponent<Cannon>(), _shootingDirection ? _spawnPointRight.position : _spawnPointLeft.position, Quaternion.identity);
        instantiate.transform.SetParent(_cannonHolderGameObject.transform);
        
        //Make sure it moves in the direction that the ship is turned towards 
        instantiate.SetPosition(_shootingDirection
            ? new Vector2(Mathf.Cos((Mathf.PI / 180) * transform.rotation.eulerAngles.z),
                Mathf.Sin((Mathf.PI / 180) * transform.rotation.eulerAngles.z))
            : new Vector2(-Mathf.Cos((Mathf.PI / 180) * transform.rotation.eulerAngles.z),
                -Mathf.Sin((Mathf.PI / 180) * transform.rotation.eulerAngles.z)));
        
        //Make sure it moves as fast as the ship was moving
        instantiate.GetComponent<Rigidbody2D>().velocity = transform.GetComponent<Rigidbody2D>().velocity;
    }

    
    //Changes which direction the player wants to shoot from and updates the visual indicators to match 
    public void ChangeShootingDirection()
    {
        _shootingDirection = !_shootingDirection;
        SetColor();
    }

    //Updates the indicator, make the one that player is shooting from highlighted while the other one is grayed out 
    private void SetColor()
    {
        if (_shootingDirection)
        {
            _leftPlayerIndicatorImage.color = Color.gray;
            _rightPlayerIndicatorImage.color = Color.white;
        }
        else
        {
            _leftPlayerIndicatorImage.color = Color.white;
            _rightPlayerIndicatorImage.color = Color.gray;
        }
    }
}
