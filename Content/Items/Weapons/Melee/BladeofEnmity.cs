using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class BladeofEnmity : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.25f;
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
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
        }

        

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LifeAlloy>(5).
                AddIngredient<CoreofCalamity>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
