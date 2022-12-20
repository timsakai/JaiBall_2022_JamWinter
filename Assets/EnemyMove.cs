using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    //　オフメッシュリンクを使用中かどうか
    bool isUseOffmeshLink;
    //　オフメッシュリンクのスタート位置
    Vector3 startPos;
    //　オフメッシュリンクのエンド位置
    Vector3 endPos;
    //　オフメッシュリンクを移動するスピード
    float offmeshLinkSpeed;
    //　オフメッシュリンクのendPos方向に回転するスピード
    float offmeshLinkRotateSpeed;
    //　オフメッシュリンクの方向
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
        //　オフメッシュリンクがある場所なら
        if (agent.isOnOffMeshLink)
        {
            //　初期設定
            if (!isUseOffmeshLink)
            {
                isUseOffmeshLink = true;
                startPos = agent.transform.position;
                endPos = agent.currentOffMeshLinkData.endPos;
                endDirection = (endPos - startPos).normalized;

                Debug.Log("ナビメッシュリンク開始");
                animator.SetBool("Jump", true);
            }
            //　キャラクターの向きを設定
            var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(endDirection), offmeshLinkRotateSpeed * Time.deltaTime);
            agent.transform.rotation = Quaternion.Euler(agent.transform.eulerAngles.x, rot.eulerAngles.y, agent.transform.eulerAngles.z);
            //　キャラクターの位置を設定
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, offmeshLinkSpeed * Time.deltaTime);
            //　オフメッシュリンクを終了させるかどうか（距離が0.1より短ければ終了）
            if (Vector3.Distance(agent.transform.position, endPos) < 0.1f)
            {
                isUseOffmeshLink = false;
                agent.CompleteOffMeshLink();

                Debug.Log("ナビメッシュリンク終了");
            }
        }
        //while (agent.pathPending)
        //{
        //    yield return null;
        //}
        NavMeshPath path = agent.path; //経路パス（曲がり角座標のVector3配列）を取得
        float dist = 0f;
        Vector3 corner = transform.position; //自分の現在位置
                                             //曲がり角間の距離を累積していく
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
