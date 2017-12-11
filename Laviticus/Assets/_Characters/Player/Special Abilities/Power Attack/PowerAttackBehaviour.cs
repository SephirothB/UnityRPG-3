using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class PowerAttackBehaviour : SpecialAbilityBehaviour
    {
        

        public override void Engage(AbilityUseParams useParams)
        {
            float damagetoDeal = useParams.baseDamage + (config as PowerAttackConfig).GetDamageMulti();
            useParams.target.TakeDamage(damagetoDeal);
            PlayAbilitySound();
        }

    }
}