using DG.Tweening;
using PlayerSystem.Attack.Data;
using UnityEngine;
using Utils;

namespace PlayerSystem.Items.Collectable
{
    public class WeaponCollectable : MonoBehaviour
    {
        [field: SerializeField] private Color lightColor;
        [field: SerializeField] private float coloringTime;
        [field: SerializeField] private WeaponBaseDataSO weapon;
        [field: SerializeField] private SpriteRenderer flashingRenderer;
        [field: SerializeField] private LayerMask interactionLayer;
        
        private Color _originalColor;
        private Tween _lightOnTween;
        private Tween _lightBackTween;

        private void Start()
        {
            _originalColor = flashingRenderer.color;
            LightOn();
        }
        
        private void OnDestroy()
        {
            _lightOnTween?.Kill();
            _lightBackTween?.Kill();
        }

        private void LightOn()
        {
            _lightOnTween = flashingRenderer.DOColor(lightColor, coloringTime);
            _lightOnTween.onComplete += LightBack;
        }

        private void LightBack()
        {
            _lightBackTween = flashingRenderer.DOColor(_originalColor, coloringTime);
            _lightBackTween.onComplete += LightOn;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactionLayer))
                return;

            if (!other.TryGetComponent(out PlayerLinks playerLinks))
                return;
            
            playerLinks.PlayerWeaponsData.TryAddWeapon(weapon);
            Destroy(gameObject);
        }
    }
}