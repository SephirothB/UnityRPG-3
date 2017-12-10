using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Nano Heal"))]
    public class NanoHealConfig : SpecialAbility
    {
        [Header("Nano Heal Attributes")]
        //[SerializeField] float energyCostToHealMulti;
        [SerializeField] float baseAmtToHeal;

        public override void AttachComponent(GameObject engagingObject)
        {

            var behaviourComponent = engagingObject.AddComponent<NanoHealBehavior>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
           

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