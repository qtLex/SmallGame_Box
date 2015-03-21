using UnityEngine;
using System.Collections;

public class CameraHideObsticles : MonoBehaviour {
	
	private bool _isOnView = false;
	public bool isOnView{
		set{
			_isOnView = value;
		}
	}

	private bool _initialized = false;
	public bool Initialized{
		set{
			_initialized = value;
		}
	}

	private Renderer   _renderer;
	private Renderer[] _childrenRenderer;

	private float SourceAlpha = 0.0f;
	private float DestAlpha   = 0.9f;
	
	public Shader TransparencyShader;

	private bool Initialize(){
		_renderer         = GetComponent<Renderer>();
		_childrenRenderer = GetComponentsInChildren<Renderer>();

		return true;
	}

	private void SetRendererAlpha(Renderer alphaItem){
		if(!alphaItem || !alphaItem.enabled)
			return;

		if (_isOnView){
			alphaItem.material.shader = TransparencyShader;
			DestAlpha = 0.9f;
		}
		else{
			DestAlpha = 0.0f;
		}

		Debug.Log(alphaItem);
		
		SourceAlpha = alphaItem.material.GetFloat("_AlphaTransparency");
		float difference = Mathf.Abs(DestAlpha - SourceAlpha);

		if(difference != 0){
			float coef = (difference < 0.001f) ? 1f : 4 * Time.deltaTime;
			alphaItem.material.SetFloat("_AlphaTransparency", Mathf.Lerp(SourceAlpha, DestAlpha, coef));
		}
	}

	void Update() {

		if(!_initialized){
			_initialized = Initialize();
		}

		SetRendererAlpha(_renderer);

		foreach(Renderer item in _childrenRenderer){
			SetRendererAlpha(item);
		}
		
	}
	
	void LateUpdate(){
		_isOnView = false;
	}
	
	
}