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

namespace UserModule_RANDOM_NUMBER_GENERATOR
{
    public class UserModuleClass_RANDOM_NUMBER_GENERATOR : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput GENERATE_NUMBER;
        UShortParameter LOWERLIMIT;
        UShortParameter UPPERLIMIT;
        Crestron.Logos.SplusObjects.AnalogOutput NUMBER_OUT;
        object GENERATE_NUMBER_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                ushort RANNUMBER = 0;
                
                
                __context__.SourceCodeLine = 41;
                RANNUMBER = (ushort) ( Functions.Random( (ushort)( LOWERLIMIT  .Value ) , (ushort)( UPPERLIMIT  .Value ) ) ) ; 
                __context__.SourceCodeLine = 42;
                NUMBER_OUT  .Value = (ushort) ( RANNUMBER ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    
    public override void LogosSplusInitialize()
    {
        SocketInfo __socketinfo__ = new SocketInfo( 1, this );
        InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
        _SplusNVRAM = new SplusNVRAM( this );
        
        GENERATE_NUMBER = new Crestron.Logos.SplusObjects.DigitalInput( GENERATE_NUMBER__DigitalInput__, this );
        m_DigitalInputList.Add( GENERATE_NUMBER__DigitalInput__, GENERATE_NUMBER );
        
        NUMBER_OUT = new Crestron.Logos.SplusObjects.AnalogOutput( NUMBER_OUT__AnalogSerialOutput__, this );
        m_AnalogOutputList.Add( NUMBER_OUT__AnalogSerialOutput__, NUMBER_OUT );
        
        LOWERLIMIT = new UShortParameter( LOWERLIMIT__Parameter__, this );
        m_ParameterList.Add( LOWERLIMIT__Parameter__, LOWERLIMIT );
        
        UPPERLIMIT = new UShortParameter( UPPERLIMIT__Parameter__, this );
        m_ParameterList.Add( UPPERLIMIT__Parameter__, UPPERLIMIT );
        
        
        GENERATE_NUMBER.OnDigitalPush.Add( new InputChangeHandlerWrapper( GENERATE_NUMBER_OnPush_0, false ) );
        
        _SplusNVRAM.PopulateCustomAttributeList( true );
        
        NVRAM = _SplusNVRAM;
        
    }
    
    public override void LogosSimplSharpInitialize()
    {
        
        
    }
    
    public UserModuleClass_RANDOM_NUMBER_GENERATOR ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}
    
    
    
    
    const uint GENERATE_NUMBER__DigitalInput__ = 0;
    const uint LOWERLIMIT__Parameter__ = 10;
    const uint UPPERLIMIT__Parameter__ = 11;
    const uint NUMBER_OUT__AnalogSerialOutput__ = 0;
    
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
