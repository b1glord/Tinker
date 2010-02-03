﻿Imports Strilbrary.Collections
Imports Strilbrary.Streams
Imports Strilbrary.Values
Imports Strilbrary.Time
Imports Strilbrary.Threading
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Tinker
Imports Tinker.WC3
Imports Tinker.WC3.Protocol
Imports Tinker.Pickling
Imports System.Collections.Generic

<TestClass()>
Public Class DownloadManagerTest
    Private Class TestGame
        Implements IGameDownloadAspect
        Public Shared ReadOnly HostPid As New PID(1)
        Private Shared ReadOnly _data As Byte() = Enumerable.Repeat(CByte(1), 5000).ToArray
        Private ReadOnly _startPlayerHoldPoint As New HoldPoint(Of IPlayerDownloadAspect)()
        Private Shared ReadOnly SharedMap As New Map(
            streamFactory:=Function() New IO.MemoryStream(_data, writable:=False),
            advertisedPath:="TestMap",
            fileSize:=CUInt(_data.Length),
            fileChecksumCRC32:=_data.CRC32,
            mapChecksumxoro:=1,
            mapChecksumSHA1:=Enumerable.Repeat(CByte(1), 20).ToArray.AsReadableList,
            playableWidth:=256,
            playableHeight:=256,
            isMelee:=True,
            name:="Test",
            slots:={New Slot(1, False)}.AsReadableList)
        Private ReadOnly outQueue As New TaskedCallQueue()
        Private ReadOnly _players As New AsyncViewableCollection(Of TestPlayer)(outQueue:=outQueue)
        Private ReadOnly _logger As New Logger
        Public ReadOnly Property Logger As Logger Implements IGameDownloadAspect.Logger
            Get
                Return _logger
            End Get
        End Property
        Public ReadOnly Property Map As Map Implements IGameDownloadAspect.Map
            Get
                Return SharedMap
            End Get
        End Property
        Public ReadOnly Property StartPlayerHoldPoint As HoldPoint(Of IPlayerDownloadAspect) Implements IGameDownloadAspect.StartPlayerHoldPoint
            Get
                Return _startPlayerHoldPoint
            End Get
        End Property
        Public Function QueueCreatePlayersAsyncView(ByVal adder As Action(Of IGameDownloadAspect, IPlayerDownloadAspect),
                                                    ByVal remover As Action(Of IGameDownloadAspect, IPlayerDownloadAspect)) As IFuture(Of IDisposable) _
                                                    Implements IGameDownloadAspect.QueueCreatePlayersAsyncView
            SyncLock Me
                Return _players.BeginSync(adder:=Sub(sender, player) adder(Me, player),
                                          remover:=Sub(sender, player) remover(Me, player)).Futurized
            End SyncLock
        End Function
        Public Function QueueSendMapPiece(ByVal player As IPlayerDownloadAspect, ByVal position As UInteger) As IFuture Implements IGameDownloadAspect.QueueSendMapPiece
            Return CType(player, TestPlayer).QueueSendPacket(MakeMapFileData(
                    position,
                    Enumerable.Repeat(CByte(1), CInt(Math.Min(Packets.MaxFileDataSize, Map.FileSize - position))).ToArray.AsReadableList,
                    player.PID,
                    HostPid))
        End Function
        Public Function AddPlayer(ByVal player As TestPlayer) As ifuture
            SyncLock Me
                _players.Add(player)
                Return _startPlayerHoldPoint.Hold(player)
            End SyncLock
        End Function
        Public Sub RemovePlayer(ByVal player As TestPlayer)
            SyncLock Me
                _players.Remove(player)
            End SyncLock
        End Sub
    End Class
    Private Class TestPlayer
        Inherits FutureDisposable
        Implements IPlayerDownloadAspect
        Private ReadOnly _failFuture As New FutureAction()
        Private ReadOnly _pid As PID
        Private ReadOnly _pq As New Queue(Of Packet)()
        Private ReadOnly _lock As New System.Threading.ManualResetEvent(initialState:=False)
        Private ReadOnly _handler As New W3PacketHandler("TestSource")
        Private ReadOnly _logger As Logger
        Private ReadOnly _name As InvariantString
        Public Sub New(ByVal pid As PID,
                       ByVal logger As Logger,
                       Optional ByVal name As InvariantString? = Nothing)
            Me._pid = pid
            Me._logger = logger
            Me._name = If(name Is Nothing, "TestPlayer{0}".Frmt(pid.Index), name.Value.ToString)
        End Sub
        Public Function MakePacketOtherPlayerJoined() As Packet Implements IPlayerDownloadAspect.MakePacketOtherPlayerJoined
            Return MakeOtherPlayerJoined(Name, PID, 0, New Net.IPEndPoint(Net.IPAddress.Loopback, 6112))
        End Function
        Public ReadOnly Property Name As InvariantString Implements IPlayerDownloadAspect.Name
            Get
                Return _name
            End Get
        End Property
        Public ReadOnly Property PID As PID Implements IPlayerDownloadAspect.PID
            Get
                Return _pid
            End Get
        End Property
        Private Function QueueAddPacketHandler(Of T)(ByVal id As PacketId, ByVal jar As IParseJar(Of T), ByVal handler As Func(Of IPickle(Of T), IFuture)) As IFuture(Of IDisposable) Implements IPlayerDownloadAspect.QueueAddPacketHandler
            SyncLock Me
                Return _handler.AddHandler(id, Function(data)
                                                   Dim result = handler(jar.Parse(data))
                                                   result.Catch(Sub(ex) _failFuture.TrySetFailed(ex))
                                                   Return result
                                               End Function).Futurized
            End SyncLock
        End Function
        Public Function QueueSendPacket(ByVal packet As Packet) As IFuture Implements IPlayerDownloadAspect.QueueSendPacket
            SyncLock Me
                _pq.Enqueue(packet)
                _lock.Set()
                Dim result = New FutureAction()
                result.SetSucceeded()
                Return result
            End SyncLock
        End Function

        Public Function InjectReceivedPacket(ByVal packet As Packet) As IFuture
            SyncLock Me
                Return _handler.HandlePacket(
                        New Byte() {Packets.PacketPrefix, packet.id}.Concat(
                                    CUShort(packet.Payload.Data.Count + 4).Bytes).Concat(
                                    packet.Payload.Data).ToArray.AsReadableList
                ).Catch(Sub(ex) _failFuture.SetFailed(ex))
            End SyncLock
        End Function
        Public Function TryInterceptSentPacket(Optional ByVal timeoutMilliseconds As Integer = 10000) As Packet
            SyncLock Me
                If _pq.Count > 0 Then Return _pq.Dequeue
                _lock.Reset()
            End SyncLock
            _lock.WaitOne(timeoutMilliseconds)
            SyncLock Me
                If _pq.Count > 0 Then Return _pq.Dequeue
            End SyncLock
            Return Nothing
        End Function
        Public Sub ExpectNoPacket(Optional ByVal timeoutMilliseconds As Integer = 10)
            Dim packet = TryInterceptSentPacket(timeoutMilliseconds)
            If packet IsNot Nothing Then
                Throw New IO.InvalidDataException("Unexpected packet.")
            End If
        End Sub
        Public Sub ExpectSentPacket(ByVal expected As Packet, Optional ByVal timeoutMilliseconds As Integer = 10000)
            Dim received = TryInterceptSentPacket(timeoutMilliseconds)
            If received Is Nothing Then Throw New IO.IOException("No sent packet intercepted.")
            If expected.id <> received.id Then
                Throw New IO.InvalidDataException("Incorrect packet type: received {0} instead of {1}.".Frmt(received.id, expected.id))
            ElseIf Not expected.Payload.Data.SequenceEqual(received.Payload.Data) Then
                Throw New IO.InvalidDataException("Incorrect packet contents: received {0} instead of {1}.".Frmt(received.Payload.Description.Value, expected.Payload.Description.Value))
            End If
        End Sub
        Public ReadOnly Property FailFuture As IFuture
            Get
                Return _failFuture
            End Get
        End Property
        Public Function InjectClientMapInfo(ByVal state As MapTransferState, ByVal position As UInt32) As ifuture
            Return InjectReceivedPacket(MakeClientMapInfo(state, position))
        End Function
        Public Function InjectMapDataReceived(ByVal position As UInt32, ByVal senderPid As PID) As ifuture
            Return InjectReceivedPacket(MakeMapFileDataReceived(senderPid, PID, position))
        End Function
    End Class

    <TestMethod()>
    Public Sub NoDownload()
        Dim game = New TestGame()
        Dim clock = New ManualClock()
        Dim dm = New DownloadManager(clock, game, allowDownloads:=True, allowUploads:=True)
        Dim player = New TestPlayer(New PID(2), game.Logger)
        BlockOnFuture(game.AddPlayer(player))

        player.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        clock.Advance(DownloadManager.UpdatePeriod)
        player.ExpectNoPacket()
    End Sub

    <TestMethod()>
    Public Sub HostDownload()
        Dim game = New TestGame()
        Dim clock = New ManualClock()
        Dim dm = New DownloadManager(clock, game, allowDownloads:=True, allowUploads:=True)
        Dim dler = New TestPlayer(New PID(2), game.Logger)
        BlockOnFuture(game.AddPlayer(dler))

        dler.InjectClientMapInfo(MapTransferState.Idle, 0)
        dler.ExpectNoPacket()
        clock.Advance(DownloadManager.UpdatePeriod)
        For p = Packets.MaxFileDataSize To game.Map.FileSize Step Packets.MaxFileDataSize
            dler.ExpectSentPacket(MakeMapFileData(p - Packets.MaxFileDataSize,
                                                  Enumerable.Repeat(CByte(1), Packets.MaxFileDataSize).ToArray.AsReadableList,
                                                  dler.PID,
                                                  TestGame.HostPid))
            dler.InjectClientMapInfo(MapTransferState.Idle, p)
        Next p
        dler.ExpectSentPacket(MakeMapFileData(game.Map.FileSize.FloorMultiple(Packets.MaxFileDataSize),
                                                Enumerable.Repeat(CByte(1), CInt(game.Map.FileSize Mod Packets.MaxFileDataSize)).ToArray.AsReadableList,
                                                dler.PID,
                                                TestGame.HostPid))
        dler.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        dler.ExpectNoPacket()

        Assert.IsFalse(BlockOnFuture(dler.FailFuture, 50.Milliseconds))
    End Sub

    <TestMethod()>
    Public Sub PeerDownload()
        Dim game = New TestGame()
        Dim clock = New ManualClock()
        Dim dm = New DownloadManager(clock, game, allowDownloads:=True, allowUploads:=False)
        Dim uler = New TestPlayer(New PID(2), game.Logger)
        Dim dler = New TestPlayer(New PID(3), game.Logger)
        BlockOnFuture(game.AddPlayer(dler))
        BlockOnFuture(game.AddPlayer(uler))

        dler.InjectClientMapInfo(MapTransferState.Idle, 0)
        uler.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        dler.InjectReceivedPacket(MakePeerConnectionInfo({uler.PID}))
        uler.InjectReceivedPacket(MakePeerConnectionInfo({dler.PID}))
        uler.ExpectNoPacket()
        dler.ExpectNoPacket()
        clock.Advance(DownloadManager.UpdatePeriod)

        dler.ExpectSentPacket(MakeSetDownloadSource(uler.PID))
        uler.ExpectSentPacket(MakeSetUploadTarget(dler.PID, 0))
        dler.ExpectNoPacket()
        uler.ExpectNoPacket()
        dler.InjectClientMapInfo(MapTransferState.Downloading, 0)
        uler.InjectClientMapInfo(MapTransferState.Uploading, game.Map.FileSize)
        dler.InjectClientMapInfo(MapTransferState.Downloading, game.Map.FileSize)
        dler.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        uler.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)

        Assert.IsFalse(BlockOnFuture(dler.FailFuture, 50.Milliseconds))
        Assert.IsFalse(BlockOnFuture(uler.FailFuture, 50.Milliseconds))
    End Sub

    <TestMethod()>
    Public Sub PeerDownloadFail_NoUploader()
        Dim game = New TestGame()
        Dim clock = New ManualClock()
        Dim dm = New DownloadManager(clock, game, allowDownloads:=True, allowUploads:=False)
        Dim uler = New TestPlayer(New PID(2), game.Logger)
        Dim dler = New TestPlayer(New PID(3), game.Logger)
        BlockOnFuture(game.AddPlayer(dler))
        BlockOnFuture(game.AddPlayer(uler))

        dler.InjectClientMapInfo(MapTransferState.Idle, 0)
        uler.InjectClientMapInfo(MapTransferState.Idle, 0)
        dler.InjectReceivedPacket(MakePeerConnectionInfo({uler.PID}))
        uler.InjectReceivedPacket(MakePeerConnectionInfo({dler.PID}))
        clock.Advance(DownloadManager.UpdatePeriod)

        uler.ExpectNoPacket()
        dler.ExpectNoPacket()
        Assert.IsFalse(BlockOnFuture(dler.FailFuture, 50.Milliseconds))
        Assert.IsFalse(BlockOnFuture(uler.FailFuture, 50.Milliseconds))
    End Sub
    <TestMethod()>
    Public Sub PeerDownloadFail_NoPeerConnectionInfo()
        Dim game = New TestGame()
        Dim clock = New ManualClock()
        Dim dm = New DownloadManager(clock, game, allowDownloads:=True, allowUploads:=False)
        Dim uler = New TestPlayer(New PID(2), game.Logger)
        Dim dler = New TestPlayer(New PID(3), game.Logger)
        BlockOnFuture(game.AddPlayer(dler))
        BlockOnFuture(game.AddPlayer(uler))

        dler.InjectClientMapInfo(MapTransferState.Idle, 0)
        uler.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        clock.Advance(DownloadManager.UpdatePeriod)

        uler.ExpectNoPacket()
        dler.ExpectNoPacket()
        Assert.IsFalse(BlockOnFuture(dler.FailFuture, 50.Milliseconds))
        Assert.IsFalse(BlockOnFuture(uler.FailFuture, 50.Milliseconds))
    End Sub

    <TestMethod()>
    Public Sub PeerDownload_TimeoutSwitch()
        Dim game = New TestGame()
        Dim clock = New ManualClock()
        Dim dm = New DownloadManager(clock, game, allowDownloads:=True, allowUploads:=False)
        Dim dler = New TestPlayer(New PID(2), game.Logger, "dler")
        Dim uler1 = New TestPlayer(New PID(3), game.Logger, "uler1")
        Dim uler2 = New TestPlayer(New PID(4), game.Logger, "uler2")

        'Start initial transfer
        WaitUntilFutureSucceeds(game.AddPlayer(dler))
        WaitUntilFutureSucceeds(game.AddPlayer(uler1))
        dler.InjectClientMapInfo(MapTransferState.Idle, 0)
        uler1.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        dler.InjectReceivedPacket(MakePeerConnectionInfo({uler1.PID}))
        WaitUntilFutureSucceeds(uler1.InjectReceivedPacket(MakePeerConnectionInfo({dler.PID})))
        uler1.ExpectNoPacket()
        dler.ExpectNoPacket()
        clock.Advance(DownloadManager.UpdatePeriod)

        'Check for transfer
        dler.ExpectSentPacket(MakeSetDownloadSource(uler1.PID))
        uler1.ExpectSentPacket(MakeSetUploadTarget(dler.PID, 0))
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()

        'Cancel and start new transfer
        WaitUntilFutureSucceeds(game.AddPlayer(uler2))
        uler2.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        uler2.InjectReceivedPacket(MakePeerConnectionInfo({dler.PID}))
        WaitUntilFutureSucceeds(dler.InjectReceivedPacket(MakePeerConnectionInfo({uler1.PID, uler2.PID})))
        clock.Advance(DownloadManager.FreezePeriod)
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()
        uler2.ExpectNoPacket()
        clock.Advance(DownloadManager.UpdatePeriod)

        'Check for cancellation
        dler.ExpectSentPacket(MakeOtherPlayerLeft(uler1.PID, PlayerLeaveType.Disconnect))
        uler1.ExpectSentPacket(MakeOtherPlayerLeft(dler.PID, PlayerLeaveType.Disconnect))
        dler.ExpectSentPacket(uler1.MakePacketOtherPlayerJoined())
        uler1.ExpectSentPacket(dler.MakePacketOtherPlayerJoined())
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()
        uler2.ExpectNoPacket()
        clock.Advance(DownloadManager.UpdatePeriod)

        'Check for new transfer
        dler.ExpectSentPacket(MakeSetDownloadSource(uler2.PID))
        uler2.ExpectSentPacket(MakeSetUploadTarget(dler.PID, 0))
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()
        uler2.ExpectNoPacket()

        Assert.IsFalse(BlockOnFuture(dler.FailFuture, 50.Milliseconds))
        Assert.IsFalse(BlockOnFuture(uler1.FailFuture, 50.Milliseconds))
        Assert.IsFalse(BlockOnFuture(uler2.FailFuture, 50.Milliseconds))
    End Sub

    <TestMethod()>
    Public Sub PeerDownload_SlowSwitch()
        Dim game = New TestGame()
        Dim clock = New ManualClock()
        Dim dm = New DownloadManager(clock, game, allowDownloads:=True, allowUploads:=False)
        Dim dler = New TestPlayer(New PID(2), game.Logger, "dler")
        Dim uler1 = New TestPlayer(New PID(3), game.Logger, "uler1")
        Dim uler2 = New TestPlayer(New PID(4), game.Logger, "uler2")

        'Start initial transfer
        WaitUntilFutureSucceeds(game.AddPlayer(dler))
        WaitUntilFutureSucceeds(game.AddPlayer(uler1))
        dler.InjectClientMapInfo(MapTransferState.Idle, 0)
        uler1.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        dler.InjectReceivedPacket(MakePeerConnectionInfo({uler1.PID}))
        WaitUntilFutureSucceeds(uler1.InjectReceivedPacket(MakePeerConnectionInfo({dler.PID})))
        uler1.ExpectNoPacket()
        dler.ExpectNoPacket()
        clock.Advance(DownloadManager.UpdatePeriod)

        'Check for transfer
        dler.ExpectSentPacket(MakeSetDownloadSource(uler1.PID))
        uler1.ExpectSentPacket(MakeSetUploadTarget(dler.PID, 0))
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()

        'Add second peer
        WaitUntilFutureSucceeds(game.AddPlayer(uler2))
        uler2.InjectClientMapInfo(MapTransferState.Idle, game.Map.FileSize)
        uler2.InjectReceivedPacket(MakePeerConnectionInfo({dler.PID}))
        dler.InjectReceivedPacket(MakePeerConnectionInfo({uler1.PID, uler2.PID}))

        'Go slowly
        Dim dt = DownloadManager.MinSwitchPeriod
        Dim i = 0UI
        While dt >= DownloadManager.UpdatePeriod
            clock.Advance(DownloadManager.UpdatePeriod)
            dt -= DownloadManager.UpdatePeriod
            i += 1UI
            WaitUntilFutureSucceeds(dler.InjectClientMapInfo(MapTransferState.Downloading, i))
        End While
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()
        uler2.ExpectNoPacket()

        'Check for cancellation
        clock.Advance(DownloadManager.UpdatePeriod)
        dler.ExpectSentPacket(MakeOtherPlayerLeft(uler1.PID, PlayerLeaveType.Disconnect))
        uler1.ExpectSentPacket(MakeOtherPlayerLeft(dler.PID, PlayerLeaveType.Disconnect))
        dler.ExpectSentPacket(uler1.MakePacketOtherPlayerJoined())
        uler1.ExpectSentPacket(dler.MakePacketOtherPlayerJoined())
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()
        uler2.ExpectNoPacket()
        WaitUntilFutureSucceeds(dler.InjectClientMapInfo(MapTransferState.Idle, i))
        clock.Advance(DownloadManager.UpdatePeriod)

        'Check for new transfer
        dler.ExpectSentPacket(MakeSetDownloadSource(uler2.PID))
        uler2.ExpectSentPacket(MakeSetUploadTarget(dler.PID, i))
        dler.ExpectNoPacket()
        uler1.ExpectNoPacket()
        uler2.ExpectNoPacket()

        Assert.IsFalse(BlockOnFuture(dler.FailFuture, 50.Milliseconds))
        Assert.IsFalse(BlockOnFuture(uler1.FailFuture, 50.Milliseconds))
        Assert.IsFalse(BlockOnFuture(uler2.FailFuture, 50.Milliseconds))
    End Sub
End Class