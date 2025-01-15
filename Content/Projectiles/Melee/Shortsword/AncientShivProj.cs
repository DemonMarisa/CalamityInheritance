using CalamityInheritance.CIPlayer;
using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class AncientShivProj : BaseShortswordProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(15);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>(); ;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 30 / 2;
            const int HalfSpriteHeight = 30 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
            {
                int MagicMirror = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.MagicMirror, (float)(Projectile.direction * 2), 0f, 15, default, 1.3f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityInheritancePlayer modPlayer = Main.player[Projectile.owner].GetModPlayer<CalamityInheritancePlayer>();

            modPlayer.ProjectilHitCounter++;
            if (modPlayer.ProjectilHitCounter >= 5)
            {
                var source = Projectile.GetSource_FromThis();
                Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<BlueAura>(), Projectile.damage * 2, Projectile.knockBack);
                modPlayer.ProjectilHitCounter = 0;
            }
        }
    }
}
