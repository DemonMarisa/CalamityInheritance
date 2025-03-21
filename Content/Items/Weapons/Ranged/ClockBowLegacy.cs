using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using rail;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ClockBowLegacy: CIRanged, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 100;
            Item.useTime = Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = CISoundID.SoundBow;
            Item.autoReuse = true; 
            Item.shootSpeed = 30f;
            Item.useAmmo = 40;
            Item.Calamity().canFirePointBlankShots = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Cog, 50).
                AddIngredient(ItemID.LunarBar, 15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float speed = Item.shootSpeed;
            player.itemTime = Item.useTime;

            int pCounts = 15;
            for (int i = 3; i < 6; i++)
            {
                if (Main.rand.NextBool(i))
                    pCounts++;
            }
            for (int j = 0; j < pCounts; j++)
            {
                float newX = player.position.X + (float)player.width / 2 + Main.rand.NextFloat(201f) * - (float)player.direction + Main.mouseX + Main.screenPosition.X - player.position.X;
                Vector2 bowRot = new(newX, player.MountedCenter.Y);
                bowRot.X = (bowRot.X + player.Center.X) /2f + Main.rand.NextFloat(-200f,201f);
                bowRot.Y -= 100 * j;
                float ammoVeloX = Main.mouseX + Main.screenPosition.X - bowRot.X;
                float ammoVeloY = Main.mouseY + Main.screenPosition.Y - bowRot.Y;
                if (ammoVeloX < 0f)   
                    ammoVeloX *= -1f;
                if (ammoVeloX < 20f)
                    ammoVeloX = 20f;
                float getDist = CIFunction.TryGetVectorMud(ammoVeloX, ammoVeloY);
                getDist = speed/getDist;
                ammoVeloX *= getDist;
                ammoVeloY *= getDist;
                float realProjSpeedX = ammoVeloX + Main.rand.NextFloat(-600, 601) * 0.02f;
                float realProjSpeedY = ammoVeloY + Main.rand.NextFloat(-600, 601) * 0.02f;
                int p = Projectile.NewProjectile(source, bowRot.X, bowRot.Y, realProjSpeedX, realProjSpeedY, type, damage, knockback, Main.myPlayer);
                Main.projectile[p].tileCollide = false;
                Main.projectile[p].timeLeft = 240;
                Main.projectile[p].noDropItem = true;
            }
            return false;
        }
    }
}