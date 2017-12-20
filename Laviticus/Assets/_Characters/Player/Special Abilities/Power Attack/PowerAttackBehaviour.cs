using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class PowerAttackBehaviour : SpecialAbilityBehaviour
    {


        public override void Engage(GameObject target)
        {
            DealPowerAttack(target);
            PlayAbilitySound();
        }

        private void DealPowerAttack(GameObject target)
        {
            float damagetoDeal = (config as PowerAttackConfig).GetDamageMulti();
            target.GetComponent<HealthSystem>().TakeDamage(damagetoDeal);
        }
    }
}