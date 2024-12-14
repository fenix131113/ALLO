using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace LevelGenerationSystem.Segments
{
    public class EndTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private int liftSceneIndex;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(LayerService.CheckLayersEquality(other.gameObject.layer, playerLayer))
                SceneManager.LoadScene(liftSceneIndex);
        }
    }
}
