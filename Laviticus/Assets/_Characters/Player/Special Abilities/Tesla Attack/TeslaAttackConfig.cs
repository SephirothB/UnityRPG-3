using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Tesla Attack"))]
    public class TeslaAttackConfig : SpecialAbilityConfig
    {

        [Header("Tesla Attack Specific")]
        [SerializeField] float attackRadius = 3.14f; 
        [SerializeField] float damageToEachTarget = 1f;


        public override SpecialAbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<TeslaAttackBehaviour>();
        }

        public float GetDamageMulti()
        {
            return damageToEachTarget;
        }

        public float GetRadius()
        {
            return attackRadius;
        }
    }
}