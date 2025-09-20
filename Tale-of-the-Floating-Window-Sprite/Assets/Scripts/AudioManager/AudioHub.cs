using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    /// <summary>
    /// ��Чϵͳ�ܹܣ��������ء�ͨ������������ơ�BGM ���뵭����
    /// </summary>
    public class AudioHub : MonoBehaviour, IAudioPlayer
    {
        public static AudioHub Instance { get; private set; }

        [Header("��������")]
        [SerializeField] private GameObject audioPrefab;
        [SerializeField] private int initPoolSize = 16;
        [SerializeField] private bool dontDestroyOnLoad = true;

        [Header("������")]
        public AudioMixerGroup bgmGroup;
        public AudioMixerGroup sfxGroup;
        public AudioMixerGroup uiGroup;

        [Header("ͨ������")]
        public int sfxVoices = 16;
        public int uiVoices = 8;

        private Queue<AudioSourceWrapper> pool = new Queue<AudioSourceWrapper>();
        private Dictionary<AudioChannel, LinkedList<AudioSourceWrapper>> activeByChannel;

        // �������ƣ����ⰴť���ˢ��Ч
        private Dictionary<AudioClip, float> lastPlayedTime = new Dictionary<AudioClip, float>();

        // BGM ˫ͨ�����뵭��
        private AudioSource bgmA, bgmB;
        private bool bgmAIsActive = true;
        private Coroutine bgmCrossRoutine;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

            // ��ʼ����
            for (int i = 0; i < initPoolSize; i++) CreateNew();

            // ��ʼ����Ծ�б�
            activeByChannel = new Dictionary<AudioChannel, LinkedList<AudioSourceWrapper>>
            {
                { AudioChannel.SFX, new LinkedList<AudioSourceWrapper>() },
                { AudioChannel.UI, new LinkedList<AudioSourceWrapper>() }
            };

            // ��ʼ�� BGM ˫Դ
            bgmA = CreateBgmSource("BGM_A");
            bgmB = CreateBgmSource("BGM_B");
        }

        private AudioSource CreateBgmSource(string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.loop = true;
            src.playOnAwake = false;
            src.outputAudioMixerGroup = bgmGroup;
            src.spatialBlend = 0f;
            return src;
        }

        private AudioSourceWrapper CreateNew()
        {
            var go = Instantiate(audioPrefab, transform);
            var wrapper = go.GetComponent<AudioSourceWrapper>();
            wrapper.Init(this);
            go.SetActive(false);
            pool.Enqueue(wrapper);
            return wrapper;
        }

        private AudioSourceWrapper GetFromPool()
        {
            if (pool.Count == 0) CreateNew();
            return pool.Dequeue();
        }

        public void ReturnToPool(AudioSourceWrapper wrapper)
        {
            wrapper.gameObject.SetActive(false);
            pool.Enqueue(wrapper);
        }

        // ----------- Active������ -----------
        public void NotifyActive(AudioSourceWrapper wrapper, AudioChannel channel)
        {
            if (channel == AudioChannel.BGM) return;
            var list = activeByChannel[channel];
            list.AddLast(wrapper);

            int limit = (channel == AudioChannel.SFX) ? sfxVoices : uiVoices;
            if (list.Count > limit)
            {
                // ��̭�����
                list.First.Value.Stop();
            }
        }

        public void NotifyInactive(AudioSourceWrapper wrapper, AudioChannel channel)
        {
            if (channel == AudioChannel.BGM) return;
            activeByChannel[channel].Remove(wrapper);
        }

        // ----------- IAudioPlayerʵ�� -----------

        public void PlayOneShot(AudioClip clip, float volume = 1f, AudioChannel channel = AudioChannel.SFX, float pitch = 1f, float throttleInterval = 0f)
        {
            if (!clip) return;
            if (IsThrottled(clip, throttleInterval)) return;

            var wrapper = GetFromPool();
            var group = GetGroup(channel);

            wrapper.PlayOneShot(clip, volume, pitch, channel, group, null,
                channel == AudioChannel.UI ? 0f : 1f); // UI=2D, ����=3D
        }

        public void PlayAtPosition(AudioClip clip, Vector3 pos, float volume = 1f, AudioChannel channel = AudioChannel.SFX, float pitch = 1f, float spatialBlend = 1f, float throttleInterval = 0f)
        {
            if (!clip) return;
            if (IsThrottled(clip, throttleInterval)) return;

            var wrapper = GetFromPool();
            var group = GetGroup(channel);
            wrapper.PlayOneShot(clip, volume, pitch, channel, group, pos, spatialBlend);
        }

        public IAudioHandle PlayLoop(AudioClip clip, AudioChannel channel = AudioChannel.SFX, float volume = 1f, float pitch = 1f, float spatialBlend = 1f)
        {
            if (!clip) return null;
            var wrapper = GetFromPool();
            var group = GetGroup(channel);
            wrapper.PlayLoop(clip, volume, pitch, channel, group, null, spatialBlend);
            return wrapper;
        }

        public void StopAllOnChannel(AudioChannel channel)
        {
            if (channel == AudioChannel.BGM) { StopBGM(); return; }
            var list = activeByChannel[channel];
            foreach (var w in new List<AudioSourceWrapper>(list))
                w.Stop();
            list.Clear();
        }

        // ----------- BGM ���뵭�� -----------

        public void PlayBGM(AudioClip clip, float fadeSeconds = 0.75f, float targetVolume = 1f)
        {
            if (!clip) return;

            var from = bgmAIsActive ? bgmA : bgmB;
            var to = bgmAIsActive ? bgmB : bgmA;
            bgmAIsActive = !bgmAIsActive;

            to.clip = clip;
            to.volume = 0f;
            to.Play();

            if (bgmCrossRoutine != null) StopCoroutine(bgmCrossRoutine);
            bgmCrossRoutine = StartCoroutine(CrossFade(from, to, fadeSeconds, targetVolume));
        }

        public void StopBGM(float fadeSeconds = 0.5f)
        {
            var active = bgmAIsActive ? bgmA : bgmB;
            if (bgmCrossRoutine != null) StopCoroutine(bgmCrossRoutine);
            StartCoroutine(FadeOutAndStop(active, fadeSeconds));
        }

        private IEnumerator CrossFade(AudioSource from, AudioSource to, float dur, float targetVol)
        {
            float t = 0f;
            float fromStart = from ? from.volume : 0f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                float k = t / dur;
                if (from) from.volume = Mathf.Lerp(fromStart, 0f, k);
                to.volume = Mathf.Lerp(0f, targetVol, k);
                yield return null;
            }
            if (from) from.Stop();
            to.volume = targetVol;
        }

        private IEnumerator FadeOutAndStop(AudioSource src, float dur)
        {
            float start = src.volume;
            float t = 0f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                src.volume = Mathf.Lerp(start, 0f, t / dur);
                yield return null;
            }
            src.Stop();
            src.volume = start;
        }

        // ----------- ���ߺ��� -----------

        private AudioMixerGroup GetGroup(AudioChannel ch)
        {
            switch (ch)
            {
                case AudioChannel.BGM: return bgmGroup;
                case AudioChannel.UI: return uiGroup ? uiGroup : sfxGroup;
                default: return sfxGroup;
            }
        }

        private bool IsThrottled(AudioClip clip, float interval)
        {
            if (interval <= 0f) return false;
            float now = Time.unscaledTime;
            if (lastPlayedTime.TryGetValue(clip, out var last) && now - last < interval) return true;
            lastPlayedTime[clip] = now;
            return false;
        }
    }
}
