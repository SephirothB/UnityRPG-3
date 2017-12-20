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




        public override void Engage(GameObject target)
        {
            PlayAbilitySound();
            DealSpecialAttack(target);
            PlayParticleEffect();

        }



        private void DealSpecialAttack(GameObject target)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as TeslaAttackConfig).GetRadius(),
                Vector3.up,
                (config as TeslaAttackConfig).GetRadius()
                );


            foreach (RaycastHit hit in hits)
            {
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (target != null && !hitPlayer)
                {
                    float damagetoDeal = (config as TeslaAttackConfig).GetDamageMulti();
                    target.GetComponent<HealthSystem>().TakeDamage(damagetoDeal);
                }
            }
        }
    }
}
