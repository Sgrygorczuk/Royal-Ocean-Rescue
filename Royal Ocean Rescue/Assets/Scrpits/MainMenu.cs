using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    private Animator _animator;         //Controls the Fade Animator 
    public string sceneName;            //Tells what scene we want to go to 
    public bool isTimed = false;        //Tells us if we want to set it to be timed exit 

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //Sets up the animator and timer if selected 
    private void Start()
    {
        _animator = GameObject.Find("Fade Canvas").GetComponent<Animator>();
        if (isTimed)
        {
            StartCoroutine(TimerToEnd());
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        //Checks if the player is playing a desktop version if they are enables them to use ESC to quit out of the game 
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
        //Moves to the other screen 
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Wipe());
        }
    }

    //Waits for the timer to end 
    private IEnumerator TimerToEnd()
    {
        yield return new WaitForSeconds(10f);
        StartCoroutine(Wipe());
    }
    
    //Fades the screen and moves on to a different screen 
    private IEnumerator Wipe()
    {
        _animator.Play("ScreenFadeIn");
        yield return new WaitForSeconds(0.95f);
        SceneManager.LoadScene(sceneName);
    }
}
