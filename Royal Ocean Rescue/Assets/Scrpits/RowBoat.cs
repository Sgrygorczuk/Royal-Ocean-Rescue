using UnityEngine;


public class RowBoat : MonoBehaviour
{
    public Transform shipToFollow;
    public Transform shipRotation;
    public Transform ownRear;
    public new Rigidbody2D rigidbody2D;
    public Person person;
    public int health = 2;
    private Animator _animator;
    public SpriteRenderer spriteRenderer;
    public Sprite healedSprite;
    public Sprite damagedSprite;
    private GameObject _origin;
    public int _position;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetPreFabData(Transform rear, Transform rotation, Rigidbody2D rigid, int position, GameObject origin)
    {
        shipToFollow = rear;
        shipRotation = rotation;
        rigidbody2D = rigid;
        _origin = origin;
        _position = position;
    }

    private void FixedUpdate()
    {
        if (transform.position == shipToFollow.position)
        {
            return;
        }

        //transform.position = Mathf.Abs(rigidbody2D.velocity.magnitude) < 1 ? Vector3.Lerp(transform.position, shipToFollow.position, Time.deltaTime) : Vector3.Lerp(transform.position, shipToFollow.position, Time.deltaTime * rigidbody2D.velocity.magnitude);;
        transform.position = Mathf.Abs(rigidbody2D.velocity.magnitude) < 1
            ? Vector3.MoveTowards(transform.position, shipToFollow.position, Time.deltaTime)
            : Vector3.MoveTowards(transform.position, shipToFollow.position,
                Time.deltaTime * rigidbody2D.velocity.magnitude * 3);
        transform.rotation = shipRotation.rotation;

    }

    private void UpdateId(int newPosition)
    {
        _position = newPosition;
    }

    private void UpdateRear(Transform newRear)
    {
        shipToFollow = newRear;
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
        
        if (health == 0)
        {
            if (_origin.transform.childCount > 1)
            {
                if (_position == _origin.transform.childCount - 1)
                {
                    Instantiate(person, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                else
                {
                    _origin.transform.GetChild(_position + 1).gameObject.GetComponent<RowBoat>().UpdateRear(shipToFollow);
                    for (var i = _position; i < _origin.transform.childCount; i++)
                    {
                        _origin.transform.GetChild(i).gameObject.GetComponent<RowBoat>().UpdateId(i - 1);
                    }
                    Instantiate(person, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            else
            {
                Instantiate(person, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
