using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    //�@�I�t���b�V�������N���g�p�����ǂ���
    bool isUseOffmeshLink;
    //�@�I�t���b�V�������N�̃X�^�[�g�ʒu
    Vector3 startPos;
    //�@�I�t���b�V�������N�̃G���h�ʒu
    Vector3 endPos;
    //�@�I�t���b�V�������N���ړ�����X�s�[�h
    float offmeshLinkSpeed;
    //�@�I�t���b�V�������N��endPos�����ɉ�]����X�s�[�h
    float offmeshLinkRotateSpeed;
    //�@�I�t���b�V�������N�̕���
    private Vector3 endDirection;

    [SerializeField] Animator animator;
    [SerializeField] GameObject bulletPrefab;
    private float bulletCoolTimeBase = 1.5f;
    private float bulletCoolTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        offmeshLinkSpeed = agent.speed;
        offmeshLinkRotateSpeed = agent.angularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Jump"))
        {
            animator.SetBool("Jump", false);
        }
        if (target != null)
        {
            agent.destination = target.position;
        }
        //�@�I�t���b�V�������N������ꏊ�Ȃ�
        if (agent.isOnOffMeshLink)
        {
            //�@�����ݒ�
            if (!isUseOffmeshLink)
            {
                isUseOffmeshLink = true;
                startPos = agent.transform.position;
                endPos = agent.currentOffMeshLinkData.endPos;
                endDirection = (endPos - startPos).normalized;

                Debug.Log("�i�r���b�V�������N�J�n");
                animator.SetBool("Jump", true);
            }
            //�@�L�����N�^�[�̌�����ݒ�
            var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(endDirection), offmeshLinkRotateSpeed * Time.deltaTime);
            agent.transform.rotation = Quaternion.Euler(agent.transform.eulerAngles.x, rot.eulerAngles.y, agent.transform.eulerAngles.z);
            //�@�L�����N�^�[�̈ʒu��ݒ�
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, offmeshLinkSpeed * Time.deltaTime);
            //�@�I�t���b�V�������N���I�������邩�ǂ����i������0.1���Z����ΏI���j
            if (Vector3.Distance(agent.transform.position, endPos) < 0.1f)
            {
                isUseOffmeshLink = false;
                agent.CompleteOffMeshLink();

                Debug.Log("�i�r���b�V�������N�I��");
            }
        }
        //while (agent.pathPending)
        //{
        //    yield return null;
        //}
        NavMeshPath path = agent.path; //�o�H�p�X�i�Ȃ���p���W��Vector3�z��j���擾
        float dist = 0f;
        Vector3 corner = transform.position; //�����̌��݈ʒu
                                             //�Ȃ���p�Ԃ̋�����ݐς��Ă���
        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 corner2 = path.corners[i];
            dist += Vector3.Distance(corner, corner2);
            corner = corner2;
        }
        if (path.corners.Length <= 100 && dist < 5)
        {
            agent.speed = 0;
            bulletCoolTime += Time.deltaTime;
            if (bulletCoolTime > bulletCoolTimeBase)
            {
                Transform bullet = Instantiate(bulletPrefab).transform;
                bullet.position = transform.position;
                bullet.rotation = transform.rotation;
                bullet.GetComponent<projectile>().Throw();
                bulletCoolTime = 0;
            }
        }
        else
        {
            agent.speed = offmeshLinkSpeed;
        }

        animator.SetFloat("Speed", agent.speed);
    }

    private void OnDrawGizmos()
    {
        if (agent && agent.enabled)
        {
            Gizmos.color = Color.red;
            Vector3 prepos = transform.position;

            foreach (Vector3 pos in agent.path.corners)
            {
                Gizmos.DrawLine(prepos, pos);
                prepos = pos;
            }
        }
    }
}
