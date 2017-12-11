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
    public abstract class SpecialAbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AudioClip[] abilitySounds;

        protected SpecialAbilityBehaviour behaviour;

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

        public AudioClip GetRandomAbilitySound()
        {
            return abilitySounds[Random.Range(0, abilitySounds.Length)];
        }
        
    }

}