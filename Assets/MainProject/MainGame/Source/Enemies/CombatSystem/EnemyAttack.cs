using System;
using UnityEngine;

namespace MainGame
{
    public class EnemyAttack : MonoBehaviour
    {
        public event Action<Collider> OnHit;

        [SerializeField] private EnemyAttackData _attackData;
        [SerializeField] private Transform _attackPoint;

        public EnemyAttackData AttackData => _attackData;
        public void Attack()
        {
            if (Physics.Raycast(_attackPoint.position, _attackPoint.forward, out RaycastHit hitInfo, _attackData.Distance))
            {
                OnHit?.Invoke(hitInfo.collider);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackData.Distance);
        }
    }
}
