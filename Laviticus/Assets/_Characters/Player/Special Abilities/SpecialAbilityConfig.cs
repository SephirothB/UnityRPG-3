using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;


namespace RPG.Character
{
    public struct AbilityUseParams
    {
        public IDamagable target;
        public float baseDamage;

        public AbilityUseParams(IDamagable SATarget, float SABaseDamage)
        {
            this.target = SATarget;
            this.baseDamage = SABaseDamage;

        }
    }
    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;

        protected ISpecialAbility behaviour;

        abstract public void AttachComponent(GameObject objectEngaging);

        public void Engage(AbilityUseParams useParams)
        {
            behaviour.Engage(useParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }
        
    }
    public interface ISpecialAbility
    {
        void Engage(AbilityUseParams useParams);
    }
}