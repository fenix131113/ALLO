using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace LevelGenerationSystem.Segments
{
    public class EndTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(LayerService.CheckLayersEquality(other.gameObject.layer, playerLayer))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
