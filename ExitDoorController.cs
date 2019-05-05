#pragma warning disable CS0618

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// controller for handling the exit door of the levels
    /// </summary>
    public class ExitDoorController : MainBehavior {
        /// <summary>
        /// the list of the characters currently in the exit
        /// </summary>
        private List<PlayerCharacterController> chars = new List<PlayerCharacterController>();
        /// <summary>
        /// the inherited onstart method
        /// </summary>
        /// <param name="onStartProperties"></param>
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            //find all the amount of players
            int amountOfCharacters = GameObject.FindObjectsOfType<PlayerCharacterController>().Length;
            //when something triggers the inside of the exit
            AddOnTriggerEnterListener(x => {
                //get the character controller
                var character = x.GetComponent<PlayerCharacterController>();
                //is it a character and is it not already in the list
                if (!chars.Contains(character) && character != null) {
                    //add it
                    chars.Add(character);
                }
                //is the amount of characters inside all the characters
                if (chars.Count >= amountOfCharacters) {
                    //get this level
                    int thisLevel = SceneManager.GetActiveScene().buildIndex;
                    //if we havnt completed it
                    if (Utilities.LevelsCompleted <= thisLevel) {
                        //make it so that we have
                        Utilities.LevelsCompleted = thisLevel;
                    }
                    //if it is the lats level
                    if (thisLevel == SceneManager.sceneCountInBuildSettings - 1) {
                        //load the menu
                        SceneManager.LoadScene(0);
                    }
                    //otherwise
                    else {
                        //load the next level
                        SceneManager.LoadScene(thisLevel + 1);
                    }
                }
            });
            //when something exits the trigger of the insode of the exit
            AddOnTriggerExitListener(x => {
                var character = x.GetComponent<PlayerCharacterController>();
                //is it a char and is it already inside the list
                if (chars.Contains(character) && character != null) {
                    //remove it
                    chars.Remove(character);
                }
            });
        }

        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
