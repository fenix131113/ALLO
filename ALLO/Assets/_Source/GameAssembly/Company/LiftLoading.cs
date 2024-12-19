using PlayerSystem;
using PlayerSystem.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Company
{
    public class LiftLoading : IInitializable
    {
        private const int COMPANY_LEVEL_INDEX = 2;

        private readonly PlayerUpgradesGenerator _playerUpgradeGenerator;
        private readonly CompanyInterLevelDataContainer _companyData;

        public UpgradesDataSO FirstUpgrade { get; private set; }
        public UpgradesDataSO SecondUpgrade { get; private set; }
        public UpgradesDataSO ThirdUpgrade { get; private set; }

        [Inject]
        public LiftLoading(PlayerUpgradesGenerator playerUpgradeGenerator, CompanyInterLevelDataContainer companyData)
        {
            _playerUpgradeGenerator = playerUpgradeGenerator;
            _companyData = companyData;
        }

        public void Initialize()
        {
            _playerUpgradeGenerator.GenerateUpgrades(_companyData.CurrentUpgradesLevels,
                out var first,
                out var second,
                out var third);

            FirstUpgrade = first;
            SecondUpgrade = second;
            ThirdUpgrade = third;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public static void LoadNextLevel()
        {
            SceneManager.LoadScene(COMPANY_LEVEL_INDEX);
        }
    }
}