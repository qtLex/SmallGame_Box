using UnityEngine;
using System.Collections;
using BoxClasses;

public class TrapBox : BaseBox {
	
	private MeshRenderer meshRenderer;

	public override void UserStay(){

		MeshRenderer[] childrenMeshRenderers = GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer childrenMeshRenderer in childrenMeshRenderers){
			if(childrenMeshRenderer.gameObject.name == "LightEffect")
				meshRenderer = childrenMeshRenderer;
		}

		if(meshRenderer!=null){
			Debug.Log("Int");
			meshRenderer.material.SetColor("_TintColor", new Color(255f, 0f, 0f));
		}


		Messenger.Invoke("Dead", this);
	}
}
