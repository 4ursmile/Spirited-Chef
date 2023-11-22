using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Animations.Rigging;
namespace Character
{
    public class CharacterCustomsie : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private MeshRenderer[] _hairMeshRenderer;
   

        private Material[] materials;
        [SerializeField] private Transform _hatTransform;
        [SerializeField] CharacterCustomSO _customSO;
        private void Awake() {
            materials = _meshRenderer.materials;

        }
        // Start is called before the first frame update
        void Start()
        {
            var mainMa = materials[0];
            mainMa.SetColor("_SkinColor", _customSO.SkinColor);
            mainMa.SetColor("_ClothColor", _customSO.ShirtColor);
            mainMa.SetColor("_HairColor", _customSO.HairColor);
            foreach (var item in _hairMeshRenderer)
            {
                item.material = mainMa;
            }
            if (_customSO.HatStringAddress != "")
            {
                LoadHat();
            }

        }
        private void LoadHat()
        {
            var handle = Database.Table("Hat").GetAsync<GameObject>(_customSO.HatStringAddress);
            handle.Completed += (obj) => {
                var hat = Instantiate(obj.Result, _hatTransform);
                hat.transform.localPosition = Vector3.zero;
                hat.transform.localRotation = Quaternion.Euler(0, -90, 0);
                hat.transform.localScale = Vector3.one;
                hat.GetComponent<MeshRenderer>().material = materials[0];
            };
        }
        public void Select()
        {
            var mainMa = materials[0];
            mainMa.SetColor("_OutlineColor", _customSO.SelectedOutlineColor);
            mainMa.SetFloat("_OutlineWidth", _customSO.OutlineWidthSelected);
        }
        public void Deselect()
        {
            var mainMa = materials[0];
            mainMa.SetColor("_OutlineColor", _customSO.NormalOutlineColor);
            mainMa.SetFloat("_OutlineWidth", _customSO.OutlineWidthNormal);
        }


    }
}

