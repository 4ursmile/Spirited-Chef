using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class InteractiveTouchHandle : MonoBehaviour
{
    [SerializeField] InputUIBridgeSO _inputUIBridgeSO;
    private Image _interactiveImage;
    private void Awake() {
        _interactiveImage = GetComponent<Image>();
        _inputUIBridgeSO.SetInteractiveImage(_interactiveImage);
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
