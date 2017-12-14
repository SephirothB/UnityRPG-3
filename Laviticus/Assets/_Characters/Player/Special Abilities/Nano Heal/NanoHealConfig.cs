using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Nano Heal"))]
    public class NanoHealConfig : SpecialAbilityConfig
    {
        [Header("Nano Heal Attributes")]
        //[SerializeField] float energyCostToHealMulti;
        [SerializeField] float baseAmtToHeal;

        public override SpecialAbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<NanoHealBehavior>();
        }


        public float GetHealAmount()
        {
            return baseAmtToHeal;
        }

        //public float GetEnergyCostMulti()
        //{
        //    return energyCostToHealMulti;
        //}
    }
}