﻿using CelesteEngine;
using System;
using System.Diagnostics;
using CheckFuncs = System.Tuple<System.Func<object[], bool>, System.Func<object[], bool>>;

namespace SpaceCardGame
{
    public class WindowsInputManager : InputManager
    {
        public WindowsInputManager()
        {
            AddChild(GameKeyboard.Instance);
            AddChild(GameMouse.Instance);
        }

        #region Virtual Functions

        /// <summary>
        /// Uses the input eventName to find the platform check functions and then executes the appropriate one, thus tesing whether input was sufficient to meet the event on our platform.
        /// Returns true if platform input was sufficient for the event and false if not.
        /// </summary>
        /// <param name="eventName"></param>
        public override bool CheckInputEvent(string eventName, object[] windowsParameters, object[] androidParameters)
        {
            CheckFuncs checkFuncs = new CheckFuncs(EmptyCheck, EmptyCheck);
            Debug.Assert(EventFuncMap.TryGetValue(eventName, out checkFuncs));

            return checkFuncs.Item1.Invoke(windowsParameters);
        }

        #endregion
    }
}
