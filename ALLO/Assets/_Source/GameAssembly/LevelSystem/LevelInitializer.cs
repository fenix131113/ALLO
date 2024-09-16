using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LevelSystem
{
	public class LevelInitializer : MonoBehaviour
	{
		private List<ILevelInitialized> _initializers;

		[Inject]
		private void Construct(List<ILevelInitialized> initializes)
		{
			_initializers = initializes;
		}

		private void Awake()
		{
			Initialize();
		}

		/// <summary>
		/// Initialize level
		/// </summary>
		private void Initialize()
		{
			foreach (var initializer in _initializers)
				initializer.Initialize();
		}
	}
}