using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Ranged;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Ranged;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamityMod.Graphics.Metaballs;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Projectiles.ExoLore;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    /*星流传颂之物重做计划:
    右键: 射弹将完全穿墙， 
    */

    public class PhotovisceratorCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<Photoviscerator>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();

            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                damage.Base = 810;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Ranged.PhotovisceratorChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    public class PhotovisceratorProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<ExoFlareCluster>();
        //干掉原本的AI，我们重写一个
        public Color sparkColor;
        public int Time = 0;
        public override void AI(Projectile projectile)
        {
            //在AI内更新射弹非穿墙的属性
            Player player = Main.player[projectile.owner];
            var usPlayer = player.CIMod();
            Time++;
            if ((usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                projectile.tileCollide = false;
                //在行程路径上生成若干星流碎片
                //不好孩子们，星流碎片被这个b白面团挡完了
                if (Time % 10 == 0)
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.velocity * 0.1f, ModContent.ProjectileType<PhotovisceratorCrystal>(), projectile.damage / 2, projectile.knockBack, projectile.owner);
            }
            sparkColor = Main.rand.Next(4) switch
            {
                0 => Color.Red,
                1 => Color.MediumTurquoise,
                2 => Color.Orange,
                _ => Color.LawnGreen,
            };
            
            PhotoMetaball.SpawnParticle(projectile.Center, 90);
            PhotoMetaball2.SpawnParticle(projectile.Center, 85);
            CIFunction.HomeInOnNPC(projectile, true, 1200f, 12f, 20f);
        }
    }
}
