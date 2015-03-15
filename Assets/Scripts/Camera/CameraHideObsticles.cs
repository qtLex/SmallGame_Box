using UnityEngine;
using System.Collections;

public class CameraHideObsticles : MonoBehaviour {
	
	private bool _isOnView = false;
	public bool isOnView{
		set{
			_isOnView = value;
		}
	}

	private Renderer _renderer;
	private float SourceAlpha = 0.0f;
	private float DestAlpha = 0.9f;
	
	public Shader TransparencyShader;
	
	void Update() {

		if(!_renderer)
			_renderer = GetComponent<Renderer>();

		if(!_renderer)
			return;
		
		if (_isOnView){
			
			_renderer.material.shader = TransparencyShader;
			
			SourceAlpha = _renderer.material.GetFloat("_AlphaTransparency");
			DestAlpha = 0.9f;
			_renderer.material.SetFloat("_AlphaTransparency", Mathf.Lerp(SourceAlpha, DestAlpha, 4 * Time.deltaTime));
			
		}else{
			
			SourceAlpha = _renderer.material.GetFloat("_AlphaTransparency");
			DestAlpha = 0.0f;
			_renderer.material.SetFloat("_AlphaTransparency", Mathf.Lerp(SourceAlpha, DestAlpha, 4 * Time.deltaTime));
		}
		
	}
	
	void LateUpdate(){
		_isOnView = false;
	}
	
	
}