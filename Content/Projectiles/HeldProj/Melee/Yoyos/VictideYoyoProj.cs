using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Melee.Yoyos;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Yoyos
{
    public class VictideYoyoProj : CIMeleeProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<VictideYoyo>();
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 7f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 190f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 11f;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = 1.15f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 90f)
            {
                if (Projectile.IsLocalPlayer())
                {
                    NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 300);
                    if (npc is not null)
                    {
                        Vector2 fireVel = LAPUtilities.GetVector2(Projectile.Center, npc.Center);
                        Projectile.NewProj(ProjectileType<VictideShell>(), Projectile.Center, fireVel * 12f, Projectile.damage, Projectile.knockBack);
                        Projectile.localAI[0] = 0;
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
