using UnityEngine;
using UnityEngine.Advertisements;

/**
 * バナー広告を表示するサンプル
 */
public class AdvertisementBannerExample : MonoBehaviour
{
    [SerializeField]
    private string appStoreGameId = "3228680";
    [SerializeField]
    private string googlePlayGameId = "3228681";
    [SerializeField]
    private bool testMode = false;
    [SerializeField]
    // バナー用のplacementはダッシュボードで新しく作る必要がある
    private string placementId = "banner";
    [SerializeField]
    private BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    private string _gameId = "";
    private BannerLoadOptions _bannerLoadOptions;
    private BannerOptions _bannerOptions;

    private void Start()
    {
        // 初期化
        InitUnityAds();
        
        // バナーをロードして表示
        //LoadAndShowBanner();
    }

    /**
     * 初期化
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
        
        // バナーを表示するポジションを設定する
        Advertisement.Banner.SetPosition(bannerPosition);
        
        // コールバックを設定する
        _bannerLoadOptions = new BannerLoadOptions()
        {
            loadCallback = LoadCallback,
            errorCallback = ErrorCallback
        };
        _bannerOptions = new BannerOptions()
        {
            showCallback = ShowCallback,
            hideCallback = HideCallback
        };
    }

    /**
     * バナー広告をロードする（コールバックなし）
     */
    public void LoadBanner()
    {
        Advertisement.Banner.Load(placementId);
    }

    /**
     * バナー広告をロードして表示する
     * （ここではロードが終わり次第コールバックでバナーを表示するようにしてある）
     */
    public void LoadAndShowBanner()
    {
        Advertisement.Banner.Load(placementId, _bannerLoadOptions);
    }

    /**
     * バナー表示位置を指定し、バナー広告をロードして表示する
     */
    public void LoadAndShowBanner(BannerPosition bannerPosition)
    {
        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Banner.Load(placementId, _bannerLoadOptions);
    }

    /**
     * バナー広告をロードし終えた際に呼ばれるコールバック
     */
    private void LoadCallback()
    {
        // バナー広告を表示する（コールバック有り）
        Advertisement.Banner.Show(placementId, _bannerOptions);
    }
    
    /**
     * バナー広告のロードでエラーの場合に呼ばれるコールバック
     */
    private void ErrorCallback(string message)
    {
        Debug.Log($"Unity Ads Banner Load Error {nameof(message)}: {message}");
    }
    
    /**
     * バナー広告を表示した際に呼ばれるコールバック
     */
    private void ShowCallback()
    {
        Debug.Log("Show Banner");
    }

    /**
     * バナー広告を非表示にした際に呼ばれるコールバック
     */
    private void HideCallback()
    {
        Debug.Log("Hide Banner");
    }
}