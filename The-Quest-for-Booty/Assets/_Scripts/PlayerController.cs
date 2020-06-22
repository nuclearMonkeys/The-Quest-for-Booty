using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Vector2 mouseInput;

    public float mouseSensitivity = 1f;
    public GameObject bulletImpact;
    public int currentAmmo;
    public Animator gunAnimator;

    private void Awake()
    {
        if (instance)
            Destroy(this);
        instance = this;
    }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 x = transform.up * -moveInput.x;

        Vector3 y = transform.right * moveInput.y;

        rb.velocity = (x + y) * moveSpeed;

        // player view control
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z - mouseInput.x);

        Camera.main.transform.localRotation = Quaternion.Euler(Camera.main.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));

        // shooting
        if(Input.GetMouseButtonDown(0))
        {
            if(currentAmmo <= 0)
                return;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                // Debug.Log("Owie!: " + hit.transform.name);
                Instantiate(bulletImpact, hit.point, transform.rotation);
            } else {
                Debug.Log("I guess they never miss huh!");
            }
            currentAmmo--;
            gunAnimator.SetTrigger("Shoot");
        }
    }
}
