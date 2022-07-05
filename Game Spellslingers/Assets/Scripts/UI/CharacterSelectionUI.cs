using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** 
 * <summary>
 * A class that manages the character
 * selection UI.
 * </summary>
 */
public class CharacterSelectionUI : MonoBehaviour
{
    public static CharacterSelectionUI instance { get; private set; }
    [SerializeField]
    private GameObject[] characters;
    private int _charIndex;
    public delegate void SelectEventHandler<T, U>(T sender, U eventArgs);
    public event SelectEventHandler<CharacterSelectionUI, EventArgs> ArcherSelected;
    public event SelectEventHandler<CharacterSelectionUI, EventArgs> MageSelected;
    public event SelectEventHandler<CharacterSelectionUI, EventArgs> WarriorSelected;

    public int CharIndex
    {
        get { return _charIndex; }
        set { _charIndex = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) 
    {
        if (scene.name == "Gameplay") {
            GameObject character = Instantiate(characters[CharIndex]);
            switch (CharIndex)
            {
                case 0:
                    OnArcherSelected();
                    Player.instance.playerClass = "Archer";
                    break;
                case 1:
                    OnWarriorSelected();
                    Player.instance.playerClass = "Warrior";
                    break;
                case 2:
                    OnMageSelected();
                    Player.instance.playerClass = "Mage";
                    break;
            }
        }
    }

    private void OnArcherSelected()
    {
        ArcherSelected?.Invoke(this, EventArgs.Empty);
    }
    private void OnMageSelected()
    {
        MageSelected?.Invoke(this, EventArgs.Empty);
    }
    private void OnWarriorSelected()
    {
        WarriorSelected?.Invoke(this, EventArgs.Empty);
    }

    public void SelectArcher()
    {
        CharIndex = 0;
        SceneManager.LoadScene("Gameplay");
    }
    public void SelectWarrior()
    {
        CharIndex = 1;
        SceneManager.LoadScene("Gameplay");
    }
    public void SelectMage()
    {
        CharIndex = 2;
        SceneManager.LoadScene("Gameplay");
    }
}
