using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.CIPlayer.Dash;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class AsgardianAegisold : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.DashAccessories";
        public const int ShieldSlamDamage = 1000;
        public const float ShieldSlamKnockback = 15f;
        public const int ShieldSlamIFrames = 12;
        public const int RamExplosionDamage = 1000;
        public const float RamExplosionKnockback = 20f;
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:60,
            itemHeight:54,
            itemRare:ModContent.RarityType<DeepBlue>(),
            itemValue:CIShopValue.RarityPriceDeepBlue,
            itemDefense:32
        );
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.CIMod().AegisOn;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.AegisLegacyOn = true;
            usPlayer.CIDashID = AsgardianAegisDashold.ID;
            usPlayer.ElysianAegis = true;
            player.Calamity().DashID = string.Empty;
            player.dashType = 0;
            player.noKnockback = true;
            player.fireWalk = true;
            //启用共享且不重复的debuff免疫
            usPlayer.AsgardsValorImmnue = true;
            usPlayer.ElysianAegisImmnue = true;
            player.statLifeMax2 += 80;
            //上述两者共享的debuff免疫单独打表:
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true; //出于某些原因我没有看到阿斯加德本身免疫狱火
            player.buffImmune[ModContent.BuffType<HolyFlames>()] = true;
            //阿斯加德庇佑本身免疫弑神怒火
            player.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = true;

            //所谓的. 更强的"Debuff"
            player.buffImmune[ModContent.BuffType<ArmorCrunch>()] = true; // 更强的, 碎甲
            player.buffImmune[ModContent.BuffType<BrainRot>()] = true; // 更强的"流血"
            player.buffImmune[ModContent.BuffType<BurningBlood>()] = true; // 同上
            player.buffImmune[BuffID.Venom] = true; // 更强的"剧毒"
            player.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = true; // 更强的"剧毒"
            player.buffImmune[BuffID.Webbed] = true; // 更强的"缓慢"
            player.buffImmune[BuffID.Blackout] = true; // 更强的"黑暗"

            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.endurance += 0.5f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElysianAegisold>().
                AddIngredient<AsgardsValorold>().
                AddIngredient<CosmiliteBar>(10).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
