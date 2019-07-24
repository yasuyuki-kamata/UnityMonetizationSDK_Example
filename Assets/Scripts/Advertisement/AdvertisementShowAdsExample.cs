using System;
using UnityEngine;
using UnityEngine.Advertisements;

/**
 * Advertisementクラスを使った動画広告の表示サンプル
 * IUnityAdsListenerインターフェイスを実装している
 */
public class AdvertisementShowAdsExample : MonoBehaviour, IUnityAdsListener
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

    private void Start()
    {
        InitUnityAds();
    }

    private void OnDisable()
    {
        Advertisement.RemoveListener(this);
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

#if UNITY_ANDROID
        _gameId = googlePlayGameId;
#elif UNITY_IOS
        _gameId = appStoreGameId;
#endif
        
        // 初期化
        Advertisement.Initialize(_gameId, testMode);
        
        // リスナーをセットする
        Advertisement.AddListener(this);
    }

    /**
     * Advertisementクラスを使って動画広告を表示する
     */
    public void ShowAds()
    {
        // 指定のplacementが準備完了であるかどうかを確かめる
        // 簡易チェックでよければこれでOK
        //if (Advertisement.IsReady(placementId) == false) return;
        
        // 指定のplacementがどのような状態にあるかを調べる
        var placementState = Advertisement.GetPlacementState(placementId);
        switch (placementState)
        {
            case PlacementState.Ready:
                // 広告が表示できる状態のとき
                Debug.Log($"placement id: {placementId} Ready");
                break;
            case PlacementState.NotAvailable:
                // 広告枠が存在しないとき　placement idを間違えている場合はここ
                Debug.Log($"placement id: {placementId} Not Available");
                return;
            case PlacementState.Disabled:
                // 広告枠が無効化されているとき
                Debug.Log($"placement id: {placementId} Disabled");
                return;
            case PlacementState.NoFill:
                // 広告在庫切れのとき
                Debug.Log($"placement id: {placementId} No Fill");
                break;
            case PlacementState.Waiting:
                // 次の広告の準備中
                Debug.Log($"placement id: {placementId} Waiting");
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        // 広告を表示
        Advertisement.Show(placementId);
    }

    #region IUnityAdsListenerの実装
    
    /**
     * 広告が準備完了した時点で呼ばれる
     */
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log($"UnityAdsReady {nameof(placementId)}: {placementId}");
    }

    /**
     * 広告でエラーが発生したら呼ばれる
     */
    public void OnUnityAdsDidError(string message)
    {
        Debug.Log($"UnityAdsDidError {nameof(message)}: {message}");
    }

    /**
     * 動画広告の視聴開始時点で呼ばれる
     */
    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log($"UnityAdsDidStart {nameof(placementId)}: {placementId}");
    }

    /**
     * 動画広告の視聴完了時点で呼ばれる
     */
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log($"UnityAdsDidFinish {nameof(placementId)}: {placementId}," +
                  $" {nameof(showResult)}: {Enum.GetName(typeof(ShowResult), showResult)}");
        switch (showResult)
        {
            case ShowResult.Finished:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(showResult), showResult, null);
        }
    }
    
    #endregion
}
