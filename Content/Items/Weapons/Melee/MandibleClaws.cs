using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class MandibleClaws : CIMelee, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 7;
            Item.useTurn = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
