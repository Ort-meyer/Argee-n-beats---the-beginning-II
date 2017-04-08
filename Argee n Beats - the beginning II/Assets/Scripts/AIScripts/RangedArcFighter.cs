using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedArcFighter : AttackBase {
    public Vector3 verticalOffset;
    public float offset;
    public GameObject projectile;
    public float airspeed;
    public float impulse;
    public float pushRadius;
    public float damageRadius;
    // Use this for initialization
    void Start () {
        base.BaseStart();
	}
	
	// Update is called once per frame
	void Update () {
        base.BaseUpdate();
        if (base.CanIAttackTarget())
        {
            attackTimer = 1 / attackSpeed;
            MakeAttack();
        }
	}

    private void MakeAttack()
    {
        Vector3 velocity;
        Vector3 acceleration = Physics.gravity;
        Vector3 position = transform.position;
        Vector3 positionObj = objectToAttack.transform.position + new Vector3(0, 1, 0); // TODO get form collsion box halfway
        Vector3 velocityObj = Vector3.zero;
        Rigidbody targetRigid = objectToAttack.GetComponent<Rigidbody>();
        if (targetRigid != null)
        {
            velocityObj = targetRigid.velocity;
        }

        //Projectile.ProjectileInfo projInfo = GetComponent<UnitInformation>().projectiles[0];
        //float airTime = projInfo.projectileAirTime;
        float airTime = (objectToAttack.transform.position - transform.position).magnitude / airspeed;

        velocity = (positionObj + velocityObj * airTime - position - verticalOffset - (acceleration * airTime * airTime / 2.0f)) / airTime;


        GameObject proj = Instantiate(projectile, transform.position + verticalOffset + velocity.normalized * offset, Quaternion.identity);
        //proj.transform.LookAt(proj.transform.position + velocity);
        Rigidbody rigProj = proj.GetComponent<Rigidbody>();
        rigProj.AddForce(velocity, ForceMode.VelocityChange);
        Projectile projectileScript = proj.GetComponent<Projectile>();
        projectileScript.impulse = impulse;
        projectileScript.damage = damage;
        projectileScript.radius = pushRadius;
        projectileScript.damageRadius = damageRadius;
        projectileScript.launcher = gameObject;
    }
}
