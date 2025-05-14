using System;
using UnityEngine;

namespace MainGame
{
    public class EnemyAttack : MonoBehaviour
    {
        public event Action<Collider> OnHit;

        [SerializeField] private EnemyAttackData _attackData;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private LayerMask _layerMask;

        public EnemyAttackData AttackData => _attackData;
        public void Attack()
        {
            Debug.Log("Enemy Attack");
            if (Physics.Raycast(_attackPoint.position, _attackPoint.forward, out RaycastHit hitInfo, _attackData.Distance, _layerMask, QueryTriggerInteraction.Ignore))
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
