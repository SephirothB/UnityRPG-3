using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Character
{
    public class TeslaAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        TeslaAttackConfig config;
              

        int layerMask = 1 << 8;

        
        public void SetConfig(TeslaAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        public void Engage(AbilityUseParams useParams)
        {
            DealSpecialAttack(useParams);
            PlayParticleEffect();

        }

        private void PlayParticleEffect()
        {
           
            GameObject newParticlePrefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            ParticleSystem newParticle = newParticlePrefab.GetComponent<ParticleSystem>();
            newParticle.Play();
            Destroy(newParticlePrefab, newParticle.main.duration);
            
        }

        private void DealSpecialAttack(AbilityUseParams useParams)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetRadius(),
                Vector3.up,
                config.GetRadius()
                );


            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamagable>();

                if (damageable != null)
                {
                    float damagetoDeal = useParams.baseDamage + config.GetDamageMulti();
                    damageable.TakeDamage(damagetoDeal);
                }
            }
        }
    }
}
