using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Projectiles.Legendary;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class VesuviusLegendary : LegendaryWeapon
    {
        public override Color DrawColor => Color.HotPink;
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }
        public override int BaseDamage => 60;
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Magic;
            Item.width = 62;
            Item.height = 62;
            Item.damage = BaseDamage;
            Item.mana = 9;
            Item.useAnimation = Item.useTime = 11;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item88;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileType<VesuviusLegendProj>();
        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
                mult *= 1.5f;
        }
        public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse != 2)
                return 1.2f;
            return 1.50f;
        }
        public override void HoldItem(Player player)
        {
            if(TierThreeMatcher)
            {
                player.lifeRegen += 4;
                player.GetDamage<GenericDamageClass>() += .20f;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //设置后坐力手持射弹。
            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<CIGeneralRecoilProj>(), 0, 0, player.whoAmI);
            if (proj.ModProjectile is CIGeneralRecoilProj holdout)
            {
                holdout.SetUpHoldoutData(Type, 0f, Item.useAnimation, Vector2.Zero);
            }
            float pSpriteType = Main.rand.Next(6);
            if (player.altFunctionUse == 2)
            {
                int meteorAmt = Main.rand.Next(4, 6);
                //T2样式加强：左键与右键多2颗陨石
                if (TierTwoMatcher)
                    meteorAmt += 3;
                for (int i = 0; i < meteorAmt; ++i)
                {
                    float SpeedX = velocity.X + Main.rand.Next(-30, 31) * 0.05f;
                    float SpeedY = velocity.Y + Main.rand.Next(-30, 31) * 0.05f;
                    float ai0 = Main.rand.Next(6);
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, pSpriteType, 0.5f + (float)Main.rand.NextDouble() * 0.9f);
                }
                return false;
            }
            else
            //下面这段史不太好改看情况吧
            {
                float meteorSpeed = Item.shootSpeed;
                Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
                float mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                float mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (player.gravDir == -1f)
                {
                    mouseYDist = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - realPlayerPos.Y;
                }
                float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                if (float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist) || mouseXDist == 0f && mouseYDist == 0f)
                {
                    mouseXDist = player.direction;
                    mouseYDist = 0f;
                    mouseDistance = meteorSpeed;
                }
                else
                {
                    mouseDistance = meteorSpeed / mouseDistance;
                }
                int pAmt = 4;
                for (int i = 0; i < pAmt; i++)
                {
                    realPlayerPos = new Vector2(player.position.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 800f);
                    realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                    realPlayerPos.Y += 100 * i;
                    mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X + Main.rand.Next(-40, 41) * 0.03f;
                    mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                    if (mouseYDist < 0f)
                    {
                        mouseYDist *= -1f;
                    }
                    if (mouseYDist < 20f)
                    {
                        mouseYDist = 20f;
                    }
                    mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                    mouseDistance = meteorSpeed / mouseDistance;
                    mouseXDist *= mouseDistance;
                    mouseYDist *= mouseDistance;
                    float meteorSpawnXOffset = mouseXDist;
                    float meteorSpawnYOffset = mouseYDist + Main.rand.Next(-40, 41) * 0.02f;
                    float ai0 = Main.rand.Next(6);
                    Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, meteorSpawnXOffset * 0.75f, meteorSpawnYOffset * 0.75f, type, damage, knockback, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f); //0.3

                }
                //从地底下发射额外的射弹
                int j = 0;
                if (TierTwoMatcher)
                {
                    for (; j < 2; j++)
                    {
                        float pPosX = Main.MouseWorld.X + Main.rand.NextFloat(-200f, 201f);
                        float pPosY = Main.MouseWorld.Y + Main.rand.NextFloat(670f, 1080f);
                        Vector2 newPos = new(pPosX, pPosY);
                        //速度
                        Vector2 spd = Main.MouseWorld - newPos;
                        //水平速度随机度
                        spd.X += Main.rand.NextFloat(-15f, 16f);
                        float pSpeed = 24f;
                        float tarDist = spd.Length();
                        //?
                        tarDist = pSpeed / tarDist;
                        spd.X *= tarDist;
                        spd.Y *= tarDist;
                        float ai0 = Main.rand.Next(6);
                        Projectile.NewProjectile(source, newPos, spd, type, damage, knockback, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f);
                    }
                }
                return false;
            }
        }

    }
}
