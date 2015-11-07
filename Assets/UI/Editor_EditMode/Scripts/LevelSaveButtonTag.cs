using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSaveButtonTag : AnyButtonScript
{
    public enum LevelSaveButtonCommand {NEWFILESAVE, UPDATECURRENTFILE};
    public LevelSaveButtonCommand command;
    public Text inputFieldText;
    private LevelManager levelManager;

    private LevelManager.Level _currentLevel;
    public LevelManager.Level currentLevel
    {
        get
        {
            return _currentLevel;
        }

        set
        {
            _currentLevel = value;
        }
    }

    void Awake()
    {
        base.AwakeBase();
    }

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>() as LevelManager;
    }

    public override void onClick()
    {
        if (command == LevelSaveButtonCommand.NEWFILESAVE)
        {
            SaveNewFile();
        }
        else if (command == LevelSaveButtonCommand.UPDATECURRENTFILE)
        {
            UpdateCurrentFile();
        }
    }

    private void SaveNewFile()
    {
        if(levelManager == null)
            return;

        if(inputFieldText.text.Length == 0)
            return;

        CubeGrid grid = GlobalOptions.Grid;
        grid.SerializeMe(levelManager.GetLevelFolder()+"/"+inputFieldText.text+".xml");
    }

    private void UpdateCurrentFile()
    {
        if(_currentLevel.path.Length == 0)
            return;
        
        CubeGrid grid = GlobalOptions.Grid;
        grid.SerializeMe(_currentLevel.path);
    }
}
