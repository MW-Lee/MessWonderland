////////////////////////////////////////////////
//
// BaseScript
//
// 정보만 가진 클래스 및 구조체를 모아놓은 스크립트
// 서버 / 클라 공용
// 
// 20. 03. 12
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 전역 공통으로 사용할 변수 모음
/// </summary>
public static class Constant
{
    // ------------------------------------- //
    // Server Packet Header > nID 구분하는 변수
    // ------------------------------------- //

    // Request IN
    public const short REQ_IN = 1;

    // Result IN
    public const short RES_IN = 2;

    // Request Chat
    public const short REQ_CHAT = 6;

    // Result(Notice) Chat
    public const short NOTICE_CHAT = 7;

    // Request Make / Connect Room
    public const short REQ_ROOM = 11;

    // Result Make / Connect Room
    public const short RES_CREATE = 12;

    // Result Join
    public const short RES_JOIN = 13;

    // Notice interaction object
    public const short NOTICE_PUZZLE = 16;

    // ------------------------------------- //
    // Server Packet 사용을 위한 변수
    // ------------------------------------- //

    // Chat > Max Name Length
    public const int MAX_NAME_LEN = 17;

    // Chat > Max Message Length
    public const int MAX_MESSAGE_LEN = 512;

    // Server > Max Receive Buffer Length
    public const int MAX_RECEIVE_BUFFER_LEN = 512;
}

/// <summary>
/// 서버에서 주고받을 Packet의 Header 구조체
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8)]
public struct PACKET_HEADER
{
    // c++ 에서 short = 2byte c# 에서는 4byte 이므로 int 형으로 변경

    // Packet Header ID
    [MarshalAs(UnmanagedType.SysInt)]
    public int nID;

    // Packet Buffer Size
    [MarshalAs(UnmanagedType.SysInt)]
    public int nSize;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="id">Send시 보내는 Packet의 ID</param>
    /// <param name="size">Send시 보내는 Packet의 크기</param>
    public PACKET_HEADER(int id, int size)
    {
        nID = id;
        nSize = size;
    }
}

[StructLayout(LayoutKind.Sequential, Size = 5)]
public struct PKT_RESULT_IN
{
    [MarshalAs(UnmanagedType.Bool)]
    public bool bIsSuccess;

    [MarshalAs(UnmanagedType.SysInt)]
    public int nFlag;
}


/// <summary>
/// 받은 Packet이 Chat일 때 사용하기 위한 구조체
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 529)]
public struct PKT_NOTICE_CHAT
{
    // Chat > NickName Who Send Message
    [MarshalAs(UnmanagedType.ByValArray,SizeConst = Constant.MAX_NAME_LEN)]
    public char[] szName;

    // Chat > Message
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constant.MAX_MESSAGE_LEN)]
    public char[] szMessage;
}

/// <summary>
/// The struct for request make room
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 5)]
public struct PKT_REQ_ROOM
{
    public char cType; // c, j, l

    [MarshalAs(UnmanagedType.SysInt)]
    public int nNum;

    public PKT_REQ_ROOM(char ctype, int num)
    {
        cType = ctype;
        nNum = num;        
    }
}

/// <summary>
/// The struct for result make room
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 10)]
public struct PKT_RES_CREATE
{
    [MarshalAs(UnmanagedType.Bool)]
    public bool bSuccess;

    [MarshalAs(UnmanagedType.SysInt)]
    public int nNum;

    [MarshalAs(UnmanagedType.SysInt)]
    public int nSeq;

    public PKT_RES_CREATE(bool bFlag, int num, int seq)
    {
        bSuccess = bFlag;
        nNum = num;
        nSeq = seq;
    }
}

/// <summary>
/// The struct for result join the room
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 2)]
public struct PKT_RES_JOIN
{
    [MarshalAs(UnmanagedType.Bool)]
    public bool bSuccess;

    public PKT_RES_JOIN(bool success)
    {
        bSuccess = success;
    }
}

/// <summary>
/// The struct for notice interaction object
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct PKT_NOTICE_PUZZLE
{
    [MarshalAs(UnmanagedType.SysInt)]
    public int nPuzzleID;

    [MarshalAs(UnmanagedType.SysInt)]
    public int nNum;

    public PKT_NOTICE_PUZZLE(int c, int num)
    {
        nPuzzleID = c;
        nNum = num;
    }
}