using System;
using UnityEngine;
using UnityEngine.Monetization;
using ShowResult = UnityEngine.Monetization.ShowResult;

/**
 * Monetizationクラスで動画広告等を表示するサンプル
 */
public class MonetizationShowAdsExample : MonoBehaviour
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
    private ShowAdCallbacks _callbacks;

    private void Start()
    {
        // 初期化
        InitUnityAds();
    }
    
    /**
     * Monetizationクラスを使って初期化する
     */
    private void InitUnityAds()
    {
        // 動画広告をサポートしていないプラットフォームだったらreturn
        if (Monetization.isSupported == false) return;
        
        // すでに初期化していたらreturn
        if (Monetization.isInitialized) return;

#if UNITY_ANDROID
        _gameId = googlePlayGameId;
#elif UNITY_IOS
        _gameId = appStoreGameId;
#endif

        // 初期化
        Monetization.Initialize(_gameId, testMode);
        
        // 広告視聴のコールバックを設定
        _callbacks = new ShowAdCallbacks()
        {
            startCallback = OnAdStart,
            finishCallback = OnAdFinished
        };
        
        // PlacementContentのコールバックを設定
        Monetization.onPlacementContentReady += OnPlacementContentReady;
        Monetization.onPlacementContentStateChange += OnPlacementContentStateChange;
    }

    /**
     * Monetizationクラスを使って動画広告を表示する
     */
    public void ShowAds()
    {
        // 指定のplacementが準備完了であるかどうかを確かめる
        // 簡易チェックでよければこれでOK
        //if (Monetization.IsReady(placementId) == false) return;

        // PlacementContentを取得する
        var placementContent =
            (ShowAdPlacementContent) Monetization.GetPlacementContent(placementId);

        // PlacementContentの状態を確認する
        switch (placementContent.state)
        {
            case PlacementContentState.Ready:
                // 広告が表示できる状態のとき
                Debug.Log($"placement id: {placementId} Ready");
                break;
            case PlacementContentState.NotAvailable:
                // 広告枠が存在しないとき　placement idを間違えている場合はここ
                Debug.Log($"placement id: {placementId} Not Available");
                return;
            case PlacementContentState.Disabled:
                // 広告枠が無効化されているとき
                Debug.Log($"placement id: {placementId} Disabled");
                return;
            case PlacementContentState.NoFill:
                // 広告在庫切れのとき
                Debug.Log($"placement id: {placementId} No Fill");
                break;
            case PlacementContentState.Waiting:
                // 次の広告の準備中
                Debug.Log($"placement id: {placementId} Waiting");
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        // 動画広告を表示（コールバックなし）
        //placementContent.Show();
        
        // 動画視聴完了後のコールバックを指定して広告を表示
        //placementContent.Show(OnAdFinished);
        
        // 動画広告視聴時、完了後のコールバックを指定して広告を表示
        placementContent.Show(_callbacks);
    }

    #region イベント用の関数

    /**
     * 広告視聴開始時に呼ばれる関数
     */
    private void OnAdStart()
    {
        Debug.Log("Ad Start");
    }

    /**
     * 広告視聴完了時に呼ばれる関数
     * @param ShowResult result 広告視聴完了時の結果ステータス
     */
    private void OnAdFinished(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                // 動画視聴を完了したとき
                Debug.Log("Ad Finished");
                break;
            case ShowResult.Skipped:
                // 動画視聴をスキップして完了したとき
                Debug.Log("Ad Skipped");
                break;
            case ShowResult.Failed:
                // 動画視聴が失敗したとき
                Debug.Log("Ad Failed");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result), result, null);
        }
    }

    /**
     * PlacementContentがReady（準備完了）になった時に呼ばれる関数
     */
    private void OnPlacementContentReady(object sender, PlacementContentReadyEventArgs e)
    {
        Debug.Log($"PlacementContentReady {nameof(sender)}: {sender}," +
                  $" {nameof(e.placementId)}: {e.placementId}," +
                  $" {nameof(e.placementContent)}: {e.placementContent}");
    }

    /**
     * PlacementContentStateが変化した時に呼ばれる関数
     */
    private void OnPlacementContentStateChange(object sender, PlacementContentStateChangeEventArgs e)
    {
        Debug.Log($"PlacementContentStateChange {nameof(sender)}: {sender}," +
                  $" {nameof(e.placementId)}: {e.placementId}," +
                  $" {nameof(e.placementContent)}: {e.placementContent}," +
                  $" {nameof(e.previousState)}: {e.previousState}," +
                  $" {nameof(e.newState)}: {e.newState}");
    }

    #endregion
}
