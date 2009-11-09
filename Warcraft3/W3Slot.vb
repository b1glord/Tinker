﻿Namespace Warcraft3
    Public NotInheritable Class W3Slot
        Public ReadOnly index As Byte
        Public ReadOnly game As W3Game
        Public color As PlayerColor
        Private _team As Byte
        Public handicap As Byte = 100
        Public race As Races = Races.Random
        Private _contents As W3SlotContents
        Public locked As Lock

        Public Property Contents As W3SlotContents
            Get
                Contract.Ensures(Contract.Result(Of W3SlotContents)() IsNot Nothing)
                Return Me._contents
            End Get
            Set(ByVal value As W3SlotContents)
                Contract.Requires(value IsNot Nothing)
                Me._contents = value
            End Set
        End Property
        Public Property Team As Byte
            Get
                Contract.Ensures(Contract.Result(Of Byte)() <= 12)
                Return _team
            End Get
            Set(ByVal value As Byte)
                Contract.Requires(value <= 12)
                _team = value
            End Set
        End Property

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(_contents IsNot Nothing)
            Contract.Invariant(_team <= 12)
        End Sub

#Region "Enums"
        Public Const ObserverTeamIndex As Byte = 12
        Public Enum ComputerLevel As Byte
            Easy = 0
            Normal = 1
            Insane = 2
        End Enum
        <Flags()>
        Public Enum Races As Byte
            Human = 1 << 0
            Orc = 1 << 1
            NightElf = 1 << 2
            Undead = 1 << 3
            Random = 1 << 5
            Unlocked = 1 << 6
        End Enum
        Public Enum PlayerColor As Byte
            Red = 0
            Blue = 1
            Teal = 2
            Purple = 3
            Yellow = 4
            Orange = 5
            Green = 6
            Pink = 7
            Grey = 8
            LightBlue = 9
            DarkGreen = 10
            Brown = 11
        End Enum
        Public Enum Lock
            Unlocked = 0
            Sticky = 1
            Frozen = 2
        End Enum
        Public Enum Match
            None = 0
            Contents = 1
            Color = 2
            Index = 3
        End Enum
#End Region

        Public Sub New(ByVal game As W3Game, ByVal index As Byte)
            Me.game = game
            Me.index = index
            Me.Contents = New W3SlotContentsOpen(Me)
        End Sub
        Public ReadOnly Property MatchableId() As String
            Get
                Contract.Ensures(Contract.Result(Of String)() IsNot Nothing)
                Return (index + 1).ToString(CultureInfo.InvariantCulture)
            End Get
        End Property
        Public Function Matches(ByVal query As String) As Match
            Contract.Requires(query IsNot Nothing)
            If query = (index + 1).ToString(CultureInfo.InvariantCulture) Then Return Match.Index
            If query.ToUpperInvariant = color.ToString.ToUpperInvariant Then Return Match.Color
            If Contents.Matches(query) Then Return Match.Contents
            Return Match.None
        End Function
        Public Function Cloned() As W3Slot
            Dim slot = New W3Slot(game, index)
            slot.color = color
            slot.team = team
            slot.handicap = handicap
            slot.race = race
            slot.Contents = Contents.Clone(slot)
            slot.locked = locked
            Return slot
        End Function
        Public Function GenerateDescription() As IFuture(Of String)
            Dim s = ""
            If team = ObserverTeamIndex Then
                s = "Observer"
            Else
                s = "Team {0}, {1}, {2}".Frmt(team + 1, race, color)
            End If
            Select Case locked
                Case W3Slot.Lock.Frozen
                    s = "(LOCKED) " + s
                Case W3Slot.Lock.Sticky
                    s = "(STICKY) " + s
            End Select
            Return Contents.GenerateDescription.Select(Function(desc) Padded(s, 30) + desc)
        End Function
    End Class

    Public Enum SlotContentType
        Empty
        Computer
        Player
    End Enum
    <ContractClass(GetType(W3SlotContents.ContractClass))>
    Public MustInherit Class W3SlotContents
        Private ReadOnly _parent As W3Slot
        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(_parent IsNot Nothing)
        End Sub

        Protected Sub New(ByVal parent As W3Slot)
            Contract.Requires(parent IsNot Nothing)
            Me._parent = parent
        End Sub

