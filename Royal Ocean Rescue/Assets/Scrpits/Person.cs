using UnityEngine;
using Random = System.Random;

public class Person : MonoBehaviour
{
    public Sprite[] peopleSprites;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var rand = new Random();
        var index = rand.Next(0, peopleSprites.Length);
        _spriteRenderer.sprite = peopleSprites[index];
        transform.eulerAngles = new Vector3(0, 0, rand.Next(0, 360));
        print(transform.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
