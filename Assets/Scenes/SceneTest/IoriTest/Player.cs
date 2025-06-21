using NPC.StateAI;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Player : PlayerBase
{
    public enum PlayerState
    {
        Idle,
        Run,
        Throwed,
        Dead
    }

    protected PlayerState currentState = PlayerState.Idle;

    //プロパティ
    public PlayerState PlayerStateProperty
    {
        get => currentState;
        set => currentState = value;
    }
    public float GetRstickX => RstickX;

    void FixedUpdate()
    {
        //プレイヤーの進む方向
        Vector3 targetVelocity;

        //ステータスを取得
        speed = player.Speed;
        weight = player.Weight;
        rotation = player.Rotation;

        //右スティックで回転
        if (!isAttacking && !isAttacked) RstickX = Input.GetAxis("Horizontal2") * rotateSpeed * 2f * Time.fixedDeltaTime;
        else RstickX = 0f;
        transform.Rotate(0f, RstickX, 0f);

        //固有の重力
        rigidbody.AddForce(new Vector3(0f, -gravity, 0f), ForceMode.Acceleration);

        //反射準備
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.black, 1f, false);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.tag == "Wall" || hit.collider.tag == "Player" || hit.collider.tag == "NPC")
            {
                if(targetTag == null) Debug.Log("標的に接近");
                else if(targetTag != hit.collider.tag) Debug.Log("標的を" + hit.collider.name + "に変更");
                targetTag = hit.collider.tag;
                // 現在の進行方向（速度）
                incomingVelocity = rigidbody.linearVelocity;
                //Debug.DrawRay(transform.position, Vector3.ClampMagnitude(incomingVelocity, 3f), Color.yellow, 3f, false);
            }
        }
        else if (!Physics.Raycast(ray, 3f))
        {
            targetTag = null;
        }
            //現在のステートによって行動を変える
            switch (currentState)
            {
                case PlayerState.Idle:

                    break;

                case PlayerState.Run:

                    //移動関係
                    if (rigidbody)
                    {

                        //地面についている場合のみスティックまたはWASD,矢印キーの入力を受付
                        if (player.IsGrounded(transform.position, rayDistance))
                        {
                            //カメラ基準の方向
                            x = camera.transform.right * Input.GetAxis("Horizontal");
                            z = camera.transform.forward * Input.GetAxis("Vertical");

                            x.y = 0f;
                            z.y = 0f;
                            x.Normalize();
                            z.Normalize();
                        }

                        //方向決定
                        targetVelocity = Vector3.ClampMagnitude(x + z, 1f) * speed;
                        //加減速処理
                        float lerpRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
                        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);
                        if (isDecelerating)
                        {
                            reflection = Vector3.Lerp(reflection, Vector3.zero, lerpRate * Time.fixedDeltaTime);

                            // 減速が十分に小さくなったら解除
                            if (reflection.magnitude < 0.05f)
                            {
                                reflection = Vector3.zero;
                                isDecelerating = false;
                            }
                        }
                        //移動
                        if (reflection.magnitude > 0.5f) rigidbody.MovePosition(rigidbody.position + reflection * Time.fixedDeltaTime);
                        else if (!isModeChanged && !isAttacking && !isAttacked) rigidbody.MovePosition(rigidbody.position + currentVelocity * Time.fixedDeltaTime);
                        else if (!isAttacked) rigidbody.linearVelocity = Vector3.zero;

                    }

                    //Enterキーで状態を変化
                    /*
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        ChangeMode();
                    }
                    */
                    /*
                    //ゲージの増減
                    if (isModeChanged)
                    {
                        if (isGaugeIncreasing)
                        {
                            currentGaugeValue += currentGaugeSpeed * Time.fixedDeltaTime;
                        }
                        else if (!isGaugeIncreasing)
                        {
                            currentGaugeValue -= currentGaugeSpeed * Time.fixedDeltaTime;
                        }

                        if (currentGaugeValue < 0f)
                        {
                            currentGaugeValue = 0f;
                            isGaugeIncreasing = true;
                        }
                        else if (currentGaugeValue > maxGaugeValue)
                        {
                            currentGaugeValue = maxGaugeValue;
                            isGaugeIncreasing = false;
                        }

                        gauge.value = currentGaugeValue;
                        throwPower = (maxThrowPower * (currentGaugeValue / maxGaugeValue) / 2f) + maxThrowPower / 2f;
                    }
                    //スペースキーで投擲
                    if (isModeChanged && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)))
                    {
                        Throw();
                    }
                    */

                    //スペースキーまたはAボタンで攻撃
                    if (!isAttacking && player.IsGrounded(transform.position, rayDistance) && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)))
                    {
                        //Attack();
                    }

                    break;

                case PlayerState.Throwed:

                    //Enterキーで状態を変化
                    /*
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        ChangeMode();
                    }
                    */

                    if (player.IsGrounded(transform.position, rayDistance))
                    {
                        //プレイヤー基準の方向
                        x = this.transform.right * Input.GetAxis("Horizontal");

                        x.y = 0f;
                        x.Normalize();

                        targetVelocity = Vector3.ClampMagnitude(x, 1f) * speed;
                        //加減速処理
                        float lerpRate = (targetVelocity.magnitude > 0.1f) ? acceleration : deceleration;
                        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpRate * Time.fixedDeltaTime);

                        rigidbody.linearVelocity = throwVelocity + currentVelocity / 3f;
                        //Debug.DrawRay(transform.position, Vector3.ClampMagnitude(rigidbody.linearVelocity, 3f), Color.black, 3f, false);
                    }


                    break;

                case PlayerState.Dead:

                    camera.StopCameraMove();
                    this.gameObject.SetActive(false);
                    scoreManager.FrameSaveSystem();
                    foreach (var player in GameManager.Instance.players)
                    {
                        if (player != null)
                            Destroy(player.gameObject);
                    }
                    GameManager.Instance.players.Clear();
                    SceneChangeManager.Instance.ResetScene("Main");
                    break;

            }
    }

    //投擲後に壁やNPCに衝突した際の処理
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && collision.contactCount > 0 && targetTag == "Wall")
        {
            // 衝突面の法線
            Vector3 normal = collision.contacts[0].normal;
            //Debug.DrawRay(transform.position, Vector3.ClampMagnitude(normal, 3f), Color.magenta, 3f, false);

            // 反射ベクトルの計算
            Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal).normalized;
            //Debug.DrawRay(transform.position, Vector3.ClampMagnitude(reflectVelocity, 3f), Color.cyan, 3f, false);

            // 法線方向に少し押し返しを加える
            //reflectVelocity += normal * 0.2f;

            reflectVelocity.Normalize();
            if (currentState == PlayerState.Throwed)
            {
                transform.forward = reflectVelocity;
                throwVelocity = reflectVelocity * speed * throwPower;
                rigidbody.linearVelocity = throwVelocity;
            }
            else if (currentState == PlayerState.Run)
            {
                Reflect(reflectVelocity);
            }
        }
        else if ((collision.gameObject.CompareTag("NPC") || collision.gameObject.CompareTag("Player") && collision.contactCount > 0))
        {
            if (incomingVelocity != Vector3.zero)
            {
                transform.forward = incomingVelocity;
                throwVelocity = new Vector3(incomingVelocity.x, incomingVelocity.y, -incomingVelocity.z);
            }
            else
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                transform.forward = rb.linearVelocity;
                throwVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, -rb.linearVelocity.z);
            }
                rigidbody.linearVelocity = throwVelocity;
        }
    }

    /// <summary>
    /// 投擲
    /// </summary>
    public void Throw()
    {
        //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, camera.GetCameraRotationY(), transform.eulerAngles.z);
        Vector3 forward = camera.transform.forward;
        forward.y = 0f;
        transform.rotation = Quaternion.LookRotation(forward);
        throwVelocity = transform.forward * speed * throwPower;
        rigidbody.linearVelocity = throwVelocity;
        //camera.StopCameraMove();
        /*
        if (!isThrowTimerStarted)
        {
            isThrowTimerStarted = true;
            throwTimer = 0f;
        }
        */
        //Debug.Log("投擲");
        camera.ChangeCameraMode();
        currentState = PlayerState.Throwed;
    }

    protected async void Attack()
    {
        isAttacking = true;
        AttackZone = Instantiate(attackArea, this.transform);
        AttackZone.transform.localPosition = new Vector3(0.6f, 0f, 0f);
        await Task.Delay(500);
        isAttacking = false;
        Destroy(AttackZone);
    }

    public async void Attacked(Vector3 attackedVelocity)
    {
        if (isAttacking)
        {
            isAttacking = false;
            Destroy(AttackZone);
        }
        isAttacked = true;
        rigidbody.linearVelocity = attackedVelocity;
        await Task.Delay(1000);
        isAttacked = false;
    }


    //Run状態の時の衝突処理
    public async void Reflect(Vector3 reflectVelocity)
    {
        isReflecting = true;
        reflection = reflectVelocity * speed;
        await Task.Delay(300);
        isReflecting = false;
        isDecelerating = true;
    }




}
