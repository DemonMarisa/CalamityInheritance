using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Melee.ArkWeapons;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Common.CalamityModCross;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.ArkWeapons
{
    public class TrueBiomeBlade : CIMelee
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.damage = 160;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<TrueBiomeOrb>();
            Item.shootSpeed = 12f;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Dirt);
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                AddIngredient<BiomeBlade>().
                AddIngredient(CalMaterialsID.LivingShardID, 5).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddIngredient(CalamityMaterials.DepthCells, 10).
                AddIngredient(CalamityMaterials.Lumenyl, 10).
                AddIngredient(CalamityMaterials.Voidstone, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
            }
            else
            {

                CreateRecipe().
                AddIngredient<BiomeBlade>().
                AddIngredient(ItemID.Ectoplasm, 5).
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();
            }
        }
    }
}
