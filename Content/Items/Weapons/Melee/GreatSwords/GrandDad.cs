using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords
{
    public class GrandDad : CIMelee
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
            Item.rare = RarityType<MaliceChallengeDrop>();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss)
            {
                target.knockBackResist = 7f;
                target.defense = 0;
            }
            else
            {
                target.defense = 0;
            }
        }
        public override void AddRecipes()
        {
        }
    }
}
