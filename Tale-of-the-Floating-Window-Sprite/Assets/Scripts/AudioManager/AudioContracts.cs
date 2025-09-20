using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    /// <summary>
    /// ��Ƶͨ������
    /// </summary>
    public enum AudioChannel
    {
        BGM,
        SFX,
        UI
    }
    ///<summary>
    ///��ѭ�����ŵľ��
    ///</summary>
    public interface IAudioHandle
    {
        bool IsPlaying { get; }
        AudioSource Source { get; }
        void Stop(float fadeOut = 0f);
    }
    ///<summary>///
    ///����ͳһ����Ƶ���Žӿ�
    ///</summary>
    public interface IAudioPlayer
    {
        //һ���Զ���Ч
        void PlayOneShot(AudioClip clip, float volume = 1f, AudioChannel channel = AudioChannel.SFX, float pitch = 1f, float throttleInterval = 0f);
        //��������2d/3d��Ч spatialblendΪ2d/3d���
        void PlayAtPosition(AudioClip clip, Vector3 pos, float volume = 1f, AudioChannel channel = AudioChannel.SFX, float pitch = 1f, float spatialBlend = 1f, float throttleInterval = 0f);
        //ѭ����
        IAudioHandle PlayLoop(AudioClip clip, AudioChannel channel = AudioChannel.SFX, float volume = 1f, float pitch = 1f, float spatialBlend = 1f);

        //�л� ����bgm
        void PlayBGM(AudioClip clip, float fadeSeconds = 0.75f, float targetVolume = 1f);
        //ֹͣbgm
        void StopBGM(float fadeSeconds = 0.5f);

        //ͣ��ĳ��ͨ���ϵ���������
        void StopAllOnChannel(AudioChannel channel);
    }
}
