#include-once

; #INDEX# =======================================================================================================================
; Title .........: WinAPIDlg Constants UDF Library for AutoIt3
; AutoIt Version : 3.8 / 3.3.8.0
; Language ......: English
; Description ...: Constants that can be used with UDF library
; Author(s) .....: Yashied, Jpm
; Requirements...: AutoIt v3.3 +, Developed/Tested on Windows XP Pro Service Pack 2 and Windows Vista/7
; ===============================================================================================================================

; #CONSTANTS# ===================================================================================================================
Global Const $__DLG_WM_USER = 0x400

; _WinAPI_BrowseForFolderDlg()
Global Const $BIF_BROWSEFILEJUNCTIONS = 0x00010000
Global Const $BIF_BROWSEFORCOMPUTER = 0x00001000
Global Const $BIF_BROWSEFORPRINTER = 0x00002000
Global Const $BIF_BROWSEINCLUDEFILES = 0x00004000
Global Const $BIF_BROWSEINCLUDEURLS = 0x00000080
Global Const $BIF_DONTGOBELOWDOMAIN = 0x00000002
Global Const $BIF_EDITBOX = 0x00000010
Global Const $BIF_NEWDIALOGSTYLE = 0x00000040
Global Const $BIF_NONEWFOLDERBUTTON = 0x00000200
Global Const $BIF_NOTRANSLATETARGETS = 0x00000400
Global Const $BIF_RETURNFSANCESTORS = 0x00000008
Global Const $BIF_RETURNONLYFSDIRS = 0x00000001
Global Const $BIF_SHAREABLE = 0x00008000
Global Const $BIF_STATUSTEXT = 0x00000004
Global Const $BIF_USENEWUI = BitOR($BIF_EDITBOX, $BIF_NEWDIALOGSTYLE)
Global Const $BIF_UAHINT = 0x00000100
Global Const $BIF_VALIDATE = 0x00000020

Global Const $BFFM_INITIALIZED = 1
Global Const $BFFM_IUNKNOWN = 5
Global Const $BFFM_SELCHANGED = 2
Global Const $BFFM_VALIDATEFAILED = 4

Global Const $BFFM_SETSTATUSTEXTA = $__DLG_WM_USER + 100
Global Const $BFFM_ENABLEOK = $__DLG_WM_USER + 101
Global Const $BFFM_SETSELECTIONA = $__DLG_WM_USER + 102
Global Const $BFFM_SETSELECTIONW = $__DLG_WM_USER + 103
Global Const $BFFM_SETSTATUSTEXTW = $__DLG_WM_USER + 104
Global Const $BFFM_SETOKTEXT = $__DLG_WM_USER + 105
Global Const $BFFM_SETEXPANDED = $__DLG_WM_USER + 106

; _WinAPI_CommDlgExtendedErrorEx()
Global Const $CDERR_DIALOGFAILURE = 0xFFFF
Global Const $CDERR_FINDRESFAILURE = 0x0006
Global Const $CDERR_INITIALIZATION = 0x0002
Global Const $CDERR_LOADRESFAILURE = 0x0007
Global Const $CDERR_LOADSTRFAILURE = 0x0005
Global Const $CDERR_LOCKRESFAILURE = 0x0008
Global Const $CDERR_MEMALLOCFAILURE = 0x0009
Global Const $CDERR_MEMLOCKFAILURE = 0x000A
Global Const $CDERR_NOHINSTANCE = 0x0004
Global Const $CDERR_NOHOOK = 0x000B
Global Const $CDERR_NOTEMPLATE = 0x0003
Global Const $CDERR_REGISTERMSGFAIL = 0x000C
Global Const $CDERR_STRUCTSIZE = 0x0001

