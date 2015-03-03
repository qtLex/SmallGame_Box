using UnityEngine;
using System.Collections;
using BoxClasses;

public class SpawnPoint : BaseBox {

	public override void OnSpawn ()
	{
		base.OnSpawn ();
		if (GlobalOptions.Player == null) return;

		// перенесем игрока и камеру на точку начала
		GlobalOptions.Player.transform.position = transform.position;

		// повернем в нужном направлении
		GlobalOptions.Player.transform.up = transform.up;
		GlobalOptions.Player.transform.right = transform.right;
		GlobalOptions.Player.transform.forward = transform.forward;

		// поставим камеру
		Transform camera = GameObject.Find("Main Camera").transform;

		camera.position = new Vector3(transform.position.x - transform.forward.x,
		                              transform.position.y - transform.forward.y,
		                              transform.position.z - transform.forward.z);
		camera.up      = transform.up;
		camera.right   = transform.right;
		camera.forward = transform.forward;

		camera.LookAt(GlobalOptions.Player.transform);
	}

}
