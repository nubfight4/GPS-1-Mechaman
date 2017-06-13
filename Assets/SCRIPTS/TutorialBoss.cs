using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class tutorialBoss : LifeObject
{
    public float initialSpeed = 0f;
    private Mecha target;
    private float speed;
    private float timer;
    private Direction movingDirection;
    private bool faceLeft;
    private bool attacked;
    private bool isEnraged;
    private int enrageHpThreshold;
    private bool nearToTarget;

    enum Direction
    {
        LEFT,
        RIGHT,
        NONE
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Mecha>();
        SetSpeed(GetInitialSpeed());
        timer = 0f;
        attacked = false;
        SetMaxHP(1000);
        isEnraged = false;
        enrageHpThreshold = 300;
        SetHP(GetMaxHP());
        //ReceiveDamage (700);
    }

    void Update()
    {
        if (isAlive)
        {
            CheckDeath();

            if (target != null)
            {
                UpdateMonsterCondition(); // Change monster to enrage when HP falls below enrage hp threshold

                if (!isEnraged)
                {
                    UpdateAction();
                }
                else
                {
                    UpdateActionWhileEnraged();
                }

                if (movingDirection == Direction.LEFT)
                {
                    MoveLeft();
                }
                else if (movingDirection == Direction.RIGHT)
                {
                    MoveRight();
                }
                timer += Time.deltaTime;
            }
        }

    }

    void ChangeMovingDirection()
    {

    }

    void Reset()
    {
        SetSpeed(GetInitialSpeed());
        timer = 0;
        attacked = false;
    }

    void UpdateAction()
    {

    }

    void UpdateActionWhileEnraged()
    {

    }

    void Flip()
    {
        Vector3 v3 = transform.localScale;
        v3.x *= -1;
        transform.localScale = v3;
    }

    void MoveLeft()
    {
        faceLeft = true;
        transform.Translate(Vector3.left * Time.deltaTime * speed);
        if (transform.localScale.x < 0)
        {
            Flip();
        }
    }

    void MoveRight()
    {
        faceLeft = false;
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        if (transform.localScale.x > 0)
        {
            Flip();
        }
    }

    Direction SeekTarget()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        if (targetDir.x < 0)
            return Direction.LEFT;
        else if (targetDir.x > 0)
            return Direction.RIGHT;
        else
            return Direction.NONE;
    }

    float GetDistanceFromTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    void FaceTarget()
    {
        float initSpeed = GetSpeed();
        SetSpeed(0);
        if (movingDirection != SeekTarget())
        {
            if (SeekTarget() == Direction.LEFT)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
        SetSpeed(initSpeed);
    }

    IEnumerator ApplyDamageWithDelay(int damage, Vector3 knockbackDir, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.ReceiveDamage(damage);
        target.Knockback(knockbackDir, 1);
    }

    void UpdateMonsterCondition()
    {
        if (!isEnraged && GetHP() <= enrageHpThreshold)
        {
            isEnraged = true;
            Reset();
            Debug.Log("Monster is enraged!");
        }
    }

    private float GetInitialSpeed()
    {
        return this.initialSpeed;
    }

    private void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private float GetSpeed()
    {
        return this.speed;
    }
}