using System;

namespace SncPucmm.Controller
{
	public enum eState
	{
		Exploring,
		Tour,
		MenuMain,
		MenuBuilding,
		MenuNavigation,
		MenuInsideBuilding,
		Menu,
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

