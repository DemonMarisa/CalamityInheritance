using System;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class DragonBow: ModProjectile
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<DrataliornusLegacy>();
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Ranged/DrataliornusLegacy";

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 84;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }

        public override void AI() //mostly phangasm code
        {
            Lighting.AddLight(Projectile.Center, 255f / 255f, 154f / 255f, 58f / 255f);
            Player player = Main.player[Projectile.owner];

            Projectile.ai[1]--; //usetime timer

            if (Projectile.ai[0] >= 0) //spinup timer
            {
                Projectile.ai[0]++;

                switch ((int)Projectile.ai[0])
                {
                    case 20:
                    case 35:
                    case 50:
                    case 75:
                    case 100:
                    case 150:
                    case 200:
                    case 250:
                    case 270:
                        Projectile.localAI[0]++;
                        break;

                    case 300:
                        Projectile.localAI[0]++;
                        Projectile.ai[0] = -1f; //fully spun up, don't need timer anymore

                        const int constant = 36; //dusts indicate fully spun up
                        for (int i = 0; i < constant; i++)
                        {
                            Vector2 rotate = Vector2.Normalize(Projectile.velocity) * 9f;
                            rotate = rotate.RotatedBy((i - (constant / 2 - 1)) * 6.28318548f / constant, default) + player.Center;
                            Vector2 faceDirection = rotate - player.Center;
                            int dust = Dust.NewDust(rotate + faceDirection, 0, 0, DustID.Flare, 0f, 0f, 0, default, 4f);
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].velocity = faceDirection;
                        }
                        break;
                }
            }

            int baseUseTime = 44;
            int modifier = 4;
            bool timeToFire = false;
            if (Projectile.ai[1] <= 0f)
            {
                Projectile.ai[1] = baseUseTime - modifier * Projectile.localAI[0];
                timeToFire = true;
            }

            bool canFire = !CalamityUtils.CantUseHoldout(player) && player.HasAmmo(player.ActiveItem());

            if (Projectile.soundDelay <= 0 && canFire)
            {
                Projectile.soundDelay = baseUseTime - modifier * (int)Projectile.localAI[0];
                if (Projectile.ai[0] != 1f)
                    SoundEngine.PlaySound(SoundID.Item5, Projectile.position);
            }

            if (timeToFire && Main.myPlayer == Projectile.owner)
            {
                if (canFire) //fire an angery flame
                {
                    int type = ProjectileID.WoodenArrowFriendly; //Gets changed below anyways
                    float scaleFactor = 18f;
                    int damage = player.GetWeaponDamage(player.ActiveItem());
                    float knockBack = player.ActiveItem().knockBack;

                    player.PickAmmo(player.ActiveItem(), out type, out scaleFactor, out damage, out knockBack, out _);

                    type = ModContent.ProjectileType<DragonBowFlameRework>();
                    knockBack = player.GetWeaponKnockback(player.ActiveItem(), knockBack);

                    Vector2 playerPosition = player.RotatedRelativePoint(player.MountedCenter, true);
                    Projectile.velocity = Main.screenPosition - playerPosition;
                    Projectile.velocity.X += Main.mouseX;
                    Projectile.velocity.Y += Main.mouseY;
                    if (player.gravDir == -1f)
                        Projectile.velocity.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - playerPosition.Y;

                    Projectile.velocity.Normalize();

                    float variation = (1f + Projectile.localAI[0]) * 3f; //variation increases as fire rate increases
                    Vector2 position = playerPosition + Utils.RandomVector2(Main.rand, -variation, variation);
                    Vector2 speed = Projectile.velocity * scaleFactor * (0.6f + Main.rand.NextFloat() * 0.6f);
                    var usPlayer = player.CalamityInheritance();
                    float ai0;
                    //数值调整:
                    //全部弹幕的速度下调, 将伤害细分成多个小模块
                    if (Projectile.ai[0] < 0f)
                    {

                        if (Main.rand.NextBool(2))//当允许追踪时
                        {
                            ai0 = 2f;
                            if(Main.rand.NextBool(3))
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed, type, (int)(damage * 0.75f), knockBack, Projectile.owner, ai0, 0f);
                            if(usPlayer.ElementalQuiver) //装备元素箭袋时必定多发送一个追踪弹幕
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed * 1.5f, type, (int)(damage * 0.75f), knockBack, Projectile.owner, ai0, 0f);
                            damage *= (int)1.2f; 
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed, type, damage, knockBack, Projectile.owner, ai0, 0f);
                        }
                        else
                        {
                            ai0 = 1f;
                            if(Main.rand.NextBool(4))
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed*2.0f, type, (int)(damage * 0.65f), knockBack, Projectile.owner, ai0, 0f);
                            if(usPlayer.ElementalQuiver) //装备元素箭袋时必定多发送一个非追踪弹幕
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed * 1.5f, type, (int)(damage * 0.75f), knockBack, Projectile.owner, ai0, 0f);
                            damage *= (int)1.30f; 
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed*1.2f, type, damage, knockBack, Projectile.owner, ai0, 0f);
                        }
                    }
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, speed, type, damage, knockBack, Projectile.owner, 0f, 0f);
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            //display projectile
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 displayOffset = new Vector2(32, 0).RotatedBy(Projectile.rotation);
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, true) + displayOffset;
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);
        }

        public override bool? CanDamage() => false;

        public override void OnKill(int timeLeft)
        {
            const int constant = 24; //玩家取消左键/承伤时生成粒子
            for (int i = 0; i < constant; i++)
            {
                Vector2 rotate = Vector2.Normalize(Projectile.velocity) * 2f;
                rotate = rotate.RotatedBy((i - (constant / 2 - 1)) * 6.28318548f / constant, default) + Projectile.Center;
                Vector2 faceDirection = rotate - Projectile.Center;
                int dust = Dust.NewDust(rotate + faceDirection, 0, 0, DustID.Flare, 0f, 0f, 0, default, 4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = faceDirection;
            }
        }
    }
}
