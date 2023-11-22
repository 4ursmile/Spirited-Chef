using UnityEngine;
namespace ObjectS
{    
    public class SpriteFollow : MonoBehaviour 
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector2 _size = Vector2.one;
        private Transform _target;
        public BaseFoodSO FoodSO {get; private set;}
    
        public void SetTarget(Transform target, BaseFoodSO foodSO = null)
        {
            _target = target;
            if (foodSO == null) return;
            _spriteRenderer.sprite = foodSO.Icon;
            _spriteRenderer.size = _size * foodSO.IconScale;
            FoodSO = foodSO;
            foodSO.SetIngameFoodInstance(this);

        }
        public void UnSetTarget()
        {
            _target = null;
        }
        private void Update()
        {
            if (_target == null) return;
            transform.position = _target.position + _offset;
        }        
    }
}
