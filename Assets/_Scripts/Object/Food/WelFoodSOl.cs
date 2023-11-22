
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ObjectS
{
    [CreateAssetMenu(fileName = "WelFoodSOl", menuName = "ScriptableObjects/Food/WelFoodSO", order = 1)]
    public class WellFoodSO : BaseFoodSO
    {
        [field: Header("Food")]
        [field: SerializeField] public List<BaseFoodSO> Intergients {get; private set;}
        public override int Price => Intergients.Sum(x => x.Price) + base.Price;
        public override void Set(BaseFoodSO baseFoodSO)
        {
            base.Set(baseFoodSO);
            var wellFoodSO = baseFoodSO as WellFoodSO;
            if (wellFoodSO == null)
                return;
            Intergients = wellFoodSO.Intergients;
        }
    }

}

