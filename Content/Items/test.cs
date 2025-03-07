using CalamityInheritance.CIPlayer;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.CICooldowns;
using CalamityMod.Cooldowns;

namespace CalamityInheritance.Content.Items
{
    public class Test : ModItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public new string LocalizationCategory => "Content.Items.Weapons.Melee";

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<MiniRocket>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            Projectile.NewProjectile(source, position, velocity * 1.5f, ModContent.ProjectileType<MiniRocket>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override bool? UseItem(Player player)
        {
            player.RemoveCalCooldown(GodSlayerDash.ID);
            player.RemoveCalCooldown(LifeSteal.ID);
            return base.CanUseItem(player);
        }
    }
}
