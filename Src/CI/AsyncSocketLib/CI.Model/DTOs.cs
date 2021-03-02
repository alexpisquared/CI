﻿using System.Runtime.InteropServices;

namespace AsyncSocketLib.CI.Model
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct MessageHeader
  {
    public int m_size;
    public MessageType m_type;
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
  public unsafe struct ChangeRequest // straight from RMSMessage.h
  {
    public MessageHeader m_messageHeader;
    public OrderUpdateData m_data;
  };

  [StructLayout(LayoutKind.Explicit)]
  public unsafe struct OrderUpdateData // try this if union below is of value; otherwise use Sequential layout.
  {
    [FieldOffset(sizeof(int) * 0)] public int m_updateID;
    [FieldOffset(sizeof(int) * 1)] public int /*UpdateType*/ m_type;
    [FieldOffset(sizeof(int) * 2)] public int m_orderID;
    [FieldOffset(sizeof(int) * 3)] public int m_parentID;
    [FieldOffset(sizeof(int) * 4)] public int /*OrderStatus*/ m_status;
    [FieldOffset(sizeof(int) * 5)] public int m_lastShares;
    [FieldOffset(sizeof(int) * 6)] public double m_price;
    [FieldOffset(sizeof(int) * 6 + sizeof(double))] public int m_userID;
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)]
    [FieldOffset(sizeof(int) * 7 + sizeof(double))] public fixed char m_bbsNote[101];
    //union 
    [FieldOffset(sizeof(int) * 7 + sizeof(double) + sizeof(char) * 101)] public System.Runtime.InteropServices.ComTypes.FILETIME m_time;
    [FieldOffset(sizeof(int) * 7 + sizeof(double) + sizeof(char) * 101)] public long  /*long long*/ m_int64Time;
  };

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public unsafe struct OrderUpdateData2 // straight from RMSMessage.h
  {
    public int m_updateID;
    public int /*UpdateType*/ m_type;
    public int m_orderID;
    public int m_parentID;
    public int /*OrderStatus*/ m_status;
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
}