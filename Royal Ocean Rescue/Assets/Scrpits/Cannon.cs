using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    //===== Cannon Flying In Air 
    public float timeRemaining = 0;         //How long should we allow the cannon to fly without hitting anything before getting destroyed  
    public float speed = 2;                 //How fast the cannon is moving 

    //=== Cannon Exploding 
    private Animator _animator;                //Animates the explosion 
    private bool _isExploding = false;         //Tells us if we hit something and if so to prepare to destroy the object 
    private Vector3 _position;                 //Used to get position of where it's spawned in respect to the ship 
    private AudioSource _audioSource;          //Plays the explosion sound effect 

   //==================================================================================================================
   // Functions 
   //==================================================================================================================
    
    private void Awake()
    {
        //Gets the animator of the game object 
        _animator = GetComponent<Animator>();
        //Sets the tag to say "Cannon" so that collision will recognize it 
        transform.tag = "Cannon";
        //Import all audio sources attached to the game object and pulls the second one which play when ball explodes 
        var sources = GetComponents<AudioSource>();
        _audioSource = sources[1];
    }

    //Uses to set position when the object is instantiated by the ship shooting 
    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    //Updates the timer till the cannon is de-spawned unless it starts exploding 
    private void Update()
    {
        if (_isExploding) return;
        //Check if timer hit 0
        if (timeRemaining > 0)
        {
            transform.localScale *= 0.99f;
            timeRemaining -= Time.deltaTime;
        }
        //If it did destroy the ball 
        else
        {
            Destroy(gameObject);
        }
    }
    
    //If the cannon is not exploding it moves in the provided direction 
    private void FixedUpdate()
    {
        if(_isExploding) return;
        transform.position += _position * (speed * Time.deltaTime);

    }

    //When the cannon collides with something that can be hit it triggers the Explode
    //Which stops ift from moving, initiate an explode animation and SFX 
    public void Explode()
    {
        //Turns off collider 
        transform.GetComponent<CircleCollider2D>().enabled = false;
        //Makes the velocity 0
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //Sets it to exploding which makes it stop moving 
        _isExploding = true;
        
        //Starts animation and SFX 
        _animator.Play($"CannonExplode");
        _audioSource.Play();
        
        //Decouples the object from ship so player can fire again 
        gameObject.transform.parent = null;
        
        //Starts a coroutine that waits till time is over and then destroys the object 
        StartCoroutine(ExplodeDestroy(_audioSource.clip.length));
    }
    
    //Starts a coroutine that waits till time is over and then destroys the object 
    private IEnumerator ExplodeDestroy(float sourceTime)
    {
        yield return new WaitForSeconds(sourceTime/2f);
        Destroy(gameObject);
    }
}
