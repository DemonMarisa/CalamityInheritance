using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Summon.Worms;
using CalamityInheritance.Content.Projectiles.Summon.Worms;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.Summon;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadSummonLegacy : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 12; //132
            Item.rare = RarityType<CatalystViolet>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var usPlayer = player.CIMod();
            if (usPlayer.AuricSilvaFakeDeath)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    string Details = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.AuricTeslaHeadSummonLegacy.Details");
                    tooltips.Add(new TooltipLine(Mod, "Details", Details));
                }
            }
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AuricTeslaBodyArmorold>() && legs.type == ItemType<AuricTeslaCuissesold>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer CIPlayer = player.GetModPlayer<CalamityInheritancePlayer>();
            var modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraSummon = true;
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareSummon = true;
            CIPlayer.AuricSilvaFakeDeath = true;
            modPlayer.godSlayer = true;
            CIPlayer.AuricDebuffImmune = true;
            CIPlayer.GodSlayerSummonSet = true;

            player.setBonus = this.GetLocalizedValue("SetBonus");
            CIPlayer.GodSlayerReborn = true;
            CIPlayer.CanUseLegacyGodSlayerDash = true;

            CIPlayer.SilvaSummonSetLegacy = true;
            modPlayer.WearingPostMLSummonerSet = true;

            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            player.GetDamage<SummonDamageClass>() += 1.2f;
            player.maxMinions += 1;

            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_ItemUse(Item);
                if (player.FindBuffIndex(BuffType<SilvaCrystalBuff>()) == -1)
                {
                    player.AddBuff(BuffType<SilvaCrystalBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ProjectileType<SilvaCrystal>()] < 1)
                {
                    var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(1000);
                    var p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, -1f, ProjectileType<SilvaCrystal>(), damage, 0f, Main.myPlayer, -20f, 0f);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 1000;
                }
                if (CIConfig.Instance.GodSlayerWorm)
                {
                    if (player.ownedProjectileCounts[ProjectileType<DOGworm_Auric>()] < 1)
                    {
                        ModItem item = ItemLoader.GetItem(ItemType<StaffofDOG>());
                        int p = Projectile.NewProjectile(player.GetSource_ItemUse(item.Item), player.Center + new Vector2(600, 300), Vector2.UnitX, ProjectileType<DOGworm_Auric>(), StaffofDOG.BaseDamage, 1, player.whoAmI);
                        Main.projectile[p].originalDamage = StaffofDOG.BaseDamage * 3;
                    }
                }
            }
        }
        public override void UpdateEquip(Player player)
        {
            var modPlayer1 = player.CIMod();
            modPlayer1.auricBoostold = true;
            player.maxMinions += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadSummonold>().
                AddIngredient<GodSlayerHeadSummonold>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>(1).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<SilvaHeadSummonold>().
                AddIngredient<GodSlayerHeadSummonold>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
