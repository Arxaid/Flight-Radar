// This file is part of the Project RLS.
//
// Copyright (c) 2019 Vladislav Sosedov.

using System;
using System.Windows.Forms;

namespace ProjectSQL
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Interface());
        }
    }
}