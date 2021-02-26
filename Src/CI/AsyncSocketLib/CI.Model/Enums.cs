using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncSocketLib.CI.Model
{
  public enum MessageType
  {
    mtNotSet = 0,
    mtResponse = 1,
    mtLogin = 2,
    mtChangeRequest = 3,
    mtNewRequestNotification = 4
  };
  public enum ResponseCode
  {
    rcNotSet = -1,
    rcOK = 0,
    rcUserNotFound = 1,
    rcNotLoggedIn = 2,
    rcInternalError = 3,
    rcChangeRequestError = 4
  }
  public enum RequestStatus
  {
    rsSent = 1,
    rcReceived = 2,
    rsProcessing = 3,
    rsRejected = 4,
    rsCancelled = 5,
    rsPartialyDone = 6,
    rsDone = 7,
    rsCancelRequested = 8 // new 2021
  };
}
