using UnityEngine;

public class FrameMoveCameraScript : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform menObject;

    [Tooltip("menObject の位置に追加されるオフセット（ワールド座標系）")]
    [SerializeField] private Vector3 positionOffset = Vector3.zero;

    [Tooltip("menObject の角度をそのまま使うか？ false の場合は fixedRotation を使う")]
    [SerializeField] private bool useMenObjectRotation = true;

    [Tooltip("固定角度（useMenObjectRotation = false のとき適用）")]
    [SerializeField] private Vector3 fixedRotationEuler = Vector3.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))/*デバックコマンド*/
        {
            WarpCameraToMenObject();
        }
    }

    /// <summary>
    /// カメラを menObject の位置と角度にワープさせる
    /// menobjectにはTVの面を設定
    /// cameraにはmainCameraを設定
    /// </summary>
    public void WarpCameraToMenObject()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;/*なんもついてないならmainCameraにつく*/
        }

        if (mainCamera != null && menObject != null)/*ちゃんとついてるか*/
        {
            mainCamera.transform.position = menObject.position + positionOffset;/*指定位置に物体の前に行く*/

            if (useMenObjectRotation)
            {
                mainCamera.transform.rotation = menObject.rotation;/*指定角度で物体の前に行く*/
            }
            else
            {
                mainCamera.transform.rotation = Quaternion.Euler(fixedRotationEuler);
            }
        }
        else
        {
            Debug.LogWarning("mainCamera または menObject が設定されていません。");
        }
    }
}
