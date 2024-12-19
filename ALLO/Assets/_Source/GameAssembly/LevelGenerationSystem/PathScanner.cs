using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace LevelGenerationSystem
{
    public class PathScanner : MonoBehaviour
    {
        [SerializeField] private AstarPath pathFinder;
        [SerializeField] private float againScanTime;

        private void Start()
        {
            pathFinder.Scan();
            StartCoroutine(ScanAgainCoroutine());
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private IEnumerator ScanAgainCoroutine()
        {
            yield return new WaitForSeconds(againScanTime);
            
            Task.Run(() => pathFinder.ScanAsync());
            StartCoroutine(ScanAgainCoroutine());
        }
    }
}