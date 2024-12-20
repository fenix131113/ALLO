using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip arenaMusic;
        [SerializeField] private AudioClip companyMusic;
        [SerializeField] private AudioClip tutorialMusic;
        [SerializeField] private int companySceneIndex;
        [SerializeField] private int tutorialSceneIndex;
        [SerializeField] private int arenaSceneIndex;
        
        private void Start()
        {
            DontDestroyOnLoad(this);
            
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void OnDestroy() => SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;

        private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (SceneManager.GetActiveScene().buildIndex == arenaSceneIndex)
            {
                audioSource.clip = arenaMusic;
                audioSource.Play();
            }
            else if (SceneManager.GetActiveScene().buildIndex == companySceneIndex)
            {
                audioSource.clip = companyMusic;
                audioSource.Play();
            }
            else if (SceneManager.GetActiveScene().buildIndex == tutorialSceneIndex)
            {
                audioSource.clip = tutorialMusic;
                audioSource.Play();
            }
        }
    }
}