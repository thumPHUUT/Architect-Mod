using EntityStates;
using ArchitectMod.Survivors.Architect;
using ArchitectMod.Survivors.Architect.Components;
using System;
using RoR2;
using RoR2.UI;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using R2API;
using UnityEngine.Networking;

namespace ArchitectMod.Survivors.Architect.SkillStates
{
    public class Evade : BaseSkillState
    {
        public static float duration = 0.5f; //changed to scale with attack speed
        public static float initialSpeedCoefficient = 5f;
        public static float finalSpeedCoefficient = 2.5f;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;


        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;


        private bool placedProjectile;
        public static GameObject projectilePrefab;

        //default values from Pilot mod
        public static GameObject tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/CaptainDefenseMatrix/TracerCaptainDefenseMatrix.prefab").WaitForCompletion();
        public static string muzzleName = "Muzzle";

        public override void OnEnter()
        {
            base.OnEnter();
            //for placing wall
            if (isAuthority)
            {
                this.PlaceProjectile();
            }
            this.DoAnim();

            animator = GetModelAnimator();

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = (inputBank.moveVector == Vector3.zero ? characterDirection.forward : inputBank.moveVector).normalized;
            }

            Vector3 rhs = characterDirection ? characterDirection.forward : forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            float num = Vector3.Dot(forwardDirection, rhs);
            float num2 = Vector3.Dot(forwardDirection, rhs2);

            RecalculateEvadeSpeed();

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity.y = 0f;
                characterMotor.velocity = forwardDirection * rollSpeed;
            }

            Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            previousPosition = transform.position - b;

            PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", duration);
            Util.PlaySound(dodgeSoundString, gameObject);

        }

        private void RecalculateEvadeSpeed()
        {
            rollSpeed = moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RecalculateEvadeSpeed();

            if (characterDirection) characterDirection.forward = forwardDirection;
            if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);

            Vector3 normalized = (transform.position - previousPosition).normalized;
            if (characterMotor && characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * rollSpeed;
                float d = Mathf.Max(Vector3.Dot(vector, forwardDirection), 0f);
                vector = forwardDirection * d;
                vector.y = 0f;

                characterMotor.velocity = vector;
            }
            previousPosition = transform.position;


            
            if (fixedAge >= duration && isAuthority)
            {
                ArchitectStaticValues.hurlDamageMultiplier = ArchitectStaticValues.hurlEvadeMultiplier;
                ArchitectStaticValues.isCloseHurl = false;
                outer.SetNextState(new Store());
                return;
            }
        }

        public override void OnExit()
        {
            if (cameraTargetParams) cameraTargetParams.fovOverride = -1f;
            base.OnExit();

            characterMotor.disableAirControlUntilCollision = false;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }

        #region place wall

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
                PlaceWall.CreateProjectile(projectilePrefab, 0f, base.gameObject, base.RollCrit(), hitInfo.point); //setting dmg to zero
            }
            return false;
        }

        public static void CreatePrefab()
        {
            //characterBody.GetComponent<ProjectileTrackerComponent>().deletableProjectile

            projectilePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIcewallPillarProjectile.prefab").WaitForCompletion(), "CastWall");
            //RoR2/Base/Mage/MageIcewallPillarProjectile.prefab   ||   

            //rescaling (note that create projectile will rotate upright afterwards, wall is currently horizontal, y should be height, z width)
            //projectilePrefab.transform.localScale = new Vector3(1f, 3f, 6f);

            //adding colision (this has not been tested)
            //projectilePrefab.transform.GetChild(1).gameObject.AddComponent<BoxCollider>();

        }
        #endregion place wall

    }
}