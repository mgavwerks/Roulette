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
        BETSTRUCTURE [] PLAYERBETS;
        ushort CURRENTBETARRAYINDEX = 0;
        ushort TOTALBET = 0;
        ushort PLAYERBANK = 0;
        ushort BETAMOUNT = 0;
        ushort BETSREMAINING = 0;
        ushort PREVWININDEX = 0;
        CrestronString PLACEBETTYPE;
        private ushort STRINGBETTOANALOG (  SplusExecutionContext __context__ ) 
            { 
            ushort BET = 0;
            
            
            __context__.SourceCodeLine = 96;
            BETAMOUNT = (ushort) ( Functions.Atoi( BETAMOUNT__DOLLAR__ ) ) ; 
            __context__.SourceCodeLine = 97;
            return (ushort)( BETAMOUNT) ; 
            
            }
            
        private void ADDTOTOTALBET (  SplusExecutionContext __context__, ushort BET ) 
            { 
            
            __context__.SourceCodeLine = 103;
            TOTALBET = (ushort) ( (TOTALBET + BET) ) ; 
            __context__.SourceCodeLine = 104;
            PLAYERTOTALBET  .Value = (ushort) ( TOTALBET ) ; 
            
            }
            
        private void RETURNPREVIOUSWINNERS (  SplusExecutionContext __context__, ushort WINNER ) 
            { 
            
            __context__.SourceCodeLine = 108;
            PREVWIN [ PREVWININDEX]  .Value = (ushort) ( WINNER ) ; 
            __context__.SourceCodeLine = 109;
            PREVWININDEX = (ushort) ( (PREVWININDEX + 1) ) ; 
            __context__.SourceCodeLine = 111;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PREVWININDEX == 6))  ) ) 
                { 
                __context__.SourceCodeLine = 112;
                PREVWININDEX = (ushort) ( 1 ) ; 
                } 
            
            
            }
            
        private void DISPLAYMESSAGE (  SplusExecutionContext __context__, ushort MSGNUM ) 
            { 
            
            __context__.SourceCodeLine = 117;
            
                {
                int __SPLS_TMPVAR__SWTCH_1__ = ((int)MSGNUM);
                
                    { 
                    if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 1) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 121;
                        ERRORMESSAGE  .UpdateValue ( "Not enough bank. Please adjust your bet or reset the game"  ) ; 
                        __context__.SourceCodeLine = 122;
                        Functions.Pulse ( 200, DISPLAYERRORMESSAGE ) ; 
                        } 
                    
                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 2) ) ) ) 
                        { 
                        __context__.SourceCodeLine = 126;
                        ERRORMESSAGE  .UpdateValue ( "You do not have any bets remaining."  ) ; 
                        __context__.SourceCodeLine = 127;
                        Functions.Pulse ( 200, DISPLAYERRORMESSAGE ) ; 
                        } 
                    
                    else 
                        { 
                        } 
                    
                    } 
                    
                }
                
            
            
            }
            
        private void UPDATEPLAYERBANK (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 139;
            PLAYERBANK = (ushort) ( (PLAYERBANK - STRINGBETTOANALOG( __context__ )) ) ; 
            __context__.SourceCodeLine = 140;
            PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
            
            }
            
        private void ADDSPLITBET (  SplusExecutionContext __context__, ushort NUMBER1 , ushort NUMBER2 ) 
            { 
            
            __context__.SourceCodeLine = 152;
            Trace( "Storing SPLIT bet Index:{0:d} with Numbers:{1:d} and {2:d}, with Bet:{3:d}", (short)CURRENTBETARRAYINDEX, (short)NUMBER1, (short)NUMBER2, (short)STRINGBETTOANALOG( __context__ )) ; 
            __context__.SourceCodeLine = 155;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Split"  ) ; 
            __context__.SourceCodeLine = 156;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUMBER1 ) ; 
            __context__.SourceCodeLine = 157;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 158;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 159;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 162;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 163;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Split"  ) ; 
            __context__.SourceCodeLine = 164;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUMBER2 ) ; 
            __context__.SourceCodeLine = 165;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 166;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 167;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 170;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 171;
            Trace( "Current Bet Index: {0:d}", (short)CURRENTBETARRAYINDEX) ; 
            __context__.SourceCodeLine = 175;
            UPDATEPLAYERBANK (  __context__  ) ; 
            
            }
            
        private void ADDSTRAIGHTBET (  SplusExecutionContext __context__, ushort NUMBERTOBET ) 
            { 
            
            __context__.SourceCodeLine = 182;
            Trace( "Storing STRAIGHT bet Index:{0:d} with Number:{1:d} and Bet:{2:d}", (short)CURRENTBETARRAYINDEX, (short)NUMBERTOBET, (short)STRINGBETTOANALOG( __context__ )) ; 
            __context__.SourceCodeLine = 185;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Straight"  ) ; 
            __context__.SourceCodeLine = 186;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUMBERTOBET ) ; 
            __context__.SourceCodeLine = 187;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 188;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 189;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 192;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 193;
            Trace( "New bet index: {0:d}", (short)CURRENTBETARRAYINDEX) ; 
            __context__.SourceCodeLine = 196;
            UPDATEPLAYERBANK (  __context__  ) ; 
            
            }
            
        private void ADDCORNERBET (  SplusExecutionContext __context__, ushort NUM1 , ushort NUM2 , ushort NUM3 , ushort NUM4 ) 
            { 
            
            __context__.SourceCodeLine = 202;
            Trace( "Storing CORNER bet Index:{0:d} with Numbers:{1:d}, {2:d}, {3:d}, and {4:d}, with Bet:{5:d}", (short)CURRENTBETARRAYINDEX, (short)NUM1, (short)NUM2, (short)NUM3, (short)NUM4, (short)STRINGBETTOANALOG( __context__ )) ; 
            __context__.SourceCodeLine = 205;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
            __context__.SourceCodeLine = 206;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM1 ) ; 
            __context__.SourceCodeLine = 207;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 208;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 209;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 210;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 211;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
            __context__.SourceCodeLine = 212;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM2 ) ; 
            __context__.SourceCodeLine = 213;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 214;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 215;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 216;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 217;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
            __context__.SourceCodeLine = 218;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM3 ) ; 
            __context__.SourceCodeLine = 219;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 220;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 221;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 222;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 223;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Corner"  ) ; 
            __context__.SourceCodeLine = 224;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM4 ) ; 
            __context__.SourceCodeLine = 225;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 226;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 227;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 230;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 231;
            Trace( "Current Bet Index: {0:d}", (short)CURRENTBETARRAYINDEX) ; 
            __context__.SourceCodeLine = 235;
            UPDATEPLAYERBANK (  __context__  ) ; 
            
            }
            
        private void ADDSTREETBET (  SplusExecutionContext __context__, ushort NUM1 , ushort NUM2 , ushort NUM3 ) 
            { 
            
            __context__.SourceCodeLine = 241;
            Trace( "Storing Street bet Index:{0:d} with Numbers:{1:d}, {2:d}, and {3:d}, with Bet:{4:d}", (short)CURRENTBETARRAYINDEX, (short)NUM1, (short)NUM2, (short)NUM3, (short)STRINGBETTOANALOG( __context__ )) ; 
            __context__.SourceCodeLine = 244;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Street"  ) ; 
            __context__.SourceCodeLine = 245;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM1 ) ; 
            __context__.SourceCodeLine = 246;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 247;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 248;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 249;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 250;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Street"  ) ; 
            __context__.SourceCodeLine = 251;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM2 ) ; 
            __context__.SourceCodeLine = 252;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 253;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 254;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 255;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 256;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Street"  ) ; 
            __context__.SourceCodeLine = 257;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM3 ) ; 
            __context__.SourceCodeLine = 258;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 259;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 260;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 263;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 264;
            Trace( "Current Bet Index: {0:d}", (short)CURRENTBETARRAYINDEX) ; 
            __context__.SourceCodeLine = 268;
            UPDATEPLAYERBANK (  __context__  ) ; 
            
            }
            
        private void ADDLINEBET (  SplusExecutionContext __context__, ushort NUM1 , ushort NUM2 , ushort NUM3 , ushort NUM4 , ushort NUM5 , ushort NUM6 ) 
            { 
            
            __context__.SourceCodeLine = 273;
            Trace( "Storing Line bet Index:{0:d} with Numbers:{1:d}, {2:d}, {3:d}, {4:d}, {5:d}, and {6:d}, with Bet:{7:d}", (short)CURRENTBETARRAYINDEX, (short)NUM1, (short)NUM2, (short)NUM3, (short)NUM4, (short)NUM5, (short)NUM6, (short)STRINGBETTOANALOG( __context__ )) ; 
            __context__.SourceCodeLine = 276;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Line"  ) ; 
            __context__.SourceCodeLine = 277;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM1 ) ; 
            __context__.SourceCodeLine = 278;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 279;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 280;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 281;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 282;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Line"  ) ; 
            __context__.SourceCodeLine = 283;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM2 ) ; 
            __context__.SourceCodeLine = 284;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 285;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 286;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 287;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 288;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Line"  ) ; 
            __context__.SourceCodeLine = 289;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM3 ) ; 
            __context__.SourceCodeLine = 290;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 291;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 292;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 293;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Line"  ) ; 
            __context__.SourceCodeLine = 294;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM4 ) ; 
            __context__.SourceCodeLine = 295;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 296;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 297;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 298;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 299;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Line"  ) ; 
            __context__.SourceCodeLine = 300;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM5 ) ; 
            __context__.SourceCodeLine = 301;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 302;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 303;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 304;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 305;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Line"  ) ; 
            __context__.SourceCodeLine = 306;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( NUM6 ) ; 
            __context__.SourceCodeLine = 307;
            PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
            __context__.SourceCodeLine = 308;
            BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
            __context__.SourceCodeLine = 309;
            BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
            __context__.SourceCodeLine = 312;
            CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
            __context__.SourceCodeLine = 313;
            Trace( "Current Bet Index: {0:d}", (short)CURRENTBETARRAYINDEX) ; 
            __context__.SourceCodeLine = 316;
            UPDATEPLAYERBANK (  __context__  ) ; 
            
            }
            
        private void PLACEBET (  SplusExecutionContext __context__, CrestronString PLACEBETTYPE , ushort INDEX ) 
            { 
            
            __context__.SourceCodeLine = 321;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( BETSREMAINING > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 322;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBANK != 0) ) && Functions.TestForTrue ( Functions.BoolToInt ( BETAMOUNT <= PLAYERBANK ) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 323;
                    
                        {
                        int __SPLS_TMPVAR__SWTCH_2__ = ((int)INDEX);
                        
                            { 
                            if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 1) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 326;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 327;
                                    ADDSPLITBET (  __context__ , (ushort)( 1 ), (ushort)( 2 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 329;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 330;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 332;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 333;
                                            ADDCORNERBET (  __context__ , (ushort)( 1 ), (ushort)( 2 ), (ushort)( 4 ), (ushort)( 5 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 335;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 336;
                                                ADDSTREETBET (  __context__ , (ushort)( 1 ), (ushort)( 2 ), (ushort)( 3 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 338;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 339;
                                                    ADDLINEBET (  __context__ , (ushort)( 1 ), (ushort)( 2 ), (ushort)( 3 ), (ushort)( 4 ), (ushort)( 5 ), (ushort)( 6 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 342;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 2) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 346;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 347;
                                    ADDSPLITBET (  __context__ , (ushort)( 2 ), (ushort)( 3 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 349;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 350;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 352;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 353;
                                            ADDCORNERBET (  __context__ , (ushort)( 2 ), (ushort)( 3 ), (ushort)( 5 ), (ushort)( 6 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 355;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 356;
                                                ADDSTREETBET (  __context__ , (ushort)( 4 ), (ushort)( 5 ), (ushort)( 6 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 358;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 359;
                                                    ADDLINEBET (  __context__ , (ushort)( 4 ), (ushort)( 5 ), (ushort)( 6 ), (ushort)( 7 ), (ushort)( 8 ), (ushort)( 9 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 362;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 3) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 366;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 367;
                                    ADDSPLITBET (  __context__ , (ushort)( 1 ), (ushort)( 4 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 369;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 370;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 372;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 373;
                                            ADDCORNERBET (  __context__ , (ushort)( 4 ), (ushort)( 5 ), (ushort)( 7 ), (ushort)( 8 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 375;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 376;
                                                ADDSTREETBET (  __context__ , (ushort)( 7 ), (ushort)( 8 ), (ushort)( 9 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 378;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 379;
                                                    ADDLINEBET (  __context__ , (ushort)( 7 ), (ushort)( 8 ), (ushort)( 9 ), (ushort)( 10 ), (ushort)( 11 ), (ushort)( 12 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 382;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 4) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 387;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 388;
                                    ADDSPLITBET (  __context__ , (ushort)( 2 ), (ushort)( 5 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 390;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 391;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 393;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 394;
                                            ADDCORNERBET (  __context__ , (ushort)( 5 ), (ushort)( 6 ), (ushort)( 8 ), (ushort)( 9 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 396;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 397;
                                                ADDSTREETBET (  __context__ , (ushort)( 10 ), (ushort)( 11 ), (ushort)( 12 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 399;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 400;
                                                    ADDLINEBET (  __context__ , (ushort)( 10 ), (ushort)( 11 ), (ushort)( 12 ), (ushort)( 13 ), (ushort)( 14 ), (ushort)( 15 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 403;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 5) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 408;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 409;
                                    ADDSPLITBET (  __context__ , (ushort)( 3 ), (ushort)( 6 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 411;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 412;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 414;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 415;
                                            ADDCORNERBET (  __context__ , (ushort)( 7 ), (ushort)( 8 ), (ushort)( 10 ), (ushort)( 11 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 417;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 418;
                                                ADDSTREETBET (  __context__ , (ushort)( 13 ), (ushort)( 14 ), (ushort)( 15 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 420;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 421;
                                                    ADDLINEBET (  __context__ , (ushort)( 13 ), (ushort)( 14 ), (ushort)( 15 ), (ushort)( 16 ), (ushort)( 17 ), (ushort)( 18 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 424;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 6) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 428;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 429;
                                    ADDSPLITBET (  __context__ , (ushort)( 4 ), (ushort)( 5 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 431;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 432;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 434;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 435;
                                            ADDCORNERBET (  __context__ , (ushort)( 8 ), (ushort)( 9 ), (ushort)( 11 ), (ushort)( 12 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 437;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 438;
                                                ADDSTREETBET (  __context__ , (ushort)( 16 ), (ushort)( 17 ), (ushort)( 18 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 440;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 441;
                                                    ADDLINEBET (  __context__ , (ushort)( 16 ), (ushort)( 17 ), (ushort)( 18 ), (ushort)( 19 ), (ushort)( 20 ), (ushort)( 21 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 444;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 7) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 448;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 449;
                                    ADDSPLITBET (  __context__ , (ushort)( 5 ), (ushort)( 6 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 451;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 452;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 454;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 455;
                                            ADDCORNERBET (  __context__ , (ushort)( 10 ), (ushort)( 11 ), (ushort)( 13 ), (ushort)( 14 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 457;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 458;
                                                ADDSTREETBET (  __context__ , (ushort)( 19 ), (ushort)( 20 ), (ushort)( 21 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 460;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 461;
                                                    ADDLINEBET (  __context__ , (ushort)( 19 ), (ushort)( 20 ), (ushort)( 21 ), (ushort)( 22 ), (ushort)( 23 ), (ushort)( 24 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 464;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 8) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 468;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 469;
                                    ADDSPLITBET (  __context__ , (ushort)( 4 ), (ushort)( 7 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 471;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 472;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 474;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 475;
                                            ADDCORNERBET (  __context__ , (ushort)( 11 ), (ushort)( 12 ), (ushort)( 14 ), (ushort)( 15 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 477;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 478;
                                                ADDSTREETBET (  __context__ , (ushort)( 22 ), (ushort)( 23 ), (ushort)( 24 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 480;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 481;
                                                    ADDLINEBET (  __context__ , (ushort)( 22 ), (ushort)( 23 ), (ushort)( 24 ), (ushort)( 25 ), (ushort)( 26 ), (ushort)( 27 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 484;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 9) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 490;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 491;
                                    ADDSPLITBET (  __context__ , (ushort)( 5 ), (ushort)( 8 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 493;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 494;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 496;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 497;
                                            ADDCORNERBET (  __context__ , (ushort)( 13 ), (ushort)( 14 ), (ushort)( 16 ), (ushort)( 17 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 499;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 500;
                                                ADDSTREETBET (  __context__ , (ushort)( 25 ), (ushort)( 26 ), (ushort)( 27 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 502;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 503;
                                                    ADDLINEBET (  __context__ , (ushort)( 25 ), (ushort)( 26 ), (ushort)( 27 ), (ushort)( 28 ), (ushort)( 29 ), (ushort)( 30 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 506;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 10) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 510;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 511;
                                    ADDSPLITBET (  __context__ , (ushort)( 6 ), (ushort)( 9 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 513;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 514;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 516;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 517;
                                            ADDCORNERBET (  __context__ , (ushort)( 14 ), (ushort)( 15 ), (ushort)( 17 ), (ushort)( 18 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 519;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 520;
                                                ADDSTREETBET (  __context__ , (ushort)( 28 ), (ushort)( 29 ), (ushort)( 30 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 522;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 523;
                                                    ADDLINEBET (  __context__ , (ushort)( 28 ), (ushort)( 29 ), (ushort)( 30 ), (ushort)( 31 ), (ushort)( 32 ), (ushort)( 33 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 526;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 11) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 530;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 531;
                                    ADDSPLITBET (  __context__ , (ushort)( 7 ), (ushort)( 8 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 533;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 534;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 536;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 537;
                                            ADDCORNERBET (  __context__ , (ushort)( 16 ), (ushort)( 17 ), (ushort)( 19 ), (ushort)( 20 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 539;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 540;
                                                ADDSTREETBET (  __context__ , (ushort)( 31 ), (ushort)( 32 ), (ushort)( 33 )) ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 542;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Line"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 543;
                                                    ADDLINEBET (  __context__ , (ushort)( 31 ), (ushort)( 32 ), (ushort)( 33 ), (ushort)( 34 ), (ushort)( 35 ), (ushort)( 36 )) ; 
                                                    } 
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 546;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 12) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 550;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 551;
                                    ADDSPLITBET (  __context__ , (ushort)( 8 ), (ushort)( 9 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 553;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 554;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 556;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 557;
                                            ADDCORNERBET (  __context__ , (ushort)( 17 ), (ushort)( 18 ), (ushort)( 20 ), (ushort)( 21 )) ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 559;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Street"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 560;
                                                ADDSTREETBET (  __context__ , (ushort)( 34 ), (ushort)( 35 ), (ushort)( 36 )) ; 
                                                } 
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 563;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 13) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 567;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 568;
                                    ADDSPLITBET (  __context__ , (ushort)( 7 ), (ushort)( 10 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 570;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 571;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 573;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 574;
                                            ADDCORNERBET (  __context__ , (ushort)( 19 ), (ushort)( 20 ), (ushort)( 22 ), (ushort)( 23 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 577;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 14) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 581;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 582;
                                    ADDSPLITBET (  __context__ , (ushort)( 8 ), (ushort)( 11 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 584;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 585;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 587;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 588;
                                            ADDCORNERBET (  __context__ , (ushort)( 20 ), (ushort)( 21 ), (ushort)( 23 ), (ushort)( 24 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 591;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 15) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 595;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 596;
                                    ADDSPLITBET (  __context__ , (ushort)( 9 ), (ushort)( 12 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 598;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 599;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 601;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 602;
                                            ADDCORNERBET (  __context__ , (ushort)( 22 ), (ushort)( 23 ), (ushort)( 25 ), (ushort)( 26 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 605;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 16) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 609;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 610;
                                    ADDSPLITBET (  __context__ , (ushort)( 10 ), (ushort)( 11 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 612;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 613;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 615;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 616;
                                            ADDCORNERBET (  __context__ , (ushort)( 23 ), (ushort)( 24 ), (ushort)( 26 ), (ushort)( 27 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 619;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 17) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 623;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 624;
                                    ADDSPLITBET (  __context__ , (ushort)( 11 ), (ushort)( 12 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 626;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 627;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 629;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 630;
                                            ADDCORNERBET (  __context__ , (ushort)( 25 ), (ushort)( 26 ), (ushort)( 28 ), (ushort)( 29 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 633;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 18) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 637;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 638;
                                    ADDSPLITBET (  __context__ , (ushort)( 10 ), (ushort)( 13 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 640;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 641;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 643;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 644;
                                            ADDCORNERBET (  __context__ , (ushort)( 26 ), (ushort)( 27 ), (ushort)( 29 ), (ushort)( 30 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 647;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 19) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 651;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 652;
                                    ADDSPLITBET (  __context__ , (ushort)( 11 ), (ushort)( 14 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 654;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 655;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 657;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 658;
                                            ADDCORNERBET (  __context__ , (ushort)( 28 ), (ushort)( 29 ), (ushort)( 31 ), (ushort)( 32 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 661;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 20) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 665;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 666;
                                    ADDSPLITBET (  __context__ , (ushort)( 12 ), (ushort)( 15 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 668;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 669;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 671;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 672;
                                            ADDCORNERBET (  __context__ , (ushort)( 29 ), (ushort)( 30 ), (ushort)( 32 ), (ushort)( 33 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 675;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 21) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 679;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 680;
                                    ADDSPLITBET (  __context__ , (ushort)( 13 ), (ushort)( 14 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 682;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 683;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 685;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 686;
                                            ADDCORNERBET (  __context__ , (ushort)( 31 ), (ushort)( 32 ), (ushort)( 34 ), (ushort)( 35 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 689;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 22) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 693;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 694;
                                    ADDSPLITBET (  __context__ , (ushort)( 14 ), (ushort)( 15 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 696;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 697;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 699;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Corner"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 700;
                                            ADDCORNERBET (  __context__ , (ushort)( 32 ), (ushort)( 33 ), (ushort)( 35 ), (ushort)( 36 )) ; 
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                __context__.SourceCodeLine = 703;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 23) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 707;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 708;
                                    ADDSPLITBET (  __context__ , (ushort)( 13 ), (ushort)( 16 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 710;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 711;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 714;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 24) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 718;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 719;
                                    ADDSPLITBET (  __context__ , (ushort)( 14 ), (ushort)( 17 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 721;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 722;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 725;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 25) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 729;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 730;
                                    ADDSPLITBET (  __context__ , (ushort)( 15 ), (ushort)( 18 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 732;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 733;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 736;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 26) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 740;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 741;
                                    ADDSPLITBET (  __context__ , (ushort)( 16 ), (ushort)( 17 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 743;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 744;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 747;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 27) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 751;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 752;
                                    ADDSPLITBET (  __context__ , (ushort)( 17 ), (ushort)( 18 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 754;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 755;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 758;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 28) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 762;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 763;
                                    ADDSPLITBET (  __context__ , (ushort)( 16 ), (ushort)( 19 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 765;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 766;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 769;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 29) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 773;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 774;
                                    ADDSPLITBET (  __context__ , (ushort)( 17 ), (ushort)( 20 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 776;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 777;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 780;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 30) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 784;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 785;
                                    ADDSPLITBET (  __context__ , (ushort)( 18 ), (ushort)( 21 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 787;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 788;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 791;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 31) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 795;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 796;
                                    ADDSPLITBET (  __context__ , (ushort)( 19 ), (ushort)( 20 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 798;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 799;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 802;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 32) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 806;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 807;
                                    ADDSPLITBET (  __context__ , (ushort)( 20 ), (ushort)( 21 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 809;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 810;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 813;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 33) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 817;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 818;
                                    ADDSPLITBET (  __context__ , (ushort)( 19 ), (ushort)( 22 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 820;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 821;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 824;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 34) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 828;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 829;
                                    ADDSPLITBET (  __context__ , (ushort)( 20 ), (ushort)( 23 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 831;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 832;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 835;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 35) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 839;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 840;
                                    ADDSPLITBET (  __context__ , (ushort)( 21 ), (ushort)( 24 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 842;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 843;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 846;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 36) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 850;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 851;
                                    ADDSPLITBET (  __context__ , (ushort)( 22 ), (ushort)( 23 )) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 853;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Straight"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 854;
                                        ADDSTRAIGHTBET (  __context__ , (ushort)( INDEX )) ; 
                                        } 
                                    
                                    }
                                
                                __context__.SourceCodeLine = 857;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 37) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 861;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 862;
                                    ADDSPLITBET (  __context__ , (ushort)( 23 ), (ushort)( 24 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 865;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 38) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 869;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 870;
                                    ADDSPLITBET (  __context__ , (ushort)( 22 ), (ushort)( 25 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 873;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 39) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 877;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 878;
                                    ADDSPLITBET (  __context__ , (ushort)( 23 ), (ushort)( 26 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 881;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 40) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 885;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 886;
                                    ADDSPLITBET (  __context__ , (ushort)( 24 ), (ushort)( 27 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 889;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 41) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 893;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 894;
                                    ADDSPLITBET (  __context__ , (ushort)( 25 ), (ushort)( 26 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 897;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 42) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 901;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 902;
                                    ADDSPLITBET (  __context__ , (ushort)( 26 ), (ushort)( 27 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 905;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 43) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 909;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 910;
                                    ADDSPLITBET (  __context__ , (ushort)( 25 ), (ushort)( 28 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 913;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 44) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 917;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 918;
                                    ADDSPLITBET (  __context__ , (ushort)( 26 ), (ushort)( 29 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 921;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 45) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 925;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 926;
                                    ADDSPLITBET (  __context__ , (ushort)( 27 ), (ushort)( 30 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 929;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 46) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 933;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 934;
                                    ADDSPLITBET (  __context__ , (ushort)( 28 ), (ushort)( 29 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 937;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 47) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 941;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 942;
                                    ADDSPLITBET (  __context__ , (ushort)( 29 ), (ushort)( 30 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 945;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 48) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 949;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 950;
                                    ADDSPLITBET (  __context__ , (ushort)( 28 ), (ushort)( 31 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 953;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 49) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 957;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 958;
                                    ADDSPLITBET (  __context__ , (ushort)( 29 ), (ushort)( 32 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 961;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 50) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 965;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 966;
                                    ADDSPLITBET (  __context__ , (ushort)( 30 ), (ushort)( 33 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 969;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 51) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 973;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 974;
                                    ADDSPLITBET (  __context__ , (ushort)( 31 ), (ushort)( 32 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 977;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 52) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 981;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 983;
                                    ADDSPLITBET (  __context__ , (ushort)( 32 ), (ushort)( 33 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 986;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 53) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 990;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 991;
                                    ADDSPLITBET (  __context__ , (ushort)( 31 ), (ushort)( 34 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 994;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 54) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 998;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 999;
                                    ADDSPLITBET (  __context__ , (ushort)( 32 ), (ushort)( 35 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 1002;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 55) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1006;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1007;
                                    ADDSPLITBET (  __context__ , (ushort)( 33 ), (ushort)( 36 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 1010;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 56) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1014;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1015;
                                    ADDSPLITBET (  __context__ , (ushort)( 34 ), (ushort)( 35 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 1018;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 57) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 1022;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLACEBETTYPE == "Split"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 1023;
                                    ADDSPLITBET (  __context__ , (ushort)( 35 ), (ushort)( 36 )) ; 
                                    } 
                                
                                __context__.SourceCodeLine = 1026;
                                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                                } 
                            
                            else 
                                { 
                                } 
                            
                            } 
                            
                        }
                        
                    
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 1031;
                    DISPLAYMESSAGE (  __context__ , (ushort)( 1 )) ; 
                    } 
                
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 1033;
                DISPLAYMESSAGE (  __context__ , (ushort)( 2 )) ; 
                } 
            
            
            }
            
        private void PLACEOUTSIDEBET (  SplusExecutionContext __context__, ushort BETNUMBER , CrestronString POSITION ) 
            { 
            
            __context__.SourceCodeLine = 1039;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBANK != 0) ) && Functions.TestForTrue ( Functions.BoolToInt ( BETAMOUNT <= PLAYERBANK ) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1040;
                PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Outside"  ) ; 
                __context__.SourceCodeLine = 1041;
                PLAYERBETS [ CURRENTBETARRAYINDEX] . NUMBERPLAYED = (ushort) ( BETNUMBER ) ; 
                __context__.SourceCodeLine = 1042;
                PLAYERBETS [ CURRENTBETARRAYINDEX] . BETAMOUNT = (ushort) ( STRINGBETTOANALOG( __context__ ) ) ; 
                __context__.SourceCodeLine = 1043;
                Trace( "Placing Outside Bet: {0} on Index: {1:d} with value: {2:d}", POSITION , (short)CURRENTBETARRAYINDEX, (short)STRINGBETTOANALOG( __context__ )) ; 
                __context__.SourceCodeLine = 1044;
                BETSREMAINING = (ushort) ( (BETSREMAINING - 1) ) ; 
                __context__.SourceCodeLine = 1045;
                BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
                __context__.SourceCodeLine = 1046;
                ADDTOTOTALBET (  __context__ , (ushort)( STRINGBETTOANALOG( __context__ ) )) ; 
                __context__.SourceCodeLine = 1049;
                CURRENTBETARRAYINDEX = (ushort) ( (CURRENTBETARRAYINDEX + 1) ) ; 
                __context__.SourceCodeLine = 1050;
                Trace( "Current Bet Index: {0:d}", (short)CURRENTBETARRAYINDEX) ; 
                __context__.SourceCodeLine = 1053;
                UPDATEPLAYERBANK (  __context__  ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 1055;
                DISPLAYMESSAGE (  __context__ , (ushort)( 1 )) ; 
                } 
            
            
            }
            
        object STRAIGHTBET_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                ushort MODIFIEDARRAY = 0;
                
                
                __context__.SourceCodeLine = 1065;
                MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
                __context__.SourceCodeLine = 1067;
                PLACEBET (  __context__ , "Straight", (ushort)( MODIFIEDARRAY )) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object SPLITBET_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            ushort MODIFIEDARRAY = 0;
            
            
            __context__.SourceCodeLine = 1072;
            MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
            __context__.SourceCodeLine = 1074;
            PLACEBET (  __context__ , "Split", (ushort)( MODIFIEDARRAY )) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object CORNERBET_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1079;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1081;
        PLACEBET (  __context__ , "Corner", (ushort)( MODIFIEDARRAY )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object STREETBET_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1086;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1088;
        PLACEBET (  __context__ , "Street", (ushort)( MODIFIEDARRAY )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LINEBET_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1093;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1095;
        PLACEBET (  __context__ , "Line", (ushort)( MODIFIEDARRAY )) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETLOW_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1101;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 40 ), "Low") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETHIGH_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1105;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 41 ), "High") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETEVEN_OnPush_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1109;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 42 ), "Even") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETODD_OnPush_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1113;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 43 ), "Odd") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETBLACK_OnPush_9 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1117;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 44 ), "Black") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETRED_OnPush_10 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1120;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 45 ), "Red") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETCOLUMN_OnPush_11 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort MODIFIEDARRAY = 0;
        
        
        __context__.SourceCodeLine = 1125;
        MODIFIEDARRAY = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 1127;
        PLAYERBETS [ CURRENTBETARRAYINDEX] . BETTYPE  .UpdateValue ( "Outside"  ) ; 
        __context__.SourceCodeLine = 1129;
        
            {
            int __SPLS_TMPVAR__SWTCH_3__ = ((int)MODIFIEDARRAY);
            
                { 
                if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 1) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 1130;
                    PLACEOUTSIDEBET (  __context__ , (ushort)( 46 ), "Column 1") ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 2) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 1131;
                    PLACEOUTSIDEBET (  __context__ , (ushort)( 47 ), "Column 2") ; 
                    } 
                
                else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 3) ) ) ) 
                    { 
                    __context__.SourceCodeLine = 1132;
                    PLACEOUTSIDEBET (  __context__ , (ushort)( 48 ), "Column 3") ; 
                    } 
                
                else 
                    { 
                    } 
                
                } 
                
            }
            
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETFIRSTTWELVE_OnPush_12 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1139;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 49 ), "First Twelve") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETSECONDTWELVE_OnPush_13 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1143;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 50 ), "Second Twelve") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETTHIRDTWELVE_OnPush_14 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1147;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 51 ), "Third Twelve") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETZERO_OnPush_15 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1151;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 0 ), "Zero") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETDBLZERO_OnPush_16 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1155;
        PLACEOUTSIDEBET (  __context__ , (ushort)( 37 ), "Dbl Zero") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CLEARBETS_OnPush_17 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1163;
        CURRENTBETARRAYINDEX = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1164;
        PLAYERTOTALBET  .Value = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1165;
        TOTALBET = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1166;
        BETSREMAINING = (ushort) ( 50 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RESETGAME_OnPush_18 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1172;
        CURRENTBETARRAYINDEX = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1173;
        PLAYERBANK = (ushort) ( 1000 ) ; 
        __context__.SourceCodeLine = 1174;
        PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
        __context__.SourceCodeLine = 1175;
        TOTALBET = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 1176;
        BETSREMAINING = (ushort) ( 50 ) ; 
        __context__.SourceCodeLine = 1177;
        BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
        __context__.SourceCodeLine = 1178;
        Functions.Pulse ( 200, RESETPUSHED ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object WINNINGNUMBER_OnChange_19 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort WINNUM = 0;
        
        ushort I = 0;
        
        ushort PAYOUT = 0;
        
        
        __context__.SourceCodeLine = 1187;
        WINNUM = (ushort) ( WINNINGNUMBER  .UshortValue ) ; 
        __context__.SourceCodeLine = 1188;
        Trace( "winning number chosen: {0:d}", (short)WINNUM) ; 
        __context__.SourceCodeLine = 1190;
        ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
        ushort __FN_FOREND_VAL__1 = (ushort)(CURRENTBETARRAYINDEX - 1); 
        int __FN_FORSTEP_VAL__1 = (int)1; 
        for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
            { 
            __context__.SourceCodeLine = 1192;
            Trace( "Player bets: {0}, {1:d}", PLAYERBETS [ I] . BETTYPE , (short)PLAYERBETS[ I ].NUMBERPLAYED) ; 
            __context__.SourceCodeLine = 1194;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Straight") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1195;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 35)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1198;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Split") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1199;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 17)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1202;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Corner") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1203;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 8)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1206;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Street") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1207;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 11)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1210;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Line") ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 1211;
                PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 5)) ) ; 
                } 
            
            __context__.SourceCodeLine = 1214;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (PLAYERBETS[ I ].BETTYPE == "Outside"))  ) ) 
                { 
                __context__.SourceCodeLine = 1216;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( WINNUM <= 18 ) ) && Functions.TestForTrue ( Functions.BoolToInt (WINNUM != 0) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 40) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1217;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1220;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( WINNUM >= 19 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( WINNUM <= 36 ) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 41) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1221;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1224;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMEVEN  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 42) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1225;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1228;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMODD  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 43) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1229;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1232;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMBLACK  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 44) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1233;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1236;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMRED  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 45) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1237;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1240;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMCOLUMN[ 1 ] .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 46) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1241;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1244;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMCOLUMN[ 2 ] .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 47) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1245;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1248;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMCOLUMN[ 3 ] .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 48) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1249;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1252;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMFIRSTTWELVE  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 49) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1253;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1256;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMSECONDTWELVE  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 50) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1257;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1260;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMTHIRDTWELVE  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == 51) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1261;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 2)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1264;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMZERO  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1265;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 35)) ) ; 
                    } 
                
                __context__.SourceCodeLine = 1268;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NUMDBLZERO  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (PLAYERBETS[ I ].NUMBERPLAYED == WINNUM) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 1269;
                    PAYOUT = (ushort) ( (PAYOUT + (PLAYERBETS[ I ].BETAMOUNT * 35)) ) ; 
                    } 
                
                } 
            
            __context__.SourceCodeLine = 1273;
            Trace( "Index {0:d} payout is:{1:d}", (short)I, (short)PAYOUT) ; 
            __context__.SourceCodeLine = 1190;
            } 
        
        __context__.SourceCodeLine = 1276;
        RETURNPREVIOUSWINNERS (  __context__ , (ushort)( WINNUM )) ; 
        __context__.SourceCodeLine = 1278;
        PLAYERWINNINGS  .Value = (ushort) ( PAYOUT ) ; 
        __context__.SourceCodeLine = 1279;
        PLAYERBANK = (ushort) ( (PLAYERBANK + PAYOUT) ) ; 
        __context__.SourceCodeLine = 1280;
        PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
        __context__.SourceCodeLine = 1281;
        BETSREMAINING = (ushort) ( 50 ) ; 
        __context__.SourceCodeLine = 1282;
        BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object BETAMOUNT__DOLLAR___OnChange_20 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 1286;
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
        
        __context__.SourceCodeLine = 1302;
        CURRENTBETARRAYINDEX = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 1303;
        PLAYERBANK = (ushort) ( 1000 ) ; 
        __context__.SourceCodeLine = 1304;
        PLAYERBANKOUT  .Value = (ushort) ( PLAYERBANK ) ; 
        __context__.SourceCodeLine = 1305;
        BETSREMAINING = (ushort) ( 50 ) ; 
        __context__.SourceCodeLine = 1306;
        BETCOUNT  .Value = (ushort) ( BETSREMAINING ) ; 
        __context__.SourceCodeLine = 1307;
        PREVWININDEX = (ushort) ( 1 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    PLACEBETTYPE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 8, this );
    PLAYERBETS  = new BETSTRUCTURE[ 51 ];
    for( uint i = 0; i < 51; i++ )
    {
        PLAYERBETS [i] = new BETSTRUCTURE( this, true );
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
        
    for( uint i = 0; i < 57; i++ )
        SPLITBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( SPLITBET_OnPush_1, false ) );
        
    for( uint i = 0; i < 22; i++ )
        CORNERBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( CORNERBET_OnPush_2, false ) );
        
    for( uint i = 0; i < 12; i++ )
        STREETBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( STREETBET_OnPush_3, false ) );
        
    for( uint i = 0; i < 11; i++ )
        LINEBET[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( LINEBET_OnPush_4, false ) );
        
    BETLOW.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETLOW_OnPush_5, false ) );
    BETHIGH.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETHIGH_OnPush_6, false ) );
    BETEVEN.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETEVEN_OnPush_7, false ) );
    BETODD.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETODD_OnPush_8, false ) );
    BETBLACK.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETBLACK_OnPush_9, false ) );
    BETRED.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETRED_OnPush_10, false ) );
    for( uint i = 0; i < 3; i++ )
        BETCOLUMN[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( BETCOLUMN_OnPush_11, false ) );
        
    BETFIRSTTWELVE.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETFIRSTTWELVE_OnPush_12, false ) );
    BETSECONDTWELVE.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETSECONDTWELVE_OnPush_13, false ) );
    BETTHIRDTWELVE.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETTHIRDTWELVE_OnPush_14, false ) );
    BETZERO.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETZERO_OnPush_15, false ) );
    BETDBLZERO.OnDigitalPush.Add( new InputChangeHandlerWrapper( BETDBLZERO_OnPush_16, false ) );
    CLEARBETS.OnDigitalPush.Add( new InputChangeHandlerWrapper( CLEARBETS_OnPush_17, false ) );
    RESETGAME.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESETGAME_OnPush_18, false ) );
    WINNINGNUMBER.OnAnalogChange.Add( new InputChangeHandlerWrapper( WINNINGNUMBER_OnChange_19, false ) );
    BETAMOUNT__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( BETAMOUNT__DOLLAR___OnChange_20, false ) );
    
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
public class BETSTRUCTURE : SplusStructureBase
{

    [SplusStructAttribute(0, false, false)]
    public CrestronString  BETTYPE;
    
    [SplusStructAttribute(1, false, false)]
    public ushort  NUMBERPLAYED = 0;
    
    [SplusStructAttribute(2, false, false)]
    public ushort  BETAMOUNT = 0;
    
    
    public BETSTRUCTURE( SplusObject __caller__, bool bIsStructureVolatile ) : base ( __caller__, bIsStructureVolatile )
    {
        BETTYPE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 8, Owner );
        
        
    }
    
}

}
