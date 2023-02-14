using UnityEngine;
public class Recoil : MonoBehaviour
{
    [SerializeField] GunData gun;
    //rotations
    public Vector3 currentRotation, targetRotation;
    Player playerScript;
    //recoil values

    private void Awake()
    {
        playerScript = FindObjectOfType<Player>();
    }
    private void Update()
    {
        if (playerScript.gravityEnabled)
        {
            gun.recoilX = -Mathf.Abs(gun.recoilX);
        }
        else if (!playerScript.gravityEnabled)
        {
            gun.recoilX = Mathf.Abs(gun.recoilX);
        }
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gun.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gun.snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);

        
    }

    public void FireRecoil()
    {
        targetRotation += new Vector3(gun.recoilX, Random.Range(gun.recoilY, -gun.recoilY), Random.Range(gun.recoilZ, -gun.recoilZ));
    }
}
