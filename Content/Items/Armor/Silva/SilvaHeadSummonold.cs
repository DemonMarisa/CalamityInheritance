using CalamityMod.Buffs.Summon;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Items.Placeables;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Head)]
    [LegacyName("SilvaHelmet")]
        public class SilvaHeadSummonold : ModItem, ILocalizedModType
        {
            public new string LocalizationCategory => "Items.Armor.PostMoonLord";
            public static readonly SoundStyle ActivationSound = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaActivation");
            public static readonly SoundStyle DispelSound = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaDispel");

            public override void SetDefaults()
            {
                Item.width = 28;
                Item.height = 24;
                Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
                Item.defense = 13; //110
                Item.rare = ModContent.RarityType<DarkBlue>();
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                bool isSilvaSetNEW = body.type == ModContent.ItemType<SilvaArmor>() && legs.type == ModContent.ItemType<SilvaLeggings>();
                bool isSilvaSetOLD = body.type == ModContent.ItemType<SilvaArmorold>() && legs.type == ModContent.ItemType<SilvaLeggingsold>();
                return isSilvaSetNEW || isSilvaSetOLD;
            }

            public override void ArmorSetShadows(Player player)
            {
                player.armorEffectDrawShadow = true;
            }

            public override void UpdateArmorSet(Player player)
            {
                var modPlayer1 = player.CalamityInheritance();
                var modPlayer = player.Calamity();
                modPlayer1.auricsilvaset = true;
                modPlayer.silvaSummon = true;
                modPlayer1.silvaSummonEx = true;
                modPlayer1.silvaRebornMark = true;
                modPlayer.WearingPostMLSummonerSet = true;
                player.setBonus = this.GetLocalizedValue("SetBonus");
                if (player.whoAmI == Main.myPlayer)
                {
                    var source = player.GetSource_ItemUse(Item);
                    if (player.FindBuffIndex(ModContent.BuffType<SilvaCrystalBuff>()) == -1)
                    {
                        player.AddBuff(ModContent.BuffType<SilvaCrystalBuff>(), 3600, true);
                    }
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<SilvaCrystal>()] < 1)
                    {
                        // 08DEC2023: Ozzatron: Silva Crystals spawned with Old Fashioned active will retain their bonus damage indefinitely. Oops. Don't care.
                        int baseDamage = player.ApplyArmorAccDamageBonusesTo(1500);
                        var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);

                        var p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<SilvaCrystal>(), damage, 0f, Main.myPlayer, -20f, 0f);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = 1500;
                    }
                }
                player.GetDamage<SummonDamageClass>() += 0.75f;
            }

            public override void UpdateEquip(Player player)
            {
                player.maxMinions += 5;
            } 

            public override void AddRecipes()
            {
                CreateRecipe().
                    AddIngredient<PlantyMush>(6).
                    AddIngredient<EffulgentFeather>(5).
                    AddIngredient<AscendantSpiritEssence>(2).
                    AddTile<CosmicAnvil>().
                    Register();
            }
        }
    
}
