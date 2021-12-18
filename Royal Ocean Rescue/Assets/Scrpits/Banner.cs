using UnityEngine;

public class Banner : MonoBehaviour
{
   public Sprite bannerDestroyed;
   private SpriteRenderer _spriteRenderer;
   private BoxCollider2D _boxCollider2D;
   
   private void Awake()
   {
       _spriteRenderer = GetComponent<SpriteRenderer>();
       _boxCollider2D = GetComponent<BoxCollider2D>();
   }

   public void OnTriggerEnter2D(Collider2D hitBox)
   {
       if (!hitBox.CompareTag($"Cannon")) return;
       hitBox.GetComponent<Cannon>().Explode();
       _spriteRenderer.sprite = bannerDestroyed;
       _boxCollider2D.enabled = false;
   }
}
