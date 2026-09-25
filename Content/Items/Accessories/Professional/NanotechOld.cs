using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class NanotechOld : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 15;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().NanoTech = true;
            player.GetDamage<ThrowingDamageClass>() += 0.5f;
            player.GetDamage<ThrowingDamageClass>() *= 1.1f;
        }
        public static void ProjSpilt(Projectile proj)
        {
            if (!proj.DamageType.CountsAsClass<ThrowingDamageClass>())
                return;
            if (Main.player[proj.owner].miscCounter % 30 == 0 && proj.FinalExtraUpdate())
            {
                if (proj.owner == Main.myPlayer)
                {
                    int p = Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, Vector2.Zero, ProjectileType<NanotechOldProj>(), (int)(proj.damage * 0.05) + 1, 0f, proj.owner);
                    //确保这个东西指定为全局伤害
                    Main.projectile[p].DamageType = DamageClass.Generic;
                    Main.projectile[p].alpha = 255;
                }
            }
        }
        public static void ModifyHitNPC(Projectile projectile, ref NPC.HitModifiers modifiers, NPC target)
        {
            if (projectile.CI().Stealth)
            {
                if (!projectile.Owner().CI().HasCount("NanoTechHealCD"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 source = new Vector2(target.Center.X + Main.rand.Next(-201, 201), Main.screenPosition.Y - 600f - Main.rand.Next(50));
                        Vector2 velocity = (target.Center - source) / 40f;

                        Projectile.NewProjectile(projectile.GetSource_FromThis(), source, velocity, ProjectileType<NanoFlareLegacy>(), (int)(projectile.damage * 0.05), 3f, projectile.owner);
                    }
                    projectile.Owner().CI().Player.NCHeal(10);
                    projectile.Owner().CI().AddCount("NanoTechHealCD", 90);
                }
                projectile.ArmorPenetration += 100;
                modifiers.SourceDamage *= 1.05f;
            }
        }
    }
}
