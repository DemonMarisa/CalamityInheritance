using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Ranged.Flamethrower;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Flamethrower
{
    public class HalleysInfernoLegacy : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 1350;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Gel;
            Item.shootSpeed = 14.6f;
            Item.shoot = ProjectileType<HalleysCometLegacy>();

            Item.width = 84;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = CISoundID.SoundFlamethrower;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 20;

        public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(100) >= 50;

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                AddIngredient(ItemID.RifleScope).
                AddIngredient(CalamityMaterials.Lumenyl, 6).
                AddIngredient(CalamityMaterials.RuinousSoul, 4).
                AddIngredient(CalamityMaterials.ExodiumCluster, 12).
                AddTile(TileID.LunarCraftingStation).
                Register();
            }
            else
            {
            }
        }
    }
}
