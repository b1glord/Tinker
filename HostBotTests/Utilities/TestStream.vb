﻿Imports System.Collections.Generic
Imports Strilbrary.Collections
Imports Strilbrary.Values
Imports Tinker

Public Class TestStream
    Inherits IO.Stream
    Private ReadOnly readBuffer As New Queue(Of Byte)
    Private ReadOnly readLock As New Threading.AutoResetEvent(initialState:=False)
    Private readBufferDone As Boolean

    Private ReadOnly writeBuffer As New Queue(Of Byte)
    Private ReadOnly writeLock As New Threading.AutoResetEvent(initialState:=False)
    Private closed As Boolean

    Private ReadOnly lock As New Object()

    Public Sub EnqueueRead(data() As Byte)
        If data Is Nothing OrElse data.Length = 0 Then Return
        SyncLock lock
            If readBufferDone Then Throw New InvalidOperationException("Read buffer is done.")
            For Each e In data
                readBuffer.Enqueue(e)
            Next e
        End SyncLock
        readLock.Set()
    End Sub
    Public Sub EnqueuedReadPacket(preheader As IEnumerable(Of Byte), sizeByteCount As Integer, body As IEnumerable(Of Byte))
        EnqueueRead(Concat(preheader,
                           CUInt(preheader.Count + sizeByteCount + body.Count).Bytes.Take(sizeByteCount),
                           body).ToArray)
    End Sub
    Public Sub EnqueueClosed()
        SyncLock lock
            readBufferDone = True
        End SyncLock
        readLock.Set()
    End Sub
    Public Function RetrieveWritePacket(Optional preheaderByteCount As Integer = 2,
                                   Optional sizeByteCount As Integer = 2,
                                   Optional millisecondsTimeout As Integer = 10000) As Byte()
        Dim headerSize = preheaderByteCount + sizeByteCount
        Dim header = Me.RetrieveWriteData(length:=headerSize, millisecondsTimeout:=millisecondsTimeout)
        Dim size = CInt(header.Skip(preheaderByteCount).Take(sizeByteCount).ToUValue)
        Dim body = Me.RetrieveWriteData(length:=size - headerSize, millisecondsTimeout:=millisecondsTimeout)
        Return header.Concat(body).ToArray
    End Function

    Public Function RetrieveWriteData(length As Integer, Optional millisecondsTimeout As Integer = 10000) As Byte()
        Dim n = writeBuffer.Count
        While n < length AndAlso writeLock.WaitOne(millisecondsTimeout)
            If writeBuffer.Count = n Then Throw New InvalidOperationException("Expected more stream data.")
            n = writeBuffer.Count
        End While

        SyncLock lock
            If writeBuffer.Count < length Then
                If closed Then Throw New IO.InvalidDataException("Stream closed")
                Throw New IO.InvalidDataException("Not enough data written.")
            End If
            Dim result(0 To length - 1) As Byte
            For Each i In length.Range
                result(i) = writeBuffer.Dequeue()
            Next i
            Return result
        End SyncLock
    End Function
    Public Function RetrieveClosed(Optional timeout As Integer = 10000) As Boolean
        If closed Then Return True
        writeLock.WaitOne()
        Return closed
    End Function

    Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
        Dim numRead = 0
        While numRead < count
            While readBuffer.Count = 0
                If readBufferDone Then Return numRead
                readLock.WaitOne()
                If readBufferDone Then Return numRead
            End While

            SyncLock lock
                buffer(offset + numRead) = readBuffer.Dequeue()
            End SyncLock
            numRead += 1
        End While
        Return numRead
    End Function

    Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
        SyncLock lock
            If count <= 0 Then Return
            For i = offset To offset + count - 1
                writeBuffer.Enqueue(buffer(i))
            Next i
        End SyncLock
        writeLock.Set()
    End Sub

    Public Overrides Sub Close()
        MyBase.Close()
        SyncLock lock
            closed = True
        End SyncLock
        writeLock.Set()
    End Sub

#Region "Boilerplate"
    Public Overrides ReadOnly Property CanRead As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property CanSeek As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property CanWrite As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides Sub Flush()
    End Sub
    Public Overrides ReadOnly Property Length As Long
        Get
            Throw New NotSupportedException
        End Get
    End Property
    Public Overrides Property Position As Long
        Get
            Throw New NotSupportedException
        End Get
        Set(value As Long)
            Throw New NotSupportedException
        End Set
    End Property
    Public Overrides Function Seek(offset As Long, origin As System.IO.SeekOrigin) As Long
        Throw New NotSupportedException
    End Function
    Public Overrides Sub SetLength(value As Long)
        Throw New NotSupportedException
    End Sub
#End Region
End Class
