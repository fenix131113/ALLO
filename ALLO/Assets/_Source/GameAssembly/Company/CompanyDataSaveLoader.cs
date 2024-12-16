using LevelGenerationSystem;
using PlayerSystem;
using PlayerSystem.Attack;
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

        [Inject]
        public CompanyDataSaveLoader(CompanyInterLevelDataContainer companyData, PlayerWeaponsData playerWeaponsData,
            PlayerMutation playerMutation, LevelGeneration generation)
        {
            _companyData = companyData;
            _playerWeaponsData = playerWeaponsData;
            _playerMutation = playerMutation;
            _generation = generation;

            ConstructLoad();
        }

        ~CompanyDataSaveLoader() => Expose();

        public void Initialize()
        {
            LoadData();
            Bind();
        }

        #region Load Data

        private void ConstructLoad()
        {
            LoadCompletedLevels();
        }

        private void LoadData()
        {
            LoadWeapons();
        }

        private void LoadWeapons()
        {
            foreach (var weapon in _companyData.Weapons)
                _playerWeaponsData.TryAddWeapon(weapon);
        }

        private void LoadCompletedLevels() => _generation.SetCompletedLevels(_companyData.CompleteLevels);

        #endregion

        #region Save Data

        private void SaveWeapons() => _companyData.SetWeaponsData(_playerWeaponsData.Weapons);

        #endregion

        private void DeleteSavedData() => _companyData.ClearSaves();

        private void Bind()
        {
            _playerWeaponsData.OnWeaponListChanged += SaveWeapons;
            _playerMutation.OnDead += DeleteSavedData;
            _playerMutation.OnCompanyLevelComplete += _companyData.IncreaseCompleteLevels;
        }

        private void Expose()
        {
            _playerWeaponsData.OnWeaponListChanged -= SaveWeapons;
            _playerMutation.OnDead -= DeleteSavedData;
            _playerMutation.OnCompanyLevelComplete -= _companyData.IncreaseCompleteLevels;
        }
    }
}