using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PlayerSystem.View
{
    public class MutationProgressView : MonoBehaviour
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color deactivatedColor;
        [SerializeField] private Image playerMutationFiller;

        private PlayerMutation _playerMutation;

        [Inject]
        private void Construct(PlayerMutation playerMutation) => _playerMutation = playerMutation;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Redraw()
        {
            playerMutationFiller.fillAmount = (float)_playerMutation.ScoredKills / _playerMutation.KillsToMutation;

            playerMutationFiller.color = Mathf.Approximately(playerMutationFiller.fillAmount, 1f)
                ? activeColor
                : deactivatedColor;
        }

        private void Bind() => _playerMutation.OnPlayerKillsChanged += Redraw;

        private void Expose() => _playerMutation.OnPlayerKillsChanged -= Redraw;
    }
}