using UnityEngine;
using static UnityEngine.GraphicsBuffer;




public class FrontCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //前提としてカメラにアタッチする
    public GameObject Player0;
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;//プレイヤーども

    public GameObject FastGoal;//bowlingエリアまでののゴール

    public Vector3 offset = new Vector3(0f, 5f, -10f);  // カメラの相対位置
    public float followSpeed = 5f;  // カメラの追従スピード
    public Vector3 rotationEuler = new Vector3(20f, 0f, 0f); // インスペクターで設定するカメラの角度


    void Start()
    {
        if (Player0 == null || Player1 == null || Player2 == null || Player3 == null)
        {
            Debug.Log("中に誰も居ませんよ");
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        int flag =0;
        double px, pz, gx, gz, gk;
        double[] pk = new double[4];

        {//for使えば良かった↓
            px = Player0.transform.position.x;
            pz = Player0.transform.position.z;
            gx = FastGoal.transform.position.x;
            gz = FastGoal.transform.position.z;
            gk = gx * gx + gz * gz;
            pk[0] = (px * px + pz * pz) - gk;

        }
        {
            px = Player1.transform.position.x;
            pz = Player1.transform.position.z;
            pk[1] = (px * px + pz * pz) - gk;

        }
        {
            px = Player2.transform.position.x;
            gz = Player2.transform.position.z;
            pk[2] = (px * px + pz * pz) - gk;

        }
        {
            px = Player3.transform.position.x;
            pz = Player3.transform.position.z;
            pk[3] = (px * px + pz * pz) - gk;

        }//for使えばよかった↑

        for (int i = 0; i < 4; i++)
        {
            
                if (pk[i] <= pk[0] && pk[i] <= pk[1] && pk[i] <= pk[2] && pk[i] <= pk[3])//総当たりして一番近い人を探す
                {
                flag = i;
                }
        }
        if (flag == 0)
        {
            Vector3 targetPosition = Player0.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotationEuler); // Inspectorで指定された角度にする
        } 
        else if (flag == 1)
        {
            Vector3 targetPosition = Player1.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotationEuler); // Inspectorで指定された角度にする
        }
        else if (flag == 2)
        {
            Vector3 targetPosition = Player2.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotationEuler); // Inspectorで指定された角度にする
        }
        else if (flag == 3)
        {
            Vector3 targetPosition = Player3.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotationEuler); // Inspectorで指定された角度にする
        }
    }
}
