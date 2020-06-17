using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;
using System.Threading;

namespace SagensVision.VisionTool
{
    public  class ClassShow3D
    {
#if !(NO_EXPORT_MAIN || NO_EXPORT_APP_MAIN)
        public ClassShow3D()
        {
            // Default settings used in HDevelop 
            HOperatorSet.SetSystem("width", 25000);
            HOperatorSet.SetSystem("height", 25000);
            if (HalconAPI.isWindows)
                HOperatorSet.SetSystem("use_window_thread", "true");
            //action();
        }
#endif

        HTuple gDispObjOffset;
        HTuple gLabelsDecor;
        HTuple gInfoDecor;
        HTuple gInfoPos;
        HTuple gTitlePos;
        HTuple gTitleDecor;
        HTuple gTerminationButtonLabel;
        HTuple gAlphaDeselected;
        HTuple gIsSinglePose;
        HTuple gUsesOpenGL;
        HTuple ExpGetGlobalVar_gDispObjOffset()
        {
            return gDispObjOffset;
        }
        void ExpSetGlobalVar_gDispObjOffset(HTuple val)
        {
            gDispObjOffset = val;
        }

        HTuple ExpGetGlobalVar_gLabelsDecor()
        {
            return gLabelsDecor;
        }
        void ExpSetGlobalVar_gLabelsDecor(HTuple val)
        {
            gLabelsDecor = val;
        }

        HTuple ExpGetGlobalVar_gInfoDecor()
        {
            return gInfoDecor;
        }
        void ExpSetGlobalVar_gInfoDecor(HTuple val)
        {
            gInfoDecor = val;
        }

        HTuple ExpGetGlobalVar_gInfoPos()
        {
            return gInfoPos;
        }
        void ExpSetGlobalVar_gInfoPos(HTuple val)
        {
            gInfoPos = val;
        }

        HTuple ExpGetGlobalVar_gTitlePos()
        {
            return gTitlePos;
        }
        void ExpSetGlobalVar_gTitlePos(HTuple val)
        {
            gTitlePos = val;
        }

        HTuple ExpGetGlobalVar_gTitleDecor()
        {
            return gTitleDecor;
        }
        void ExpSetGlobalVar_gTitleDecor(HTuple val)
        {
            gTitleDecor = val;
        }

        HTuple ExpGetGlobalVar_gTerminationButtonLabel()
        {
            return gTerminationButtonLabel;
        }
        void ExpSetGlobalVar_gTerminationButtonLabel(HTuple val)
        {
            gTerminationButtonLabel = val;
        }

        HTuple ExpGetGlobalVar_gAlphaDeselected()
        {
            return gAlphaDeselected;
        }
        void ExpSetGlobalVar_gAlphaDeselected(HTuple val)
        {
            gAlphaDeselected = val;
        }

        HTuple ExpGetGlobalVar_gIsSinglePose()
        {
            return gIsSinglePose;
        }
        void ExpSetGlobalVar_gIsSinglePose(HTuple val)
        {
            gIsSinglePose = val;
        }

        HTuple ExpGetGlobalVar_gUsesOpenGL()
        {
            return gUsesOpenGL;
        }
        void ExpSetGlobalVar_gUsesOpenGL(HTuple val)
        {
            gUsesOpenGL = val;
        }

        // Procedures 
        // External procedures 
        // Chapter: Develop
        // Short Description: Switch dev_update_pc, dev_update_var and dev_update_window to 'off'. 
        public void dev_update_off()
        {

            // Initialize local and output iconic variables 
            //This procedure sets different update settings to 'off'.
            //This is useful to get the best performance and reduce overhead.
            //
            // dev_update_pc(...); only in hdevelop
            // dev_update_var(...); only in hdevelop
            // dev_update_window(...); only in hdevelop

            return;
        }

