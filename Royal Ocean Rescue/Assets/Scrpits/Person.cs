using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Person : MonoBehaviour
{
    //=========== Vars 
    public Sprite[] peopleSprites;               //Holds all the possible sprites 
    private CircleCollider2D _circleCollider2D;  //Circle collider interacts with everything but the player
    private SpriteRenderer _spriteRenderer;      //Used to set the skin for the sailor 

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //On start connect to the components 
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        StartCoroutine(StopMoving());
    }

    //Gets the random angle an sprite the person will be using 
    private void Start()
    {
        var index = Random.Range(0, peopleSprites.Length - 1);
        _spriteRenderer.sprite = peopleSprites[index];
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }

    //Turns the collider off if interacting with player 
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Ship"))
        {
            _circleCollider2D.enabled = false;
        }
    }
    
    //Turns the collider on if isn't touched by player 
    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Ship"))
        {
            _circleCollider2D.enabled = true;
        }
    }
    
    //Stops the person from floating too far away after crashing 
    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
