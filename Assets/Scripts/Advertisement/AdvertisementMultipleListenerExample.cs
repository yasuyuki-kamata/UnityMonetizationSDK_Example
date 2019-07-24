using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Advertisements;
using ShowResult = UnityEngine.Advertisements.ShowResult;

/**
 * Advertisementクラスでリスナーを複数登録するサンプル
 */
public class AdvertisementMultipleListenerExample : MonoBehaviour
{
    [SerializeField]
    private string appStoreGameId = "3228680";
    [SerializeField]
    private string googlePlayGameId = "3228681";
    [SerializeField]
    private bool testMode = false;
    [SerializeField]
    private string placementId = "rewardedVideo";

    private string _gameId = "";
    
    public List<IUnityAdsListener> unityAdsListeners = new List<IUnityAdsListener>()
    {
        new UnityAdsListenerExample1(),
        new UnityAdsListenerExample2()
    };

    private void Start()
    {
#if UNITY_ANDROID
        _gameId = googlePlayGameId;
#elif UNITY_IOS
        _gameId = appStoreGameId;
#endif
        
        InitUnityAds();
    }

    /**
     * Advertisementクラスを使って初期化する
     */
    private void InitUnityAds()
    {
        // 動画広告をサポートしていないプラットフォームだったらreturn
        if (Advertisement.isSupported == false) return;
        
        // すでに初期化していたらreturn
        if (Advertisement.isInitialized) return;

        // 初期化
        Advertisement.Initialize(_gameId, testMode);
        
        // リスナーを追加する
        foreach (var listener in unityAdsListeners)
        {
            Advertisement.AddListener(listener);
        }
    }

    private void OnDisable()
    {
        // リスナーを削除する
        foreach (var listener in unityAdsListeners)
        {
            Advertisement.RemoveListener(listener);
        }
    }

    /**
     * 広告を表示する
     */
    public void ShowAds()
    {
        if (Advertisement.IsReady(placementId) == false) return;
        
        Advertisement.Show(placementId);
    }

    /**
     * UnityAdsListenerのサンプルクラス1
     */
    private class UnityAdsListenerExample1 : IUnityAdsListener
    {
        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" placement id: {placementId}");
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" message: {message}");
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" placement id: {placementId}");
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" placement id: {placementId}," +
                      $" show result: {Enum.GetName(typeof(ShowResult), showResult)}");
        }
    }
    
    /**
     * UnityAdsListenerのサンプルクラス2
     */
    private class  UnityAdsListenerExample2 : IUnityAdsListener
    {
        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" placement id: {placementId}");
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" message: {message}");
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" placement id: {placementId}");
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            Debug.Log($"class name: {GetType().Name}," +
                      $" method name: {MethodBase.GetCurrentMethod().Name}," +
                      $" placement id: {placementId}," +
                      $" show result: {Enum.GetName(typeof(ShowResult), showResult)}");
        }
    }
}

