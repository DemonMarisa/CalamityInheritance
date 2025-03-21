using Terraria.DataStructures;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class AuroraBlazerLegacy : CIRanged, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 49;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.knockBack = 2f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AuroraFireLegacy>();
            Item.shootSpeed = 7.5f;
            Item.useAmmo = AmmoID.Gel;

            Item.width = 68;
            Item.height = 36;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = CISoundID.SoundFlamethrower;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.NextFloat(-15f, 15f) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.NextFloat(-15f, 15f) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, Main.rand.Next(4, 11), 0f);
            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/AuroraBlazerLegacyGlow").Value);
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(100) >= 60;
    }
}
