using UnityEngine;

public class Munition : MonoBehaviour {
    private Vector3 basePosition = new Vector3(15f, 0f, 0f);
    private Rigidbody2D rb2d;
    public WeaponType weaponType;

    public void activate(Vector3 position) {
        transform.position = position;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public void deactivate() {
        transform.position = basePosition;
        rb2d.bodyType = RigidbodyType2D.Static;
    }

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
    }
}