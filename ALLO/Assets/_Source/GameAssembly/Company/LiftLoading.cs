using UnityEngine.SceneManagement;

namespace Company
{
    public class LiftLoading
    {
        private const int COMPANY_LEVEL_INDEX = 2;
        
        public void LoadNextLevel()
        {
            SceneManager.LoadScene(COMPANY_LEVEL_INDEX);
        }
    }
}