using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.MiscDate;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Hammer
{
    public class BasherLegacy : CIMelee
    {
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 60;
            Item.damage = 50;
            Item.DamageType = TrueMelee.Instance;
            Item.useAnimation = Item.useTime = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIIrradiated>(), 300);
            target.AddBuff(BuffID.Poisoned, 300);
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.Acidwood, 30).
                    AddIngredient(CalamityMaterials.SulphuricScale, 12).
                    AddTile(TileID.Anvils).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.Wood, 30).
                    AddRecipeGroup(VanillaRecipeGroups.IronBar, 12).
                    AddTile(TileID.Anvils).
                    Register();
            }
        }
    }
}
