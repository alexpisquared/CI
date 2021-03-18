using System;
using System.Runtime.InteropServices;

namespace AsyncSocketLib.CI.Model
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct MessageHeader
  {
    public int m_size;
    public MessageType m_type;
    public override string ToString() => $" {m_type,-18}{m_size,4} bytes  ";
  };

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct Response
  {
    public MessageHeader m_header;
    public ResponseCode m_code;
  };

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct LoginRequest
  {
    public MessageHeader m_header;
    public fixed byte m_userName[128];
    public fixed byte m_password[32];
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct NewRequestNotification
  {
    public MessageHeader m_header;
    public int m_requestID;
  }
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  unsafe public struct ChangeRequestMessage
  {
    public MessageHeader m_header;
    public int m_requestID;
    public RequestStatus m_status;
    public uint m_doneQty;
    public fixed byte m_bbsNote[100];
  };
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  unsafe public struct ChangeRequestMessage_RMS
  {
    public MessageHeader m_header;

    public int m_updateID;
    public int m_type;
    public int m_orderID;
    public int m_parentID;
    public int m_status;
    public int m_lastShares;
    public double m_price;
    public int m_userID;
    public fixed byte m_bbsNote[101];
    public System.Runtime.InteropServices.ComTypes.FILETIME m_time;        //public Int64 m_time; -- wrong for RMS
  };
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct ChangeRequest // straight from RMSMessage.h
  {
    public MessageHeader m_messageHeader;
    public OrderUpdateData_expl m_data;
  };

  [StructLayout(LayoutKind.Explicit)]
  public unsafe struct OrderUpdateData_expl // try this if union below is of value; otherwise use Sequential layout.
  {
    [FieldOffset(sizeof(int) * 0)] public int m_updateID;
    [FieldOffset(sizeof(int) * 1)] public int m_type;
    [FieldOffset(sizeof(int) * 2)] public int m_orderID;
    [FieldOffset(sizeof(int) * 3)] public int m_parentID;
    [FieldOffset(sizeof(int) * 4)] public int m_status;
    [FieldOffset(sizeof(int) * 5)] public int m_lastShares;
    [FieldOffset(sizeof(int) * 6)] public double m_price;
    [FieldOffset(sizeof(int) * 6 + sizeof(double))] public int m_userID;
    [FieldOffset(sizeof(int) * 7 + sizeof(double))] public fixed byte m_bbsNote[101]; //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)]
                                                                                      //union 
    [FieldOffset(sizeof(int) * 7 + sizeof(double) + 101)] public System.Runtime.InteropServices.ComTypes.FILETIME m_time;
    [FieldOffset(sizeof(int) * 7 + sizeof(double) + 101)] public long  /*long long*/ m_int64Time;
  };
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct OrderUpdateData2 // straight from RMSMessage.h
  {
    public int m_updateID;
    public int m_type;
    public int m_orderID;
    public int m_parentID;
    public int m_status;
    public int m_lastShares;
    public double m_price;
    public int m_userID;
    public fixed char m_bbsNote[101]; //todo: or maybe byte instead of char.
    public System.Runtime.InteropServices.ComTypes.FILETIME m_time;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)] // from RMSClient\ServerSessions.cs - presumably wrong ... but worth trying just in case.
  public unsafe struct ChangeRequest3//todo: !!! see more at   $\\trunk\Server\RMS\Common\RMSMessage.h:170 - struct ChangeRequest  +  RMS.ChangeRequest structure.png here.
  {
    public MessageHeader m_header;
    public int m_requestID;
    public RequestStatus m_status;
    public uint m_doneQty;
    public fixed byte m_bbsNote[100];
  };

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct LoginResponse
  {
    public MessageHeader m_header;
    public ResponseCode m_code;
    public int m_userID;
    public override string ToString() => $"{m_header}  {m_code,-16}    user:{m_userID,3}";
  }
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct ChangeResponse
  {
    public MessageHeader m_header;
    public ResponseCode m_code;
    public int m_userID;
    public int m_orderID;
    public int m_updateID;
    public override string ToString() => $"{m_header}  rc:{m_code,-16}    user:{m_userID,3}    order:{m_orderID,6}    updateID:{m_updateID,4}";
  };

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct UnknownType
  {
    public object m_obj;
  };
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct RiskBaseMsg
  {
    public RiskMsgType m_type;
    public long m_seq;
    public int m_size;
    public long m_time;
    public int iGuidID;
  }
  public enum RiskMsgType // risk messages types
  {
    RISK_MSG_INFO = 0,
    RISK_MSG_INIT_PROGRESS,
    RISK_MSG_POSITION,
    RISK_MSG_DATA,
    RISK_MSG_ACCOUNT,
    RISK_MSG_MARGIN_AVAILABLE,
    RISK_MSG_LOGON,
    RISK_MSG_LOGON_ACKN,
    RISK_MSG_MARGIN_REQUEST,
    RISK_MSG_MARGIN_ADP_REQUEST,
    RISK_MSG_LOGOUT,
    RISK_MSG_INI_STATISTICS,
    RISK_MSG_UPDATE_ACCOUNT,
    RISK_MSG_OPTION_REQUEST,
    RISK_MSG_OPTION_DATA,
    RISK_MSG_NUM_OF_SYMB_REQUEST,
    RISK_MSG_NUM_OF_SYMB,
    RISK_MSG_BUY_CASH_REQUEST,
    RISK_MSG_BUY_CASH,
    RISK_MSG_HEARTBEAT,
    RISK_MSG_DLL_EVENT,
    RISK_MSG_RRSP_UPDATE_REQUEST,
    RISK_MSG_RRSP_UPDATE_ACKN,
    RISK_MSG_MAX_QTY_UPDATE_REQUEST,
    RISK_MSG_MAX_QTY_UPDATE_ACKN,
    RISK_MSG_FLIP_DB,
    RISK_MSG_FLIP_DB_ACKN,
    RISK_MSG_SWAP_CASH,
    RISK_MSG_UPDATE_EQ_POS,
    RISK_MSG_PORTFOLIO_REQUEST,
    RISK_MSG_PORTFOLIO_RESULT,
    RISK_MSG_LIQUID_MODE_REQUEST,
    RISK_MSG_BP_COEFF_REQUEST,
    RISK_MSG_CLOSING_PX_REQUEST,
    RISK_MSG_REPUBLISH_OPT_REQUEST,
    RISK_MSG_REPUBLISH_EXCEL_DATA,
    RISK_MSG_EOD_STATUS,
    RISK_MSG_MAXIMUM // like undefided
  };

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct LockOrderRequest // straight from \dev\trunk\Server\RMS\Common\RMSMessage.h
  {
    public MessageHeader m_messageHeader;
    public int m_orderID;
    public bool m_isLock;
  };
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct LockOrderResponse
  {
    public MessageHeader m_header;
    public ResponseCode m_code;
    public int m_orderID;
    public int m_userID;
    public bool m_isLock;
    public override string ToString() => $"{m_header}  rc:{m_code,-16}    user:{m_userID,3}    order:{m_orderID,6}    isLock:{m_isLock,4}";
  };
}