Global Const $PDERR_CREATEICFAILURE = 0x100A
Global Const $PDERR_DEFAULTDIFFERENT = 0x100C
Global Const $PDERR_DNDMMISMATCH = 0x1009
Global Const $PDERR_GETDEVMODEFAIL = 0x1005
Global Const $PDERR_INITFAILURE = 0x1006
Global Const $PDERR_LOADDRVFAILURE = 0x1004
Global Const $PDERR_NODEFAULTPRN = 0x1008
Global Const $PDERR_NODEVICES = 0x1007
Global Const $PDERR_PARSEFAILURE = 0x1002
Global Const $PDERR_PRINTERNOTFOUND = 0x100B
Global Const $PDERR_RETDEFFAILURE = 0x1003
Global Const $PDERR_SETUPFAILURE = 0x1001

Global Const $CFERR_MAXLESSTHANMIN = 0x2002
Global Const $CFERR_NOFONTS = 0x2001

Global Const $FNERR_BUFFERTOOSMALL = 0x3003
Global Const $FNERR_INVALIDFILENAME = 0x3002
Global Const $FNERR_SUBCLASSFAILURE = 0x3001

Global Const $FRERR_BUFFERLENGTHZERO = 0x4001

; _WinAPI_FindText(), _WinAPI_ReplaceText()
Global Const $FR_DIALOGTERM = 0x00000040
Global Const $FR_DOWN = 0x00000001
Global Const $FR_ENABLEHOOK = 0x00000100
Global Const $FR_ENABLETEMPLATE = 0x00000200
Global Const $FR_ENABLETEMPLATEHANDLE = 0x00002000
Global Const $FR_FINDNEXT = 0x00000008
Global Const $FR_HIDEUPDOWN = 0x00004000
Global Const $FR_HIDEMATCHCASE = 0x00008000
Global Const $FR_HIDEWHOLEWORD = 0x00010000
Global Const $FR_MATCHCASE = 0x00000004
Global Const $FR_NOMATCHCASE = 0x00000800
Global Const $FR_NOUPDOWN = 0x00000400
Global Const $FR_NOWHOLEWORD = 0x00001000
Global Const $FR_REPLACE = 0x00000010
Global Const $FR_REPLACEALL = 0x00000020
Global Const $FR_SHOWHELP = 0x00000080
Global Const $FR_WHOLEWORD = 0x00000002

; _WinAPI_FormatDriveDlg()
Global Const $SHFMT_ID_DEFAULT = 0xFFFF

Global Const $SHFMT_OPT_FULL = 0x00
Global Const $SHFMT_OPT_QUICKFORMAT = 0x01
Global Const $SHFMT_OPT_SYSONLY = 0x02

Global Const $SHFMT_ERROR = -1
Global Const $SHFMT_CANCEL = -2
Global Const $SHFMT_NOFORMAT = -3

; _WinAPI_OpenFileDlg(), _WinAPI_SaveFileDlg()
Global Const $CDM_FIRST = $__DLG_WM_USER + 100
Global Const $CDM_GETSPEC = $CDM_FIRST
Global Const $CDM_GETFILEPATH = $CDM_FIRST + 1
Global Const $CDM_GETFOLDERPATH = $CDM_FIRST + 2
Global Const $CDM_GETFOLDERIDLIST = $CDM_FIRST + 3
Global Const $CDM_SETCONTROLTEXT = $CDM_FIRST + 4
Global Const $CDM_HIDECONTROL = $CDM_FIRST + 5
Global Const $CDM_SETDEFEXT = $CDM_FIRST + 6
Global Const $CDM_LAST = $__DLG_WM_USER + 200

Global Const $CDN_FIRST = -601
Global Const $CDN_INITDONE = $CDN_FIRST
Global Const $CDN_SELCHANGE = $CDN_FIRST - 1
Global Const $CDN_FOLDERCHANGE = $CDN_FIRST - 2
Global Const $CDN_SHAREVIOLATION = $CDN_FIRST - 3
Global Const $CDN_HELP = $CDN_FIRST - 4
Global Const $CDN_FILEOK = $CDN_FIRST - 5
Global Const $CDN_TYPECHANGE = $CDN_FIRST - 6
Global Const $CDN_INCLUDEITEM = $CDN_FIRST - 7
Global Const $CDN_LAST = -699

