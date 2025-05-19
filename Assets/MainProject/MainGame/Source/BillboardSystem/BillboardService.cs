using System;
using System.Collections.Generic;
using GlobalSource;
using UnityEditor;
using UnityEngine;

namespace MainGame
{
    public class BillboardService : MonoServiceBase
    {
        [SerializeField] protected Camera _camera;

        // [SerializeField] protected BillboardBase[] _billboards;
        [SerializeField] protected List<Billboard> _billboards = new List<Billboard>();
        
        public override Type Type { get; } = typeof(BillboardService);
        
        [ContextMenu("Find Billboards")]
        private void FindBillboards()
        {
#if UNITY_EDITOR
            _billboards.AddRange(FindObjectsOfType<Billboard>(true));
            EditorUtility.SetDirty(this);
#endif
        }

        protected override void Awake()
        {
            base.Awake();
            
            for (int i = 0; i < _billboards.Count; ++i)
                _billboards[i].SetCamera(_camera);
        }
        
        public void AddBillboard(Billboard billboard)
        {
            if (billboard == null) return;

            if (!_billboards.Contains(billboard))
            {
                _billboards.Add(billboard);
                billboard.SetCamera(_camera);
            }
        }
    }
}