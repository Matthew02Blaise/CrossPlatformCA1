using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    private Button startButton;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(StartGameButton);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGameButton()
    {
        Debug.Log(startButton.gameObject.name + " was clicked");
        gameManager.StartGame();

    }
}
