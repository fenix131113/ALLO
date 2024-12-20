using UnityEngine;

namespace Core
{
    public class MusicControllerSpawner : MonoBehaviour
    {
        [SerializeField] private MusicController musicControllerPrefab;
        
        private void Awake()
        {
            var musicManager = FindObjectOfType<MusicController>();
            
            if(!musicManager)
                Instantiate(musicControllerPrefab);
        }
    }
}