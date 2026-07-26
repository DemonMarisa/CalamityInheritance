using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Spears
{
    public class VictideThrowSpearProj : CIMeleeProj
    {
        public override string Texture => GetInstance<VictideSpear>().Texture;
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<VictideSpear>();
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(3))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustWater, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 1.2f);
            if (Main.rand.NextBool(4))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustWaterCandle, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 0.3f);
        }
        public override void OnKill(int timeLeft)
        {
            CIUtils.DustCircle(Projectile.Center, 24, 2f, CIDustID.DustWater, true, 12f);
            //而后，生成水环
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position,
                new Vector2(Main.rand.NextFloat(-0.4f, 0.5f), Main.rand.NextFloat(-3, -6)),
                ProjectileType<VictideWaterRing>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.BaseProjPreDraw(TextureAssets.Projectile[Type].Value, lightColor, MathHelper.PiOver4);
            return false;
        }
    }
}
