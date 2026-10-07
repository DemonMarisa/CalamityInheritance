using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Magic.GreatStaff;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Projectiles.ExoWeapons;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class SubsumingVortexold : CIExoWeapon
    {
        public static readonly SoundStyle[] TossSound =
        [
            CISounds.VortexToss1,
            CISounds.VortexToss2,
            CISounds.VortexToss3
        ];
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 235;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 78;
            Item.width = 38;
            Item.height = 48;
            Item.UseSound = SoundID.Item84;
            Item.useTime = Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.rare = RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<EnormousConsumingVortexold>();
            Item.shootSpeed = 7f;
        }
        public override bool CanUseItem(Player player)
        {
            if (UseAlt)
            {
                Item.UseSound = Utils.SelectRandom(Main.rand, TossSound);
            }
            else Item.UseSound = SoundID.Item84;
            return true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (UseAlt)
                damage.Base *= 0.6f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseAlt)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(Item, 0, null), position, velocity * 4, ProjectileType<EnormousConsumingVortexoldExoLore>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            }
            else
            {
                Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(Item, 0, null), position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f, 0f);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void AddRecipes()
        {
        }
    }
}
