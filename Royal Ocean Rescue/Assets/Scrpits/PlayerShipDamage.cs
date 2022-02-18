using System.Collections;
using UnityEngine;

public class PlayerShipDamage : MonoBehaviour
{
    /// <summary>
    /// This Script controls the player health, it's visual representation and respawning 
    /// </summary>

    //============= Sprites 
    private SpriteRenderer _spriteRenderer;  //Controls what sprite is shown
    public Sprite health3;  //If the player has full health 
    public Sprite health2;  //If the player has 2 health
    public Sprite health1;  //If the player has 1 health
    public Sprite health0;  //If the player is dead 
    
    //=========== Boat Controls 
    public int health = 3;                              //Current health of the player
    private Transform _respawnPoint;                    //Place where it will come back to if dies 
    private GameObject _rowBoatHolderGameObject;        //The holder of the row boats, if player dies they all break apart
    private GameObject _person;                   //A copy of the Person preFab for when the boats break apart 
    private BoxCollider2D _boxCollider2D;   //Turns the box collider on and off when dying and coming back
    
    //=========== Player Feedback 
    public AudioSource damage;              //Damage Sounds 
    public AudioSource death;               //Death Sounds
    private Animator _animator;             //Plays the hurt and heal animations 
    private Animator _fadeAnimator;         //Plays the fade in and out animation when player dies

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //Connects all the components 
    private void Awake()
    {
        //PreFab of the person from Resources 
        _person =  Resources.Load("PreFabs/Person") as GameObject;
        
        //Where all the row boats are held if we have any 
        _rowBoatHolderGameObject = transform.Find($"Boats").gameObject;
        
        //Internal boat parts 
        _respawnPoint = GameObject.Find($"Ship_With_Respawn").transform.Find($"RespawnPoint");
        _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _fadeAnimator = GameObject.Find($"Fade Canvas").GetComponent<Animator>();
        _spriteRenderer.sprite = health3;
    }

    //Checks if the player has hit anything that might hurt it 
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        //Checks if the player hit a hurt object or a cannon 
        if (hitBox.CompareTag($"Hurt") || hitBox.CompareTag($"Cannon"))
        {
            //If it's a cannon compels it to explode 
            if (hitBox.CompareTag($"Cannon"))
            {
                hitBox.GetComponent<Cannon>().Explode();
            }
            //Else play damage sound 
            else
            {
                damage.Play();
            }
            
            //Lose health show it visually 
            health--;
            _animator.Play($"NewHurtAnimation");
            UpdateShipState();
        }
        //Checks if player entered a fix station which set it's as it's new respawn and fully heals the boat 
        else if (hitBox.CompareTag($"Fix"))
        {
            _spriteRenderer.sprite = health3;
            health = 3;
            _animator.Play($"NewHealAnimaton");
            hitBox.GetComponent<AudioSource>().Play();
            _respawnPoint.position = hitBox.transform.position;
        }
    }
    
    //Updates the visual look and if the player has fully lost health starts the respawn process 
    private void UpdateShipState()
    {
        switch (health)
        {
            case 2:
            {
                _spriteRenderer.sprite = health2;
                break;
            }
            case 1:
            {
                _spriteRenderer.sprite = health1;
                break;
            }
            case 0:
            {
                death.Play();
                _spriteRenderer.sprite = health0;
                StartCoroutine(Respawn());
                break;
            }
        }
    }
    
    //Gets the player back in position of where they last healed and destroys any boats that the player might have been
    //Carrying with them 
    private IEnumerator Respawn()
    {
        //Disables the collider and tail 
        _boxCollider2D.enabled = false;
        transform.GetComponent<TrailRenderer>().enabled = false;
        
        //Checks if there are any rowboats following and if there are destroy them and replace with people  
        if (_rowBoatHolderGameObject.transform.childCount > 0)
        {
            for (var i = _rowBoatHolderGameObject.transform.childCount - 1; i > -1; i--)
            {
                Instantiate(_person,_rowBoatHolderGameObject.transform.GetChild(i).gameObject.transform.position, Quaternion.identity);
                Destroy(_rowBoatHolderGameObject.transform.GetChild(i).gameObject);
            }
        }
        //Fade in 
        _fadeAnimator.Play($"FadeInAndOut");
        
        //Move the player and bring them back up to full health and update sprite 
        yield return new WaitForSeconds(0.5f);
        transform.position = _respawnPoint.position;
        health = 3;
        _spriteRenderer.sprite = health3;
        
        yield return new WaitForSeconds(0.5f);
        
        //Enable the collider and tail 
        transform.GetComponent<TrailRenderer>().enabled = true;
        _boxCollider2D.enabled = true;
    }
}
