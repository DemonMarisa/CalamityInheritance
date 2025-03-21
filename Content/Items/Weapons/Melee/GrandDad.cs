﻿using CalamityMod.NPCs;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class GrandDad : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 124;
            Item.height = 124;
            Item.damage = 777;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.knockBack = 77f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CalamityGlobalNPC.ShouldAffectNPC(target))
            {
                target.knockBackResist = 7f;
                target.defense = 0;
            }
        }
        public override void AddRecipes()
        {
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe().
                    AddIngredient<LoreRequiem>().
                    AddTile(TileID.AncientMythrilBrick).
                    Register();

                CreateRecipe().
                    AddIngredient<KnowledgeMoonLord>().
                    AddTile(TileID.AncientMythrilBrick).
                    Register();
            }
        }
    }
}
