using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ranged.StarCannon;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.StarCannon
{
    public class StarCannonEX : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 102;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 74;
            Item.height = 24;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Lime;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = CIShopValue.RarityPriceLime;
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<FallenStarProj>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.FallenStar;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void UseItemFrame(Player player)
        {
            CIUtils.NoHeldProjUpdateAim(player, 0, 1);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = 3;
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
                type = Utils.SelectRandom(Main.rand, new int[]
                {
                    ProjectileType<AstralStarLegacy>(),
                    ProjectileID.StarCannonStar,
                    ProjectileID.SuperStar,
                    ProjectileType<FallenStarProj>()
                });
                int star = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
                Main.projectile[star].DamageType = DamageClass.Ranged;
            }
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.SuperStarCannon).
                    AddIngredient(CalamityMaterials.AureusCell, 10).
                    AddIngredient(CalamityMaterials.StarblightSoot, 25).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.SuperStarCannon).
                    AddIngredient<CoreofSunlight>(3).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
