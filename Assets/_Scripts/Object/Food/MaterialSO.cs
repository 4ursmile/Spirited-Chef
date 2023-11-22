using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectS
{
    [CreateAssetMenu(fileName = "Material", menuName = "ScriptableObjects/Food/Material", order = 1)]
    public class MaterialSO : BaseFoodSO
    {
        [field: Header("Material")]
        [field: SerializeField] public MaterialType Type { get; private set; }
        [field: SerializeField] public float WashTime { get; private set; }
        [field: SerializeField] public List<BaseFoodSO> ForwardsMaterial { get; private set; }
        public bool NeedPrepare => ForwardsMaterial.Count > 0;
        public override void Set(BaseFoodSO baseFoodSO)
        {
            base.Set(baseFoodSO);
            var materialSO = baseFoodSO as MaterialSO;
            if (materialSO == null)
                return;
            Type = materialSO.Type;
            WashTime = materialSO.WashTime;
            ForwardsMaterial = materialSO.ForwardsMaterial;
        }
    }
}

