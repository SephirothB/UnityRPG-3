using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName =("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbility
    {

        [Header("Power Attack Specific")]
        [SerializeField] float damageMultiplier = 1f;
        // Use this for initialization
        public override void AttachComponent(GameObject objectEngaging)
        {
            var behaviourComponent = objectEngaging.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float GetDamageMulti()
        {
            return damageMultiplier;
        }
    }
}