using _2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Reflection;
using UnitTestFramework;

namespace SpaceCardGameUnitTestGameProject
{
    /// <summary>
    /// The main menu screen for our unit testing project.
    /// Will contain a button to proceed to a sceen where we can choose tests to run, or an exit button.
    /// </summary>
    public class UnitTestGameMainMenuScreen : MenuScreen
    {
        #region Properties and Fields

        private Button GenerateButton { get; set; }

        #endregion

        public UnitTestGameMainMenuScreen(string mainMenuScreenDataAsset = "Screens\\UnitTestGameMainMenuScreen.xml") :
            base(mainMenuScreenDataAsset)
        {
            AutogenUnitTest.AutoGenDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..", "Autogen");
        }

        #region Virtual Functions

        /// <summary>
        /// Adds a button for transitioning to a screen where we choose unit tests to run.
        /// Adds a button to exit.
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            Button runTestsButton = AddScreenUIObject(new Button("Run Tests", new Vector2(ScreenCentre.X, ScreenCentre.Y * 0.5f)));
            runTestsButton.ClickableModule.OnLeftClicked += TransitionToRunTestsScreen;

            GenerateButton = runTestsButton.AddChild(new Button("Generate Tests", Anchor.kBottomCentre, 1));
            GenerateButton.ClickableModule.OnLeftClicked += GenerateTests;

            Button exitButton = GenerateButton.AddChild(new Button("Exit", Anchor.kBottomCentre, 1));
            exitButton.ClickableModule.OnLeftClicked += Exit;
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// Transitions to a screen where we can choose unit tests to run.
        /// </summary>
        /// <param name="baseObject"></param>
        private void TransitionToRunTestsScreen(BaseObject baseObject)
        {
            Transition(new RunUnitTestsScreen());
        }

        /// <summary>
        /// Iterates over all of the types in the executing assembly and auto generates any tests that need it
        /// </summary>
        /// <param name="baseObject"></param>
        private void GenerateTests(BaseObject baseObject)
        {
            foreach (Type type in Assembly.GetExecutingAssembly().DefinedTypes)
            {
                if (type.IsSubclassOf(typeof(AutogenUnitTest)) && !type.IsAbstract)
                {
                    AutogenUnitTest autogenTest = (AutogenUnitTest)Activator.CreateInstance(type, null);
                    autogenTest.Generate();
                }
            }

            TextDialogBoxCommand box = CommandManager.Instance.AddChild(new TextDialogBoxCommand("Tests generated successfully"), true, true);
            box.OnDeathCallback += Box_OnDeathCallback;
        }

        private void Box_OnDeathCallback(Command command)
        {
            Exit(null);
        }

        /// <summary>
        /// Quits the test game program.
        /// </summary>
        /// <param name="clickable"></param>
        private void Exit(BaseObject baseObject)
        {
            ScreenManager.Instance.EndGame();
        }

        #endregion
    }
}