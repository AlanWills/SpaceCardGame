using _2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Reflection;
using UnitTestFramework;
using System.IO;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace SpaceCardGameUnitTestGameProject
{
    /// <summary>
    /// A screen which has buttons for each unit test class registered with our UnitTestLibrary.
    /// Clicking these buttons will run the test class.
    /// </summary>
    public class RunUnitTestsScreen : MenuScreen
    {
        #region Properties and Fields

        /// <summary>
        /// Runs all of the tests registered
        /// </summary>
        private Button RunAllTestsButton { get; set; }

        /// <summary>
        /// The list control for the buttons which will run our unit tests.
        /// </summary>
        private ListControl RunTestsListControl { get; set; }

        /// <summary>
        /// The list control for the output strings from our unit tests.
        /// </summary>
        private ListControl OutputLog { get; set; }

        /// <summary>
        /// The thread running our test(s)
        /// </summary>
        private CustomThread RunningTestThread { get; set; }

        /// <summary>
        /// The assembly for our auto generated tests using the AutogenUnitTest
        /// </summary>
        private Assembly GeneratedAssembly { get; set; }

        /// <summary>
        /// Store the unit tests in a dictionary with their appropriate buttons so that we can inspect all the unit tests using button references without having to do iteration over the UI
        /// </summary>
        private Dictionary<Button, UnitTest> UnitTestsDictionary { get; set; }

        private UnitTest selectedTest;
        
        #endregion

        public RunUnitTestsScreen(string runTestsScreenDataAsset = "Screens\\RunTestsScreen.xml") :
            base(runTestsScreenDataAsset)
        {
            UnitTestsDictionary = new Dictionary<Button, UnitTest>();
        }

        #region Virtual Functions

        /// <summary>
        /// Compile all of the generated files so that they will be up to date when we run them
        /// </summary>
        public override void LoadContent()
        {
            CheckShouldLoad();
            //CSharpCodeProvider csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            //CompilerParameters parameters = new CompilerParameters(new string[] { "mscorlib.dll", "System.Core.dll" });

            //FileInfo[] files = new DirectoryInfo(AutogenUnitTest.AutoGenDirectory).GetFiles("*.cs", SearchOption.AllDirectories);
            //List<string> fileNames = new List<string>(files.Length);

            //// Go through the generated files and compile them
            //foreach (FileInfo file in files)
            //{
            //    fileNames.Add(file.FullName);
            //}


            // Doesn't work yet
            //CompilerResults results = csc.CompileAssemblyFromFile(parameters, fileNames.ToArray());
            //GeneratedAssembly = results.CompiledAssembly;

            base.LoadContent();
        }

        /// <summary>
        /// Adds our buttons for the unit test classes in our CallingAssembly
        /// </summary>
        protected override void AddInitialUI()
        {
            base.AddInitialUI();

            RunTestsListControl = AddScreenUIObject(new ListControl(new Vector2(ScreenDimensions.X * 0.45f, ScreenDimensions.Y * 0.95f), new Vector2(ScreenCentre.X * 0.5f, ScreenCentre.Y)));

            MakeTestsUI(Assembly.GetExecutingAssembly());
            //MakeTestsUI(GeneratedAssembly);

            OutputLog = AddScreenUIObject(new ListControl(new Vector2(ScreenDimensions.X * 0.45f, ScreenDimensions.Y * 0.95f), new Vector2(ScreenCentre.X * 1.5f, ScreenCentre.Y)));

            // Add this button last so it is on top of all the other UI
            RunAllTestsButton = AddScreenUIObject(new Button("Run All", new Vector2(ScreenCentre.X, 100)));
            RunAllTestsButton.ClickableModule.OnLeftClicked += RunAllTestsEvent;
        }

        private void MakeTestsUI(Assembly assembly)
        {
            foreach (TypeInfo typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.IsSubclassOf(typeof(UnitTest)))
                {
                    UnitTest unitTest = (UnitTest)Activator.CreateInstance(typeInfo);

                    Button runTestButton = RunTestsListControl.AddChild(new Button(unitTest.GetType().Name, Vector2.Zero, AssetManager.DefaultNarrowButtonTextureAsset, AssetManager.DefaultNarrowButtonHighlightedTextureAsset));
                    runTestButton.ClickableModule.OnLeftClicked += RunTestInThread;

                    UnitTestsDictionary.Add(runTestButton, unitTest);
                }
            }
        }

        /// <summary>
        /// Quits our running test if we press Q
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        /// <param name="mousePosition"></param>
        public override void HandleInput(float elapsedGameTime, Vector2 mousePosition)
        {
            base.HandleInput(elapsedGameTime, mousePosition);

            if (GameKeyboard.IsKeyPressed(Keys.Q) && RunningTestThread != null)
            {
                RunningTestThread.Stop();

                // Log the fact that we have aborted the test
                LogInfo("Tests aborted");
            }
        }

        /// <summary>
        /// Go back to the main menu screen for unit testing from this screen
        /// </summary>
        protected override void GoToPreviousScreen()
        {
            Transition(new UnitTestGameMainMenuScreen());
        }

        #endregion

        #region Click Callbacks

        /// <summary>
        /// Runs all the tests in a separate thread and deactivates the UI
        /// </summary>
        /// <param name="baseObject"></param>
        private void RunAllTestsEvent(BaseObject baseObject)
        {
            RunTestsListControl.ShouldHandleInput = false;
            RunningTestThread = ThreadManager.CreateThread(RunAllTests, FinishedTestingRun);
        }

        /// <summary>
        /// Runs our stored unit test in a separate thread when clicked and deactivates the UI
        /// </summary>
        /// <param name="baseObject"></param>
        private void RunTestInThread(BaseObject baseObject)
        {
            Debug.Assert(baseObject is Button);
            Button button = baseObject as Button;

            Debug.Assert(UnitTestsDictionary.TryGetValue(button, out selectedTest));

            RunTestsListControl.ShouldHandleInput = false;
            RunningTestThread = ThreadManager.CreateThread(RunTest, FinishedTestingRun);
        }

        /// <summary>
        /// Run all the tests by iterating over the buttons and extracting each test in turn and running it
        /// </summary>
        private void RunAllTests()
        {
            foreach (UnitTest test in UnitTestsDictionary.Values)
            {
                LogOutput(test.Run());
            }
        }

        /// <summary>
        /// Run a single selected test
        /// </summary>
        private void RunTest()
        {
            UnitTestFramework.DebugUtils.AssertNotNull(selectedTest);
            LogOutput(selectedTest.Run());
        }

        /// <summary>
        /// Sets the thread we use to run the test(s) to null and reactivates our UI
        /// </summary>
        private void FinishedTestingRun()
        {
            RunningTestThread = null;
            RunTestsListControl.ShouldHandleInput = true;
        }

        /// <summary>
        /// Adds the inputted text to our Output
        /// </summary>
        /// <param name="text"></param>
        private void LogInfo(string text)
        {
            Label resultLabel = OutputLog.AddChild(new Label(text, Vector2.Zero), true, true);
            resultLabel.Colour = Color.White;
        }

        /// <summary>
        /// Adds the text in the inputted list to our Output
        /// </summary>
        /// <param name="text"></param>
        private void LogOutput(List<string> textList)
        {
            foreach (string text in textList)
            {
                LogInfo(text);
            }
        }

        #endregion
    }
}