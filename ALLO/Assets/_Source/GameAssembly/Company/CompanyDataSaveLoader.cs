using System.Linq;
using LevelGenerationSystem;
using PlayerSystem;
using PlayerSystem.Attack;
using PlayerSystem.Attack.Shooting;
using PlayerSystem.Attack.Throwable;
using Zenject;

namespace Company
{
    //TODO: Don't load on non-company scenes
    public class CompanyDataSaveLoader : IInitializable
    {
        private readonly CompanyInterLevelDataContainer _companyData;
        private readonly PlayerWeaponsData _playerWeaponsData;
        private readonly PlayerMutation _playerMutation;
        private readonly LevelGeneration _generation;
        private readonly PlayerUpgrade _playerUpgrade;
        private readonly PlayerAmmoContainer _playerAmmoContainer;
        private readonly PlayerThrowable _playerThrowable;

        [Inject]
        public CompanyDataSaveLoader(CompanyInterLevelDataContainer companyData, PlayerWeaponsData playerWeaponsData,
            PlayerMutation playerMutation, LevelGeneration generation, PlayerUpgrade playerUpgrade,
            PlayerAmmoContainer playerAmmoContainer, PlayerThrowable playerThrowable)
        {
            _companyData = companyData;
            _playerWeaponsData = playerWeaponsData;
            _playerMutation = playerMutation;
            _generation = generation;
            _playerUpgrade = playerUpgrade;
            _playerAmmoContainer = playerAmmoContainer;
            _playerThrowable = playerThrowable;

            ConstructLoad();
        }

        ~CompanyDataSaveLoader() => Expose();

        public void Initialize()
        {
            InitLoad();
            Bind();
        }

        #region Load Data

        private void ConstructLoad()
        {
            LoadCompletedLevels();
        }

        private void InitLoad()
        {
            if (_companyData.CompleteLevels == 0)
                return;

            LoadWeapons();

            //Load players data
            _playerMutation.DefaultPlayer.LoadData(_companyData.DefaultPlayerHealth);
            _playerMutation.MutatedPlayer.LoadData(_companyData.MutatedPlayerHealth);

            // Load player upgrades (need to be after players data load always)
            _playerUpgrade.LoadUpgrades(_companyData.CurrentUpgradesLevels);

            // Load player weapon data
            _playerAmmoContainer.LoadData(_companyData.PlayerWeaponAmmo, _companyData.PlayerStorageAmmo);
            
            // Load player throwable data
            _playerThrowable.LoadData(_companyData.GrenadesCount);
        }

        private void LoadWeapons()
        {
            foreach (var weapon in _companyData.Weapons)
                _playerWeaponsData.TryAddWeapon(weapon);
        }

        private void LoadCompletedLevels() => _generation.SetCompletedLevels(_companyData.CompleteLevels);

        #endregion

        #region Save Data

        private void SaveData()
        {
            //Save weapons
            _companyData.SetWeaponsData(_playerWeaponsData.Weapons);

            //Save players data
            _companyData.SetDefaultPlayerHealth(_playerMutation.DefaultPlayer.Health);
            _companyData.SetMutatedPlayerHealth(_playerMutation.MutatedPlayer.Health);

            //Increase completed levels in saved data
            _companyData.IncreaseCompleteLevels();

            // Save Player Ammo Data
            _companyData.SetWeaponAmmoData(
                _playerAmmoContainer.PlayerWeaponAmmo.ToDictionary(key => key.Key, value => value.Value),
                _playerAmmoContainer.PlayerStorageAmmo.ToDictionary(key => key.Key, value => value.Value));
            
            // Save throwable data
            _companyData.SetGrenadesCount(_playerThrowable.GrenadesCount);
        }

        #endregion

        private void DeleteSavedData() => _companyData.ClearSaves();

        private void Bind()
        {
            _playerMutation.OnCompanyLevelComplete += SaveData;
            _playerMutation.OnDead += DeleteSavedData;
        }

        private void Expose()
        {
            _playerMutation.OnCompanyLevelComplete -= SaveData;
            _playerMutation.OnDead -= DeleteSavedData;
        }
    }
}