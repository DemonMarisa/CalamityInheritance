using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    [AutoloadEquip(EquipType.Shield)]
    public class AsgardianAegisold : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public const int ShieldSlamDamage = 1000;
        public const float ShieldSlamKnockback = 15f;
        public const int ShieldSlamIFrames = 12;
        public const int RamExplosionDamage = 1000;
        public const float RamExplosionKnockback = 20f;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 60;
            Item.height = 54;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 32;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.SetLAPDash(LAPContent.DashType<AsgardianAegisDashold>());
            player.CI().ElysianAegis = true;

            player.dashType = DashID.None;

            player.CI().ElysianAegis = true;

            player.noKnockback = true;
            player.fireWalk = true;
            player.LAP().MaxLifeAdditive += 80;
            //上述两者共享的debuff免疫单独打表:
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true; //出于某些原因我没有看到阿斯加德本身免疫狱火
            player.buffImmune[BuffType<CIHolyFlames>()] = true;
            //阿斯加德庇佑本身免疫弑神怒火
            player.buffImmune[BuffType<CIGodSlayerInferno>()] = true;
            //所谓的. 更强的"Debuff"
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
                player.buffImmune[CalDeBuff.GodSlayerInferno] = true;
                player.buffImmune[CalDeBuff.HolyFlames] = true;
            }
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.AddDR(0.5f);
            }
        }
    }
}
