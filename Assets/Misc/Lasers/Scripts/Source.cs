using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour {

    public float maxDistance = 20f;
    float beamlength;
    LineRenderer lr;
	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();

        //StartCoroutine("UpdateBeam");
	}
	
	// Update is called once per frame
	void Update () {
        
        //Vector3 hitpoint = Vector3.zero;
        //if (hit.collider == null) hitpoint = transform.up * maxDistance;
        //else hitpoint = hit.point;
        ////Debug.Log("hitcount:" + hit.transform.name);
        //Debug.DrawLine(transform.position, hit.point);
        //Vector3 transformedPoint = hitpoint;
        //if( hit.collider != null) transformedPoint = transform.InverseTransformPoint(hitpoint);
        //lr.SetPosition(1, transformedPoint);

        // if we got a hit raycast from there and keep going with remaining distance. 
	}

    IEnumerator UpdateBeam()
    {
        yield return null;
        RaycastHit2D[] hit;
        int points = 1;
        Vector3 lastHitPoint = transform.position;
        Vector3 dirToFire = transform.up;
        beamlength = 0;
        while (beamlength < maxDistance)
        {
            hit = Physics2D.RaycastAll(lastHitPoint, dirToFire, maxDistance - beamlength);
            for (int i = 0; i < hit.Length; i++)
            {
                if (new Vector3(hit[i].point.x, hit[i].point.y, lastHitPoint.z) == lastHitPoint && hit.Length > 1)
                    continue;
                else if(new Vector3(hit[i].point.x, hit[i].point.y, lastHitPoint.z) == lastHitPoint && hit.Length == 1)
                {
                    beamlength = maxDistance;
                    break;
                }

                if (hit[i].collider == null || hit.Length == 1)
                {
                    lr.SetPosition(points, dirToFire * (maxDistance - beamlength));
                    beamlength = maxDistance;
                }
                else
                {
                    beamlength += hit[i].distance;
                    lr.SetPosition(points, transform.InverseTransformPoint(hit[i].point));
                    points++;
                    lr.positionCount = points + 1;
                    lastHitPoint = hit[i].point;

                    // get reflection
                    dirToFire = Vector3.Reflect(dirToFire, hit[i].normal);

                }
                break; // only care about the first thing we hit.
            }
        }
        StartCoroutine("UpdateBeam");
    }
}
//[RequireComponent(typeof(LineRenderer))]
//public class BouncingLaser : MonoBehaviour
//{

//    public int laserDistance;
//    public LineRenderer mLineRenderer;
//    public string bounceTag;
//    public int maxBounce;
//    private float timer = 0;

//    // Use this for initialization
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown("space") && !mLineRenderer.enabled)
//        {
//            timer = 0;
//            StartCoroutine("FireMahLazer");
//        }
//    }

//    IEnumerator FireMahLazer()
//    {
//        //Debug.Log("Running");
//        mLineRenderer.enabled = true;
//        int laserReflected = 1; //How many times it got reflected
//        int vertexCounter = 1; //How many line segments are there
//        bool loopActive = true; //Is the reflecting loop active?

//        Vector3 laserDirection = transform.forward; //direction of the next laser
//        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

//        mLineRenderer.SetVertexCount(1);
//        mLineRenderer.SetPosition(0, transform.position);
//        RaycastHit hit;

//        while (loopActive)
//        {

//            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && hit.transform.gameObject.tag == bounceTag)
//            {

//                Debug.Log("Bounce");
//                laserReflected++;
//                vertexCounter += 3;
//                mLineRenderer.SetVertexCount(vertexCounter);
//                mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
//                mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
//                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
//                mLineRenderer.SetWidth(.1f, .1f);
//                lastLaserPosition = hit.point;
//                laserDirection = Vector3.Reflect(laserDirection, hit.normal);
//            }
//            else
//            {

//                Debug.Log("No Bounce");
//                laserReflected++;
//                vertexCounter++;
//                mLineRenderer.SetVertexCount(vertexCounter);
//                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * laserDistance);
//                Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
//                mLineRenderer.SetPosition(vertexCounter - 1, lastLaserPosition + (laserDirection.normalized * laserDistance));

//                loopActive = false;
//            }
//            if (laserReflected > maxBounce)
//                loopActive = false;
//        }

//        if (Input.GetKey("space") && timer < 2)
//        {
//            yield return new WaitForEndOfFrame();
//            timer += Time.deltaTime;
//            StartCoroutine("FireMahLazer");
//        }
//        else
//        {
//            yield return null;
//            mLineRenderer.enabled = false;
//        }
//    }
//}


