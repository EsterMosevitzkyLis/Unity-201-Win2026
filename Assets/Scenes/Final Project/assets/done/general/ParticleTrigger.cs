using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem burstParticle;

    public void PlayBurst()
    {
        burstParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        burstParticle.Play();
    }
}