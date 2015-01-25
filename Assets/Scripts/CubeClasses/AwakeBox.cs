using UnityEngine;
using System.Collections;

public class AwakeBox : MonoBehaviour 
{
	void Start()
	{
		PlayerDetector[] Children = this.gameObject.GetComponentsInChildren<PlayerDetector>();

		foreach(PlayerDetector Child in Children)
		{

			if((Child.gameObject.tag != "Finish") && (Child.gameObject != this.gameObject)){
				MeshRenderer _meshRenderer = Child.GetComponent<MeshRenderer>();
				if(_meshRenderer != null) {_meshRenderer.enabled = false;}
				}
		}

	}
}
