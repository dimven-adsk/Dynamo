using System;
using System.Windows;
using System.Windows.Controls;
using Dynamo.Logging;
using Dynamo.Utilities;
using Dynamo.ViewModels;

namespace Dynamo.UI.Controls
{
    public class InOutPortPanel : VirtualizingStackPanel
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (this.Children.Count <= 0)
            {
                // A port list without any port in it.
                return base.ArrangeOverride(arrangeSize);
            }

            var itemsControl = WpfUtilities.FindUpVisualTree<ItemsControl>(this);
            var generator = itemsControl.ItemContainerGenerator;

            int itemIndex = 0;
            double x = 0, y = 0;
            foreach (UIElement child in this.Children)
            {
                try
                {
                    var portVm = generator.ItemFromContainer(child) as PortViewModel;
                    var lineIndex = portVm.PortModel.LineIndex;
                    var multiplier = ((lineIndex == -1) ? itemIndex : lineIndex);
                    var portHeight = portVm.PortModel.Height;

                    y = multiplier * portHeight;
                    child.Arrange(new Rect(x, y, arrangeSize.Width, portHeight));
                    itemIndex = itemIndex + 1;
                }
                catch (Exception ex)
                {
                    Analytics.TrackException(ex, true);
                }
            }

            return base.ArrangeOverride(arrangeSize);
        }
    }
}
