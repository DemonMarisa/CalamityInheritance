using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Axes
{
    public class Waraxe : CIMelee
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.knockBack = 5.25f;
            Item.useTime = 18;
            Item.useAnimation = 22;
            Item.axe = 85 / 5;

            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 40;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
                target.AddBuff(BuffType<WarCleave>(), 1800);
        }
    }
}
