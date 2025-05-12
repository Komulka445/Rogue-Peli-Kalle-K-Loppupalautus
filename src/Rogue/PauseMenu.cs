using RayGuiCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    internal class PauseMenu
    {
        public event EventHandler BackButtonPressedEvent;
        public event EventHandler OptionsButtonPressedEvent;

        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            int menuStartX = 10;
            int menuStartY = 0;
            int rowHeight = Raylib.GetScreenHeight() / 20;
            int menuWidth = Raylib.GetScreenWidth() / 4;

            // HUOM MenuCreator luodaan aina uudestaan ennen kuin valikko piirrettään.
            MenuCreator creator = new MenuCreator(menuStartX, menuStartY, rowHeight, menuWidth);

            creator.Label("Pause Menu");

            if (creator.Button("Options"))
            {
                OptionsButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            if (creator.Button("Close"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            Raylib.EndDrawing();
        }
    }
}
