using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using CalamityInheritance.Core.Keys;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Shield)]
    public class ElysianAegisold : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public const int ShieldSlamDamage = 500;
        public const float ShieldSlamKnockback = 15f;
        public const int ShieldSlamIFrames = 12;

        public const int RamExplosionDamage = 500;
        public const float RamExplosionKnockback = 20f;

        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CIKeybinds.AegisHotKey);
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 48;
            Item.height = 42;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 18;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.SetLAPDash(LAPContent.DashType<ElysianAegisDashold>());

            player.dashType = DashID.None;
            player.CI().ElysianAegis = true;
            player.noKnockback = true;
            player.fireWalk = true;

            player.LAP().MaxLifeAdditive += 40;
            player.LAP().LifeRegen += 4;

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
        }
        public static void ElysianAegis_PostUpdate(CIPlayer player)
        {
            if (player.ElysianGuard)
            {
                if (player.Player.whoAmI == Main.myPlayer)
                    player.Player.AddBuff(BuffType<ElysianGuard>(), 2);
                player.Player.GetDamage<GenericDamageClass>() += 0.15f;
                player.Player.GetCritChance<GenericDamageClass>() += 15;
                player.Player.statDefense += 30;
                player.Player.moveSpeed *= 0.85f;
            }
            else
            {
                player.Player.ClearBuff(BuffType<ElysianGuard>());
            }
        }
    }
}
