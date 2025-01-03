using CalamityMod.Items.Weapons.Rogue;
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
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.LoreItems;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class Quasar : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.damage = 80; //50
            Item.crit += 12;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 0f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 48;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.shoot = ModContent.ProjectileType<QuasarKnife>();
            Item.shootSpeed = 20f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.rare = ItemRarityID.Cyan;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
            {
                Recipe recipe1 = CreateRecipe();
                recipe1.AddIngredient<LoreAstrumDeus>();
                recipe1.AddTile(TileID.AncientMythrilBrick);
                recipe1.Register();

                Recipe recipe2 = CreateRecipe();
                recipe2.AddIngredient<KnowledgeAstrumDeus>();
                recipe2.AddTile(TileID.AncientMythrilBrick);
                recipe2.Register();
            }
        }
    }
}
