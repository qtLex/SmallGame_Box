using UnityEngine;
using System.Collections;
using BoxClasses;

public class TrapBox : BaseBox {

	private ParticleSystem _ps;
	
	public override void UserStay(){
		if(!_ps){
			_ps = GetComponentInChildren<ParticleSystem>();
		}

		if(!_ps.isPlaying)
			_ps.Play(true);

		Messenger.Invoke("Dead", this);
	}
}
