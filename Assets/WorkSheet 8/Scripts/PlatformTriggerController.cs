using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformTriggerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "MovePlatform";
    [SerializeField] private Key triggerKey = Key.Space;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (animator == null || Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current[triggerKey].wasPressedThisFrame)
        {
            animator.SetTrigger(triggerName);
        }
    }
}
