using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class NanoHealBehavior : SpecialAbilityBehaviour
    {
        Player player;



        public override void Engage(GameObject target)
        {
            var playerHealth = GetComponent<HealthSystem>();
            playerHealth.AddHealth((config as NanoHealConfig).GetHealAmount());
            PlayAbilitySound();
            PlayParticleEffect();


        }



        void Start()
        {
            player = GetComponent<Player>();

        }

    }
}