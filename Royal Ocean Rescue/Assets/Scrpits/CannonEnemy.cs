using UnityEngine;

public class CannonEnemy : MonoBehaviour
{
    //========== Player facing 
    private bool _playerInRange = false;    //Tells us if player is near 
    private Transform _shipTransform;       //Keeps track of the player transform 

    //============= Shoot Timer 
    private float _spawnTime = 2;   //The timer
    public float startSpawn = 2;    //Max time the timer can be a
    //The Game object from PreFabs that will instate th cannon ball 
    private GameObject _cannonPreFabGameObject;
    private Transform _spawnPoint;
    
    //========= Getting Hit
    private BoxCollider2D _boxCollider2D;       //Checks if a cannon hits the cannon spawner directly 
    public Sprite deathSprite;                  //Only done public because the directories are badly set up
    private SpriteRenderer _spriteRenderer;     //Used to update sprite after it gets hit
    private bool _isDead = false;               //Tells us if it's dead don't shoot or move 

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    private void Awake()
    {
        _shipTransform = GameObject.Find($"Ship").transform;
        _spawnPoint = transform.Find($"Spawn_Point");
        _boxCollider2D = GetComponent<BoxCollider2D>(); 
        _spriteRenderer = transform.Find($"Sprite").GetComponent<SpriteRenderer>();
        
        //Loads the cannon game object from the Resources/PreFabs/Cannon prefab 
        _cannonPreFabGameObject =  Resources.Load($"PreFabs/Cannon") as GameObject;
    }

    //If the player is in range of the cannon update possible to face them and shoot if possible 
    private void Update()
    {
        if (!_playerInRange || _isDead) return;
        UpdatePosition();
        ShootCannon();
    }

    //Updates the rotation of the cannon to face the player 
    private void UpdatePosition()
    {
        var difference = _shipTransform.position - transform.position;
        var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    //Counts down till it can shoot at the player, the shoots 
    private void ShootCannon()
    {
        if (_spawnTime <= 0)
        {
            var instantiate = Instantiate(_cannonPreFabGameObject.GetComponent<Cannon>(), _spawnPoint.position, Quaternion.identity);
            //Make sure it moves in the direction that the ship is turned towards 
            instantiate.SetPosition( _shipTransform.transform.position - _spawnPoint.position);
            instantiate.speed = 1;
            //Resets the spawn timer 
            _spawnTime = startSpawn;
        }
        else
        {
            //Updates the spawn timer 
            _spawnTime -= Time.deltaTime;
        }
    }
    
    //Checks if the player has entered the shoot range and if a cannon landed a hit 
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        //Checks if player entered the shooting range
        if (hitBox.CompareTag($"Ship"))
        {
            _playerInRange = true;
        }

        //Checks if cannon hit the cannon spawner and it need to die 
        if (!hitBox.CompareTag($"Cannon") || !_boxCollider2D.IsTouching(hitBox)) return;
        _isDead = true;
        _spriteRenderer.sprite = deathSprite;
        _boxCollider2D.enabled = false;
        hitBox.GetComponent<Cannon>().Explode();
    }

    //Checks if the player has left the shoot range 
    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Ship"))
        {
            _playerInRange = false;
        }
    }
}
