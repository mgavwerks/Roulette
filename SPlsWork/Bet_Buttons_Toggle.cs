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

namespace UserModule_BET_BUTTONS_TOGGLE
{
    public class UserModuleClass_BET_BUTTONS_TOGGLE : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.DigitalInput RESETBUTTONS;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> BETBUTTONS;
        InOutArray<Crestron.Logos.SplusObjects.DigitalOutput> BETBUTTONSOUT;
        object BETBUTTONS_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                ushort INDEX = 0;
                
                
                __context__.SourceCodeLine = 21;
                INDEX = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
                __context__.SourceCodeLine = 22;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BETBUTTONSOUT[ INDEX ] .Value != 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 23;
                    BETBUTTONSOUT [ INDEX]  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 24;
                    BETBUTTONSOUT [ INDEX]  .Value = (ushort) ( 0 ) ; 
                    } 
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object RESETBUTTONS_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 28;
            Functions.SetArray ( BETBUTTONSOUT , (ushort)0) ; 
            
            
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
        
        __context__.SourceCodeLine = 33;
        Functions.SetArray ( BETBUTTONSOUT , (ushort)0) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    RESETBUTTONS = new Crestron.Logos.SplusObjects.DigitalInput( RESETBUTTONS__DigitalInput__, this );
    m_DigitalInputList.Add( RESETBUTTONS__DigitalInput__, RESETBUTTONS );
    
    BETBUTTONS = new InOutArray<DigitalInput>( 154, this );
    for( uint i = 0; i < 154; i++ )
    {
        BETBUTTONS[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( BETBUTTONS__DigitalInput__ + i, BETBUTTONS__DigitalInput__, this );
        m_DigitalInputList.Add( BETBUTTONS__DigitalInput__ + i, BETBUTTONS[i+1] );
    }
    
    BETBUTTONSOUT = new InOutArray<DigitalOutput>( 154, this );
    for( uint i = 0; i < 154; i++ )
    {
        BETBUTTONSOUT[i+1] = new Crestron.Logos.SplusObjects.DigitalOutput( BETBUTTONSOUT__DigitalOutput__ + i, this );
        m_DigitalOutputList.Add( BETBUTTONSOUT__DigitalOutput__ + i, BETBUTTONSOUT[i+1] );
    }
    
    
    for( uint i = 0; i < 154; i++ )
        BETBUTTONS[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( BETBUTTONS_OnPush_0, false ) );
        
    RESETBUTTONS.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESETBUTTONS_OnPush_1, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_BET_BUTTONS_TOGGLE ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint RESETBUTTONS__DigitalInput__ = 0;
const uint BETBUTTONS__DigitalInput__ = 1;
const uint BETBUTTONSOUT__DigitalOutput__ = 0;

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


}
