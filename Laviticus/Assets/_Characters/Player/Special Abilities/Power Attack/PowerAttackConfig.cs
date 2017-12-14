using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName =("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {

        [Header("Power Attack Specific")]
        [SerializeField] float damageMultiplier = 1f;
        // Use this for initialization

        public override SpecialAbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<PowerAttackBehaviour>(); 
        }


        public float GetDamageMulti()
        {
            return damageMultiplier;
        }
    }
}