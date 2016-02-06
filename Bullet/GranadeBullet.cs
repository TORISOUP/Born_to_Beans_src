using System;
using UnityEngine;
using System.Collections;
using GGJ.Bullet;
using GGJ.Damages;
using UniRx;
using UniRx.Triggers;
using Random = UnityEngine.Random;

public class GranadeBullet : BaseBullet {

    Rigidbody rigidBody;

    [SerializeField]
    private float lifeTime = 3.0f;

    [SerializeField] private int createBulletCount = 30;

    [SerializeField] private GameObject smallBullet;

    protected override void OnStart()
    {

        rigidBody = GetComponent<Rigidbody>();

        this.FixedUpdateAsObservable().First().Subscribe(_ =>
        {
            rigidBody.velocity = transform.forward * _speed;
        });


        //一定時間後に爆発
        Observable.Timer(TimeSpan.FromSeconds(lifeTime)).AsUnitObservable()
            .Subscribe(_ =>
            {
                Explosion();
                Destroy(this.gameObject);
            }).AddTo(gameObject);

        //発射直後に爆発しないように少し遅れてSubscribe
        Observable.Timer(TimeSpan.FromMilliseconds(50))
            .SelectMany(_ => Observable.Merge(
                this.OnCollisionEnterAsObservable().AsUnitObservable(),
                this.OnTriggerEnterAsObservable().AsUnitObservable()
                ).FirstOrDefault())
            .Subscribe(_ =>
            {
                //何かにあたっても爆発
                Explosion();
                Destroy(this.gameObject);
            }).AddTo(gameObject);

    }

    private void Explosion()
    {
        for (int i = 0; i < createBulletCount; i++)
        {
            CreateBullet();
        }
        PlayShotHit();
    }

    private void CreateBullet()
    {
        var startPos = this.transform.position;
        var randomQuaternion = Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.right)
                               *Quaternion.AngleAxis(Random.Range(-180.0f, 180.0f), Vector3.up);
        var b = Instantiate(smallBullet, startPos, randomQuaternion) as GameObject;
        b.GetComponent<BaseBullet>().RegisterAttacker(attacker);
        

    }

    protected override void Hit(GameObject hitTarget)
    {
      
    }

}
