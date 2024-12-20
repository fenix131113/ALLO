using System;
using PlayerSystem.Attack.Throwable;
using TMPro;
using UnityEngine;
using Zenject;

namespace PlayerSystem.View
{
    public class GrenadesView : MonoBehaviour
    {
        [SerializeField] private TMP_Text grenadesCountText;

        private PlayerThrowable _throwable;
        
        [Inject]
        private void Construct(PlayerThrowable throwable) => _throwable = throwable;

        private void OnDestroy() => Expose();

        private void Start()
        {
            Bind();
            Draw();
        }

        private void Draw() => grenadesCountText.text = _throwable.GrenadesCount.ToString();

        private void Bind() => _throwable.OnThrowableCountChanged += Draw;

        private void Expose() => _throwable.OnThrowableCountChanged -= Draw;
    }
}