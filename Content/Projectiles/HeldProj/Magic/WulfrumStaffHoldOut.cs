using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Content.Projectiles.Wulfrum;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic
{
    public class WulfrumStaffHoldOut : BaseHeldProjMagic, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override float OffsetX => 18;
        public override float OffsetY => -5;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 20;
        public override float AimResponsiveness => 0.25f;
        // public override string TexturePath => "Wulfrum/WulfrumStaff";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void HoldoutAI()
        {
            // 使用计时器
            ref float UseCounter = ref Projectile.ai[1];
            // 第一次的计数
            ref float secondcount = ref Projectile.ai[2];

            // Update damage based on curent magic damage stat (so Mana Sickness affects it)
            Projectile.damage = Owner.HeldItem is null ? 0 : Owner.GetWeaponDamage(Owner.HeldItem);

            // 开火方向
            Vector2 firedirection = Vector2.UnitX.RotatedBy(Projectile.rotation - MathHelper.ToRadians(WeaponRotation * Owner.direction));
            firedirection = firedirection.SafeNormalize(Vector2.UnitX);
            UseCounter++;
            if (secondcount > 0)
                secondcount--;
            if (UseCounter == 1)
            {
                secondcount = 30;
            }
            if (UseCounter % Owner.HeldItem.useTime == 0)
            {
                secondcount = 30;
            }
            if (secondcount > 0 && secondcount % 10 == 0)
            {
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                
                if(Main.zenithWorld)
                {
                    int[] pType =
                    [
                        ModContent.ProjectileType<GalaxyStarold>(),
                        ModContent.ProjectileType<ProfanedNuke>(),
                        ModContent.ProjectileType<ExoFlareClusterold>(),
                        ModContent.ProjectileType<ChickenRound>(),
                        ModContent.ProjectileType<Celestus2>(),
                        ModContent.ProjectileType<DragonRageProj>(),
                        ModContent.ProjectileType<PhantasmalRuinProjold>(),
                        ModContent.ProjectileType<RogueTypeHammerGalaxySmasherProjClone>(),
                        ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>(),
                        ModContent.ProjectileType<RogueTypeHammerTruePaladinsProjClone>(),
                        ModContent.ProjectileType<SupernovaBombold>(),
                        ModContent.ProjectileType<CIVividBeamExoLore>(),
                        ModContent.ProjectileType<SoulEdgeSoulLegacyLarge>(),
                        ModContent.ProjectileType<DragonsBreathRound>(),
                    ];
                    int randomProjectileType = pType[Main.rand.Next(pType.Length)];
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, firedirection * 9f, randomProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
                }

                SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, firedirection * 9f, ModContent.ProjectileType<WulfrumBoltOld>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

        }
        #region 删除条件
        public override void DelCondition()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 30)
                Projectile.Kill();
        }
        #endregion
    }
}
