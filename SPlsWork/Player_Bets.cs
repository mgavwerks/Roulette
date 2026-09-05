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

namespace UserModule_PLAYER_BETS
{
    public class UserModuleClass_PLAYER_BETS : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        
        Crestron.Logos.SplusObjects.AnalogInput WINNINGNUMBER;
        Crestron.Logos.SplusObjects.DigitalInput CLEARBETS;
        Crestron.Logos.SplusObjects.DigitalInput RESETGAME;
        Crestron.Logos.SplusObjects.DigitalInput NUMFIRSTTWELVE;
        Crestron.Logos.SplusObjects.DigitalInput NUMSECONDTWELVE;
        Crestron.Logos.SplusObjects.DigitalInput NUMTHIRDTWELVE;
        Crestron.Logos.SplusObjects.DigitalInput NUMEVEN;
        Crestron.Logos.SplusObjects.DigitalInput NUMODD;
        Crestron.Logos.SplusObjects.DigitalInput NUMRED;
        Crestron.Logos.SplusObjects.DigitalInput NUMBLACK;
        Crestron.Logos.SplusObjects.DigitalInput NUMHIGH;
        Crestron.Logos.SplusObjects.DigitalInput NUMLOW;
        Crestron.Logos.SplusObjects.DigitalInput NUMZERO;
        Crestron.Logos.SplusObjects.DigitalInput NUMDBLZERO;
        Crestron.Logos.SplusObjects.DigitalInput BETLOW;
        Crestron.Logos.SplusObjects.DigitalInput BETFIRSTTWELVE;
        Crestron.Logos.SplusObjects.DigitalInput BETEVEN;
        Crestron.Logos.SplusObjects.DigitalInput BETSECONDTWELVE;
        Crestron.Logos.SplusObjects.DigitalInput BETRED;
        Crestron.Logos.SplusObjects.DigitalInput BETBLACK;
        Crestron.Logos.SplusObjects.DigitalInput BETTHIRDTWELVE;
        Crestron.Logos.SplusObjects.DigitalInput BETODD;
        Crestron.Logos.SplusObjects.DigitalInput BETHIGH;
        Crestron.Logos.SplusObjects.DigitalInput BETZERO;
        Crestron.Logos.SplusObjects.DigitalInput BETDBLZERO;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> NUMCOLUMN;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> BETCOLUMN;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> STRAIGHTBET;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> SPLITBET;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> LINEBET;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> STREETBET;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> CORNERBET;
        Crestron.Logos.SplusObjects.StringInput BETAMOUNT__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput ERRORMESSAGE;
        Crestron.Logos.SplusObjects.DigitalOutput DISPLAYERRORMESSAGE;
        Crestron.Logos.SplusObjects.DigitalOutput RESETPUSHED;
        Crestron.Logos.SplusObjects.AnalogOutput PLAYERBANKOUT;
        Crestron.Logos.SplusObjects.AnalogOutput PLAYERTOTALBET;
        Crestron.Logos.SplusObjects.AnalogOutput PLAYERWINNINGS;
        Crestron.Logos.SplusObjects.AnalogOutput BETCOUNT;
        InOutArray<Crestron.Logos.SplusObjects.AnalogOutput> PREVWIN;
        STRUCTBET [] PLAYERBETS;
        ushort TOTALBET = 0;
        ushort PLAYERBANK = 0;
        ushort BETAMOUNT = 0;
        ushort TEMPBETAMOUNT = 0;
        ushort BETSREMAINING = 0;
        ushort ISRESETTING = 0;
        ushort PREVWININDEX = 0;
        CrestronString PLACEBETTYPE;
        private void RETURNPREVIOUSWINNERS (  SplusExecutionContext __context__, ushort WINNER ) 
            { 
            
            __context__.SourceCodeLine = 98;
            PREVWIN [ PREVWININDEX]  .Value = (ushort) ( WINNER ) ; 
            __context__.SourceCodeLine = 99;
            PREVWININDEX = (ushort) ( (PREVWININDEX + 1) ) ; 
            __context__.SourceCodeLine = 101;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PREVWININDEX == 6))  ) ) 
                { 
                __context__.SourceCodeLine = 102;
                PREVWININDEX = (ushort) ( 1 ) ; 
                } 
            
            
            }
            
        private void DISPLAYMESSAGE (  SplusExecutionContext __context__, ushort MSGNUM ) 
            { 
            
            __context__.SourceCodeLine = 107;
            
                {
                int __SPLS_TMPVAR__SWTCH_1__ = ((int)MSGNUM);
                
                    { 
                    if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 1) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 111;
                        ERRORMESSAGE  .UpdateValue ( "Not enough bank. Please adjust your bet or reset the game"  ) ; 
                        __context__.SourceCodeLine = 112;
                        Functions.Pulse ( 200, DISPLAYERRORMESSAGE ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 2) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 116;
                        ERRORMESSAGE  .UpdateValue ( "You do not have any bets remaining."  ) ; 
                        __context__.SourceCodeLine = 117;
                        Functions.Pulse ( 200, DISPLAYERRORMESSAGE ) ; 
                        } 
                    
                    else 
                        { 
                        } 
                    
                    } 
                    
                }
                
            
            
            }
            
        private ushort STRINGBETTOANALOG (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 130;
            BETAMOUNT = (ushort) ( Functions.Atoi( BETAMOUNT__DOLLAR__ ) ) ; 
            __context__.SourceCodeLine = 131;
            return (ushort)( BETAMOUNT) ; 
            
            }
            
        private void SUBTRACTPLAYERBANK (  SplusExecutionContext __context__, ushort BETVALUE ) 
            { 
            
            __context__.SourceCodeLine = 136;
            Trace( "Subracting {0:d} from bank", (short)BETVALUE) ; 
            __context__.SourceCodeLine = 137;
            PLAYERBANK = (ushort) ( (PLAYERBANK - BETVALUE) ) ; 
            __context__.SourceCodeLine = 138;
            PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
            __context__.SourceCodeLine = 140;
            TOTALBET = (ushort) ( (TOTALBET + BETVALUE) ) ; 
            __context__.SourceCodeLine = 141;
            PLAYERTOTALBET  .Value = (ushort) ( TOTALBET ) ; 
            
            }
            
        private void ADDPLAYERBANK (  SplusExecutionContext __context__, ushort BETVALUE ) 
            { 
            
            __context__.SourceCodeLine = 145;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ISRESETTING != 1))  ) ) 
                { 
                __context__.SourceCodeLine = 146;
                Trace( "Adding {0:d} to bank", (short)BETVALUE) ; 
                __context__.SourceCodeLine = 148;
                PLAYERBANK = (ushort) ( (PLAYERBANK + BETVALUE) ) ; 
                __context__.SourceCodeLine = 149;
                PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
                __context__.SourceCodeLine = 151;
                TOTALBET = (ushort) ( (TOTALBET - BETVALUE) ) ; 
                __context__.SourceCodeLine = 152;
                PLAYERTOTALBET  .Value = (ushort) ( TOTALBET ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 153;
                Trace( "Game resetting. AddPlayerBank bypassed.") ; 
                }
            
            
            }
            
        private ushort COUNTREMAININGBETS (  SplusExecutionContext __context__ ) 
            { 
            ushort I = 0;
            
            ushort REMAINING = 0;
            
            
            __context__.SourceCodeLine = 159;
            REMAINING = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 160;
            Trace( "Calculating remaining bets...") ; 
            __context__.SourceCodeLine = 161;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 162;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 163;
                    REMAINING = (ushort) ( (REMAINING + 1) ) ; 
                    } 
                
                __context__.SourceCodeLine = 161;
                } 
            
            __context__.SourceCodeLine = 166;
            Trace( "{0:d} bets counted.", (short)REMAINING) ; 
            __context__.SourceCodeLine = 167;
            BETCOUNT  .Value = (ushort) ( REMAINING ) ; 
            __context__.SourceCodeLine = 168;
            BETSREMAINING = (ushort) ( BETCOUNT  .Value ) ; 
            __context__.SourceCodeLine = 169;
            return (ushort)( REMAINING) ; 
            
            }
            
        private void RESETBETS (  SplusExecutionContext __context__ ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 175;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 176;
                PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Open"  ) ; 
                __context__.SourceCodeLine = 175;
                } 
            
            __context__.SourceCodeLine = 179;
            Trace( "All bets reset.") ; 
            
            }
            
        private void ADDSTRAIGHTBET (  SplusExecutionContext __context__, ushort NUMBERTOBET ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 189;
            Trace( "Storing Straight bet with Number:{0:d} and Bet:{1:d}", (short)NUMBERTOBET, (short)BETAMOUNT) ; 
            __context__.SourceCodeLine = 192;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 193;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 194;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Straight"  ) ; 
                    __context__.SourceCodeLine = 195;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUMBERTOBET ) ; 
                    __context__.SourceCodeLine = 196;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 197;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 192;
                } 
            
            __context__.SourceCodeLine = 200;
            SUBTRACTPLAYERBANK (  __context__ , (ushort)( BETAMOUNT )) ; 
            
            }
            
        private void ADDSPLITBET (  SplusExecutionContext __context__, ushort NUMBER1 , ushort NUMBER2 ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 206;
            Trace( "Storing Split bet with Numbers:{0:d} and {1:d}, with Bet:{2:d}", (short)NUMBER1, (short)NUMBER2, (short)BETAMOUNT) ; 
            __context__.SourceCodeLine = 209;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 210;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 211;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Split"  ) ; 
                    __context__.SourceCodeLine = 212;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUMBER1 ) ; 
                    __context__.SourceCodeLine = 213;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 214;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 209;
                } 
            
            __context__.SourceCodeLine = 219;
            ushort __FN_FORSTART_VAL__2 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__2 = (ushort)50; 
            int __FN_FORSTEP_VAL__2 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (I  >= __FN_FORSTART_VAL__2) && (I  <= __FN_FOREND_VAL__2) ) : ( (I  <= __FN_FORSTART_VAL__2) && (I  >= __FN_FOREND_VAL__2) ) ; I  += (ushort)__FN_FORSTEP_VAL__2) 
                { 
                __context__.SourceCodeLine = 220;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 221;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Split"  ) ; 
                    __context__.SourceCodeLine = 222;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUMBER2 ) ; 
                    __context__.SourceCodeLine = 223;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 224;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 219;
                } 
            
            __context__.SourceCodeLine = 227;
            SUBTRACTPLAYERBANK (  __context__ , (ushort)( BETAMOUNT )) ; 
            
            }
            
        private void ADDSTREETBET (  SplusExecutionContext __context__, ushort NUM1 , ushort NUM2 , ushort NUM3 ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 234;
            Trace( "Storing Street bet with Numbers:{0:d}, {1:d}, and {2:d}, with Bet:{3:d}", (short)NUM1, (short)NUM2, (short)NUM3, (short)BETAMOUNT) ; 
            __context__.SourceCodeLine = 237;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 238;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 239;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Street"  ) ; 
                    __context__.SourceCodeLine = 240;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM1 ) ; 
                    __context__.SourceCodeLine = 241;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 242;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 237;
                } 
            
            __context__.SourceCodeLine = 245;
            ushort __FN_FORSTART_VAL__2 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__2 = (ushort)50; 
            int __FN_FORSTEP_VAL__2 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (I  >= __FN_FORSTART_VAL__2) && (I  <= __FN_FOREND_VAL__2) ) : ( (I  <= __FN_FORSTART_VAL__2) && (I  >= __FN_FOREND_VAL__2) ) ; I  += (ushort)__FN_FORSTEP_VAL__2) 
                { 
                __context__.SourceCodeLine = 246;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 247;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Street"  ) ; 
                    __context__.SourceCodeLine = 248;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM2 ) ; 
                    __context__.SourceCodeLine = 249;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 250;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 245;
                } 
            
            __context__.SourceCodeLine = 253;
            ushort __FN_FORSTART_VAL__3 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__3 = (ushort)50; 
            int __FN_FORSTEP_VAL__3 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__3; (__FN_FORSTEP_VAL__3 > 0)  ? ( (I  >= __FN_FORSTART_VAL__3) && (I  <= __FN_FOREND_VAL__3) ) : ( (I  <= __FN_FORSTART_VAL__3) && (I  >= __FN_FOREND_VAL__3) ) ; I  += (ushort)__FN_FORSTEP_VAL__3) 
                { 
                __context__.SourceCodeLine = 254;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 255;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Street"  ) ; 
                    __context__.SourceCodeLine = 256;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM3 ) ; 
                    __context__.SourceCodeLine = 257;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 258;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 253;
                } 
            
            __context__.SourceCodeLine = 261;
            SUBTRACTPLAYERBANK (  __context__ , (ushort)( BETAMOUNT )) ; 
            
            }
            
        private void ADDCORNERBET (  SplusExecutionContext __context__, ushort NUM1 , ushort NUM2 , ushort NUM3 , ushort NUM4 ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 267;
            Trace( "Storing Corner bet with Numbers:{0:d}, {1:d}, {2:d}, and {3:d}, with Bet:{4:d}", (short)NUM1, (short)NUM2, (short)NUM3, (short)NUM4, (short)BETAMOUNT) ; 
            __context__.SourceCodeLine = 270;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 271;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 272;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
                    __context__.SourceCodeLine = 273;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM1 ) ; 
                    __context__.SourceCodeLine = 274;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 275;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 270;
                } 
            
            __context__.SourceCodeLine = 278;
            ushort __FN_FORSTART_VAL__2 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__2 = (ushort)50; 
            int __FN_FORSTEP_VAL__2 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (I  >= __FN_FORSTART_VAL__2) && (I  <= __FN_FOREND_VAL__2) ) : ( (I  <= __FN_FORSTART_VAL__2) && (I  >= __FN_FOREND_VAL__2) ) ; I  += (ushort)__FN_FORSTEP_VAL__2) 
                { 
                __context__.SourceCodeLine = 279;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 280;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
                    __context__.SourceCodeLine = 281;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM2 ) ; 
                    __context__.SourceCodeLine = 282;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 283;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 278;
                } 
            
            __context__.SourceCodeLine = 286;
            ushort __FN_FORSTART_VAL__3 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__3 = (ushort)50; 
            int __FN_FORSTEP_VAL__3 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__3; (__FN_FORSTEP_VAL__3 > 0)  ? ( (I  >= __FN_FORSTART_VAL__3) && (I  <= __FN_FOREND_VAL__3) ) : ( (I  <= __FN_FORSTART_VAL__3) && (I  >= __FN_FOREND_VAL__3) ) ; I  += (ushort)__FN_FORSTEP_VAL__3) 
                { 
                __context__.SourceCodeLine = 287;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 288;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
                    __context__.SourceCodeLine = 289;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM3 ) ; 
                    __context__.SourceCodeLine = 290;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 291;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 286;
                } 
            
            __context__.SourceCodeLine = 294;
            ushort __FN_FORSTART_VAL__4 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__4 = (ushort)50; 
            int __FN_FORSTEP_VAL__4 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__4; (__FN_FORSTEP_VAL__4 > 0)  ? ( (I  >= __FN_FORSTART_VAL__4) && (I  <= __FN_FOREND_VAL__4) ) : ( (I  <= __FN_FORSTART_VAL__4) && (I  >= __FN_FOREND_VAL__4) ) ; I  += (ushort)__FN_FORSTEP_VAL__4) 
                { 
                __context__.SourceCodeLine = 295;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 296;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
                    __context__.SourceCodeLine = 297;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM4 ) ; 
                    __context__.SourceCodeLine = 298;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 299;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 294;
                } 
            
            __context__.SourceCodeLine = 302;
            SUBTRACTPLAYERBANK (  __context__ , (ushort)( BETAMOUNT )) ; 
            
            }
            
        private void ADDLINEBET (  SplusExecutionContext __context__, ushort NUM1 , ushort NUM2 , ushort NUM3 , ushort NUM4 , ushort NUM5 , ushort NUM6 ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 309;
            Trace( "Storing Line bet with Numbers:{0:d}, {1:d}, {2:d}, {3:d}, {4:d}, and {5:d}, with Bet:{6:d}", (short)NUM1, (short)NUM2, (short)NUM3, (short)NUM4, (short)NUM5, (short)NUM6, (short)BETAMOUNT) ; 
            __context__.SourceCodeLine = 312;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 313;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 314;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Line"  ) ; 
                    __context__.SourceCodeLine = 315;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM1 ) ; 
                    __context__.SourceCodeLine = 316;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 317;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 312;
                } 
            
            __context__.SourceCodeLine = 320;
            ushort __FN_FORSTART_VAL__2 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__2 = (ushort)50; 
            int __FN_FORSTEP_VAL__2 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (I  >= __FN_FORSTART_VAL__2) && (I  <= __FN_FOREND_VAL__2) ) : ( (I  <= __FN_FORSTART_VAL__2) && (I  >= __FN_FOREND_VAL__2) ) ; I  += (ushort)__FN_FORSTEP_VAL__2) 
                { 
                __context__.SourceCodeLine = 321;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 322;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Line"  ) ; 
                    __context__.SourceCodeLine = 323;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM2 ) ; 
                    __context__.SourceCodeLine = 324;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 325;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 320;
                } 
            
            __context__.SourceCodeLine = 328;
            ushort __FN_FORSTART_VAL__3 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__3 = (ushort)50; 
            int __FN_FORSTEP_VAL__3 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__3; (__FN_FORSTEP_VAL__3 > 0)  ? ( (I  >= __FN_FORSTART_VAL__3) && (I  <= __FN_FOREND_VAL__3) ) : ( (I  <= __FN_FORSTART_VAL__3) && (I  >= __FN_FOREND_VAL__3) ) ; I  += (ushort)__FN_FORSTEP_VAL__3) 
                { 
                __context__.SourceCodeLine = 329;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 330;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Line"  ) ; 
                    __context__.SourceCodeLine = 331;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM3 ) ; 
                    __context__.SourceCodeLine = 332;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 333;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 328;
                } 
            
            __context__.SourceCodeLine = 336;
            ushort __FN_FORSTART_VAL__4 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__4 = (ushort)50; 
            int __FN_FORSTEP_VAL__4 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__4; (__FN_FORSTEP_VAL__4 > 0)  ? ( (I  >= __FN_FORSTART_VAL__4) && (I  <= __FN_FOREND_VAL__4) ) : ( (I  <= __FN_FORSTART_VAL__4) && (I  >= __FN_FOREND_VAL__4) ) ; I  += (ushort)__FN_FORSTEP_VAL__4) 
                { 
                __context__.SourceCodeLine = 337;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 338;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Line"  ) ; 
                    __context__.SourceCodeLine = 339;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM4 ) ; 
                    __context__.SourceCodeLine = 340;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 341;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 336;
                } 
            
            __context__.SourceCodeLine = 344;
            ushort __FN_FORSTART_VAL__5 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__5 = (ushort)50; 
            int __FN_FORSTEP_VAL__5 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__5; (__FN_FORSTEP_VAL__5 > 0)  ? ( (I  >= __FN_FORSTART_VAL__5) && (I  <= __FN_FOREND_VAL__5) ) : ( (I  <= __FN_FORSTART_VAL__5) && (I  >= __FN_FOREND_VAL__5) ) ; I  += (ushort)__FN_FORSTEP_VAL__5) 
                { 
                __context__.SourceCodeLine = 345;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 346;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Line"  ) ; 
                    __context__.SourceCodeLine = 347;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM5 ) ; 
                    __context__.SourceCodeLine = 348;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 349;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 344;
                } 
            
            __context__.SourceCodeLine = 352;
            ushort __FN_FORSTART_VAL__6 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__6 = (ushort)50; 
            int __FN_FORSTEP_VAL__6 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__6; (__FN_FORSTEP_VAL__6 > 0)  ? ( (I  >= __FN_FORSTART_VAL__6) && (I  <= __FN_FOREND_VAL__6) ) : ( (I  <= __FN_FORSTART_VAL__6) && (I  >= __FN_FOREND_VAL__6) ) ; I  += (ushort)__FN_FORSTEP_VAL__6) 
                { 
                __context__.SourceCodeLine = 353;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 354;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Line"  ) ; 
                    __context__.SourceCodeLine = 355;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( NUM6 ) ; 
                    __context__.SourceCodeLine = 356;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 357;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 352;
                } 
            
            __context__.SourceCodeLine = 360;
            SUBTRACTPLAYERBANK (  __context__ , (ushort)( BETAMOUNT )) ; 
            
            }
            
        private void REMOVEBET (  SplusExecutionContext __context__, CrestronString BETTYPE , ushort BETNUMBER ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 365;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ISRESETTING != 1))  ) ) 
                { 
                __context__.SourceCodeLine = 366;
                ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                ushort __FN_FOREND_VAL__1 = (ushort)50; 
                int __FN_FORSTEP_VAL__1 = (int)1; 
                for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 367;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == BETNUMBER) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == BETTYPE) )) ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 368;
                        Trace( "Bet Found! Undoing bet for {0:d}, type {1}, at bet number {2:d}", (short)BETNUMBER, BETTYPE , (short)I) ; 
                        __context__.SourceCodeLine = 369;
                        PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Open"  ) ; 
                        __context__.SourceCodeLine = 370;
                        PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( 0 ) ; 
                        __context__.SourceCodeLine = 371;
                        TEMPBETAMOUNT = (ushort) ( PLAYERBETS[ I ].BETAMOUNT ) ; 
                        __context__.SourceCodeLine = 372;
                        PLAYERBETS [ I] . BETAMOUNT = (ushort) ( 0 ) ; 
                        __context__.SourceCodeLine = 373;
                        break ; 
                        } 
                    
                    __context__.SourceCodeLine = 366;
                    } 
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 376;
                Trace( "Game is resetting. RemoveBets bypassed.") ; 
                }
            
            
            }
            
        private void BETACTION (  SplusExecutionContext __context__, CrestronString PLACEBETTYPE , ushort BUTTONINDEX , ushort BUTTONSTATE ) 
            { 
            
            __context__.SourceCodeLine = 380;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 381;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBANK != 0) ) && Functions.TestForTrue ( Functions.BoolToInt ( BETAMOUNT <= PLAYERBANK ) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 382;
                    
                        {
                        int __SPLS_TMPVAR__SWTCH_2__ = ((int)BUTTONINDEX);
                        
                            { 
                            if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 1) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 384;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 385;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 385;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 386;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 387;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 389;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 391;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 391;
                                            ADDSPLITBET (  __context__ , (ushort)( 1 ), (ushort)( 2 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 392;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 1 )) ; 
                                            __context__.SourceCodeLine = 393;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 2 )) ; 
                                            __context__.SourceCodeLine = 394;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 396;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 398;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 398;
                                                ADDSTREETBET (  __context__ , (ushort)( 1 ), (ushort)( 2 ), (ushort)( 3 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 399;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 1 )) ; 
                                                __context__.SourceCodeLine = 400;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 2 )) ; 
                                                __context__.SourceCodeLine = 401;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 3 )) ; 
                                                __context__.SourceCodeLine = 402;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 404;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 406;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 406;
                                                    ADDCORNERBET (  __context__ , (ushort)( 1 ), (ushort)( 2 ), (ushort)( 4 ), (ushort)( 5 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 407;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 1 )) ; 
                                                    __context__.SourceCodeLine = 408;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 2 )) ; 
                                                    __context__.SourceCodeLine = 409;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 3 )) ; 
                                                    __context__.SourceCodeLine = 410;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 4 )) ; 
                                                    __context__.SourceCodeLine = 411;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 413;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 415;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 415;
                                                        ADDLINEBET (  __context__ , (ushort)( 1 ), (ushort)( 2 ), (ushort)( 3 ), (ushort)( 4 ), (ushort)( 5 ), (ushort)( 6 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 416;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 1 )) ; 
                                                        __context__.SourceCodeLine = 417;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 2 )) ; 
                                                        __context__.SourceCodeLine = 418;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 3 )) ; 
                                                        __context__.SourceCodeLine = 419;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 4 )) ; 
                                                        __context__.SourceCodeLine = 420;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 5 )) ; 
                                                        __context__.SourceCodeLine = 421;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 6 )) ; 
                                                        __context__.SourceCodeLine = 422;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 2) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 428;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 429;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 429;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 430;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 431;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 433;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 434;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 434;
                                            ADDSPLITBET (  __context__ , (ushort)( 2 ), (ushort)( 3 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 435;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 2 )) ; 
                                            __context__.SourceCodeLine = 436;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 3 )) ; 
                                            __context__.SourceCodeLine = 437;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 439;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 440;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 440;
                                                ADDSTREETBET (  __context__ , (ushort)( 4 ), (ushort)( 5 ), (ushort)( 6 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 441;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 4 )) ; 
                                                __context__.SourceCodeLine = 442;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 5 )) ; 
                                                __context__.SourceCodeLine = 443;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 6 )) ; 
                                                __context__.SourceCodeLine = 444;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 446;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 447;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 447;
                                                    ADDCORNERBET (  __context__ , (ushort)( 2 ), (ushort)( 3 ), (ushort)( 5 ), (ushort)( 6 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 448;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 2 )) ; 
                                                    __context__.SourceCodeLine = 449;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 3 )) ; 
                                                    __context__.SourceCodeLine = 450;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 5 )) ; 
                                                    __context__.SourceCodeLine = 451;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 6 )) ; 
                                                    __context__.SourceCodeLine = 452;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 454;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 455;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 455;
                                                        ADDLINEBET (  __context__ , (ushort)( 4 ), (ushort)( 5 ), (ushort)( 6 ), (ushort)( 7 ), (ushort)( 8 ), (ushort)( 9 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 456;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 4 )) ; 
                                                        __context__.SourceCodeLine = 457;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 5 )) ; 
                                                        __context__.SourceCodeLine = 458;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 6 )) ; 
                                                        __context__.SourceCodeLine = 459;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 7 )) ; 
                                                        __context__.SourceCodeLine = 460;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 8 )) ; 
                                                        __context__.SourceCodeLine = 461;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 9 )) ; 
                                                        __context__.SourceCodeLine = 462;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 3) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 468;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 469;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 469;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 470;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 471;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 473;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 474;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 474;
                                            ADDSPLITBET (  __context__ , (ushort)( 1 ), (ushort)( 4 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 475;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 1 )) ; 
                                            __context__.SourceCodeLine = 476;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 4 )) ; 
                                            __context__.SourceCodeLine = 477;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 479;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 480;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 480;
                                                ADDSTREETBET (  __context__ , (ushort)( 7 ), (ushort)( 8 ), (ushort)( 9 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 481;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 7 )) ; 
                                                __context__.SourceCodeLine = 482;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 8 )) ; 
                                                __context__.SourceCodeLine = 483;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 9 )) ; 
                                                __context__.SourceCodeLine = 484;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 486;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 487;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 487;
                                                    ADDCORNERBET (  __context__ , (ushort)( 4 ), (ushort)( 5 ), (ushort)( 7 ), (ushort)( 8 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 488;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 4 )) ; 
                                                    __context__.SourceCodeLine = 489;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 5 )) ; 
                                                    __context__.SourceCodeLine = 490;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 7 )) ; 
                                                    __context__.SourceCodeLine = 491;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 8 )) ; 
                                                    __context__.SourceCodeLine = 492;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 494;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 495;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 495;
                                                        ADDLINEBET (  __context__ , (ushort)( 7 ), (ushort)( 8 ), (ushort)( 9 ), (ushort)( 10 ), (ushort)( 11 ), (ushort)( 12 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 496;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 7 )) ; 
                                                        __context__.SourceCodeLine = 497;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 8 )) ; 
                                                        __context__.SourceCodeLine = 498;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 9 )) ; 
                                                        __context__.SourceCodeLine = 499;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 10 )) ; 
                                                        __context__.SourceCodeLine = 500;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 11 )) ; 
                                                        __context__.SourceCodeLine = 501;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 12 )) ; 
                                                        __context__.SourceCodeLine = 502;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 4) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 508;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 509;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 509;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 510;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 511;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 513;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 514;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 514;
                                            ADDSPLITBET (  __context__ , (ushort)( 2 ), (ushort)( 5 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 515;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 2 )) ; 
                                            __context__.SourceCodeLine = 516;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 5 )) ; 
                                            __context__.SourceCodeLine = 517;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 519;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 520;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 520;
                                                ADDSTREETBET (  __context__ , (ushort)( 10 ), (ushort)( 11 ), (ushort)( 12 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 521;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 10 )) ; 
                                                __context__.SourceCodeLine = 522;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 11 )) ; 
                                                __context__.SourceCodeLine = 523;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 12 )) ; 
                                                __context__.SourceCodeLine = 524;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 526;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 527;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 527;
                                                    ADDCORNERBET (  __context__ , (ushort)( 5 ), (ushort)( 6 ), (ushort)( 8 ), (ushort)( 9 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 528;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 5 )) ; 
                                                    __context__.SourceCodeLine = 529;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 6 )) ; 
                                                    __context__.SourceCodeLine = 530;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 8 )) ; 
                                                    __context__.SourceCodeLine = 531;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 9 )) ; 
                                                    __context__.SourceCodeLine = 532;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 534;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 535;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 535;
                                                        ADDLINEBET (  __context__ , (ushort)( 10 ), (ushort)( 11 ), (ushort)( 12 ), (ushort)( 13 ), (ushort)( 14 ), (ushort)( 15 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 536;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 10 )) ; 
                                                        __context__.SourceCodeLine = 537;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 11 )) ; 
                                                        __context__.SourceCodeLine = 538;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 12 )) ; 
                                                        __context__.SourceCodeLine = 539;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 13 )) ; 
                                                        __context__.SourceCodeLine = 540;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 14 )) ; 
                                                        __context__.SourceCodeLine = 541;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 15 )) ; 
                                                        __context__.SourceCodeLine = 542;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 5) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 548;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 549;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 549;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 550;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 551;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 553;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 554;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 554;
                                            ADDSPLITBET (  __context__ , (ushort)( 3 ), (ushort)( 6 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 555;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 3 )) ; 
                                            __context__.SourceCodeLine = 556;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 6 )) ; 
                                            __context__.SourceCodeLine = 557;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 559;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 560;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 560;
                                                ADDSTREETBET (  __context__ , (ushort)( 13 ), (ushort)( 14 ), (ushort)( 15 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 561;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 13 )) ; 
                                                __context__.SourceCodeLine = 562;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 14 )) ; 
                                                __context__.SourceCodeLine = 563;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 15 )) ; 
                                                __context__.SourceCodeLine = 564;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 566;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 567;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 567;
                                                    ADDCORNERBET (  __context__ , (ushort)( 10 ), (ushort)( 11 ), (ushort)( 7 ), (ushort)( 8 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 568;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 10 )) ; 
                                                    __context__.SourceCodeLine = 569;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 11 )) ; 
                                                    __context__.SourceCodeLine = 570;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 7 )) ; 
                                                    __context__.SourceCodeLine = 571;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 8 )) ; 
                                                    __context__.SourceCodeLine = 572;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 574;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 575;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 575;
                                                        ADDLINEBET (  __context__ , (ushort)( 13 ), (ushort)( 14 ), (ushort)( 15 ), (ushort)( 16 ), (ushort)( 17 ), (ushort)( 18 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 576;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 13 )) ; 
                                                        __context__.SourceCodeLine = 577;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 14 )) ; 
                                                        __context__.SourceCodeLine = 578;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 15 )) ; 
                                                        __context__.SourceCodeLine = 579;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 16 )) ; 
                                                        __context__.SourceCodeLine = 580;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 17 )) ; 
                                                        __context__.SourceCodeLine = 581;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 18 )) ; 
                                                        __context__.SourceCodeLine = 582;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 6) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 588;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 589;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 589;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 590;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 591;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 593;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 594;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 594;
                                            ADDSPLITBET (  __context__ , (ushort)( 5 ), (ushort)( 4 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 595;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 5 )) ; 
                                            __context__.SourceCodeLine = 596;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 4 )) ; 
                                            __context__.SourceCodeLine = 597;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 599;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 600;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 600;
                                                ADDSTREETBET (  __context__ , (ushort)( 17 ), (ushort)( 18 ), (ushort)( 16 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 601;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 17 )) ; 
                                                __context__.SourceCodeLine = 602;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 18 )) ; 
                                                __context__.SourceCodeLine = 603;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 16 )) ; 
                                                __context__.SourceCodeLine = 604;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 606;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 607;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 607;
                                                    ADDCORNERBET (  __context__ , (ushort)( 9 ), (ushort)( 11 ), (ushort)( 12 ), (ushort)( 8 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 608;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 9 )) ; 
                                                    __context__.SourceCodeLine = 609;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 11 )) ; 
                                                    __context__.SourceCodeLine = 610;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 12 )) ; 
                                                    __context__.SourceCodeLine = 611;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 8 )) ; 
                                                    __context__.SourceCodeLine = 612;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 614;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 615;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 615;
                                                        ADDLINEBET (  __context__ , (ushort)( 16 ), (ushort)( 17 ), (ushort)( 18 ), (ushort)( 19 ), (ushort)( 20 ), (ushort)( 21 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 616;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 17 )) ; 
                                                        __context__.SourceCodeLine = 617;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 18 )) ; 
                                                        __context__.SourceCodeLine = 618;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 19 )) ; 
                                                        __context__.SourceCodeLine = 619;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 20 )) ; 
                                                        __context__.SourceCodeLine = 620;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 21 )) ; 
                                                        __context__.SourceCodeLine = 621;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 16 )) ; 
                                                        __context__.SourceCodeLine = 622;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 7) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 628;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 629;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 629;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 630;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 631;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 633;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 634;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 634;
                                            ADDSPLITBET (  __context__ , (ushort)( 5 ), (ushort)( 6 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 635;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 5 )) ; 
                                            __context__.SourceCodeLine = 636;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 6 )) ; 
                                            __context__.SourceCodeLine = 637;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 639;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 640;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 640;
                                                ADDSTREETBET (  __context__ , (ushort)( 19 ), (ushort)( 20 ), (ushort)( 21 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 641;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 19 )) ; 
                                                __context__.SourceCodeLine = 642;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 20 )) ; 
                                                __context__.SourceCodeLine = 643;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 21 )) ; 
                                                __context__.SourceCodeLine = 644;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 646;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 647;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 647;
                                                    ADDCORNERBET (  __context__ , (ushort)( 10 ), (ushort)( 11 ), (ushort)( 13 ), (ushort)( 14 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 648;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 10 )) ; 
                                                    __context__.SourceCodeLine = 649;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 11 )) ; 
                                                    __context__.SourceCodeLine = 650;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 13 )) ; 
                                                    __context__.SourceCodeLine = 651;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 14 )) ; 
                                                    __context__.SourceCodeLine = 652;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 654;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 655;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 655;
                                                        ADDLINEBET (  __context__ , (ushort)( 19 ), (ushort)( 20 ), (ushort)( 21 ), (ushort)( 22 ), (ushort)( 23 ), (ushort)( 24 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 656;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 19 )) ; 
                                                        __context__.SourceCodeLine = 657;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 20 )) ; 
                                                        __context__.SourceCodeLine = 658;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 21 )) ; 
                                                        __context__.SourceCodeLine = 659;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 22 )) ; 
                                                        __context__.SourceCodeLine = 660;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 23 )) ; 
                                                        __context__.SourceCodeLine = 661;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 24 )) ; 
                                                        __context__.SourceCodeLine = 662;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 8) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 667;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 668;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 668;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 669;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 670;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 672;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 673;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 673;
                                            ADDSPLITBET (  __context__ , (ushort)( 4 ), (ushort)( 7 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 674;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 7 )) ; 
                                            __context__.SourceCodeLine = 675;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 4 )) ; 
                                            __context__.SourceCodeLine = 676;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 678;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 679;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 679;
                                                ADDSTREETBET (  __context__ , (ushort)( 22 ), (ushort)( 23 ), (ushort)( 24 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 680;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 22 )) ; 
                                                __context__.SourceCodeLine = 681;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 23 )) ; 
                                                __context__.SourceCodeLine = 682;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 24 )) ; 
                                                __context__.SourceCodeLine = 683;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 685;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 686;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 686;
                                                    ADDCORNERBET (  __context__ , (ushort)( 11 ), (ushort)( 12 ), (ushort)( 14 ), (ushort)( 15 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 687;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 11 )) ; 
                                                    __context__.SourceCodeLine = 688;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 12 )) ; 
                                                    __context__.SourceCodeLine = 689;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 14 )) ; 
                                                    __context__.SourceCodeLine = 690;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 15 )) ; 
                                                    __context__.SourceCodeLine = 691;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 693;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 694;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 694;
                                                        ADDLINEBET (  __context__ , (ushort)( 22 ), (ushort)( 23 ), (ushort)( 24 ), (ushort)( 25 ), (ushort)( 26 ), (ushort)( 27 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 695;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 22 )) ; 
                                                        __context__.SourceCodeLine = 696;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 23 )) ; 
                                                        __context__.SourceCodeLine = 697;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 24 )) ; 
                                                        __context__.SourceCodeLine = 698;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 25 )) ; 
                                                        __context__.SourceCodeLine = 699;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 26 )) ; 
                                                        __context__.SourceCodeLine = 700;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 27 )) ; 
                                                        __context__.SourceCodeLine = 701;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 9) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 707;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 708;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 708;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 709;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 710;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 712;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 713;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 713;
                                            ADDSPLITBET (  __context__ , (ushort)( 5 ), (ushort)( 8 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 714;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 5 )) ; 
                                            __context__.SourceCodeLine = 715;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 8 )) ; 
                                            __context__.SourceCodeLine = 716;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 718;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 719;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 719;
                                                ADDSTREETBET (  __context__ , (ushort)( 25 ), (ushort)( 26 ), (ushort)( 27 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 720;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 25 )) ; 
                                                __context__.SourceCodeLine = 721;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 26 )) ; 
                                                __context__.SourceCodeLine = 722;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 27 )) ; 
                                                __context__.SourceCodeLine = 723;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 725;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 726;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 726;
                                                    ADDCORNERBET (  __context__ , (ushort)( 13 ), (ushort)( 14 ), (ushort)( 16 ), (ushort)( 17 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 727;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 13 )) ; 
                                                    __context__.SourceCodeLine = 728;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 14 )) ; 
                                                    __context__.SourceCodeLine = 729;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 16 )) ; 
                                                    __context__.SourceCodeLine = 730;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 17 )) ; 
                                                    __context__.SourceCodeLine = 731;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 733;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 734;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 734;
                                                        ADDLINEBET (  __context__ , (ushort)( 25 ), (ushort)( 26 ), (ushort)( 27 ), (ushort)( 28 ), (ushort)( 29 ), (ushort)( 30 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 735;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 25 )) ; 
                                                        __context__.SourceCodeLine = 736;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 26 )) ; 
                                                        __context__.SourceCodeLine = 737;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 27 )) ; 
                                                        __context__.SourceCodeLine = 738;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 28 )) ; 
                                                        __context__.SourceCodeLine = 739;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 29 )) ; 
                                                        __context__.SourceCodeLine = 740;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 30 )) ; 
                                                        __context__.SourceCodeLine = 741;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 10) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 746;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 747;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 747;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 748;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 749;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 751;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 752;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 752;
                                            ADDSPLITBET (  __context__ , (ushort)( 6 ), (ushort)( 9 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 753;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 6 )) ; 
                                            __context__.SourceCodeLine = 754;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 9 )) ; 
                                            __context__.SourceCodeLine = 755;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 757;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 758;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 758;
                                                ADDSTREETBET (  __context__ , (ushort)( 28 ), (ushort)( 29 ), (ushort)( 30 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 759;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 28 )) ; 
                                                __context__.SourceCodeLine = 760;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 29 )) ; 
                                                __context__.SourceCodeLine = 761;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 30 )) ; 
                                                __context__.SourceCodeLine = 762;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 764;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 765;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 765;
                                                    ADDCORNERBET (  __context__ , (ushort)( 14 ), (ushort)( 15 ), (ushort)( 17 ), (ushort)( 18 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 766;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 14 )) ; 
                                                    __context__.SourceCodeLine = 767;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 15 )) ; 
                                                    __context__.SourceCodeLine = 768;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 17 )) ; 
                                                    __context__.SourceCodeLine = 769;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 18 )) ; 
                                                    __context__.SourceCodeLine = 770;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 772;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 773;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 773;
                                                        ADDLINEBET (  __context__ , (ushort)( 28 ), (ushort)( 29 ), (ushort)( 30 ), (ushort)( 31 ), (ushort)( 32 ), (ushort)( 33 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 774;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 28 )) ; 
                                                        __context__.SourceCodeLine = 775;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 29 )) ; 
                                                        __context__.SourceCodeLine = 776;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 30 )) ; 
                                                        __context__.SourceCodeLine = 777;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 31 )) ; 
                                                        __context__.SourceCodeLine = 778;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 32 )) ; 
                                                        __context__.SourceCodeLine = 779;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 33 )) ; 
                                                        __context__.SourceCodeLine = 780;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 11) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 785;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 786;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 786;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 787;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 788;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 790;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 791;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 791;
                                            ADDSPLITBET (  __context__ , (ushort)( 7 ), (ushort)( 8 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 792;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 7 )) ; 
                                            __context__.SourceCodeLine = 793;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 8 )) ; 
                                            __context__.SourceCodeLine = 794;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 796;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 797;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 797;
                                                ADDSTREETBET (  __context__ , (ushort)( 31 ), (ushort)( 32 ), (ushort)( 33 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 798;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 31 )) ; 
                                                __context__.SourceCodeLine = 799;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 32 )) ; 
                                                __context__.SourceCodeLine = 800;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 33 )) ; 
                                                __context__.SourceCodeLine = 801;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 803;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 804;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 804;
                                                    ADDCORNERBET (  __context__ , (ushort)( 16 ), (ushort)( 17 ), (ushort)( 19 ), (ushort)( 20 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 805;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 16 )) ; 
                                                    __context__.SourceCodeLine = 806;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 17 )) ; 
                                                    __context__.SourceCodeLine = 807;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 19 )) ; 
                                                    __context__.SourceCodeLine = 808;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 20 )) ; 
                                                    __context__.SourceCodeLine = 809;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 811;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 812;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 812;
                                                        ADDLINEBET (  __context__ , (ushort)( 31 ), (ushort)( 32 ), (ushort)( 33 ), (ushort)( 34 ), (ushort)( 35 ), (ushort)( 36 )) ; 
                                                        } 
                                                    
                                                    else 
                                                        { 
                                                        __context__.SourceCodeLine = 813;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 31 )) ; 
                                                        __context__.SourceCodeLine = 814;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 32 )) ; 
                                                        __context__.SourceCodeLine = 815;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 33 )) ; 
                                                        __context__.SourceCodeLine = 816;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 34 )) ; 
                                                        __context__.SourceCodeLine = 817;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 35 )) ; 
                                                        __context__.SourceCodeLine = 818;
                                                        REMOVEBET (  __context__ , "Line", (ushort)( 36 )) ; 
                                                        __context__.SourceCodeLine = 819;
                                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                        } 
                                                    
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 12) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 824;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 825;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 825;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 826;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 827;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 829;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 830;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 830;
                                            ADDSPLITBET (  __context__ , (ushort)( 8 ), (ushort)( 9 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 831;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 8 )) ; 
                                            __context__.SourceCodeLine = 832;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 9 )) ; 
                                            __context__.SourceCodeLine = 833;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 835;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 836;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 836;
                                                ADDSTREETBET (  __context__ , (ushort)( 34 ), (ushort)( 35 ), (ushort)( 36 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 837;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 34 )) ; 
                                                __context__.SourceCodeLine = 838;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 35 )) ; 
                                                __context__.SourceCodeLine = 839;
                                                REMOVEBET (  __context__ , "Street", (ushort)( 36 )) ; 
                                                __context__.SourceCodeLine = 840;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 842;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 843;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 843;
                                                    ADDCORNERBET (  __context__ , (ushort)( 17 ), (ushort)( 18 ), (ushort)( 20 ), (ushort)( 21 )) ; 
                                                    } 
                                                
                                                else 
                                                    { 
                                                    __context__.SourceCodeLine = 844;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 17 )) ; 
                                                    __context__.SourceCodeLine = 845;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 18 )) ; 
                                                    __context__.SourceCodeLine = 846;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 20 )) ; 
                                                    __context__.SourceCodeLine = 847;
                                                    REMOVEBET (  __context__ , "Corner", (ushort)( 21 )) ; 
                                                    __context__.SourceCodeLine = 848;
                                                    ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                    } 
                                                
                                                } 
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 13) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 853;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 854;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 854;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 855;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 856;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 858;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 859;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 859;
                                            ADDSPLITBET (  __context__ , (ushort)( 7 ), (ushort)( 10 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 860;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 7 )) ; 
                                            __context__.SourceCodeLine = 861;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 10 )) ; 
                                            __context__.SourceCodeLine = 862;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 864;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 865;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 865;
                                                ADDCORNERBET (  __context__ , (ushort)( 19 ), (ushort)( 20 ), (ushort)( 22 ), (ushort)( 23 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 866;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 19 )) ; 
                                                __context__.SourceCodeLine = 867;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 20 )) ; 
                                                __context__.SourceCodeLine = 868;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 22 )) ; 
                                                __context__.SourceCodeLine = 869;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 23 )) ; 
                                                __context__.SourceCodeLine = 870;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 14) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 875;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 876;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 876;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 877;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 878;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 880;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 881;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 881;
                                            ADDSPLITBET (  __context__ , (ushort)( 8 ), (ushort)( 11 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 882;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 8 )) ; 
                                            __context__.SourceCodeLine = 883;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 11 )) ; 
                                            __context__.SourceCodeLine = 884;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 886;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 887;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 887;
                                                ADDCORNERBET (  __context__ , (ushort)( 20 ), (ushort)( 21 ), (ushort)( 23 ), (ushort)( 24 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 888;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 20 )) ; 
                                                __context__.SourceCodeLine = 889;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 21 )) ; 
                                                __context__.SourceCodeLine = 890;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 24 )) ; 
                                                __context__.SourceCodeLine = 891;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 23 )) ; 
                                                __context__.SourceCodeLine = 892;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 15) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 897;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 898;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 898;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 899;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 900;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 902;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 903;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 903;
                                            ADDSPLITBET (  __context__ , (ushort)( 9 ), (ushort)( 12 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 904;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 9 )) ; 
                                            __context__.SourceCodeLine = 905;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 12 )) ; 
                                            __context__.SourceCodeLine = 906;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 908;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 909;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 909;
                                                ADDCORNERBET (  __context__ , (ushort)( 22 ), (ushort)( 23 ), (ushort)( 25 ), (ushort)( 26 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 910;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 22 )) ; 
                                                __context__.SourceCodeLine = 911;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 23 )) ; 
                                                __context__.SourceCodeLine = 912;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 25 )) ; 
                                                __context__.SourceCodeLine = 913;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 26 )) ; 
                                                __context__.SourceCodeLine = 914;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 16) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 919;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 920;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 920;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 921;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 922;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 924;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 925;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 925;
                                            ADDSPLITBET (  __context__ , (ushort)( 10 ), (ushort)( 11 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 926;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 11 )) ; 
                                            __context__.SourceCodeLine = 927;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 10 )) ; 
                                            __context__.SourceCodeLine = 928;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 930;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 931;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 931;
                                                ADDCORNERBET (  __context__ , (ushort)( 23 ), (ushort)( 24 ), (ushort)( 26 ), (ushort)( 27 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 932;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 23 )) ; 
                                                __context__.SourceCodeLine = 933;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 24 )) ; 
                                                __context__.SourceCodeLine = 934;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 26 )) ; 
                                                __context__.SourceCodeLine = 935;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 27 )) ; 
                                                __context__.SourceCodeLine = 936;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 17) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 941;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 942;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 942;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 943;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 944;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 946;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 947;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 947;
                                            ADDSPLITBET (  __context__ , (ushort)( 11 ), (ushort)( 12 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 948;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 11 )) ; 
                                            __context__.SourceCodeLine = 949;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 12 )) ; 
                                            __context__.SourceCodeLine = 950;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 952;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 953;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 953;
                                                ADDCORNERBET (  __context__ , (ushort)( 25 ), (ushort)( 26 ), (ushort)( 28 ), (ushort)( 29 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 954;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 25 )) ; 
                                                __context__.SourceCodeLine = 955;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 26 )) ; 
                                                __context__.SourceCodeLine = 956;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 28 )) ; 
                                                __context__.SourceCodeLine = 957;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 29 )) ; 
                                                __context__.SourceCodeLine = 958;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 18) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 963;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 964;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 964;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 965;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 966;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 968;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 969;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 969;
                                            ADDSPLITBET (  __context__ , (ushort)( 10 ), (ushort)( 13 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 970;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 13 )) ; 
                                            __context__.SourceCodeLine = 971;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 10 )) ; 
                                            __context__.SourceCodeLine = 972;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 974;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 975;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 975;
                                                ADDCORNERBET (  __context__ , (ushort)( 26 ), (ushort)( 27 ), (ushort)( 29 ), (ushort)( 30 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 976;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 26 )) ; 
                                                __context__.SourceCodeLine = 977;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 27 )) ; 
                                                __context__.SourceCodeLine = 978;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 29 )) ; 
                                                __context__.SourceCodeLine = 979;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 30 )) ; 
                                                __context__.SourceCodeLine = 980;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 19) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 985;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 986;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 986;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 987;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 988;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 990;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 991;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 991;
                                            ADDSPLITBET (  __context__ , (ushort)( 11 ), (ushort)( 14 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 992;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 11 )) ; 
                                            __context__.SourceCodeLine = 993;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 14 )) ; 
                                            __context__.SourceCodeLine = 994;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 996;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 997;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 997;
                                                ADDCORNERBET (  __context__ , (ushort)( 28 ), (ushort)( 29 ), (ushort)( 31 ), (ushort)( 32 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 998;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 28 )) ; 
                                                __context__.SourceCodeLine = 999;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 29 )) ; 
                                                __context__.SourceCodeLine = 1000;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 31 )) ; 
                                                __context__.SourceCodeLine = 1001;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 32 )) ; 
                                                __context__.SourceCodeLine = 1002;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 20) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1007;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1008;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1008;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1009;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1010;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1012;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1013;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1013;
                                            ADDSPLITBET (  __context__ , (ushort)( 12 ), (ushort)( 15 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1014;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 12 )) ; 
                                            __context__.SourceCodeLine = 1015;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 15 )) ; 
                                            __context__.SourceCodeLine = 1016;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 1018;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1019;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 1019;
                                                ADDCORNERBET (  __context__ , (ushort)( 29 ), (ushort)( 30 ), (ushort)( 32 ), (ushort)( 33 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 1020;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 29 )) ; 
                                                __context__.SourceCodeLine = 1021;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 30 )) ; 
                                                __context__.SourceCodeLine = 1022;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 32 )) ; 
                                                __context__.SourceCodeLine = 1023;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 33 )) ; 
                                                __context__.SourceCodeLine = 1024;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 21) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1029;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1030;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1030;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1031;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1032;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1034;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1035;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1035;
                                            ADDSPLITBET (  __context__ , (ushort)( 13 ), (ushort)( 14 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1036;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 13 )) ; 
                                            __context__.SourceCodeLine = 1037;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 14 )) ; 
                                            __context__.SourceCodeLine = 1038;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 1040;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1041;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 1041;
                                                ADDCORNERBET (  __context__ , (ushort)( 31 ), (ushort)( 32 ), (ushort)( 34 ), (ushort)( 35 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 1042;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 31 )) ; 
                                                __context__.SourceCodeLine = 1043;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 32 )) ; 
                                                __context__.SourceCodeLine = 1044;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 34 )) ; 
                                                __context__.SourceCodeLine = 1045;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 35 )) ; 
                                                __context__.SourceCodeLine = 1046;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 22) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1051;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1052;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1052;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1053;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1054;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1056;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1057;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1057;
                                            ADDSPLITBET (  __context__ , (ushort)( 14 ), (ushort)( 15 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1058;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 14 )) ; 
                                            __context__.SourceCodeLine = 1059;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 15 )) ; 
                                            __context__.SourceCodeLine = 1060;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 1062;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1063;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 1063;
                                                ADDCORNERBET (  __context__ , (ushort)( 32 ), (ushort)( 33 ), (ushort)( 35 ), (ushort)( 36 )) ; 
                                                } 
                                            
                                            else 
                                                { 
                                                __context__.SourceCodeLine = 1064;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 32 )) ; 
                                                __context__.SourceCodeLine = 1065;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 33 )) ; 
                                                __context__.SourceCodeLine = 1066;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 35 )) ; 
                                                __context__.SourceCodeLine = 1067;
                                                REMOVEBET (  __context__ , "Corner", (ushort)( 36 )) ; 
                                                __context__.SourceCodeLine = 1068;
                                                ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 23) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1073;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1074;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1074;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1075;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1076;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1078;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1079;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1079;
                                            ADDSPLITBET (  __context__ , (ushort)( 13 ), (ushort)( 16 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1080;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 13 )) ; 
                                            __context__.SourceCodeLine = 1081;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 16 )) ; 
                                            __context__.SourceCodeLine = 1082;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 24) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1087;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1088;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1088;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1089;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1090;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1092;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1093;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1093;
                                            ADDSPLITBET (  __context__ , (ushort)( 14 ), (ushort)( 17 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1094;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 14 )) ; 
                                            __context__.SourceCodeLine = 1095;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 17 )) ; 
                                            __context__.SourceCodeLine = 1096;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 25) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1101;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1102;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1102;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1103;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1104;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1106;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1107;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1107;
                                            ADDSPLITBET (  __context__ , (ushort)( 15 ), (ushort)( 18 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1108;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 18 )) ; 
                                            __context__.SourceCodeLine = 1109;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 15 )) ; 
                                            __context__.SourceCodeLine = 1110;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 26) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1115;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1116;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1116;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1117;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1118;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1120;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1121;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1121;
                                            ADDSPLITBET (  __context__ , (ushort)( 16 ), (ushort)( 17 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1122;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 16 )) ; 
                                            __context__.SourceCodeLine = 1123;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 17 )) ; 
                                            __context__.SourceCodeLine = 1124;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 27) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1129;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1130;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1130;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1131;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1132;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1134;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1135;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1135;
                                            ADDSPLITBET (  __context__ , (ushort)( 17 ), (ushort)( 18 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1136;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 17 )) ; 
                                            __context__.SourceCodeLine = 1137;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 18 )) ; 
                                            __context__.SourceCodeLine = 1138;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 28) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1143;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1144;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1144;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1145;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1146;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1148;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1149;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1149;
                                            ADDSPLITBET (  __context__ , (ushort)( 16 ), (ushort)( 19 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1150;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 16 )) ; 
                                            __context__.SourceCodeLine = 1151;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 19 )) ; 
                                            __context__.SourceCodeLine = 1152;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 29) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1157;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1158;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1158;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1159;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1160;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1162;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1163;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1163;
                                            ADDSPLITBET (  __context__ , (ushort)( 17 ), (ushort)( 20 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1164;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 17 )) ; 
                                            __context__.SourceCodeLine = 1165;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 20 )) ; 
                                            __context__.SourceCodeLine = 1166;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 30) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1171;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1172;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1172;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1173;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1174;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1176;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1177;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1177;
                                            ADDSPLITBET (  __context__ , (ushort)( 18 ), (ushort)( 21 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1178;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 18 )) ; 
                                            __context__.SourceCodeLine = 1179;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 21 )) ; 
                                            __context__.SourceCodeLine = 1180;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 31) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1185;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1186;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1186;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1187;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1188;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1190;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1191;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1191;
                                            ADDSPLITBET (  __context__ , (ushort)( 19 ), (ushort)( 20 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1192;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 19 )) ; 
                                            __context__.SourceCodeLine = 1193;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 20 )) ; 
                                            __context__.SourceCodeLine = 1194;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 32) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1199;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1200;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1200;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1201;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1202;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1204;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1205;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1205;
                                            ADDSPLITBET (  __context__ , (ushort)( 20 ), (ushort)( 21 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1206;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 20 )) ; 
                                            __context__.SourceCodeLine = 1207;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 21 )) ; 
                                            __context__.SourceCodeLine = 1208;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 33) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1213;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1214;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1214;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1215;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1216;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1218;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1219;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1219;
                                            ADDSPLITBET (  __context__ , (ushort)( 19 ), (ushort)( 22 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1220;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 19 )) ; 
                                            __context__.SourceCodeLine = 1221;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 22 )) ; 
                                            __context__.SourceCodeLine = 1222;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 34) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1227;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1228;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1228;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1229;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1230;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1232;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1233;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1233;
                                            ADDSPLITBET (  __context__ , (ushort)( 20 ), (ushort)( 23 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1234;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 20 )) ; 
                                            __context__.SourceCodeLine = 1235;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 23 )) ; 
                                            __context__.SourceCodeLine = 1236;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 35) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1241;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1242;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1242;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1243;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1244;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1246;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1247;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1247;
                                            ADDSPLITBET (  __context__ , (ushort)( 21 ), (ushort)( 24 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1248;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 21 )) ; 
                                            __context__.SourceCodeLine = 1249;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 24 )) ; 
                                            __context__.SourceCodeLine = 1250;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 36) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1255;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1256;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1256;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( BUTTONINDEX )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1257;
                                        REMOVEBET (  __context__ , "Straight", (ushort)( BUTTONINDEX )) ; 
                                        __context__.SourceCodeLine = 1258;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 1260;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1261;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 1261;
                                            ADDSPLITBET (  __context__ , (ushort)( 22 ), (ushort)( 23 )) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 1262;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 22 )) ; 
                                            __context__.SourceCodeLine = 1263;
                                            REMOVEBET (  __context__ , "Split", (ushort)( 23 )) ; 
                                            __context__.SourceCodeLine = 1264;
                                            ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                            } 
                                        
                                        } 
                                    
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 37) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1269;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1270;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1270;
                                        ADDSPLITBET (  __context__ , (ushort)( 23 ), (ushort)( 24 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1271;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 23 )) ; 
                                        __context__.SourceCodeLine = 1272;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 24 )) ; 
                                        __context__.SourceCodeLine = 1273;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 38) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1278;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1279;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1279;
                                        ADDSPLITBET (  __context__ , (ushort)( 22 ), (ushort)( 25 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1280;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 22 )) ; 
                                        __context__.SourceCodeLine = 1281;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 25 )) ; 
                                        __context__.SourceCodeLine = 1282;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 39) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1287;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1288;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1288;
                                        ADDSPLITBET (  __context__ , (ushort)( 23 ), (ushort)( 26 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1289;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 23 )) ; 
                                        __context__.SourceCodeLine = 1290;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 26 )) ; 
                                        __context__.SourceCodeLine = 1291;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 40) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1296;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1297;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1297;
                                        ADDSPLITBET (  __context__ , (ushort)( 24 ), (ushort)( 27 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1298;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 24 )) ; 
                                        __context__.SourceCodeLine = 1299;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 27 )) ; 
                                        __context__.SourceCodeLine = 1300;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 41) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1305;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1306;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1306;
                                        ADDSPLITBET (  __context__ , (ushort)( 25 ), (ushort)( 26 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1307;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 25 )) ; 
                                        __context__.SourceCodeLine = 1308;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 26 )) ; 
                                        __context__.SourceCodeLine = 1309;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 42) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1314;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1315;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1315;
                                        ADDSPLITBET (  __context__ , (ushort)( 26 ), (ushort)( 27 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1316;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 26 )) ; 
                                        __context__.SourceCodeLine = 1317;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 27 )) ; 
                                        __context__.SourceCodeLine = 1318;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 43) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1323;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1324;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1324;
                                        ADDSPLITBET (  __context__ , (ushort)( 25 ), (ushort)( 28 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1325;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 25 )) ; 
                                        __context__.SourceCodeLine = 1326;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 28 )) ; 
                                        __context__.SourceCodeLine = 1327;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 44) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1332;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1333;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1333;
                                        ADDSPLITBET (  __context__ , (ushort)( 26 ), (ushort)( 29 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1334;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 26 )) ; 
                                        __context__.SourceCodeLine = 1335;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 29 )) ; 
                                        __context__.SourceCodeLine = 1336;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 45) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1341;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1342;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1342;
                                        ADDSPLITBET (  __context__ , (ushort)( 27 ), (ushort)( 30 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1343;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 27 )) ; 
                                        __context__.SourceCodeLine = 1344;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 30 )) ; 
                                        __context__.SourceCodeLine = 1345;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 46) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1350;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1351;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1351;
                                        ADDSPLITBET (  __context__ , (ushort)( 28 ), (ushort)( 29 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1352;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 28 )) ; 
                                        __context__.SourceCodeLine = 1353;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 29 )) ; 
                                        __context__.SourceCodeLine = 1354;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 47) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1359;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1360;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1360;
                                        ADDSPLITBET (  __context__ , (ushort)( 29 ), (ushort)( 30 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1361;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 29 )) ; 
                                        __context__.SourceCodeLine = 1362;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 30 )) ; 
                                        __context__.SourceCodeLine = 1363;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 48) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1368;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1369;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1369;
                                        ADDSPLITBET (  __context__ , (ushort)( 28 ), (ushort)( 31 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1370;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 28 )) ; 
                                        __context__.SourceCodeLine = 1371;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 31 )) ; 
                                        __context__.SourceCodeLine = 1372;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 49) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1377;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1378;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1378;
                                        ADDSPLITBET (  __context__ , (ushort)( 29 ), (ushort)( 32 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1379;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 29 )) ; 
                                        __context__.SourceCodeLine = 1380;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 32 )) ; 
                                        __context__.SourceCodeLine = 1381;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 50) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1386;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1387;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1387;
                                        ADDSPLITBET (  __context__ , (ushort)( 30 ), (ushort)( 33 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1388;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 30 )) ; 
                                        __context__.SourceCodeLine = 1389;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 33 )) ; 
                                        __context__.SourceCodeLine = 1390;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 51) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1395;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1396;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1396;
                                        ADDSPLITBET (  __context__ , (ushort)( 31 ), (ushort)( 32 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1397;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 31 )) ; 
                                        __context__.SourceCodeLine = 1398;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 32 )) ; 
                                        __context__.SourceCodeLine = 1399;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 52) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1404;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1405;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1405;
                                        ADDSPLITBET (  __context__ , (ushort)( 32 ), (ushort)( 33 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1406;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 32 )) ; 
                                        __context__.SourceCodeLine = 1407;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 33 )) ; 
                                        __context__.SourceCodeLine = 1408;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 53) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1413;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1414;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1414;
                                        ADDSPLITBET (  __context__ , (ushort)( 31 ), (ushort)( 34 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1415;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 31 )) ; 
                                        __context__.SourceCodeLine = 1416;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 34 )) ; 
                                        __context__.SourceCodeLine = 1417;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 54) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1422;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1423;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1423;
                                        ADDSPLITBET (  __context__ , (ushort)( 32 ), (ushort)( 35 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1424;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 32 )) ; 
                                        __context__.SourceCodeLine = 1425;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 35 )) ; 
                                        __context__.SourceCodeLine = 1426;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 55) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1431;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1432;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1432;
                                        ADDSPLITBET (  __context__ , (ushort)( 33 ), (ushort)( 36 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1433;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 33 )) ; 
                                        __context__.SourceCodeLine = 1434;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 36 )) ; 
                                        __context__.SourceCodeLine = 1435;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 56) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1440;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1441;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1441;
                                        ADDSPLITBET (  __context__ , (ushort)( 34 ), (ushort)( 35 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1442;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 34 )) ; 
                                        __context__.SourceCodeLine = 1443;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 35 )) ; 
                                        __context__.SourceCodeLine = 1444;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 57) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1449;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1450;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BUTTONSTATE == 1))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 1450;
                                        ADDSPLITBET (  __context__ , (ushort)( 35 ), (ushort)( 36 )) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 1451;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 35 )) ; 
                                        __context__.SourceCodeLine = 1452;
                                        REMOVEBET (  __context__ , "Split", (ushort)( 36 )) ; 
                                        __context__.SourceCodeLine = 1453;
                                        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else 
                                { 
                                } 
                            
                            } 
                            
                        }
                        
                    
                    __context__.SourceCodeLine = 1459;
                    COUNTREMAININGBETS (  __context__  ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 1461;
                    DISPLAYMESSAGE (  __context__ , (ushort)( 1 )) ; 
                    } 
                
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 1463;
                DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
                } 
            
            
            }
            
        private void PLACEOUTSIDEBET (  SplusExecutionContext __context__, CrestronString POSITION , ushort BETNUMBER ) 
            { 
            ushort I = 0;
            
            
            __context__.SourceCodeLine = 1469;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)50; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 1470;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Open"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1471;
                    PLAYERBETS [ I] . BETTYPE  .UpdateValue ( "Outside"  ) ; 
                    __context__.SourceCodeLine = 1472;
                    PLAYERBETS [ I] . NUMBERPLAYED = (ushort) ( BETNUMBER ) ; 
                    __context__.SourceCodeLine = 1473;
                    PLAYERBETS [ I] . BETAMOUNT = (ushort) ( BETAMOUNT ) ; 
                    __context__.SourceCodeLine = 1474;
                    break ; 
                    } 
                
                __context__.SourceCodeLine = 1469;
                } 
            
            __context__.SourceCodeLine = 1477;
            Trace( "Placing Outside bet: {0}, with value: {1:d}", POSITION , (short)BETAMOUNT) ; 
            __context__.SourceCodeLine = 1481;
            SUBTRACTPLAYERBANK (  __context__ , (ushort)( BETAMOUNT )) ; 
            __context__.SourceCodeLine = 1482;
            COUNTREMAININGBETS (  __context__  ) ; 
            
            }
            
        object STRAIGHTBET_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                ushort MODIFIEDARRAY = 0;
                
                
                __context__.SourceCodeLine = 1492;
                MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
                __context__.SourceCodeLine = 1493;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1493;
                    BETACTION (  __context__ , "Straight", (ushort)( MODIFIEDARRAY ), (ushort)( 1 )) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 1494;
                    DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
                    }
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object STRAIGHTBET_OnRelease_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            ushort MODIFIEDARRAY = 0;
            
            
            __context__.SourceCodeLine = 1499;
            MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
            __context__.SourceCodeLine = 1500;
            BETACTION (  __context__ , "Straight", (ushort)( MODIFIEDARRAY ), (ushort)( 0 )) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object SPLITBET_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1506;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1507;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 2 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1507;
            BETACTION (  __context__ , "Split", (ushort)( MODIFIEDARRAY ), (ushort)( 1 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1508;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SPLITBET_OnRelease_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1513;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1514;
        BETACTION (  __context__ , "Split", (ushort)( MODIFIEDARRAY ), (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CORNERBET_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1519;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1520;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 4 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1520;
            BETACTION (  __context__ , "Corner", (ushort)( MODIFIEDARRAY ), (ushort)( 1 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1521;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CORNERBET_OnRelease_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1526;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1527;
        BETACTION (  __context__ , "Corner", (ushort)( MODIFIEDARRAY ), (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object STREETBET_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1532;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1533;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 3 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1533;
            BETACTION (  __context__ , "Street", (ushort)( MODIFIEDARRAY ), (ushort)( 1 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1534;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object STREETBET_OnRelease_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1539;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1540;
        BETACTION (  __context__ , "Street", (ushort)( MODIFIEDARRAY ), (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LINEBET_OnPush_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1546;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1547;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 6 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1547;
            BETACTION (  __context__ , "Line", (ushort)( MODIFIEDARRAY ), (ushort)( 1 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1548;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LINEBET_OnRelease_9 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1553;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1554;
        BETACTION (  __context__ , "Line", (ushort)( MODIFIEDARRAY ), (ushort)( 0 )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETLOW_OnPush_10 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1560;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1560;
            PLACEOUTSIDEBET (  __context__ , "Low", (ushort)( 40 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1561;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETLOW_OnRelease_11 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1565;
        REMOVEBET (  __context__ , "Outside", (ushort)( 40 )) ; 
        __context__.SourceCodeLine = 1566;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1567;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETHIGH_OnPush_12 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1571;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1571;
            PLACEOUTSIDEBET (  __context__ , "High", (ushort)( 41 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1572;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETHIGH_OnRelease_13 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1576;
        REMOVEBET (  __context__ , "Outside", (ushort)( 41 )) ; 
        __context__.SourceCodeLine = 1577;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1578;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETEVEN_OnPush_14 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1582;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1582;
            PLACEOUTSIDEBET (  __context__ , "Even", (ushort)( 42 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1583;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETEVEN_OnRelease_15 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1587;
        REMOVEBET (  __context__ , "Outside", (ushort)( 42 )) ; 
        __context__.SourceCodeLine = 1588;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1589;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETODD_OnPush_16 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1593;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1593;
            PLACEOUTSIDEBET (  __context__ , "Odd", (ushort)( 43 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1594;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETODD_OnRelease_17 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1598;
        REMOVEBET (  __context__ , "Outside", (ushort)( 43 )) ; 
        __context__.SourceCodeLine = 1599;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1600;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETBLACK_OnPush_18 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1604;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1604;
            PLACEOUTSIDEBET (  __context__ , "Black", (ushort)( 44 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1605;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETBLACK_OnRelease_19 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1609;
        REMOVEBET (  __context__ , "Outside", (ushort)( 44 )) ; 
        __context__.SourceCodeLine = 1610;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1611;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETRED_OnPush_20 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1615;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1615;
            PLACEOUTSIDEBET (  __context__ , "Red", (ushort)( 45 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1616;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETRED_OnRelease_21 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1620;
        REMOVEBET (  __context__ , "Outside", (ushort)( 45 )) ; 
        __context__.SourceCodeLine = 1621;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1622;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETCOLUMN_OnPush_22 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1627;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1628;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1629;
            
                {
                int __SPLS_TMPVAR__SWTCH_3__ = ((int)MODIFIEDARRAY);
                
                    { 
                    if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 1) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 1630;
                        PLACEOUTSIDEBET (  __context__ , "Column 1", (ushort)( 46 )) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 2) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 1631;
                        PLACEOUTSIDEBET (  __context__ , "Column 2", (ushort)( 47 )) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 3) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 1632;
                        PLACEOUTSIDEBET (  __context__ , "Column 3", (ushort)( 48 )) ; 
                        } 
                    
                    else 
                        { 
                        } 
                    
                    } 
                    
                }
                
            
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1636;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETCOLUMN_OnRelease_23 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1641;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1642;
        
            {
            int __SPLS_TMPVAR__SWTCH_4__ = ((int)MODIFIEDARRAY);
            
                { 
                if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_4__ == ( 1) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 1643;
                    REMOVEBET (  __context__ , "Outside", (ushort)( 46 )) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_4__ == ( 2) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 1644;
                    REMOVEBET (  __context__ , "Outside", (ushort)( 47 )) ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_4__ == ( 3) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 1645;
                    REMOVEBET (  __context__ , "Outside", (ushort)( 48 )) ; 
                    } 
                
                else 
                    { 
                    } 
                
                } 
                
            }
            
        
        __context__.SourceCodeLine = 1648;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1649;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETFIRSTTWELVE_OnPush_24 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1653;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1653;
            PLACEOUTSIDEBET (  __context__ , "First Twelve", (ushort)( 49 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1655;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETFIRSTTWELVE_OnRelease_25 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1659;
        REMOVEBET (  __context__ , "Outside", (ushort)( 49 )) ; 
        __context__.SourceCodeLine = 1660;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1661;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETSECONDTWELVE_OnPush_26 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1665;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1665;
            PLACEOUTSIDEBET (  __context__ , "Second Twelve", (ushort)( 50 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1667;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETSECONDTWELVE_OnRelease_27 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1671;
        REMOVEBET (  __context__ , "Outside", (ushort)( 50 )) ; 
        __context__.SourceCodeLine = 1672;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1673;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETTHIRDTWELVE_OnPush_28 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1677;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1677;
            PLACEOUTSIDEBET (  __context__ , "Third Twelve", (ushort)( 51 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1679;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETTHIRDTWELVE_OnRelease_29 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1683;
        REMOVEBET (  __context__ , "Outside", (ushort)( 51 )) ; 
        __context__.SourceCodeLine = 1684;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1685;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETZERO_OnPush_30 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1689;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1689;
            PLACEOUTSIDEBET (  __context__ , "Zero", (ushort)( 0 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1691;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETZERO_OnRelease_31 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1695;
        REMOVEBET (  __context__ , "Outside", (ushort)( 0 )) ; 
        __context__.SourceCodeLine = 1696;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1697;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETDBLZERO_OnPush_32 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1701;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 1 ))  ) ) 
            { 
            __context__.SourceCodeLine = 1701;
            PLACEOUTSIDEBET (  __context__ , "Dbl Zero", (ushort)( 37 )) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 1703;
            DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETDBLZERO_OnRelease_33 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1707;
        REMOVEBET (  __context__ , "Outside", (ushort)( 37 )) ; 
        __context__.SourceCodeLine = 1708;
        ADDPLAYERBANK (  __context__ , (ushort)( TEMPBETAMOUNT )) ; 
        __context__.SourceCodeLine = 1709;
        COUNTREMAININGBETS (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CLEARBETS_OnPush_34 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1717;
        ISRESETTING = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1718;
        TOTALBET = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1719;
        PLAYERTOTALBET  .Value = (ushort) ( TOTALBET ) ; 
        __context__.SourceCodeLine = 1720;
        BETCOUNT  .Value = (ushort) ( COUNTREMAININGBETS( __context__ ) ) ; 
        __context__.SourceCodeLine = 1721;
        RESETBETS (  __context__  ) ; 
        __context__.SourceCodeLine = 1722;
        Functions.Delay (  (int) ( 100 ) ) ; 
        __context__.SourceCodeLine = 1723;
        ISRESETTING = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RESETGAME_OnPush_35 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1728;
        ISRESETTING = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1729;
        PLAYERBANK = (ushort) ( 1000 ) ; 
        __context__.SourceCodeLine = 1730;
        PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
        __context__.SourceCodeLine = 1731;
        TOTALBET = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1732;
        PLAYERTOTALBET  .Value = (ushort) ( TOTALBET ) ; 
        __context__.SourceCodeLine = 1733;
        RESETBETS (  __context__  ) ; 
        __context__.SourceCodeLine = 1734;
        BETCOUNT  .Value = (ushort) ( COUNTREMAININGBETS( __context__ ) ) ; 
        __context__.SourceCodeLine = 1735;
        Functions.Pulse ( 200, RESETPUSHED ) ; 
        __context__.SourceCodeLine = 1736;
        Functions.Delay (  (int) ( 100 ) ) ; 
        __context__.SourceCodeLine = 1737;
        ISRESETTING = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object WINNINGNUMBER_OnChange_36 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort WINNUM = 0;
        
        ushort I = 0;
        
        ushort PAYOUT = 0;
        
        
        __context__.SourceCodeLine = 1744;
        PAYOUT = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1746;
        WINNUM = (ushort) ( WINNINGNUMBER  .UshortValue ) ; 
        __context__.SourceCodeLine = 1747;
        Trace( "winning number chosen: {0:d}", (short)WINNUM) ; 
        __context__.SourceCodeLine = 1749;
        ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
        ushort __FN_FOREND_VAL__1 = (ushort)50; 
        int __FN_FORSTEP_VAL__1 = (int)1; 
        for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
            { 
            __context__.SourceCodeLine = 1751;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE != "Open"))  ) ) 
                { 
                __context__.SourceCodeLine = 1751;
                Trace( "Player bets: {0}, {1:d}", PLAYERBETS [ I] . BETTYPE , (short)PLAYERBETS[ I ].NUMBERPLAYED) ; 
                } 
            
            __context__.SourceCodeLine = 1753;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Straight") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1754;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 35)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1757;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Split") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1758;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 17)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1761;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Corner") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1762;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 8)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1765;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Street") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1766;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 11)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1769;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Line") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1770;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 5)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1773;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Outside"))  ) ) 
                { 
                __context__.SourceCodeLine = 1775;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( WINNUM <= 18 ) ) && Functions.TestForTrue ( Functions.BoolToInt (WINNUM != 0) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 40) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1776;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1779;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( WINNUM >= 19 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( WINNUM <= 36 ) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 41) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1780;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1783;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMEVEN  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 42) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1784;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1787;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMODD  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 43) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1788;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1791;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMBLACK  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 44) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1792;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1795;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMRED  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 45) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1796;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1799;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMCOLUMN[ 1 ] .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 46) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1800;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1803;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMCOLUMN[ 2 ] .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 47) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1804;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1807;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMCOLUMN[ 3 ] .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 48) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1808;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1811;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMFIRSTTWELVE  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 49) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1812;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1815;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMSECONDTWELVE  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 50) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1816;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1819;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMTHIRDTWELVE  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 51) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1820;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1823;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMZERO  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1824;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 35)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1827;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMDBLZERO  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1828;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 35)) ) ; 
                    } 
                
                } 
            
            __context__.SourceCodeLine = 1832;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE != "Open"))  ) ) 
                { 
                __context__.SourceCodeLine = 1832;
                Trace( "Index {0:d} payout is:{1:d}", (short)I, (short)PAYOUT) ; 
                } 
            
            __context__.SourceCodeLine = 1749;
            } 
        
        __context__.SourceCodeLine = 1835;
        RETURNPREVIOUSWINNERS (  __context__ , (ushort)( WINNUM )) ; 
        __context__.SourceCodeLine = 1837;
        ISRESETTING = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1838;
        PLAYERWINNINGS  .Value = (ushort) ( PAYOUT ) ; 
        __context__.SourceCodeLine = 1839;
        PLAYERBANK = (ushort) ( (PLAYERBANK + PAYOUT) ) ; 
        __context__.SourceCodeLine = 1840;
        PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
        __context__.SourceCodeLine = 1841;
        TOTALBET = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1842;
        PLAYERTOTALBET  .Value = (ushort) ( TOTALBET ) ; 
        __context__.SourceCodeLine = 1843;
        RESETBETS (  __context__  ) ; 
        __context__.SourceCodeLine = 1844;
        BETCOUNT  .Value = (ushort) ( COUNTREMAININGBETS( __context__ ) ) ; 
        __context__.SourceCodeLine = 1845;
        Functions.Pulse ( 200, RESETPUSHED ) ; 
        __context__.SourceCodeLine = 1846;
        Functions.Delay (  (int) ( 200 ) ) ; 
        __context__.SourceCodeLine = 1847;
        ISRESETTING = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETAMOUNT__DOLLAR___OnChange_37 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1851;
        STRINGBETTOANALOG (  __context__  ) ; 
        
        
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
        
        __context__.SourceCodeLine = 1860;
        ISRESETTING = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1861;
        PLAYERBANK = (ushort) ( 1000 ) ; 
        __context__.SourceCodeLine = 1862;
        PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
        __context__.SourceCodeLine = 1863;
        BETSREMAINING = (ushort) ( 50 ) ; 
        __context__.SourceCodeLine = 1864;
        PREVWININDEX = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1865;
        RESETBETS (  __context__  ) ; 
        __context__.SourceCodeLine = 1866;
        BETCOUNT  .Value = (ushort) ( COUNTREMAININGBETS( __context__ ) ) ; 
        __context__.SourceCodeLine = 1867;
        Functions.Delay (  (int) ( 100 ) ) ; 
        __context__.SourceCodeLine = 1868;
        ISRESETTING = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    PLACEBETTYPE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 8, this );
    PLAYERBETS  = new STRUCTBET[ 51 ];
    for( uint i = 0; i < 51; i++ )
    {
        PLAYERBETS [i] = new STRUCTBET( this, true );
        PLAYERBETS [i].PopulateCustomAttributeList( false );
        
    }
    
    CLEARBETS = new Crestron.Logos.SplusObjects.DigitalInput( CLEARBETS__DigitalInput__, this );
    m_DigitalInputList.Add( CLEARBETS__DigitalInput__, CLEARBETS );
    
    RESETGAME = new Crestron.Logos.SplusObjects.DigitalInput( RESETGAME__DigitalInput__, this );
    m_DigitalInputList.Add( RESETGAME__DigitalInput__, RESETGAME );
    
    NUMFIRSTTWELVE = new Crestron.Logos.SplusObjects.DigitalInput( NUMFIRSTTWELVE__DigitalInput__, this );
    m_DigitalInputList.Add( NUMFIRSTTWELVE__DigitalInput__, NUMFIRSTTWELVE );
    
    NUMSECONDTWELVE = new Crestron.Logos.SplusObjects.DigitalInput( NUMSECONDTWELVE__DigitalInput__, this );
    m_DigitalInputList.Add( NUMSECONDTWELVE__DigitalInput__, NUMSECONDTWELVE );
    
    NUMTHIRDTWELVE = new Crestron.Logos.SplusObjects.DigitalInput( NUMTHIRDTWELVE__DigitalInput__, this );
    m_DigitalInputList.Add( NUMTHIRDTWELVE__DigitalInput__, NUMTHIRDTWELVE );
    
    NUMEVEN = new Crestron.Logos.SplusObjects.DigitalInput( NUMEVEN__DigitalInput__, this );
    m_DigitalInputList.Add( NUMEVEN__DigitalInput__, NUMEVEN );
    
    NUMODD = new Crestron.Logos.SplusObjects.DigitalInput( NUMODD__DigitalInput__, this );
    m_DigitalInputList.Add( NUMODD__DigitalInput__, NUMODD );
    
    NUMRED = new Crestron.Logos.SplusObjects.DigitalInput( NUMRED__DigitalInput__, this );
    m_DigitalInputList.Add( NUMRED__DigitalInput__, NUMRED );
    
    NUMBLACK = new Crestron.Logos.SplusObjects.DigitalInput( NUMBLACK__DigitalInput__, this );
    m_DigitalInputList.Add( NUMBLACK__DigitalInput__, NUMBLACK );
    
    NUMHIGH = new Crestron.Logos.SplusObjects.DigitalInput( NUMHIGH__DigitalInput__, this );
    m_DigitalInputList.Add( NUMHIGH__DigitalInput__, NUMHIGH );
    
    NUMLOW = new Crestron.Logos.SplusObjects.DigitalInput( NUMLOW__DigitalInput__, this );
    m_DigitalInputList.Add( NUMLOW__DigitalInput__, NUMLOW );
    
    NUMZERO = new Crestron.Logos.SplusObjects.DigitalInput( NUMZERO__DigitalInput__, this );
    m_DigitalInputList.Add( NUMZERO__DigitalInput__, NUMZERO );
    
    NUMDBLZERO = new Crestron.Logos.SplusObjects.DigitalInput( NUMDBLZERO__DigitalInput__, this );
    m_DigitalInputList.Add( NUMDBLZERO__DigitalInput__, NUMDBLZERO );
    
    BETLOW = new Crestron.Logos.SplusObjects.DigitalInput( BETLOW__DigitalInput__, this );
    m_DigitalInputList.Add( BETLOW__DigitalInput__, BETLOW );
    
    BETFIRSTTWELVE = new Crestron.Logos.SplusObjects.DigitalInput( BETFIRSTTWELVE__DigitalInput__, this );
    m_DigitalInputList.Add( BETFIRSTTWELVE__DigitalInput__, BETFIRSTTWELVE );
    
    BETEVEN = new Crestron.Logos.SplusObjects.DigitalInput( BETEVEN__DigitalInput__, this );
    m_DigitalInputList.Add( BETEVEN__DigitalInput__, BETEVEN );
    
    BETSECONDTWELVE = new Crestron.Logos.SplusObjects.DigitalInput( BETSECONDTWELVE__DigitalInput__, this );
    m_DigitalInputList.Add( BETSECONDTWELVE__DigitalInput__, BETSECONDTWELVE );
    
    BETRED = new Crestron.Logos.SplusObjects.DigitalInput( BETRED__DigitalInput__, this );
    m_DigitalInputList.Add( BETRED__DigitalInput__, BETRED );
    
    BETBLACK = new Crestron.Logos.SplusObjects.DigitalInput( BETBLACK__DigitalInput__, this );
    m_DigitalInputList.Add( BETBLACK__DigitalInput__, BETBLACK );
    
    BETTHIRDTWELVE = new Crestron.Logos.SplusObjects.DigitalInput( BETTHIRDTWELVE__DigitalInput__, this );
    m_DigitalInputList.Add( BETTHIRDTWELVE__DigitalInput__, BETTHIRDTWELVE );
    
    BETODD = new Crestron.Logos.SplusObjects.DigitalInput( BETODD__DigitalInput__, this );
    m_DigitalInputList.Add( BETODD__DigitalInput__, BETODD );
    
    BETHIGH = new Crestron.Logos.SplusObjects.DigitalInput( BETHIGH__DigitalInput__, this );
    m_DigitalInputList.Add( BETHIGH__DigitalInput__, BETHIGH );
    
    BETZERO = new Crestron.Logos.SplusObjects.DigitalInput( BETZERO__DigitalInput__, this );
    m_DigitalInputList.Add( BETZERO__DigitalInput__, BETZERO );
    
    BETDBLZERO = new Crestron.Logos.SplusObjects.DigitalInput( BETDBLZERO__DigitalInput__, this );
    m_DigitalInputList.Add( BETDBLZERO__DigitalInput__, BETDBLZERO );
    
    NUMCOLUMN = new InOutArray<DigitalInput>( 3, this );
    for( uint i = 0; i < 3; i++ )
    {
        NUMCOLUMN[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( NUMCOLUMN__DigitalInput__ + i, NUMCOLUMN__DigitalInput__, this );
        m_DigitalInputList.Add( NUMCOLUMN__DigitalInput__ + i, NUMCOLUMN[i+1] );
    }
    
    BETCOLUMN = new InOutArray<DigitalInput>( 3, this );
    for( uint i = 0; i < 3; i++ )
    {
        BETCOLUMN[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( BETCOLUMN__DigitalInput__ + i, BETCOLUMN__DigitalInput__, this );
        m_DigitalInputList.Add( BETCOLUMN__DigitalInput__ + i, BETCOLUMN[i+1] );
    }
    
    STRAIGHTBET = new InOutArray<DigitalInput>( 36, this );
    for( uint i = 0; i < 36; i++ )
    {
        STRAIGHTBET[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( STRAIGHTBET__DigitalInput__ + i, STRAIGHTBET__DigitalInput__, this );
        m_DigitalInputList.Add( STRAIGHTBET__DigitalInput__ + i, STRAIGHTBET[i+1] );
    }
    
    SPLITBET = new InOutArray<DigitalInput>( 57, this );
    for( uint i = 0; i < 57; i++ )
    {
        SPLITBET[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( SPLITBET__DigitalInput__ + i, SPLITBET__DigitalInput__, this );
        m_DigitalInputList.Add( SPLITBET__DigitalInput__ + i, SPLITBET[i+1] );
    }
    
    LINEBET = new InOutArray<DigitalInput>( 11, this );
    for( uint i = 0; i < 11; i++ )
    {
        LINEBET[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( LINEBET__DigitalInput__ + i, LINEBET__DigitalInput__, this );
        m_DigitalInputList.Add( LINEBET__DigitalInput__ + i, LINEBET[i+1] );
    }
    
    STREETBET = new InOutArray<DigitalInput>( 12, this );
    for( uint i = 0; i < 12; i++ )
    {
        STREETBET[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( STREETBET__DigitalInput__ + i, STREETBET__DigitalInput__, this );
        m_DigitalInputList.Add( STREETBET__DigitalInput__ + i, STREETBET[i+1] );
    }
    
    CORNERBET = new InOutArray<DigitalInput>( 22, this );
    for( uint i = 0; i < 22; i++ )
    {
        CORNERBET[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( CORNERBET__DigitalInput__ + i, CORNERBET__DigitalInput__, this );
        m_DigitalInputList.Add( CORNERBET__DigitalInput__ + i, CORNERBET[i+1] );
    }
    
    DISPLAYERRORMESSAGE = new Crestron.Logos.SplusObjects.DigitalOutput( DISPLAYERRORMESSAGE__DigitalOutput__, this );
    m_DigitalOutputList.Add( DISPLAYERRORMESSAGE__DigitalOutput__, DISPLAYERRORMESSAGE );
    
    RESETPUSHED = new Crestron.Logos.SplusObjects.DigitalOutput( RESETPUSHED__DigitalOutput__, this );
    m_DigitalOutputList.Add( RESETPUSHED__DigitalOutput__, RESETPUSHED );
    
    WINNINGNUMBER = new Crestron.Logos.SplusObjects.AnalogInput( WINNINGNUMBER__AnalogSerialInput__, this );
    m_AnalogInputList.Add( WINNINGNUMBER__AnalogSerialInput__, WINNINGNUMBER );
    
    PLAYERBANKOUT = new Crestron.Logos.SplusObjects.AnalogOutput( PLAYERBANKOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( PLAYERBANKOUT__AnalogSerialOutput__, PLAYERBANKOUT );
    
    PLAYERTOTALBET = new Crestron.Logos.SplusObjects.AnalogOutput( PLAYERTOTALBET__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( PLAYERTOTALBET__AnalogSerialOutput__, PLAYERTOTALBET );
    
    PLAYERWINNINGS = new Crestron.Logos.SplusObjects.AnalogOutput( PLAYERWINNINGS__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( PLAYERWINNINGS__AnalogSerialOutput__, PLAYERWINNINGS );
    
    BETCOUNT = new Crestron.Logos.SplusObjects.AnalogOutput( BETCOUNT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( BETCOUNT__AnalogSerialOutput__, BETCOUNT );
    
    PREVWIN = new InOutArray<AnalogOutput>( 5, this );
    for( uint i = 0; i < 5; i++ )
    {
        PREVWIN[i+1] = new Crestron.Logos.SplusObjects.AnalogOutput( PREVWIN__AnalogSerialOutput__ + i, this );
        m_AnalogOutputList.Add( PREVWIN__AnalogSerialOutput__ + i, PREVWIN[i+1] );
    }
    
    BETAMOUNT__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( BETAMOUNT__DOLLAR____AnalogSerialInput__, 5, this );
    m_StringInputList.Add( BETAMOUNT__DOLLAR____AnalogSerialInput__, BETAMOUNT__DOLLAR__ );
    
    ERRORMESSAGE = new Crestron.Logos.SplusObjects.StringOutput( ERRORMESSAGE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( ERRORMESSAGE__AnalogSerialOutput__, ERRORMESSAGE );
    
    
    for( uint i = 0; i < 36; i++ )
        STRAIGHTBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( STRAIGHTBET_OnPush_0, false ) );
        
    for( uint i = 0; i < 36; i++ )
        STRAIGHTBET[i+1].OnDigitalRelease.Add( new InputChangeHandlerWrapper( STRAIGHTBET_OnRelease_1, false ) );
        
    for( uint i = 0; i < 57; i++ )
        SPLITBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( SPLITBET_OnPush_2, false ) );
        
    for( uint i = 0; i < 57; i++ )
        SPLITBET[i+1].OnDigitalRelease.Add( new InputChangeHandlerWrapper( SPLITBET_OnRelease_3, false ) );
        
    for( uint i = 0; i < 22; i++ )
        CORNERBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( CORNERBET_OnPush_4, false ) );
        
    for( uint i = 0; i < 22; i++ )
        CORNERBET[i+1].OnDigitalRelease.Add( new InputChangeHandlerWrapper( CORNERBET_OnRelease_5, false ) );
        
    for( uint i = 0; i < 12; i++ )
        STREETBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( STREETBET_OnPush_6, false ) );
        
    for( uint i = 0; i < 12; i++ )
        STREETBET[i+1].OnDigitalRelease.Add( new InputChangeHandlerWrapper( STREETBET_OnRelease_7, false ) );
        
    for( uint i = 0; i < 11; i++ )
        LINEBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( LINEBET_OnPush_8, false ) );
        
    for( uint i = 0; i < 11; i++ )
        LINEBET[i+1].OnDigitalRelease.Add( new InputChangeHandlerWrapper( LINEBET_OnRelease_9, false ) );
        
    BETLOW.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETLOW_OnPush_10, false ) );
    BETLOW.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETLOW_OnRelease_11, false ) );
    BETHIGH.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETHIGH_OnPush_12, false ) );
    BETHIGH.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETHIGH_OnRelease_13, false ) );
    BETEVEN.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETEVEN_OnPush_14, false ) );
    BETEVEN.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETEVEN_OnRelease_15, false ) );
    BETODD.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETODD_OnPush_16, false ) );
    BETODD.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETODD_OnRelease_17, false ) );
    BETBLACK.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETBLACK_OnPush_18, false ) );
    BETBLACK.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETBLACK_OnRelease_19, false ) );
    BETRED.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETRED_OnPush_20, false ) );
    BETRED.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETRED_OnRelease_21, false ) );
    for( uint i = 0; i < 3; i++ )
        BETCOLUMN[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( BETCOLUMN_OnPush_22, false ) );
        
    for( uint i = 0; i < 3; i++ )
        BETCOLUMN[i+1].OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETCOLUMN_OnRelease_23, false ) );
        
    BETFIRSTTWELVE.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETFIRSTTWELVE_OnPush_24, false ) );
    BETFIRSTTWELVE.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETFIRSTTWELVE_OnRelease_25, false ) );
    BETSECONDTWELVE.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETSECONDTWELVE_OnPush_26, false ) );
    BETSECONDTWELVE.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETSECONDTWELVE_OnRelease_27, false ) );
    BETTHIRDTWELVE.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETTHIRDTWELVE_OnPush_28, false ) );
    BETTHIRDTWELVE.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETTHIRDTWELVE_OnRelease_29, false ) );
    BETZERO.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETZERO_OnPush_30, false ) );
    BETZERO.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETZERO_OnRelease_31, false ) );
    BETDBLZERO.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETDBLZERO_OnPush_32, false ) );
    BETDBLZERO.OnDigitalRelease.Add( new InputChangeHandlerWrapper( BETDBLZERO_OnRelease_33, false ) );
    CLEARBETS.OnDigitalPush.Add( new InputChangeHandlerWrapper( CLEARBETS_OnPush_34, false ) );
    RESETGAME.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESETGAME_OnPush_35, false ) );
    WINNINGNUMBER.OnAnalogChange.Add( new InputChangeHandlerWrapper( WINNINGNUMBER_OnChange_36, false ) );
    BETAMOUNT__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( BETAMOUNT__DOLLAR___OnChange_37, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_PLAYER_BETS ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint WINNINGNUMBER__AnalogSerialInput__ = 0;
const uint CLEARBETS__DigitalInput__ = 0;
const uint RESETGAME__DigitalInput__ = 1;
const uint NUMFIRSTTWELVE__DigitalInput__ = 2;
const uint NUMSECONDTWELVE__DigitalInput__ = 3;
const uint NUMTHIRDTWELVE__DigitalInput__ = 4;
const uint NUMEVEN__DigitalInput__ = 5;
const uint NUMODD__DigitalInput__ = 6;
const uint NUMRED__DigitalInput__ = 7;
const uint NUMBLACK__DigitalInput__ = 8;
const uint NUMHIGH__DigitalInput__ = 9;
const uint NUMLOW__DigitalInput__ = 10;
const uint NUMZERO__DigitalInput__ = 11;
const uint NUMDBLZERO__DigitalInput__ = 12;
const uint BETLOW__DigitalInput__ = 13;
const uint BETFIRSTTWELVE__DigitalInput__ = 14;
const uint BETEVEN__DigitalInput__ = 15;
const uint BETSECONDTWELVE__DigitalInput__ = 16;
const uint BETRED__DigitalInput__ = 17;
const uint BETBLACK__DigitalInput__ = 18;
const uint BETTHIRDTWELVE__DigitalInput__ = 19;
const uint BETODD__DigitalInput__ = 20;
const uint BETHIGH__DigitalInput__ = 21;
const uint BETZERO__DigitalInput__ = 22;
const uint BETDBLZERO__DigitalInput__ = 23;
const uint NUMCOLUMN__DigitalInput__ = 24;
const uint BETCOLUMN__DigitalInput__ = 27;
const uint STRAIGHTBET__DigitalInput__ = 30;
const uint SPLITBET__DigitalInput__ = 66;
const uint LINEBET__DigitalInput__ = 123;
const uint STREETBET__DigitalInput__ = 134;
const uint CORNERBET__DigitalInput__ = 146;
const uint BETAMOUNT__DOLLAR____AnalogSerialInput__ = 1;
const uint ERRORMESSAGE__AnalogSerialOutput__ = 0;
const uint DISPLAYERRORMESSAGE__DigitalOutput__ = 0;
const uint RESETPUSHED__DigitalOutput__ = 1;
const uint PLAYERBANKOUT__AnalogSerialOutput__ = 1;
const uint PLAYERTOTALBET__AnalogSerialOutput__ = 2;
const uint PLAYERWINNINGS__AnalogSerialOutput__ = 3;
const uint BETCOUNT__AnalogSerialOutput__ = 4;
const uint PREVWIN__AnalogSerialOutput__ = 5;

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
public class STRUCTBET : SplusStructureBase
{

    [SplusStructAttribute(0, false, false)]
    public CrestronString  BETTYPE;
    
    [SplusStructAttribute(1, false, false)]
    public ushort  NUMBERPLAYED = 0;
    
    [SplusStructAttribute(2, false, false)]
    public ushort  BETAMOUNT = 0;
    
    
    public STRUCTBET( SplusObject __caller__, bool bIsStructureVolatile ) : base ( __caller__, bIsStructureVolatile )
    {
        BETTYPE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 8, Owner );
        
        
    }
    
}

}
