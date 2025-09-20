using UnityEngine;

public class FireflyController : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float minSize = 0.1f;
    public float maxSize = 0.3f;
    public float rotationSpeed = 50f;
    public Vector3 targetPosition;
    
    private new ParticleSystem particleSystem; // Changed here
    private ParticleSystem.Particle[] particles;
    private Transform cachedTransform;
    private Vector3[] positions;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        cachedTransform = transform;
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        positions = new Vector3[particles.Length];
    }

    void Update()
    {
        int count = particleSystem.GetParticles(particles);
        for (int i = 0; i < count; i++)
        {
            // Move particles towards the target position
            particles[i].position = Vector3.MoveTowards(particles[i].position, targetPosition, Random.Range(minSpeed, maxSpeed) * Time.deltaTime);

            // Randomize particle size
            float newSize = Random.Range(minSize, maxSize);
            particles[i].startSize = newSize;

            // Apply rotation to particles
            Quaternion rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            particles[i].rotation3D += rotation.eulerAngles * rotationSpeed * Time.deltaTime;

            positions[i] = particles[i].position;
        }

        particleSystem.SetParticles(particles, count);
        cachedTransform.position = targetPosition;
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }
}
