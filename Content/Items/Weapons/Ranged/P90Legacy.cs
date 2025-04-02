using Microsoft.Xna.Framework;
using CalamityMod;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class P90Legacy : CIRanged, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = Main.zenithWorld? 12 : 6;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 60;
            Item.height = 28;
            Item.useTime = Main.zenithWorld? 1 : 2;
            Item.useAnimation = 2;
            Item.ArmorPenetration = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.useAmmo = 97;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-14, -1);
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.damage = 12;
                Item.useTime = 1;
            }
            else
            {
                Item.damage = 6;
                Item.useTime = 2;
            }
            return default;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
                float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
                if(!Main.zenithWorld)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
                }
                else
                {
                    for(int i = 0; i < 12 ; i++)
                    {
                        Vector2 spread = new Vector2(SpeedX,SpeedY).RotatedByRandom(180f)*Main.rand.NextFloat(0.9f, 1.2f);
                        Projectile.NewProjectile(source, position, spread, type, damage, knockback, player.whoAmI, 0f, 0f);
                    }
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
                }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IronBar, 10).
                AddIngredient<CoreofEleum>(7).
                AddTile(TileID.MythrilAnvil).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.LeadBar, 10).
                AddIngredient<CoreofEleum>(7).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
