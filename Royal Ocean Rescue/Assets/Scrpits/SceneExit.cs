using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Ship"))
        {
            StartCoroutine(Wipe());
        }
    }

    private IEnumerator Wipe()
    {
        animator.Play("ScreenFadeIn");
        yield return new WaitForSeconds(0.95f);
        SceneManager.LoadScene($"TBC");
    }
}
