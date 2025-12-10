using UnityEngine;

public class PlayOnSceneStart : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "Play";

    private void Start()
    {
        animator.SetTrigger(triggerName);
    }
}
