using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords
{
    public class CorruptedCrusherBlade : CIMelee
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.DamageType = DamageClass.Melee;
            Item.width = 66;
            Item.height = 80;
            Item.scale = 1.25f;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.75f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(7))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.CursedInferno, 120);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.EbonstoneBlock, 50).
                AddIngredient(ItemID.ShadowScale, 5).
                AddIngredient(ItemID.IronBar, 4).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
