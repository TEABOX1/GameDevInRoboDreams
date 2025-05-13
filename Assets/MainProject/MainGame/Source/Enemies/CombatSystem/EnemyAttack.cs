using System;
using UnityEngine;

namespace MainGame
{
    public class EnemyAttack : MonoBehaviour
    {
        public event Action<Collider> OnHit;
        public event Action OnMeeleAttackStart; //added for animation

        [SerializeField] private EnemyAttackData _attackData;
        [SerializeField] private Transform _attackPoint;

        //added for animation
        [ContextMenu("Force Attack")]
        private void ForceState()
        {
            Attack();
        }
        //end of animation

        public EnemyAttackData AttackData => _attackData;
        public void Attack()
        {
            OnMeeleAttackStart?.Invoke();  //added for animation
            Debug.Log("Enemy Attack");
            if (Physics.Raycast(_attackPoint.position, _attackPoint.forward, out RaycastHit hitInfo, _attackData.Distance))
            {
                OnHit?.Invoke(hitInfo.collider);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_attackPoint == null || _attackData == null)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackData.Distance);
        }
    }
}
