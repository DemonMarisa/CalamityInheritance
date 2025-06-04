using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using CalamityInheritance.Utilities;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityInheritance.Content.Projectiles.HeldProj.Magic;

namespace CalamityInheritance.Content.Items.Weapons.Wulfrum
{
    public class WulfrumStaff : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<WulfrumProsthesis>(false);
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 2;
            Item.width = 44;
            Item.height = 46;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<WulfrumStaffHoldOut>();
            Item.shootSpeed = 9f;

            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.rare = ModContent.RarityType<IchikaBlack>();
                Item.value = CIShopValue.RarityPricePureRed;
                Item.UseSound = CISoundID.SoundFart;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            else
            {
                Item.rare = ItemRarityID.Blue;
                Item.value = CIShopValue.RarityPriceBlue;
                Item.UseSound = CISoundID.SoundStaffDiamond;
                Item.useStyle = ItemUseStyleID.Shoot;
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<WulfrumStaffHoldOut>()] < 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo projSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.ownedProjectileCounts[ModContent.ProjectileType<WulfrumStaffHoldOut>()] < 1)
                Projectile.NewProjectileDirect(projSource, position, velocity, ModContent.ProjectileType<WulfrumStaffHoldOut>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumMetalScrap> (12).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
