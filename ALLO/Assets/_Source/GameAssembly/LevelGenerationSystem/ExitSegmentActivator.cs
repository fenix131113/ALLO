using UnityEngine;

namespace LevelGenerationSystem
{
    public class ExitSegmentActivator : MonoBehaviour
    {
        [SerializeField] private GameObject[] activateWhenDoorDown;
        [SerializeField] private GameObject[] activateWhenDoorLeft;
        [SerializeField] private GameObject[] activateWhenDoorUp;
        
        
        public void ActivateWhenDoorDown()
        {
            foreach (var item in activateWhenDoorDown)
                item.SetActive(true);
        }

        public void ActivateWhenDoorLeft()
        {
            foreach (var item in activateWhenDoorLeft)
                item.SetActive(true);   
        }
        
        public void ActivateWhenDoorUp()
        {
            foreach (var item in activateWhenDoorUp)
                item.SetActive(true);
        }
    }
}