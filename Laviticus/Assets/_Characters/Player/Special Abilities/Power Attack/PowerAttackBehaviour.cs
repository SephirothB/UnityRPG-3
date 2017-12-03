using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class PowerAttackBehaviour : MonoBehaviour, SpecialAbility.ISpecialAbility
    {
        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        public void Engage(AbilityUseParams useParams)
        {
            float damagetoDeal = useParams.baseDamage + config.GetDamageMulti();
            useParams.target.TakeDamage(damagetoDeal);
        }

    }
}