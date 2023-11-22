using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using DG.Tweening;
namespace UI
{
    public class LoadingUIManager : MonoBehaviour
    {
        [field: SerializeField] public Image BackgroundImage { get; private set; }
        [field: SerializeField] public Image GameLogo { get; private set; }
        [field: SerializeField] public Image LoadingBar { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LoadingPercentage { get; private set; }
        [field: SerializeField] public TextMeshProUGUI LoadingTip { get; private set; }

        public void SetLoadingPercentage(float percentage)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(percentage.ToString("0.00"));
            sb.Append("%");
            LoadingPercentage.text = sb.ToString();
            LoadingBar.DOFillAmount(percentage / 100, 0.25f).SetEase(Ease.InSine);
        }
        public void SetToolTip(string tip)
        {
            DOTween.To(() => LoadingTip.text, x => LoadingTip.text = x, tip, 0.25f).SetEase(Ease.InSine);
        }
        public void SetBackgroundImage(Sprite sprite, float fadeTime = 0.5f)
        {
            Image image = Instantiate(BackgroundImage, BackgroundImage.transform.parent);
            image.color = new Color(1, 1, 1, 0);
            image.sprite = sprite;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(BackgroundImage.DOFade(0, fadeTime).SetEase(Ease.InOutCubic));
            sequence.Join(image.DOFade(1, fadeTime).SetEase(Ease.InOutCubic));
            sequence.AppendCallback(() =>
            {
                
                BackgroundImage.sprite = sprite;
                BackgroundImage.color = new Color(1, 1, 1, 1);
                Destroy(image.gameObject);
            });
            sequence.Play();
        }
        public void SetGameLogo(Sprite sprite, float fadeTime = 0.5f)
        {
            Image image = Instantiate(GameLogo, GameLogo.transform.parent);
            image.color = new Color(1, 1, 1, 0);
            image.sprite = sprite;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(GameLogo.DOFade(0, fadeTime).SetEase(Ease.InOutCubic));
            sequence.Join(image.DOFade(1, fadeTime).SetEase(Ease.InOutCubic));
            sequence.AppendCallback(() =>
            {
                
                GameLogo.sprite = sprite;
                GameLogo.color = new Color(1, 1, 1, 1);
                Destroy(image.gameObject);
            });
            sequence.Play();
        }

    }
}

