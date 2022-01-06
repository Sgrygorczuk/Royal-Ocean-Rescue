using TMPro;
using UnityEngine;

/*
 * Chat Bubble create a NPC that will say a line to the player if they are within vicinity. 
 */
public class ChatBubble : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;             //The image of the background bubble    
    public TextMeshPro textMeshPro;                   //The text processing object 
    public Vector2 padding = new Vector2(2f, 2f);   //The padding that the background would have around the text
    public int goal;
    public int current;
    public GameObject itemToDestroy;

    /*
     * Processes the given text, updating the size of the background bubble to fit around it, makes them both
     * invisible till player walks in 
     */
    private void Start()
    {
        //Updates the text 
        textMeshPro.SetText(current + "/" + goal);
        textMeshPro.ForceMeshUpdate();
        //Gets Text Size 
        var textSize = textMeshPro.GetRenderedValues(false);
        textSize = new Vector2(textSize.y, textSize.x);
        //Updates the size of the text bubble 
        spriteRenderer.size = textSize + padding;
        //Sets them to invisible 
        spriteRenderer.color = new Color(1, 1, 1, 0);
        textMeshPro.color = new Color(1, 1, 1, 0);
    }
    
    /**
    * Input: hitBox
    * Purpose: Check if the player exits into any triggering hitBoxes, if they make the text and bubble invisible 
    */
    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (!hitBox.CompareTag($"Ship")) return;
        spriteRenderer.color = new Color(1, 1, 1, 0);
        textMeshPro.color = new Color(1, 1, 1, 0);
    }
    
    /**
    * Input: hitBox
    * Purpose: Check if the player enters into any triggering hitBoxes , if they make the text and bubble visable    
    */
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"RowBoat"))
        {
            current++;
            textMeshPro.SetText(current + "/" + goal);
            if (current == goal)
            {
                Destroy(itemToDestroy);
            }
        }
        
        if (!hitBox.CompareTag($"Ship")) return;
        spriteRenderer.color = new Color(0, 0, 0, 1);
        textMeshPro.color = new Color(1, 1, 1, 1);
    }
}