using System;
using UnityEngine;
using Random = System.Random;

public class Person : MonoBehaviour
{
    public Sprite[] peopleSprites;
    public BoxCollider2D boxCollider2D;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var rand = new Random();
        var rand2 = new Random();
        var index = rand.Next(rand2.Next(0, peopleSprites.Length-1), peopleSprites.Length);
        _spriteRenderer.sprite = peopleSprites[index];
        transform.eulerAngles = new Vector3(0, 0, rand.Next(0, 360));
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Ship"))
        {
            boxCollider2D.enabled = false;
        }
    }
}
