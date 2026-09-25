using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Typeless.LuxorsGiftLegacy;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class LuxorsGiftLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 48;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().LuxorsGiftLegacyShoot = true;
        }
        public static void Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage)
        {
            if (player.whoAmI == Main.myPlayer && !item.channel)
            {
                if (player.CI().LuxorsGiftLegacyShoot)
                {
                    int Melee = (int)(damage * 0.25f);
                    int Ranged = (int)(damage * 0.15f);
                    int Magic = (int)(damage * 0.3f);
                    int Summon = damage;
                    int Rogue = (int)(damage * 0.2f);
                    int Cap = 1200;
                    if (item.CountsAsClass<MeleeDamageClass>())
                    {
                        int meleeDamage = LAPUtilities.DamageSoftCap(Melee, Cap);
                        if (meleeDamage >= 1)
                        {
                            int projectile = Projectile.NewProjectile(source, position, velocity * 0.5f, ProjectileType<LuxorsGiftMeleeLegacy>(), meleeDamage, 0f, player.whoAmI);
                            if (projectile.InBounds(Main.maxProjectiles))
                                Main.projectile[projectile].DamageType = DamageClass.Generic;
                        }
                    }
                    else if (item.CountsAsClass<ThrowingDamageClass>())
                    {
                        int throwingDamage = LAPUtilities.DamageSoftCap(Rogue, Cap);
                        if (throwingDamage >= 1)
                        {
                            int projectile = Projectile.NewProjectile(source, position, velocity, ProjectileType<LuxorsGiftRogueLegacy>(), (int)throwingDamage, 0f, player.whoAmI);
                            if (projectile.InBounds(Main.maxProjectiles))
                                Main.projectile[projectile].DamageType = DamageClass.Generic;
                        }
                    }
                    else if (item.CountsAsClass<RangedDamageClass>())
                    {
                        int rangedDamage = LAPUtilities.DamageSoftCap(Ranged, Cap);
                        if (rangedDamage >= 1)
                        {
                            int projectile = Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileType<LuxorsGiftRangedLegacy>(), (int)rangedDamage, 0f, player.whoAmI);
                            if (projectile.InBounds(Main.maxProjectiles))
                                Main.projectile[projectile].DamageType = DamageClass.Generic;
                        }
                    }
                    else if (item.CountsAsClass<MagicDamageClass>())
                    {
                        int magicDamage = LAPUtilities.DamageSoftCap(Magic, Cap);
                        if (magicDamage >= 1)
                        {
                            int projectile = Projectile.NewProjectile(source, position, velocity, ProjectileType<LuxorsGiftMagicLegacy>(), (int)magicDamage, 0f, player.whoAmI);
                            if (projectile.InBounds(Main.maxProjectiles))
                                Main.projectile[projectile].DamageType = DamageClass.Generic;
                        }
                    }
                    else if (item.CountsAsClass<SummonDamageClass>() && player.ownedProjectileCounts[ProjectileType<LuxorsGiftSummonLegacy>()] < 1)
                    {
                        if (damage >= 1)
                        {
                            int baseDamage = LAPUtilities.DamageSoftCap(item.damage, Cap);
                            int summonDamage = Summon;
                            int projectile = Projectile.NewProjectile(source, position, Vector2.Zero, ProjectileType<LuxorsGiftSummonLegacy>(), summonDamage, 0f, player.whoAmI);
                            if (projectile.InBounds(Main.maxProjectiles))
                            {
                                Main.projectile[projectile].DamageType = DamageClass.Generic;
                                Main.projectile[projectile].originalDamage = baseDamage;
                            }
                        }
                    }
                }
            }
        }
        public static void PostUpdate(Player player)
        {
            if (Main.myPlayer != player.whoAmI || player.ItemTimeIsZero || player.HeldItem is null || player.HeldItem.damage == 0)
                return;
            if (player.CI().HasCount("LuxorsGiftLegacyCooldown"))
                return;
            Item item = player.ActiveItem();
            IEntitySource source = player.GetSource_Accessory(item);
            Vector2 position = player.Center;
            Vector2 velocity = LAPUtilities.GetVector2(player.Center, player.LocalMouseWorld()) * item.shootSpeed;
            player.CI().AddCount("LuxorsGiftLegacyCooldown", player.ApplyWeaponAttackSpeed(item, item.useTime, 0));
            int damage = player.GetWeaponDamage(item);
            int Melee = (int)(damage * 0.25f);
            int Ranged = (int)(damage * 0.15f);
            int Magic = (int)(damage * 0.3f);
            int Summon = damage;
            int Rogue = (int)(damage * 0.2f);
            int Cap = 1200;
            if (item.CountsAsClass<MeleeDamageClass>())
            {
                int meleeDamage = LAPUtilities.DamageSoftCap(Melee, Cap);
                if (meleeDamage >= 1)
                {
                    int projectile = Projectile.NewProjectile(source, position, velocity * 0.5f, ProjectileType<LuxorsGiftMeleeLegacy>(), meleeDamage, 0f, player.whoAmI);
                    if (projectile.InBounds(Main.maxProjectiles))
                        Main.projectile[projectile].DamageType = DamageClass.Generic;
                }
            }
            else if (item.CountsAsClass<ThrowingDamageClass>())
            {
                int throwingDamage = LAPUtilities.DamageSoftCap(Rogue, Cap);
                if (throwingDamage >= 1)
                {
                    int projectile = Projectile.NewProjectile(source, position, velocity, ProjectileType<LuxorsGiftRogueLegacy>(), (int)throwingDamage, 0f, player.whoAmI);
                    if (projectile.InBounds(Main.maxProjectiles))
                        Main.projectile[projectile].DamageType = DamageClass.Generic;
                }
            }
            else if (item.CountsAsClass<RangedDamageClass>())
            {
                int rangedDamage = LAPUtilities.DamageSoftCap(Ranged, Cap);
                if (rangedDamage >= 1)
                {
                    int projectile = Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileType<LuxorsGiftRangedLegacy>(), (int)rangedDamage, 0f, player.whoAmI);
                    if (projectile.InBounds(Main.maxProjectiles))
                        Main.projectile[projectile].DamageType = DamageClass.Generic;
                }
            }
            else if (item.CountsAsClass<MagicDamageClass>())
            {
                int magicDamage = LAPUtilities.DamageSoftCap(Magic, Cap);
                if (magicDamage >= 1)
                {
                    int projectile = Projectile.NewProjectile(source, position, velocity, ProjectileType<LuxorsGiftMagicLegacy>(), (int)magicDamage, 0f, player.whoAmI);
                    if (projectile.InBounds(Main.maxProjectiles))
                        Main.projectile[projectile].DamageType = DamageClass.Generic;
                }
            }
            else if (item.CountsAsClass<SummonDamageClass>() && player.ownedProjectileCounts[ProjectileType<LuxorsGiftSummonLegacy>()] < 1)
            {
                if (damage >= 1)
                {
                    int baseDamage = LAPUtilities.DamageSoftCap(item.damage, Cap);
                    int summonDamage = Summon;
                    int projectile = Projectile.NewProjectile(source, position, Vector2.Zero, ProjectileType<LuxorsGiftSummonLegacy>(), summonDamage, 0f, player.whoAmI);
                    if (projectile.InBounds(Main.maxProjectiles))
                    {
                        Main.projectile[projectile].DamageType = DamageClass.Generic;
                        Main.projectile[projectile].originalDamage = baseDamage;
                    }
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ruby, 1).
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                AddIngredient(ItemID.HellstoneBar, 5).
                AddIngredient(ItemID.Bone, 5).
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.BeeWax, 5).
                DisableDecraft().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
