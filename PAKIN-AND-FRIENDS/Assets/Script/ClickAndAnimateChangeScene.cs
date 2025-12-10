using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickPlayAnimationChangeScene : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "Play";
    public string nextSceneName;

    private bool clicked = false;

    private void OnMouseDown()
    {
        if (clicked) return;
        clicked = true;

        Debug.Log("Object clicked!");

        animator.SetTrigger(triggerName);
        StartCoroutine(WaitForAnimation());
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1.5f); // ตั้งตามความยาวคลิป
        SceneManager.LoadScene(nextSceneName);
    }
}
