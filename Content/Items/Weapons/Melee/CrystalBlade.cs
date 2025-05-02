using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityInheritance.Content.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class CrystalBlade : CIMelee, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.damage = 45;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<CrystalDust>();
            Item.shootSpeed = 3f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CrystalShard, 10).
                AddIngredient(ItemID.PixieDust, 10).
                AddRecipeGroup("AnyCobaltBar", 8).
                AddIngredient<SeaPrism>(15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
