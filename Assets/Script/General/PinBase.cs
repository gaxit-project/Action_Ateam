using System.Threading;
using UnityEngine;
using static Player2;

public class PinBase : MonoBehaviour
{
    [SerializeField] private float knockDownAngleThreshold = 45f;
    [SerializeField] private GameObject outAreaObj;

    public Vector3 center = new Vector3(0f, -0.6f, 0f);
    private Rigidbody rb;

    private Quaternion initialRotation;
    private bool isFallDown = false;

    public string KnockedByPlayerID = null;
    public Color KnockedByPlayerColor;
    private GameManager gameManager;

    private void Start()
    {
        isFallDown = false;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.centerOfMass = center;
        }
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        SetPinBody();
    }

    private void Update()
    {
        float angle = Quaternion.Angle(initialRotation, transform.rotation);
        if (angle > knockDownAngleThreshold)
        {
            isFallDown = true;
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        Material[] materials = renderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name.Contains("Body"))
            {
                if (isFallDown && KnockedByPlayerColor != default)
                {
                    materials[i].color = KnockedByPlayerColor;
                }
                else
                {
                    materials[i].color = Color.white;
                }
            }
            else if (materials[i].name.Contains("Strip"))
            {
                if (isFallDown && KnockedByPlayerColor != default)
                {
                    materials[i].color = KnockedByPlayerColor;
                }
                else
                {
                    materials[i].color = Color.red;
                }
            }
            else if (materials[i].name.Contains("Bline"))
            {
                if (isFallDown && KnockedByPlayerColor != default)
                {
                    materials[i].color = KnockedByPlayerColor;
                }
                else
                {
                    materials[i].color = Color.black;
                }
            }
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pin" || other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            AudioManager.Instance.PlaySound(1);
        }
        if (other.gameObject.tag == "Pin")
        {
            
        }

        if(other.gameObject.tag == "Player" || other.gameObject.tag == "NPC")
        {
            PlayerBase playerID = other.gameObject.GetComponent<PlayerBase>();
            if (isFallDown == false){
                KnockedByPlayerID = playerID.GetPlayerID();
                KnockedByPlayerColor = playerID.GetPlayerColor();
            }
        }

        if(other.gameObject.tag == "Pin")
        {
            var otherPin = other.gameObject.GetComponent<PinBase>();
            if (!string.IsNullOrEmpty(otherPin.KnockedByPlayerID))
            {
                if (!isFallDown)
                {
                    KnockedByPlayerID = otherPin.KnockedByPlayerID;
                    KnockedByPlayerColor = otherPin.KnockedByPlayerColor;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider outAreaObj)
    {
        isFallDown = true;
    }

    public bool IsKnockedDownPin(string playerID)
    {
        if (isFallDown && KnockedByPlayerID == playerID)
        {
            return true;
        }

        float angle = Quaternion.Angle(initialRotation, transform.rotation);
        if(angle > knockDownAngleThreshold && KnockedByPlayerID == playerID)
        {
            isFallDown = true;
            return true;
        }
        return false;
    }

    public void SetPinBody()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        Material[] materials = renderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            
            if (materials[i].name.Contains("Body") || materials[i].name.Contains("White"))
            {
                materials[i].color = Color.white;
            }
            else if (materials[i].name.Contains("Strip"))
            {
                materials[i].color = Color.red;
            }
            else if (materials[i].name.Contains("Bline"))
            {
                materials[i].color = Color.black;
            }
        }
    }
    public bool GiveIsFallDown()
    {
        return isFallDown;
    }
}
