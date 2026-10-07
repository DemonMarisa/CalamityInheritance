using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Projectiles.Typeless.HomeIn;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class MangroveChakramProj : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<MangroveChakram>();
        public override string Texture => GetInstance<MangroveChakram>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
        }

        public override void AI()
        {
            float spin = Projectile.direction <= 0 ? -1f : 1f;
            Projectile.rotation += spin * 0.15f;
            Lighting.AddLight(Projectile.Center, 0f, 0.25f, 0f);
            if (Projectile.CI().Stealth)
            {
                Projectile.localAI[0] -= Main.rand.Next(1, 3);
                if (Projectile.localAI[0] <= 0)
                {
                    Projectile.localAI[0] = 60f;
                    Vector2 flowerSpawnPosition = Projectile.Center + Main.rand.NextVector2Square(-10f, 10f);
                    Vector2 flowerShootVelocity = Projectile.velocity.RotatedByRandom(0.1f) * 0.25f;
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), flowerSpawnPosition, flowerShootVelocity, ProjectileType<PinkFlower>(), Projectile.damage / 4, 0f, Projectile.owner);
                    Main.projectile[p].SetStealthAttack();
                    Main.projectile[p].DamageType = RogueDamage.Instance;
                }
            }
            if (Projectile.timeLeft <= 180 - 45)
            {
                Player owner = Main.player[Projectile.owner];
                Projectile.PredictHomeIn(12f, 12f, owner);
                if (!Projectile.CI().Stealth)
                    Projectile.extraUpdates = 1;
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
                if (Main.rand.NextBool(10))
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.JungleSpore, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            else
            {
                if (Main.rand.NextBool(5))
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.JungleSpore, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Venom, 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Venom, 120);
    }
}
