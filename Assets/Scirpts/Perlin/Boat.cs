using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public GameObject playerObject;

    public PlayerStats PlayerStats => playerObject?.GetComponent<PlayerStats>();
    public Inventory PlayerInventory => playerObject?.GetComponent<Inventory>();

    public Transform seatPoint;
    public Camera boatCamera;
    public float moveSpeed = 5f;
    public float turnSpeed = 90f;

    private GameObject player;

    private Camera playerCamera;
    private bool isDriving = false;

    private bool isPlayerNear = false;

    void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject == null)
            {
                Debug.LogWarning("[Boat] Player 오브젝트를 찾지 못했습니다. 태그 확인 필요.");
            }
        }
        boatCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDriving)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            transform.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up * h * turnSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space)) // Space로 하차
            {
                ExitBoat();
            }
        }
        else if (isPlayerNear && Input.GetMouseButtonDown(1)) // 우클릭으로 탑승
        {
            EnterBoat();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerCamera = player.GetComponentInChildren<Camera>();
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    void EnterBoat()
    {
        if (player == null || playerCamera == null) return;

        isDriving = true;

        playerCamera.enabled = false;
        boatCamera.gameObject.SetActive(true);

        player.SetActive(false);
    }

    void ExitBoat()
    {
        isDriving = false;

        boatCamera.gameObject.SetActive(false);
        playerCamera.enabled = true;

        // 플레이어가 보트 위로 배치
        Vector3 safePosition = seatPoint.position + Vector3.up * 1f; // 보트 위 살짝 띄우기
        player.transform.position = safePosition;
        player.SetActive(true);
    }
}
