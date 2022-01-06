using System.Collections;
using UnityEngine;

public class ShipDamage : MonoBehaviour
{
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite health0;
    public int health = 3;
    public Transform respawnPoint;
    public Animator canvasAnimator;
    public GameObject boats;
    public Person person;
    public AudioSource damage;
    public AudioSource death;
    
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer.sprite = health3;
        canvasAnimator.Play($"ScreenFade");
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Hurt") || hitBox.CompareTag($"Cannon"))
        {
            if (hitBox.CompareTag($"Cannon"))
            {
                hitBox.GetComponent<Cannon>().Explode();
            }
            else
            {
                damage.Play();
            }
            health--;
            _animator.Play($"NewHurtAnimation");
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
        else if (hitBox.CompareTag($"Fix"))
        {
            _spriteRenderer.sprite = health3;
            health = 3;
            _animator.Play($"NewHealAnimaton");
            hitBox.GetComponent<AudioSource>().Play();
            respawnPoint.position = hitBox.transform.position;
        }
    }
    
    private IEnumerator Respawn()
    {
        canvasAnimator.Play($"FadeInAndOut");
        _boxCollider2D.enabled = false;
        if (boats.transform.childCount > 0)
        {
            for (var i = boats.transform.childCount - 1; i > -1; i--)
            {
                Instantiate(person,boats.transform.GetChild(i).gameObject.transform.position, Quaternion.identity);
                Destroy(boats.transform.GetChild(i).gameObject);
            }
        }
        transform.GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        transform.position = respawnPoint.position;
        health = 3;
        _spriteRenderer.sprite = health3;
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<TrailRenderer>().enabled = true;
        _boxCollider2D.enabled = true;
    }
}
