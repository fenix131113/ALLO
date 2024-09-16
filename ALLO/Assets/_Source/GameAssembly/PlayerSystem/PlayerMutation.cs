using UnityEngine;
using Zenject;

namespace PlayerSystem
{
	public class PlayerMutation : MonoBehaviour
	{
		[field: SerializeField] public Player DefaultPlayer { get; private set; }
		[field: SerializeField] public Player MutatedPlayer { get; private set; }
		
		public Player CurrentPlayer { get; private set; }

		[Inject]
		private void Construct(Player startPlayer)
		{
			CurrentPlayer = startPlayer;
		}
		
		private void SetPlayer(Player newPlayer)
		{
			CurrentPlayer.gameObject.SetActive(false);
			newPlayer.transform.position = CurrentPlayer.transform.position;
			CurrentPlayer = newPlayer;
			CurrentPlayer.gameObject.SetActive(true);
			CurrentPlayer.OnMutated();
		}
		
		public void SwitchMutate()
		{
			SetPlayer(CurrentPlayer == DefaultPlayer ? MutatedPlayer : DefaultPlayer);
		}
	}
}