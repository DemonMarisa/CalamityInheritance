using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class BasherLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 60;
            Item.damage = 50;
            Item.DamageType = GetInstance<TrueMeleeDamageClass>();
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
            target.AddBuff(BuffType<Irradiated>(), 300);
            target.AddBuff(BuffID.Poisoned, 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Acidwood>(30).
                AddIngredient<SulphuricScale>(12).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
