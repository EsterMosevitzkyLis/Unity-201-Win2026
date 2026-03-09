using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [Header("Settings")]
    public bool startActive = false; // The checkbox you asked for!

    [Header("Burst Effect")]
    public ParticleSystem burstParticles;

    [Header("Steady Effect")]
    public ParticleSystem steadyParticles;

    void Awake()
    {
        // This sets the initial state based on your checkbox
        if (steadyParticles != null)
        {
            var emission = steadyParticles.emission;
            emission.enabled = startActive;
            
            if (startActive) steadyParticles.Play();
            else steadyParticles.Stop();
        }
    }

    public void PlayBurst()
    {
        if (burstParticles != null) burstParticles.Play();
    }

    public void SetSteadyState(int state)
    {
        if (steadyParticles == null) return;

        var emission = steadyParticles.emission;
        bool shouldBeActive = (state == 1);
        
        emission.enabled = shouldBeActive;

        if (shouldBeActive)
        {
            steadyParticles.Play();
        }
        else
        {
            // We use Stop() here so existing particles can finish their life
            // but no NEW particles are created.
            steadyParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}