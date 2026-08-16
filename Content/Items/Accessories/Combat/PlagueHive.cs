using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Projectiles.Typeless.HomeIn;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class PlagueHive : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CIShopValue.RarityPriceCyan;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().PlagueHiveBee = true;
            player.CI().ToxicHeart = true;
            player.honeyCombItem = Item;
            player.strongBees = true;
            if (HasCalamity())
                player.buffImmune[CalDeBuff.Plague] = true;
            player.buffImmune[BuffType<CIPlague>()] = true;
        }
        public static void OnHitNPC_PlagueHive(Player player, NPC target, NPC.HitInfo hit)
        {
            target.AddBuff(BuffType<CIPlague>(), 360);
            if (player.HasProjCount<PlagueBeeLegacy>() < 1 && !player.CI().HasCount("PlagueHiveBeeCoolDown"))
            {
                int Type = ProjectileType<PlagueBeeLegacy>();
                for (int i = 0; i < 6; i++)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis(), target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height) * 1.5f, Main.rand.NextVector2Circular(0.25f, 0.25f), Type, 45, 1, player.whoAmI);
                }
                player.CI().AddCount("PlagueHiveBeeCoolDown", 10);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ToxicHeartLegacy>().
                AddIngredient(ItemID.HiveBackpack).
                AddIngredient(ItemID.HoneyComb).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
