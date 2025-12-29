using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class DragonStaff: CIMagic, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<PhoenixFlameBarrage>();
        }

        public override void SetDefaults()
        {
            Item.damage = 290;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 72;
            Item.height = 70;
            Item.useTime = 15;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<DragonStaffProj>();
            Item.shootSpeed = 30f;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? RarityType<YharonFire>() : RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float pSpeed = Item.shootSpeed;
            player.itemTime = Item.useTime;
            Vector2 src = player.RotatedRelativePoint(player.MountedCenter, true);
            float projX = Main.mouseX + Main.screenPosition.X - src.X;
            float projY = Main.mouseY + Main.screenPosition.Y - src.Y;
            if (player.gravDir == -1f)
            {
                projY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - src.Y;
            }
            float projDist = (float)Math.Sqrt((double)(projX * projX + projY * projY));
            if (float.IsNaN(projX) && float.IsNaN(projY) || projX == 0f && projY == 0f)
            {
                projX = player.direction;
                projY = 0f;
                projDist = pSpeed;
            }
            else
            {
                projDist = pSpeed / projDist;
            }

            int pCounts = 30;
            if (Main.rand.NextBool(3))
            {
                pCounts++;
            }
            if (Main.rand.NextBool(3))
            {
                pCounts++;
            }
            for (int i = 0; i < pCounts; i++)
            {
                //确认射弹位置
                src = new Vector2(player.position.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                src.X = (src.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                src.Y -= 100 * i;
                projX = Main.mouseX + Main.screenPosition.X - src.X;
                projY = Main.mouseY + Main.screenPosition.Y - src.Y;
                if (projY < 0f)
                {
                    projY *= -1f;
                }
                if (projY < 20f)
                {
                    projY = 20f;
                }
                projDist = (float)Math.Sqrt((double)(projX * projX + projY * projY));
                projDist = pSpeed / projDist;
                projX *= projDist;
                projY *= projDist;
                float pSpeedX = projX + Main.rand.Next(-30, 31) * 0.02f;
                float pSpeedY = projY + Main.rand.Next(-30, 31) * 0.02f;
                Projectile.NewProjectile(source, src.X, src.Y, pSpeedX, pSpeedY, type, damage, knockback, player.whoAmI, 0f, (float)Main.rand.Next(15));
            }
            return false;
        }
    }
}
