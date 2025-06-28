using Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace psdPH.Photoshop
{
    public class KostylExecutor
    {
        // Импорт WinAPI функций
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowTitle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const uint WM_CLOSE = 0x0010;
        const string PhotoshopDialogTitle = "Adobe Photoshop"; // Заголовок окна (может отличаться)

        static bool isDialogClosed = false;
        public bool tryAction(Action action)
        {
            bool result = false;
            // Запускаем поток для отслеживания диалогового окна
            Thread watcherThread = new Thread(ClosePhotoshopDialogIfExists);
            watcherThread.Start();

            try
            {
                Console.WriteLine("Пытаемся скопировать стиль слоя...");
                action();
                result = true;
            }
            catch (COMException ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
                if (isDialogClosed)
                {
                    Console.WriteLine("Диалог был закрыт автоматически. Продолжаем работу.");
                }
            }
            finally
            {
                watcherThread.Join(); // Ожидаем завершения потока-наблюдателя
            }
            return result;
        }

        static void ClosePhotoshopDialogIfExists()
        {
            // Ждём 500 мс, чтобы основная операция успела начаться
            Thread.Sleep(500);

            // Пытаемся найти окно Photoshop
            IntPtr hWnd = FindWindow(null, PhotoshopDialogTitle);
            if (hWnd != IntPtr.Zero)
            {
                Console.WriteLine("Найдено диалоговое окно. Закрываем...");
                SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                isDialogClosed = true;
            }
        }
    }
}
