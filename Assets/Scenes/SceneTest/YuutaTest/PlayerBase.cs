using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : SingletonMonoBehaviour<PlayerBase>
{
    //�����ݒ�
    private new Rigidbody rigidbody;
    Player player = new Player();

    //�X�e�[�^�X
    [SerializeField] protected float speed = 300f;
    [SerializeField] protected float weight = 10f;
    protected float rotation = 0f;

    //�ړ��֘A
    [SerializeField] private Vector3 gravity = new Vector3(0f, -75f, 0f);

    public float rotateSpeed = 100f;
        float yRotation = 0f;

    void Start()
    {
        //Rigidbody���擾
        rigidbody = GetComponent<Rigidbody>();
        //�W���̏d�͂𖳌�������
        rigidbody.useGravity = false;
        //�N���X���̃X�e�[�^�X�̏�����
        player.InitializeStatus(speed, weight);

        
    }

    
    void Update()
    {
        //�X�e�[�^�X�̍X�V
        speed = player.Speed;
        weight = player.Weight;
        rotation = player.Rotation;

        //�ړ��֘A�̏���
        transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (rigidbody)
        {
            //�X�e�B�b�N�AWASD�A���L�[�ňړ�
            rigidbody.linearVelocity = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized * speed * Time.deltaTime;
            //�d�͂̕ύX
            rigidbody.AddForce(gravity, ForceMode.Acceleration);
        }
        else
        {
            Debug.LogError("RigidBody���A�^�b�`����Ă��܂���I");
        }

        
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime; //左右回転
        yRotation += mouseX;
            transform.Rotate(0f,yRotation,0f);
            
    }
        
        

    public class Character
    {
        //�X�e�[�^�X
        public float Speed;
        public float Weight;
        public float Rotation;

        /// <summary>
        /// �X�e�[�^�X�̏�����
        /// </summary>
        public void InitializeStatus(float speed, float weight)
        {
            Speed = speed;
            Weight = weight;
        }

    }

    public class Player : Character
    {
        
    }
}
