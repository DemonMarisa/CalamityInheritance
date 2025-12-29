using CalamityMod;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class IchorSpearLegacy : CIRogueClass
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<IchorSpear>(false);
        }
        public override void ExSD()
        {
            Item.width = 52;
            Item.damage = 96;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 52;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ProjectileType<IchorSpearProjLegacy>();
            Item.shootSpeed = 20f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ichor, 20).
                AddIngredient(ItemID.Vertebrae, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
