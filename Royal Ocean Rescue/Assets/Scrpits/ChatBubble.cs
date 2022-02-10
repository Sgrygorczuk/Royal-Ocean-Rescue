using TMPro;
using UnityEngine;

/*
 * Chat Bubble create a NPC that will say a line to the player if they are within vicinity. 
 */
public class ChatBubble : MonoBehaviour
{

    //==== Display Settings 
    private SpriteRenderer _spriteRenderer;             //The image of the background bubble    
    private TextMeshPro _textMeshPro;                   //The text processing object 
    public Vector2 padding = new Vector2(2f, 2f);   //The padding that the background would have around the text
    
    //====== Win State 
    private int _goal;                                   //The goal we want to reach 
    private int _current;                               //The current amount the player has collected 
    public GameObject itemToDestroy;                    //The Object that the designer sets to destroy after current = goal
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================

    /*
     * Processes the given text, updating the size of the background bubble to fit around it, makes them both
     * invisible till player walks in 
     */
    private void Start()
    {
        PreConnectVariables();
        PreSetData();
    }

    //Connects all the vars to necessary components 
    private void PreConnectVariables()
    {
        _spriteRenderer = transform.Find("TextBackground").GetComponent<SpriteRenderer>();
        _textMeshPro = transform.Find($"ScoreText").GetComponent<TextMeshPro>();
    }

    //Pre loads the data and updates visibility 
    private void PreSetData()
    {
        //Updates the text 
        _textMeshPro.SetText(_current + "/" + _goal);
        _textMeshPro.ForceMeshUpdate();
        //Gets Text Size 
        var textSize = _textMeshPro.GetRenderedValues(false);
        textSize = new Vector2(textSize.y, textSize.x);
        //Updates the size of the text bubble 
        _spriteRenderer.size = textSize + padding;
        //Sets them to invisible 
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        _textMeshPro.color = new Color(1, 1, 1, 0);
    }
    
    //TODO: Gotta see if the connect to anything or not
    public ChatBubble(int goal)
    {
        _goal = goal;
    }

    
    /**
    * Input: hitBox
    * Purpose: Check if the player exits into any triggering hitBoxes, if they make the text and bubble invisible 
    */
    private void OnTriggerExit2D(Collider2D hitBox)
    {
        //Checks if player left 
        if (!hitBox.CompareTag($"Ship")) return;
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        _textMeshPro.color = new Color(1, 1, 1, 0);
    }
    
    /**
    * Input: hitBox
    * Purpose: Check if the player enters into any triggering hitBoxes , if they make the text and bubble visible    
    */
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        //Checks if the rowboat entered then adds it to the counter and destroys the boat 
        if (hitBox.CompareTag($"RowBoat"))
        {
            _current++;
            _textMeshPro.SetText(_current + "/" + _goal);
            if (_current == _goal)
            {
                Destroy(itemToDestroy);
            }
        }
        
        //Checks if the player is in to show the stats 
        if (!hitBox.CompareTag($"Ship")) return;
        _spriteRenderer.color = new Color(0, 0, 0, 1);
        _textMeshPro.color = new Color(1, 1, 1, 1);
    }
}