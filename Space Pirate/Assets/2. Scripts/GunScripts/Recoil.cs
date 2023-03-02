using UnityEngine;
public class Recoil : MonoBehaviour
{
    //rotations
    public Vector3 currentRotation, targetRotation;
    Player playerScript;
    public Gun gunScript;
    //recoil values

    private void Awake()
    {
        playerScript = FindObjectOfType<Player>();
        gunScript = FindObjectOfType<Gun>();
    }
    private void Update()
    {
        if (playerScript.gravityEnabled)
        {
            gunScript.currentGun.recoilX = -Mathf.Abs(gunScript.currentGun.recoilX);
        }
        else if (!playerScript.gravityEnabled)
        {
            gunScript.currentGun.recoilX = Mathf.Abs(gunScript.currentGun.recoilX);
        }
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gunScript.currentGun.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gunScript.currentGun.snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void FireRecoil()
    {
        targetRotation += new Vector3(gunScript.currentGun.recoilX,
            Random.Range(gunScript.currentGun.recoilY, -gunScript.currentGun.recoilY), Random.Range(gunScript.currentGun.recoilZ, -gunScript.currentGun.recoilZ));
    }
}
