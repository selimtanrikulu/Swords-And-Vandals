using UnityEngine;

public class Projectile : SkillImpact
{
    public float hitOffset = 0f;
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    [HideInInspector] public Vector3 targetPosition;
    [SerializeField] public float projectileSpeed;

    new void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            
            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject,5);
	}

    new void Update()
    {
        Vector3 dir = (targetPosition - transform.position);
        dir.Normalize();
        rb.velocity = dir * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ControllerBase controllerBase))
        {
            if (creator == controllerBase)
            {
                return;
            }
        }
        
        
        
        Vector3 position = transform.position;
        Vector3 collisionPoint = other.ClosestPoint(position);
        Vector3 collisionNormal = position - collisionPoint;
        
        
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, collisionNormal);
        Vector3 pos = collisionPoint + collisionNormal * hitOffset;

        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);

            hitInstance.transform.LookAt(collisionPoint + collisionNormal); 

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }

        //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }
        //Destroy projectile on collision
        Destroy(gameObject);
    }

    
}
