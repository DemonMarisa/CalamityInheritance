using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee.Swords.AOTCNew;
using CalamityInheritance.Utilities;
using LAP.Core.LAPUI.CustomCD;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CalamityInheritance.Content.CICooldowns
{
    public class AOTCCharge : BaseCD
    {
        public int CurAOTCCharge => Main.LocalPlayer.CIMod().CurAOTCCharge;
        public int MaxAOTCCharge => CalamityInheritancePlayer.MaxAOTCCharge;
        public override Rectangle OverLayerRec => new Rectangle(0, 0, CDTexture_OverLayer.Width, (int)(CDTexture_OverLayer.Height - (CDTexture_OverLayer.Height * (CurAOTCCharge / (float)MaxAOTCCharge))));
        public override LocalizedText DisplayName() =>  CIFunction.GetText($"UI.Cooldowns.AOTCCharge");
        public override void OnRegister()
        {
            Buff = false;
            DeBuff = false;
            Info = true;
        }
        public override void OnSpawn(Player player)
        {
            MaxTime = CalamityInheritancePlayer.MaxAOTCCharge;
        }
        public override void Update(Player player)
        {
            if (player.HeldItem.type == ModContent.ItemType<ArkoftheCosmosNew>())
            {
                Time = 2;
                MaxTime = CalamityInheritancePlayer.MaxAOTCCharge;
            }
        }
        public override bool PreDrawTime(DynamicSpriteFont MGRFont)
        {
            int thisCdRemin = CurAOTCCharge;
            if (thisCdRemin > CalamityInheritancePlayer.MaxAOTCCharge)
                thisCdRemin = CalamityInheritancePlayer.MaxAOTCCharge;
            string Count = $"{thisCdRemin}";
            Vector2 stringsize = ChatManager.GetStringSize(MGRFont, Count, Vector2.One);
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, MGRFont, Count, DrawPosition + new Vector2(0, 24), Color.White, 0f, stringsize / 2, new Vector2(0.4f));
            return false;
        }
    }
}
