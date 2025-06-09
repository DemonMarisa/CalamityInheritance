using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Summon;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class SarosPossessionLegacy : CISummon, ILocalizedModType
    {
        int radianceSlots;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 56;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_BetsyFlameBreath;

            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.damage = 200;
            Item.knockBack = 4f;
            Item.useTime = Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<SarosAuraLegacy>();
            Item.shootSpeed = 10f;

            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void HoldItem(Player player)
        {
            double minionCount = 0;
            for (int j = 0; j < Main.projectile.Length; j++)
            {
                Projectile proj = Main.projectile[j];
                if (proj.active && proj.owner == player.whoAmI && proj.minion && proj.type != Item.shoot)
                {
                    minionCount += proj.minionSlots;
                }
            }
            radianceSlots = (int)(player.maxMinions - minionCount);
        }

        public override bool CanUseItem(Player player)
        {
            return radianceSlots >= 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityUtils.KillShootProjectiles(true, type, player);
            int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, radianceSlots);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Sirius>().
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<DarksunFragment>(8).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<SiriusLegacy>().
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<DarksunFragment>(8).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
