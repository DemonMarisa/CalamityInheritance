using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.Sounds;
using Terraria.Audio;
using CalamityInheritance.Content.Projectiles.Ranged;
using LAP.Content.RecipeGroupAdd;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ChargedDartRifle : CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 325;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 74;
            Item.height = 40;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.shootSpeed = 22f;
            Item.shoot = ModContent.ProjectileType<ChargedBlast>();
            Item.useAmmo = AmmoID.Dart;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool AltFunctionUse(Player player) => true;


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CommonCalamitySounds.LaserCannonSound);
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargedBlastLegacy3>(), (int)((double)damage * 2), knockback, player.whoAmI, 0f, 0f);
                return false;
            }
            else
            {
                int num6 = Main.rand.Next(2, 5);
                for (int index = 0; index < num6; ++index)
                {
                    float SpeedX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                    float SpeedY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                    int projectile = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage / 2, knockback, player.whoAmI, 0f, 0f);
                }
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargedBlastLegacy>(), damage, knockback, player.whoAmI, 0f, 0f);
                return false;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddRecipeGroup(LAPRecipeGroup.AnyDartGun)
                .AddIngredient(ItemID.MartianConduitPlating, 25)
                .AddIngredient<CoreofEleum>(3)
                .AddIngredient(ItemID.FragmentVortex, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
