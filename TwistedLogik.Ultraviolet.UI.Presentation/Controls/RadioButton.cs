﻿using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a radio button.
    /// </summary>
    [UvmlKnownType(null, "TwistedLogik.Ultraviolet.UI.Presentation.Controls.Templates.RadioButton.xml")]
    public class RadioButton : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadioButton"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="name">The element's identifying name within its namescope.</param>
        public RadioButton(UltravioletContext uv, String id)
            : base(uv, id)
        {

        }

        /// <inheritdoc/>
        protected override void ToggleChecked()
        {
            if (!Checked)
            {
                Checked = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnCheckedChanged()
        {
            if (Checked)
            {
                var parent = LogicalTreeHelper.GetParent(this);
                if (parent != null)
                {
                    var children = LogicalTreeHelper.GetChildrenCount(parent);
                    for (int i = 0; i < children; i++)
                    {
                        var sibling = LogicalTreeHelper.GetChild(parent, i);
                        if (sibling == this)
                            continue;

                        var radioButton = sibling as RadioButton;
                        if (radioButton == null)
                            continue;

                        radioButton.Checked = false;
                    }
                }
            }
            base.OnCheckedChanged();
        }

        /// <summary>
        /// Gets a <see cref="Visibility"/> value that describes the visibility state
        /// of the radio button's mark.
        /// </summary>
        private Visibility MarkVisibility
        {
            get { return Checked ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
