using UnityEngine;
using System.Collections;

public class game_over : MonoBehaviour {

    //public GUISkin skin;
    public Texture text;

    bool gameOver;

	// Use this for initialization
	void Start () {
        gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (gameOver)
        {
            Event eve = Event.current;
            //GUI.skin = skin;
            drawScreen();
        }
    }

    void drawScreen()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        //GUI.Box(screenRect, "", skin.GetStyle("game_over"));
        GUI.DrawTexture(screenRect, text);
    }

    public void gameState(bool state)
    {
        gameOver = state;
    }

}
