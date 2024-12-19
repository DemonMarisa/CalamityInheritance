using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class FlameburstShortsword : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 36;
            Item.height = 38;
            Item.damage = 35;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<FlameburstShortswordProj>();
            Item.shootSpeed = 4f;
            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HellstoneBar, 7).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
