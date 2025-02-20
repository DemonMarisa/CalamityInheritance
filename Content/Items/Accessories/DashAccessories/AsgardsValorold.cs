using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer.Dash;
using CalamityMod.Buffs.DamageOverTime;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class AsgardsValorold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.DashAccessories";
        public const int ShieldSlamDamage = 200;
        public const float ShieldSlamKnockback = 9f;
        public const int ShieldSlamIFrames = 12;

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 44;
            Item.rare = ItemRarityID.Lime;
            Item.value = CIShopValue.RarityPriceLime;
            Item.defense = 16;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            modPlayer1.CIDashID = AsgardsValorDashold.ID;
            player.Calamity().DashID = string.Empty;
            player.dashType = 0;
            player.noKnockback = true;
            player.fireWalk = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.WindPushed] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Daybreak] = true;
            player.buffImmune[ModContent.BuffType<HolyFlames>()] = true;
            player.statLifeMax2 += 20;

            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            { 
                player.endurance += 0.1f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AnkhShield).
                AddIngredient<OrnateShield>().
                AddIngredient<ShieldoftheOcean>().
                AddIngredient<CoreofCalamity>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
