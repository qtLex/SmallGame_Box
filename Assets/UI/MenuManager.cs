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
		Messenger.AddListener("ShowHideBoxSelection", ShowHideBoxSelectionMenu, 0);
	
	}

	void Update () {

		if(!StartingMenu) return;

		if(GlobalOptions.isEditMode){



		}else if (GlobalOptions.isPlayMode){

			if (Input.GetKeyUp(MenuKey)){

				//StartingMenu.gameObject.SetActive(!StartingMenu.gameObject.activeSelf);

				if (CurrentActiveMenu != null){
					CurrentActiveMenu.SetActive(!CurrentActiveMenu.activeSelf);
				}

				CurrentActiveMenu = StartingMenu.gameObject;
			}
		}
	
	}

	public void ShowHideBoxSelectionMenu(object sender, EventArgs args){

     	ShowMenu(3, true);
	}


	public void ShowMenu(int index, bool Invert = false){


		if (index > MenuCollection.Length - 1) return;

		GameObject _obj = MenuCollection[index];

		if (_obj == null) return;

		// Здесь можно описать переход.

		if (CurrentActiveMenu && CurrentActiveMenu != _obj){
			CurrentActiveMenu.SetActive(false);
		};

		if (CurrentActiveMenu != _obj){		
			_obj.SetActive(true);
		}else if(Invert){
			_obj.SetActive(!_obj.activeSelf);
		};
		
		CurrentActiveMenu = _obj;

	}

	public void ShowDeadMenu(object sender, EventArgs args){
		ShowMenu(2);
	}

	public void HideCurrentMenu(){
		if(CurrentActiveMenu != null)
			CurrentActiveMenu.SetActive(false);

		CurrentActiveMenu = StartingMenu.gameObject;
	}
}
