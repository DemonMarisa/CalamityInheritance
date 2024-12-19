using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class ExcaliburShortsword : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 10;
            Item.width = 40;
            Item.height = 40;
            Item.damage = 140;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<ExcaliburShortswordProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HallowedBar,7).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
