using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class PlayTimelineController : MonoBehaviour
{
    public enum PlayMode { Restart, Resume }
    public enum UpdateMode { GameTime, UnscaledGameTime }

    [Header("Timeline")]
    public PlayableDirector timeline;
    public PlayMode playMode = PlayMode.Restart;
    public UpdateMode updateMode = UpdateMode.GameTime;

    [Header("Playback Options")]
    [Tooltip("Nếu bật, timeline chỉ được phép chạy đúng 1 lần. Các lần gọi sau sẽ bị bỏ qua cho đến khi ResetOneTimeLock().")]
    public bool playOneTime = false;

    [Header("Events")]
    public UnityEvent onTimelineStarted;
    public UnityEvent onTimelineCompleted;

    private bool _hasPlayed = false;

    private void Awake()
    {
        if (timeline != null)
        {
            timeline.stopped -= OnStopped;
            timeline.stopped += OnStopped;
        }
    }


    public void ResetOneTimeLock()
    {
        _hasPlayed = false;
    }

    /// <summary>
    /// Gọi hàm này từ code hoặc Inspector để play Timeline.
    /// Tôn trọng cài đặt playOneTime.
    /// </summary>
    public void PlayTimeline()
    {
        if (timeline == null)
        {
            Debug.LogWarning("[PlayTimelineController] Chưa gán PlayableDirector!");
            return;
        }

        // Nếu chỉ cho chạy 1 lần và đã chạy rồi -> bỏ qua
        if (playOneTime && _hasPlayed)
            return;

        timeline.timeUpdateMode = (updateMode == UpdateMode.GameTime)
            ? DirectorUpdateMode.GameTime
            : DirectorUpdateMode.UnscaledGameTime;

        if (playMode == PlayMode.Restart)
        {
            if (timeline.state == PlayState.Playing)
                timeline.Stop();

            timeline.time = 0;
            timeline.Evaluate(); // áp frame đầu, tránh nhấp nháy
            timeline.Play();
        }
        else // Resume
        {
            if (timeline.state != PlayState.Playing)
            {
                if (timeline.duration > 0 && timeline.time >= timeline.duration)
                    timeline.time = 0;

                timeline.Play();
            }
            else
            {
                // đang chạy rồi thì không làm gì thêm
                return;
            }
        }

        // Đánh dấu là đã chạy nếu bật chế độ one-time
        if (playOneTime)
            _hasPlayed = true;

        onTimelineStarted?.Invoke();
    }

    public void StopTimeline()
    {
        if (timeline != null && timeline.state == PlayState.Playing)
            timeline.Stop();
    }

    private void OnStopped(PlayableDirector d)
    {
        onTimelineCompleted?.Invoke();
    }
}
