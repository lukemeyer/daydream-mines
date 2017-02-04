using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Mines
{
	public class SweeperManager : MonoBehaviour
	{

		public BlockField field;
		public TilePool pool;

		public GameObject menuRoot;
		public bool gameHasStarted = false;
		public bool menuVisible = true;

		public SimpleHelvetica winDisplay;

		private string currentSizeKey = "Small";
		private string currentDifficultyKey = "Easy";

		public Dictionary<string,int[]> BoardSizes = new Dictionary<string, int[]> () {
			{ "Small", new int[]{ 4, 5 } },
			{ "Medium", new int[]{ 5, 6 } },
			{ "Large", new int[]{ 10, 10 } }
		};

		public Dictionary<string,float> Difficulty = new Dictionary<string, float> () {
			{ "Easy", .15f },
			{ "Normal", .25f },
			{ "Hard", .35f }
		};

		public MenuButton[] sizeButtons;
		public MenuButton[] difficultyButtons;

		void Start ()
		{
			SceneManager.LoadSceneAsync ("island_flat", LoadSceneMode.Additive);
			field.manager = this;
			field.setSize (BoardSizes [currentSizeKey]);
			field.setMineDensity (Difficulty [currentDifficultyKey]);
			pool.Init ();
			field.Init ();
			ShowMenu ();
			//updateSizeDisplay ();
			setMenuSelections ();
		}

		public void ToggleMenu ()
		{
			if (gameHasStarted) {
				menuVisible = !menuRoot.activeSelf;
				menuRoot.SetActive (menuVisible);
			}
		}

		public void ShowMenu(){
			menuRoot.SetActive (true);
			menuVisible = true;
			winDisplay.DisableSelf ();
		}

		public void HideMenu(){
			menuRoot.SetActive (false);
			menuVisible = false;
		}

		public void setSize (string sizeKey)
		{
			if (BoardSizes.ContainsKey (sizeKey)) {
				Debug.Log ("Setting Size to " + sizeKey);
				currentSizeKey = sizeKey;
				field.setSize (BoardSizes [currentSizeKey]);
				field.setTiles ();
			}
		}

		public void setDifficulty (string difKey)
		{
			if (Difficulty.ContainsKey (difKey)) {
				Debug.Log ("Setting Difficulty to " + difKey);
				currentDifficultyKey = difKey;
				field.setMineDensity (Difficulty [currentDifficultyKey]);
			}
		}

		public void setMenuSelections ()
		{
			foreach (var button in difficultyButtons) {
				button.stickyActive = button.key == currentDifficultyKey;
				button.setActiveState (button.stickyActive);
			}

			foreach (var button in sizeButtons) {
				button.stickyActive = button.key == currentSizeKey;
				button.setActiveState (button.stickyActive);
			}
		}

		public void StartStartGame (){
			StartCoroutine (StartGame ());
		}

		public IEnumerator StartGame ()
		{
			Debug.Log ("Starting Game");
			gameHasStarted = true;
			field.setSize (BoardSizes [currentSizeKey]);
			field.setMineDensity (Difficulty [currentDifficultyKey]);
			field.setTiles ();
			//wait for "start game" sound
			yield return new WaitForSeconds (.75f);
			HideMenu ();
		}

		public IEnumerator EndGame (bool success, Tile final)
		{
			if (success) {
				// Do winning stuff
				winDisplay.EnableSelf();
				winDisplay.Text = "WINNER!";
				winDisplay.GenerateText ();
				yield return new WaitForSeconds (3);
				ShowMenu();
			} else {
				gameHasStarted = false;
				// Do losing stuff
				final.beepSource.Play();
				field.blowMines (final.transform.position, true);
				yield return new WaitForSeconds (3);
				final.beepSource.Stop ();
				final.explosionSource.Play ();
				field.blowMines (final.transform.position, false);
				yield return new WaitForSeconds (2);
				ShowMenu ();
			}
			gameHasStarted = false;
		}
	}
}