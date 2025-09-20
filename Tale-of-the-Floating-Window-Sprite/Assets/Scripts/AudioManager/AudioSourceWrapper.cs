using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    /// <summary>
    /// ��Ч����ĵ��������ˡ������𲥷���Ч�����ڲ��Ž�����黹�����ӡ�
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceWrapper : MonoBehaviour, IAudioHandle
    {
        private AudioSource source;      // Unity ����Դ
        private AudioHub hub;            // �����ܹܣ������黹�Լ�
        private AudioChannel channel;    // ��ǰ����ͨ��
        private bool isLoop;             // �Ƿ�ѭ������
        private Coroutine lifeRoutine;   // �����������ڵ�Э��

        // IAudioHandle�ӿ�ʵ��
        public bool IsPlaying => source && source.isPlaying;
        public AudioSource Source => source;

        /// <summary>��ʼ������ AudioHub ���ã���</summary>
        public void Init(AudioHub hubRef)
        {
            hub = hubRef;
            source = GetComponent<AudioSource>();
            gameObject.SetActive(false);
        }

        /// <summary>����һ������Ч����ѭ������</summary>
        public void PlayOneShot(AudioClip clip, float volume, float pitch, AudioChannel ch, AudioMixerGroup group, Vector3? worldPos, float spatialBlend)
        {
            ConfigureSource(group, volume, pitch, spatialBlend, loop: false);
            channel = ch;
            isLoop = false;

            if (worldPos.HasValue) transform.position = worldPos.Value;
            gameObject.SetActive(true);

            source.clip = clip;
            source.Play();

            // Э�̵ȴ�������� �� ����
            lifeRoutine = StartCoroutine(ReturnWhenFinished());
            hub.NotifyActive(this, ch);
        }

        /// <summary>ѭ�����ţ��ⲿ��Ҫ Stop����</summary>
        public void PlayLoop(AudioClip clip, float volume, float pitch, AudioChannel ch, AudioMixerGroup group, Vector3? worldPos, float spatialBlend)
        {
            ConfigureSource(group, volume, pitch, spatialBlend, loop: true);
            channel = ch;
            isLoop = true;

            if (worldPos.HasValue) transform.position = worldPos.Value;
            gameObject.SetActive(true);

            source.clip = clip;
            source.Play();
            hub.NotifyActive(this, ch);
        }

        private void ConfigureSource(AudioMixerGroup group, float volume, float pitch, float spatialBlend, bool loop)
        {
            source.outputAudioMixerGroup = group;
            source.volume = Mathf.Clamp01(volume);
            source.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
            source.loop = loop;
            source.spatialBlend = Mathf.Clamp01(spatialBlend); // 0=2D, 1=3D
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 1f;
            source.maxDistance = 30f;
        }

        private IEnumerator ReturnWhenFinished()
        {
            while (source && source.isPlaying) yield return null;
            StopAndReturnImmediate();
        }

        public void Stop(float fadeOut = 0f)
        {
            if (!source) return;
            if (fadeOut <= 0f) { StopAndReturnImmediate(); return; }
            StartCoroutine(FadeOutThenStop(fadeOut));
        }

        private IEnumerator FadeOutThenStop(float dur)
        {
            float startVol = source.volume;
            float t = 0f;
            while (t < dur && source)
            {
                t += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(startVol, 0f, t / dur);
                yield return null;
            }
            StopAndReturnImmediate();
        }

        private void StopAndReturnImmediate()
        {
            if (lifeRoutine != null) StopCoroutine(lifeRoutine);
            if (source) source.Stop();

            hub.NotifyInactive(this, channel);   // �����ܹܣ���������
            hub.ReturnToPool(this);              // �����س�
        }
    }
}
