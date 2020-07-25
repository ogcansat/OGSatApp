using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;

using Xamarin.Forms;
using Timer = System.Timers.Timer;

namespace OGSatApp.Pages.Behaviors
{
    internal static class GUIAnimations
    {
        public static CancellationTokenSource DotLoadingAnimation(Label label, string title, int length, double interval)
        {

            var token = new CancellationTokenSource();
            Timer timer = new Timer(interval);
            timer.Elapsed += (s, a) =>
            {
                if (token.IsCancellationRequested)
                {
                    Device.InvokeOnMainThreadAsync(() => label.Text = "");
                    timer.Stop();
                    return;
                }

                Device.InvokeOnMainThreadAsync(() =>
                {
                    label.Text = !label.Text.StartsWith(title) ? title : label.Text;
                    label.Text += ".";
                    if (label.Text.Length >= title.Length + length) label.Text = title;
                });

                
            };
            timer.Start();

            return token;
           
        }

        public static void FillTableSection(TableSection section, string[] columns, string[] values, EventHandler handler = null)
        {
            section.Clear();

            for (int i = 0; i < columns.Length; i++)
            {
                var layout = new StackLayout() { Padding = 10};
                layout.Children.Add(new Label() { Text = columns[i], TextColor = Color.Black });
                if (values != null)
                    layout.Children.Add(new Label() { Text = values[i], Padding=new Thickness(10,0,10,0) });
                var cell = new ViewCell() { View = layout };
                cell.Tapped += handler;
                section.Add(cell);
            }
        }
    }
}
