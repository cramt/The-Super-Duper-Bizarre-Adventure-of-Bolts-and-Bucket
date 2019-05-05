using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable CS0618

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets.UI {
    /// <summary>
    /// for controlling the menu canvas
    /// </summary>
    public class MenuController : MainBehavior {

        /// <summary>
        /// buttons for all the levels
        /// </summary>
        public Button[] Levels;
        /// <summary>
        /// override for when shit starts 
        /// </summary>
        /// <param name="onStartProperties"></param>
        public override void OnStart(OnStartProperties onStartProperties) {
            //get how many levels we completed
            int levels = Utilities.LevelsCompleted;
            //loop though possible levels
            for (int i = 0; i < Levels.Length; i++) {
                //define a localI, which is the current level number
                int localI = i + 1;
                // if we havnt completed it change its colour a little
                if (i > levels) {
                    var a = Levels[i].colors;
                    a.normalColor = new Color(1f, 1f, 1f, 0.5f);
                    Levels[i].colors = a;
                }
                // if we have completed it
                // change the scene to that level when the button is clicked
                else {
                    Levels[i].onClick.AddListener(() => {
                        SceneManager.LoadSceneAsync(localI);
                    });
                }
            }
        }

        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {

        }
    }
}
