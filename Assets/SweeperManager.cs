using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SweeperManager : MonoBehaviour {

	public BlockField field;
	public TilePool pool;

	public GameObject menuRoot;
	public bool gameHasStarted = false;

	public SimpleHelvetica sizeDisplay;

	private string currentSizeKey = "Small";
	private string currentDifficultyKey = "Easy";

	public Dictionary<string,int[]> BoardSizes = new Dictionary<string, int[]> () {
		{ "Small", new int[]{4,5} },
		{ "Medium", new int[]{5,6} },
		{ "Large", new int[]{10,10} }
	};

	public Dictionary<string,float> Difficulty = new Dictionary<string, float> () {
		{ "Easy", .15f },
		{ "Normal", .25f },
		{ "Hard", .35f }
	};

	public MenuButton[] sizeButtons;
	public MenuButton[] difficultyButtons;

	void Start () {
		SceneManager.LoadSceneAsync ("island_flat",LoadSceneMode.Additive);
		field.manager = this;
		field.setSize (BoardSizes [currentSizeKey]);
		field.setMineDensity (Difficulty [currentDifficultyKey]);
		pool.Init ();
		field.Init ();
		//updateSizeDisplay ();
		setMenuSelections ();
	}

	public void updateSizeDisplay(){
		sizeDisplay.Text = field.fieldWidth.ToString();
		sizeDisplay.GenerateText ();
	}

	public void ToggleMenu(){
		if (gameHasStarted) {
			menuRoot.SetActive (!menuRoot.activeSelf);
		}
	}

	public void setSize( string sizeKey ){
		if (BoardSizes.ContainsKey (sizeKey)) {
			currentSizeKey = sizeKey;
			field.setSize (BoardSizes [currentSizeKey]);
			field.setTiles ();
		}
	}

	public void setDifficulty( string difKey ){
		if (Difficulty.ContainsKey (difKey)) {
			currentDifficultyKey = difKey;
			field.setMineDensity (Difficulty [currentDifficultyKey]);
		}
	}

	public void setMenuSelections(){
		foreach (var button in difficultyButtons) {
			button.stickyActive = button.key == currentDifficultyKey;
			button.setActiveState (button.stickyActive);
		}

		foreach (var button in sizeButtons) {
			button.stickyActive = button.key == currentSizeKey;
			button.setActiveState (button.stickyActive);
		}
	}

	public void StartGame(){
		gameHasStarted = true;
		field.setSize (BoardSizes [currentSizeKey]);
		field.setMineDensity (Difficulty [currentDifficultyKey]);
		field.setTiles ();
		menuRoot.SetActive (false);
	}

	public IEnumerator EndGame( bool success, Tile final ) {
		if (success) {
			// Do winning stuff
			ToggleMenu();
		} else {
			// Do losing stuff
			field.blowMines(final.transform.position);
			yield return new WaitForSeconds (3);
			ToggleMenu ();
		}
		gameHasStarted = false;
	}
}
