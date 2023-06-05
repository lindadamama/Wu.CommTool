﻿using Prism.Mvvm;
using System;
using Wu.CommTool.Shared.Enums;
using Wu.Extensions;
using Wu.CommTool.Shared.Common;

namespace Wu.CommTool.Modules.ConvertTools.Models
{
    /// <summary>
    /// 值转换
    /// </summary>
    public class ValueCvt : BindableBase
    {

        /// <summary>
        /// 将相关的16位值赋值null
        /// </summary>
        private void Set16wNull()
        {
            SetProperty(ref _ABCD_16wHex, null, nameof(ABCD_16Int));
            SetProperty(ref _ABCD_16Int, null, nameof(ABCD_16Int));
            SetProperty(ref _ABCD_16Uint, null, nameof(ABCD_16Uint));

            SetProperty(ref _DCBA_16wHex, null, nameof(DCBA_16wHex));
            SetProperty(ref _DCBA_16Int, null, nameof(DCBA_16Int));
            SetProperty(ref _DCBA_16Uint, null, nameof(DCBA_16Uint));

            SetProperty(ref _BADC_16wHex, null, nameof(BADC_16wHex));
            SetProperty(ref _BADC_16Int, null, nameof(BADC_16Int));
            SetProperty(ref _BADC_16Uint, null, nameof(BADC_16Uint));

            SetProperty(ref _CDAB_16wHex, null, nameof(CDAB_16wHex));
            SetProperty(ref _CDAB_16Int, null, nameof(CDAB_16Int));
            SetProperty(ref _CDAB_16Uint, null, nameof(CDAB_16Uint));
        }

        /// <summary>
        /// 根据16进制字符串更新相关数值 (需要将相应的16进制字符串先更新再调用该方法)
        /// </summary>
        public void Set16wValue()
        {
            SetProperty(ref _ABCD_16Int, Convert.ToInt16(_ABCD_16wHex, 16), nameof(ABCD_16Int));
            SetProperty(ref _ABCD_16Uint, Convert.ToUInt16(_ABCD_16wHex, 16), nameof(ABCD_16Uint));

            SetProperty(ref _DCBA_16Int, Convert.ToInt16(_DCBA_16wHex, 16), nameof(DCBA_16Int));
            SetProperty(ref _DCBA_16Uint, Convert.ToUInt16(_DCBA_16wHex, 16), nameof(DCBA_16Uint));

            SetProperty(ref _BADC_16Int, Convert.ToInt16(_BADC_16wHex, 16), nameof(BADC_16Int));
            SetProperty(ref _BADC_16Uint, Convert.ToUInt16(_BADC_16wHex, 16), nameof(BADC_16Uint));

            SetProperty(ref _CDAB_16Int, Convert.ToInt16(_CDAB_16wHex, 16), nameof(CDAB_16Int));
            SetProperty(ref _CDAB_16Uint, Convert.ToUInt16(_CDAB_16wHex, 16), nameof(CDAB_16Uint));
        }

        #region ABCD
        /// <summary>
        /// ABCD 16位16进制
        /// </summary>
        public string? ABCD_16wHex
        {
            get => _ABCD_16wHex;
            set
            {
                var val = value?.RemoveSpace().TrimStart('0');//移除空格 移除左侧的0
                if (string.IsNullOrEmpty(val))
                {
                    val = "0000";
                }
                //含非合法16进制字符
                else if (!Shared.Common.Utils.IsHexString(val))
                {
                    Set16wNull();//将相关的值赋null
                    SetProperty(ref _ABCD_16wHex, val); //当前值保留 可供修改
                    return;
                }
                //字符串符合16进制字符
                else
                {
                    //若位数不够则高位补0
                    if (val.Length < 4)
                        val = val.PadLeft(4, '0');//高位补0
                    //若位数超过4位则判断值是否超过FFFF
                    else if (val.Length > 4)
                        val = "FFFF";
                }

                SetProperty(ref _ABCD_16wHex, val);
                SetProperty(ref _DCBA_16wHex, Shared.Common.Utils.ConvertByteOrder(_ABCD_16wHex!, ModbusByteOrder.DCBA), nameof(DCBA_16wHex));
                SetProperty(ref _BADC_16wHex, Shared.Common.Utils.ConvertByteOrder(_ABCD_16wHex!, ModbusByteOrder.BADC), nameof(BADC_16wHex));
                SetProperty(ref _CDAB_16wHex, Shared.Common.Utils.ConvertByteOrder(_ABCD_16wHex!, ModbusByteOrder.CDAB), nameof(CDAB_16wHex));
                Set16wValue();//根据16进制字符串更新其他数值
            }
        }
        private string? _ABCD_16wHex = "0000";

