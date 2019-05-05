using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// an abstract class that can be attached to any game object in unity
    /// </summary>
    public abstract class MainBehavior : MonoBehaviour {

        private List<KeyValuePair<Action, TaskCompletionSource<OnUpdateProperties>>> invokeFunctionList = new List<KeyValuePair<Action, TaskCompletionSource<OnUpdateProperties>>>();
        /// <summary>
        /// a function that allows you to run things on the main thread
        /// </summary>
        /// <param name="func">the function on the main thread</param>
        /// <returns></returns>
        public Task InvokeFunction(Action func) {
            // create a tcs
            TaskCompletionSource<OnUpdateProperties> tcs = new TaskCompletionSource<OnUpdateProperties>();
            // add the tcs to the list
            invokeFunctionList.Add(new KeyValuePair<Action, TaskCompletionSource<OnUpdateProperties>>(func, tcs));
            // wait for the tcs
            return tcs.Task;
        }

        void Start() {
            bool a = Utilities.Load;
            // run the OnStart
            OnStart(new OnStartProperties {

            });
        }
        void Update() {
            OnUpdateProperties props = new OnUpdateProperties {
                DeltaTime = Time.deltaTime,
            };
            // run all the functions asked to be run on main thread, and clear them from the list
            while (invokeFunctionList.Count != 0) {
                invokeFunctionList[0].Key();
                invokeFunctionList[0].Value.SetResult(props);
                invokeFunctionList.RemoveAt(0);
            }
            // run the OnUpdate
            OnUpdate(props);
        }
        /// <summary>
        /// overwrite this for a method that is run when the object starts existing
        /// </summary>
        /// <param name="onStartProperties">properties for stating</param>
        public virtual void OnStart(OnStartProperties onStartProperties) { }
        /// <summary>
        /// overwrite this for a method that is runs each frame
        /// </summary>
        /// <param name="onUpdateProperties">properties for updating</param>
        public virtual void OnUpdate(OnUpdateProperties onUpdateProperties) { }

        //listeners for all kinds of collision

        private void OnCollisionEnter(Collision collision) {
            onCollisionEnterListener.ForEach(x => x(collision));
        }

        private void OnCollisionExit(Collision collision) {
            onCollisionExitListener.ForEach(x => x(collision));
        }

        private void OnCollisionStay(Collision collision) {
            onCollisionStayListener.ForEach(x => x(collision));
        }


        private List<Action<Collision>> onCollisionEnterListener = new List<Action<Collision>>();

        public void AddOnCollisionEnterListener(Action<Collision> func) {
            onCollisionEnterListener.Add(func);
        }

        public void RemoveOnCollisionEnterListener(Action<Collision> func) {
            onCollisionEnterListener.Remove(func);
        }

        private List<Action<Collision>> onCollisionExitListener = new List<Action<Collision>>();

        public void AddOnCollisionExitListener(Action<Collision> func) {
            onCollisionExitListener.Add(func);
        }

        public void RemoveOnCollisionExitListener(Action<Collision> func) {
            onCollisionExitListener.Remove(func);
        }

        private List<Action<Collision>> onCollisionStayListener = new List<Action<Collision>>();

        public void AddOnCollisionStayListener(Action<Collision> func) {
            onCollisionStayListener.Add(func);
        }

        public void RemoveOnCollisionStayListener(Action<Collision> func) {
            onCollisionStayListener.Remove(func);
        }



        private void OnTriggerEnter(Collider collision) {
            onTriggerEnterListener.ForEach(x => x(collision));
        }

        private void OnTriggerExit(Collider collision) {
            onTriggerExitListener.ForEach(x => x(collision));
        }

        private void OnTriggerStay(Collider collision) {
            onTriggerStayListener.ForEach(x => x(collision));
        }

        private List<Action<Collider>> onTriggerEnterListener = new List<Action<Collider>>();

        public void AddOnTriggerEnterListener(Action<Collider> func) {
            onTriggerEnterListener.Add(func);
        }

        public void RemoveOnTriggerEnterListener(Action<Collider> func) {
            onTriggerEnterListener.Remove(func);
        }

        private List<Action<Collider>> onTriggerExitListener = new List<Action<Collider>>();

        public void AddOnTriggerExitListener(Action<Collider> func) {
            onTriggerExitListener.Add(func);
        }

        public void RemoveOnTriggerExitListener(Action<Collider> func) {
            onTriggerExitListener.Remove(func);
        }

        private List<Action<Collider>> onTriggerStayListener = new List<Action<Collider>>();

        public void AddOnTriggerStayListener(Action<Collider> func) {
            onTriggerStayListener.Add(func);
        }

        public void RemoveOnTriggerStayListener(Action<Collider> func) {
            onTriggerStayListener.Remove(func);
        }

    }
    public class OnStartProperties {

    }
    public class OnUpdateProperties {
        public float DeltaTime { get; set; }
    }
}