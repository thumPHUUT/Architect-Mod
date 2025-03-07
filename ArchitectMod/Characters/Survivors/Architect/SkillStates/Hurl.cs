using EntityStates;
using ArchitectMod.Survivors.Architect;
using ArchitectMod.Survivors.Architect.Components;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ArchitectMod.Survivors.Architect.SkillStates
{
    public class Hurl : GenericProjectileBaseState
    {

        public static float BaseDuration = 0.65f;
        //delays for projectiles feel absolute ass so only do this if you know what you're doing, otherwise it's best to keep it at 0
        public static float BaseDelayDuration = 0.0f;

        public static float DamageCoefficient = ArchitectStaticValues.hurlDamageCoefficient * ArchitectStaticValues.hurlDamageMultiplier;

        public override void OnEnter()
        {

            //Changing speed my making a clone of a game projectile is much easier and works. the code below is a NON FUNCTIONAL example
            //
            //"object you want to instantiate is null" -> this doesnt work
            /*
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = ArchitectAssets.bombProjectilePrefab,
                speedOverride = 10f,
                
                position = base.inputBank.GetAimRay().GetPoint(256f),
                rotation = Quaternion.identity,
                owner = base.gameObject,
                damage = DamageCoefficient,
                force = 80f, //arbitrary number
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                target = null,
                fuseOverride = -1f
            };
            */
            
            //close hurl needs re-work (currently unused as special is no longer hurlabe)
            if (ArchitectStaticValues.isCloseHurl)
            {
                projectilePrefab = ArchitectAssets.closeBombProjectilePrefab;
            }
            else
            {
                projectilePrefab = ArchitectAssets.farBombProjectilePrefab;
            }
            
            //base.effectPrefab = Modules.Assets.SomeMuzzleEffect;
            //targetmuzzle = "muzzleThrow"

            

            attackSoundString = "HenryBombThrow";

            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = BaseDelayDuration;

            damageCoefficient = DamageCoefficient;
            //proc coefficient is set on the components of the projectile prefab
            force = 80f;

            //base.projectilePitchBonus = 0;
            //base.minSpread = 0;
            //base.maxSpread = 0;

            recoilAmplitude = 0.1f;
            bloom = 10;
            

            base.OnEnter();

            //delete projectile (a segment of ice wall or bubble sheild)
            UnityEngine.GameObject.Destroy(characterBody.GetComponent<ProjectileTrackerComponent>().deletableProjectile);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            
            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
            
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void PlayAnimation(float duration)
        {

            if (GetModelAnimator())
            {
                PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration); //can pick either
                //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1f);
            }
        }

        private void Fire()
        {
            RaycastHit raycastHit;
            Vector3 point = base.inputBank.GetAimRay().GetPoint(256f); //need to set arg of GetPoint to some float (this will be max distance) 256f is range of shoot
            
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = ArchitectAssets.closeBombProjectilePrefab,
                position = point,
                rotation = Quaternion.identity,
                owner = base.gameObject,
                damage = DamageCoefficient,
                force = 80f, //arbitrary number
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                target = null,
                speedOverride = 2f, //dont know what this scale is
                fuseOverride = -1f
            };
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }


        /*
        these are things i found in dnspy that i think might be useful/helpful to see
        

        //this is part of fire mortar from treebot.weapon in dnspy. imma try it
        private void Fire()
		{
			RaycastHit raycastHit;
			Vector3 point;
			if (base.inputBank.GetAimRaycast(FireMortar2.maxDistance, out raycastHit))
			{
				point = raycastHit.point;
			}
			else
			{
				point = base.inputBank.GetAimRay().GetPoint(FireMortar2.maxDistance);
			}
			FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
			{
				projectilePrefab = FireMortar2.projectilePrefab,
				position = point,
				rotation = Quaternion.identity,
				owner = base.gameObject,
				damage = FireMortar2.damageCoefficient * this.damageStat,
				force = FireMortar2.force,
				crit = base.RollCrit(),
				damageColorIndex = DamageColorIndex.Default,
				target = null,
				speedOverride = 0f,
				fuseOverride = -1f
			};
			ProjectileManager.instance.FireProjectile(fireProjectileInfo);
		}
        
        public void FireProjectile(GameObject prefab, Vector3 position, Quaternion rotation, GameObject owner, float damage, float force, bool crit, DamageColorIndex damageColorIndex = DamageColorIndex.Default, GameObject target = null, float speedOverride = -1f)
        {
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = ArchitectAssets.bombProjectilePrefab,
                position = position,
                rotation = rotation,
                owner = owner,
                damage = damage,
                force = force,
                crit = crit,
                damageColorIndex = damageColorIndex,
                target = target,
                speedOverride = -1f,
                fuseOverride = -1f,
                damageTypeOverride = null
            };
            this.FireProjectile(fireProjectileInfo);
        }


        protected override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo)
		{
			base.ModifyProjectile(ref fireProjectileInfo);
			fireProjectileInfo.position = this.currentTrajectoryInfo.hitPoint;
			fireProjectileInfo.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
			fireProjectileInfo.speedOverride = 0f;
		}


        */
    }
}