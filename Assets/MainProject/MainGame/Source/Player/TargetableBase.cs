using UnityEngine;

namespace MainGame
{
    public class TargetableBase : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _targetPivot;
        [SerializeField] private Health _health;

        //public static TargetableBase Player { get; private set; }

        public CharacterController CharacterController => _characterController;
        public Transform TargetPivot => _targetPivot;
        public Health Health => _health;

        /*private void Awake()
        {
            Player = this;
        }

        private void OnDestroy()
        {
            if (Player == this)
                Player = null;
        }*/
    }
}
