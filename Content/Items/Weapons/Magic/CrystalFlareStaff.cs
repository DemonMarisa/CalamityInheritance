using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic;
using Terraria.DataStructures;
using CalamityMod.Buffs.StatBuffs;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class CrystalFlareStaff : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Fires blue frost flames that explode");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 44;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.25f;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpiritFlameCurse>();
            Item.shootSpeed = 14f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //5->7
            int pCounts = Main.rand.Next(5, 8);
            for (int i = 0; i < pCounts; i++)
            {
                Vector2 spread = velocity.RotatedByRandom(MathHelper.ToRadians(3f))  * Main.rand.NextFloat(0.8f, 1.1f);
                Projectile.NewProjectile(source, position, spread, ModContent.ProjectileType<SpiritFlameCurse>(), damage / 2 , knockback, Main.myPlayer);
            }
            //需注意的是这句话会直接往鼠标指针的方向发射一个，也就是总共8个射弹
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 3);
            recipe.AddIngredient<CryoBar>(10);
            recipe.AddIngredient(ItemID.FrostCore);
            recipe.AddIngredient(ItemID.FrostStaff);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
