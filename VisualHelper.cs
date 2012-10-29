using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media;
using System.Windows.Controls;


namespace EZTVMetro {
    public static class VisualHelper {

        public static T FindFirstElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject {
            var count = VisualTreeHelper.GetChildrenCount(parentElement);
            if (count == 0)
                return null;

            for (int i = 0; i < count; i++) {
                var child = VisualTreeHelper.GetChild(parentElement, i);

                if (child != null && child is T) {
                    return (T)child;
                } else {
                    var result = FindFirstElementInVisualTree<T>(child);
                    if (result != null)
                        return result;

                }
            }
            return null;
        }


        public static StackPanel SearchVisualTree(DependencyObject targetElement) {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);
            if (count == 0)
                return null;

            for (int i = 0; i < count; i++) {
                var child = VisualTreeHelper.GetChild(targetElement, i);

                if (child is StackPanel) {
                    return (child) as StackPanel;
                    ;
                }
                /*
                if (child is TextBlock) {
                    TextBlock targetItem = (TextBlock)child;

                    if (targetItem.Text == "Item2") {
                        targetItem.Foreground = new SolidColorBrush(Colors.Green);
                        return;
                    }
                } else {
                    SearchVisualTree(child);
                }
                 * */

            }
            return null;
        }
    }
}
