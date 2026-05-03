using UnityEngine;
using UnityEngine.InputSystem;

public class BurstEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem burstParticles;
    [SerializeField] private Key triggerKey = Key.F;

    private void Update()
    {
        if (burstParticles == null || Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current[triggerKey].wasPressedThisFrame)
        {
            burstParticles.Play();
        }
    }
}
