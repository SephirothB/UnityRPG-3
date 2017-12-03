using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Tesla Attack"))]
    public class TeslaAttackConfig : SpecialAbility
    {

        [Header("Tesla Attack Specific")]
        [SerializeField] float attackRadius = 3.14f; 
        [SerializeField] float damageToEachTarget = 1f;
        // Use this for initialization
        public override void AttachComponent(GameObject objectEngaging)
        {
            var behaviourComponent = objectEngaging.AddComponent<TeslaAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
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