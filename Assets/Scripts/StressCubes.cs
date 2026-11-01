using UnityEngine;

// Week 3 worst case: 200 pooled cubes created once at start and moved every
// frame. Only needed if the game itself has nothing heavy to profile yet.
public class StressCubes : MonoBehaviour
{
    public int count = 200;
    public float spacing = 1.5f;
    public float amplitude = 1f;
    public float speed = 2f;

    Transform[] _cubes;
    Vector3[] _home;

    void Start()
    {
        _cubes = new Transform[count];
        _home = new Vector3[count];
        int side = Mathf.CeilToInt(Mathf.Sqrt(count));
        for (int i = 0; i < count; i++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(go.GetComponent<Collider>());   // no physics cost, just rendering + transforms
            go.transform.SetParent(transform, false);
            Vector3 p = new Vector3((i % side - side / 2f) * spacing, 0f, (i / side) * spacing);
            go.transform.localPosition = p;
            _cubes[i] = go.transform;
            _home[i] = p;
        }
    }

    void Update()
    {
        float t = Time.time * speed;
        for (int i = 0; i < _cubes.Length; i++)
        {
            Vector3 p = _home[i];
            p.y += Mathf.Sin(t + i * 0.3f) * amplitude;
            _cubes[i].localPosition = p;
            _cubes[i].localRotation = Quaternion.Euler(0f, t * 40f + i, 0f);
        }
    }
}