; _WinAPI_PageSetupDlg()
Global Const $PSD_DEFAULTMINMARGINS = 0x00000000
Global Const $PSD_DISABLEMARGINS = 0x00000010
Global Const $PSD_DISABLEORIENTATION = 0x00000100
Global Const $PSD_DISABLEPAGEPAINTING = 0x00080000
Global Const $PSD_DISABLEPAPER = 0x00000200
Global Const $PSD_DISABLEPRINTER = 0x00000020
Global Const $PSD_ENABLEPAGEPAINTHOOK = 0x00040000
Global Const $PSD_ENABLEPAGESETUPHOOK = 0x00002000
Global Const $PSD_ENABLEPAGESETUPTEMPLATE = 0x00008000
Global Const $PSD_ENABLEPAGESETUPTEMPLATEHANDLE = 0x00020000
Global Const $PSD_INHUNDREDTHSOFMILLIMETERS = 0x00000008
Global Const $PSD_INTHOUSANDTHSOFINCHES = 0x00000004
Global Const $PSD_MARGINS = 0x00000002
Global Const $PSD_MINMARGINS = 0x00000001
Global Const $PSD_NONETWORKBUTTON = 0x00200000
Global Const $PSD_NOWARNING = 0x00000080
Global Const $PSD_RETURNDEFAULT = 0x00000400
Global Const $PSD_SHOWHELP = 0x00000800

Global Const $WM_PSD_PAGESETUPDLG = $__DLG_WM_USER
Global Const $WM_PSD_FULLPAGERECT = $__DLG_WM_USER + 1
Global Const $WM_PSD_MINMARGINRECT = $__DLG_WM_USER + 2
Global Const $WM_PSD_MARGINRECT = $__DLG_WM_USER + 3
Global Const $WM_PSD_GREEKTEXTRECT = $__DLG_WM_USER + 4
Global Const $WM_PSD_ENVSTAMPRECT = $__DLG_WM_USER + 5
Global Const $WM_PSD_YAFULLPAGERECT = $__DLG_WM_USER + 6

; _WinAPI_PrintDlg(), _WinAPI_PrintDlgEx()
Global Const $PD_ALLPAGES = 0x00000000
Global Const $PD_COLLATE = 0x00000010
Global Const $PD_CURRENTPAGE = 0x00400000
Global Const $PD_DISABLEPRINTTOFILE = 0x00080000
Global Const $PD_ENABLEPRINTHOOK = 0x00001000
Global Const $PD_ENABLEPRINTTEMPLATE = 0x00004000
Global Const $PD_ENABLEPRINTTEMPLATEHANDLE = 0x00010000
Global Const $PD_ENABLESETUPHOOK = 0x00002000
Global Const $PD_ENABLESETUPTEMPLATE = 0x00008000
Global Const $PD_ENABLESETUPTEMPLATEHANDLE = 0x00020000
Global Const $PD_EXCLUSIONFLAGS = 0x01000000
Global Const $PD_HIDEPRINTTOFILE = 0x00100000
Global Const $PD_NOCURRENTPAGE = 0x00800000
Global Const $PD_NONETWORKBUTTON = 0x00200000
Global Const $PD_NOPAGENUMS = 0x00000008
Global Const $PD_NOSELECTION = 0x00000004
Global Const $PD_NOWARNING = 0x00000080
Global Const $PD_PAGENUMS = 0x00000002
Global Const $PD_PRINTSETUP = 0x00000040
Global Const $PD_PRINTTOFILE = 0x00000020
Global Const $PD_RETURNDC = 0x00000100
Global Const $PD_RETURNDEFAULT = 0x00000400
Global Const $PD_RETURNIC = 0x00000200
Global Const $PD_SELECTION = 0x00000001
Global Const $PD_SHOWHELP = 0x00000800
Global Const $PD_USEDEVMODECOPIES = 0x00040000
Global Const $PD_USEDEVMODECOPIESANDCOLLATE = $PD_USEDEVMODECOPIES
Global Const $PD_USELARGETEMPLATE = 0x10000000

