using RoR2;
using UnityEngine;
using ArchitectMod.Modules;
using System;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using R2API;

//Kept all but namespace in Henry as this references Unity assets that are still named in Henry
namespace ArchitectMod.Survivors.Architect
{
    public static class ArchitectAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject farBombProjectilePrefab;
        public static GameObject closeBombProjectilePrefab;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();
        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");
        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateFarBombProjectile();
            CreateCloseBombProjectile();
            Content.AddProjectilePrefab(farBombProjectilePrefab);
            Content.AddProjectilePrefab(closeBombProjectilePrefab);
        }

        private static void CreateCloseBombProjectile()
        {
            //can also try (RoR2/Base/Commando/CommandoGrenadeProjectile.prefab) RoR2/Base/Merc/EvisProjectile.prefab which i think is slicing winds prefab, tho shiv works well as is
            closeBombProjectilePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoGrenadeProjectile.prefab").WaitForCompletion(), "HenryBombProjectile");
            //original is HenryBombProjectile. this is just the name of the projectile if Im not mistaken

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(closeBombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = closeBombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();


            bombImpactExplosion.blastRadius = 12f; //original value was 16
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.Linear; //could change back to none. exponential/sweetspot is too punishing imo
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 16f; //original val of 12
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;

            ProjectileController bombController = closeBombProjectilePrefab.GetComponent<ProjectileController>();

            //this seems to change the colour pallette of the projectile clone, but not its mesh?
            //switched "HenryBombGhost" with "donutProjectile" (test) also trying evadeWall1
            if (_assetBundle.LoadAsset<GameObject>("donutProjectile") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("donutProjectile");
            //else
            //    bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");

            bombController.startSound = "";
        }

    private static void CreateFarBombProjectile()
        {
            //If I cant change projectile visuals, evis projectile is most thematically in line with what I want (for all projectiles)
            
            //can also try RoR2/Base/Merc/EvisProjectile.prefab which i think is slicing winds prefab, RoR2/Base/Croco/CrocoDiseaseProjectile.prefab,  RoR2/Base/Bandit2/Bandit2ShivProjectile.prefab
            farBombProjectilePrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/EvisProjectile.prefab").WaitForCompletion(), "donutProjectile");
            //original is HenryBombProjectile. this is just the name of the projectile if Im not mistaken

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(farBombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = farBombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();
            

            bombImpactExplosion.blastRadius = 20f; //original value was 16
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.Linear; //could change back to none. exponential/sweetspot is too punishing imo
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f; //original val of 12
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0f;
            
            ProjectileController bombController = farBombProjectilePrefab.GetComponent<ProjectileController>();
            
            
            //this is goofy af, turn it on and see what happens (not what youd think)
            //bombController.ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIcewallPillarProjectile.prefab").WaitForCompletion();

            //ghost changes appearance. Need to find a way to make mage wall work (currently not in assetBundle)
            if (_assetBundle.LoadAsset<GameObject>("RoR2/Base/Mage/MageIcewallPillarProjectile.prefab") != null)
            {
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("RoR2/Base/Mage/MageIcewallPillarProjectile.prefab");
            }
           
            bombController.startSound = "";

        }
        #endregion projectiles
    }
}
