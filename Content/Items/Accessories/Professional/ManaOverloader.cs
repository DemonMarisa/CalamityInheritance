using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Heals;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class ManaOverloader : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance<MagicDamageClass>() += 15;
            player.LAP().MaxManaAdditive += 75;
            player.CI().ManaOverloaderHeal = true;
            if (player.statMana > player.statManaMax2)
                player.LAP().LifeRegen -= 6;
        }
        public static void HitNPC(NPC target, Projectile proj, int damage, Player player)
        {
            if (player.CI().HasCount("ManaOverLoaderCD"))
                return;
            player.CI().AddCount("ManaOverLoaderCD", 15);
            double healMult = 0.2;
            healMult -= proj.numHits * healMult * 0.5;
            int heal = (int)Math.Round(damage * healMult * (player.statMana / (double)player.statManaMax2));
            if (heal > 75)
                heal = 75;
            if (healMult > 0D && heal > 0 && player.statMana <= player.statManaMax2)
                player.SpawnHealProj(player.GetSource_FromThis(), ProjectileType<ManaPolarizerHeal>(),target.Center, Vector2.Zero, heal);
        }
    }
}