﻿namespace Wu.CommTool.ViewModels.DesignViewModels
{
    public class ModbusRtuDesignViewModel : ModbusRtuViewModel
    {
        private static ModbusRtuDesignViewModel _Instance;
        public static ModbusRtuDesignViewModel Instance => _Instance ??= new();
        public ModbusRtuDesignViewModel()
        {
            IsDrawersOpen.IsLeftDrawerOpen = true;
        }
    }
}