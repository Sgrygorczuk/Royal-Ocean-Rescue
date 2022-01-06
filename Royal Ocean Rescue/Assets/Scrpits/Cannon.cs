using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Animator _animator;
    private bool _isExploding = false;
    public float timeRemaining = 0;
    private Vector3 _position;
    public float speed = 2;
    public AudioSource audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        transform.tag = "Cannon";
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    private void Update()
    {
        if (_isExploding) return;
        if (timeRemaining > 0)
        {
            transform.localScale *= 0.99f;
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        if(_isExploding) return;
        transform.position += _position * speed * Time.deltaTime;

    }

    public void Explode()
    {
        transform.GetComponent<CircleCollider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _animator.Play($"CannonExplode");
        audioSource.Play();
        _isExploding = true;
        StartCoroutine(ExplodeDestroy());
    }
    
    private IEnumerator ExplodeDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
