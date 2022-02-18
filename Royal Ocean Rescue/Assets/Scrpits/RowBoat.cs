using System.Collections;
using UnityEngine;
using Pathfinding;

public class RowBoat : MonoBehaviour
{
    /// <summary>
    /// This Script controls the row boats following behavior and reattaching to new rear if the one it's following
    /// gets destroyed 
    /// </summary>

    //=============== Following 
    private Transform _shipToFollow;  //The current ship it's following 
    private AIDestinationSetter _destinationSetter;
    
    //================ Self 
    private Transform _ownRear;         //It's own rear that it'll pass on to a new ship 
    private Rigidbody2D _rigidbody2D;   //Rigidbody 
    private BoxCollider2D _boxCollider2D;

    //=================== Visuals 
    private SpriteRenderer _spriteRenderer; //The sprite render that shows it off
    public Sprite healedSprite;           //The full health sprite 
    public Sprite damagedSprite;          //The damaged sprite 
    private Animator _animator;           //The animation that will flash hurt or heal animations
    private int _health = 2;              //How much health the boat has before getting destroyed 
    
    //================ Row Management 
    private GameObject _rowBoatHolderGameObject;    //Where all the boats are held
    private GameObject _person;                     //The Person PreFab in case the boat gets destroyed 
    private int _position;                          //What position is it in following the boats 
    private bool _isDestroyed = false;              //Tells us if the object has been destroyed and we're waiting for sfx to finish 
    
    //=============== SFX
    public AudioSource death;
    public AudioSource celebrate;
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================

    //Sets up all the connections 
    private void Awake()
    {
        //PreFab of the person from Resources 
        _person =  Resources.Load("PreFabs/Person") as GameObject;
        
        _animator = GetComponent<Animator>();
        _ownRear = transform.Find($"Rear");
        _spriteRenderer = transform.Find($"Sprite").GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

    }

    //Bring in the data from the Player when the boat is instantiated  
    public void SetPreFabData(Transform rear, Transform rotation, Rigidbody2D rigid, int position, GameObject origin)
    {
        _shipToFollow = rear;
        _destinationSetter.target = _shipToFollow;
        _rigidbody2D = rigid;
        _rowBoatHolderGameObject = origin;
        _position = position;
    }

    //Changes the position when a row boat is destroyed 
    private void UpdateId(int newPosition)
    {
        _position = newPosition;
        _destinationSetter.target = _shipToFollow;
    }

    //Updates what it should be following 
    private void UpdateRear(Transform newRear)
    {
        _shipToFollow = newRear;
        _destinationSetter.target = _shipToFollow;
    }

    //Returns this boat rear so something else can follow it 
    public Transform GetRear()
    {
        return _ownRear;
    }

    public void TurnOffCollision()
    {
        _boxCollider2D.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        //If the boat is destroyed just leave 
        if(_isDestroyed){return;}
        
        //If the boat is hurt by a rock or a cannon it gets damaged and if it's fully damaged
        if (hitBox.CompareTag($"Hurt") || hitBox.CompareTag($"Cannon"))
        {
            if (hitBox.CompareTag($"Cannon"))
            {
                hitBox.GetComponent<Cannon>().Explode();
            }
            _health--;
            _animator.Play($"NewHurtAnimation");
            _spriteRenderer.sprite = damagedSprite;
        }
        //If the boat goes into healing zone it gets healed 
        else if (hitBox.CompareTag($"Fix"))
        {
            _spriteRenderer.sprite = healedSprite;
            _health = 2;
            _animator.Play($"NewHealAnimaton");
        }
        
        if (hitBox.CompareTag($"RowBoat"))
        {
            _boxCollider2D.enabled = false;
        }

        //If the boat enters the drop off area or dies remove it and update the other boats positions
        if (_health != 0 && !hitBox.CompareTag($"DropOff")) return;
        _isDestroyed = true;
        UpdateBoatOrder();
        DestructionResults();
    }

    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"RowBoat"))
        {
            _boxCollider2D.enabled = true;
        }
    }

    //If the boat that's being destroyed is not the last boat update the order of all the boats 
    private void UpdateBoatOrder()
    {
        if (_rowBoatHolderGameObject.transform.childCount <= 1 || _position == _rowBoatHolderGameObject.transform.childCount - 1) return;
        
        //Updates whatever was following this boat to follow the boat ahead of it
        _rowBoatHolderGameObject.transform.GetChild(_position + 1).gameObject.GetComponent<RowBoat>().UpdateRear(_shipToFollow);
        
        //Updates all the affected boats with new position ids
        for (var i = _position; i < _rowBoatHolderGameObject.transform.childCount; i++)
        {
            _rowBoatHolderGameObject.transform.GetChild(i).gameObject.GetComponent<RowBoat>().UpdateId(i - 1);
        }
    }
    
    //What happens to the boat when it gets destroyed 
    private void DestructionResults()
    {
        float time = 0;
        //If the boat died create a person and give them some movement 
        if (_health == 0)
        {
            var instantiate = Instantiate(_person, transform.position, Quaternion.identity);
            instantiate.GetComponent<Rigidbody2D>().velocity = -_rowBoatHolderGameObject.transform.parent.GetComponent<Rigidbody2D>().velocity / 2;
            death.Play();
            time = death.clip.length;
        }
        //Else if it's dropped 
        else
        {
            celebrate.Play();
            time = celebrate.clip.length;
        }
        StartCoroutine(DestroyBoat(time));
    }
    
    
    //Plays the sound effect and once it's over destroys the boat 
    private IEnumerator DestroyBoat(float time)
    {
        transform.transform.localScale = Vector3.zero;
        transform.GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        
    }
}
