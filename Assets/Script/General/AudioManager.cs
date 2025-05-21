using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using System.Diagnostics;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    // 音ファイル
    [SerializeField] AudioClip[] _seLists;
    [SerializeField] AudioClip[] _bgmLists;

    //音の鳴らし方の指定
    [Header("SEのAudioSource")]
    [SerializeField] public AudioSource _audioSourceSE;
    [Header("BGMのAudioSource")]
    [SerializeField] public AudioSource _audioSourceBGM;

    [Header("スライダー")]
    public Slider SESlider;
    public Slider BGMSlider;

    private float seVolume = 0.25f;
    private float bgmVolume = 0.25f;

    private int BuildIndex;

    private void Start()
    {
        LoadVolumeSetting();

        if(SESlider != null && BGMSlider != null)
        {
            InitializeSliders();
        }
        if(_audioSourceSE == null)
        {
            _audioSourceSE = gameObject.AddComponent<AudioSource>();
        }
        if (_audioSourceBGM != null)
        {
            _audioSourceBGM = gameObject.AddComponent<AudioSource>();
        }

        PlayBGM(0);
    }

    /// <summary>
    /// スライダー初期化
    /// </summary>
    public void InitializeSliders()
    {
        SESlider = GameObject.Find("Canvas/SESlider").GetComponent<Slider>();
        BGMSlider = GameObject.Find("Canvas/BGMSlider").GetComponent<Slider>();

        //スライダー初期値を反映
        SESlider.value = seVolume;
        BGMSlider.value = bgmVolume;


        //イベントリスナーを登録
        SESlider.onValueChanged.RemoveAllListeners();
        SESlider.onValueChanged.AddListener(delegate { OnSEVolumeChange(); });

        BGMSlider.onValueChanged.RemoveAllListeners();
        BGMSlider.onValueChanged.AddListener(delegate { OnBGMVolumeChange(); });


    }

    #region シーンを移動してSettingシーン戻ってきた際の処理
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BuildIndex = scene.buildIndex; //ビルド番号を取得

        if (scene.name == "Setting") // Settingシーンにいるときだけ処理を実行
        {
            InitializeSliders();
        }
    }
    #endregion

    #region SE・BGM操作
    /// <summary>
    /// SEの値が変更されたときの処理
    /// </summary>
    public void OnSEVolumeChange()
    {
        seVolume = SESlider.value;
        _audioSourceSE.volume = SESlider.value;
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// BGMの値が変更されたときの処理
    /// </summary>
    public void OnBGMVolumeChange()
    {
        bgmVolume = BGMSlider.value;
        _audioSourceBGM.volume = BGMSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();
    }

    public float SEVolume
    {
        get { return _audioSourceSE.volume; }
        set { _audioSourceSE.volume = value;}
    }

    public float BGMVolume
    {
        get { return _audioSourceBGM.volume; }
        set { _audioSourceBGM.volume = value; }
    }
    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="index">再生したいSEList番号</param>
    public void PlaySound(int index)
    {
        _audioSourceSE.clip = _seLists[index];
        _audioSourceSE.PlayOneShot(_seLists[index]);
    }
    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="index">再生したいBGMList番号</param>
    public void PlayBGM(int index)
    {
        _audioSourceBGM.clip = _bgmLists[index];
        _audioSourceBGM.Play();
    }
    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM()
    {
        _audioSourceBGM.Stop();
    }
    /// <summary>
    /// オーバーライド
    /// SE再生
    /// </summary>
    /// <param name="index">再生したいSEList番号</param>
    /// <param name="_audioSource">SEのAudioSource</param>
    public void PlaySound(int index, AudioSource _audioSource)
    {
        _audioSourceSE.clip = _seLists[index];
        _audioSourceSE.PlayOneShot(_seLists[index]);
    }
    /// <summary>
    /// オーバーライド
    /// BGM再生
    /// </summary>
    /// <param name="index">再生したいBGMList番号</param>
    /// <param name="_audioSource">BGMのAudioSource</param>
    public void PlayBGM(int index, AudioSource _audioSource)
    {
        _audioSourceBGM.clip = _bgmLists[index];
        _audioSourceBGM.Play();
    }
    /// <summary>
    /// オーバーライド
    /// BGM停止
    /// </summary>
    /// <param name="_audioSource">BGMのAudioSource</param>
    public void StopBGM(AudioSource _audioSource)
    {
        _audioSourceBGM.Stop();
    }

    #endregion

    #region BGM・SEの音量をゲーム終了時に保存・ロードする
    /// <summary>
    /// 音量を保存する
    /// </summary>
    private void SaveVolumeSetting()
    {
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// 音量設定をロードする
    /// </summary>
    private void LoadVolumeSetting()
    {
        if(PlayerPrefs.HasKey("SEVolume") && PlayerPrefs.HasKey("BGMVolume"))
        {
            seVolume = PlayerPrefs.GetFloat("SEVolume");
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        }

        _audioSourceSE.volume = seVolume;
        _audioSourceBGM.volume = bgmVolume;
    }
    /// <summary>
    /// ゲーム終了時に音量保存
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveVolumeSetting();
    }
    #endregion
}   