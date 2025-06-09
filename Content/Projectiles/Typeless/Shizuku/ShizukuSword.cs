using System;
using CalamityInheritance.Content.Items.Weapons.Typeless;
using CalamityInheritance.Utilities;
using Microsoft.Build.Evaluation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuSword : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public int TargetIndex
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public ref float AttackTimer => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 112;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.timeLeft = 18000 * 5;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            CheckHeldItem();
            NPC anyTarget = Projectile.FindClosestTarget(CIFunction.SetDistance(100));
            if (anyTarget is null)
                IdlePosition();
            else
            {
                TargetIndex = anyTarget.whoAmI;
                DoAttacking();
            }
        }

        public void DoAttacking()
        {
            NPC target = Main.npc[TargetIndex];

        }

        public void IdlePosition()
        {

        }

        public void CheckHeldItem()
        {
            if (Owner.HeldItem.type != ModContent.ItemType<ShizukuEdge>())
            {
                Projectile.Kill();
            }
        }
    }
}