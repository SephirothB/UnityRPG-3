using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class NanoHealBehavior : MonoBehaviour, ISpecialAbility
    {
        NanoHealConfig config;
        Player player;
        AudioSource audioPlayer;


        public void Engage(AbilityUseParams useParams)
        {

            player.AddHealth(config.GetHealAmount());
            audioPlayer.clip = config.GetAudioClip();
            audioPlayer.Play();
            PlayParticleEffect();


        }

        private void PlayParticleEffect()
        {

            GameObject newParticlePrefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            newParticlePrefab.transform.parent = transform;
            ParticleSystem newParticle = newParticlePrefab.GetComponent<ParticleSystem>();
            newParticle.Play();
            Destroy(newParticlePrefab, newParticle.main.duration);

        }

        void Start()
        {
            player = GetComponent<Player>();
            audioPlayer = GetComponent<AudioSource>();
        }
        // Update is called once per frame
        public void SetConfig(NanoHealConfig configToSet)
        {
            this.config = configToSet;
        }
    }
}