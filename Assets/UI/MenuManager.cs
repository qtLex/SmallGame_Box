using UnityEngine;
using System.Collections;
using System;

public class MenuManager : MonoBehaviour {

	public RectTransform StartingMenu;
	public KeyCode MenuKey;
	public GameObject[] MenuCollection;

	private GameObject CurrentActiveMenu;

	void Start () {
		if(MenuKey == KeyCode.None){
			MenuKey = KeyCode.Escape;
		}

		Messenger.AddListener("Dead", ShowDeadMenu, 2);
	}

	void Update () {

		if(!StartingMenu) return;

		if (Input.GetKeyUp(KeyCode.Escape)){

			//StartingMenu.gameObject.SetActive(!StartingMenu.gameObject.activeSelf);

			if (CurrentActiveMenu != null){
				CurrentActiveMenu.SetActive(!CurrentActiveMenu.activeSelf);
			}

			CurrentActiveMenu = StartingMenu.gameObject;
		}
	
	}

	public void ShowMenu(int index){


		if (index > MenuCollection.Length - 1) return;

		GameObject _obj = MenuCollection[index];

		if (_obj == null) return;

		// Здесь можно описать переход.
		CurrentActiveMenu.SetActive(false);
		_obj.SetActive(true);
		CurrentActiveMenu = _obj;

	}

	public void ShowDeadMenu(object sender, EventArgs args){
		ShowMenu(2);
	}
}
