using CalamityMod.Buffs.Summon;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Summon;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Placeables.Abyss;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Head)]
        public class SilvaHeadSummonold : CIArmor, ILocalizedModType
        {
            
            public static readonly SoundStyle ActivationSound = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaActivation");
            public static readonly SoundStyle DispelSound = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaDispel");
            public override void SetStaticDefaults()
            {
                Item.ResearchUnlockCount = 1;
            }
            public override void SetDefaults()
            {
                Item.width = 28;
                Item.height = 24;
                Item.value = CIShopValue.RarityPriceDeepBlue;
                Item.rare = ModContent.RarityType<DeepBlue>();
                Item.defense = 13; //110
            }
            public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<SilvaArmorold>() && legs.type == ModContent.ItemType<SilvaLeggingsold>();
            public override void ArmorSetShadows(Player player)
            {
                player.armorEffectDrawShadow = true;
            }

            public override void UpdateArmorSet(Player player)
            {
                var usPlayer = player.CIMod();
                var calPlayer = player.Calamity();
                calPlayer.silvaSummon = true;
                usPlayer.SilvaSummonSetLegacy = true;
                usPlayer.SilvaFakeDeath = true;
                calPlayer.WearingPostMLSummonerSet = true;
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
                        var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);

                        var p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<SilvaCrystal>(), damage, 0f, Main.myPlayer, -20f, 0f);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = 1500;
                    }
                }
                player.GetDamage<SummonDamageClass>() += 0.75f;
                player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 0.15f;
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
                    AddIngredient(ModContent.ItemType<DarksunFragment>(), 10).
                    AddTile<CosmicAnvil>().
                    Register();
            }
        }
    
}
