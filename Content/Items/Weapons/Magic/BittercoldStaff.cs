using System;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class BittercoldStaff: CIMagic, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Magic";

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 54;
            Item.damage = 64;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 14;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
            Item.UseSound = SoundID.Item46;
            Item.shoot = ModContent.ProjectileType<IceRain>();
            Item.shootSpeed = 14f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float projSpeed = Item.shootSpeed;
            Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float getPosX = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
            float getPosY = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
            if(player.gravDir == -1f)
            getPosY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - realPlayerPos.Y;
            float getDist = (float)Math.Sqrt((double)(getPosX * getPosX+getPosY * getPosY));
            if((float.IsNaN(getPosX) && float.IsNaN(getPosY)) || (getPosX == 0f && getPosY == 0f))
            {
                getPosX = player.direction;
                getPosY = 0f;
                getDist = projSpeed;
            }
            else getDist = projSpeed/getDist;
            getPosX *=getDist;
            getPosY *=getDist;
            int projCounts = 2;
            for (int i = 2; i < 17; i *= 2)
                //1/2, 1/4, 1/8, 1/16
                projCounts += Main.rand.NextBool(i) ? 2 : 0;

            for(int i = 0; i < projCounts; i++)
            {
                float projSpwanX = getPosX;
                float projSpwanY = getPosY;
                float angle = 0.05f * i;
                projSpwanX += Main.rand.NextFloat(-100f, 100f) * angle;
                projSpwanY += Main.rand.NextFloat(-100f, 100f) * angle;
                getDist = (float)Math.Sqrt((double)(projSpwanX*projSpwanX+ projSpwanY*projSpwanY));
                getDist = projSpeed/getDist;
                projSpwanX*= getDist;
                projSpwanY*= getDist;
                float x2 = realPlayerPos.X;
                float y2 = realPlayerPos.Y;
                Projectile.NewProjectile(player.GetSource_FromThis(), new Vector2(x2,y2), new Vector2(projSpwanX, projSpwanY), type, damage + damage/projCounts, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
        }
    }
}