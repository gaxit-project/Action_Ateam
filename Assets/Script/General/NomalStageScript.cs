using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private GameObject[] PlayerObjects = new GameObject[4];

    [Header("Pin")]
    [SerializeField] private GameObject[] PinObjects = new GameObject[10];

    [Header("Cube")]
    [SerializeField] private GameObject[] CubeAreas = new GameObject[4];

    [Header("Cube2")]
    [SerializeField] private GameObject[] Cube2Areas = new GameObject[10];


    void Update()
    {
        // F�L�[�������ꂽ�Ƃ��Ɉړ������s
        if (Input.GetKeyDown(KeyCode.F))
        {
            MoveAllObjects();
        }
    }

    void MoveAllObjects()
    {

        for (int i = 0; i < PlayerObjects.Length; i++)
        {
            MoveObjectToCube(i);
        }


        for (int j = 0; j < PinObjects.Length; j++)
        {
            MoveObjectToCube2(j);
        }
    }

    void MoveObjectToCube(int i)
    {
        if (PlayerObjects[i] != null && CubeAreas[i] != null)
        {
            Vector3 CubePosition = CubeAreas[i].transform.position;


            PlayerObjects[i].transform.position = CubePosition;

            PlayerObjects[i].transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            Rigidbody rb = PlayerObjects[i].GetComponent<Rigidbody>();//���x��0��
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.linearVelocity = new Vector3(20f, rb.linearVelocity.y, rb.linearVelocity.z);
            }


        }

    }

    void MoveObjectToCube2(int j)
    {
        if (PinObjects[j] != null && Cube2Areas[j] != null)
        {
            Vector3 CubePosition = Cube2Areas[j].transform.position;


            PinObjects[j].transform.position = CubePosition;

            PinObjects[j].transform.rotation = Quaternion.Euler(0f, 0f, 0f);


            Rigidbody rb = PinObjects[j].GetComponent<Rigidbody>();//���x��0��
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

    }

}
