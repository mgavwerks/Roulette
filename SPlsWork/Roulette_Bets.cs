using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;

namespace UserModule_ROULETTE_BETS
{
    public class UserModuleClass_ROULETTE_BETS : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.DigitalInput RESETOUTPUTS;
        Crestron.Logos.SplusObjects.AnalogInput WINNINGNUMBER;
        Crestron.Logos.SplusObjects.DigitalOutput ODD;
        Crestron.Logos.SplusObjects.DigitalOutput EVEN;
        Crestron.Logos.SplusObjects.DigitalOutput BLACK;
        Crestron.Logos.SplusObjects.DigitalOutput RED;
        Crestron.Logos.SplusObjects.DigitalOutput FIRSTTWELVE;
        Crestron.Logos.SplusObjects.DigitalOutput SECONDTWELVE;
        Crestron.Logos.SplusObjects.DigitalOutput THIRDTWELVE;
        Crestron.Logos.SplusObjects.DigitalOutput COLUMNONE;
        Crestron.Logos.SplusObjects.DigitalOutput COLUMNTWO;
        Crestron.Logos.SplusObjects.DigitalOutput COLUMNTHREE;
        Crestron.Logos.SplusObjects.DigitalOutput ZERO;
        Crestron.Logos.SplusObjects.DigitalOutput DBLZERO;
        Crestron.Logos.SplusObjects.AnalogOutput WINNINGNUMBEROUT;
        ushort [] ALLARRAY;
        ushort [] BETSARRAY;
        ushort [] ODDARRAY;
        ushort [] EVENARRAY;
        ushort [] FIRSTTWELVEARRAY;
        ushort [] SECONDTWELVEARRAY;
        ushort [] THIRDTWELVEARRAY;
        ushort [] COLUMNONEARRAY;
        ushort [] COLUMNTWOARRAY;
        ushort [] COLUMNTHREEARRAY;
        BLACKNUMBERS BLACKS;
        STREETS [] ROW;
        private void BUILDARRAYS (  SplusExecutionContext __context__ ) 
            { 
            ushort INDEX = 0;
            
            ushort CURRENTODD = 0;
            
            ushort CURRENTEVEN = 0;
            
            ushort FIRSTTWELVENUM = 0;
            
            ushort SECONDTWELVENUM = 0;
            
            ushort THIRDTWELVENUM = 0;
            
            ushort COLONESTART = 0;
            
            ushort COLTWOSTART = 0;
            
            ushort COLTHREESTART = 0;
            
            
            __context__.SourceCodeLine = 75;
            CURRENTODD = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 76;
            CURRENTEVEN = (ushort) ( 2 ) ; 
            __context__.SourceCodeLine = 77;
            FIRSTTWELVENUM = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 78;
            SECONDTWELVENUM = (ushort) ( 13 ) ; 
            __context__.SourceCodeLine = 79;
            THIRDTWELVENUM = (ushort) ( 25 ) ; 
            __context__.SourceCodeLine = 80;
            COLONESTART = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 81;
            COLTWOSTART = (ushort) ( 2 ) ; 
            __context__.SourceCodeLine = 82;
            COLTHREESTART = (ushort) ( 3 ) ; 
            __context__.SourceCodeLine = 85;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)37; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__1) && (INDEX  <= __FN_FOREND_VAL__1) ) : ( (INDEX  <= __FN_FORSTART_VAL__1) && (INDEX  >= __FN_FOREND_VAL__1) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 86;
                ALLARRAY [ INDEX] = (ushort) ( (INDEX - 1) ) ; 
                __context__.SourceCodeLine = 85;
                } 
            
            __context__.SourceCodeLine = 89;
            ushort __FN_FORSTART_VAL__2 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__2 = (ushort)18; 
            int __FN_FORSTEP_VAL__2 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__2) && (INDEX  <= __FN_FOREND_VAL__2) ) : ( (INDEX  <= __FN_FORSTART_VAL__2) && (INDEX  >= __FN_FOREND_VAL__2) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__2) 
                { 
                __context__.SourceCodeLine = 90;
                ODDARRAY [ INDEX] = (ushort) ( CURRENTODD ) ; 
                __context__.SourceCodeLine = 91;
                CURRENTODD = (ushort) ( (CURRENTODD + 2) ) ; 
                __context__.SourceCodeLine = 89;
                } 
            
            __context__.SourceCodeLine = 94;
            ushort __FN_FORSTART_VAL__3 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__3 = (ushort)18; 
            int __FN_FORSTEP_VAL__3 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__3; (__FN_FORSTEP_VAL__3 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__3) && (INDEX  <= __FN_FOREND_VAL__3) ) : ( (INDEX  <= __FN_FORSTART_VAL__3) && (INDEX  >= __FN_FOREND_VAL__3) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__3) 
                { 
                __context__.SourceCodeLine = 95;
                EVENARRAY [ INDEX] = (ushort) ( CURRENTEVEN ) ; 
                __context__.SourceCodeLine = 96;
                CURRENTEVEN = (ushort) ( (CURRENTEVEN + 2) ) ; 
                __context__.SourceCodeLine = 94;
                } 
            
            __context__.SourceCodeLine = 99;
            ushort __FN_FORSTART_VAL__4 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__4 = (ushort)12; 
            int __FN_FORSTEP_VAL__4 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__4; (__FN_FORSTEP_VAL__4 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__4) && (INDEX  <= __FN_FOREND_VAL__4) ) : ( (INDEX  <= __FN_FORSTART_VAL__4) && (INDEX  >= __FN_FOREND_VAL__4) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__4) 
                { 
                __context__.SourceCodeLine = 100;
                FIRSTTWELVEARRAY [ INDEX] = (ushort) ( FIRSTTWELVENUM ) ; 
                __context__.SourceCodeLine = 101;
                FIRSTTWELVENUM = (ushort) ( (FIRSTTWELVENUM + 1) ) ; 
                __context__.SourceCodeLine = 99;
                } 
            
            __context__.SourceCodeLine = 104;
            ushort __FN_FORSTART_VAL__5 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__5 = (ushort)12; 
            int __FN_FORSTEP_VAL__5 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__5; (__FN_FORSTEP_VAL__5 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__5) && (INDEX  <= __FN_FOREND_VAL__5) ) : ( (INDEX  <= __FN_FORSTART_VAL__5) && (INDEX  >= __FN_FOREND_VAL__5) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__5) 
                { 
                __context__.SourceCodeLine = 105;
                SECONDTWELVEARRAY [ INDEX] = (ushort) ( SECONDTWELVENUM ) ; 
                __context__.SourceCodeLine = 106;
                SECONDTWELVENUM = (ushort) ( (SECONDTWELVENUM + 1) ) ; 
                __context__.SourceCodeLine = 104;
                } 
            
            __context__.SourceCodeLine = 109;
            ushort __FN_FORSTART_VAL__6 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__6 = (ushort)12; 
            int __FN_FORSTEP_VAL__6 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__6; (__FN_FORSTEP_VAL__6 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__6) && (INDEX  <= __FN_FOREND_VAL__6) ) : ( (INDEX  <= __FN_FORSTART_VAL__6) && (INDEX  >= __FN_FOREND_VAL__6) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__6) 
                { 
                __context__.SourceCodeLine = 110;
                THIRDTWELVEARRAY [ INDEX] = (ushort) ( THIRDTWELVENUM ) ; 
                __context__.SourceCodeLine = 111;
                THIRDTWELVENUM = (ushort) ( (THIRDTWELVENUM + 1) ) ; 
                __context__.SourceCodeLine = 109;
                } 
            
            __context__.SourceCodeLine = 114;
            ushort __FN_FORSTART_VAL__7 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__7 = (ushort)12; 
            int __FN_FORSTEP_VAL__7 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__7; (__FN_FORSTEP_VAL__7 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__7) && (INDEX  <= __FN_FOREND_VAL__7) ) : ( (INDEX  <= __FN_FORSTART_VAL__7) && (INDEX  >= __FN_FOREND_VAL__7) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__7) 
                { 
                __context__.SourceCodeLine = 115;
                COLUMNONEARRAY [ INDEX] = (ushort) ( COLONESTART ) ; 
                __context__.SourceCodeLine = 116;
                COLONESTART = (ushort) ( (COLONESTART + 3) ) ; 
                __context__.SourceCodeLine = 114;
                } 
            
            __context__.SourceCodeLine = 119;
            ushort __FN_FORSTART_VAL__8 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__8 = (ushort)12; 
            int __FN_FORSTEP_VAL__8 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__8; (__FN_FORSTEP_VAL__8 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__8) && (INDEX  <= __FN_FOREND_VAL__8) ) : ( (INDEX  <= __FN_FORSTART_VAL__8) && (INDEX  >= __FN_FOREND_VAL__8) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__8) 
                { 
                __context__.SourceCodeLine = 120;
                COLUMNTWOARRAY [ INDEX] = (ushort) ( COLTWOSTART ) ; 
                __context__.SourceCodeLine = 121;
                COLTWOSTART = (ushort) ( (COLTWOSTART + 3) ) ; 
                __context__.SourceCodeLine = 119;
                } 
            
            __context__.SourceCodeLine = 124;
            ushort __FN_FORSTART_VAL__9 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__9 = (ushort)12; 
            int __FN_FORSTEP_VAL__9 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__9; (__FN_FORSTEP_VAL__9 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__9) && (INDEX  <= __FN_FOREND_VAL__9) ) : ( (INDEX  <= __FN_FORSTART_VAL__9) && (INDEX  >= __FN_FOREND_VAL__9) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__9) 
                { 
                __context__.SourceCodeLine = 125;
                COLUMNTHREEARRAY [ INDEX] = (ushort) ( COLTHREESTART ) ; 
                __context__.SourceCodeLine = 126;
                COLTHREESTART = (ushort) ( (COLTHREESTART + 3) ) ; 
                __context__.SourceCodeLine = 124;
                } 
            
            __context__.SourceCodeLine = 129;
            BLACKS . NUMBER [ 1] = (ushort) ( 2 ) ; 
            __context__.SourceCodeLine = 130;
            BLACKS . NUMBER [ 2] = (ushort) ( 4 ) ; 
            __context__.SourceCodeLine = 131;
            BLACKS . NUMBER [ 3] = (ushort) ( 6 ) ; 
            __context__.SourceCodeLine = 132;
            BLACKS . NUMBER [ 4] = (ushort) ( 8 ) ; 
            __context__.SourceCodeLine = 133;
            BLACKS . NUMBER [ 5] = (ushort) ( 10 ) ; 
            __context__.SourceCodeLine = 134;
            BLACKS . NUMBER [ 6] = (ushort) ( 11 ) ; 
            __context__.SourceCodeLine = 135;
            BLACKS . NUMBER [ 7] = (ushort) ( 13 ) ; 
            __context__.SourceCodeLine = 136;
            BLACKS . NUMBER [ 8] = (ushort) ( 15 ) ; 
            __context__.SourceCodeLine = 137;
            BLACKS . NUMBER [ 9] = (ushort) ( 17 ) ; 
            __context__.SourceCodeLine = 138;
            BLACKS . NUMBER [ 10] = (ushort) ( 20 ) ; 
            __context__.SourceCodeLine = 139;
            BLACKS . NUMBER [ 11] = (ushort) ( 22 ) ; 
            __context__.SourceCodeLine = 140;
            BLACKS . NUMBER [ 12] = (ushort) ( 24 ) ; 
            __context__.SourceCodeLine = 141;
            BLACKS . NUMBER [ 13] = (ushort) ( 26 ) ; 
            __context__.SourceCodeLine = 142;
            BLACKS . NUMBER [ 14] = (ushort) ( 28 ) ; 
            __context__.SourceCodeLine = 143;
            BLACKS . NUMBER [ 15] = (ushort) ( 29 ) ; 
            __context__.SourceCodeLine = 144;
            BLACKS . NUMBER [ 16] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 145;
            BLACKS . NUMBER [ 17] = (ushort) ( 33 ) ; 
            __context__.SourceCodeLine = 146;
            BLACKS . NUMBER [ 18] = (ushort) ( 35 ) ; 
            
            }
            
        private void CHECKODDBETS (  SplusExecutionContext __context__, ushort BET ) 
            { 
            ushort INDEX = 0;
            
            
            __context__.SourceCodeLine = 186;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)18; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__1) && (INDEX  <= __FN_FOREND_VAL__1) ) : ( (INDEX  <= __FN_FORSTART_VAL__1) && (INDEX  >= __FN_FOREND_VAL__1) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 188;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ODDARRAY[ INDEX ] == BET))  ) ) 
                    { 
                    __context__.SourceCodeLine = 190;
                    ODD  .Value = (ushort) ( 1 ) ; 
                    } 
                
                __context__.SourceCodeLine = 186;
                } 
            
            
            }
            
        private void CHECKEVENBETS (  SplusExecutionContext __context__, ushort BET ) 
            { 
            ushort INDEX = 0;
            
            
            __context__.SourceCodeLine = 198;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)18; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__1) && (INDEX  <= __FN_FOREND_VAL__1) ) : ( (INDEX  <= __FN_FORSTART_VAL__1) && (INDEX  >= __FN_FOREND_VAL__1) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 200;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (EVENARRAY[ INDEX ] == BET))  ) ) 
                    { 
                    __context__.SourceCodeLine = 202;
                    EVEN  .Value = (ushort) ( 1 ) ; 
                    } 
                
                __context__.SourceCodeLine = 198;
                } 
            
            
            }
            
        private void CHECKBLACKORRED (  SplusExecutionContext __context__, ushort NUMBER ) 
            { 
            ushort INDEX = 0;
            
            ushort ISMATCH = 0;
            
            
            __context__.SourceCodeLine = 210;
            ISMATCH = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 212;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NUMBER != 0))  ) ) 
                { 
                __context__.SourceCodeLine = 213;
                ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                ushort __FN_FOREND_VAL__1 = (ushort)18; 
                int __FN_FORSTEP_VAL__1 = (int)1; 
                for ( INDEX  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__1) && (INDEX  <= __FN_FOREND_VAL__1) ) : ( (INDEX  <= __FN_FORSTART_VAL__1) && (INDEX  >= __FN_FOREND_VAL__1) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 215;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NUMBER == BLACKS.NUMBER[ INDEX ]))  ) ) 
                        { 
                        __context__.SourceCodeLine = 217;
                        ISMATCH = (ushort) ( 1 ) ; 
                        } 
                    
                    __context__.SourceCodeLine = 213;
                    } 
                
                __context__.SourceCodeLine = 221;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ISMATCH == 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 222;
                    BLACK  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 225;
                    RED  .Value = (ushort) ( 1 ) ; 
                    } 
                
                } 
            
            
            }
            
        private void CHECKTWELVES (  SplusExecutionContext __context__, ushort NUMBER ) 
            { 
            
            __context__.SourceCodeLine = 231;
            FIRSTTWELVE  .Value = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 232;
            SECONDTWELVE  .Value = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 233;
            THIRDTWELVE  .Value = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 235;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( NUMBER >= 1 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( NUMBER <= 12 ) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 236;
                FIRSTTWELVE  .Value = (ushort) ( 1 ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 238;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( NUMBER >= 13 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( NUMBER <= 24 ) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 239;
                    SECONDTWELVE  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 241;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( NUMBER >= 25 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( NUMBER <= 36 ) )) ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 242;
                        THIRDTWELVE  .Value = (ushort) ( 1 ) ; 
                        } 
                    
                    }
                
                }
            
            
            }
            
        private void CHECKCOLUMNS (  SplusExecutionContext __context__, ushort NUMBER ) 
            { 
            ushort INDEX = 0;
            
            
            __context__.SourceCodeLine = 249;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)12; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( INDEX  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (INDEX  >= __FN_FORSTART_VAL__1) && (INDEX  <= __FN_FOREND_VAL__1) ) : ( (INDEX  <= __FN_FORSTART_VAL__1) && (INDEX  >= __FN_FOREND_VAL__1) ) ; INDEX  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 250;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NUMBER == COLUMNONEARRAY[ INDEX ]))  ) ) 
                    { 
                    __context__.SourceCodeLine = 251;
                    COLUMNONE  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 253;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NUMBER == COLUMNTWOARRAY[ INDEX ]))  ) ) 
                        { 
                        __context__.SourceCodeLine = 254;
                        COLUMNTWO  .Value = (ushort) ( 1 ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 256;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NUMBER == COLUMNTHREEARRAY[ INDEX ]))  ) ) 
                            { 
                            __context__.SourceCodeLine = 257;
                            COLUMNTHREE  .Value = (ushort) ( 1 ) ; 
                            } 
                        
                        }
                    
                    }
                
                __context__.SourceCodeLine = 249;
                } 
            
            
            }
            
        object RESETOUTPUTS_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 266;
                ODD  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 267;
                EVEN  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 268;
                BLACK  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 269;
                RED  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 270;
                FIRSTTWELVE  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 271;
                SECONDTWELVE  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 272;
                THIRDTWELVE  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 273;
                COLUMNONE  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 274;
                COLUMNTWO  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 275;
                COLUMNTHREE  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 276;
                ZERO  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 277;
                DBLZERO  .Value = (ushort) ( 0 ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object WINNINGNUMBER_OnChange_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            ushort NUMBER = 0;
            
            
            __context__.SourceCodeLine = 283;
            NUMBER = (ushort) ( WINNINGNUMBER  .UshortValue ) ; 
            __context__.SourceCodeLine = 285;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NUMBER == 0))  ) ) 
                { 
                __context__.SourceCodeLine = 286;
                ZERO  .Value = (ushort) ( 1 ) ; 
                } 
            
            __context__.SourceCodeLine = 287;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NUMBER == 37))  ) ) 
                { 
                __context__.SourceCodeLine = 288;
                DBLZERO  .Value = (ushort) ( 1 ) ; 
                } 
            
            __context__.SourceCodeLine = 290;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMBER != 0) ) && Functions.TestForTrue ( Functions.BoolToInt (NUMBER != 37) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 291;
                Trace( "Checking {0:d}...", (short)NUMBER) ; 
                __context__.SourceCodeLine = 292;
                CHECKODDBETS (  __context__ , (ushort)( NUMBER )) ; 
                __context__.SourceCodeLine = 293;
                CHECKEVENBETS (  __context__ , (ushort)( NUMBER )) ; 
                __context__.SourceCodeLine = 294;
                CHECKBLACKORRED (  __context__ , (ushort)( NUMBER )) ; 
                __context__.SourceCodeLine = 295;
                CHECKTWELVES (  __context__ , (ushort)( NUMBER )) ; 
                __context__.SourceCodeLine = 296;
                CHECKCOLUMNS (  __context__ , (ushort)( NUMBER )) ; 
                } 
            
            __context__.SourceCodeLine = 298;
            WINNINGNUMBEROUT  .Value = (ushort) ( NUMBER ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 308;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 309;
        BUILDARRAYS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    ALLARRAY  = new ushort[ 38 ];
    BETSARRAY  = new ushort[ 101 ];
    ODDARRAY  = new ushort[ 19 ];
    EVENARRAY  = new ushort[ 19 ];
    FIRSTTWELVEARRAY  = new ushort[ 13 ];
    SECONDTWELVEARRAY  = new ushort[ 13 ];
    THIRDTWELVEARRAY  = new ushort[ 13 ];
    COLUMNONEARRAY  = new ushort[ 13 ];
    COLUMNTWOARRAY  = new ushort[ 13 ];
    COLUMNTHREEARRAY  = new ushort[ 13 ];
    BLACKS  = new BLACKNUMBERS( this, true );
    BLACKS .PopulateCustomAttributeList( false );
    ROW  = new STREETS[ 13 ];
    for( uint i = 0; i < 13; i++ )
    {
        ROW [i] = new STREETS( this, true );
        ROW [i].PopulateCustomAttributeList( false );
        
    }
    
    RESETOUTPUTS = new Crestron.Logos.SplusObjects.DigitalInput( RESETOUTPUTS__DigitalInput__, this );
    m_DigitalInputList.Add( RESETOUTPUTS__DigitalInput__, RESETOUTPUTS );
    
    ODD = new Crestron.Logos.SplusObjects.DigitalOutput( ODD__DigitalOutput__, this );
    m_DigitalOutputList.Add( ODD__DigitalOutput__, ODD );
    
    EVEN = new Crestron.Logos.SplusObjects.DigitalOutput( EVEN__DigitalOutput__, this );
    m_DigitalOutputList.Add( EVEN__DigitalOutput__, EVEN );
    
    BLACK = new Crestron.Logos.SplusObjects.DigitalOutput( BLACK__DigitalOutput__, this );
    m_DigitalOutputList.Add( BLACK__DigitalOutput__, BLACK );
    
    RED = new Crestron.Logos.SplusObjects.DigitalOutput( RED__DigitalOutput__, this );
    m_DigitalOutputList.Add( RED__DigitalOutput__, RED );
    
    FIRSTTWELVE = new Crestron.Logos.SplusObjects.DigitalOutput( FIRSTTWELVE__DigitalOutput__, this );
    m_DigitalOutputList.Add( FIRSTTWELVE__DigitalOutput__, FIRSTTWELVE );
    
    SECONDTWELVE = new Crestron.Logos.SplusObjects.DigitalOutput( SECONDTWELVE__DigitalOutput__, this );
    m_DigitalOutputList.Add( SECONDTWELVE__DigitalOutput__, SECONDTWELVE );
    
    THIRDTWELVE = new Crestron.Logos.SplusObjects.DigitalOutput( THIRDTWELVE__DigitalOutput__, this );
    m_DigitalOutputList.Add( THIRDTWELVE__DigitalOutput__, THIRDTWELVE );
    
    COLUMNONE = new Crestron.Logos.SplusObjects.DigitalOutput( COLUMNONE__DigitalOutput__, this );
    m_DigitalOutputList.Add( COLUMNONE__DigitalOutput__, COLUMNONE );
    
    COLUMNTWO = new Crestron.Logos.SplusObjects.DigitalOutput( COLUMNTWO__DigitalOutput__, this );
    m_DigitalOutputList.Add( COLUMNTWO__DigitalOutput__, COLUMNTWO );
    
    COLUMNTHREE = new Crestron.Logos.SplusObjects.DigitalOutput( COLUMNTHREE__DigitalOutput__, this );
    m_DigitalOutputList.Add( COLUMNTHREE__DigitalOutput__, COLUMNTHREE );
    
    ZERO = new Crestron.Logos.SplusObjects.DigitalOutput( ZERO__DigitalOutput__, this );
    m_DigitalOutputList.Add( ZERO__DigitalOutput__, ZERO );
    
    DBLZERO = new Crestron.Logos.SplusObjects.DigitalOutput( DBLZERO__DigitalOutput__, this );
    m_DigitalOutputList.Add( DBLZERO__DigitalOutput__, DBLZERO );
    
    WINNINGNUMBER = new Crestron.Logos.SplusObjects.AnalogInput( WINNINGNUMBER__AnalogSerialInput__, this );
    m_AnalogInputList.Add( WINNINGNUMBER__AnalogSerialInput__, WINNINGNUMBER );
    
    WINNINGNUMBEROUT = new Crestron.Logos.SplusObjects.AnalogOutput( WINNINGNUMBEROUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( WINNINGNUMBEROUT__AnalogSerialOutput__, WINNINGNUMBEROUT );
    
    
    RESETOUTPUTS.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESETOUTPUTS_OnPush_0, false ) );
    WINNINGNUMBER.OnAnalogChange.Add( new InputChangeHandlerWrapper( WINNINGNUMBER_OnChange_1, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_ROULETTE_BETS ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint RESETOUTPUTS__DigitalInput__ = 0;
const uint WINNINGNUMBER__AnalogSerialInput__ = 0;
const uint ODD__DigitalOutput__ = 0;
const uint EVEN__DigitalOutput__ = 1;
const uint BLACK__DigitalOutput__ = 2;
const uint RED__DigitalOutput__ = 3;
const uint FIRSTTWELVE__DigitalOutput__ = 4;
const uint SECONDTWELVE__DigitalOutput__ = 5;
const uint THIRDTWELVE__DigitalOutput__ = 6;
const uint COLUMNONE__DigitalOutput__ = 7;
const uint COLUMNTWO__DigitalOutput__ = 8;
const uint COLUMNTHREE__DigitalOutput__ = 9;
const uint ZERO__DigitalOutput__ = 10;
const uint DBLZERO__DigitalOutput__ = 11;
const uint WINNINGNUMBEROUT__AnalogSerialOutput__ = 0;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}

[SplusStructAttribute(-1, true, false)]
public class BLACKNUMBERS : SplusStructureBase
{

    [SplusStructAttribute(0, false, false)]
    public ushort  [] NUMBER;
    
    
    public BLACKNUMBERS( SplusObject __caller__, bool bIsStructureVolatile ) : base ( __caller__, bIsStructureVolatile )
    {
        NUMBER  = new ushort[ 19 ];
        
        
    }
    
}
[SplusStructAttribute(-1, true, false)]
public class STREETS : SplusStructureBase
{

    [SplusStructAttribute(0, false, false)]
    public ushort  COL1 = 0;
    
    [SplusStructAttribute(1, false, false)]
    public ushort  COL2 = 0;
    
    [SplusStructAttribute(2, false, false)]
    public ushort  COL3 = 0;
    
    
    public STREETS( SplusObject __caller__, bool bIsStructureVolatile ) : base ( __caller__, bIsStructureVolatile )
    {
        
        
    }
    
}

}
