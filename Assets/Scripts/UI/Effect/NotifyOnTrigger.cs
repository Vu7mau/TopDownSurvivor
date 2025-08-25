using UnityEngine;

public class NotifyOnContact : MonoBehaviour
{
    public enum ContactMode
    {
        TriggerEnter,
        TriggerStay,
        TriggerExit,
        CollisionEnter,
        CollisionStay,
        CollisionExit,
        TriggerEnter2D,
        TriggerStay2D,
        TriggerExit2D,
        CollisionEnter2D,
        CollisionStay2D,
        CollisionExit2D,
    }

    [Header("Contact")]
    [SerializeField] public ContactMode mode = ContactMode.TriggerEnter;

    [Header("Message")]
    [SerializeField, TextArea] private string message = "Nội dung thông báo";

    [Header("Filter")]
    [SerializeField] private bool requireTag = false;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private LayerMask layerMask = ~0; // mặc định: mọi layer

    [Header("Trigger Options")]
    [SerializeField] private bool onlyOnce = true;
    [SerializeField] private float cooldown = 0f; // giây
    private bool _done = false;
    private float _lastTime = -9999f;

    [Header("Dialogue Overrides (tuỳ chọn)")]
    [SerializeField] private bool overrideDuration = false;
    [SerializeField, Min(0.1f)] private float duration = 1.5f;

    [SerializeField] private bool overrideAnchor = false;
    [SerializeField] private DialogueAnchor anchor = DialogueAnchor.Footer;

    [SerializeField] private bool overrideTypingSpeed = false;
    [Tooltip("Giây / ký tự. <= 0 để hiện ngay.")]
    [SerializeField] private float typingSpeedPerChar = 0.03f;

    [SerializeField] private bool overrideSpeaker = false;
    [SerializeField] private string speakerName = "System";
    [SerializeField] private Sprite speakerAvatar;

    [Header("SFX (optional)")]
    [SerializeField] private AudioClip sfx;

    private void Reset()
    {
        // Nếu chọn Trigger* thì cố gắng đặt collider về trigger cho thuận tiện
        if (IsTriggerMode(mode))
        {
            var col = GetComponent<Collider>();
            if (col) col.isTrigger = true;

            var col2D = GetComponent<Collider2D>();
            if (col2D) col2D.isTrigger = true;
        }
    }

    // =========================
    // 3D TRIGGER
    // =========================
    private void OnTriggerEnter(Collider other)
    {
        if (mode == ContactMode.TriggerEnter)
            TryFire(other.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (mode == ContactMode.TriggerStay)
            TryFire(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (mode == ContactMode.TriggerExit)
            TryFire(other.gameObject);
    }

    // =========================
    // 3D COLLISION
    // =========================
    private void OnCollisionEnter(Collision collision)
    {
        if (mode == ContactMode.CollisionEnter)
            TryFire(collision.collider.gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (mode == ContactMode.CollisionStay)
            TryFire(collision.collider.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (mode == ContactMode.CollisionExit)
            TryFire(collision.collider.gameObject);
    }

    // =========================
    // 2D TRIGGER
    // =========================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mode == ContactMode.TriggerEnter2D)
            TryFire(other.gameObject);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (mode == ContactMode.TriggerStay2D)
            TryFire(other.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (mode == ContactMode.TriggerExit2D)
            TryFire(other.gameObject);
    }

    // =========================
    // 2D COLLISION
    // =========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (mode == ContactMode.CollisionEnter2D)
            TryFire(collision.collider.gameObject);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (mode == ContactMode.CollisionStay2D)
            TryFire(collision.collider.gameObject);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (mode == ContactMode.CollisionExit2D)
            TryFire(collision.collider.gameObject);
    }

    // =========================
    // CORE
    // =========================
    private void TryFire(GameObject other)
    {
        if (!PassesFilter(other)) return;

        if (onlyOnce && _done) return;
        if (Time.time - _lastTime < cooldown) return;

        _lastTime = Time.time;
        if (onlyOnce) _done = true;

        float? time = overrideDuration ? duration : (float?)null;
        DialogueAnchor? anch = overrideAnchor ? anchor : (DialogueAnchor?)null;
        float? typing = overrideTypingSpeed ? typingSpeedPerChar : (float?)null;
        string spk = overrideSpeaker ? speakerName : null;
        Sprite avt = overrideSpeaker ? speakerAvatar : null;

        var notify = ChatNotify.Instance;
        if (notify == null)
        {
            Debug.LogWarning("[NotifyOnContact] ChatNotify.Instance chưa tồn tại trong scene.");
            return;
        }

        notify.SayCustom(
            msg: string.IsNullOrWhiteSpace(message) ? "(empty)" : message,
            sfx: sfx,
            time: time,
            anch: anch,
            speakerOverride: spk,
            avatarOverride: avt,
            typingSpeed: typing
        );
    }

    private bool PassesFilter(GameObject other)
    {
        if (((1 << other.layer) & layerMask) == 0) return false;
        if (requireTag && !other.CompareTag(targetTag)) return false;
        return true;
    }

    private static bool IsTriggerMode(ContactMode m)
    {
        return m == ContactMode.TriggerEnter
            || m == ContactMode.TriggerStay
            || m == ContactMode.TriggerExit
            || m == ContactMode.TriggerEnter2D
            || m == ContactMode.TriggerStay2D
            || m == ContactMode.TriggerExit2D;
    }
}
