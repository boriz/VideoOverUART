// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "SerialPort.h"


CSerialPort _serial_port;


BOOL APIENTRY DllMain( HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}


extern "C" 
{
    __declspec(dllexport) int OpenPort(int port_number)
    {
        try
        {
            _serial_port.Open(port_number, 921600, CSerialPort::Parity::NoParity, 8, CSerialPort::StopBits::OneStopBit, CSerialPort::FlowControl::NoFlowControl);
            _serial_port.SetDTR();
            _serial_port.Setup(10000, 10000);

            return 0;
        }
        catch (CSerialException& e)
        {
            return -1;
        }
    }


    __declspec(dllexport) int ClosePort()
    {
        try
        {
            _serial_port.Flush();
            _serial_port.Close();
            return 0;
        }
        catch (const std::exception&)
        {
            return -1;
        }
    }


    __declspec(dllexport) int SendArray(BYTE byte_array[], int size_bytes)
    {
        try
        {
            //HANDLE hPort = cs.Detach();
            //cs.Attach(hPort);

            //DCB dcb;
            //cs.GetState(dcb);

            //DWORD dwErrors = 0;
            //cs.ClearError(dwErrors);

            //COMSTAT stat{};
            //cs.GetStatus(stat);

            //COMMTIMEOUTS timeouts{};
            //cs.GetTimeouts(timeouts);
            //timeouts.WriteTotalTimeoutConstant = 10;
            //cs.SetTimeouts(timeouts);

            //cs.Setup(10000, 10000);

            //cs.ClearReadBuffer();            

            //for (int i = 0; i < size_bytes; i++)
            //{
            //    char c = byte_array[i];
            //    cs.TransmitChar(c);                
            //}

            _serial_port.ClearReadBuffer();
            _serial_port.Write(byte_array, size_bytes);
            return 0;
        }
        catch (CSerialException& e)
        {
            return -1;
        }
    }

}