#Region "Properties"
        Public Overridable ReadOnly Property ContentType() As SlotContentType
            Get
                Return SlotContentType.Empty
            End Get
        End Property
        Public ReadOnly Property Parent As W3Slot
            Get
                Contract.Ensures(Contract.Result(Of W3Slot)() IsNot Nothing)
                Return _parent
            End Get
        End Property
        Public Overridable ReadOnly Property Moveable() As Boolean
            Get
                Return True
            End Get
        End Property
        Public Overridable ReadOnly Property PlayerIndex() As Byte
            Get
                Return 0
            End Get
        End Property

        Public Enum State As Byte
            Open = 0
            Closed = 1
            Occupied = 2
        End Enum
        Public Overridable ReadOnly Property DataState(ByVal receiver As W3Player) As State
            Get
                Contract.Requires(receiver IsNot Nothing)
                Return State.Open
            End Get
        End Property
        Public Overridable ReadOnly Property DataComputerLevel() As W3Slot.ComputerLevel
            Get
                Return W3Slot.ComputerLevel.Normal
            End Get
        End Property
        Public Overridable ReadOnly Property DataPlayerIndex(ByVal receiver As W3Player) As Byte
            Get
                Contract.Requires(receiver IsNot Nothing)
                Return 0
            End Get
        End Property
        Public Overridable ReadOnly Property DataDownloadPercent(ByVal receiver As W3Player) As Byte
            Get
                Contract.Requires(receiver IsNot Nothing)
                Return 255
            End Get
        End Property
#End Region

#Region "Player Methods"
        Public Enum WantPlayerPriority
            Filled = 0
            Closed = 1
            Open = 2
            Reserved = 3
        End Enum
        Public Overridable Function WantPlayer(ByVal name As String) As WantPlayerPriority
            Return WantPlayerPriority.Filled
        End Function
        Public Overridable Function TakePlayer(ByVal player As W3Player) As W3SlotContents
            Contract.Requires(player IsNot Nothing)
            Throw New InvalidOperationException()
        End Function
        Public Overridable Function RemovePlayer(ByVal player As W3Player) As W3SlotContents
            Contract.Requires(player IsNot Nothing)
            Contract.Ensures(Contract.Result(Of W3SlotContents)() IsNot Nothing)
            Throw New InvalidOperationException()
        End Function
        Public Overridable Function EnumPlayers() As IEnumerable(Of W3Player)
            Contract.Ensures(Contract.Result(Of IEnumerable(Of W3Player))() IsNot Nothing)
            Return New W3Player() {}
        End Function
#End Region

#Region "Misc Methods"
        Public MustOverride Function GenerateDescription() As IFuture(Of String)
        Public Overridable Function Clone(ByVal parent As W3Slot) As W3SlotContents
            Contract.Requires(parent IsNot Nothing)
            Contract.Ensures(Contract.Result(Of W3SlotContents)() IsNot Nothing)
            Throw New NotSupportedException()
        End Function
        Public Overridable Function Matches(ByVal query As String) As Boolean
            Contract.Requires(query IsNot Nothing)
            Return False
        End Function
