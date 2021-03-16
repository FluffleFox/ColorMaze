using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button Selected;
    public Button SelectedOnSecondScreen;
    int Difficult = 2;
    public MazeGen MazeGenerator;
    public Comunication ShaderCom;

    public Canvas[] CanvasLayers;
    // 0 Main Menu
    // 1 InGameHud
    // 2 Pause
    // 3 AfterGame

    int State = 0;
    public void Select(Button Taped)
    {
        if (Selected != null)
        {
            Selected.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        }
        Selected = Taped;
        Selected.GetComponent<Image>().color = new Color(0.27f, 0.27f, 0.27f, 0.6f);
    }
    public void ChangeToo(Button ToChange)
    {
        if (SelectedOnSecondScreen != null)
        {
            SelectedOnSecondScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        }
        SelectedOnSecondScreen = ToChange;
        SelectedOnSecondScreen.GetComponent<Image>().color = new Color(0.27f, 0.27f, 0.27f, 0.6f);
    }

    public void SelectDifficult(int diff)
    {
        Difficult = diff;
    }

    public void Play()
    {
        ShaderCom.Start();
        switch (Difficult)
        {
            case 1: { MazeGenerator.Generate(70, 0.3f); break; }
            case 2: { MazeGenerator.Generate(50, 0.3f); break; }
            case 3: { MazeGenerator.Generate(30, 0.3f); break; }
        }
        State = 1;
        Time.timeScale = 1;
        ChangeState();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        State = 2;
        ChangeState();
    }

    public void Continue()
    {
        Time.timeScale = 1;
        State = 1;
        ChangeState();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        State = 3;
        ChangeState();
    }

    public void BackToMenu()
    {
        State = 0;
        ChangeState();
    }

    void ChangeState()
    {
        CanvasLayers[0].enabled = false;
        CanvasLayers[1].enabled = false;
        CanvasLayers[2].enabled = false;
        CanvasLayers[3].enabled = false;

        CanvasLayers[State].enabled = true;
    }
}
