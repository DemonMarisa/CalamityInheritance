using CalamityMod.NPCs;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class GrandDad : CIMelee, ILocalizedModType
    {
        
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
            Item.useTurn = true;
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
            if (CIServerConfig.Instance.LegendaryitemsRecipes)
            {
                CreateRecipe().
                    AddIngredient<KnowledgeMoonLord>().
                    DisableDecraft().
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
