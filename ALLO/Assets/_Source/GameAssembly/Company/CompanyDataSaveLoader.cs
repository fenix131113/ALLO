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

        [Inject]
        public CompanyDataSaveLoader(CompanyInterLevelDataContainer companyData, PlayerWeaponsData playerWeaponsData,
            PlayerMutation playerMutation)
        {
            _companyData = companyData;
            _playerWeaponsData = playerWeaponsData;
            _playerMutation = playerMutation;
        }

        ~CompanyDataSaveLoader() => Expose();

        public void Initialize()
        {
            LoadData();
            Bind();
        }

        #region Load Data

        private void LoadData()
        {
            LoadWeapons();
        }

        private void LoadWeapons()
        {
            foreach (var weapon in _companyData.Weapons)
                _playerWeaponsData.TryAddWeapon(weapon);
        }

        #endregion

        #region Save Data

        private void SaveWeapons()
        {
            _companyData.SetWeaponsData(_playerWeaponsData.Weapons);
        }

        #endregion

        private void DeleteSavedData() => _companyData.ClearSaves();

        private void Bind()
        {
            _playerWeaponsData.OnWeaponListChanged += SaveWeapons;
            _playerMutation.OnDead += DeleteSavedData;
        }

        private void Expose()
        {
            _playerWeaponsData.OnWeaponListChanged -= SaveWeapons;
            _playerMutation.OnDead -= DeleteSavedData;
        }
    }
}