        /// <summary>
        /// ABCD 32位16进制
        /// </summary>
        public string? ABCD_32wHex
        {
            get => _ABCD_32wHex;
            set
            {
                //验证是否为32进制字符串
                if (string.IsNullOrWhiteSpace(value)) return;
                int i = Convert.ToInt32(value, 16);
                if (i > 0xFFFF)
                {
                    value = "FFFFFFFF";
                }
                SetProperty(ref _ABCD_32wHex, value);
            }
        }
        private string? _ABCD_32wHex = "00000000";

        /// <summary>
        /// ABCD 64位16进制
        /// </summary>
        public string? ABCD_64wHex
        {
            get => _ABCD_64wHex;
            set
            {
                //验证是否为64进制字符串
                if (string.IsNullOrWhiteSpace(value)) return;
                var i = Convert.ToInt64(value, 16);
                if (i > 0xFFFF)
                {
                    value = "FFFF";
                }
                SetProperty(ref _ABCD_64wHex, value);
            }
        }
        private string? _ABCD_64wHex = "0000000000000000";

        /// <summary>
        /// ABCD 16进制无符号整型
        /// </summary>
        public ushort? ABCD_16Uint
        {
            get => _ABCD_16Uint;
            set
            {
                SetProperty(ref _ABCD_16Uint, value);
                byte[] xx = BitConverter.GetBytes((ushort)value!);
                Array.Reverse(xx);
                ABCD_16wHex = BitConverter.ToString(xx, 0).Replace("-", "");
            }
        }
        private ushort? _ABCD_16Uint;

        /// <summary>
        /// ABCD 16进制有符号整型
        /// </summary>
        public short? ABCD_16Int
        {
            get => _ABCD_16Int;
            set
            {
                SetProperty(ref _ABCD_16Int, value);
                byte[] xx = BitConverter.GetBytes((short)value!);
                Array.Reverse(xx);
                ABCD_16wHex = BitConverter.ToString(xx, 0).Replace("-", "");
            }
        }
        private short? _ABCD_16Int;

        /// <summary>
        /// ABCD 32进制无符号整型
        /// </summary>
        public ushort? ABCD_32Uint { get => _ABCD_32Uint; set => SetProperty(ref _ABCD_32Uint, value); }
        private ushort? _ABCD_32Uint;

        /// <summary>
        /// ABCD 32进制有符号整型
        /// </summary>
        public short? ABCD_32Int { get => _ABCD_32Int; set => SetProperty(ref _ABCD_32Int, value); }
        private short? _ABCD_32Int;

        /// <summary>
        /// ABCD 32位浮点型
        /// </summary>
        public float? ABCD_Float { get => _ABCD_Float; set => SetProperty(ref _ABCD_Float, value); }
        private float? _ABCD_Float;

        /// <summary>
        /// ABCD 64进制无符号整型
        /// </summary>
        public ushort? ABCD_64Uint { get => _ABCD_64Uint; set => SetProperty(ref _ABCD_64Uint, value); }
        private ushort? _ABCD_64Uint;

        /// <summary>
        /// ABCD 64进制有符号整型
        /// </summary>
        public short? ABCD_64Int { get => _ABCD_64Int; set => SetProperty(ref _ABCD_64Int, value); }
        private short? _ABCD_64Int;

        /// <summary>
        /// ABCD 64位浮点型
        /// </summary>
        public double? ABCD_Double { get => _ABCD_Double; set => SetProperty(ref _ABCD_Double, value); }
        private double? _ABCD_Double;
        #endregion

        #region DCBA
        /// <summary>
        /// DCBA 16进制
        /// </summary>
        public string? DCBA_16wHex { get => _DCBA_16wHex; set => SetProperty(ref _DCBA_16wHex, value); }
        private string? _DCBA_16wHex;

        /// <summary>
        /// DCBA 16进制无符号整型
        /// </summary>
        public ushort? DCBA_16Uint { get => _DCBA_16Uint; set => SetProperty(ref _DCBA_16Uint, value); }
        private ushort? _DCBA_16Uint;

        /// <summary>
        /// DCBA 16进制有符号整型
        /// </summary>
        public short? DCBA_16Int { get => _DCBA_16Int; set => SetProperty(ref _DCBA_16Int, value); }
        private short? _DCBA_16Int;

        /// <summary>
        /// DCBA 32进制无符号整型
        /// </summary>
        public ushort? DCBA_32Uint { get => _DCBA_32Uint; set => SetProperty(ref _DCBA_32Uint, value); }
        private ushort? _DCBA_32Uint;

        /// <summary>
        /// DCBA 32进制有符号整型
        /// </summary>
        public short? DCBA_32Int { get => _DCBA_32Int; set => SetProperty(ref _DCBA_32Int, value); }
        private short? _DCBA_32Int;

        /// <summary>
        /// DCBA 32位浮点型
        /// </summary>
        public float? DCBA_Float { get => _DCBA_Float; set => SetProperty(ref _DCBA_Float, value); }
        private float? _DCBA_Float;

        /// <summary>
        /// DCBA 64进制无符号整型
        /// </summary>
        public ushort? DCBA_64Uint { get => _DCBA_64Uint; set => SetProperty(ref _DCBA_64Uint, value); }
        private ushort? _DCBA_64Uint;

