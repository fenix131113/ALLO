using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Company.View
{
    public class LiftLoadingView : MonoBehaviour
    {
        [SerializeField] private Image fade;
        [SerializeField] private float fadeOutTime;
        [SerializeField] private float fadeInTime;
        
        private LiftLoading _liftLoading;

        [Inject]
        private void Construct(LiftLoading liftLoading) => _liftLoading = liftLoading;

        private void Start()
        {
            var seq = DOTween.Sequence();
            seq.Append(fade.DOFade(0f, fadeOutTime));
            seq.AppendInterval(4f);
            seq.Append(fade.DOFade(1.0f, fadeInTime));
            seq.onComplete += _liftLoading.LoadNextLevel;
        }
    }
}