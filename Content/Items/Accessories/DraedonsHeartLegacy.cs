using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using MonoMod.ModInterop;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class DraedonsHeartLegacy: CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        //防损机制是人能做出来的东西吗?
        public static readonly float DefenseDamageResistance = 0.01f;
        //75%提防御对那个时候本来也没啥，但是搬到现在配上数值爆破级别的武器和收缩的更惨的boss伤害直接就炸了
        //反正我已经给常驻1%防损量了, ban掉免伤+降低这个值也没什么
        //public static readonly float DefenseMultipler = 0.75f; //提高75%防御值
        public static readonly float DefenseMultipler = 0.35f; //提高35%防御值
        //附：这个-50%的伤害对现在的武器来说根本不是事，
        //因为就嘉登心这个时期的武器就算-50%伤害也能打出比原灾更高的dps
        public static readonly float DamageReduceRatio = 0.5f; //50%伤害的降低
        public static readonly int LifeRegenSpeed = 16; //8HP/s生命恢复
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 26;
            Item.accessory = true;
            Item.defense = 48;
            Item.rare = ModContent.RarityType<PureRed>();
            Item.value = CIShopValue.RarityPricePureRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            var usPlayer = player.CIMod();
            calPlayer.defenseDamageRatio *= DefenseDamageResistance; //防御损伤的损失比直接乘以0.01f,即1%
            usPlayer.DraedonsHeartLegacyStats = true;
        }
    }
}