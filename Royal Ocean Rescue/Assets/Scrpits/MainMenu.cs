using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public TextMeshPro textMeshPro;                   //The text processing object 
    public Animator animator;
    private bool goingUp = false;
    private float alpha = 1;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            alpha += 0.01f;
        }
        else
        {
            alpha -= 0.01f;
        }
        
        if (alpha < 0 || alpha > 1)
        {
            goingUp = !goingUp;
        }

        textMeshPro.color = new Color(0, 0, 0, alpha);
        
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Wipe());
        }

    }
    
    private IEnumerator Wipe()
    {
        animator.Play("ScreenFadeIn");
        yield return new WaitForSeconds(0.95f);
        SceneManager.LoadScene($"LevelOne");
    }
}