#End Region

        <ContractClassFor(GetType(W3SlotContents))>
        Public MustInherit Class ContractClass
            Inherits W3SlotContents

            Public Sub New(ByVal parent As W3Slot)
                MyBase.New(parent)
                Throw New NotSupportedException
            End Sub
            Public Overrides Function GenerateDescription() As IFuture(Of String)
                Contract.Ensures(Contract.Result(Of IFuture(Of String))() IsNot Nothing)
                Throw New NotSupportedException
            End Function
        End Class
    End Class

    Public Class W3SlotContentsOpen
        Inherits W3SlotContents
        Public Sub New(ByVal parent As W3Slot)
            MyBase.New(parent)
            Contract.Requires(parent IsNot Nothing)
        End Sub
        Public Overrides Function TakePlayer(ByVal player As W3Player) As W3SlotContents
            Return New W3SlotContentsPlayer(Parent, player)
        End Function
        Public Overrides Function WantPlayer(ByVal name As String) As WantPlayerPriority
            Return WantPlayerPriority.Open
        End Function
        Public Overrides Function GenerateDescription() As IFuture(Of String)
            Return "Open".Futurized
        End Function
        Public Overrides Function Clone(ByVal parent As W3Slot) As W3SlotContents
            Return New W3SlotContentsOpen(parent)
        End Function
    End Class

    Public Class W3SlotContentsClosed
        Inherits W3SlotContentsOpen
        Public Sub New(ByVal parent As W3Slot)
            MyBase.New(parent)
            Contract.Requires(parent IsNot Nothing)
        End Sub
        Public Overrides Function WantPlayer(ByVal name As String) As WantPlayerPriority
            Return WantPlayerPriority.Closed
        End Function
        Public Overrides Function GenerateDescription() As IFuture(Of String)
            Return "Closed".Futurized
        End Function
        Public Overrides Function Clone(ByVal parent As W3Slot) As W3SlotContents
            Return New W3SlotContentsClosed(parent)
        End Function
        Public Overrides ReadOnly Property DataState(ByVal receiver As W3Player) As State
            Get
                Return State.Closed
            End Get
        End Property
    End Class

    Public NotInheritable Class W3SlotContentsComputer
        Inherits W3SlotContentsClosed
        Private ReadOnly level As W3Slot.ComputerLevel
        Public Sub New(ByVal parent As W3Slot, ByVal level As W3Slot.ComputerLevel)
            MyBase.New(parent)
            Contract.Requires(parent IsNot Nothing)
            Me.level = level
        End Sub
        Public Overrides Function GenerateDescription() As IFuture(Of String)
            Return "Computer ({0})".Frmt(DataComputerLevel).Futurized
        End Function
        Public Overrides Function Clone(ByVal parent As W3Slot) As W3SlotContents
            Return New W3SlotContentsComputer(parent, DataComputerLevel)
        End Function
        Public Overrides ReadOnly Property ContentType() As SlotContentType
            Get
                Return SlotContentType.Computer
            End Get
        End Property
        Public Overrides ReadOnly Property DataComputerLevel() As W3Slot.ComputerLevel
            Get
                Return level
            End Get
        End Property
        Public Overrides ReadOnly Property DataState(ByVal receiver As W3Player) As State
            Get
                Return State.Occupied
            End Get
        End Property
    End Class

    Public Class W3SlotContentsPlayer
        Inherits W3SlotContents
        Protected ReadOnly player As W3Player

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(player IsNot Nothing)
        End Sub

        Public Sub New(ByVal parent As W3Slot, ByVal player As W3Player)
            MyBase.New(parent)
            Contract.Requires(parent IsNot Nothing)
            Contract.Requires(player IsNot Nothing)
            Me.player = player
        End Sub
        Public Overrides Function EnumPlayers() As IEnumerable(Of W3Player)
            Return New W3Player() {player}
        End Function
        Public Overrides Function GenerateDescription() As IFuture(Of String)
            Return If(player.isFake, "(Fake){0}".Frmt(player.name).Futurized, player.Description)
        End Function
        Public Overrides ReadOnly Property PlayerIndex() As Byte
            Get
                Return player.Index
            End Get
        End Property
        Public Overrides Function WantPlayer(ByVal name As String) As W3SlotContents.WantPlayerPriority
            If player IsNot Nothing AndAlso name IsNot Nothing AndAlso
                                            player.isFake AndAlso
                                            player.name.ToUpperInvariant = name.ToUpperInvariant Then
                Return WantPlayerPriority.Reserved
            Else
                Return WantPlayerPriority.Filled
            End If
        End Function
        Public Overrides Function Clone(ByVal parent As W3Slot) As W3SlotContents
            Return New W3SlotContentsPlayer(parent, player)
        End Function
        Public Overrides Function Matches(ByVal query As String) As Boolean
            Return query.ToUpperInvariant = player.name.ToUpperInvariant
        End Function
        Public Overrides Function RemovePlayer(ByVal targetPlayer As W3Player) As W3SlotContents
            If Me.player Is targetPlayer Then
                Return New W3SlotContentsOpen(Parent)
            Else
                Throw New InvalidOperationException()
            End If
        End Function
        Public Overrides ReadOnly Property ContentType() As SlotContentType
            Get
                Return SlotContentType.Player
            End Get
        End Property
        Public Overrides ReadOnly Property DataPlayerIndex(ByVal receiver As W3Player) As Byte
            Get
                Return player.Index
            End Get
        End Property
        Public Overrides ReadOnly Property DataDownloadPercent(ByVal receiver As W3Player) As Byte
            Get
                Return player.GetDownloadPercent
            End Get
        End Property
        Public Overrides ReadOnly Property DataState(ByVal receiver As W3Player) As State
            Get
                Return State.Occupied
            End Get
        End Property
    End Class

    Public NotInheritable Class W3SlotContentsCovering
        Inherits W3SlotContentsPlayer
        Private ReadOnly _coveredSlot As W3Slot

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(_coveredSlot IsNot Nothing)
        End Sub

        Public ReadOnly Property CoveredSlot As W3Slot
            Get
                Contract.Ensures(Contract.Result(Of W3Slot)() IsNot Nothing)
                Return _coveredSlot
            End Get
        End Property

        Public Sub New(ByVal parent As W3Slot, ByVal coveredSlot As W3Slot, ByVal player As W3Player)
            MyBase.New(parent, player)
            Contract.Requires(parent IsNot Nothing)
            Contract.Requires(player IsNot Nothing)
            Contract.Requires(coveredSlot IsNot Nothing)
            Me._coveredSlot = coveredSlot
        End Sub
        Public Overrides Function GenerateDescription() As IFuture(Of String)
            Return MyBase.GenerateDescription.Select(Function(desc) "[Covering {0}] {1}".Frmt(coveredSlot.color, desc))
        End Function
        Public Overrides Function Clone(ByVal parent As W3Slot) As W3SlotContents
            Return New W3SlotContentsCovering(parent, coveredSlot, player)
        End Function
        Public Overrides Function RemovePlayer(ByVal player As W3Player) As W3SlotContents
            Throw New InvalidOperationException()
        End Function
        Public Overrides ReadOnly Property DataDownloadPercent(ByVal receiver As W3Player) As Byte
            Get
                Return CByte(coveredSlot.contents.EnumPlayers.Count)
            End Get
        End Property
        Public Overrides ReadOnly Property Moveable() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class

    Public NotInheritable Class W3SlotContentsCovered
        Inherits W3SlotContents
        Public ReadOnly coveringSlot As W3Slot
        Private ReadOnly _playerIndex As Byte
        Private ReadOnly players As List(Of W3Player)

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(players IsNot Nothing)
            Contract.Invariant(coveringSlot IsNot Nothing)
        End Sub

        Public Sub New(ByVal parent As W3Slot, ByVal coveringSlot As W3Slot, ByVal playerIndex As Byte, ByVal players As IEnumerable(Of W3Player))
            MyBase.New(parent)
            Contract.Requires(parent IsNot Nothing)
            Contract.Requires(players IsNot Nothing)
            Contract.Requires(coveringSlot IsNot Nothing)
            Me.coveringSlot = coveringSlot
            Me.players = players.ToList
            Me._playerIndex = playerIndex
        End Sub
        Public Overrides Function GenerateDescription() As IFuture(Of String)
            Return "[Covered by {0}] Players: {1}".Frmt(coveringSlot.color, (From player In players Select player.name).StringJoin(", ")).Futurized
        End Function
        Public Overrides Function Clone(ByVal parent As W3Slot) As W3SlotContents
            Return New W3SlotContentsCovered(parent, coveringSlot, _playerIndex, players)
        End Function
        Public Overrides Function RemovePlayer(ByVal player As W3Player) As W3SlotContents
            If Not players.Contains(player) Then Throw New InvalidOperationException()
            Return New W3SlotContentsCovered(Parent, coveringSlot, _playerIndex, (From p In players Where p IsNot player))
        End Function
        Public Overrides Function TakePlayer(ByVal player As W3Player) As W3SlotContents
            If players.Contains(player) Then Throw New InvalidOperationException()
            Return New W3SlotContentsCovered(Parent, coveringSlot, _playerIndex, players.Concat(New W3Player() {player}))
        End Function
        Public Overrides ReadOnly Property DataPlayerIndex(ByVal receiver As W3Player) As Byte
            Get
                Return If(players.Contains(receiver), receiver.index, CByte(0))
            End Get
        End Property
        Public Overrides ReadOnly Property PlayerIndex() As Byte
            Get
                Return _playerIndex
            End Get
        End Property
        Public Overrides ReadOnly Property DataDownloadPercent(ByVal receiver As W3Player) As Byte
            Get
                If players.Contains(receiver) Then
                    Return receiver.GetDownloadPercent
                Else
                    Return 100
                End If
            End Get
        End Property
        Public Overrides ReadOnly Property DataState(ByVal receiver As W3Player) As State
            Get
                Return If(players.Contains(receiver), State.Occupied, State.Closed)
            End Get
        End Property
        Public Overrides ReadOnly Property ContentType() As SlotContentType
            Get
                Return SlotContentType.Player
            End Get
        End Property
        Public Overrides ReadOnly Property Moveable() As Boolean
            Get
                Return False
            End Get
        End Property
    End Class
End Namespace