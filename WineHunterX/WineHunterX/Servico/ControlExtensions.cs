using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WineHunterX.Servico
{
    static class ControlExtensions
    {
        internal static void AddTap(this Label label, Action action)
        {
            label.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(action),
            });
        }

        internal static void AddTap(this Image image, Action action)
        {
            image.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(action),
            });
        }


        internal static void AddTap(this WebView view, Action action)
        {
            view.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(action),
            });
        }

        internal static void AddTap(this WebView view, Command command, object commandParameter = null)
        {
            view.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = command,
                CommandParameter = commandParameter,
            });
        }
    }
}
