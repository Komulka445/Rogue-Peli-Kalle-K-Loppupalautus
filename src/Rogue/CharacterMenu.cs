using RayGuiCreator;
using ZeroElectric.Vinculum;

namespace Rogue
{
    public class CharacterMenu
    {
        // List of possible difficulty choices. The indexing starts at 0
        MultipleChoiceEntry difficultyDropDown = new MultipleChoiceEntry(
            new string[] { "Easy", "Medium", "Hard" });

        // List of possible class choices.
        MultipleChoiceEntry classChoices = new MultipleChoiceEntry(
            new string[] { "Warrior", "Thief", "Magic User" });

        // This records whether a checkbox is active or not.
        // The value is changed by the MenuCreator
        bool checkBox = false;

        // Volume value is modified by the volume slider
        float volume = 1.0f;

        // Textbox data for player's name
        TextBoxEntry playerNameEntry = new TextBoxEntry(15);

        // Level number modified by the spinner
        int levelSpinner = 1;
        // Is the spinner active or not. This is changed by the MenuCreator
        bool spinnerEditActive = false;

        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            int width = Raylib.GetScreenWidth() / 2;
            // Fit 22 rows on the screen
            int rows = 22;
            int rowHeight = Raylib.GetScreenHeight() / rows;
            // Center the menu horizontally
            int x = (Raylib.GetScreenWidth() / 2) - (width / 2);
            // Center the menu vertically
            int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;
            // 3 pixels between rows, text 3 pixels smaller than row height
            MenuCreator c = new MenuCreator(x, y, rowHeight, width, 3, -3);
            c.Label("Main menu");

            c.Label("Player name");
            c.TextBox(playerNameEntry);

            if (c.Button("Honk!"))
            {
                Console.Write("Honk!");
            }

            c.Checkbox("Ranked match", ref checkBox);

            c.Label("Character class");
            c.DropDown(classChoices);

            c.Label("Volume");
            c.Slider("Quiet", "Max", ref volume, 0.0f, 1.0f);

            c.Spinner("Starting level", ref levelSpinner, 1, 12, ref spinnerEditActive);

            c.Label("Difficulty toggle");
            c.ToggleGroup(difficultyDropDown);

            if (c.LabelButton(">>Print values to console"))
            {
                Print();
            }

            // Draws open dropdowns over other menu items
            int menuHeight = c.EndMenu();

            // Draws a rectangle around the menu
            int padding = 2;
            Raylib.DrawRectangleLines(
                x - padding,
                y - padding,
                width + padding * 2,
                menuHeight + padding * 2,
                MenuCreator.GetLineColor());
            Raylib.EndDrawing();
        }
        public void Print()
        {
            Console.WriteLine(" Menu values: ");
            Console.WriteLine($"Ranked:{checkBox}\n" +
            $"Volume: {volume}\n" +
            $"Player name: \"{playerNameEntry}\"\n" + // calls ToString() automatically
            $"Player class {classChoices.GetIndex()}: {classChoices.GetSelected()}\n" +
            $"Difficulty: {difficultyDropDown.GetIndex()}: {difficultyDropDown.GetSelected()}\n" +
            $"Starting level: {levelSpinner}");
        }
    }
}
