using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Character
{
    public class TeslaAttackBehaviour : SpecialAbilityBehaviour
    {
        //TeslaAttackConfig config;
              

        int layerMask = 1 << 8;

        
       

        public override void Engage(AbilityUseParams useParams)
        {
            PlayAbilitySound();
            DealSpecialAttack(useParams);
            PlayParticleEffect();

        }

        

        private void DealSpecialAttack(AbilityUseParams useParams)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as TeslaAttackConfig).GetRadius(),
                Vector3.up,
                (config as TeslaAttackConfig).GetRadius()
                );


            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamagable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float damagetoDeal = useParams.baseDamage + (config as TeslaAttackConfig).GetDamageMulti();
                    damageable.TakeDamage(damagetoDeal);
                }
            }
        }
    }
}