        /// <summary>
        /// DCBA 64进制有符号整型
        /// </summary>
        public short? DCBA_64Int { get => _DCBA_64Int; set => SetProperty(ref _DCBA_64Int, value); }
        private short? _DCBA_64Int;

        /// <summary>
        /// DCBA 64位浮点型
        /// </summary>
        public double? DCBA_Double { get => _DCBA_Double; set => SetProperty(ref _DCBA_Double, value); }
        private double? _DCBA_Double;
        #endregion

        #region BADC
        /// <summary>
        /// BADC 16进制
        /// </summary>
        public string? BADC_16wHex { get => _BADC_16wHex; set => SetProperty(ref _BADC_16wHex, value); }
        private string? _BADC_16wHex;

        /// <summary>
        /// BADC 16进制无符号整型
        /// </summary>
        public ushort? BADC_16Uint { get => _BADC_16Uint; set => SetProperty(ref _BADC_16Uint, value); }
        private ushort? _BADC_16Uint;

        /// <summary>
        /// BADC 16进制有符号整型
        /// </summary>
        public short? BADC_16Int { get => _BADC_16Int; set => SetProperty(ref _BADC_16Int, value); }
        private short? _BADC_16Int;

        /// <summary>
        /// BADC 32进制无符号整型
        /// </summary>
        public ushort? BADC_32Uint { get => _BADC_32Uint; set => SetProperty(ref _BADC_32Uint, value); }
        private ushort? _BADC_32Uint;

        /// <summary>
        /// BADC 32进制有符号整型
        /// </summary>
        public short? BADC_32Int { get => _BADC_32Int; set => SetProperty(ref _BADC_32Int, value); }
        private short? _BADC_32Int;

        /// <summary>
        /// BADC 32位浮点型
        /// </summary>
        public float? BADC_Float { get => _BADC_Float; set => SetProperty(ref _BADC_Float, value); }
        private float? _BADC_Float;

        /// <summary>
        /// BADC 64进制无符号整型
        /// </summary>
        public ushort? BADC_64Uint { get => _BADC_64Uint; set => SetProperty(ref _BADC_64Uint, value); }
        private ushort? _BADC_64Uint;

        /// <summary>
        /// BADC 64进制有符号整型
        /// </summary>
        public short? BADC_64Int { get => _BADC_64Int; set => SetProperty(ref _BADC_64Int, value); }
        private short? _BADC_64Int;

        /// <summary>
        /// BADC 64位浮点型
        /// </summary>
        public double? BADC_Double { get => _BADC_Double; set => SetProperty(ref _BADC_Double, value); }
        private double? _BADC_Double;
        #endregion

        #region CDAB
        /// <summary>
        /// CDAB 16进制
        /// </summary>
        public string? CDAB_16wHex { get => _CDAB_16wHex; set => SetProperty(ref _CDAB_16wHex, value); }
        private string? _CDAB_16wHex;

        /// <summary>
        /// CDAB 16进制无符号整型
        /// </summary>
        public ushort? CDAB_16Uint { get => _CDAB_16Uint; set => SetProperty(ref _CDAB_16Uint, value); }
        private ushort? _CDAB_16Uint;

        /// <summary>
        /// CDAB 16进制有符号整型
        /// </summary>
        public short? CDAB_16Int { get => _CDAB_16Int; set => SetProperty(ref _CDAB_16Int, value); }
        private short? _CDAB_16Int;

        /// <summary>
        /// CDAB 32进制无符号整型
        /// </summary>
        public ushort? CDAB_32Uint { get => _CDAB_32Uint; set => SetProperty(ref _CDAB_32Uint, value); }
        private ushort? _CDAB_32Uint;

        /// <summary>
        /// CDAB 32进制有符号整型
        /// </summary>
        public short? CDAB_32Int { get => _CDAB_32Int; set => SetProperty(ref _CDAB_32Int, value); }
        private short? _CDAB_32Int;

        /// <summary>
        /// CDAB 32位浮点型
        /// </summary>
        public float? CDAB_Float { get => _CDAB_Float; set => SetProperty(ref _CDAB_Float, value); }
        private float? _CDAB_Float;

        /// <summary>
        /// CDAB 64进制无符号整型
        /// </summary>
        public ushort? CDAB_64Uint { get => _CDAB_64Uint; set => SetProperty(ref _CDAB_64Uint, value); }
        private ushort? _CDAB_64Uint;

        /// <summary>
        /// CDAB 64进制有符号整型
        /// </summary>
        public short? CDAB_64Int { get => _CDAB_64Int; set => SetProperty(ref _CDAB_64Int, value); }
        private short? _CDAB_64Int;

        /// <summary>
        /// CDAB 64位浮点型
        /// </summary>
        public double? CDAB_Double { get => _CDAB_Double; set => SetProperty(ref _CDAB_Double, value); }
        private double? _CDAB_Double;
        #endregion
    }
}