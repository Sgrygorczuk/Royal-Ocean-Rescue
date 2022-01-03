using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private Animator _animator;
    private bool _isExploding = false;
    public float timeRemaining = 0;
    private Transform _position;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetPosition(Transform position)
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
        transform.position = Vector3.MoveTowards(transform.position, _position.position, Time.deltaTime);

    }

    public void Explode()
    {
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
