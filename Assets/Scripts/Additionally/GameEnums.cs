using UnityEngine;
using System.Collections;

namespace GameEnums{

	public enum ActionHistoryType{
		Forward,
		Back,
		Right,
		Left,
        Direction,
		Empty
	}

	public enum UserActions{
		Come,
		Action,
		Spawn
	}

	public enum GameModes{
		EditMode,
		PlayMode
	}
}
