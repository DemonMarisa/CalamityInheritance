using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Common.CalamityModCross;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class BladeofEnmity : CIMelee, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.25f;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 64;
            Item.scale = 1.5f;
            Item.useTurn = true;
            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.knockBack = 8f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIBrimstoneFlames>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffType<CIBrimstoneFlames>(), 300);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(CalMaterialsID.LifeAlloyID, 5).
                AddIngredient(CalamityMaterials.CoreofCalamity, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
