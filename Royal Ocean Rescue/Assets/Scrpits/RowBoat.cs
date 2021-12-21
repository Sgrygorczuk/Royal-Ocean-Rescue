using UnityEngine;


public class RowBoat : MonoBehaviour
{

    public Transform shipToFollow;
    public Transform shipRotation;
    public Transform ownRear;
    public new Rigidbody2D rigidbody2D;

    public int health = 2;
    private Animator _animator;
    public SpriteRenderer spriteRenderer;
    public Sprite healedSprite;
    public Sprite damagedSprite;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetPreFabData(Transform rear, Transform rotation, Rigidbody2D rigid)
    {
        shipToFollow = rear;
        shipRotation = rotation;
        rigidbody2D = rigid;
    }

    private void FixedUpdate()
    {
        if(transform.position == shipToFollow.position){return; }

        //transform.position = Mathf.Abs(rigidbody2D.velocity.magnitude) < 1 ? Vector3.Lerp(transform.position, shipToFollow.position, Time.deltaTime) : Vector3.Lerp(transform.position, shipToFollow.position, Time.deltaTime * rigidbody2D.velocity.magnitude);;
        transform.position = Mathf.Abs(rigidbody2D.velocity.magnitude) < 1 ? Vector3.MoveTowards(transform.position, shipToFollow.position, Time.deltaTime) : Vector3.MoveTowards(transform.position, shipToFollow.position, Time.deltaTime * rigidbody2D.velocity.magnitude);
        transform.rotation = shipRotation.rotation;

        if (health == 0)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Hurt"))
        {
            health--;
            _animator.Play($"NewHurtAnimation");
            spriteRenderer.sprite = damagedSprite;
        }
        else if (hitBox.CompareTag($"Fix"))
        {
            spriteRenderer.sprite = healedSprite;
            health = 2;
            _animator.Play($"NewHealAnimaton");
        }
    }
}
