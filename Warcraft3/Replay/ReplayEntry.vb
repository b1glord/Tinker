﻿Imports Tinker.Pickling

Namespace WC3.Replay
    <DebuggerDisplay("{ToString}")>
    Public NotInheritable Class ReplayEntry
        Implements IEquatable(Of ReplayEntry)

        Private Shared ReadOnly SharedJar As New ReplayEntryJar()
        Private ReadOnly _definition As Format.Definition
        Private ReadOnly _payload As Object

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(_payload IsNot Nothing)
            Contract.Invariant(_definition IsNot Nothing)
        End Sub

        Private Sub New(ByVal definition As Format.Definition, ByVal payload As Object)
            Contract.Requires(definition IsNot Nothing)
            Contract.Requires(payload IsNot Nothing)
            Contract.Ensures(Me.Definition Is definition)
            Contract.Ensures(Me.Payload Is payload)
            Me._definition = definition
            Me._payload = payload
        End Sub
        Public Shared Function FromDefinitionAndValue(Of TPayload)(ByVal definition As Format.Definition(Of TPayload),
                                                                   ByVal payload As TPayload) As ReplayEntry
            Contract.Requires(definition IsNot Nothing)
            Contract.Requires(payload IsNot Nothing)
            Contract.Ensures(Contract.Result(Of ReplayEntry)() IsNot Nothing)
            Return New ReplayEntry(definition, payload)
        End Function

        Public ReadOnly Property Definition As Format.Definition
            Get
                Contract.Ensures(Contract.Result(Of Format.Definition)() IsNot Nothing)
                Return _definition
            End Get
        End Property
        Public ReadOnly Property Payload As Object
            Get
                Contract.Ensures(Contract.Result(Of Object)() IsNot Nothing)
                Return _payload
            End Get
        End Property

        Public Shared Widening Operator CType(ByVal value As ReplayEntry) As KeyValuePair(Of ReplayEntryId, Object)
            Contract.Requires(value IsNot Nothing)
            Return value.Definition.Id.KeyValue(value.Payload)
        End Operator
        Public Shared Widening Operator CType(ByVal value As KeyValuePair(Of ReplayEntryId, Object)) As ReplayEntry
            Contract.Ensures(Contract.Result(Of ReplayEntry)() IsNot Nothing)
            Contract.Assume(value.Value IsNot Nothing)
            Return New ReplayEntry(Format.DefinitionFor(value.Key), value.Value)
        End Operator

        Public Overloads Function Equals(ByVal other As ReplayEntry) As Boolean Implements System.IEquatable(Of ReplayEntry).Equals
            If other Is Nothing Then Return False
            Return SharedJar.Pack(Me).SequenceEqual(SharedJar.Pack(other))
        End Function
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return Me.Equals(TryCast(obj, ReplayEntry))
        End Function
        Public Overrides Function GetHashCode() As Integer
            Return _definition.Id.GetHashCode()
        End Function
        Public Overrides Function ToString() As String
            Return SharedJar.Describe(Me)
        End Function
    End Class

    Public NotInheritable Class ReplayEntryJar
        Inherits BaseJar(Of ReplayEntry)

        Private Shared ReadOnly SubJar As New KeyPrefixedJar(Of ReplayEntryId)(
            keyJar:=New EnumByteJar(Of ReplayEntryId)(),
            valueJars:=Replay.Format.AllDefinitions.ToDictionary(
                keySelector:=Function(e) e.Id,
                elementSelector:=Function(e) e.Jar))

        <ContractVerification(False)>
        Public Overrides Function Pack(ByVal value As ReplayEntry) As IEnumerable(Of Byte)
            Return SubJar.Pack(value)
        End Function

        'verification disabled due to stupid verifier (1.2.30118.5)
        <ContractVerification(False)>
        Public Overrides Function Parse(ByVal data As IReadableList(Of Byte)) As ParsedValue(Of ReplayEntry)
            Dim parsed = SubJar.Parse(data)
            Return parsed.WithValue(CType(parsed.Value, ReplayEntry))
        End Function

        <ContractVerification(False)>
        Public Overrides Function Describe(ByVal value As ReplayEntry) As String
            Return SubJar.Describe(value)
        End Function
        Public Overrides Function Parse(ByVal text As String) As ReplayEntry
            Return SubJar.Parse(text)
        End Function

        Public Overrides Function MakeControl() As IValueEditor(Of ReplayEntry)
            Dim subControl = SubJar.MakeControl()
            Return New DelegatedValueEditor(Of ReplayEntry)(
                Control:=subControl.Control,
                eventAdder:=Sub(action) AddHandler subControl.ValueChanged, Sub() action(),
                getter:=Function() subControl.Value,
                setter:=Sub(value) subControl.Value = value,
                disposer:=Sub() subControl.Dispose())
        End Function
    End Class
End Namespace
