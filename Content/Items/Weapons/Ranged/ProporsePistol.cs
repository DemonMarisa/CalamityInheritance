using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ProporsePistol : CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 20;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<ProBolt>();
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, -3);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y - 5, velocity.X, velocity.Y, ModContent.ProjectileType<ProBolt>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void UseItemFrame(Player player)
        {
            player.ChangeDir(Math.Sign((player.Calamity().mouseWorld - player.Center).X));

            float animProgress = 0.5f - player.itemTime / (float)player.itemTimeMax;
            // 向鼠标的旋转
            float rotation = (player.Center - player.Calamity().mouseWorld).ToRotation() * player.gravDir + MathHelper.PiOver2;
            float offset = -0.03f * (float)Math.Pow((0.6f - animProgress) / 0.6f, 2);
            if (animProgress < 0.4f)
                rotation += offset * player.direction;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rotation);
            CIFunction.NoHeldProjUpdateAim(player, MathHelper.ToDegrees(offset), 1);
        }
    }
}
