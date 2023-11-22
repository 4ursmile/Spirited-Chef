
using UnityEngine;
using UnityEngine.UI;
namespace UI 
{
    [RequireComponent(typeof(Image))]
    public class ImageItem : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}

