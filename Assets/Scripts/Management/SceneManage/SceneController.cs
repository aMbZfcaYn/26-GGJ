using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utilities;

namespace Management.SceneManage
{
    public class SceneController : GenericSingleton<SceneController>
    {
        public event Action BeforeOldSceneUnload;

        public Scene CurrentScene => SceneManager.GetActiveScene();

        [SerializeField] [SceneName] private string startSceneName;

        //[SerializeField] private string mainSceneName;
        //[SerializeField] private CanvasGroup fadeCanvasGroup;
        //[SerializeField] private float fadeDuration = 0.75f;
        [SerializeField] private AstarPath astarPath;

        public void LoadScene(string newSceneName)
        {
            print("Loading" + newSceneName);
            StartCoroutine(SwitchScene(CurrentScene.name, newSceneName));
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (!astarPath)
                astarPath = GameObject.Find("A*").GetComponent<AstarPath>();
        }

        private void Start()
        {
            //StartCoroutine(SwitchScene(string.Empty, startSceneName));
        }

        private IEnumerator SwitchScene(string oldSceneName, string newSceneName)
        {
            // TODO: back to yield break
            if (newSceneName == SceneManager.GetActiveScene().name)
                //yield break;
                Debug.Log("Switching from " + oldSceneName);

            if (oldSceneName != string.Empty)
            {
                //yield return FadeTransition(1);
                BeforeOldSceneUnload?.Invoke();
                yield return SceneManager.UnloadSceneAsync(oldSceneName);
            }

            //if (oldSceneName == mainSceneName || newSceneName == mainSceneName)
            //    GlobalGUIManager.Instance.ToggleMainGameGroup();

            yield return SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName));

            astarPath.Scan();

            Debug.Log(SceneManager.GetActiveScene().name);
            GameEventManager.Instance.OnNewSceneLoaded?.Invoke();

            //yield return FadeTransition(0);
        }

        //private IEnumerator FadeTransition(float targetAlpha)
        //{
        //    fadeCanvasGroup.blocksRaycasts = true;

        //    float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        //    while (Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) > 1e-3f)
        //    {
        //        fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
        //        yield return null;
        //    }

        //    fadeCanvasGroup.blocksRaycasts = false;
        //}
    }
}