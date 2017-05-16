using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour {
    public Transform canvas;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause")) {
            pause ();
        }
	}

    public void pause() {
        if (canvas.gameObject.activeInHierarchy == false) {
            canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            GameObject.FindGameObjectWithTag ("pc").GetComponent<Controller>().enabled = false;
        } else {
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            GameObject.FindGameObjectWithTag ("pc").GetComponent<Controller>().enabled = true;
        }
    }

    public void restartLevel() {
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag ("pc").GetComponent<Controller>().enabled = true;
        SceneManager.LoadSceneAsync (SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

	public void exitToMainMenu() {
		SceneManager.LoadSceneAsync ("MainMenu", LoadSceneMode.Single);
	}

    public void exitGame() {
        Application.Quit ();
    }

	public void startGame() {
		SceneManager.LoadSceneAsync (1, LoadSceneMode.Single);
	}
}