Global Const $PD_RESULT_APPLY = 2
Global Const $PD_RESULT_CANCEL = 0
Global Const $PD_RESULT_PRINT = 1

; _WinAPI_RestartDlg()
Global Const $EWX_LOGOFF = 0
Global Const $EWX_POWEROFF = 8
Global Const $EWX_REBOOT = 2
Global Const $EWX_SHUTDOWN = 1
Global Const $EWX_FORCE = 4
Global Const $EWX_FORCEIFHUNG = 16

; _WinAPI_ShellOpenWithDlg()
Global Const $OAIF_ALLOW_REGISTRATION = 0x00000001
Global Const $OAIF_REGISTER_EXT = 0x00000002
Global Const $OAIF_EXEC = 0x00000004
Global Const $OAIF_FORCE_REGISTRATION = 0x00000008
Global Const $OAIF_HIDE_REGISTRATION = 0x00000020
Global Const $OAIF_URL_PROTOCOL = 0x00000040

; _WinAPI_ShellStartNetConnectionDlg() in WinNet.au3
; Global Const $RESOURCETYPE_ANY = 0x00
; Global Const $RESOURCETYPE_DISK = 0x01
; Global Const $RESOURCETYPE_PRINT = 0x02

; _WinAPI_ShellUserAuthenticationDlg()
Global Const $CREDUI_FLAGS_ALWAYS_SHOW_UI = 0x00000080
Global Const $CREDUI_FLAGS_COMPLETE_USERNAME = 0x00000800
Global Const $CREDUI_FLAGS_DO_NOT_PERSIST = 0x00000002
Global Const $CREDUI_FLAGS_EXCLUDE_CERTIFICATES = 0x00000008
Global Const $CREDUI_FLAGS_EXPECT_CONFIRMATION = 0x00020000
Global Const $CREDUI_FLAGS_GENERIC_CREDENTIALS = 0x00040000
Global Const $CREDUI_FLAGS_INCORRECT_PASSWORD = 0x00000001
Global Const $CREDUI_FLAGS_KEEP_USERNAME = 0x00100000
Global Const $CREDUI_FLAGS_PASSWORD_ONLY_OK = 0x00000200
Global Const $CREDUI_FLAGS_PERSIST = 0x00001000
Global Const $CREDUI_FLAGS_REQUEST_ADMINISTRATOR = 0x00000004
Global Const $CREDUI_FLAGS_REQUIRE_CERTIFICATE = 0x00000010
Global Const $CREDUI_FLAGS_REQUIRE_SMARTCARD = 0x00000100
Global Const $CREDUI_FLAGS_SERVER_CREDENTIAL = 0x00004000
Global Const $CREDUI_FLAGS_SHOW_SAVE_CHECK_BOX = 0x00000040
Global Const $CREDUI_FLAGS_USERNAME_TARGET_CREDENTIALS = 0x00080000
Global Const $CREDUI_FLAGS_VALIDATE_USERNAME = 0x00000400

; _WinAPI_ShellUserAuthenticationDlgEx()
Global Const $CREDUIWIN_AUTHPACKAGE_ONLY = 0x00000010
Global Const $CREDUIWIN_CHECKBOX = 0x00000002
Global Const $CREDUIWIN_ENUMERATE_ADMINS = 0x00000100
Global Const $CREDUIWIN_ENUMERATE_CURRENT_USER = 0x00000200
Global Const $CREDUIWIN_GENERIC = 0x00000001
Global Const $CREDUIWIN_IN_CRED_ONLY = 0x00000020
Global Const $CREDUIWIN_SECURE_PROMPT = 0x00001000
Global Const $CREDUIWIN_PACK_32_WOW = 0x10000000
Global Const $CREDUIWIN_PREPROMPTING = 0x00002000

; Global Const $CRED_PACK_GENERIC_CREDENTIALS = 0x04
; Global Const $CRED_PACK_PROTECTED_CREDENTIALS = 0x01
; Global Const $CRED_PACK_WOW_BUFFER = 0x02
; ===============================================================================================================================
