using System;

namespace SncPucmm.Controller
{
	public enum eState
	{
		Navigation,
		Security,
		Tour,
		MenuMain,
		MenuBuildingDescriptor,
		MenuDirection,
		Exit
	}

	public class State
	{
		private static eState state = eState.Navigation;

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