        // Chapter: Develop
        // Short Description: Switch dev_update_pc, dev_update_var and dev_update_window to 'on'. 
        public void dev_update_on()
        {

            // Initialize local and output iconic variables 
            //This procedure sets different update settings to 'on'.
            //
            // dev_update_pc(...); only in hdevelop
            // dev_update_var(...); only in hdevelop
            // dev_update_window(...); only in hdevelop

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: This procedure writes a text message. 
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_MaxAscent = null;
            HTuple hv_MaxDescent = null, hv_MaxWidth = null, hv_MaxHeight = null;
            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRow = new HTuple();
            HTuple hv_FactorColumn = new HTuple(), hv_UseShadow = null;
            HTuple hv_ShadowColor = null, hv_Exception = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_CurrentColor = new HTuple();
            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 
            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically
            //   for each new textline.
            //Box: If Box[0] is set to 'true', the text is written within an orange box.
            //     If set to' false', no box is displayed.
            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
            //       the text is written in a box of that color.
            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
            //       'true' -> display a shadow in a default color
            //       'false' -> display no shadow (same as if no second value is given)
            //       otherwise -> use given string as color string for the shadow color
            //
            //Prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //Transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //Display text box depending on text size
            hv_UseShadow = 1;
            hv_ShadowColor = "gray";
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
            {
                if (hv_Box_COPY_INP_TMP == null)
                    hv_Box_COPY_INP_TMP = new HTuple();
                hv_Box_COPY_INP_TMP[0] = "#fce9d4";
                hv_ShadowColor = "#f28d26";
            }
            if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
                1))) != 0)
            {
                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
                {
                    //Use default ShadowColor set above
                }
                else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
                    "false"))) != 0)
                {
                    hv_UseShadow = 0;
                }
                else
                {
                    hv_ShadowColor = hv_Box_COPY_INP_TMP[1];
                    //Valid color?
                    try
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(
                            1));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        hv_Exception = "Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)";
                        throw new HalconException(hv_Exception);
                    }
                }
            }
            if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
            {
                //Valid color?
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                //Calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //Display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                //Set shadow color
                HOperatorSet.SetColor(hv_WindowHandle, hv_ShadowColor);
                if ((int)(hv_UseShadow) != 0)
                {
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1, hv_C2 + 1);
                }
                //Set box color
                HOperatorSet.SetColor(hv_WindowHandle, hv_Box_COPY_INP_TMP.TupleSelect(0));
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //Reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: Set font independent of OS 
        public void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

            // Initialize local and output iconic variables 
            //This procedure sets the text font of the current window with
            //the specified attributes.
            //It is assumed that following fonts are installed on the system:
            //Windows: Courier New, Arial Times New Roman
            //Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
            //Linux: courier, helvetica, times
            //Because fonts are displayed smaller on Linux than on Windows,
            //a scaling factor of 1.25 is used the get comparable results.
            //For Linux, only a limited number of font sizes is supported,
            //to get comparable results, it is recommended to use one of the
            //following sizes: 9, 11, 14, 16, 20, 27
            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
            //
            //Input parameters:
            //WindowHandle: The graphics window for which the font will be set
            //Size: The font size. If Size=-1, the default of 16 is used.
            //Bold: If set to 'true', a bold font is used
            //Slant: If set to 'true', a slanted font is used
            //
            HOperatorSet.GetSystem("operating_system", out hv_OS);
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
            {
                hv_Size_COPY_INP_TMP = 16;
            }
            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
            {
                //Set font on Windows systems
                try
                {
                    //Check, if font scaling is switched on
                    HOperatorSet.OpenWindow(0, 0, 256, 256, 0, "buffer", "", out hv_BufferWindowHandle);
                    HOperatorSet.SetFont(hv_BufferWindowHandle, "-Consolas-16-*-0-*-*-1-");
                    HOperatorSet.GetStringExtents(hv_BufferWindowHandle, "test_string", out hv_Ascent,
                        out hv_Descent, out hv_Width, out hv_Height);
                    //Expected width is 110
                    hv_Scale = 110.0 / hv_Width;
                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
                    HOperatorSet.CloseWindow(hv_BufferWindowHandle);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Courier New";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Consolas";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Arial";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "Times New Roman";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 1;
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = 0;
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Dar"))) != 0)
            {
                //Set font on Mac OS X systems. Since OS X does not have a strict naming
                //scheme for font attributes, we use tables to determine the correct font
                //name.
                hv_SubFamily = 0;
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(1);
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_SubFamily = hv_SubFamily.TupleBor(2);
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Menlo-Regular";
                    hv_Fonts[1] = "Menlo-Italic";
                    hv_Fonts[2] = "Menlo-Bold";
                    hv_Fonts[3] = "Menlo-BoldItalic";
                }
                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "CourierNewPSMT";
                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
                    hv_Fonts[2] = "CourierNewPS-BoldMT";
                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "ArialMT";
                    hv_Fonts[1] = "Arial-ItalicMT";
                    hv_Fonts[2] = "Arial-BoldMT";
                    hv_Fonts[3] = "Arial-BoldItalicMT";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "TimesNewRomanPSMT";
                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
                }
                else
                {
                    //Attempt to figure out which of the fonts installed on the system
                    //the user could have meant.
                    HOperatorSet.QueryFont(hv_WindowHandle, out hv_SystemFonts);
                    hv_Fonts = new HTuple();
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                    //Guess name of bold slanted font
                    hv_Guess = new HTuple();
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                    {
                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
                        {
                            if (hv_Fonts == null)
                                hv_Fonts = new HTuple();
                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
                            break;
                        }
                    }
                }
                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //throw (Exception)
                }
            }
            else
            {
                //Set font for UNIX systems
                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
                hv_AllowedFontSizes = new HTuple();
                hv_AllowedFontSizes[0] = 11;
                hv_AllowedFontSizes[1] = 14;
                hv_AllowedFontSizes[2] = 17;
                hv_AllowedFontSizes[3] = 20;
                hv_AllowedFontSizes[4] = 25;
                hv_AllowedFontSizes[5] = 34;
                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
                    -1))) != 0)
                {
                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
                        0));
                }
                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
                    "Courier")))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "courier";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "helvetica";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Font_COPY_INP_TMP = "times";
                }
                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "bold";
                }
                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Bold_COPY_INP_TMP = "medium";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
                {
                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
                    {
                        hv_Slant_COPY_INP_TMP = "i";
                    }
                    else
                    {
                        hv_Slant_COPY_INP_TMP = "o";
                    }
                }
                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
                {
                    hv_Slant_COPY_INP_TMP = "r";
                }
                else
                {
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(((hv_OS.TupleSubstr(0, 4))).TupleEqual("Linux"))).TupleAnd(
                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
                    {
                        HOperatorSet.QueryFont(hv_WindowHandle, out hv_Fonts);
                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
                            hv_FontSelRegexp);
                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
                            0))) != 0)
                        {
                            hv_Exception = "Wrong font name";
                            //throw (Exception)
                        }
                        else
                        {
                            try
                            {
                                HOperatorSet.SetFont(hv_WindowHandle, (((hv_FontsCourier.TupleSelect(
                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException2)
                            {
                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
                                //throw (Exception)
                            }
                        }
                    }
                    //throw (Exception)
                }
            }
            // dev_set_preferences(...); only in hdevelop

            return;
        }
        public static bool breakOut = false;
        // Chapter: Graphics / Output
        // Short Description: Interactively display 3D object models 

        bool showEnd = true;
        public void visualize_object_model_3d(HTuple hv_WindowHandle, HTuple hv_ObjectModel3D,
                HTuple hv_CamParam, HTuple hv_PoseIn, HTuple hv_GenParamName, HTuple hv_GenParamValue,
                HTuple hv_Title, HTuple hv_Label, HTuple hv_Information, out HTuple hv_PoseOut)
        {
            isRunOver = false;
            // Local iconic variables 

            HObject ho_Image, ho_ImageDump = null;

            // Local control variables 

            HTuple ExpTmpLocalVar_gDispObjOffset = null;
            HTuple ExpTmpLocalVar_gLabelsDecor = null, ExpTmpLocalVar_gInfoDecor = null;
            HTuple ExpTmpLocalVar_gInfoPos = null, ExpTmpLocalVar_gTitlePos = null;
            HTuple ExpTmpLocalVar_gTitleDecor = null, ExpTmpLocalVar_gTerminationButtonLabel = null;
            HTuple ExpTmpLocalVar_gAlphaDeselected = null, ExpTmpLocalVar_gIsSinglePose = new HTuple();
            HTuple ExpTmpLocalVar_gUsesOpenGL = null, hv_TrackballSize = null;
            HTuple hv_VirtualTrackball = null, hv_MouseMapping = null;
            HTuple hv_WaitForButtonRelease = null, hv_MaxNumModels = null;
            HTuple hv_WindowCenteredRotation = null, hv_NumModels = null;
            HTuple hv_SelectedObject = null, hv_ClipRegion = null;
            HTuple hv_CPLength = null, hv_RowNotUsed = null, hv_ColumnNotUsed = null;
            HTuple hv_Width = null, hv_Height = null, hv_WPRow1 = null;
            HTuple hv_WPColumn1 = null, hv_WPRow2 = null, hv_WPColumn2 = null;
            HTuple hv_CamWidth = new HTuple(), hv_CamHeight = new HTuple();
            HTuple hv_Scale = new HTuple(), hv_Indices = null, hv_DispBackground = null;
            HTuple hv_Mask = new HTuple(), hv_Center = null, hv_Poses = new HTuple();
            HTuple hv_HomMat3Ds = new HTuple(), hv_Sequence = new HTuple();
            HTuple hv_PoseEstimated = new HTuple(), hv_WindowHandleBuffer = null;
            HTuple hv_Font = null, hv_Exception = null, hv_OpenGLInfo = new HTuple();
            HTuple hv_DummyObjectModel3D = new HTuple(), hv_Scene3DTest = new HTuple();
            HTuple hv_CameraIndexTest = new HTuple(), hv_PoseTest = new HTuple();
            HTuple hv_InstanceIndexTest = new HTuple(), hv_MinImageSize = null;
            HTuple hv_TrackballRadiusPixel = null, hv_Ascent = null;
            HTuple hv_Descent = null, hv_TextWidth = null, hv_TextHeight = null;
            HTuple hv_NumChannels = null, hv_ColorImage = null, hv_Scene3D = null;
            HTuple hv_CameraIndex = null, hv_AllInstances = null, hv_SetLight = null;
            HTuple hv_LightParam = new HTuple(), hv_LightPosition = new HTuple();
            HTuple hv_LightKind = new HTuple(), hv_LightIndex = new HTuple();
            HTuple hv_PersistenceParamName = null, hv_PersistenceParamValue = null;
            HTuple hv_ValueListSS3P = null, hv_ValueListSS3IP = null;
            HTuple hv_AlphaOrig = null, hv_UsedParamMask = null, hv_I = null;
            HTuple hv_ParamName = new HTuple(), hv_ParamValue = new HTuple();
            HTuple hv_UseParam = new HTuple(), hv_ParamNameTrunk = new HTuple();
            HTuple hv_Instance = new HTuple(), hv_GenParamNameRemaining = new HTuple();
            HTuple hv_GenParamValueRemaining = new HTuple(), hv_HomMat3D = null;
            HTuple hv_Qx = null, hv_Qy = null, hv_Qz = null, hv_TBCenter = null;
            HTuple hv_TBSize = null, hv_ButtonHold = null, hv_VisualizeTB = new HTuple();
            HTuple hv_MaxIndex = new HTuple(), hv_TrackballCenterRow = new HTuple();
            HTuple hv_TrackballCenterCol = new HTuple(), hv_GraphEvent = new HTuple();
            HTuple hv_Exit = new HTuple(), hv_GraphButtonRow = new HTuple();
            HTuple hv_GraphButtonColumn = new HTuple(), hv_GraphButton = new HTuple();
            HTuple hv_ButtonReleased = new HTuple();
            HTuple hv_CamParam_COPY_INP_TMP = hv_CamParam.Clone();
            HTuple hv_GenParamName_COPY_INP_TMP = hv_GenParamName.Clone();
            HTuple hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue.Clone();
            HTuple hv_Label_COPY_INP_TMP = hv_Label.Clone();
            HTuple hv_PoseIn_COPY_INP_TMP = hv_PoseIn.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageDump);
            hv_PoseOut = new HTuple();
            try
            {
                //The procedure visualize_object_model_3d can be used to display
                //one or more 3d object models and to interactively modify
                //the object poses by using the mouse.
                //
                //The pose can be modified by moving the mouse while
                //pressing a mouse button. The default settings are:
                //
                // Left mouse button:   Modify the object orientation
                // Shift+ left mouse button  or
                // center mouse button: Modify the object distance
                // Right mouse button:  Modify the object position
                // Ctrl + Left mouse button: (De-)select object(s)
                // Alt + Mouse button: Low mouse sensitiviy
                // (Default may be changed with the variable MouseMapping below)
                //
                //In GenParamName and GenParamValue all generic Paramters
                //of disp_object_model_3d are supported.
                //
                //**********************************************************
                //Define global variables
                //**********************************************************
                //
                //global def tuple gDispObjOffset
                //global def tuple gLabelsDecor
                //global def tuple gInfoDecor
                //global def tuple gInfoPos
                //global def tuple gTitlePos
                //global def tuple gTitleDecor
                //global def tuple gTerminationButtonLabel
                //global def tuple gAlphaDeselected
                //global def tuple gIsSinglePose
                //global def tuple gUsesOpenGL
                //
                //**********************************************************
                //First some user defines that may be adapted if desired
                //**********************************************************
                //
                //TrackballSize defines the diameter of the trackball in
                //the image with respect to the smaller image dimension.
                hv_TrackballSize = 0.8;
                //
                //VirtualTrackball defines the type of virtual trackball that
                //shall be used ('shoemake' or 'bell').
                hv_VirtualTrackball = "shoemake";
                //VirtualTrackball := 'bell'
                //
                //Functionality of mouse buttons
                //    1: Left Button
                //    2: Middle Button
                //    4: Right Button
                //    5: Left+Right Mousebutton
                //  8+x: Shift + Mousebutton
                // 16+x: Ctrl + Mousebutton
                // 48+x: Ctrl + Alt + Mousebutton
                //in the order [Translate, Rotate, Scale, ScaleAlternative1, ScaleAlternative2, SelectObjects, ToggleSelectionMode]
                hv_MouseMapping = new HTuple();
                hv_MouseMapping[0] = 17;
                hv_MouseMapping[1] = 1;
                hv_MouseMapping[2] = 2;
                hv_MouseMapping[3] = 5;
                hv_MouseMapping[4] = 9;
                hv_MouseMapping[5] = 4;
                hv_MouseMapping[6] = 49;
                //
                //The labels of the objects appear next to their projected
                //center. With gDispObjOffset a fixed offset is added
                //                  R,  C
                ExpTmpLocalVar_gDispObjOffset = new HTuple();
                ExpTmpLocalVar_gDispObjOffset[0] = -30;
                ExpTmpLocalVar_gDispObjOffset[1] = 0;
                ExpSetGlobalVar_gDispObjOffset(ExpTmpLocalVar_gDispObjOffset);
                //
                //Customize the decoration of the different text elements
                //              Color,   Box
                ExpTmpLocalVar_gInfoDecor = new HTuple();
                ExpTmpLocalVar_gInfoDecor[0] = "white";
                ExpTmpLocalVar_gInfoDecor[1] = "false";
                ExpSetGlobalVar_gInfoDecor(ExpTmpLocalVar_gInfoDecor);
                ExpTmpLocalVar_gLabelsDecor = new HTuple();
                ExpTmpLocalVar_gLabelsDecor[0] = "white";
                ExpTmpLocalVar_gLabelsDecor[1] = "false";
                ExpSetGlobalVar_gLabelsDecor(ExpTmpLocalVar_gLabelsDecor);
                ExpTmpLocalVar_gTitleDecor = new HTuple();
                ExpTmpLocalVar_gTitleDecor[0] = "black";
                ExpTmpLocalVar_gTitleDecor[1] = "true";
                ExpSetGlobalVar_gTitleDecor(ExpTmpLocalVar_gTitleDecor);
                //
                //Customize the position of some text elements
                //  gInfoPos has one of the values
                //  {'UpperLeft', 'LowerLeft', 'UpperRight'}
                ExpTmpLocalVar_gInfoPos = "LowerLeft";
                ExpSetGlobalVar_gInfoPos(ExpTmpLocalVar_gInfoPos);
                //  gTitlePos has one of the values
                //  {'UpperLeft', 'UpperCenter', 'UpperRight'}
                ExpTmpLocalVar_gTitlePos = "UpperLeft";
                ExpSetGlobalVar_gTitlePos(ExpTmpLocalVar_gTitlePos);
                //Alpha value (=1-transparency) that is used for visualizing
                //the objects that are not selected
                ExpTmpLocalVar_gAlphaDeselected = 0.3;
                ExpSetGlobalVar_gAlphaDeselected(ExpTmpLocalVar_gAlphaDeselected);
                //Customize the label of the continue button
                ExpTmpLocalVar_gTerminationButtonLabel = " Continue ";
                ExpSetGlobalVar_gTerminationButtonLabel(ExpTmpLocalVar_gTerminationButtonLabel);
                //Define if the continue button responds to a single click event or
                //if it responds only if the mouse button is released while being placed
                //over the continue button.
                //'true':  Wait until the continue button has been released.
                //         This should be used to avoid unwanted continuations of
                //         subsequent calls of visualize_object_model_3d, which can
                //         otherwise occur if the mouse button remains pressed while the
                //         next visualization is active.
                //'false': Continue the execution already if the continue button is
                //         pressed. This option allows a fast forwarding through
                //         subsequent calls of visualize_object_model_3d.
                hv_WaitForButtonRelease = "true";
                //Number of 3D Object models that can be handled individually
                //if there are more models passed then this number, some calculations
                //are performed differently. And the individual handling of models is not
                //supported anymore
                hv_MaxNumModels = 5;
                //Defines the default for the initial state of the rotation center:
                //(1) The rotation center is fixed in the center of the image and lies
                //    on the surface of the object.
                //(2) The rotation center lies in the center of the object.
                hv_WindowCenteredRotation = 2;
                //
                //**********************************************************
                //
                //Initialize some values
                hv_NumModels = new HTuple(hv_ObjectModel3D.TupleLength());
                hv_SelectedObject = HTuple.TupleGenConst(hv_NumModels, 1);
                //
                //Apply some system settings
                // dev_set_preferences(...); only in hdevelop
                // dev_get_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop
                // dev_get_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop
                HOperatorSet.GetSystem("clip_region", out hv_ClipRegion);
                HOperatorSet.SetSystem("clip_region", "false");

                //
                //Refactor camera parameters to fit to window size
                //
                hv_CPLength = new HTuple(hv_CamParam_COPY_INP_TMP.TupleLength());
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowNotUsed, out hv_ColumnNotUsed,
                    out hv_Width, out hv_Height);
                HOperatorSet.GetPart(hv_WindowHandle, out hv_WPRow1, out hv_WPColumn1, out hv_WPRow2,
                    out hv_WPColumn2);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height - 1, hv_Width - 1);
                if ((int)(new HTuple(hv_CPLength.TupleEqual(0))) != 0)
                {
                    hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[0] = 0.06;
                    hv_CamParam_COPY_INP_TMP[1] = 0;
                    hv_CamParam_COPY_INP_TMP[2] = 8.5e-6;
                    hv_CamParam_COPY_INP_TMP[3] = 8.5e-6;
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Width / 2);
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Height / 2);
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Width);
                    hv_CamParam_COPY_INP_TMP = hv_CamParam_COPY_INP_TMP.TupleConcat(hv_Height);
                    hv_CPLength = new HTuple(hv_CamParam_COPY_INP_TMP.TupleLength());
                }
                else
                {
                    hv_CamWidth = ((hv_CamParam_COPY_INP_TMP.TupleSelect(hv_CPLength - 2))).TupleReal()
                        ;
                    hv_CamHeight = ((hv_CamParam_COPY_INP_TMP.TupleSelect(hv_CPLength - 1))).TupleReal()
                        ;
                    hv_Scale = ((((hv_Width / hv_CamWidth)).TupleConcat(hv_Height / hv_CamHeight))).TupleMin()
                        ;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 6] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 6)) / hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 5] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 5)) / hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 4] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 4)) * hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 3] = (hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 3)) * hv_Scale;
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 2] = (((hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 2)) * hv_Scale)).TupleInt();
                    if (hv_CamParam_COPY_INP_TMP == null)
                        hv_CamParam_COPY_INP_TMP = new HTuple();
                    hv_CamParam_COPY_INP_TMP[hv_CPLength - 1] = (((hv_CamParam_COPY_INP_TMP.TupleSelect(
                        hv_CPLength - 1)) * hv_Scale)).TupleInt();
                }
                //
                //Check the generic parameters for window_centered_rotation
                //(Note that the default is set above to WindowCenteredRotation := 2)
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("inspection_mode");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        0)))).TupleEqual("surface"))) != 0)
                    {
                        hv_WindowCenteredRotation = 1;
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(
                        hv_Indices.TupleSelect(0)))).TupleEqual("standard"))) != 0)
                    {
                        hv_WindowCenteredRotation = 2;
                    }
                    else
                    {
                        //Wrong parameter value, use default value
                    }
                    hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices);
                    hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove(
                        hv_Indices);
                }
                //
                //Check the generic parameters for disp_background
                //(The former parameter name 'use_background' is still supported
                // for compatibility reasons)
                hv_DispBackground = "false";
                if ((int)(new HTuple((new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    )).TupleGreater(0))) != 0)
                {
                    hv_Mask = ((hv_GenParamName_COPY_INP_TMP.TupleEqualElem("disp_background"))).TupleOr(
                        hv_GenParamName_COPY_INP_TMP.TupleEqualElem("use_background"));
                    hv_Indices = hv_Mask.TupleFind(1);
                }
                else
                {
                    hv_Indices = -1;
                }
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    hv_DispBackground = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        0));
                    if ((int)((new HTuple(hv_DispBackground.TupleNotEqual("true"))).TupleAnd(
                        new HTuple(hv_DispBackground.TupleNotEqual("false")))) != 0)
                    {
                        //Wrong parameter value: Only 'true' and 'false' are allowed
                        throw new HalconException("Wrong value for parameter 'disp_background' (must be either 'true' or 'false')");
                    }
                    //Note the the background is handled explicitely in this procedure
                    //and therefore, the parameter is removed from the list of
                    //parameters and disp_background is always set to true (see below)
                    hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices);
                    hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove(
                        hv_Indices);
                }
                //
                //Read and check the parameter Label for each object
                if ((int)(new HTuple((new HTuple(hv_Label_COPY_INP_TMP.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    hv_Label_COPY_INP_TMP = 0;
                }
                else if ((int)(new HTuple((new HTuple(hv_Label_COPY_INP_TMP.TupleLength()
                    )).TupleEqual(1))) != 0)
                {
                    hv_Label_COPY_INP_TMP = HTuple.TupleGenConst(hv_NumModels, hv_Label_COPY_INP_TMP);
                }
                else
                {
                    if ((int)(new HTuple((new HTuple(hv_Label_COPY_INP_TMP.TupleLength())).TupleNotEqual(
                        hv_NumModels))) != 0)
                    {
                        //Error: Number of elements in Label does not match the
                        //number of object models
                        // stop(); only in hdevelop
                    }
                }
                //
                //Read and check the parameter PoseIn for each object
                get_object_models_center(hv_ObjectModel3D, out hv_Center);
                if ((int)(new HTuple((new HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength())).TupleEqual(
                    0))) != 0)
                {
                    //If no pose was specified by the caller, automatically calculate
                    //a pose that is appropriate for the visualization.
                    //Set the initial model reference pose. The orientation is parallel
                    //to the object coordinate system, the position is at the center
                    //of gravity of all models.
                    HOperatorSet.CreatePose(-(hv_Center.TupleSelect(0)), -(hv_Center.TupleSelect(
                        1)), -(hv_Center.TupleSelect(2)), 0, 0, 0, "Rp+T", "gba", "point", out hv_PoseIn_COPY_INP_TMP);
                    determine_optimum_pose_distance(hv_ObjectModel3D, hv_CamParam_COPY_INP_TMP,
                        0.9, hv_PoseIn_COPY_INP_TMP, out hv_PoseEstimated);
                    hv_Poses = new HTuple();
                    hv_HomMat3Ds = new HTuple();
                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                    hv_Poses = hv_PoseEstimated.TupleSelect(hv_Sequence % 7);
                    ExpTmpLocalVar_gIsSinglePose = 1;
                    ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                }
                else if ((int)(new HTuple((new HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength()
                    )).TupleEqual(7))) != 0)
                {
                    hv_Poses = new HTuple();
                    hv_HomMat3Ds = new HTuple();
                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                    determine_optimum_pose_distance(hv_ObjectModel3D, hv_CamParam_COPY_INP_TMP,
                       0.9, hv_PoseIn_COPY_INP_TMP, out hv_PoseEstimated);

                    hv_Poses = hv_PoseEstimated.TupleSelect(hv_Sequence % 7);
                    ExpTmpLocalVar_gIsSinglePose = 1;
                    ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                }
                else
                {
                    if ((int)(new HTuple((new HTuple(hv_PoseIn_COPY_INP_TMP.TupleLength())).TupleNotEqual(
                        (new HTuple(hv_ObjectModel3D.TupleLength())) * 7))) != 0)
                    {
                        //Error: Wrong number of values of input control parameter 'PoseIn'
                        // stop(); only in hdevelop
                    }
                    else
                    {
                        hv_Poses = hv_PoseIn_COPY_INP_TMP.Clone();
                    }
                    ExpTmpLocalVar_gIsSinglePose = 0;
                    ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                }
                //
                //Open (invisible) buffer window to avoid flickering
                //HOperatorSet.OpenWindow(0, 0, hv_Width, hv_Height, 0, "buffer", "", out hv_WindowHandleBuffer);
                hv_WindowHandleBuffer = hv_WindowHandle;
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height - 1, hv_Width - 1);
                HOperatorSet.GetFont(hv_WindowHandle, out hv_Font);
                try
                {
                    HOperatorSet.SetFont(hv_WindowHandle, hv_Font);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                //
                // Is OpenGL available and should it be used?
                ExpTmpLocalVar_gUsesOpenGL = "true";
                ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("opengl");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    ExpTmpLocalVar_gUsesOpenGL = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        0));
                    ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                    hv_GenParamName_COPY_INP_TMP = hv_GenParamName_COPY_INP_TMP.TupleRemove(hv_Indices);
                    hv_GenParamValue_COPY_INP_TMP = hv_GenParamValue_COPY_INP_TMP.TupleRemove(
                        hv_Indices);
                    if ((int)((new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleNotEqual("true"))).TupleAnd(
                        new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleNotEqual("false")))) != 0)
                    {
                        //Wrong parameter value: Only 'true' and 'false' are allowed
                        throw new HalconException("Wrong value for parameter 'opengl' (must be either 'true' or 'false')");
                    }
                }
                if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("true"))) != 0)
                {
                    HOperatorSet.GetSystem("opengl_info", out hv_OpenGLInfo);
                    if ((int)(new HTuple(hv_OpenGLInfo.TupleEqual("No OpenGL support included."))) != 0)
                    {
                        ExpTmpLocalVar_gUsesOpenGL = "false";
                        ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                    }
                    else
                    {
                        HOperatorSet.GenObjectModel3dFromPoints(0, 0, 0, out hv_DummyObjectModel3D);
                        HOperatorSet.CreateScene3d(out hv_Scene3DTest);
                        HOperatorSet.AddScene3dCamera(hv_Scene3DTest, hv_CamParam_COPY_INP_TMP,
                            out hv_CameraIndexTest);
                        determine_optimum_pose_distance(hv_DummyObjectModel3D, hv_CamParam_COPY_INP_TMP,
                            0.9, ((((((new HTuple(0)).TupleConcat(0)).TupleConcat(0)).TupleConcat(
                            0)).TupleConcat(0)).TupleConcat(0)).TupleConcat(0), out hv_PoseTest);
                        HOperatorSet.AddScene3dInstance(hv_Scene3DTest, hv_DummyObjectModel3D,
                            hv_PoseTest, out hv_InstanceIndexTest);
                        try
                        {
                            HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3DTest, hv_InstanceIndexTest);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            ExpTmpLocalVar_gUsesOpenGL = "false";
                            ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                        }
                        HOperatorSet.ClearScene3d(hv_Scene3DTest);
                        HOperatorSet.ClearObjectModel3d(hv_DummyObjectModel3D);
                    }
                }
                //
                //Compute the trackball
                hv_MinImageSize = ((hv_Width.TupleConcat(hv_Height))).TupleMin();
                hv_TrackballRadiusPixel = (hv_TrackballSize * hv_MinImageSize) / 2.0;
                //
                //Measure the text extents for the continue button in the
                //graphics window
                HOperatorSet.GetStringExtents(hv_WindowHandleBuffer, ExpGetGlobalVar_gTerminationButtonLabel() + "  ",
                    out hv_Ascent, out hv_Descent, out hv_TextWidth, out hv_TextHeight);
                //
                //Store background image
                //if ((int)(new HTuple(hv_DispBackground.TupleEqual("false"))) != 0)
                //{
                //    HOperatorSet.ClearWindow(hv_WindowHandle);
                //}
                ho_Image.Dispose();
                HOperatorSet.DumpWindowImage(out ho_Image, hv_WindowHandle);
                //Special treatment for color background images necessary
                HOperatorSet.CountChannels(ho_Image, out hv_NumChannels);
                hv_ColorImage = new HTuple(hv_NumChannels.TupleEqual(3));
                //
                HOperatorSet.CreateScene3d(out hv_Scene3D);
                HOperatorSet.AddScene3dCamera(hv_Scene3D, hv_CamParam_COPY_INP_TMP, out hv_CameraIndex);
                HOperatorSet.AddScene3dInstance(hv_Scene3D, hv_ObjectModel3D, hv_Poses, out hv_AllInstances);
                //Always set 'disp_background' to true,  because it is handled explicitely
                //in this procedure (see above)
                HOperatorSet.SetScene3dParam(hv_Scene3D, "disp_background", "true");
                //Check if we have to set light specific parameters
                hv_SetLight = new HTuple(hv_GenParamName_COPY_INP_TMP.TupleRegexpTest("light_"));
                if ((int)(hv_SetLight) != 0)
                {
                    //set position of light source
                    hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("light_position");
                    if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                        new HTuple())))) != 0)
                    {
                        //If multiple light positions are given, use the last one
                        hv_LightParam = ((((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                            (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleSplit(", "))).TupleNumber()
                            ;
                        if ((int)(new HTuple((new HTuple(hv_LightParam.TupleLength())).TupleNotEqual(
                            4))) != 0)
                        {
                            throw new HalconException("light_position must be given as a string that contains four space separated floating point numbers");
                        }
                        hv_LightPosition = hv_LightParam.TupleSelectRange(0, 2);
                        hv_LightKind = "point_light";
                        if ((int)(new HTuple(((hv_LightParam.TupleSelect(3))).TupleEqual(0))) != 0)
                        {
                            hv_LightKind = "directional_light";
                        }
                        //Currently, only one light source is supported
                        HOperatorSet.RemoveScene3dLight(hv_Scene3D, 0);
                        HOperatorSet.AddScene3dLight(hv_Scene3D, hv_LightPosition, hv_LightKind,
                            out hv_LightIndex);
                        HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                        HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                    }
                    //set ambient part of light source
                    hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("light_ambient");
                    if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                        new HTuple())))) != 0)
                    {
                        //If the ambient part is set multiple times, use the last setting
                        hv_LightParam = ((((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                            (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleSplit(", "))).TupleNumber()
                            ;
                        if ((int)(new HTuple((new HTuple(hv_LightParam.TupleLength())).TupleLess(
                            3))) != 0)
                        {
                            throw new HalconException("light_ambient must be given as a string that contains three space separated floating point numbers");
                        }
                        HOperatorSet.SetScene3dLightParam(hv_Scene3D, 0, "ambient", hv_LightParam.TupleSelectRange(
                            0, 2));
                        HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                        HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                    }
                    //set diffuse part of light source
                    hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("light_diffuse");
                    if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                        new HTuple())))) != 0)
                    {
                        //If the diffuse part is set multiple times, use the last setting
                        hv_LightParam = ((((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                            (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleSplit(", "))).TupleNumber()
                            ;
                        if ((int)(new HTuple((new HTuple(hv_LightParam.TupleLength())).TupleLess(
                            3))) != 0)
                        {
                            throw new HalconException("light_diffuse must be given as a string that contains three space separated floating point numbers");
                        }
                        HOperatorSet.SetScene3dLightParam(hv_Scene3D, 0, "diffuse", hv_LightParam.TupleSelectRange(
                            0, 2));
                        HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                        HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                    }
                }
                //
                //Handle persistence parameters separately because persistence will
                //only be activated immediately before leaving the visualization
                //procedure
                hv_PersistenceParamName = new HTuple();
                hv_PersistenceParamValue = new HTuple();
                //set position of light source
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("object_index_persistence");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual("true"))) != 0)
                    {
                        hv_PersistenceParamName = hv_PersistenceParamName.TupleConcat("object_index_persistence");
                        hv_PersistenceParamValue = hv_PersistenceParamValue.TupleConcat("true");
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(
                        hv_Indices.TupleSelect((new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual(
                        "false"))) != 0)
                    {
                    }
                    else
                    {
                        throw new HalconException("Wrong value for parameter 'object_index_persistence' (must be either 'true' or 'false')");
                    }
                    HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                    HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                }
                hv_Indices = hv_GenParamName_COPY_INP_TMP.TupleFind("depth_persistence");
                if ((int)((new HTuple(hv_Indices.TupleNotEqual(-1))).TupleAnd(new HTuple(hv_Indices.TupleNotEqual(
                    new HTuple())))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_Indices.TupleSelect(
                        (new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual("true"))) != 0)
                    {
                        hv_PersistenceParamName = hv_PersistenceParamName.TupleConcat("depth_persistence");
                        hv_PersistenceParamValue = hv_PersistenceParamValue.TupleConcat("true");
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue_COPY_INP_TMP.TupleSelect(
                        hv_Indices.TupleSelect((new HTuple(hv_Indices.TupleLength())) - 1)))).TupleEqual(
                        "false"))) != 0)
                    {
                    }
                    else
                    {
                        throw new HalconException("Wrong value for parameter 'depth_persistence' (must be either 'true' or 'false')");
                    }
                    HOperatorSet.TupleRemove(hv_GenParamName_COPY_INP_TMP, hv_Indices, out hv_GenParamName_COPY_INP_TMP);
                    HOperatorSet.TupleRemove(hv_GenParamValue_COPY_INP_TMP, hv_Indices, out hv_GenParamValue_COPY_INP_TMP);
                }
                //
                //Parse the generic parameters
                //- First, all parameters that are understood by set_scene_3d_instance_param
                HOperatorSet.GetParamInfo("set_scene_3d_param", "GenParamName", "value_list",
                    out hv_ValueListSS3P);
                HOperatorSet.GetParamInfo("set_scene_3d_instance_param", "GenParamName", "value_list",
                    out hv_ValueListSS3IP);
                hv_AlphaOrig = HTuple.TupleGenConst(hv_NumModels, 1);
                hv_UsedParamMask = HTuple.TupleGenConst(new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    ), 0);
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_ParamName = hv_GenParamName_COPY_INP_TMP.TupleSelect(hv_I);
                    hv_ParamValue = hv_GenParamValue_COPY_INP_TMP.TupleSelect(hv_I);
                    //Check if this parameter is understood by set_scene_3d_param
                    hv_UseParam = new HTuple(hv_ValueListSS3P.TupleRegexpTest(("^" + hv_ParamName) + "$"));
                    if ((int)(hv_UseParam) != 0)
                    {
                        try
                        {
                            HOperatorSet.SetScene3dParam(hv_Scene3D, hv_ParamName, hv_ParamValue);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            if ((int)((new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1204))).TupleOr(
                                new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1304)))) != 0)
                            {
                                throw new HalconException((("Wrong type or value for parameter " + hv_ParamName) + ": ") + hv_ParamValue);
                            }
                            else
                            {
                                throw new HalconException(hv_Exception);
                            }
                        }
                        if (hv_UsedParamMask == null)
                            hv_UsedParamMask = new HTuple();
                        hv_UsedParamMask[hv_I] = 1;
                        if ((int)(new HTuple(hv_ParamName.TupleEqual("alpha"))) != 0)
                        {
                            hv_AlphaOrig = HTuple.TupleGenConst(hv_NumModels, hv_ParamValue);
                        }
                        continue;
                    }
                    //Check if it is a parameter that is valid for only one instance
                    //and therefore can be set only with set_scene_3d_instance_param
                    hv_ParamNameTrunk = hv_ParamName.TupleRegexpReplace("_\\d+$", "");
                    hv_UseParam = new HTuple(hv_ValueListSS3IP.TupleRegexpTest(("^" + hv_ParamNameTrunk) + "$"));
                    if ((int)(hv_UseParam) != 0)
                    {
                        hv_Instance = ((hv_ParamName.TupleRegexpReplace(("^" + hv_ParamNameTrunk) + "_(\\d+)$",
                            "$1"))).TupleNumber();
                        if ((int)((new HTuple(hv_Instance.TupleLess(0))).TupleOr(new HTuple(hv_Instance.TupleGreater(
                            hv_NumModels - 1)))) != 0)
                        {
                            throw new HalconException(("Parameter " + hv_ParamName) + " refers to a non existing 3D object model");
                        }
                        try
                        {
                            HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Instance, hv_ParamNameTrunk,
                                hv_ParamValue);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            if ((int)((new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1204))).TupleOr(
                                new HTuple(((hv_Exception.TupleSelect(0))).TupleEqual(1304)))) != 0)
                            {
                                throw new HalconException((("Wrong type or value for parameter " + hv_ParamName) + ": ") + hv_ParamValue);
                            }
                            else
                            {
                                throw new HalconException(hv_Exception);
                            }
                        }
                        if (hv_UsedParamMask == null)
                            hv_UsedParamMask = new HTuple();
                        hv_UsedParamMask[hv_I] = 1;
                        if ((int)(new HTuple(hv_ParamNameTrunk.TupleEqual("alpha"))) != 0)
                        {
                            if (hv_AlphaOrig == null)
                                hv_AlphaOrig = new HTuple();
                            hv_AlphaOrig[hv_Instance] = hv_ParamValue;
                        }
                        continue;
                    }
                }
                //
                //Check if there are remaining parameters
                if ((int)(new HTuple((new HTuple(hv_GenParamName_COPY_INP_TMP.TupleLength()
                    )).TupleGreater(0))) != 0)
                {
                    hv_GenParamNameRemaining = hv_GenParamName_COPY_INP_TMP.TupleSelectMask(hv_UsedParamMask.TupleNot()
                        );
                    hv_GenParamValueRemaining = hv_GenParamValue_COPY_INP_TMP.TupleSelectMask(
                        hv_UsedParamMask.TupleNot());
                    if ((int)(new HTuple(hv_GenParamNameRemaining.TupleNotEqual(new HTuple()))) != 0)
                    {
                        throw new HalconException("Parameters that cannot be handled: " + (((((hv_GenParamNameRemaining + " := ") + hv_GenParamValueRemaining) + ", ")).TupleSum()
                            ));
                    }
                }
                //
                //Start the visualization loop
                HOperatorSet.PoseToHomMat3d(hv_Poses.TupleSelectRange(0, 6), out hv_HomMat3D);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Center.TupleSelect(0), hv_Center.TupleSelect(
                    1), hv_Center.TupleSelect(2), out hv_Qx, out hv_Qy, out hv_Qz);
                hv_TBCenter = new HTuple();
                hv_TBCenter = hv_TBCenter.TupleConcat(hv_Qx);
                hv_TBCenter = hv_TBCenter.TupleConcat(hv_Qy);
                hv_TBCenter = hv_TBCenter.TupleConcat(hv_Qz);
                hv_TBSize = (0.5 + ((0.5 * (hv_SelectedObject.TupleSum())) / hv_NumModels)) * hv_TrackballRadiusPixel;
                hv_ButtonHold = 0;

                //ThreadPool.QueueUserWorkItem(
                //delegate
                //{

                ////});

                while (true)
                {
                    if (breakOut)
                    {
                        isRunOver = true;
                        hv_GraphEvent = 0;
                        hv_Exit = 1;
                        break;
                    }
                    showEnd = false;

                    hv_VisualizeTB = new HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(
                            0));
                    hv_MaxIndex = ((((new HTuple(hv_ObjectModel3D.TupleLength())).TupleConcat(
                        hv_MaxNumModels))).TupleMin()) - 1;
                    //Set trackball fixed in the center of the window
                    hv_TrackballCenterRow = hv_Height / 2;
                    hv_TrackballCenterCol = hv_Width / 2;
                    if ((int)(new HTuple(hv_WindowCenteredRotation.TupleEqual(1))) != 0)
                    {
                        try
                        {
                            get_trackball_center_fixed(hv_SelectedObject.TupleSelectRange(0, hv_MaxIndex),
                                hv_TrackballCenterRow, hv_TrackballCenterCol, hv_TrackballRadiusPixel,
                                hv_Scene3D, hv_ObjectModel3D.TupleSelectRange(0, hv_MaxIndex), hv_Poses.TupleSelectRange(
                                0, ((hv_MaxIndex + 1) * 7) - 1), hv_WindowHandleBuffer, hv_CamParam_COPY_INP_TMP,
                                hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, out hv_TBCenter,
                                out hv_TBSize);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            disp_message(hv_WindowHandle, "Surface inspection mode is not available.",
                                "image", 5, 20, "red", "true");
                            hv_WindowCenteredRotation = 2;
                            get_trackball_center(hv_SelectedObject.TupleSelectRange(0, hv_MaxIndex),
                                hv_TrackballRadiusPixel, hv_ObjectModel3D.TupleSelectRange(0, hv_MaxIndex),
                                hv_Poses.TupleSelectRange(0, ((hv_MaxIndex + 1) * 7) - 1), out hv_TBCenter,
                                out hv_TBSize);
                            HOperatorSet.WaitSeconds(1);
                        }
                    }
                    else
                    {
                        get_trackball_center(hv_SelectedObject.TupleSelectRange(0, hv_MaxIndex),
                            hv_TrackballRadiusPixel, hv_ObjectModel3D.TupleSelectRange(0, hv_MaxIndex),
                            hv_Poses.TupleSelectRange(0, ((hv_MaxIndex + 1) * 7) - 1), out hv_TBCenter,
                            out hv_TBSize);
                    }
                    //显示3D
                    //HOperatorSet.SetSystem()


                    dump_image_output(ho_Image, hv_WindowHandleBuffer, hv_Scene3D, hv_AlphaOrig,
                            hv_ObjectModel3D, hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP,
                            hv_CamParam_COPY_INP_TMP, hv_Poses, hv_ColorImage, hv_Title, hv_Information,
                            hv_Label_COPY_INP_TMP, hv_VisualizeTB, "true", hv_TrackballCenterRow,
                            hv_TrackballCenterCol, hv_TBSize, hv_SelectedObject, hv_WindowCenteredRotation,
                            hv_TBCenter);


                    ho_ImageDump.Dispose();
                    HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                    HDevWindowStack.SetActive(hv_WindowHandle);

                    HOperatorSet.DispObj(ho_ImageDump, hv_WindowHandle);

                    //
                    //Check for mouse events
                    hv_GraphEvent = 0;
                    hv_Exit = 0;



                    while (true)
                    {
                        if (breakOut)
                        {
                            isRunOver = true;
                            hv_GraphEvent = 0;
                            hv_Exit = 1;
                            break;
                        }
                        showEnd = false;
                        try
                        {
                            HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_GraphButtonRow,
                                out hv_GraphButtonColumn, out hv_GraphButton);
                            //    disp_message(hv_WindowHandle, "X:"+ hv_GraphButtonColumn+" Y:" + hv_GraphButtonRow, "window", 0, 0,
                            //ExpGetGlobalVar_gTitleDecor().TupleSelect(0), ExpGetGlobalVar_gTitleDecor().TupleSelect(
                            //1));

                            if ((int)(new HTuple(hv_GraphButton.TupleNotEqual(0))) != 0)
                            {
                                if ((int)((new HTuple((new HTuple((new HTuple(hv_GraphButtonRow.TupleGreater(
                                    (hv_Height - hv_TextHeight) - 13))).TupleAnd(new HTuple(hv_GraphButtonRow.TupleLess(
                                    hv_Height))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleGreater(
                                    (hv_Width - hv_TextWidth) - 13))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleLess(
                                    hv_Width)))) != 0)
                                {
                                    //Wait until the continue button has been released
                                    if ((int)(new HTuple(hv_WaitForButtonRelease.TupleEqual("true"))) != 0)
                                    {
                                        while (true)
                                        {
                                            if (breakOut)
                                            {
                                                isRunOver = true;
                                                hv_GraphEvent = 0;
                                                hv_Exit = 1;
                                                break;
                                            }
                                            showEnd = false;
                                            HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_GraphButtonRow,
                                                    out hv_GraphButtonColumn, out hv_GraphButton);
                                            //                     disp_message(hv_WindowHandle, "X:" + hv_GraphButtonColumn + " Y:" + hv_GraphButtonRow, "window", 0, 0,
                                            //ExpGetGlobalVar_gTitleDecor().TupleSelect(0), ExpGetGlobalVar_gTitleDecor().TupleSelect(
                                            //1));

                                            if ((int)((new HTuple(hv_GraphButton.TupleEqual(0))).TupleOr(
                                                    new HTuple(hv_GraphButton.TupleEqual(new HTuple())))) != 0)
                                            {
                                                if ((int)((new HTuple((new HTuple((new HTuple(hv_GraphButtonRow.TupleGreater(
                                                    (hv_Height - hv_TextHeight) - 13))).TupleAnd(new HTuple(hv_GraphButtonRow.TupleLess(
                                                    hv_Height))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleGreater(
                                                    (hv_Width - hv_TextWidth) - 13))))).TupleAnd(new HTuple(hv_GraphButtonColumn.TupleLess(
                                                    hv_Width)))) != 0)
                                                {
                                                    hv_ButtonReleased = 1;
                                                }
                                                else
                                                {
                                                    hv_ButtonReleased = 0;
                                                }
                                                //
                                                break;
                                            }
                                            //Keep waiting until mouse button is released or moved out of the window
                                            
                                        }

                                    }
                                    else
                                    {
                                        hv_ButtonReleased = 1;
                                    }
                                    //Exit the visualization loop
                                    if ((int)(hv_ButtonReleased) != 0)
                                    {
                                        hv_Exit = 1;
                                        break;
                                    }
                                }
                                hv_GraphEvent = 1;
                                break;
                            }
                            else
                            {
                                hv_ButtonHold = 0;
                            }
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            Thread.Sleep(500);
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);

                            //hv_GraphEvent = 0;
                            //hv_Exit = 1;
                            //break;

                            //Keep waiting
                        }
                        
                    }
                    
                    if ((int)(hv_GraphEvent) != 0)
                    {
                        analyze_graph_event(ho_Image, hv_MouseMapping, hv_GraphButton, hv_GraphButtonRow,
                            hv_GraphButtonColumn, hv_WindowHandle, hv_WindowHandleBuffer, hv_VirtualTrackball,
                            hv_TrackballSize, hv_SelectedObject, hv_Scene3D, hv_AlphaOrig, hv_ObjectModel3D,
                            hv_CamParam_COPY_INP_TMP, hv_Label_COPY_INP_TMP, hv_Title, hv_Information,
                            hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP, hv_Poses,
                            hv_ButtonHold, hv_TBCenter, hv_TBSize, hv_WindowCenteredRotation, hv_MaxNumModels,
                            out hv_Poses, out hv_SelectedObject, out hv_ButtonHold, out hv_WindowCenteredRotation);
                    }
                    if ((int)(hv_Exit) != 0)
                    {
                        break;
                    }

                }
                
                //});
                //
                //Display final state with persistence, if requested
                //Note that disp_object_model_3d must be used instead of the 3D scene
                if ((int)(new HTuple((new HTuple(hv_PersistenceParamName.TupleLength())).TupleGreater(
                        0))) != 0)
                {
                    try
                    {
                        HOperatorSet.DispObjectModel3d(hv_WindowHandle, hv_ObjectModel3D, hv_CamParam_COPY_INP_TMP,
                            hv_Poses, ((new HTuple("disp_background")).TupleConcat("alpha")).TupleConcat(
                            hv_PersistenceParamName), ((new HTuple("true")).TupleConcat(0.0)).TupleConcat(
                            hv_PersistenceParamValue));
                    }
                    // catch (Exception) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        // stop(); only in hdevelop

                    }
                }
                //
                //Compute the output pose
                if ((int)(ExpGetGlobalVar_gIsSinglePose()) != 0)
                {
                    hv_PoseOut = hv_Poses.TupleSelectRange(0, 6);
                }
                else
                {
                    hv_PoseOut = hv_Poses.Clone();
                }
                //
                //Clean up
                HOperatorSet.SetSystem("clip_region", hv_ClipRegion);
                // dev_set_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop

                dump_image_output(ho_Image, hv_WindowHandleBuffer, hv_Scene3D, hv_AlphaOrig,
                       hv_ObjectModel3D, hv_GenParamName_COPY_INP_TMP, hv_GenParamValue_COPY_INP_TMP,
                       hv_CamParam_COPY_INP_TMP, hv_Poses, hv_ColorImage, hv_Title, new HTuple(),
                       hv_Label_COPY_INP_TMP, 0, "false", hv_TrackballCenterRow, hv_TrackballCenterCol,
                       hv_TBSize, hv_SelectedObject, hv_WindowCenteredRotation, hv_TBCenter);

                ho_ImageDump.Dispose();
                if (breakOut == false)
                {
                    HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                    //HDevWindowStack.SetActive(hv_WindowHandle);

                    HOperatorSet.DispObj(ho_ImageDump, hv_WindowHandle);

                    //HOperatorSet.CloseWindow(hv_WindowHandleBuffer);
                    //HOperatorSet.SetPart(hv_WindowHandle, hv_WPRow1, hv_WPColumn1, hv_WPRow2, hv_WPColumn2);
                    HOperatorSet.ClearScene3d(hv_Scene3D);
                }


                ho_Image.Dispose();
                ho_ImageDump.Dispose();
                showEnd = true;
                return;
            }
            catch (HalconException )
            {
                ho_Image.Dispose();
                ho_ImageDump.Dispose();
                showEnd = true;
                //throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Output
        // Short Description: Determine the optimum distance of the object to obtain a reasonable visualization 
        public void determine_optimum_pose_distance(HTuple hv_ObjectModel3DID, HTuple hv_CamParam,
            HTuple hv_ImageCoverage, HTuple hv_PoseIn, out HTuple hv_PoseOut)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_NumModels = null, hv_Rows = null;
            HTuple hv_Cols = null, hv_MinMinZ = null, hv_BB = null;
            HTuple hv_Seq = null, hv_DXMax = null, hv_DYMax = null;
            HTuple hv_DZMax = null, hv_Diameter = null, hv_ZAdd = null;
            HTuple hv_IBB = null, hv_BB0 = null, hv_BB1 = null, hv_BB2 = null;
            HTuple hv_BB3 = null, hv_BB4 = null, hv_BB5 = null, hv_X = null;
            HTuple hv_Y = null, hv_Z = null, hv_PoseInter = null, hv_HomMat3D = null;
            HTuple hv_CX = null, hv_CY = null, hv_CZ = null, hv_DR = null;
            HTuple hv_DC = null, hv_MaxDist = null, hv_HomMat3DRotate = new HTuple();
            HTuple hv_MinImageSize = null, hv_Zs = null, hv_ZDiff = null;
            HTuple hv_ScaleZ = null, hv_ZNew = null;
            // Initialize local and output iconic variables 
            //Determine the optimum distance of the object to obtain
            //a reasonable visualization
            //
            hv_NumModels = new HTuple(hv_ObjectModel3DID.TupleLength());
            hv_Rows = new HTuple();
            hv_Cols = new HTuple();
            hv_MinMinZ = 1e30;
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "bounding_box1", out hv_BB);
            //Calculate diameter over all objects to be visualized
            hv_Seq = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_DXMax = (((hv_BB.TupleSelect(hv_Seq + 3))).TupleMax()) - (((hv_BB.TupleSelect(
                hv_Seq))).TupleMin());
            hv_DYMax = (((hv_BB.TupleSelect(hv_Seq + 4))).TupleMax()) - (((hv_BB.TupleSelect(
                hv_Seq + 1))).TupleMin());
            hv_DZMax = (((hv_BB.TupleSelect(hv_Seq + 5))).TupleMax()) - (((hv_BB.TupleSelect(
                hv_Seq + 2))).TupleMin());
            hv_Diameter = ((((hv_DXMax * hv_DXMax) + (hv_DYMax * hv_DYMax)) + (hv_DZMax * hv_DZMax))).TupleSqrt()
                ;
            if ((int)(new HTuple(((((hv_BB.TupleAbs())).TupleSum())).TupleEqual(0.0))) != 0)
            {
                hv_BB = new HTuple();
                hv_BB = hv_BB.TupleConcat(-((new HTuple(HTuple.TupleRand(
                    3) * 1e-20)).TupleAbs()));
                hv_BB = hv_BB.TupleConcat((new HTuple(HTuple.TupleRand(
                    3) * 1e-20)).TupleAbs());
            }
            //Allow the visualization of single points or extremely small objects
            hv_ZAdd = 0.0;
            if ((int)(new HTuple(((hv_Diameter.TupleMax())).TupleLess(1e-10))) != 0)
            {
                hv_ZAdd = 0.01;
            }
            //Set extremely small diameters to 1e-10 to avoid CZ == 0.0, which would lead
            //to projection errors
            if ((int)(new HTuple(((hv_Diameter.TupleMin())).TupleLess(1e-10))) != 0)
            {
                hv_Diameter = hv_Diameter - (((((((hv_Diameter - 1e-10)).TupleSgn()) - 1)).TupleSgn()
                    ) * 1e-10);
            }
            hv_IBB = HTuple.TupleGenSequence(0, (new HTuple(hv_BB.TupleLength())) - 1, 6);
            hv_BB0 = hv_BB.TupleSelect(hv_IBB);
            hv_BB1 = hv_BB.TupleSelect(hv_IBB + 1);
            hv_BB2 = hv_BB.TupleSelect(hv_IBB + 2);
            hv_BB3 = hv_BB.TupleSelect(hv_IBB + 3);
            hv_BB4 = hv_BB.TupleSelect(hv_IBB + 4);
            hv_BB5 = hv_BB.TupleSelect(hv_IBB + 5);
            hv_X = new HTuple();
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_X = hv_X.TupleConcat(hv_BB0);
            hv_X = hv_X.TupleConcat(hv_BB3);
            hv_Y = new HTuple();
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB1);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Y = hv_Y.TupleConcat(hv_BB4);
            hv_Z = new HTuple();
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB2);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_Z = hv_Z.TupleConcat(hv_BB5);
            hv_PoseInter = hv_PoseIn.TupleReplace(2, (-(hv_Z.TupleMin())) + (2 * (hv_Diameter.TupleMax()
                )));
            HOperatorSet.PoseToHomMat3d(hv_PoseInter, out hv_HomMat3D);
            //Determine the maximum extention of the projection
            HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_X, hv_Y, hv_Z, out hv_CX, out hv_CY,
                out hv_CZ);
            HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, hv_CamParam, out hv_Rows, out hv_Cols);
            hv_MinMinZ = hv_CZ.TupleMin();
            hv_DR = hv_Rows - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 3));
            hv_DC = hv_Cols - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 4));
            hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
            hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
            hv_MaxDist = (((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt();
            //
            if ((int)(new HTuple(hv_MaxDist.TupleLess(1e-10))) != 0)
            {
                //If the object has no extension in the above projection (looking along
                //a line), we determine the extension of the object in a rotated view
                HOperatorSet.HomMat3dRotateLocal(hv_HomMat3D, (new HTuple(90)).TupleRad(),
                    "x", out hv_HomMat3DRotate);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3DRotate, hv_X, hv_Y, hv_Z, out hv_CX,
                    out hv_CY, out hv_CZ);
                HOperatorSet.Project3dPoint(hv_CX, hv_CY, hv_CZ, hv_CamParam, out hv_Rows,
                    out hv_Cols);
                hv_DR = hv_Rows - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                    )) - 3));
                hv_DC = hv_Cols - (hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                    )) - 4));
                hv_DR = (hv_DR.TupleMax()) - (hv_DR.TupleMin());
                hv_DC = (hv_DC.TupleMax()) - (hv_DC.TupleMin());
                hv_MaxDist = ((hv_MaxDist.TupleConcat((((hv_DR * hv_DR) + (hv_DC * hv_DC))).TupleSqrt()
                    ))).TupleMax();
            }
            //
            hv_MinImageSize = ((((hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 2))).TupleConcat(hv_CamParam.TupleSelect((new HTuple(hv_CamParam.TupleLength()
                )) - 1)))).TupleMin();
            //
            hv_Z = hv_PoseInter[2];
            hv_Zs = hv_MinMinZ.Clone();
            hv_ZDiff = hv_Z - hv_Zs;
            hv_ScaleZ = hv_MaxDist / (((0.5 * hv_MinImageSize) * hv_ImageCoverage) * 2.0);
            hv_ZNew = ((hv_ScaleZ * hv_Zs) + hv_ZDiff) + hv_ZAdd;
            hv_PoseOut = hv_PoseInter.TupleReplace(2, hv_ZNew);
            //

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Reflect the pose change that was introduced by the user by moving the mouse 
        public void analyze_graph_event(HObject ho_BackgroundImage, HTuple hv_MouseMapping,
            HTuple hv_Button, HTuple hv_Row, HTuple hv_Column, HTuple hv_WindowHandle, HTuple hv_WindowHandleBuffer,
            HTuple hv_VirtualTrackball, HTuple hv_TrackballSize, HTuple hv_SelectedObjectIn,
            HTuple hv_Scene3D, HTuple hv_AlphaOrig, HTuple hv_ObjectModel3DID, HTuple hv_CamParam,
            HTuple hv_Labels, HTuple hv_Title, HTuple hv_Information, HTuple hv_GenParamName,
            HTuple hv_GenParamValue, HTuple hv_PosesIn, HTuple hv_ButtonHoldIn, HTuple hv_TBCenter,
            HTuple hv_TBSize, HTuple hv_WindowCenteredRotationlIn, HTuple hv_MaxNumModels,
            out HTuple hv_PosesOut, out HTuple hv_SelectedObjectOut, out HTuple hv_ButtonHoldOut,
            out HTuple hv_WindowCenteredRotationOut)
        {




            // Local iconic variables 

            HObject ho_ImageDump = null;

            // Local control variables 

            HTuple ExpTmpLocalVar_gIsSinglePose = new HTuple();
            HTuple hv_VisualizeTB = null, hv_InvLog2 = null, hv_Seconds = new HTuple();
            HTuple hv_ModelIndex = new HTuple(), hv_Exception1 = new HTuple();
            HTuple hv_HomMat3DIdentity = new HTuple(), hv_NumModels = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_MinImageSize = new HTuple(), hv_TrackballRadiusPixel = new HTuple();
            HTuple hv_TrackballCenterRow = new HTuple(), hv_TrackballCenterCol = new HTuple();
            HTuple hv_NumChannels = new HTuple(), hv_ColorImage = new HTuple();
            HTuple hv_BAnd = new HTuple(), hv_SensFactor = new HTuple();
            HTuple hv_IsButtonTrans = new HTuple(), hv_IsButtonRot = new HTuple();
            HTuple hv_IsButtonDist = new HTuple(), hv_MRow1 = new HTuple();
            HTuple hv_MCol1 = new HTuple(), hv_ButtonLoop = new HTuple();
            HTuple hv_MRow2 = new HTuple(), hv_MCol2 = new HTuple();
            HTuple hv_PX = new HTuple(), hv_PY = new HTuple(), hv_PZ = new HTuple();
            HTuple hv_QX1 = new HTuple(), hv_QY1 = new HTuple(), hv_QZ1 = new HTuple();
            HTuple hv_QX2 = new HTuple(), hv_QY2 = new HTuple(), hv_QZ2 = new HTuple();
            HTuple hv_Len = new HTuple(), hv_Dist = new HTuple(), hv_Translate = new HTuple();
            HTuple hv_Index = new HTuple(), hv_PoseIn = new HTuple();
            HTuple hv_HomMat3DIn = new HTuple(), hv_HomMat3DOut = new HTuple();
            HTuple hv_PoseOut = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Sequence = new HTuple(), hv_Mod = new HTuple();
            HTuple hv_SequenceReal = new HTuple(), hv_Sequence2Int = new HTuple();
            HTuple hv_Selected = new HTuple(), hv_InvSelected = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_DRow = new HTuple();
            HTuple hv_TranslateZ = new HTuple(), hv_MX1 = new HTuple();
            HTuple hv_MY1 = new HTuple(), hv_MX2 = new HTuple(), hv_MY2 = new HTuple();
            HTuple hv_RelQuaternion = new HTuple(), hv_HomMat3DRotRel = new HTuple();
            HTuple hv_HomMat3DInTmp1 = new HTuple(), hv_HomMat3DInTmp = new HTuple();
            HTuple hv_PosesOut2 = new HTuple();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_PosesIn_COPY_INP_TMP = hv_PosesIn.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_TBCenter_COPY_INP_TMP = hv_TBCenter.Clone();
            HTuple hv_TBSize_COPY_INP_TMP = hv_TBSize.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageDump);
            try
            {
                //This procedure reflects
                //- the pose change that was introduced by the user by
                //  moving the mouse
                //- the selection of a single object
                //
                //global tuple gIsSinglePose
                //
                hv_ButtonHoldOut = hv_ButtonHoldIn.Clone();
                hv_PosesOut = hv_PosesIn_COPY_INP_TMP.Clone();
                hv_SelectedObjectOut = hv_SelectedObjectIn.Clone();
                hv_WindowCenteredRotationOut = hv_WindowCenteredRotationlIn.Clone();
                hv_VisualizeTB = new HTuple(((hv_SelectedObjectOut.TupleMax())).TupleNotEqual(
                    0));
                hv_InvLog2 = 1.0 / ((new HTuple(2)).TupleLog());
                //
                if ((int)(new HTuple(hv_Button.TupleEqual(hv_MouseMapping.TupleSelect(6)))) != 0)
                {
                    if ((int)(hv_ButtonHoldOut) != 0)
                    {
                        ho_ImageDump.Dispose();

                        return;
                    }
                    //Ctrl (16) + Alt (32) + left mouse button (1) => Toggle rotation center position
                    //If WindowCenteredRotation is not 1, set it to 1, otherwise, set it to 2
                    HOperatorSet.CountSeconds(out hv_Seconds);
                    if ((int)(new HTuple(hv_WindowCenteredRotationOut.TupleEqual(1))) != 0)
                    {
                        hv_WindowCenteredRotationOut = 2;
                    }
                    else
                    {
                        hv_WindowCenteredRotationOut = 1;
                    }
                    hv_ButtonHoldOut = 1;
                    ho_ImageDump.Dispose();

                    return;
                }
                if ((int)((new HTuple(hv_Button.TupleEqual(hv_MouseMapping.TupleSelect(5)))).TupleAnd(
                    new HTuple((new HTuple(hv_ObjectModel3DID.TupleLength())).TupleLessEqual(
                    hv_MaxNumModels)))) != 0)
                {
                    if ((int)(hv_ButtonHoldOut) != 0)
                    {
                        ho_ImageDump.Dispose();

                        return;
                    }
                    //Ctrl (16) + left mouse button (1) => Select an object
                    try
                    {
                        HOperatorSet.SetScene3dParam(hv_Scene3D, "object_index_persistence", "true");
                        HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                        HOperatorSet.GetDisplayScene3dInfo(hv_WindowHandleBuffer, hv_Scene3D, hv_Row_COPY_INP_TMP,
                            hv_Column_COPY_INP_TMP, "object_index", out hv_ModelIndex);
                        HOperatorSet.SetScene3dParam(hv_Scene3D, "object_index_persistence", "false");
                    }
                    // catch (Exception1) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception1);
                        //* NO OpenGL, no selection possible
                        ho_ImageDump.Dispose();

                        return;
                    }
                    if ((int)(new HTuple(hv_ModelIndex.TupleEqual(-1))) != 0)
                    {
                        //Background click:
                        if ((int)(new HTuple(((hv_SelectedObjectOut.TupleSum())).TupleEqual(new HTuple(hv_SelectedObjectOut.TupleLength()
                            )))) != 0)
                        {
                            //If all objects are already selected, deselect all
                            hv_SelectedObjectOut = HTuple.TupleGenConst(new HTuple(hv_ObjectModel3DID.TupleLength()
                                ), 0);
                        }
                        else
                        {
                            //Otherwise select all
                            hv_SelectedObjectOut = HTuple.TupleGenConst(new HTuple(hv_ObjectModel3DID.TupleLength()
                                ), 1);
                        }
                    }
                    else
                    {
                        //Object click:
                        if (hv_SelectedObjectOut == null)
                            hv_SelectedObjectOut = new HTuple();
                        hv_SelectedObjectOut[hv_ModelIndex] = ((hv_SelectedObjectOut.TupleSelect(
                            hv_ModelIndex))).TupleNot();
                    }
                    hv_ButtonHoldOut = 1;
                }
                else
                {
                    //Change the pose
                    HOperatorSet.HomMat3dIdentity(out hv_HomMat3DIdentity);
                    hv_NumModels = new HTuple(hv_ObjectModel3DID.TupleLength());
                    hv_Width = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 2];
                    hv_Height = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 1];
                    hv_MinImageSize = ((hv_Width.TupleConcat(hv_Height))).TupleMin();
                    hv_TrackballRadiusPixel = (hv_TrackballSize * hv_MinImageSize) / 2.0;
                    //Set trackball fixed in the center of the window
                    hv_TrackballCenterRow = hv_Height / 2;
                    hv_TrackballCenterCol = hv_Width / 2;
                    if ((int)(new HTuple((new HTuple(hv_ObjectModel3DID.TupleLength())).TupleLess(
                        hv_MaxNumModels))) != 0)
                    {
                        if ((int)(new HTuple(hv_WindowCenteredRotationOut.TupleEqual(1))) != 0)
                        {
                            get_trackball_center_fixed(hv_SelectedObjectIn, hv_TrackballCenterRow,
                                hv_TrackballCenterCol, hv_TrackballRadiusPixel, hv_Scene3D, hv_ObjectModel3DID,
                                hv_PosesIn_COPY_INP_TMP, hv_WindowHandleBuffer, hv_CamParam, hv_GenParamName,
                                hv_GenParamValue, out hv_TBCenter_COPY_INP_TMP, out hv_TBSize_COPY_INP_TMP);
                        }
                        else
                        {
                            get_trackball_center(hv_SelectedObjectIn, hv_TrackballRadiusPixel, hv_ObjectModel3DID,
                                hv_PosesIn_COPY_INP_TMP, out hv_TBCenter_COPY_INP_TMP, out hv_TBSize_COPY_INP_TMP);
                        }
                    }
                    if ((int)((new HTuple(((hv_SelectedObjectOut.TupleMin())).TupleEqual(0))).TupleAnd(
                        new HTuple(((hv_SelectedObjectOut.TupleMax())).TupleEqual(1)))) != 0)
                    {
                        //At this point, multiple objects do not necessary have the same
                        //pose any more. Consequently, we have to return a tuple of poses
                        //as output of visualize_object_model_3d
                        ExpTmpLocalVar_gIsSinglePose = 0;
                        ExpSetGlobalVar_gIsSinglePose(ExpTmpLocalVar_gIsSinglePose);
                    }
                    HOperatorSet.CountChannels(ho_BackgroundImage, out hv_NumChannels);
                    hv_ColorImage = new HTuple(hv_NumChannels.TupleEqual(3));
                    //Alt (32) => lower sensitivity
                    HOperatorSet.TupleRsh(hv_Button, 5, out hv_BAnd);
                    if ((int)(hv_BAnd % 2) != 0)
                    {
                        hv_SensFactor = 0.1;
                    }
                    else
                    {
                        hv_SensFactor = 1.0;
                    }
                    hv_IsButtonTrans = (new HTuple(((hv_MouseMapping.TupleSelect(0))).TupleEqual(
                        hv_Button))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(0)))).TupleEqual(
                        hv_Button)));
                    hv_IsButtonRot = (new HTuple(((hv_MouseMapping.TupleSelect(1))).TupleEqual(
                        hv_Button))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(1)))).TupleEqual(
                        hv_Button)));
                    hv_IsButtonDist = (new HTuple((new HTuple((new HTuple((new HTuple((new HTuple(((hv_MouseMapping.TupleSelect(
                        2))).TupleEqual(hv_Button))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(
                        2)))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((hv_MouseMapping.TupleSelect(
                        3))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(
                        3)))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((hv_MouseMapping.TupleSelect(
                        4))).TupleEqual(hv_Button))))).TupleOr(new HTuple(((32 + (hv_MouseMapping.TupleSelect(
                        4)))).TupleEqual(hv_Button)));
                    if ((int)(hv_IsButtonTrans) != 0)
                    {
                        //Translate in XY-direction
                        hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                        hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                        while ((int)(hv_IsButtonTrans) != 0)
                        {
                            showEnd = false;
                            try
                            {
                                HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row_COPY_INP_TMP,
                                    out hv_Column_COPY_INP_TMP, out hv_ButtonLoop);
                                //         disp_message(hv_WindowHandle, "X:" + hv_Column_COPY_INP_TMP + " Y:" + hv_Row_COPY_INP_TMP, "window", 0, 0,
                                //ExpGetGlobalVar_gTitleDecor().TupleSelect(0), ExpGetGlobalVar_gTitleDecor().TupleSelect(
                                //1));
                                hv_IsButtonTrans = new HTuple(hv_ButtonLoop.TupleEqual(hv_Button));
                                hv_MRow2 = hv_MRow1 + ((hv_Row_COPY_INP_TMP - hv_MRow1) * hv_SensFactor);
                                hv_MCol2 = hv_MCol1 + ((hv_Column_COPY_INP_TMP - hv_MCol1) * hv_SensFactor);
                                HOperatorSet.GetLineOfSight(hv_MRow1, hv_MCol1, hv_CamParam, out hv_PX,
                                    out hv_PY, out hv_PZ, out hv_QX1, out hv_QY1, out hv_QZ1);
                                HOperatorSet.GetLineOfSight(hv_MRow2, hv_MCol2, hv_CamParam, out hv_PX,
                                    out hv_PY, out hv_PZ, out hv_QX2, out hv_QY2, out hv_QZ2);
                                hv_Len = ((((hv_QX1 * hv_QX1) + (hv_QY1 * hv_QY1)) + (hv_QZ1 * hv_QZ1))).TupleSqrt()
                                    ;
                                hv_Dist = (((((hv_TBCenter_COPY_INP_TMP.TupleSelect(0)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    0))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(1)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    1)))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(2)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    2))))).TupleSqrt();
                                hv_Translate = ((((((hv_QX2 - hv_QX1)).TupleConcat(hv_QY2 - hv_QY1))).TupleConcat(
                                    hv_QZ2 - hv_QZ1)) * hv_Dist) / hv_Len;
                                hv_PosesOut = new HTuple();
                                if ((int)(new HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels))) != 0)
                                {
                                    HTuple end_val110 = hv_NumModels - 1;
                                    HTuple step_val110 = 1;
                                    for (hv_Index = 0; hv_Index.Continue(end_val110, step_val110); hv_Index = hv_Index.TupleAdd(step_val110))
                                    {
                                        hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index * 7,
                                            (hv_Index * 7) + 6);
                                        if ((int)(hv_SelectedObjectOut.TupleSelect(hv_Index)) != 0)
                                        {
                                            HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_Translate.TupleSelect(
                                                0), hv_Translate.TupleSelect(1), hv_Translate.TupleSelect(
                                                2), out hv_HomMat3DOut);
                                            HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                            HOperatorSet.SetScene3dInstancePose(hv_Scene3D, hv_Index, hv_PoseOut);
                                        }
                                        else
                                        {
                                            hv_PoseOut = hv_PoseIn.Clone();
                                        }
                                        hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut);
                                    }
                                }
                                else
                                {
                                    HOperatorSet.TupleFind(hv_SelectedObjectOut, 1, out hv_Indices);
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange((hv_Indices.TupleSelect(
                                        0)) * 7, ((hv_Indices.TupleSelect(0)) * 7) + 6);
                                    HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_Translate.TupleSelect(
                                        0), hv_Translate.TupleSelect(1), hv_Translate.TupleSelect(2),
                                        out hv_HomMat3DOut);
                                    HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                                    HOperatorSet.TupleMod(hv_Sequence, 7, out hv_Mod);
                                    hv_SequenceReal = HTuple.TupleGenSequence(0, hv_NumModels - (1.0 / 7.0),
                                        1.0 / 7.0);
                                    hv_Sequence2Int = hv_SequenceReal.TupleInt();
                                    HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, out hv_Selected);
                                    hv_InvSelected = 1 - hv_Selected;
                                    HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, out hv_PosesOut);
                                    hv_PosesOut = (hv_PosesOut * hv_Selected) + (hv_PosesIn_COPY_INP_TMP * hv_InvSelected);
                                    HOperatorSet.SetScene3dInstancePose(hv_Scene3D, HTuple.TupleGenSequence(
                                        0, hv_NumModels - 1, 1), hv_PosesOut);
                                }

                                dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_Scene3D,
                                        hv_AlphaOrig, hv_ObjectModel3DID, hv_GenParamName, hv_GenParamValue,
                                        hv_CamParam, hv_PosesOut, hv_ColorImage, hv_Title, hv_Information,
                                        hv_Labels, hv_VisualizeTB, "true", hv_TrackballCenterRow, hv_TrackballCenterCol,
                                        hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, new HTuple(hv_WindowCenteredRotationOut.TupleEqual(
                                        1)), hv_TBCenter_COPY_INP_TMP);

                                ho_ImageDump.Dispose();
                                HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                                HDevWindowStack.SetActive(hv_WindowHandle);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                                }
                                //
                                hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                                hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                                hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone();
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException1)
                            {
                                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                                //Keep waiting
                            }

                            if (breakOut)
                            {
                                break;
                            }
                        }
                    }
                    else if ((int)(hv_IsButtonDist) != 0)
                    {
                        //Change the Z distance
                        hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                        while ((int)(hv_IsButtonDist) != 0)
                        {
                            showEnd = false;
                            try
                            {
                                HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row_COPY_INP_TMP,
                                    out hv_Column_COPY_INP_TMP, out hv_ButtonLoop);
                                //         disp_message(hv_WindowHandle, "X:" + hv_Column_COPY_INP_TMP + " Y:" + hv_Row_COPY_INP_TMP, "window", 0, 0,
                                //ExpGetGlobalVar_gTitleDecor().TupleSelect(0), ExpGetGlobalVar_gTitleDecor().TupleSelect(
                                //1));
                                hv_IsButtonDist = new HTuple(hv_ButtonLoop.TupleEqual(hv_Button));
                                hv_MRow2 = hv_Row_COPY_INP_TMP.Clone();
                                hv_DRow = hv_MRow2 - hv_MRow1;
                                hv_Dist = (((((hv_TBCenter_COPY_INP_TMP.TupleSelect(0)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    0))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(1)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    1)))) + ((hv_TBCenter_COPY_INP_TMP.TupleSelect(2)) * (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    2))))).TupleSqrt();
                                hv_TranslateZ = (((-hv_Dist) * hv_DRow) * 0.003) * hv_SensFactor;
                                if (hv_TBCenter_COPY_INP_TMP == null)
                                    hv_TBCenter_COPY_INP_TMP = new HTuple();
                                hv_TBCenter_COPY_INP_TMP[2] = (hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                    2)) + hv_TranslateZ;
                                hv_PosesOut = new HTuple();
                                if ((int)(new HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels))) != 0)
                                {
                                    HTuple end_val164 = hv_NumModels - 1;
                                    HTuple step_val164 = 1;
                                    for (hv_Index = 0; hv_Index.Continue(end_val164, step_val164); hv_Index = hv_Index.TupleAdd(step_val164))
                                    {
                                        hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index * 7,
                                            (hv_Index * 7) + 6);
                                        if ((int)(hv_SelectedObjectOut.TupleSelect(hv_Index)) != 0)
                                        {
                                            //Transform the whole scene or selected object only
                                            HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, 0, 0, hv_TranslateZ,
                                                out hv_HomMat3DOut);
                                            HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                            HOperatorSet.SetScene3dInstancePose(hv_Scene3D, hv_Index, hv_PoseOut);
                                        }
                                        else
                                        {
                                            hv_PoseOut = hv_PoseIn.Clone();
                                        }
                                        hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut);
                                    }
                                }
                                else
                                {
                                    HOperatorSet.TupleFind(hv_SelectedObjectOut, 1, out hv_Indices);
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange((hv_Indices.TupleSelect(
                                        0)) * 7, ((hv_Indices.TupleSelect(0)) * 7) + 6);
                                    HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, 0, 0, hv_TranslateZ,
                                        out hv_HomMat3DOut);
                                    HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                                    HOperatorSet.TupleMod(hv_Sequence, 7, out hv_Mod);
                                    hv_SequenceReal = HTuple.TupleGenSequence(0, hv_NumModels - (1.0 / 7.0),
                                        1.0 / 7.0);
                                    hv_Sequence2Int = hv_SequenceReal.TupleInt();
                                    HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, out hv_Selected);
                                    hv_InvSelected = 1 - hv_Selected;
                                    HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, out hv_PosesOut);
                                    hv_PosesOut = (hv_PosesOut * hv_Selected) + (hv_PosesIn_COPY_INP_TMP * hv_InvSelected);
                                    HOperatorSet.SetScene3dInstancePose(hv_Scene3D, HTuple.TupleGenSequence(
                                        0, hv_NumModels - 1, 1), hv_PosesOut);
                                }

                                dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_Scene3D,
                                        hv_AlphaOrig, hv_ObjectModel3DID, hv_GenParamName, hv_GenParamValue,
                                        hv_CamParam, hv_PosesOut, hv_ColorImage, hv_Title, hv_Information,
                                        hv_Labels, hv_VisualizeTB, "true", hv_TrackballCenterRow, hv_TrackballCenterCol,
                                        hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, hv_WindowCenteredRotationOut,
                                        hv_TBCenter_COPY_INP_TMP);

                                ho_ImageDump.Dispose();
                                HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                                HDevWindowStack.SetActive(hv_WindowHandle);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                                }
                                //
                                hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                                hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone();
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException1)
                            {
                                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                                //Keep waiting
                            }
                        }
                    }
                    else if ((int)(hv_IsButtonRot) != 0)
                    {
                        //Rotate the object
                        hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                        hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                        while ((int)(hv_IsButtonRot) != 0)
                        {
                            showEnd = false;
                            try
                            {
                                HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out hv_Row_COPY_INP_TMP,
                                    out hv_Column_COPY_INP_TMP, out hv_ButtonLoop);
                                //         disp_message(hv_WindowHandle, "X:" + hv_Column_COPY_INP_TMP + " Y:" + hv_Row_COPY_INP_TMP, "window", 0, 0,
                                //ExpGetGlobalVar_gTitleDecor().TupleSelect(0), ExpGetGlobalVar_gTitleDecor().TupleSelect(
                                //1));
                                hv_IsButtonRot = new HTuple(hv_ButtonLoop.TupleEqual(hv_Button));
                                hv_MRow2 = hv_Row_COPY_INP_TMP.Clone();
                                hv_MCol2 = hv_Column_COPY_INP_TMP.Clone();
                                //Transform the pixel coordinates to relative image coordinates
                                hv_MX1 = (hv_TrackballCenterCol - hv_MCol1) / (0.5 * hv_MinImageSize);
                                hv_MY1 = (hv_TrackballCenterRow - hv_MRow1) / (0.5 * hv_MinImageSize);
                                hv_MX2 = (hv_TrackballCenterCol - hv_MCol2) / (0.5 * hv_MinImageSize);
                                hv_MY2 = (hv_TrackballCenterRow - hv_MRow2) / (0.5 * hv_MinImageSize);
                                //Compute the quaternion rotation that corresponds to the mouse
                                //movement
                                trackball(hv_MX1, hv_MY1, hv_MX2, hv_MY2, hv_VirtualTrackball, hv_TrackballSize,
                                    hv_SensFactor, out hv_RelQuaternion);
                                //Transform the quaternion to a rotation matrix
                                HOperatorSet.QuatToHomMat3d(hv_RelQuaternion, out hv_HomMat3DRotRel);
                                hv_PosesOut = new HTuple();
                                if ((int)(new HTuple(hv_NumModels.TupleLessEqual(hv_MaxNumModels))) != 0)
                                {
                                    HTuple end_val226 = hv_NumModels - 1;
                                    HTuple step_val226 = 1;
                                    for (hv_Index = 0; hv_Index.Continue(end_val226, step_val226); hv_Index = hv_Index.TupleAdd(step_val226))
                                    {
                                        hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange(hv_Index * 7,
                                            (hv_Index * 7) + 6);
                                        if ((int)(hv_SelectedObjectOut.TupleSelect(hv_Index)) != 0)
                                        {
                                            //Transform the whole scene or selected object only
                                            HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                0)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(1)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                2)), out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dCompose(hv_HomMat3DRotRel, hv_HomMat3DIn,
                                                out hv_HomMat3DIn);
                                            HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                0), hv_TBCenter_COPY_INP_TMP.TupleSelect(1), hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                                2), out hv_HomMat3DOut);
                                            HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                            HOperatorSet.SetScene3dInstancePose(hv_Scene3D, hv_Index, hv_PoseOut);
                                        }
                                        else
                                        {
                                            hv_PoseOut = hv_PoseIn.Clone();
                                        }
                                        hv_PosesOut = hv_PosesOut.TupleConcat(hv_PoseOut);
                                    }
                                }
                                else
                                {
                                    HOperatorSet.TupleFind(hv_SelectedObjectOut, 1, out hv_Indices);
                                    hv_PoseIn = hv_PosesIn_COPY_INP_TMP.TupleSelectRange((hv_Indices.TupleSelect(
                                        0)) * 7, ((hv_Indices.TupleSelect(0)) * 7) + 6);
                                    HOperatorSet.PoseToHomMat3d(hv_PoseIn, out hv_HomMat3DIn);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DIn, -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        0)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(1)), -(hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        2)), out hv_HomMat3DInTmp1);
                                    HOperatorSet.HomMat3dCompose(hv_HomMat3DRotRel, hv_HomMat3DInTmp1,
                                        out hv_HomMat3DInTmp);
                                    HOperatorSet.HomMat3dTranslate(hv_HomMat3DInTmp, hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        0), hv_TBCenter_COPY_INP_TMP.TupleSelect(1), hv_TBCenter_COPY_INP_TMP.TupleSelect(
                                        2), out hv_HomMat3DOut);
                                    HOperatorSet.HomMat3dToPose(hv_HomMat3DOut, out hv_PoseOut);
                                    hv_Sequence = HTuple.TupleGenSequence(0, (hv_NumModels * 7) - 1, 1);
                                    HOperatorSet.TupleMod(hv_Sequence, 7, out hv_Mod);
                                    hv_SequenceReal = HTuple.TupleGenSequence(0, hv_NumModels - (1.0 / 7.0),
                                        1.0 / 7.0);
                                    hv_Sequence2Int = hv_SequenceReal.TupleInt();
                                    HOperatorSet.TupleSelect(hv_SelectedObjectOut, hv_Sequence2Int, out hv_Selected);
                                    hv_InvSelected = 1 - hv_Selected;
                                    HOperatorSet.TupleSelect(hv_PoseOut, hv_Mod, out hv_PosesOut);
                                    hv_PosesOut2 = (hv_PosesOut * hv_Selected) + (hv_PosesIn_COPY_INP_TMP * hv_InvSelected);
                                    hv_PosesOut = hv_PosesOut2.Clone();
                                    HOperatorSet.SetScene3dInstancePose(hv_Scene3D, HTuple.TupleGenSequence(
                                        0, hv_NumModels - 1, 1), hv_PosesOut);
                                }

                                dump_image_output(ho_BackgroundImage, hv_WindowHandleBuffer, hv_Scene3D,
                                        hv_AlphaOrig, hv_ObjectModel3DID, hv_GenParamName, hv_GenParamValue,
                                        hv_CamParam, hv_PosesOut, hv_ColorImage, hv_Title, hv_Information,
                                        hv_Labels, hv_VisualizeTB, "true", hv_TrackballCenterRow, hv_TrackballCenterCol,
                                        hv_TBSize_COPY_INP_TMP, hv_SelectedObjectOut, hv_WindowCenteredRotationOut,
                                        hv_TBCenter_COPY_INP_TMP);

                                ho_ImageDump.Dispose();
                                HOperatorSet.DumpWindowImage(out ho_ImageDump, hv_WindowHandleBuffer);
                                HDevWindowStack.SetActive(hv_WindowHandle);
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ImageDump, HDevWindowStack.GetActive());
                                }
                                //
                                hv_MRow1 = hv_Row_COPY_INP_TMP.Clone();
                                hv_MCol1 = hv_Column_COPY_INP_TMP.Clone();
                                hv_PosesIn_COPY_INP_TMP = hv_PosesOut.Clone();
                            }
                            // catch (Exception) 
                            catch (HalconException HDevExpDefaultException1)
                            {

                                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                                //Keep waiting
                            }
                            if (breakOut)
                            {
                                break;
                            }
                        }
                    }
                    hv_PosesOut = hv_PosesIn_COPY_INP_TMP.Clone();
                }
                ho_ImageDump.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageDump.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Output
        public void disp_title_and_information(HTuple hv_WindowHandle, HTuple hv_Title,
            HTuple hv_Information)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_WinRow = null, hv_WinColumn = null;
            HTuple hv_WinWidth = null, hv_WinHeight = null, hv_NumTitleLines = null;
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_TextWidth = new HTuple(), hv_NumInfoLines = null;
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Information_COPY_INP_TMP = hv_Information.Clone();
            HTuple hv_Title_COPY_INP_TMP = hv_Title.Clone();

            // Initialize local and output iconic variables 
            //global tuple gInfoDecor
            //global tuple gInfoPos
            //global tuple gTitlePos
            //global tuple gTitleDecor
            //
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_WinRow, out hv_WinColumn,
                out hv_WinWidth, out hv_WinHeight);
            hv_Title_COPY_INP_TMP = ((("" + hv_Title_COPY_INP_TMP) + "")).TupleSplit("\n");
            hv_NumTitleLines = new HTuple(hv_Title_COPY_INP_TMP.TupleLength());
            if ((int)(new HTuple(hv_NumTitleLines.TupleGreater(0))) != 0)
            {
                hv_Row = 12;
                if ((int)(new HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual("UpperLeft"))) != 0)
                {
                    hv_Column = 12;
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual("UpperCenter"))) != 0)
                {
                    max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP, out hv_TextWidth);
                    hv_Column = (hv_WinWidth / 2) - (hv_TextWidth / 2);
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gTitlePos().TupleEqual("UpperRight"))) != 0)
                {
                    if ((int)(new HTuple(((ExpGetGlobalVar_gTitleDecor().TupleSelect(1))).TupleEqual(
                        "true"))) != 0)
                    {
                        max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP + "  ", out hv_TextWidth);
                    }
                    else
                    {
                        max_line_width(hv_WindowHandle, hv_Title_COPY_INP_TMP, out hv_TextWidth);
                    }
                    hv_Column = (hv_WinWidth - hv_TextWidth) - 10;
                }
                else
                {
                    //Unknown position!
                    // stop(); only in hdevelop
                }
                disp_message(hv_WindowHandle, hv_Title_COPY_INP_TMP, "window", hv_Row, hv_Column,
                    ExpGetGlobalVar_gTitleDecor().TupleSelect(0), ExpGetGlobalVar_gTitleDecor().TupleSelect(
                    1));
            }
            hv_Information_COPY_INP_TMP = ((("" + hv_Information_COPY_INP_TMP) + "")).TupleSplit(
                "\n");
            hv_NumInfoLines = new HTuple(hv_Information_COPY_INP_TMP.TupleLength());
            if ((int)(new HTuple(hv_NumInfoLines.TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual("UpperLeft"))) != 0)
                {
                    hv_Row = 12;
                    hv_Column = 12;
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual("UpperRight"))) != 0)
                {
                    if ((int)(new HTuple(((ExpGetGlobalVar_gInfoDecor().TupleSelect(1))).TupleEqual(
                        "true"))) != 0)
                    {
                        max_line_width(hv_WindowHandle, hv_Information_COPY_INP_TMP + "  ", out hv_TextWidth);
                    }
                    else
                    {
                        max_line_width(hv_WindowHandle, hv_Information_COPY_INP_TMP, out hv_TextWidth);
                    }
                    hv_Row = 12;
                    hv_Column = (hv_WinWidth - hv_TextWidth) - 12;
                }
                else if ((int)(new HTuple(ExpGetGlobalVar_gInfoPos().TupleEqual("LowerLeft"))) != 0)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_Information_COPY_INP_TMP,
                        out hv_Ascent, out hv_Descent, out hv_Width, out hv_Height);
                    hv_Row = (hv_WinHeight - (hv_NumInfoLines * hv_Height)) - 12;
                    hv_Column = 12;
                }
                else
                {
                    //Unknown position!
                    // stop(); only in hdevelop
                }
                disp_message(hv_WindowHandle, hv_Information_COPY_INP_TMP, "window", hv_Row,
                    hv_Column, ExpGetGlobalVar_gInfoDecor().TupleSelect(0), ExpGetGlobalVar_gInfoDecor().TupleSelect(
                    1));
            }
            //

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Renders 3d object models in a buffer window. 
        public void dump_image_output(HObject ho_BackgroundImage, HTuple hv_WindowHandleBuffer,
            HTuple hv_Scene3D, HTuple hv_AlphaOrig, HTuple hv_ObjectModel3DID, HTuple hv_GenParamName,
            HTuple hv_GenParamValue, HTuple hv_CamParam, HTuple hv_Poses, HTuple hv_ColorImage,
            HTuple hv_Title, HTuple hv_Information, HTuple hv_Labels, HTuple hv_VisualizeTrackball,
            HTuple hv_DisplayContinueButton, HTuple hv_TrackballCenterRow, HTuple hv_TrackballCenterCol,
            HTuple hv_TrackballRadiusPixel, HTuple hv_SelectedObject, HTuple hv_VisualizeRotationCenter,
            HTuple hv_RotationCenter)
        {



            // Local iconic variables 

            HObject ho_ModelContours = null, ho_Image;
            HObject ho_TrackballContour = null, ho_CrossRotCenter = null;

            // Local control variables 

            HTuple ExpTmpLocalVar_gUsesOpenGL = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Position = new HTuple(), hv_PosIdx = new HTuple();
            HTuple hv_Substrings = new HTuple(), hv_I = new HTuple();
            HTuple hv_HasExtended = new HTuple(), hv_ExtendedAttributeNames = new HTuple();
            HTuple hv_Matches = new HTuple(), hv_Exception1 = new HTuple();
            HTuple hv_DeselectedIdx = new HTuple(), hv_DeselectedName = new HTuple();
            HTuple hv_DeselectedValue = new HTuple(), hv_Pose = new HTuple();
            HTuple hv_HomMat3D = new HTuple(), hv_Center = new HTuple();
            HTuple hv_CenterCamX = new HTuple(), hv_CenterCamY = new HTuple();
            HTuple hv_CenterCamZ = new HTuple(), hv_CenterRow = new HTuple();
            HTuple hv_CenterCol = new HTuple(), hv_Label = new HTuple();
            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
            HTuple hv_TextWidth = new HTuple(), hv_TextHeight = new HTuple();
            HTuple hv_RotCenterRow = new HTuple(), hv_RotCenterCol = new HTuple();
            HTuple hv_Orientation = new HTuple(), hv_Colors = new HTuple();
            HTuple hv_RotationCenter_COPY_INP_TMP = hv_RotationCenter.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_TrackballContour);
            HOperatorSet.GenEmptyObj(out ho_CrossRotCenter);
            try
            {
                //global tuple gAlphaDeselected
                //global tuple gTerminationButtonLabel
                //global tuple gDispObjOffset
                //global tuple gLabelsDecor
                //global tuple gUsesOpenGL
                //
                //Display background image
                HSystem.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(hv_WindowHandleBuffer);
                if ((int)(hv_ColorImage) != 0)
                {
                    HOperatorSet.DispColor(ho_BackgroundImage, hv_WindowHandleBuffer);
                }
                else
                {
                    HOperatorSet.DispImage(ho_BackgroundImage, hv_WindowHandleBuffer);
                }
                //
                //Display objects
                if ((int)(new HTuple(((hv_SelectedObject.TupleSum())).TupleEqual(new HTuple(hv_SelectedObject.TupleLength()
                    )))) != 0)
                {
                    if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("true"))) != 0)
                    {
                        try
                        {
                            HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                            if ((int)((new HTuple((new HTuple((new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1306))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1305))))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1406))))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(1405)))) != 0)
                            {
                                if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleEqual(
                                    new HTuple(hv_GenParamValue.TupleLength())))) != 0)
                                {
                                    //This case means we have a Parameter with structure parameter_x with x > |ObjectModel3DID|-1
                                    for (hv_Index = new HTuple(hv_ObjectModel3DID.TupleLength()); (int)hv_Index <= (int)((2 * (new HTuple(hv_ObjectModel3DID.TupleLength()
                                        ))) + 1); hv_Index = (int)hv_Index + 1)
                                    {
                                        HOperatorSet.TupleStrstr(hv_GenParamName, "" + hv_Index, out hv_Position);
                                        for (hv_PosIdx = 0; (int)hv_PosIdx <= (int)((new HTuple(hv_Position.TupleLength()
                                            )) - 1); hv_PosIdx = (int)hv_PosIdx + 1)
                                        {
                                            if ((int)(new HTuple(((hv_Position.TupleSelect(hv_PosIdx))).TupleNotEqual(
                                                -1))) != 0)
                                            {
                                                throw new HalconException((("One of the parameters is refferring to a non-existing object model 3D:\n" + (hv_GenParamName.TupleSelect(
                                                    hv_PosIdx))) + " -> ") + (hv_GenParamValue.TupleSelect(hv_PosIdx)));
                                            }
                                        }
                                    }
                                    //Test for non-existing extended attributes:
                                    HOperatorSet.TupleStrstr(hv_GenParamName, "intensity", out hv_Position);
                                    for (hv_PosIdx = 0; (int)hv_PosIdx <= (int)((new HTuple(hv_Position.TupleLength()
                                        )) - 1); hv_PosIdx = (int)hv_PosIdx + 1)
                                    {
                                        if ((int)(new HTuple(((hv_Position.TupleSelect(hv_PosIdx))).TupleNotEqual(
                                            -1))) != 0)
                                        {
                                            HOperatorSet.TupleSplit(hv_GenParamName.TupleSelect(hv_PosIdx),
                                                "_", out hv_Substrings);
                                            if ((int)((new HTuple((new HTuple(hv_Substrings.TupleLength()
                                                )).TupleGreater(1))).TupleAnd(((hv_Substrings.TupleSelect(
                                                1))).TupleIsNumber())) != 0)
                                            {
                                                hv_I = ((hv_Substrings.TupleSelect(1))).TupleNumber();
                                                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(
                                                    hv_I), "has_extended_attribute", out hv_HasExtended);
                                                if ((int)(hv_HasExtended) != 0)
                                                {
                                                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(
                                                        hv_I), "extended_attribute_names", out hv_ExtendedAttributeNames);
                                                    HOperatorSet.TupleFind(hv_ExtendedAttributeNames, hv_GenParamValue.TupleSelect(
                                                        hv_PosIdx), out hv_Matches);
                                                }
                                                if ((int)((new HTuple(hv_HasExtended.TupleNot())).TupleOr((new HTuple(hv_Matches.TupleEqual(
                                                    -1))).TupleOr(new HTuple((new HTuple(hv_Matches.TupleLength()
                                                    )).TupleEqual(0))))) != 0)
                                                {
                                                    throw new HalconException((((("One of the parameters is refferring to an extended attribute that is not contained in the object model 3d with the handle " + (hv_ObjectModel3DID.TupleSelect(
                                                        hv_I))) + ":\n") + (hv_GenParamName.TupleSelect(hv_PosIdx))) + " -> ") + (hv_GenParamValue.TupleSelect(
                                                        hv_PosIdx)));
                                                }
                                            }
                                            else
                                            {
                                                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength()
                                                    )) - 1); hv_I = (int)hv_I + 1)
                                                {
                                                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(
                                                        hv_I), "extended_attribute_names", out hv_ExtendedAttributeNames);
                                                    HOperatorSet.TupleFind(hv_ExtendedAttributeNames, hv_GenParamValue.TupleSelect(
                                                        hv_PosIdx), out hv_Matches);
                                                    if ((int)((new HTuple(hv_Matches.TupleEqual(-1))).TupleOr(
                                                        new HTuple((new HTuple(hv_Matches.TupleLength())).TupleEqual(
                                                        0)))) != 0)
                                                    {
                                                        throw new HalconException((("One of the parameters is refferring to an extended attribute that is not contained in all object models:\n" + (hv_GenParamName.TupleSelect(
                                                            hv_PosIdx))) + " -> ") + (hv_GenParamValue.TupleSelect(
                                                            hv_PosIdx)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //
                                    throw new HalconException((new HTuple("Wrong generic parameters for display\n") + "Wrong Values are:\n") + (((((("    " + ((hv_GenParamName + " -> ") + hv_GenParamValue)) + "\n")).TupleSum()
                                        ) + "Exeption was:\n    ") + (hv_Exception.TupleSelect(2))));
                                }
                                else
                                {
                                    throw new HalconException(hv_Exception);
                                }
                            }
                            else if ((int)((new HTuple((new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(5185))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(5188))))).TupleOr(new HTuple(((hv_Exception.TupleSelect(
                                0))).TupleEqual(5187)))) != 0)
                            {
                                ExpTmpLocalVar_gUsesOpenGL = "false";
                                ExpSetGlobalVar_gUsesOpenGL(ExpTmpLocalVar_gUsesOpenGL);
                            }
                            else
                            {
                                throw new HalconException(hv_Exception);
                            }
                        }
                    }
                    if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("false"))) != 0)
                    {
                        //* NO OpenGL, use fallback
                        ho_ModelContours.Dispose();
                        disp_object_model_no_opengl(out ho_ModelContours, hv_ObjectModel3DID, hv_GenParamName,
                            hv_GenParamValue, hv_WindowHandleBuffer, hv_CamParam, hv_Poses);
                    }
                }
                else
                {
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_AlphaOrig.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        if ((int)(new HTuple(((hv_SelectedObject.TupleSelect(hv_Index))).TupleEqual(
                            1))) != 0)
                        {
                            HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index, "alpha", hv_AlphaOrig.TupleSelect(
                                hv_Index));
                        }
                        else
                        {
                            HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index, "alpha", ExpGetGlobalVar_gAlphaDeselected());
                        }
                    }
                    try
                    {
                        if ((int)(new HTuple(ExpGetGlobalVar_gUsesOpenGL().TupleEqual("false"))) != 0)
                        {
                            throw new HalconException(new HTuple());
                        }
                        HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                    }
                    // catch (Exception1) 
                    catch (HalconException HDevExpDefaultException1)
                    {
                        HDevExpDefaultException1.ToHTuple(out hv_Exception1);
                        //* NO OpenGL, use fallback
                        hv_DeselectedIdx = hv_SelectedObject.TupleFind(0);
                        if ((int)(new HTuple(hv_DeselectedIdx.TupleNotEqual(-1))) != 0)
                        {
                            hv_DeselectedName = "color_" + hv_DeselectedIdx;
                            hv_DeselectedValue = HTuple.TupleGenConst(new HTuple(hv_DeselectedName.TupleLength()
                                ), "gray");
                        }
                        ho_ModelContours.Dispose();
                        disp_object_model_no_opengl(out ho_ModelContours, hv_ObjectModel3DID, hv_GenParamName.TupleConcat(
                            hv_DeselectedName), hv_GenParamValue.TupleConcat(hv_DeselectedValue),
                            hv_WindowHandleBuffer, hv_CamParam, hv_Poses);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_AlphaOrig.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index, "alpha", hv_AlphaOrig.TupleSelect(
                            hv_Index));
                    }
                }
                ho_Image.Dispose();
                HOperatorSet.DumpWindowImage(out ho_Image, hv_WindowHandleBuffer);
                //
                //Display labels
                if ((int)(new HTuple(hv_Labels.TupleNotEqual(0))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, ExpGetGlobalVar_gLabelsDecor().TupleSelect(
                        0));
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        //Project the center point of the current model
                        hv_Pose = hv_Poses.TupleSelectRange(hv_Index * 7, (hv_Index * 7) + 6);
                        HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D);
                        HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID.TupleSelect(hv_Index),
                            "center", out hv_Center);
                        HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Center.TupleSelect(0),
                            hv_Center.TupleSelect(1), hv_Center.TupleSelect(2), out hv_CenterCamX,
                            out hv_CenterCamY, out hv_CenterCamZ);
                        HOperatorSet.Project3dPoint(hv_CenterCamX, hv_CenterCamY, hv_CenterCamZ,
                            hv_CamParam, out hv_CenterRow, out hv_CenterCol);
                        hv_Label = hv_Labels.TupleSelect(hv_Index);
                        if ((int)(new HTuple(hv_Label.TupleNotEqual(""))) != 0)
                        {
                            HOperatorSet.GetStringExtents(hv_WindowHandleBuffer, hv_Label, out hv_Ascent,
                                out hv_Descent, out hv_TextWidth, out hv_TextHeight);
                            disp_message(hv_WindowHandleBuffer, hv_Label, "window", (hv_CenterRow - (hv_TextHeight / 2)) + (ExpGetGlobalVar_gDispObjOffset().TupleSelect(
                                0)), (hv_CenterCol - (hv_TextWidth / 2)) + (ExpGetGlobalVar_gDispObjOffset().TupleSelect(
                                1)), new HTuple(), ExpGetGlobalVar_gLabelsDecor().TupleSelect(1));
                        }
                    }
                }
                //
                //Visualize the trackball if desired
                if ((int)(hv_VisualizeTrackball) != 0)
                {
                    HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, 1);
                    ho_TrackballContour.Dispose();
                    HOperatorSet.GenEllipseContourXld(out ho_TrackballContour, hv_TrackballCenterRow,
                        hv_TrackballCenterCol, 0, hv_TrackballRadiusPixel, hv_TrackballRadiusPixel,
                        0, 6.28318, "positive", 1.5);
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, "dim gray");
                    HOperatorSet.DispXld(ho_TrackballContour, hv_WindowHandleBuffer);
                }
                //
                //Visualize the rotation center if desired
                if ((int)((new HTuple(hv_VisualizeRotationCenter.TupleNotEqual(0))).TupleAnd(
                    new HTuple((new HTuple(hv_RotationCenter_COPY_INP_TMP.TupleLength())).TupleEqual(
                    3)))) != 0)
                {
                    if ((int)(new HTuple(((hv_RotationCenter_COPY_INP_TMP.TupleSelect(2))).TupleLess(
                        1e-10))) != 0)
                    {
                        if (hv_RotationCenter_COPY_INP_TMP == null)
                            hv_RotationCenter_COPY_INP_TMP = new HTuple();
                        hv_RotationCenter_COPY_INP_TMP[2] = 1e-10;
                    }
                    HOperatorSet.Project3dPoint(hv_RotationCenter_COPY_INP_TMP.TupleSelect(0),
                        hv_RotationCenter_COPY_INP_TMP.TupleSelect(1), hv_RotationCenter_COPY_INP_TMP.TupleSelect(
                        2), hv_CamParam, out hv_RotCenterRow, out hv_RotCenterCol);
                    hv_Orientation = (new HTuple(90)).TupleRad();
                    if ((int)(new HTuple(hv_VisualizeRotationCenter.TupleEqual(1))) != 0)
                    {
                        hv_Orientation = (new HTuple(45)).TupleRad();
                    }
                    ho_CrossRotCenter.Dispose();
                    HOperatorSet.GenCrossContourXld(out ho_CrossRotCenter, hv_RotCenterRow, hv_RotCenterCol,
                        hv_TrackballRadiusPixel / 25.0, hv_Orientation);
                    HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, 3);
                    HOperatorSet.QueryColor(hv_WindowHandleBuffer, out hv_Colors);
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, "light gray");
                    HOperatorSet.DispXld(ho_CrossRotCenter, hv_WindowHandleBuffer);
                    HOperatorSet.SetLineWidth(hv_WindowHandleBuffer, 1);
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, "dim gray");
                    HOperatorSet.DispXld(ho_CrossRotCenter, hv_WindowHandleBuffer);
                }
                //
                //Display title
                disp_title_and_information(hv_WindowHandleBuffer, hv_Title, hv_Information);
                //
                //Display the 'Exit' button
                //if ((int)(new HTuple(hv_DisplayContinueButton.TupleEqual("true"))) != 0)
                //{
                //    disp_continue_button(hv_WindowHandleBuffer);
                //}
                //
                HSystem.SetSystem("flush_graphic", "true");
                ho_ModelContours.Dispose();
                ho_Image.Dispose();
                ho_TrackballContour.Dispose();
                ho_CrossRotCenter.Dispose();

                return;
            }
            catch (HalconException )
            {
                ho_ModelContours.Dispose();
                ho_Image.Dispose();
                ho_TrackballContour.Dispose();
                ho_CrossRotCenter.Dispose();

                //throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Output
        // Short Description: Can replace disp_object_model_3d if there is no OpenGL available. 
        public void disp_object_model_no_opengl(out HObject ho_ModelContours, HTuple hv_ObjectModel3DID,
            HTuple hv_GenParamName, HTuple hv_GenParamValue, HTuple hv_WindowHandleBuffer,
            HTuple hv_CamParam, HTuple hv_PosesOut)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Idx = null, hv_CustomParamName = new HTuple();
            HTuple hv_CustomParamValue = new HTuple(), hv_Font = null;
            HTuple hv_IndicesDispBackGround = null, hv_Indices = new HTuple();
            HTuple hv_HasPolygons = null, hv_HasTri = null, hv_HasPoints = null;
            HTuple hv_HasLines = null, hv_NumPoints = null, hv_IsPrimitive = null;
            HTuple hv_Center = null, hv_Diameter = null, hv_OpenGlHiddenSurface = null;
            HTuple hv_CenterX = null, hv_CenterY = null, hv_CenterZ = null;
            HTuple hv_PosObjectsZ = null, hv_I = new HTuple(), hv_Pose = new HTuple();
            HTuple hv_HomMat3DObj = new HTuple(), hv_PosObjCenterX = new HTuple();
            HTuple hv_PosObjCenterY = new HTuple(), hv_PosObjCenterZ = new HTuple();
            HTuple hv_PosObjectsX = new HTuple(), hv_PosObjectsY = new HTuple();
            HTuple hv_Color = null, hv_Indices1 = new HTuple(), hv_IndicesIntensities = new HTuple();
            HTuple hv_Indices2 = new HTuple(), hv_J = null, hv_Indices3 = new HTuple();
            HTuple hv_HomMat3D = new HTuple(), hv_SampledObjectModel3D = new HTuple();
            HTuple hv_X = new HTuple(), hv_Y = new HTuple(), hv_Z = new HTuple();
            HTuple hv_HomMat3D1 = new HTuple(), hv_Qx = new HTuple();
            HTuple hv_Qy = new HTuple(), hv_Qz = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_ObjectModel3DConvexHull = new HTuple();
            HTuple hv_Exception = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            //This procedure allows to use project_object_model_3d to simulate a disp_object_model_3d
            //call for small objects. Large objects are sampled down to display.
            hv_Idx = hv_GenParamName.TupleFind("point_size");
            if ((int)((new HTuple(hv_Idx.TupleLength())).TupleAnd(new HTuple(hv_Idx.TupleNotEqual(
                -1)))) != 0)
            {
                hv_CustomParamName = "point_size";
                hv_CustomParamValue = hv_GenParamValue.TupleSelect(hv_Idx);
                if ((int)(new HTuple(hv_CustomParamValue.TupleEqual(1))) != 0)
                {
                    hv_CustomParamValue = 0;
                }
            }
            else
            {
                hv_CustomParamName = new HTuple();
                hv_CustomParamValue = new HTuple();
            }
            HOperatorSet.GetFont(hv_WindowHandleBuffer, out hv_Font);
            HOperatorSet.TupleFind(hv_GenParamName, "disp_background", out hv_IndicesDispBackGround);
            if ((int)(new HTuple(hv_IndicesDispBackGround.TupleNotEqual(-1))) != 0)
            {
                HOperatorSet.TupleFind(hv_GenParamName.TupleSelect(hv_IndicesDispBackGround),
                    "false", out hv_Indices);
                if ((int)(new HTuple(hv_Indices.TupleNotEqual(-1))) != 0)
                {
                    HOperatorSet.ClearWindow(hv_WindowHandleBuffer);
                }
            }
            set_display_font(hv_WindowHandleBuffer, 11, "mono", "false", "false");
            disp_message(hv_WindowHandleBuffer, "OpenGL missing!", "image", 5, (hv_CamParam.TupleSelect(
                6)) - 130, "red", "false");
            HOperatorSet.SetFont(hv_WindowHandleBuffer, hv_Font);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_polygons", out hv_HasPolygons);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_triangles", out hv_HasTri);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_points", out hv_HasPoints);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_lines", out hv_HasLines);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "num_points", out hv_NumPoints);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "has_primitive_data",
                out hv_IsPrimitive);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "center", out hv_Center);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "diameter", out hv_Diameter);
            HOperatorSet.GetSystem("opengl_hidden_surface_removal_enable", out hv_OpenGlHiddenSurface);
            HOperatorSet.SetSystem("opengl_hidden_surface_removal_enable", "false");
            //Sort the objects by inverse z
            hv_CenterX = hv_Center[HTuple.TupleGenSequence(0, (new HTuple(hv_Center.TupleLength()
                )) - 1, 3)];
            hv_CenterY = hv_Center[HTuple.TupleGenSequence(0, (new HTuple(hv_Center.TupleLength()
                )) - 1, 3) + 1];
            hv_CenterZ = hv_Center[HTuple.TupleGenSequence(0, (new HTuple(hv_Center.TupleLength()
                )) - 1, 3) + 2];
            hv_PosObjectsZ = new HTuple();
            if ((int)(new HTuple((new HTuple(hv_PosesOut.TupleLength())).TupleGreater(7))) != 0)
            {
                for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength())) - 1); hv_I = (int)hv_I + 1)
                {
                    hv_Pose = hv_PosesOut.TupleSelectRange(hv_I * 7, (hv_I * 7) + 6);
                    HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3DObj);
                    HOperatorSet.AffineTransPoint3d(hv_HomMat3DObj, hv_CenterX.TupleSelect(hv_I),
                        hv_CenterY.TupleSelect(hv_I), hv_CenterZ.TupleSelect(hv_I), out hv_PosObjCenterX,
                        out hv_PosObjCenterY, out hv_PosObjCenterZ);
                    hv_PosObjectsZ = hv_PosObjectsZ.TupleConcat(hv_PosObjCenterZ);
                }
            }
            else
            {
                hv_Pose = hv_PosesOut.TupleSelectRange(0, 6);
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3DObj);
                HOperatorSet.AffineTransPoint3d(hv_HomMat3DObj, hv_CenterX, hv_CenterY, hv_CenterZ,
                    out hv_PosObjectsX, out hv_PosObjectsY, out hv_PosObjectsZ);
            }
            hv_Idx = ((hv_PosObjectsZ.TupleSortIndex())).TupleInverse();
            hv_Color = "white";
            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color);
            if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleGreater(
                0))) != 0)
            {
                HOperatorSet.TupleFind(hv_GenParamName, "colored", out hv_Indices1);
                HOperatorSet.TupleFind(hv_GenParamName, "intensity", out hv_IndicesIntensities);
                HOperatorSet.TupleFind(hv_GenParamName, "color", out hv_Indices2);
                if ((int)(new HTuple(((hv_Indices1.TupleSelect(0))).TupleNotEqual(-1))) != 0)
                {
                    if ((int)(new HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect(
                        0)))).TupleEqual(3))) != 0)
                    {
                        hv_Color = new HTuple();
                        hv_Color[0] = "red";
                        hv_Color[1] = "green";
                        hv_Color[2] = "blue";
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect(
                        0)))).TupleEqual(6))) != 0)
                    {
                        hv_Color = new HTuple();
                        hv_Color[0] = "red";
                        hv_Color[1] = "green";
                        hv_Color[2] = "blue";
                        hv_Color[3] = "cyan";
                        hv_Color[4] = "magenta";
                        hv_Color[5] = "yellow";
                    }
                    else if ((int)(new HTuple(((hv_GenParamValue.TupleSelect(hv_Indices1.TupleSelect(
                        0)))).TupleEqual(12))) != 0)
                    {
                        hv_Color = new HTuple();
                        hv_Color[0] = "red";
                        hv_Color[1] = "green";
                        hv_Color[2] = "blue";
                        hv_Color[3] = "cyan";
                        hv_Color[4] = "magenta";
                        hv_Color[5] = "yellow";
                        hv_Color[6] = "coral";
                        hv_Color[7] = "slate blue";
                        hv_Color[8] = "spring green";
                        hv_Color[9] = "orange red";
                        hv_Color[10] = "pink";
                        hv_Color[11] = "gold";
                    }
                }
                else if ((int)(new HTuple(((hv_Indices2.TupleSelect(0))).TupleNotEqual(
                    -1))) != 0)
                {
                    hv_Color = hv_GenParamValue.TupleSelect(hv_Indices2.TupleSelect(0));
                }
                else if ((int)(new HTuple(((hv_IndicesIntensities.TupleSelect(0))).TupleNotEqual(
                    -1))) != 0)
                {
                }
            }
            for (hv_J = 0; (int)hv_J <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength())) - 1); hv_J = (int)hv_J + 1)
            {
                hv_I = hv_Idx.TupleSelect(hv_J);
                if ((int)((new HTuple((new HTuple((new HTuple(((hv_HasPolygons.TupleSelect(
                    hv_I))).TupleEqual("true"))).TupleOr(new HTuple(((hv_HasTri.TupleSelect(
                    hv_I))).TupleEqual("true"))))).TupleOr(new HTuple(((hv_HasPoints.TupleSelect(
                    hv_I))).TupleEqual("true"))))).TupleOr(new HTuple(((hv_HasLines.TupleSelect(
                    hv_I))).TupleEqual("true")))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        HOperatorSet.TupleFind(hv_GenParamName, "color_" + hv_I, out hv_Indices3);
                        if ((int)(new HTuple(((hv_Indices3.TupleSelect(0))).TupleNotEqual(-1))) != 0)
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_GenParamValue.TupleSelect(
                                hv_Indices3.TupleSelect(0)));
                        }
                        else
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color.TupleSelect(hv_I % (new HTuple(hv_Color.TupleLength()
                                ))));
                        }
                    }
                    if ((int)(new HTuple((new HTuple(hv_PosesOut.TupleLength())).TupleGreaterEqual(
                        (hv_I * 7) + 6))) != 0)
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(hv_I * 7, (hv_I * 7) + 6);
                    }
                    else
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(0, 6);
                    }
                    if ((int)(new HTuple(((hv_NumPoints.TupleSelect(hv_I))).TupleLess(10000))) != 0)
                    {
                        ho_ModelContours.Dispose();
                        HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_ObjectModel3DID.TupleSelect(
                            hv_I), hv_CamParam, hv_Pose, hv_CustomParamName, hv_CustomParamValue);
                        HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                    }
                    else
                    {
                        HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D);
                        HOperatorSet.SampleObjectModel3d(hv_ObjectModel3DID.TupleSelect(hv_I),
                            "fast", 0.01 * (hv_Diameter.TupleSelect(hv_I)), new HTuple(), new HTuple(),
                            out hv_SampledObjectModel3D);
                        ho_ModelContours.Dispose();
                        HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_SampledObjectModel3D,
                            hv_CamParam, hv_Pose, "point_size", 1);
                        HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, "point_coord_x",
                            out hv_X);
                        HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, "point_coord_y",
                            out hv_Y);
                        HOperatorSet.GetObjectModel3dParams(hv_SampledObjectModel3D, "point_coord_z",
                            out hv_Z);
                        HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D1);
                        HOperatorSet.AffineTransPoint3d(hv_HomMat3D1, hv_X, hv_Y, hv_Z, out hv_Qx,
                            out hv_Qy, out hv_Qz);
                        HOperatorSet.Project3dPoint(hv_Qx, hv_Qy, hv_Qz, hv_CamParam, out hv_Row,
                            out hv_Column);
                        HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                        HOperatorSet.ClearObjectModel3d(hv_SampledObjectModel3D);
                    }
                }
                else
                {
                    if ((int)(new HTuple((new HTuple(hv_GenParamName.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        HOperatorSet.TupleFind(hv_GenParamName, "color_" + hv_I, out hv_Indices3);
                        if ((int)(new HTuple(((hv_Indices3.TupleSelect(0))).TupleNotEqual(-1))) != 0)
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_GenParamValue.TupleSelect(
                                hv_Indices3.TupleSelect(0)));
                        }
                        else
                        {
                            HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color.TupleSelect(hv_I % (new HTuple(hv_Color.TupleLength()
                                ))));
                        }
                    }
                    if ((int)(new HTuple((new HTuple(hv_PosesOut.TupleLength())).TupleGreaterEqual(
                        (hv_I * 7) + 6))) != 0)
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(hv_I * 7, (hv_I * 7) + 6);
                    }
                    else
                    {
                        hv_Pose = hv_PosesOut.TupleSelectRange(0, 6);
                    }
                    if ((int)(new HTuple(((hv_IsPrimitive.TupleSelect(hv_I))).TupleEqual("true"))) != 0)
                    {
                        try
                        {
                            HOperatorSet.ConvexHullObjectModel3d(hv_ObjectModel3DID.TupleSelect(hv_I),
                                out hv_ObjectModel3DConvexHull);
                            if ((int)(new HTuple(((hv_NumPoints.TupleSelect(hv_I))).TupleLess(10000))) != 0)
                            {
                                ho_ModelContours.Dispose();
                                HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_ObjectModel3DConvexHull,
                                    hv_CamParam, hv_Pose, hv_CustomParamName, hv_CustomParamValue);
                                HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                            }
                            else
                            {
                                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_HomMat3D);
                                HOperatorSet.SampleObjectModel3d(hv_ObjectModel3DConvexHull, "fast",
                                    0.01 * (hv_Diameter.TupleSelect(hv_I)), new HTuple(), new HTuple(),
                                    out hv_SampledObjectModel3D);
                                ho_ModelContours.Dispose();
                                HOperatorSet.ProjectObjectModel3d(out ho_ModelContours, hv_SampledObjectModel3D,
                                    hv_CamParam, hv_Pose, "point_size", 1);
                                HOperatorSet.DispObj(ho_ModelContours, hv_WindowHandleBuffer);
                                HOperatorSet.ClearObjectModel3d(hv_SampledObjectModel3D);
                            }
                            HOperatorSet.ClearObjectModel3d(hv_ObjectModel3DConvexHull);
                        }
                        // catch (Exception) 
                        catch (HalconException HDevExpDefaultException1)
                        {
                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
                        }
                    }
                }
            }
            HOperatorSet.SetSystem("opengl_hidden_surface_removal_enable", hv_OpenGlHiddenSurface);

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Project an image point onto the trackball 
        public void project_point_on_trackball(HTuple hv_X, HTuple hv_Y, HTuple hv_VirtualTrackball,
            HTuple hv_TrackballSize, out HTuple hv_V)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_R = new HTuple(), hv_XP = new HTuple();
            HTuple hv_YP = new HTuple(), hv_ZP = new HTuple();
            // Initialize local and output iconic variables 
            if ((int)(new HTuple(hv_VirtualTrackball.TupleEqual("shoemake"))) != 0)
            {
                //Virtual Trackball according to Shoemake
                hv_R = (((hv_X * hv_X) + (hv_Y * hv_Y))).TupleSqrt();
                if ((int)(new HTuple(hv_R.TupleLessEqual(hv_TrackballSize))) != 0)
                {
                    hv_XP = hv_X.Clone();
                    hv_YP = hv_Y.Clone();
                    hv_ZP = (((hv_TrackballSize * hv_TrackballSize) - (hv_R * hv_R))).TupleSqrt();
                }
                else
                {
                    hv_XP = (hv_X * hv_TrackballSize) / hv_R;
                    hv_YP = (hv_Y * hv_TrackballSize) / hv_R;
                    hv_ZP = 0;
                }
            }
            else
            {
                //Virtual Trackball according to Bell
                hv_R = (((hv_X * hv_X) + (hv_Y * hv_Y))).TupleSqrt();
                if ((int)(new HTuple(hv_R.TupleLessEqual(hv_TrackballSize * 0.70710678))) != 0)
                {
                    hv_XP = hv_X.Clone();
                    hv_YP = hv_Y.Clone();
                    hv_ZP = (((hv_TrackballSize * hv_TrackballSize) - (hv_R * hv_R))).TupleSqrt();
                }
                else
                {
                    hv_XP = hv_X.Clone();
                    hv_YP = hv_Y.Clone();
                    hv_ZP = ((0.6 * hv_TrackballSize) * hv_TrackballSize) / hv_R;
                }
            }
            hv_V = new HTuple();
            hv_V = hv_V.TupleConcat(hv_XP);
            hv_V = hv_V.TupleConcat(hv_YP);
            hv_V = hv_V.TupleConcat(hv_ZP);

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Compute the 3d rotation from the mose movement 
        public void trackball(HTuple hv_MX1, HTuple hv_MY1, HTuple hv_MX2, HTuple hv_MY2,
            HTuple hv_VirtualTrackball, HTuple hv_TrackballSize, HTuple hv_SensFactor, out HTuple hv_QuatRotation)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_D = null, hv_P2 = null, hv_P1 = null;
            HTuple hv_T = null, hv_RotAngle = null, hv_Len = null;
            HTuple hv_RotAxis = null;
            // Initialize local and output iconic variables 
            hv_QuatRotation = new HTuple();
            //Compute the 3d rotation from the mouse movement
            //
            if ((int)((new HTuple(hv_MX1.TupleEqual(hv_MX2))).TupleAnd(new HTuple(hv_MY1.TupleEqual(
                hv_MY2)))) != 0)
            {
                hv_QuatRotation = new HTuple();
                hv_QuatRotation[0] = 1;
                hv_QuatRotation[1] = 0;
                hv_QuatRotation[2] = 0;
                hv_QuatRotation[3] = 0;

                return;
            }
            //Project the image point onto the trackball
            project_point_on_trackball(hv_MX1, hv_MY1, hv_VirtualTrackball, hv_TrackballSize,
                out hv_P1);
            project_point_on_trackball(hv_MX2, hv_MY2, hv_VirtualTrackball, hv_TrackballSize,
                out hv_P2);
            //The cross product of the projected points defines the rotation axis
            tuple_vector_cross_product(hv_P1, hv_P2, out hv_RotAxis);
            //Compute the rotation angle
            hv_D = hv_P2 - hv_P1;
            hv_T = (((((hv_D * hv_D)).TupleSum())).TupleSqrt()) / (2.0 * hv_TrackballSize);
            if ((int)(new HTuple(hv_T.TupleGreater(1.0))) != 0)
            {
                hv_T = 1.0;
            }
            if ((int)(new HTuple(hv_T.TupleLess(-1.0))) != 0)
            {
                hv_T = -1.0;
            }
            hv_RotAngle = (2.0 * (hv_T.TupleAsin())) * hv_SensFactor;
            hv_Len = ((((hv_RotAxis * hv_RotAxis)).TupleSum())).TupleSqrt();
            if ((int)(new HTuple(hv_Len.TupleGreater(0.0))) != 0)
            {
                hv_RotAxis = hv_RotAxis / hv_Len;
            }
            HOperatorSet.AxisAngleToQuat(hv_RotAxis.TupleSelect(0), hv_RotAxis.TupleSelect(
                1), hv_RotAxis.TupleSelect(2), hv_RotAngle, out hv_QuatRotation);

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Get string extends of several lines. 
        public void max_line_width(HTuple hv_WindowHandle, HTuple hv_Lines, out HTuple hv_MaxWidth)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Index = null, hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_LineWidth = new HTuple();
            HTuple hv_LineHeight = new HTuple();
            // Initialize local and output iconic variables 
            hv_MaxWidth = 0;
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Lines.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                HOperatorSet.GetStringExtents(hv_WindowHandle, hv_Lines.TupleSelect(hv_Index),
                    out hv_Ascent, out hv_Descent, out hv_LineWidth, out hv_LineHeight);
                hv_MaxWidth = ((hv_LineWidth.TupleConcat(hv_MaxWidth))).TupleMax();
            }

            return;
        }

        // Chapter: Tuple / Arithmetic
        // Short Description: Calculates the cross product of two vectors of length 3. 
        public void tuple_vector_cross_product(HTuple hv_V1, HTuple hv_V2, out HTuple hv_VC)
        {



            // Local iconic variables 
            // Initialize local and output iconic variables 
            //The caller must ensure that the length of both input vectors is 3
            hv_VC = ((hv_V1.TupleSelect(1)) * (hv_V2.TupleSelect(2))) - ((hv_V1.TupleSelect(2)) * (hv_V2.TupleSelect(
                1)));
            hv_VC = hv_VC.TupleConcat(((hv_V1.TupleSelect(2)) * (hv_V2.TupleSelect(0))) - ((hv_V1.TupleSelect(
                0)) * (hv_V2.TupleSelect(2))));
            hv_VC = hv_VC.TupleConcat(((hv_V1.TupleSelect(0)) * (hv_V2.TupleSelect(1))) - ((hv_V1.TupleSelect(
                1)) * (hv_V2.TupleSelect(0))));

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Compute the center of all given 3D object models. 
        public void get_object_models_center(HTuple hv_ObjectModel3DID, out HTuple hv_Center)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_Diameter = new HTuple(), hv_MD = new HTuple();
            HTuple hv_Weight = new HTuple(), hv_SumW = new HTuple();
            HTuple hv_Index = new HTuple(), hv_ObjectModel3DIDSelected = new HTuple();
            HTuple hv_C = new HTuple(), hv_InvSum = new HTuple();
            // Initialize local and output iconic variables 
            hv_Center = new HTuple();
            //Compute the mean of all model centers (weighted by the diameter of the object models)
            if ((int)(new HTuple((new HTuple(hv_ObjectModel3DID.TupleLength())).TupleGreater(
                0))) != 0)
            {
                HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DID, "diameter_axis_aligned_bounding_box",
                    out hv_Diameter);
                //Normalize Diameter to use it as weights for a weighted mean of the individual centers
                hv_MD = hv_Diameter.TupleMean();
                if ((int)(new HTuple(hv_MD.TupleGreater(1e-10))) != 0)
                {
                    hv_Weight = hv_Diameter / hv_MD;
                }
                else
                {
                    hv_Weight = hv_Diameter.Clone();
                }
                hv_SumW = hv_Weight.TupleSum();
                if ((int)(new HTuple(hv_SumW.TupleLess(1e-10))) != 0)
                {
                    hv_Weight = HTuple.TupleGenConst(new HTuple(hv_Weight.TupleLength()), 1.0);
                    hv_SumW = hv_Weight.TupleSum();
                }
                hv_Center = new HTuple();
                hv_Center[0] = 0;
                hv_Center[1] = 0;
                hv_Center[2] = 0;
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ObjectModel3DID.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_ObjectModel3DIDSelected = hv_ObjectModel3DID.TupleSelect(hv_Index);
                    HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3DIDSelected, "center",
                        out hv_C);
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[0] = (hv_Center.TupleSelect(0)) + ((hv_C.TupleSelect(0)) * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[1] = (hv_Center.TupleSelect(1)) + ((hv_C.TupleSelect(1)) * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_Center == null)
                        hv_Center = new HTuple();
                    hv_Center[2] = (hv_Center.TupleSelect(2)) + ((hv_C.TupleSelect(2)) * (hv_Weight.TupleSelect(
                        hv_Index)));
                }
                hv_InvSum = 1.0 / hv_SumW;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[0] = (hv_Center.TupleSelect(0)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[1] = (hv_Center.TupleSelect(1)) * hv_InvSum;
                if (hv_Center == null)
                    hv_Center = new HTuple();
                hv_Center[2] = (hv_Center.TupleSelect(2)) * hv_InvSum;
            }
            else
            {
                hv_Center = new HTuple();
            }

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Displays a continue button. 
        public void disp_continue_button(HTuple hv_WindowHandle)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_ContinueMessage = new HTuple(), hv_Exception = null;
            HTuple hv_Row = null, hv_Column = null, hv_Width = null;
            HTuple hv_Height = null, hv_Ascent = null, hv_Descent = null;
            HTuple hv_TextWidth = null, hv_TextHeight = null;
            // Initialize local and output iconic variables 
            //This procedure displays a 'Continue' text button
            //in the lower right corner of the screen.
            //It uses the procedure disp_message.
            //
            //Input parameters:
            //WindowHandle: The window, where the text shall be displayed
            //
            //Use the continue message set in the global variable gTerminationButtonLabel.
            //If this variable is not defined, set a standard text instead.
            //global tuple gTerminationButtonLabel
            try
            {
                hv_ContinueMessage = ExpGetGlobalVar_gTerminationButtonLabel().Clone();
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                hv_ContinueMessage = "Continue";
            }
            //Display the continue button
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_Row, out hv_Column, out hv_Width,
                out hv_Height);
            HOperatorSet.GetStringExtents(hv_WindowHandle, (" " + hv_ContinueMessage) + " ",
                out hv_Ascent, out hv_Descent, out hv_TextWidth, out hv_TextHeight);
            disp_text_button(hv_WindowHandle, hv_ContinueMessage, "window", (hv_Height - hv_TextHeight) - 12,
                (hv_Width - hv_TextWidth) - 12, "black", "#f28f26");

            return;
        }

        // Chapter: Graphics / Text
        // Short Description: This procedure writes a text message. 
        public void disp_text_button(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_TextColor, HTuple hv_ButtonColor)
        {



            // Local iconic variables 

            HObject ho_UpperLeft, ho_LowerRight, ho_Rectangle;

            // Local control variables 

            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            HTuple hv_Row1Part = null, hv_Column1Part = null, hv_Row2Part = null;
            HTuple hv_Column2Part = null, hv_RowWin = null, hv_ColumnWin = null;
            HTuple hv_WidthWin = null, hv_HeightWin = null, hv_Exception = null;
            HTuple hv_Fac = null, hv_RGBL = null, hv_RGB = new HTuple();
            HTuple hv_RGBD = null, hv_ButtonColorBorderL = null, hv_ButtonColorBorderD = null;
            HTuple hv_MaxAscent = null, hv_MaxDescent = null, hv_MaxWidth = null;
            HTuple hv_MaxHeight = null, hv_R1 = new HTuple(), hv_C1 = new HTuple();
            HTuple hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = null, hv_Index = null, hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple();
            HTuple hv_H = new HTuple(), hv_FrameHeight = null, hv_FrameWidth = null;
            HTuple hv_R2 = null, hv_C2 = null, hv_ClipRegion = null;
            HTuple hv_DrawMode = null, hv_BorderWidth = null, hv_CurrentColor = new HTuple();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();
            HTuple hv_TextColor_COPY_INP_TMP = hv_TextColor.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_UpperLeft);
            HOperatorSet.GenEmptyObj(out ho_LowerRight);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            try
            {
                //This procedure displays text in a graphics window.
                //
                //Input parameters:
                //WindowHandle: The WindowHandle of the graphics window, where
                //   the message should be displayed
                //String: A tuple of strings containing the text message to be displayed
                //CoordSystem: If set to 'window', the text position is given
                //   with respect to the window coordinate system.
                //   If set to 'image', image coordinates are used.
                //   (This may be useful in zoomed images.)
                //Row: The row coordinate of the desired text position
                //   If set to -1, a default value of 12 is used.
                //Column: The column coordinate of the desired text position
                //   If set to -1, a default value of 12 is used.
                //Color: defines the color of the text as string.
                //   If set to [], '' or 'auto' the currently set color is used.
                //   If a tuple of strings is passed, the colors are used cyclically
                //   for each new textline.
                //ButtonColor: Must be set to a color string (e.g. 'white', '#FF00CC', etc.).
                //             The text is written in a box of that color.
                //
                //prepare window
                HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
                HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part,
                    out hv_Row2Part, out hv_Column2Part);
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                    out hv_WidthWin, out hv_HeightWin);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
                //
                //default settings
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_TextColor_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
                {
                    hv_TextColor_COPY_INP_TMP = "";
                }
                //
                try
                {
                    color_string_to_rgb(hv_ButtonColor, out hv_RGB);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter ButtonColor (must be a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                hv_Fac = 0.4;
                hv_RGBL = hv_RGB + (((((255.0 - hv_RGB) * hv_Fac) + 0.5)).TupleInt());
                hv_RGBD = hv_RGB - ((((hv_RGB * hv_Fac) + 0.5)).TupleInt());
                hv_ButtonColorBorderL = "#" + ((("" + (hv_RGBL.TupleString("02x")))).TupleSum()
                    );
                hv_ButtonColorBorderD = "#" + ((("" + (hv_RGBD.TupleString("02x")))).TupleSum()
                    );
                //
                hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
                //
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                    out hv_MaxWidth, out hv_MaxHeight);
                if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
                {
                    hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                    hv_C1 = hv_Column_COPY_INP_TMP.Clone();
                }
                else
                {
                    //transform image to window coordinates
                    hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                    hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                    hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                    hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
                }
                //
                //display text box depending on text size
                //
                //calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //display rectangles
                HOperatorSet.GetSystem("clip_region", out hv_ClipRegion);
                HOperatorSet.SetSystem("clip_region", "false");
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                hv_BorderWidth = 2;
                ho_UpperLeft.Dispose();
                HOperatorSet.GenRegionPolygonFilled(out ho_UpperLeft, ((((((((hv_R1 - hv_BorderWidth)).TupleConcat(
                    hv_R1 - hv_BorderWidth))).TupleConcat(hv_R1))).TupleConcat(hv_R2))).TupleConcat(
                    hv_R2 + hv_BorderWidth), ((((((((hv_C1 - hv_BorderWidth)).TupleConcat(hv_C2 + hv_BorderWidth))).TupleConcat(
                    hv_C2))).TupleConcat(hv_C1))).TupleConcat(hv_C1 - hv_BorderWidth));
                ho_LowerRight.Dispose();
                HOperatorSet.GenRegionPolygonFilled(out ho_LowerRight, ((((((((hv_R2 + hv_BorderWidth)).TupleConcat(
                    hv_R1 - hv_BorderWidth))).TupleConcat(hv_R1))).TupleConcat(hv_R2))).TupleConcat(
                    hv_R2 + hv_BorderWidth), ((((((((hv_C2 + hv_BorderWidth)).TupleConcat(hv_C2 + hv_BorderWidth))).TupleConcat(
                    hv_C2))).TupleConcat(hv_C1))).TupleConcat(hv_C1 - hv_BorderWidth));
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetColor(hv_WindowHandle, hv_ButtonColorBorderL);
                HOperatorSet.DispObj(ho_UpperLeft, hv_WindowHandle);
                HOperatorSet.SetColor(hv_WindowHandle, hv_ButtonColorBorderD);
                HOperatorSet.DispObj(ho_LowerRight, hv_WindowHandle);
                HOperatorSet.SetColor(hv_WindowHandle, hv_ButtonColor);
                HOperatorSet.DispObj(ho_Rectangle, hv_WindowHandle);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                HOperatorSet.SetSystem("clip_region", hv_ClipRegion);
                //Write text.
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_CurrentColor = hv_TextColor_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_TextColor_COPY_INP_TMP.TupleLength()
                        )));
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                    HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                    HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index));
                }
                //reset changed window settings
                HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                    hv_Column2Part);
                ho_UpperLeft.Dispose();
                ho_LowerRight.Dispose();
                ho_Rectangle.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_UpperLeft.Dispose();
                ho_LowerRight.Dispose();
                ho_Rectangle.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Output
        // Short Description: Get the center of the virtual trackback that is used to move the camera (version for inspection_mode = 'surface'). 
        public void get_trackball_center_fixed(HTuple hv_SelectedObject, HTuple hv_TrackballCenterRow,
            HTuple hv_TrackballCenterCol, HTuple hv_TrackballRadiusPixel, HTuple hv_Scene3D,
            HTuple hv_ObjectModel3DID, HTuple hv_Poses, HTuple hv_WindowHandleBuffer, HTuple hv_CamParam,
            HTuple hv_GenParamName, HTuple hv_GenParamValue, out HTuple hv_TBCenter, out HTuple hv_TBSize)
        {



            // Local iconic variables 

            HObject ho_RegionCenter, ho_DistanceImage;
            HObject ho_Domain;

            // Local control variables 

            HTuple hv_NumModels = null, hv_Width = null;
            HTuple hv_Height = null, hv_SelectPose = null, hv_Index1 = null;
            HTuple hv_Rows = null, hv_Columns = null, hv_Grayval = null;
            HTuple hv_IndicesG = null, hv_Value = null, hv_Pos = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionCenter);
            HOperatorSet.GenEmptyObj(out ho_DistanceImage);
            HOperatorSet.GenEmptyObj(out ho_Domain);
            hv_TBCenter = new HTuple();
            hv_TBSize = new HTuple();
            try
            {
                //Determine the trackball center for the fixed trackball
                hv_NumModels = new HTuple(hv_ObjectModel3DID.TupleLength());
                hv_Width = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 2];
                hv_Height = hv_CamParam[(new HTuple(hv_CamParam.TupleLength())) - 1];
                //
                //Project the selected objects
                hv_SelectPose = new HTuple();
                for (hv_Index1 = 0; (int)hv_Index1 <= (int)((new HTuple(hv_SelectedObject.TupleLength()
                    )) - 1); hv_Index1 = (int)hv_Index1 + 1)
                {
                    hv_SelectPose = hv_SelectPose.TupleConcat(HTuple.TupleGenConst(7, hv_SelectedObject.TupleSelect(
                        hv_Index1)));
                    if ((int)(new HTuple(((hv_SelectedObject.TupleSelect(hv_Index1))).TupleEqual(
                        0))) != 0)
                    {
                        HOperatorSet.SetScene3dInstanceParam(hv_Scene3D, hv_Index1, "visible",
                            "false");
                    }
                }
                HOperatorSet.SetScene3dParam(hv_Scene3D, "depth_persistence", "true");
                HOperatorSet.DisplayScene3d(hv_WindowHandleBuffer, hv_Scene3D, 0);
                HOperatorSet.SetScene3dParam(hv_Scene3D, "visible", "true");
                //
                //determine the depth of the object point that appears closest to the trackball
                //center
                ho_RegionCenter.Dispose();
                HOperatorSet.GenRegionPoints(out ho_RegionCenter, hv_TrackballCenterRow, hv_TrackballCenterCol);
                ho_DistanceImage.Dispose();
                HOperatorSet.DistanceTransform(ho_RegionCenter, out ho_DistanceImage, "chamfer-3-4-unnormalized",
                    "false", hv_Width, hv_Height);
                ho_Domain.Dispose();
                HOperatorSet.GetDomain(ho_DistanceImage, out ho_Domain);
                HOperatorSet.GetRegionPoints(ho_Domain, out hv_Rows, out hv_Columns);
                HOperatorSet.GetGrayval(ho_DistanceImage, hv_Rows, hv_Columns, out hv_Grayval);
                HOperatorSet.TupleSortIndex(hv_Grayval, out hv_IndicesG);
                HOperatorSet.GetDisplayScene3dInfo(hv_WindowHandleBuffer, hv_Scene3D, hv_Rows.TupleSelect(
                    hv_IndicesG), hv_Columns.TupleSelect(hv_IndicesG), "depth", out hv_Value);
                HOperatorSet.TupleFind(hv_Value.TupleSgn(), 1, out hv_Pos);
                //
                HOperatorSet.SetScene3dParam(hv_Scene3D, "depth_persistence", "false");
                //
                //
                //set TBCenter
                if ((int)(new HTuple(hv_Pos.TupleNotEqual(-1))) != 0)
                {
                    //if the object is visible in the image
                    hv_TBCenter = new HTuple();
                    hv_TBCenter[0] = 0;
                    hv_TBCenter[1] = 0;
                    hv_TBCenter = hv_TBCenter.TupleConcat(hv_Value.TupleSelect(
                        hv_Pos.TupleSelect(0)));
                }
                else
                {
                    //if the object is not visible in the image, set the z coordinate to -1
                    //to indicate, the the previous z value should be used instead
                    hv_TBCenter = new HTuple();
                    hv_TBCenter[0] = 0;
                    hv_TBCenter[1] = 0;
                    hv_TBCenter[2] = -1;
                }
                //
                if ((int)(new HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(0))) != 0)
                {
                    hv_TBSize = (0.5 + ((0.5 * (hv_SelectedObject.TupleSum())) / hv_NumModels)) * hv_TrackballRadiusPixel;
                }
                else
                {
                    hv_TBCenter = new HTuple();
                    hv_TBSize = 0;
                }
                ho_RegionCenter.Dispose();
                ho_DistanceImage.Dispose();
                ho_Domain.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionCenter.Dispose();
                ho_DistanceImage.Dispose();
                ho_Domain.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Parameters
        public void color_string_to_rgb(HTuple hv_Color, out HTuple hv_RGB)
        {



            // Local iconic variables 

            HObject ho_Rectangle, ho_Image;

            // Local control variables 

            HTuple hv_WindowHandleBuffer = null, hv_Exception = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_Image);
            try
            {
                HOperatorSet.OpenWindow(0, 0, 1, 1, 0, "buffer", "", out hv_WindowHandleBuffer);
                HOperatorSet.SetPart(hv_WindowHandleBuffer, 0, 0, -1, -1);
                ho_Rectangle.Dispose();
                HOperatorSet.GenRectangle1(out ho_Rectangle, 0, 0, 0, 0);
                try
                {
                    HOperatorSet.SetColor(hv_WindowHandleBuffer, hv_Color);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    hv_Exception = "Wrong value of control parameter Color (must be a valid color string)";
                    throw new HalconException(hv_Exception);
                }
                HOperatorSet.DispObj(ho_Rectangle, hv_WindowHandleBuffer);
                ho_Image.Dispose();
                HOperatorSet.DumpWindowImage(out ho_Image, hv_WindowHandleBuffer);
                HOperatorSet.CloseWindow(hv_WindowHandleBuffer);
                HOperatorSet.GetGrayval(ho_Image, 0, 0, out hv_RGB);
                hv_RGB = hv_RGB + ((new HTuple(0)).TupleConcat(0)).TupleConcat(0);
                ho_Rectangle.Dispose();
                ho_Image.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Rectangle.Dispose();
                ho_Image.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Output
        // Short Description: Get the center of the virtual trackback that is used to move the camera. 
        public void get_trackball_center(HTuple hv_SelectedObject, HTuple hv_TrackballRadiusPixel,
            HTuple hv_ObjectModel3D, HTuple hv_Poses, out HTuple hv_TBCenter, out HTuple hv_TBSize)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_NumModels = null, hv_Centers = null;
            HTuple hv_Diameter = null, hv_MD = null, hv_Weight = new HTuple();
            HTuple hv_SumW = null, hv_Index = null, hv_ObjectModel3DIDSelected = new HTuple();
            HTuple hv_PoseSelected = new HTuple(), hv_HomMat3D = new HTuple();
            HTuple hv_TBCenterCamX = new HTuple(), hv_TBCenterCamY = new HTuple();
            HTuple hv_TBCenterCamZ = new HTuple(), hv_InvSum = new HTuple();
            // Initialize local and output iconic variables 
            hv_TBCenter = new HTuple();
            hv_TBSize = new HTuple();
            hv_NumModels = new HTuple(hv_ObjectModel3D.TupleLength());
            if (hv_TBCenter == null)
                hv_TBCenter = new HTuple();
            hv_TBCenter[0] = 0;
            if (hv_TBCenter == null)
                hv_TBCenter = new HTuple();
            hv_TBCenter[1] = 0;
            if (hv_TBCenter == null)
                hv_TBCenter = new HTuple();
            hv_TBCenter[2] = 0;
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, "center", out hv_Centers);
            HOperatorSet.GetObjectModel3dParams(hv_ObjectModel3D, "diameter_axis_aligned_bounding_box",
                out hv_Diameter);
            //Normalize Diameter to use it as weights for a weighted mean of the individual centers
            hv_MD = hv_Diameter.TupleMean();
            if ((int)(new HTuple(hv_MD.TupleGreater(1e-10))) != 0)
            {
                hv_Weight = hv_Diameter / hv_MD;
            }
            else
            {
                hv_Weight = hv_Diameter.Clone();
            }
            hv_SumW = ((hv_Weight.TupleSelectMask(((hv_SelectedObject.TupleSgn())).TupleAbs()
                ))).TupleSum();
            if ((int)(new HTuple(hv_SumW.TupleLess(1e-10))) != 0)
            {
                hv_Weight = HTuple.TupleGenConst(new HTuple(hv_Weight.TupleLength()), 1.0);
                hv_SumW = ((hv_Weight.TupleSelectMask(((hv_SelectedObject.TupleSgn())).TupleAbs()
                    ))).TupleSum();
            }
            HTuple end_val18 = hv_NumModels - 1;
            HTuple step_val18 = 1;
            for (hv_Index = 0; hv_Index.Continue(end_val18, step_val18); hv_Index = hv_Index.TupleAdd(step_val18))
            {
                if ((int)(hv_SelectedObject.TupleSelect(hv_Index)) != 0)
                {
                    hv_ObjectModel3DIDSelected = hv_ObjectModel3D.TupleSelect(hv_Index);
                    hv_PoseSelected = hv_Poses.TupleSelectRange(hv_Index * 7, (hv_Index * 7) + 6);
                    HOperatorSet.PoseToHomMat3d(hv_PoseSelected, out hv_HomMat3D);
                    HOperatorSet.AffineTransPoint3d(hv_HomMat3D, hv_Centers.TupleSelect((hv_Index * 3) + 0),
                        hv_Centers.TupleSelect((hv_Index * 3) + 1), hv_Centers.TupleSelect((hv_Index * 3) + 2),
                        out hv_TBCenterCamX, out hv_TBCenterCamY, out hv_TBCenterCamZ);
                    if (hv_TBCenter == null)
                        hv_TBCenter = new HTuple();
                    hv_TBCenter[0] = (hv_TBCenter.TupleSelect(0)) + (hv_TBCenterCamX * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_TBCenter == null)
                        hv_TBCenter = new HTuple();
                    hv_TBCenter[1] = (hv_TBCenter.TupleSelect(1)) + (hv_TBCenterCamY * (hv_Weight.TupleSelect(
                        hv_Index)));
                    if (hv_TBCenter == null)
                        hv_TBCenter = new HTuple();
                    hv_TBCenter[2] = (hv_TBCenter.TupleSelect(2)) + (hv_TBCenterCamZ * (hv_Weight.TupleSelect(
                        hv_Index)));
                }
            }
            if ((int)(new HTuple(((hv_SelectedObject.TupleMax())).TupleNotEqual(0))) != 0)
            {
                hv_InvSum = 1.0 / hv_SumW;
                if (hv_TBCenter == null)
                    hv_TBCenter = new HTuple();
                hv_TBCenter[0] = (hv_TBCenter.TupleSelect(0)) * hv_InvSum;
                if (hv_TBCenter == null)
                    hv_TBCenter = new HTuple();
                hv_TBCenter[1] = (hv_TBCenter.TupleSelect(1)) * hv_InvSum;
                if (hv_TBCenter == null)
                    hv_TBCenter = new HTuple();
                hv_TBCenter[2] = (hv_TBCenter.TupleSelect(2)) * hv_InvSum;
                hv_TBSize = (0.5 + ((0.5 * (hv_SelectedObject.TupleSum())) / hv_NumModels)) * hv_TrackballRadiusPixel;
            }
            else
            {
                hv_TBCenter = new HTuple();
                hv_TBSize = 0;
            }

            return;
        }


