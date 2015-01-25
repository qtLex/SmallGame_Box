using UnityEngine;
using System.Collections;

public class CameraHideObsticles : MonoBehaviour {

	private bool _isOnView = false;
	public bool isOnView{
		set{
			_isOnView = value;
		}
	}

	private float SourceAlpha = 0.0f;
	private float DestAlpha = 0.9f;

	public Shader TransparencyShader;

	void Update() {

		if(!renderer){return;};

		if (_isOnView){

			renderer.material.shader = TransparencyShader;

			SourceAlpha = renderer.material.GetFloat("_AlphaTransparency");
			DestAlpha = 0.9f;
			renderer.material.SetFloat("_AlphaTransparency", Mathf.Lerp(SourceAlpha, DestAlpha, 4 * Time.deltaTime));

		}else{

			SourceAlpha = renderer.material.GetFloat("_AlphaTransparency");
			DestAlpha = 0.0f;
			renderer.material.SetFloat("_AlphaTransparency", Mathf.Lerp(SourceAlpha, DestAlpha, 4 * Time.deltaTime));
		}
	
	}

	void LateUpdate(){
		_isOnView = false;
	}

	
}
