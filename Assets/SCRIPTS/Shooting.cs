using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Shooting : MonoBehaviour
{

    public GameObject bulletPrefab;

    bool inFiringRange = true;
    bool isMeleeMode = false;
    bool lookingLeft = false;
    bool lookingRight = true;

    public float centerAngle, minAngle, maxAngle;
    float currAngle;
    public float currAngleForward;
    public float currAngleBackward;
    public float lastValidAngle;

	private Mecha mecha;

    // Use this for initialization
    void Start()
    {
        centerAngle = 270.0f; // 270.0
        minAngle = centerAngle - 45.0f; //225.0
        maxAngle = centerAngle + 45.0f; //315.0

		mecha = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mecha> ();
    }

    void LookAtCode()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = target - transform.position;
        transform.LookAt(transform.forward + transform.position, direction);
    }
    // Update is called once per frame'

    void Update()
    {
		if (mecha.isMeleeMode == false) 
		{
			if(inFiringRange == true)
			{
				GetComponent<SpriteRenderer>().color = Color.yellow; 
			}

			else if (inFiringRange == false)
			{
				GetComponent<SpriteRenderer>().color = Color.black;
			}

			currAngleForward = transform.eulerAngles.z;
			currAngleBackward = transform.eulerAngles.z;

			float originalX = transform.eulerAngles.x;
			float originalY = transform.eulerAngles.y;

			if (lookingRight == true)
			{
				centerAngle = 270.0f; // 270.0
				minAngle = centerAngle - 45.0f; //225.0
				maxAngle = centerAngle + 45.0f; //315.0

				if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= transform.position.y)
				{
					if (currAngleForward > maxAngle || currAngleForward < 90.0f)
					{
						inFiringRange = false;
						//currAngleForward = maxAngle - 1f;
						transform.eulerAngles = new Vector3(originalX, originalY, currAngleForward);
						lastValidAngle = maxAngle;
					}
					else
					{
						inFiringRange = true;
					}
				}
				else if(Camera.main.ScreenToWorldPoint(Input.mousePosition).y < transform.position.y)
				{ 
					if (currAngleBackward < minAngle)
					{
						inFiringRange = false;
						//currAngleBackward = minAngle + 1f;
						transform.eulerAngles = new Vector3(originalX, originalY, currAngleBackward);
						lastValidAngle = minAngle;
					}
					else
					{
						inFiringRange = true;
					}
				}
				LookAtCode();

				if (Input.GetKeyDown(KeyCode.A))
				{
					lookingLeft  = true;
					lookingRight = false;
				}
			}

			else if (lookingLeft == true)
			{
				centerAngle = 90.0f; // 90.0
				minAngle = centerAngle - 45.0f; //45.0
				maxAngle = centerAngle + 45.0f; //135.0

				if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < transform.position.y)
				{
					if (currAngleForward > maxAngle)
					{
						inFiringRange = false;
						//currAngleForward = maxAngle - 1f;
						transform.eulerAngles = new Vector3(originalX, originalY, currAngleForward);
						lastValidAngle = maxAngle;
					}
					else
					{
						inFiringRange = true;
					}
				}
				else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y >= transform.position.y)
				{
					if (currAngleBackward < minAngle || currAngleBackward > 270.0f)
					{
						inFiringRange = false;
						//currAngleBackward = minAngle + 1f;
						transform.eulerAngles = new Vector3(originalX, originalY, currAngleBackward);
						lastValidAngle = minAngle;
					}
					else
					{
						inFiringRange = true;
					}
				}
				LookAtCode();

				if (Input.GetKeyDown(KeyCode.D))
				{
					lookingRight = true;
					lookingLeft = false;
				}
			}


			if (Input.GetMouseButtonDown(0))
			{

				//mecha.isLMB++;

				Vector3 mouseDirection;

				if (inFiringRange == true)
				{
					mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
					mouseDirection.z = 0.0f;
					mouseDirection.Normalize();
				}
				else
				{
					mouseDirection = Quaternion.Euler(0f, 0f, lastValidAngle) * Vector3.up;
				}

				if (isMeleeMode == false)
				{
					//if (inFiringRange == true)
					{
						Mecha owner = GameObject.FindGameObjectWithTag ("Player").GetComponent<Mecha> ();
						if (owner.UseAmmo (10)) {
							GameObject newBullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity);
							newBullet.GetComponent<Bullet> ().direction = mouseDirection;
						}
					}

				}
			}

		}
	}
        
}
