using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityInheritance.UI
{
    public class QolPanel : DraedonsPanelUI
    {
        public override int TotalPages => 3;
        
        public override string GetTextByPage()
        {
            return CalamityUtils.GetTextValueFromModItem<DraedonsLogHell>("ContentPage" + (Page + 1));
        }
    }
}
