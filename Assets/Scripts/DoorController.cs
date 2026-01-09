using UnityEngine;

public class DoorAutoClose : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;        // Starting angle
    public float closeSpeed = 3f;
    public float closeDelay = 5f;

    [Header("Effects")]
    public AudioSource closeSound;       // Sound to play on close
    public GameObject closeParticle;     // Particle prefab to spawn

    private Quaternion openRotation;
    private Quaternion closedRotation;
    private bool shouldClose = false;
    private bool isClosed = false;
    private bool effectsPlayed = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        // Door starts open
        transform.rotation = openRotation;

        // Schedule close
        Invoke(nameof(StartClosing), closeDelay);
    }

    void Update()
    {
        if (shouldClose && !isClosed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * closeSpeed);

            if (Quaternion.Angle(transform.rotation, closedRotation) < 0.5f)
            {
                transform.rotation = closedRotation;
                isClosed = true;

                PlayCloseEffects();
            }
        }
    }

    private void StartClosing()
    {
        shouldClose = true;
    }

    private void PlayCloseEffects()
    {
        if (effectsPlayed) return;
        effectsPlayed = true;

        if (closeSound != null)
            closeSound.Play();

        if (closeParticle != null)
            Instantiate(closeParticle, transform.position, Quaternion.identity);
    }
}
