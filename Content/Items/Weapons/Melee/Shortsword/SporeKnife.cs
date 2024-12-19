using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.Pets;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class SporeKnife : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 28;
            Item.height = 28;
            Item.damage = 33;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SporeKnifeProj>();
            Item.shootSpeed = 3f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.JungleSpores, 10).
                AddIngredient(ItemID.Stinger, 5).
                AddTile(TileID.Anvils).
                Register();
        }

    }
}
