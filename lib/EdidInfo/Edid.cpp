#include "stdafx.h"
#include "Edid.h"

BOOL GetMonitorSizeFromEDID(const HKEY hEDIDRegKey, short& WidthMm, short& HeightMm)
{
    DWORD dwType, AcutalValueNameLength = NAME_SIZE;
    TCHAR valueName[NAME_SIZE];
 
    BYTE EDIDdata[1024];
    DWORD edidsize = sizeof(EDIDdata);
 
    for (LONG i = 0, retValue = ERROR_SUCCESS; retValue != ERROR_NO_MORE_ITEMS; ++i)
    {
        retValue = RegEnumValue(hEDIDRegKey, i, &valueName[0],
            &AcutalValueNameLength, NULL, &dwType, EDIDdata, &edidsize); 
 
        if (retValue != ERROR_SUCCESS || 0 != _tcscmp(valueName, _T("EDID")))
            continue;
 
        WidthMm = ((EDIDdata[68] & 0xF0) << 4) + EDIDdata[66];
        HeightMm = ((EDIDdata[68] & 0x0F) << 8) + EDIDdata[67];
 
        return true;
    }
 
    return false; 
}
 
BOOL GetSizeForDevID(LPCTSTR TargetDevID, short& WidthMm, short& HeightMm)
{
    HDEVINFO devInfo = SetupDiGetClassDevsEx(
        &GUID_CLASS_MONITOR, 
        NULL, 
        NULL, 
        DIGCF_PRESENT | DIGCF_PROFILE, 
        NULL, 
        NULL, 
        NULL);
 
    if (NULL == devInfo)
        return false;
 
    bool bRes = false;
 
    for (ULONG i = 0; ERROR_NO_MORE_ITEMS != GetLastError(); ++i)
    {
        SP_DEVINFO_DATA devInfoData;
        memset(&devInfoData, 0, sizeof(devInfoData));
        devInfoData.cbSize = sizeof(devInfoData);
 
        if (SetupDiEnumDeviceInfo(devInfo, i, &devInfoData))
        {
            TCHAR Instance[MAX_DEVICE_ID_LEN];
            SetupDiGetDeviceInstanceId(devInfo, &devInfoData, Instance, MAX_PATH, NULL);
 
            CString sInstance(Instance);
            if (-1 == sInstance.Find(TargetDevID))
                continue;
 
            HKEY hEDIDRegKey = SetupDiOpenDevRegKey(devInfo, &devInfoData,
                DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_READ);
 
            if (!hEDIDRegKey || (hEDIDRegKey == INVALID_HANDLE_VALUE))
                continue;
 
             BOOL bMSize = GetMonitorSizeFromEDID(hEDIDRegKey, WidthMm, HeightMm);
				
			 bRes = bMSize == TRUE ? true : false;
 
            RegCloseKey(hEDIDRegKey);
        }
    }
    SetupDiDestroyDeviceInfoList(devInfo);
    return bRes;
}

BOOL DisplayDeviceFromHMonitor(MONITORINFOEX mi, DISPLAY_DEVICE& ddMonOut)
{
    DISPLAY_DEVICE dd;
    dd.cb = sizeof(dd);
    DWORD devIdx = 0; 
 
    CString DeviceID;
    bool bFoundDevice = false;
    while (EnumDisplayDevices(0, devIdx, &dd, 0))
    {
        devIdx++;
        if (0 != _tcscmp(dd.DeviceName, mi.szDevice))
            continue;
 
        DISPLAY_DEVICE ddMon;
        ZeroMemory(&ddMon, sizeof(ddMon));
        ddMon.cb = sizeof(ddMon);
        DWORD MonIdx = 0;
 
        while (EnumDisplayDevices(dd.DeviceName, MonIdx, &ddMon, 0))
        {
            MonIdx++;
 
            ddMonOut = ddMon;
            return TRUE;
 
            ZeroMemory(&ddMon, sizeof(ddMon));
            ddMon.cb = sizeof(ddMon);
        }
 
        ZeroMemory(&dd, sizeof(dd));
        dd.cb = sizeof(dd);
    }
 
    return FALSE;
}
	