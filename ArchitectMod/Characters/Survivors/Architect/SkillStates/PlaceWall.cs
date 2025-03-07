using EntityStates;
using ArchitectMod.Survivors.Architect;
using System;
using RoR2;
using ArchitectMod.Modules;
using RoR2.UI;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using R2API;


namespace ArchitectMod.Survivors.Architect.SkillStates
{
    public class PlaceWall : BaseSkillState
    {
        public static float duration = 0.2f; //duration of cast and anim
        public static float damageCoefficient = ArchitectStaticValues.placeWallDamageCoefficient;

        private bool placedProjectile;
        public static GameObject projectilePrefab;
        public static GameObject bombExplosionEffect;
        private static AssetBundle _assetBundle;


        //default values from Pilot mod
        public static GameObject tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/CaptainDefenseMatrix/TracerCaptainDefenseMatrix.prefab").WaitForCompletion();
        public static string muzzleName = "Muzzle";


        //theory: aims invisible bullet attack, and on its callback (once it hits something), places projectile at location the bullet attack hit
        public override void OnEnter()
        {
            base.OnEnter();

            this.placedProjectile = false;

            if (isAuthority)
            {
                this.PlaceProjectile();
            }
            this.DoAnim();

        }

        //following 4 methods from EnforcerGang's Pilot mod
        public virtual void PlaceProjectile()
        {
            CreatePrefab();
            Ray aimRay = base.GetAimRay();
            BulletAttack bulletAttack = new BulletAttack
            {
                tracerEffectPrefab = PlaceWall.tracerEffectPrefab,
                damage = 0f,
                procCoefficient = 0f,
                damageType = (DamageType.NonLethal | DamageType.Silent),
                owner = base.gameObject,
                aimVector = Vector3.down, //this will set the projectile at Architects feet
                isCrit = false,
                minSpread = 0f,
                maxSpread = 0f,
                origin = aimRay.origin,
                maxDistance = 2000f,
                muzzleName = PlaceWall.muzzleName,
                radius = 0.2f,
                hitCallback = new BulletAttack.HitCallback(this.PlaceProjectileHitCallback),
                stopperMask = LayerIndex.world.mask
            };
            bulletAttack.Fire();
        }

        public static void CreateProjectile(GameObject projectilePrefab, float damage, GameObject attacker, bool crit, Vector3 position)
        {
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = projectilePrefab,
                crit = crit,
                damage = damage,
                damageColorIndex = DamageColorIndex.Default,
                force = 0f,
                owner = attacker,
                position = position,
                procChainMask = default(ProcChainMask),
                rotation = Quaternion.Euler(270f, 0f, 0f),
                speedOverride = 0f
            };
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }

        protected virtual void DoAnim()
        {
            PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", duration);

        }

        protected bool PlaceProjectileHitCallback(BulletAttack bulletRef, ref BulletAttack.BulletHit hitInfo)
        {
            bool flag = !this.placedProjectile;
            if (flag)
            {
                this.placedProjectile = true;
                PlaceWall.CreateProjectile(projectilePrefab, 0f, base.gameObject, base.RollCrit(), hitInfo.point); //add impact damage and then impact explosion (like in assets)
            }
            return false;
        }

        public static void CreatePrefab()
        {


            projectilePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIcewallPillarProjectile.prefab").WaitForCompletion(), "CastWall");
            //RoR2/Base/Mage/MageIcewallPillarProjectile.prefab   ||   
            
            //rescaling (note that create projectile will rotate upright afterwards, wall is currently horizontal, y should be height, z width)
            projectilePrefab.transform.localScale = new Vector3(2f, 2f, 2f);
                       
        }

        public override void OnExit()
        {
            base.OnExit();
        }


        public override void FixedUpdate()
        {
            if (fixedAge >= duration && isAuthority)
            {
                //not making this hurlable
                //ArchitectStaticValues.hurlDamageMultiplier = ArchitectStaticValues.hurlPlaceWallMultiplier;
                //ArchitectStaticValues.isCloseHurl = true;
                //outer.SetNextState(new Store());
                outer.SetNextStateToMain();
                return;
            }
        }

        
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}