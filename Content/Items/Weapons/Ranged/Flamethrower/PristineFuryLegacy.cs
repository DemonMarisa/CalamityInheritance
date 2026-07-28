using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Ranged.Flamethrower;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Flamethrower
{
    public class PristineFuryLegacy : CIRanged
    {

        public int frameCounter = 0;
        public int frame = 0;
        public static int BaseDamage = 77;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 100;
            Item.height = 46;
            Item.useTime = 3;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundFlamethrower;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PristineFireLegacy>();
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Gel;

            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-25, -10);

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 5;
                Item.useAnimation = 20;
            }
            else
            {
                Item.useTime = 3;
                Item.useAnimation = 15;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                int flameAmt = 3;
                damage = (int)(damage * 1.2);
                for (int index = 0; index < flameAmt; ++index)
                {
                    float SpeedX = velocity.X + Main.rand.NextFloat(-1.25f, 1.25f);
                    float SpeedY = velocity.Y + Main.rand.NextFloat(-1.25f, 1.25f);
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileType<PristineLegacySecondary>(), (int)(damage * 0.8f), knockback, player.whoAmI);
                }
            }
            else
            {
                damage = (int)(damage * 0.94);
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<PristineFireLegacy>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameI, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.GetItemDrawFrame(Type, out Texture2D sourece, out Rectangle rec);
            Texture2D texture = Request<Texture2D>($"{Texture}_Animated").Value;
            spriteBatch.Draw(texture, position, rec, Color.White, 0f, origin, scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Type, out Texture2D sourece, out Rectangle rec);
            Texture2D texture = Request<Texture2D>($"{Texture}_Animated").Value;
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, rec, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            return false;
        }
        public override void AddRecipes()
        {
        }
    }
}
