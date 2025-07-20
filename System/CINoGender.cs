using CalamityInheritance.System.Configs;
using CalamityMod.Items.Weapons.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.System
{
    public class CINoGender : ModSystem
    {
        public override void PostAddRecipes()
        {
            if (!CIServerConfig.Instance.FuckYouLGBT)
                return;

            for (int i = 0; i < Main.recipe.Length; i++)
            {
                Recipe rec = Main.recipe[i];
                if(rec.HasResult<Sylvestaff>() && rec.HasIngredient(ItemID.GenderChangePotion) && rec.createItem.stack == 1)
                {
                    rec.RemoveIngredient(ItemID.GenderChangePotion);
                }
            }
        }

    }
}
