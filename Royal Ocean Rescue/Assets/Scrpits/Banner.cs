using UnityEngine;

public class Banner : MonoBehaviour
{
   public Sprite bannerDestroyed;                   //Allows designer to select which destroyed banner image they want
   private SpriteRenderer _spriteRenderer;          //Connect to the game object's sprite renderer 
   private BoxCollider2D _boxCollider2D;            //Connect to the game object's collider 
   
   //==================================================================================================================
   // Functions 
   //==================================================================================================================
   
   private void Awake()
   {
       _spriteRenderer = GetComponent<SpriteRenderer>();
       _boxCollider2D = GetComponent<BoxCollider2D>();
   }

   //Listen to if the collider gets touched by object with tag of cannon if it does, change the sprite to @bannerDestroyed
   //and turn off the collider 
   public void OnTriggerEnter2D(Collider2D hitBox)
   {
       if (!hitBox.CompareTag($"Cannon")) return;
       hitBox.GetComponent<Cannon>().Explode();
       _spriteRenderer.sprite = bannerDestroyed;
       _boxCollider2D.enabled = false;
   }
}
