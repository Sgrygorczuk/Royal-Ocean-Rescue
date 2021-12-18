using System;
using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private bool _isExploding = false;
    public float timeRemaining = 0;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void SetSpeed(Vector2 speed)
    {
        _rigidbody2D.velocity = speed;
    }

    private void Update()
    {
        if (_isExploding) return;
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _animator.Play($"CannonExplode");
        _isExploding = true;
        StartCoroutine(ExplodeDestroy());
    }
    
    private IEnumerator ExplodeDestroy()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
