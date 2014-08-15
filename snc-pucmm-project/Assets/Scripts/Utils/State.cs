using System;

namespace SncPucmm.Utils
{
	public enum eState
	{
		Exploring,
		NavigationSystem,
		SecuritySystem,
		TourSystem,
		GUISystem,
		Exit
	}

	public class State
	{
		private static eState state = eState.Exploring;

		public static eState GetCurrentState()
		{
			return state;
		}

		public static void ChangeState(eState newState)
		{
			state = newState;
		}
	}
}

