using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;


namespace RPG.Character
{

    public abstract class SpecialAbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField]
        float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AudioClip[] abilitySounds;

        protected SpecialAbilityBehaviour behaviour;

        public abstract SpecialAbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbility(GameObject objectToAttachTo)
        {
            SpecialAbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Engage(GameObject target)
        {
            behaviour.Engage(target);
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