using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Content.Items.Accessories.Defense;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Shield)]
    public class AsgardsValorold : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public const int ShieldSlamDamage = 200;
        public const float ShieldSlamKnockback = 9f;
        public const int ShieldSlamIFrames = 12;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 38;
            Item.height = 44;
            Item.rare = ItemRarityID.Lime;
            Item.value = CIShopValue.RarityPriceLime;
            Item.defense = 16;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.SetLAPDash(LAPContent.DashType<AsgardsValorDashold>());
            player.dashType = DashID.None;
            player.noKnockback = true;
            player.fireWalk = true;
            player.statLifeMax2 += 20;
            //上述两者共享的debuff免疫单独打表:
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true; //出于某些原因我没有看到阿斯加德本身免疫狱火
            player.buffImmune[BuffType<CIArmorCrunch>()] = true; // 更强的, 碎甲
            player.buffImmune[BuffType<CIBurningBlood>()] = true; // 同上
            player.buffImmune[BuffID.Venom] = true; // 更强的"剧毒"
            player.buffImmune[BuffType<CISulphuricPoisoning>()] = true;
            player.buffImmune[BuffID.Webbed] = true; // 更强的"缓慢"
            player.buffImmune[BuffID.Blackout] = true; // 更强的"黑暗"

            if (HasCalamity())
            {
                player.buffImmune[CalDeBuff.SulphuricPoisoning] = true;
                player.buffImmune[CalDeBuff.BrainRot] = true;
                player.buffImmune[CalDeBuff.BurningBlood] = true;
                player.buffImmune[CalDeBuff.ArmorCrunch] = true;
                player.buffImmune[CalDeBuff.HolyFlames] = true;
            }
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.endurance += 0.25f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AnkhShield).
                AddIngredient<ShieldoftheOceanLegacy>().
                AddIngredient<AbaddonLegacy>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
