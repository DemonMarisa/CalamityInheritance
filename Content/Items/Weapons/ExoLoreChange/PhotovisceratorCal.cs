using CalamityInheritance.Content.Projectiles.CalProjChange;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.HeldProj.CalChange.Range;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Graphics.Metaballs;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    /*星流传颂之物重做计划:
    右键: 射弹将完全穿墙， 
    */
    public class PhotovisceratorCalHook
    {
        public static void Load()
        {
            MethodInfo originalMethod = typeof(Photoviscerator).GetMethod(nameof(Photoviscerator.HoldItem));
            MonoModHooks.Add(originalMethod, HoldItem_Hook);

            MethodInfo originalMethod2 = typeof(Photoviscerator).GetMethod(nameof(Photoviscerator.Shoot));
            MonoModHooks.Add(originalMethod2, Shoot_Hook);
        }
        public static void HoldItem_Hook(Photoviscerator self, Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return;
        }
        public static bool Shoot_Hook(Photoviscerator self, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ProjectileType<PhotovisceratorHoldout>()] < 1)
                Projectile.NewProjectile(source, position, Vector2.Zero, type, 0, 0f, player.whoAmI);

            if (Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile holdout = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ProjectileType<PhotovisceratorWingman>(), damage, knockback, player.whoAmI, 0, 0, i == 0 ? 1 : -1);
                    holdout.velocity = (player.Calamity().mouseWorld - player.MountedCenter).SafeNormalize(Vector2.Zero);
                }
            }
            return false;
        }
    }
    public class PhotovisceratorCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ItemType<Photoviscerator>();
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.WeaponTextPath}Ranged.PhotovisceratorChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    public class PhotovisceratorProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileType<ExoFlareCluster>();
        //干掉原本的AI，我们重写一个
        public Color sparkColor;
        public int Time = 0;
        public override void AI(Projectile projectile)
        {
            //在AI内更新射弹非穿墙的属性
            Player player = Main.player[projectile.owner];
            Time++;
            if (player.CheckExoLore() && projectile.owner == Main.myPlayer)
            {
                projectile.tileCollide = false;
                //在行程路径上生成若干星流碎片
                //不好孩子们，星流碎片被这个b白面团挡完了
                if (Time % 10 == 0)
                    Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity * 0.1f, ProjectileType<PhotovisceratorCrystal>(), projectile.damage / 10, projectile.knockBack, projectile.owner);
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
