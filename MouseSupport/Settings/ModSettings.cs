
using UnityModManagerNet;

namespace MouseSupport.Settings
{
    public class ModSettings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Enable Slingshot Mouse Aim")]
        public bool EnableSlingshotMouseAim = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
        }
    }
}
