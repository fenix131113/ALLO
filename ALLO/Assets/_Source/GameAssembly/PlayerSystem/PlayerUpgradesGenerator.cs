using System.Collections.Generic;
using System.Linq;
using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class PlayerUpgradesGenerator
    {
        private readonly AllUpgradesConfigSO _allUpgrades;

        [Inject]
        public PlayerUpgradesGenerator(AllUpgradesConfigSO allUpgrades) => _allUpgrades = allUpgrades;

        public void GenerateUpgrades(Dictionary<UpgradeType, int> currentUpgrades, out UpgradesDataSO firstUpgrade,
            out UpgradesDataSO secondUpgrade,
            out UpgradesDataSO thirdUpgrade)
        {
            var availableUpgradesData = GetAvailableUpgradesData(currentUpgrades);
            var result = new UpgradesDataSO[3];

            if (availableUpgradesData.Count > 3)
            {
                var resultIndex = 0;

                for (var i = 0; i < 3; i++)
                {
                    var weightSum = availableUpgradesData.Sum(u => u.Weight);

                    foreach (var upgrade in availableUpgradesData.ToList())
                    {
                        if (Random.Range(0, weightSum + 1) > upgrade.Weight)
                        {
                            if (upgrade != availableUpgradesData[^1])
                                continue;

                            result[resultIndex] = upgrade;
                            resultIndex++;
                            availableUpgradesData.Remove(upgrade);
                        }
                        else
                        {
                            result[resultIndex] = upgrade;
                            resultIndex++;
                            availableUpgradesData.Remove(upgrade);
                            break;
                        }
                    }
                }
            }
            else
            {
                result[0] = availableUpgradesData.Count >= 1 ? availableUpgradesData[0] : null;
                result[1] = availableUpgradesData.Count >= 2 ? availableUpgradesData[1] : null;
                result[2] = availableUpgradesData.Count == 3 ? availableUpgradesData[2] : null;
            }

            firstUpgrade = result[0];
            secondUpgrade = result[1];
            thirdUpgrade = result[2];
        }

        private List<UpgradesDataSO> GetAvailableUpgradesData(Dictionary<UpgradeType, int> currentUpgradesLevels)
        {
            List<UpgradesDataSO> availableUpgradesData = new();
            foreach (var upgrade in _allUpgrades.AllUpgrades)
            {
                if (currentUpgradesLevels.TryGetValue(upgrade.UpgradeType, out var currentLevel) &&
                    currentLevel == upgrade.MaxLevel)
                    continue;

                availableUpgradesData.Add(upgrade);
            }

            return availableUpgradesData;
        }
    }
}