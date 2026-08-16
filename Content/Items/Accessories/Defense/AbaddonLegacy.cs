using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Typeless.Explosions;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    [AutoloadEquip(EquipType.Face)]
    public class AbaddonLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.defense = 6;
            Item.accessory = true;
            Item.width = 32;
            Item.height = 48;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().Abaddon = true;
            player.GetCritChance<GenericDamageClass>() += 10;
            player.buffImmune[BuffID.OnFire] = true;
            if (CIUtils.HasCalamity())
                player.buffImmune[CalDeBuff.BrimstoneFlames] = true;
        }
        public static void OnHitNPCWithProj_Abaddon(CIPlayer player, Projectile proj, NPC target, NPC.HitInfo hit)
        {
            if (hit.Crit && !player.HasCount("AbaddonCooldown"))
            {
                int AbaddonExploDamage = CIUtils.DamageSoftCap(hit.SourceDamage * 0.1f, 35);
                Projectile.NewProjectile(player.Player.GetSource_FromThis(), proj.Center, Vector2.Zero, ProjectileType<AbaddonCrit>(), AbaddonExploDamage, 0f, player.Player.whoAmI);
                player.AddCount("AbaddonCooldown", 15);
            }
        }
    }
}
