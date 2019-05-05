using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// resets the level if any character hits this
    /// </summary>
    public class KillzoneController : MainBehavior{
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            // if anything hits the trigger
            AddOnTriggerEnterListener(e => {
                //if it is a player
                if (e.GetComponent<PlayerCharacterController>() != null) {
                    //load the same level
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            });
        }
    }
}