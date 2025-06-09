using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
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
using CalamityMod.Items.Weapons.Summon;
using CalamityInheritance.Content.Projectiles.Summon;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class SiriusLegacy : CISummon, ILocalizedModType
    {
        int siriusSlots;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item44;

            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.damage = 175;
            Item.knockBack = 3f;
            Item.useTime = Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<SiriusMinionLegacy>();
            Item.shootSpeed = 10f;

            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void HoldItem(Player player)
        {
            double minionCount = 0;
            // 遍历弹幕来获取其它召唤物所占用的召唤栏
            for (int j = 0; j < Main.projectile.Length; j++)
            {
                Projectile proj = Main.projectile[j];
                if (proj.active && proj.owner == player.whoAmI && proj.minion && proj.type != Item.shoot)
                {
                    minionCount += proj.minionSlots;
                }
            }
            // 召唤时实际的召唤栏数量
            siriusSlots = (int)(player.maxMinions - minionCount);
        }

        public override bool CanUseItem(Player player)
        {
            return siriusSlots >= 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityUtils.KillShootProjectiles(true, type, player);
            int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, siriusSlots, 30f);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<VengefulSunStaff>().
                AddIngredient<Lumenyl>(5).
                AddIngredient<RuinousSoul>(2).
                AddIngredient<ExodiumCluster>(12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
