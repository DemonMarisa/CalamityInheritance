﻿using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    internal class ConclaveCrossfire : CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmetTo<ConferenceCall>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 26;
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;
            Item.shootSpeed = 13f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 50)
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int bulletAmt = Main.rand.Next(4, 6); ;
            for (int index = 0; index < bulletAmt; ++index)
            {
                velocity.X += Main.rand.Next(-30, 31) * 0.05f;
                velocity.Y += Main.rand.Next(-30, 31) * 0.05f;
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[proj].extraUpdates += 2;
                Main.projectile[proj].ArmorPenetration += 15;
            }

            int maxTargets = 7;
            int[] targets = new int[maxTargets];
            int targetArrayIndex = 0;
            Rectangle rectangle = new Rectangle((int)player.Center.X - 960, (int)player.Center.Y - 540, 1920, 1080);
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.chaseable && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.friendly && !npc.immortal)
                {
                    if (npc.Hitbox.Intersects(rectangle))
                    {
                        if (targetArrayIndex < maxTargets)
                        {
                            targets[targetArrayIndex] = npc.whoAmI;
                            targetArrayIndex++;
                        }
                        else
                            break;
                    }
                }
            }

            if (targetArrayIndex == 0)
                return false;

            Vector2 targetPosition;
            int extraBulletDamage = (int)(damage * 0.8);

            for (int j = 0; j < targetArrayIndex; j++)
            {
                targetPosition = new Vector2(player.position.X + player.width * 0.5f + (Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                targetPosition.X = (targetPosition.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                targetPosition.Y -= 100 * j;

                Vector2 extraBulletVel = Vector2.Normalize(Main.npc[targets[j]].Center - targetPosition) * Item.shootSpeed;

                int proj = Projectile.NewProjectile(source, targetPosition, extraBulletVel, type, extraBulletDamage, knockback, player.whoAmI);
                Main.projectile[proj].extraUpdates += 14;
                Main.projectile[proj].tileCollide = false;
                Main.projectile[proj].timeLeft /= 2;
            }

            if (targetArrayIndex == maxTargets)
                return false;

            // Fire bullets at the same targets if 12 unique targets aren't found
            for (int k = 0; k < maxTargets - targetArrayIndex; k++)
            {
                int randomTarget = Main.rand.Next(targetArrayIndex);

                targetPosition = new Vector2(player.position.X + player.width * 0.5f + (Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                targetPosition.X = (targetPosition.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                targetPosition.Y -= 100 * randomTarget;

                Vector2 extraBulletVel = Vector2.Normalize(Main.npc[targets[randomTarget]].Center - targetPosition) * Item.shootSpeed;

                int proj = Projectile.NewProjectile(source, targetPosition, extraBulletVel, type, extraBulletDamage, knockback, player.whoAmI);
                Main.projectile[proj].extraUpdates += 14;
                Main.projectile[proj].tileCollide = false;
                Main.projectile[proj].timeLeft /= 2;
            }

            return false;
        }
    }
}
