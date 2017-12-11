using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Character
{
    public abstract class SpecialAbilityBehaviour : MonoBehaviour
    {
        protected SpecialAbilityConfig config;

        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public abstract void Engage(AbilityUseParams useParams);

        public void SetConfig(SpecialAbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            GameObject newParticlePrefab = Instantiate(
                config.GetParticlePrefab(), 
                transform.position, 
                config.GetParticlePrefab().transform.rotation);

            newParticlePrefab.transform.parent = transform;

            newParticlePrefab.GetComponent<ParticleSystem>().Play();

            StartCoroutine(DestroyParticleWhenFinished(newParticlePrefab));


        }

        IEnumerator DestroyParticleWhenFinished(GameObject newParticlePrefab)
        {
            while (newParticlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(newParticlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }

    
}
