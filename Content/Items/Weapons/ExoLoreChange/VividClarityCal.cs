using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using CalamityMod;
using System;
using CalamityMod.Projectiles.Rogue;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityMod.Projectiles.Melee;
using CalamityMod.CalPlayer;
using System.Reflection;
using CalamityMod.Buffs.DamageOverTime;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class VividClarityCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<VividClarity>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                damage.Base = 265;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Magic.VividChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    public class VCBeam : GlobalProjectile
    {
        public static void Load(Mod mod)
        {
            MethodInfo originalMethod = typeof(VividBeam).GetMethod(nameof(VividBeam.OnHitNPC));
            MonoModHooks.Add(originalMethod, OnHitNPC_Hook);
        }

        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<VividBeam>();
        public static void OnHitNPC_Hook(VividBeam self, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player p = Main.player[self.Projectile.owner];
            var mp = p.CIMod();
            if (self.Projectile.owner == Main.myPlayer && !(mp.LoreExo || mp.PanelsLoreExo))
            {
                SummonLaser();
            }
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);

            void SummonLaser()
            {
                var source = self.Projectile.GetSource_FromThis();
                switch (self.Projectile.ai[1])
                {
                    case 0f:
                        CalamityUtils.ProjectileRain(source, self.Projectile.Center, 320f, 100f, 400f, 640f, 6f, ModContent.ProjectileType<VividClarityBeam>(), self.Projectile.damage, self.Projectile.knockBack, self.Projectile.owner);
                        break;

                    case 1f:
                        Projectile.NewProjectile(source, self.Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VividExplosion>(), self.Projectile.damage * 2, self.Projectile.knockBack, self.Projectile.owner);
                        break;

                    case 2f:
                        float spread = 30f * 0.01f * MathHelper.PiOver2;
                        double startAngle = Math.Atan2(self.Projectile.velocity.X, self.Projectile.velocity.Y) - spread / 2;
                        double deltaAngle = spread / 8f;
                        double offsetAngle;
                        for (int i = 0; i < 4; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(source, self.Projectile.Center.X, self.Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<VividLaser2>(), self.Projectile.damage, self.Projectile.knockBack, self.Projectile.owner);
                            Projectile.NewProjectile(source, self.Projectile.Center.X, self.Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<VividLaser2>(), self.Projectile.damage, self.Projectile.knockBack, self.Projectile.owner);
                        }
                        break;
                }
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player p = Main.player[projectile.owner];
            var mp = p.CIMod();
            if (projectile.owner == Main.myPlayer && (mp.LoreExo || mp.PanelsLoreExo))
            {
                // 射弹追踪
                SummonLaserBetter(ref projectile);
                //在击中敌人或者墙体时从场外追加射弹支援
                ExoBladeSupport(ref projectile, ref p);
            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Player p = Main.player[projectile.owner];
            if (projectile.owner == Main.myPlayer && (p.CIMod().LoreExo || p.CIMod().PanelsLoreExo))
            {
                // 射弹追踪
                SummonLaserBetter(ref projectile);
                //在击中敌人或者墙体时从场外追加射弹支援
                ExoBladeSupport(ref projectile, ref p);
            }
            var mp = p.CIMod();
            if (mp.LoreExo || mp.PanelsLoreExo)
                return false;
            else
                return true;
        }
        // Radius of the "circle of inaccuracy" surrounding the mouse. Blue bullets will aim at this circle.
        private const float MouseAimDeviation = 26f;
        public static void ExoBladeSupport(ref Projectile projectile, ref Player player)
        {
            /*
            //位置修改：在鼠标的上/下方进行尝试
            float xPos = projectile.Center.X + Main.rand.Next(-200, 201);
            float yPos = projectile.Center.Y - 600f;
            if (Main.rand.NextBool())
                yPos = projectile.Center.Y + 600f;
            Vector2 startPos = new(xPos, yPos);
            //射弹朝向玩家发射
            Vector2 velocity = player.Center - startPos;

            float dir = 13f / velocity.Length();
            velocity.X *= dir;
            velocity.Y *= dir;
            //一次生成三个射弹
            for (int i = 0; i < 3; i++)
            {
                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), startPos, velocity, ModContent.ProjectileType<Exobeam>(), projectile.damage, projectile.knockBack);
                Main.projectile[p].DamageType = DamageClass.Magic;
            }
            */
            // The warp must be performed client side because it requires knowledge of the player's mouse position.
            projectile.netUpdate = true;

            // 第一步，随机放置在玩家周围360度，并朝向玩家
            Vector2 playerToMouseVec = CalamityUtils.SafeDirectionTo(Main.LocalPlayer, Main.LocalPlayer.MountedCenter, -Vector2.UnitY);
            float warpDist = Main.rand.NextFloat(800f, 1000f);
            float warpAngle = Main.rand.NextFloat(0f , MathHelper.TwoPi);
            Vector2 warpOffset = -warpDist * playerToMouseVec.RotatedBy(warpAngle);
            projectile.position = Main.LocalPlayer.MountedCenter - warpOffset + CIFunction.RandomVelocity(100, 100);

            // 第二步，让他指向玩家
            // 故意添加了偏移
            Vector2 mouseTargetVec = Main.LocalPlayer.MountedCenter + Main.rand.NextVector2Circular(MouseAimDeviation, MouseAimDeviation);
            Vector2 bulletToMouseVec = CalamityUtils.SafeDirectionTo(projectile, mouseTargetVec, -Vector2.UnitY);
            projectile.velocity = bulletToMouseVec * 24f;

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            //一次生成三个射弹
            for (int i = 0; i < 3; i++)
            {
                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.velocity, ModContent.ProjectileType<Exobeam>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 4f);
                Main.projectile[p].DamageType = DamageClass.Magic;
            }

        }
        public static void SummonLaserBetter(ref Projectile projectile)
        {
            var source = projectile.GetSource_FromThis();
            switch (projectile.ai[1])
            {
                case 0f:
                    CalamityUtils.ProjectileRain(source, projectile.Center, 320f, 100f, 400f, 640f, 6f, ModContent.ProjectileType<VividClarityBeam>(), (int)(projectile.damage * 1.1f), projectile.knockBack, projectile.owner);
                    break;

                case 1f:
                    int pType = Main.player[projectile.owner].CIMod().LoreExo || Main.player[projectile.owner].CIMod().PanelsLoreExo ? ModContent.ProjectileType<SupernovaBoomOld>() : ModContent.ProjectileType<VividExplosion>();
                    int t = Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, pType, projectile.damage * 2, projectile.knockBack, projectile.owner);
                    //一定要把这个标记为魔法伤害
                    Main.projectile[t].DamageType = DamageClass.Magic;
                    Main.projectile[t].scale *= 0.8f;
                    break;

                case 2f:
                    float spread = 30f * 0.01f * MathHelper.PiOver2;
                    double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    int setDamage = (int)(projectile.damage * 1.05f);
                    for (int i = 0; i < 4; i++)
                    {
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                        Projectile.NewProjectile(source, projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<VividLaser2>(), setDamage, projectile.knockBack, projectile.owner);
                        Projectile.NewProjectile(source, projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<VividLaser2>(), setDamage, projectile.knockBack, projectile.owner);
                    }
                    break;
            }
        }
    }
}
