using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Rogue;
using System.Collections.Generic;
using Terraria.Localization;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using CalamityMod;
using LAP.Core.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class SupernovaCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<Supernova>();
        public override bool CanUseItem(Item item, Player player)
        {
            int proj = ModContent.ProjectileType<SupernovaBomb>();
            if (Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo)
            {
                item.shoot = proj;
                item.useAnimation = 50;
                item.useTime = 50;
            }
            else
            {
                item.shoot = proj;
                item.useAnimation = 70;
                item.useTime = 70;
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.WeaponTextPath}Rogue.SupernovaChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    public class SupernovaHugeBoom : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<SupernovaStealthBoom>();
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                modifiers.SourceDamage *= 1.7f;
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            //被击杀的时候会朝玩家方向发射一个额外的超新星。
            if ((player.CIMod().LoreExo || player.CIMod().PanelsLoreExo) && projectile.owner == Main.myPlayer && player.ActiveItem().type == ModContent.ItemType<Supernova>())
            {
                //距离向量
                Vector2 distVec = player.Center - projectile.Center;
                //距离模
                float dist = distVec.Length();
                //速度
                float speed = 24f;
                //距离向量转速度向量k
                dist = speed / dist;
                distVec.X *= dist;
                distVec.Y *= dist;
                //追加一个超新星
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, distVec, ModContent.ProjectileType<SupernovaBomb>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }
    }
}
