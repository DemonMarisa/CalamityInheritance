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
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 54;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 28;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();

            // Asgardian Aegis ram dash
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.CIDashID = AsgardianAegisDashold.ID;
            usPlayer.ElysianAegis = true;
            player.Calamity().DashID = string.Empty;
            player.dashType = 0;
            player.noKnockback = true;
            player.fireWalk = true;
            //启用共享且不重复的debuff免疫
            usPlayer.AsgardsValorImmnue = true;
            usPlayer.ElysianAegisImmnue = true;

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
                player.endurance += 0.1f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnyElysianAegis").
                AddRecipeGroup("CalamityInheritance:AnyAsgardsValor").
                AddIngredient<CosmiliteBar>(10).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
