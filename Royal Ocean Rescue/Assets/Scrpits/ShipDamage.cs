using UnityEngine;

public class ShipDamage : MonoBehaviour
{
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite health0;
    public int health = 3;
    
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer.sprite = health3;
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Hurt"))
        {
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
                    _spriteRenderer.sprite = health0;
                    _boxCollider2D.enabled = false;
                    break;
                }
            }
        }
        else if (hitBox.CompareTag($"Fix"))
        {
            _spriteRenderer.sprite = health3;
            health = 3;
            _animator.Play($"NewHealAnimaton");
        }
    }
}
