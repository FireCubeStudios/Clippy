using Microsoft.UI.Composition.SystemBackdrops;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinRT;
using WinUIEx;

namespace Clippy.Windows
{
    public class MicaWindow
    {
        MicaController? m_micaController;
        SystemBackdropConfiguration? m_configurationSource;
        public bool TrySetMicaBackdrop(WindowEx W)
        {
            if (MicaController.IsSupported())
            {
                WindowsSystemDispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();

                // Hooking up the policy object
                m_configurationSource = new SystemBackdropConfiguration();


                // Initial configuration state.
                m_configurationSource.IsInputActive = true;

                m_micaController = new MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                m_micaController.AddSystemBackdropTarget(W.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                m_micaController.SetSystemBackdropConfiguration(m_configurationSource);

                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }
    }
}
