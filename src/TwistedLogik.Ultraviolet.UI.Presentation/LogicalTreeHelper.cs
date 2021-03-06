﻿using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains helper methods for interacting with the logical tree.
    /// </summary>
    public static class LogicalTreeHelper
    {
        /// <summary>
        /// Performs an action for each of the specified object's logical children.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="state">A state value to pass to the performed action.</param>
        /// <param name="action">The action to perform on each of the specified object's logical children.</param>
        public static void ForEachChild(DependencyObject dobj, Object state, Action<DependencyObject, Object> action)
        {
            ForEachChild<DependencyObject>(dobj, state, action);
        }

        /// <summary>
        /// Performs an action for each of the specified object's logical children which are of the specified type.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="state">A state value to pass to the performed action.</param>
        /// <param name="action">The action to perform on each of the specified object's logical children.</param>
        public static void ForEachChild<T>(DependencyObject dobj, Object state, Action<T, Object> action) where T : class
        {
            Contract.Require(dobj, "dobj");
            Contract.Require(action, "action");

            var children = GetChildrenCount(dobj);
            for (int i = 0; i < children; i++)
            {
                var child = GetChild(dobj, i) as T;
                if (child != null)
                {
                    action(child, state);
                }
            }
        }

        /// <summary>
        /// Gets the parent of the specified logical object.
        /// </summary>
        /// <param name="dobj">The object for which to retrieve a parent.</param>
        /// <returns>The parent of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetParent(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var element = dobj as UIElement;
            if (element != null)
            {
                return element.Parent;
            }

            return null;
        }

        /// <summary>
        /// Gets the specified logical child of a specified parent object.
        /// </summary>
        /// <param name="dobj">The parent object.</param>
        /// <param name="childIndex">The index of the child to retrieve.</param>
        /// <returns>The specified logical child of <paramref name="dobj"/>.</returns>
        public static DependencyObject GetChild(DependencyObject dobj, Int32 childIndex)
        {
            Contract.Require(dobj, "dobj");

            var element = dobj as FrameworkElement;
            if (element != null)
            {
                return element.GetLogicalChild(childIndex);
            }

            throw new ArgumentOutOfRangeException("childIndex");
        }

        /// <summary>
        /// Gets the number of logical children belonging to the specified parent.
        /// </summary>
        /// <param name="dobj">The parent object to evaluate.</param>
        /// <returns>The number of logical children belonging to <paramref name="dobj"/>.</returns>
        public static Int32 GetChildrenCount(DependencyObject dobj)
        {
            Contract.Require(dobj, "dobj");

            var element = dobj as FrameworkElement;
            if (element != null)
            {
                return element.LogicalChildrenCount;
            }

            return 0;
        }
    }
}
