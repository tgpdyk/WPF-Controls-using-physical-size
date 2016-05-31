#include <atlstr.h>
#include <math.h>
#include <SetupApi.h>
#include <cfgmgr32.h>   
#pragma comment(lib, "setupapi.lib")

#define EXTERN_DLL_EXPORT extern "C" __declspec(dllexport)
 
#define NAME_SIZE 128
 
const GUID GUID_CLASS_MONITOR = { 0x4d36e96e, 0xe325, 0x11ce, 0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18 };

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

using namespace std;
EXTERN_DLL_EXPORT BOOL GetMonitorSizeFromEDID(const HKEY hEDIDRegKey, short& WidthMm, short& HeightMm);
EXTERN_DLL_EXPORT BOOL GetSizeForDevID(LPCTSTR TargetDevID, short& WidthMm, short& HeightMm);
EXTERN_DLL_EXPORT BOOL DisplayDeviceFromHMonitor(MONITORINFOEX mi, DISPLAY_DEVICE& ddMonOut);

