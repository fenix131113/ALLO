using System;
using System.Collections.Generic;
using System.Linq;
using EntityDrawers;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Data;
using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class PlayerUpgrade
    {
        private readonly Dictionary<UpgradeType, int> _currentUpgradesLevels = new();
        private readonly AllUpgradesConfigSO _allUpgrades;
        private readonly PlayerUpgradesGenerator _upgradesGenerator;
        private readonly PlayerMutation _playerMutation;
        private readonly PlayerShoot _playerShoot;
        private readonly PlayerMovement _playerMovement;
        private readonly MutantDrawer _mutantDrawer;

        public IReadOnlyDictionary<UpgradeType, int> CurrentUpgradesLevels => _currentUpgradesLevels;

        [Inject]
        public PlayerUpgrade(AllUpgradesConfigSO allUpgrades, PlayerUpgradesGenerator upgradesGenerator,
            PlayerMutation playerMutation, PlayerShoot playerShoot, PlayerMovement playerMovement,
            MutantDrawer mutantDrawer)
        {
            _allUpgrades = allUpgrades;
            _upgradesGenerator = upgradesGenerator;
            _playerMutation = playerMutation;
            _playerShoot = playerShoot;
            _playerMovement = playerMovement;
            _mutantDrawer = mutantDrawer;
        }

        public void GenerateUpgrades(out UpgradesDataSO firstUpgrade, out UpgradesDataSO secondUpgrade,
            out UpgradesDataSO thirdUpgrade)
        {
            _upgradesGenerator.GenerateUpgrades(_currentUpgradesLevels, out var firstUp, out var secondUp,
                out var thirdUp);

            firstUpgrade = firstUp;
            secondUpgrade = secondUp;
            thirdUpgrade = thirdUp;
        }

        private void Upgrade(UpgradeType upgrade, int level)
        {
            switch (upgrade)
            {
                case UpgradeType.DEFAULT_PLAYER_HEALTH:
                    for (var i = 0; i < level; i++)
                        if (TryUpgradeSingle(UpgradeType.DEFAULT_PLAYER_HEALTH))
                            _playerMutation.DefaultPlayer.SetMaxHealth(_playerMutation.DefaultPlayer.MaxHealth +
                                                                       _allUpgrades.AllUpgrades.First(item =>
                                                                           item.UpgradeType == upgrade).UpgradeValue);

                    break;
                case UpgradeType.MUTATED_PLAYER_HEALTH:
                    for (var i = 0; i < level; i++)
                        if (TryUpgradeSingle(UpgradeType.MUTATED_PLAYER_HEALTH))
                            _playerMutation.MutatedPlayer.SetMaxHealth(_playerMutation.MutatedPlayer.MaxHealth +
                                                                       _allUpgrades.AllUpgrades.First(item =>
                                                                           item.UpgradeType == upgrade).UpgradeValue);
                    break;
                case UpgradeType.RELOAD_SPEED:
                    for (var i = 0; i < level; i++)
                        if (TryUpgradeSingle(UpgradeType.RELOAD_SPEED))
                            _playerShoot.AddReloadTimeReductionPercentage(_allUpgrades
                                .AllUpgrades.First(item =>
                                    item.UpgradeType == upgrade).UpgradeValue);
                    break;
                case UpgradeType.MOVE_SPEED:
                    for (var i = 0; i < level; i++)
                        if (TryUpgradeSingle(UpgradeType.MOVE_SPEED))
                            _playerMovement.AddMovementUpgradePercentage(_allUpgrades.AllUpgrades.First(item =>
                                item.UpgradeType == upgrade).UpgradeValue);
                    break;
                case UpgradeType.DASH:
                    if (TryUpgradeSingle(UpgradeType.DASH))
                        _playerMovement.UnlockDash();
                    break;
                case UpgradeType.SPIT:
                    if (TryUpgradeSingle(UpgradeType.SPIT))
                        _mutantDrawer.UnlockSpit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(upgrade), upgrade, null);
            }
        }

        public void LoadUpgrades(Dictionary<UpgradeType, int> upgradesLevels)
        {
            foreach (var upgrade in upgradesLevels)
                Upgrade(upgrade.Key, upgrade.Value);
        }

        private bool TryUpgradeSingle(UpgradeType upgradeType)
        {
            _currentUpgradesLevels.TryAdd(upgradeType, 1);

            var isMaxLevel = _currentUpgradesLevels[upgradeType] == _allUpgrades.AllUpgrades
                .First(u => u.UpgradeType == upgradeType).MaxLevel;

            if (isMaxLevel)
                return true;

            _currentUpgradesLevels[upgradeType] += 1;
            return true;
        }
    }
}