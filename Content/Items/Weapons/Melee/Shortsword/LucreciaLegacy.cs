using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Shortsword;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Materials;
using LAP.Common.CalamityModCross;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class LucreciaLegacy : CIMelee
    {
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 58;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = Item.useTime = 30;
            Item.shoot = ProjectileType<LucreciaProj>();
            Item.shootSpeed = 4.2f;
            Item.knockBack = 8.25f;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
        }

        public override bool MeleePrefix() => true;

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalMaterialsID.LifeAlloyID, 5).
                    AddIngredient(CalamityMaterials.CoreofCalamity, 5).
                    AddIngredient(ItemID.SoulofLight, 5).
                    AddIngredient(ItemID.SoulofNight, 5).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.BeetleHusk, 5).
                    AddIngredient(ItemID.SoulofLight, 5).
                    AddIngredient(ItemID.SoulofNight, 5).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
