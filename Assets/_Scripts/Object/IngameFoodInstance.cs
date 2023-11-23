using UnityEngine;
namespace ObjectS
{    
    public class InGameFoodInstance : MonoBehaviour 
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector2 _size = Vector2.one;
        private Transform _target;
        public BaseFoodSO FoodSO {get; private set;}
        public bool NeedToWash {get; set;} = false;
    
        public void SetTarget(Transform target, BaseFoodSO foodSO = null)
        {
            if (foodSO == null) return;
            _target = target;
            _spriteRenderer.sprite = foodSO.Icon;
            _spriteRenderer.size = _size * foodSO.IconScale;
            FoodSO = foodSO;
            NeedToWash = foodSO.NeedToWash;
        }
        public void SetTargetOnly(Transform target)
        {
            _target = target;
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
        public void Destroy()
        {
            Destroy(gameObject);
        }
        public void SwapFood(BaseFoodSO foodSO)
        {
            FoodSO = foodSO;
            NeedToWash = foodSO.NeedToWash;
            _spriteRenderer.sprite = foodSO.Icon;
            _spriteRenderer.size = _size * foodSO.IconScale;
        }
    }
}
