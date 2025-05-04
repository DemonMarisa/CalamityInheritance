using CalamityMod.Items;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee.Spear;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Spear
{
    public class InsidiousImpalerLegacy : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 70;
            Item.damage = 320;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<InsidiousImpalerProjLegacy>();
            Item.shootSpeed = 5f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
    }
}