#if !NO_EXPORT_MAIN
        // Main procedure 
        public void Dispose3D()
        {

            breakOut = true;

            while (showEnd == false)
            {
                Thread.Sleep(100);
            }

        }
        static bool isRunOver = true; 
        public void Show3D(float[] x, float[] y, float[] z, HTuple windowhandle)
        {           
            //Dispose3D();
            HTuple hv_x = new HTuple();
            HTuple hv_y = new HTuple();
            HTuple hv_z = new HTuple();
            HTuple hv_ObjectModel3D = null, hv_PoseOut = null;
            for (int i = 0; i < x.Length; i++)
            {
                hv_x[i] = x[i];
                hv_y[i] = y[i];
                hv_z[i] = z[i];
            }

            HOperatorSet.GenObjectModel3dFromPoints(hv_x, hv_y, hv_z, out hv_ObjectModel3D);
            HObject contour;
            HOperatorSet.GenContourPolygonXld(out contour, hv_x, hv_y);
            HTuple Row, Column, Phi, Length1, Length2, PointOrder;
            HOperatorSet.FitRectangle2ContourXld(contour, "regression", -1, 0, 0, 3, 2, out Row, out Column, out Phi, out Length1, out Length2, out PointOrder);
            contour.Dispose();
            //HOperatorSet.WriteTuple(hv_x,"x.tup");
            //HOperatorSet.WriteTuple(hv_y, "y.tup");
            //HOperatorSet.WriteTuple(hv_z, "z.tup");
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    while (!isRunOver) ;//等待上一次结束
                    isRunOver = false;

                    breakOut = false;
                    HTuple Pose1 = new HTuple();
                    //HOperatorSet.SetLineWidth(windowhandle, 50);
                    HOperatorSet.CreatePose(-Row.TupleRound().ToIArr()[0], -Column.TupleRound().ToIArr()[0], 0, 0, 180, 0, "Rp+T", "gba", "point",out  Pose1);
                    //                visualize_object_model_3d(windowhandle, hv_ObjectModel3D, new HTuple(), Pose1,
                    //new HTuple("alpha").TupleConcat("disp_pose").TupleConcat("depth_persistence").TupleConcat("point_size").TupleConcat("color").TupleConcat("disp_background"),
                    //(new HTuple(0.5)).TupleConcat("false").TupleConcat("true").TupleConcat(2.5).TupleConcat("red").TupleConcat("true"), new HTuple(), new HTuple(), new HTuple(), out hv_PoseOut);

                    visualize_object_model_3d(windowhandle, hv_ObjectModel3D, new HTuple(), Pose1,
   (((new HTuple("alpha")).TupleConcat("intensity_red")).TupleConcat("intensity_red")).TupleConcat(
"intensity_red").TupleConcat("disp_pose").TupleConcat("depth_persistence").TupleConcat("point_size").TupleConcat("color"), (((new HTuple(0.5)).TupleConcat("coord_x")).TupleConcat(
"coord_y")).TupleConcat("coord_z").TupleConcat("true").TupleConcat("true").TupleConcat(3.5).TupleConcat("yellow"), new HTuple(), new HTuple(), new HTuple(), out hv_PoseOut);
                    
                });
            //Dispose3D();


        }

#endif
        

    }

}
