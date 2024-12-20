using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelGenerationSystem
{
    public class EnemiesGenerator : MonoBehaviour
    {
        [SerializeField] private EnemiesGeneratorLeveling[] enemiesLeveling;
        [SerializeField] private EnemiesGeneratorLeveling afterAllLevels;

        public void Generate(int level)
        {
            var selectedLeveling = enemiesLeveling.FirstOrDefault(leveling => leveling.Level <= level) ?? afterAllLevels;
            
            if(selectedLeveling.Groups.Length == 0)
                return;
            
            var weightSum = selectedLeveling.Groups.Sum(group => group.Weight);
            var sorted = selectedLeveling.Groups.OrderByDescending(group => group.Weight);
            foreach (var current in sorted)
            {
                if (Random.Range(0, weightSum + 1) > current.Weight)
                {
                    weightSum -= current.Weight;
                    continue;
                }

                foreach (var enemy in current.Enemies)
                    enemy.SetActive(true);

                return;
            }
        }
    }

    [Serializable]
    public class EnemiesGeneratorLeveling
    {
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public EnemiesGeneratorGroup[] Groups { get; set; }
    }
    
    [Serializable]
    public class EnemiesGeneratorGroup
    {
        [field: SerializeField] public GameObject[] Enemies { get; set; }
        [field: SerializeField] public int Weight { get; set; }
    }
}