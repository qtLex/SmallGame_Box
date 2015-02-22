using UnityEngine;
using System.Collections;
using BoxClasses;

public class SpawnPoint : BaseBox {

	public override void OnSpawn ()
	{
		base.OnSpawn ();
		if (GlobalOptions.Player == null) return;

		GlobalOptions.Player.transform.position = transform.position;
		GlobalOptions.Player.transform.up = transform.up;
		GlobalOptions.Player.transform.right = transform.right;
		GlobalOptions.Player.transform.forward = transform.forward;
	}

}
