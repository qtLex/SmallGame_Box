using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSaveUIBuilder : MonoBehaviour
{
    public GameObject ButtonPrefab;
    
    private LevelManager levelManager;

	void Start ()
    {
	    levelManager = FindObjectOfType<LevelManager>() as LevelManager;
        RebuildMenu();
	}

    public void RebuildMenu()
    {
        // чистим старые компонеты
        RectTransform[] transforms = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform rectTransform in transforms)
        {
            if (rectTransform.gameObject != this.gameObject)
            {
                DestroyImmediate(rectTransform.gameObject);
            }
        }

        // проверим все ли компоненты указаны и найдены
        if(ButtonPrefab  == null || levelManager == null)
            return;
        
        // добавляем кнопки
        foreach (LevelManager.Level level in levelManager.LevelList)
        {
            GameObject newButton = GameObject.Instantiate(ButtonPrefab);
            newButton.transform.SetParent(transform);
            
            LevelSaveButtonTag levelSaveButtonTag = newButton.GetComponent<LevelSaveButtonTag>();
            levelSaveButtonTag.currentLevel = level;

            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = level.path;
            
        }
    }
}
