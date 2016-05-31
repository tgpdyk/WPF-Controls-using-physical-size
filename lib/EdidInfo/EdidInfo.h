
#define DllExport   __declspec( dllexport )

DllExport int getnumber();
DllExport BOOL GetMonitorSizeFromEDID(const HKEY hEDIDRegKey, short& WidthMm, short& HeightMm);
DllExport BOOL GetSizeForDevID(const CString& TargetDevID, short& WidthMm, short& HeightMm);