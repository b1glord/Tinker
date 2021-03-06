﻿Namespace Pickling
    '''<summary>Pickles values which may be included side-by-side in the data multiple times (including 0 times).</summary>
    Public NotInheritable Class RepeatedFramingJar(Of T)
        Inherits BaseJar(Of IRist(Of T))
        Private ReadOnly _subJar As IJar(Of T)
        Private ReadOnly _useSingleLineDescription As Boolean

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(_subJar IsNot Nothing)
        End Sub

        Public Sub New(subJar As IJar(Of T),
                       Optional useSingleLineDescription As Boolean = False)
            Contract.Requires(subJar IsNot Nothing)
            Me._subJar = subJar
            Me._useSingleLineDescription = useSingleLineDescription
        End Sub

        Public Overrides Function Pack(value As IRist(Of T)) As IRist(Of Byte)
            Contract.Assume(value IsNot Nothing)
            Return Concat(From item In value Select _subJar.Pack(item)).ToRist()
        End Function

        Public Overrides Function Parse(data As IRist(Of Byte)) As ParsedValue(Of IRist(Of T))
            Dim values = New List(Of T)
            Dim usedDataCount = 0
            While usedDataCount < data.Count
                Dim subParsed = _subJar.Parse(data.SkipExact(usedDataCount))
                values.Add(subParsed.Value)
                usedDataCount += subParsed.UsedDataCount
            End While

            Dim result = values.ToRist.ParsedWithDataCount(usedDataCount)
            Contract.Assume(result.UsedDataCount <= data.Count)
            Return result
        End Function

        Public Overrides Function Describe(value As IRist(Of T)) As String
            Contract.Assume(value IsNot Nothing)
            Return (From item In value Select _subJar.Describe(item)).MakeListDescription(_useSingleLineDescription)
        End Function
        Public Overrides Function Parse(text As String) As IRist(Of T)
            Return (From line In text.SplitListDescription(_useSingleLineDescription)
                    Select _subJar.Parse(line)
                    ).ToRist
        End Function

        Public Overrides Function MakeControl() As IValueEditor(Of IRist(Of T))
            Return New ListValueEditor(Of T)(_subJar)
        End Function
    End Class

    '''<summary>Pickles lists of values, where the serialized form is prefixed by the number of items.</summary>
    Public NotInheritable Class ItemCountPrefixedFramingJar(Of T)
        Inherits BaseJar(Of IRist(Of T))

        Private ReadOnly _subJar As IJar(Of T)
        Private ReadOnly _prefixSize As Integer
        Private ReadOnly _useSingleLineDescription As Boolean

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(_prefixSize > 0)
            Contract.Invariant(_prefixSize <= 8)
            Contract.Invariant(_subJar IsNot Nothing)
        End Sub

        Public Sub New(subJar As IJar(Of T),
                       prefixSize As Integer,
                       Optional useSingleLineDescription As Boolean = False)
            Contract.Requires(subJar IsNot Nothing)
            Contract.Requires(prefixSize > 0)
            If prefixSize > 8 Then Throw New ArgumentOutOfRangeException("prefixSize", "prefixSize must be less than or equal to 8.")
            Me._subJar = subJar
            Me._prefixSize = prefixSize
            Me._useSingleLineDescription = useSingleLineDescription
        End Sub

        Public Overrides Function Pack(value As IRist(Of T)) As IRist(Of Byte)
            Contract.Assume(value IsNot Nothing)
            Dim sizeData = CULng(value.Count).Bytes.Take(_prefixSize)
            Dim itemData = Concat(From item In value Select _subJar.Pack(item))
            Return sizeData.Concat(itemData).ToRist()
        End Function

        Public Overrides Function Parse(data As IRist(Of Byte)) As ParsedValue(Of IRist(Of T))
            If data.Count < _prefixSize Then Throw New PicklingNotEnoughDataException("The count prefix requires {0} bytes.".Frmt(_prefixSize))
            Dim elementCount = data.TakeExact(_prefixSize).ToUValue

            Dim values = New List(Of T)
            Dim usedDataCount = _prefixSize
            For repeat = 1UL To elementCount
                Dim subParsed = _subJar.Parse(data.SkipExact(usedDataCount))
                values.Add(subParsed.Value)
                usedDataCount += subParsed.UsedDataCount
                Contract.Assume(usedDataCount <= data.Count)
            Next repeat

            Return values.AsRist().ParsedWithDataCount(usedDataCount)
        End Function

        Public Overrides Function Describe(value As IRist(Of T)) As String
            Contract.Assume(value IsNot Nothing)
            Return (From item In value Select _subJar.Describe(item)).MakeListDescription(_useSingleLineDescription)
        End Function
        Public Overrides Function Parse(text As String) As IRist(Of T)
            Return (From line In text.SplitListDescription(_useSingleLineDescription)
                    Select _subJar.Parse(line)
                    ).ToRist
        End Function

        Public Overrides Function MakeControl() As IValueEditor(Of IRist(Of T))
            Return New ListValueEditor(Of T)(_subJar)
        End Function
    End Class

    Public NotInheritable Class ListValueEditor(Of T)
        Inherits DisposableWithTask
        Implements IValueEditor(Of IRist(Of T))

        Private ReadOnly subControls As New List(Of Entry)
        Private ReadOnly mainControl As New Panel()
        Private ReadOnly addButton As New Button() With {.Text = "Add"}
        Private ReadOnly _subJar As IJar(Of T)
        Private _ignoreValueChanged As Boolean

        Public Event ValueChanged(sender As IValueEditor(Of IRist(Of T))) Implements IValueEditor(Of IRist(Of T)).ValueChanged
        Public Event ValueChangedSimple(sender As ISimpleValueEditor) Implements ISimpleValueEditor.ValueChanged

        <ContractInvariantMethod()> Private Sub ObjectInvariant()
            Contract.Invariant(subControls IsNot Nothing)
            Contract.Invariant(mainControl IsNot Nothing)
            Contract.Invariant(addButton IsNot Nothing)
            Contract.Invariant(_subJar IsNot Nothing)
        End Sub

        Private Class Entry
            Inherits DisposableWithTask

            Private ReadOnly _subControl As IValueEditor(Of T)
            Private ReadOnly _removeControl As New Button() With {.Text = "Remove"}
            Private ReadOnly _insertControl As New Button() With {.Text = "Insert"}
            Private ReadOnly _moveUpControl As New Button() With {.Text = "Move Up"}
            Private ReadOnly _commandPanel As Panel = PanelWithControls({_removeControl, _insertControl, _moveUpControl}, leftToRight:=True, margin:=0)
            Private ReadOnly _fullPanel As Panel

            <ContractInvariantMethod()> Private Sub ObjectInvariant()
                Contract.Invariant(_subControl IsNot Nothing)
                Contract.Invariant(_removeControl IsNot Nothing)
                Contract.Invariant(_insertControl IsNot Nothing)
                Contract.Invariant(_moveUpControl IsNot Nothing)
                Contract.Invariant(_commandPanel IsNot Nothing)
                Contract.Invariant(_fullPanel IsNot Nothing)
            End Sub

            Public Sub New(jar As IJar(Of T))
                Contract.Assume(jar IsNot Nothing)
                Me._subControl = jar.MakeControl()
                Me._fullPanel = PanelWithControls({_subControl.Control, _commandPanel}, borderStyle:=BorderStyle.FixedSingle)
            End Sub

            Public ReadOnly Property FullPanel As Panel
                Get
                    Contract.Ensures(Contract.Result(Of Panel)() IsNot Nothing)
                    Return _fullPanel
                End Get
            End Property
            Public ReadOnly Property MoveUpControl As Control
                Get
                    Contract.Ensures(Contract.Result(Of Control)() IsNot Nothing)
                    Return _moveUpControl
                End Get
            End Property
            Public ReadOnly Property InsertControl As Control
                Get
                    Contract.Ensures(Contract.Result(Of Control)() IsNot Nothing)
                    Return _insertControl
                End Get
            End Property
            Public ReadOnly Property RemoveControl As Control
                Get
                    Contract.Ensures(Contract.Result(Of Control)() IsNot Nothing)
                    Return _removeControl
                End Get
            End Property
            Public ReadOnly Property SubControl As IValueEditor(Of T)
                Get
                    Contract.Ensures(Contract.Result(Of IValueEditor(Of T))() IsNot Nothing)
                    Return _subControl
                End Get
            End Property

            Protected Overrides Function PerformDispose(finalizing As Boolean) As System.Threading.Tasks.Task
                If finalizing Then Return Nothing
                _subControl.Dispose()
                _removeControl.Dispose()
                _insertControl.Dispose()
                _moveUpControl.Dispose()
                _commandPanel.Dispose()
                _fullPanel.Dispose()
                Return Nothing
            End Function
        End Class

        Public Sub New(subJar As IJar(Of T))
            Contract.Assume(subJar IsNot Nothing)
            Me._subJar = subJar
            Me.mainControl = PanelWithControls({Me.addButton}, margin:=0)
            AddHandler addButton.Click, Sub() AddEntry(layout:=True)
        End Sub
        Public ReadOnly Property Control As Control Implements ISimpleValueEditor.Control
            Get
                Return mainControl
            End Get
        End Property

        Private Sub RaiseValueChanged()
            If _ignoreValueChanged Then Return
            RaiseEvent ValueChanged(Me)
            RaiseEvent ValueChangedSimple(Me)
        End Sub

        Private Sub RefreshLayout(Optional controlsChanged As Boolean = True,
                                  Optional raise As Boolean = True)
            If controlsChanged Then
                mainControl.Controls.Clear()
                For Each e In subControls
                    mainControl.Controls.Add(e.AssumeNotNull.FullPanel)
                Next e
                mainControl.Controls.Add(addButton)
            End If
            LayoutPanel(mainControl, margin:=6, spacing:=0)
            If raise Then
                RaiseValueChanged()
            End If
        End Sub
        Private Sub ChangedValueSubControl(entry As Entry)
            Contract.Requires(entry IsNot Nothing)
            LayoutPanel(entry.FullPanel, borderStyle:=BorderStyle.FixedSingle)
            RefreshLayout(controlsChanged:=False)
        End Sub
        Private Sub RemoveSubControl(entry As Entry)
            Contract.Requires(entry IsNot Nothing)
            subControls.Remove(entry)
            RefreshLayout()
        End Sub
        Private Sub MoveUpSubControl(entry As Entry)
            Contract.Requires(entry IsNot Nothing)
            Dim p = subControls.IndexOf(entry)
            If p <= 0 Then Return
            subControls(p) = subControls(p - 1)
            subControls(p - 1) = entry
            subControls(p).AssumeNotNull.MoveUpControl.Enabled = True
            entry.MoveUpControl.Enabled = p > 1
            RefreshLayout()
        End Sub
        Private Sub InsertAboveSubControl(entry As Entry)
            Dim r = AddEntry(layout:=False)
            Dim p = subControls.IndexOf(entry)
            Contract.Assume(p >= 0)
            For i = subControls.Count - 1 To p + 1 Step -1
                subControls(i) = subControls(i - 1)
            Next i
            subControls(p) = r
            RefreshLayout()
        End Sub

        Private Function AddEntry(layout As Boolean) As Entry
            Contract.Ensures(Contract.Result(Of Entry)() IsNot Nothing)
            Dim entry = New Entry(_subJar)

            AddHandler entry.SubControl.ValueChanged, Sub() ChangedValueSubControl(entry)
            AddHandler entry.RemoveControl.Click, Sub() RemoveSubControl(entry)
            AddHandler entry.InsertControl.Click, Sub() InsertAboveSubControl(entry)
            AddHandler entry.MoveUpControl.Click, Sub() MoveUpSubControl(entry)
            entry.MoveUpControl.Enabled = subControls.Count > 0
            subControls.Add(entry)
            If layout Then RefreshLayout()

            Return entry
        End Function

        Public Property Value As IRist(Of T) Implements IValueEditor(Of IRist(Of T)).Value
            Get
                Return (From e In subControls Select (e.SubControl.Value)).ToRist
            End Get
            Set(value As IRist(Of T))
                Dim needLayout = False
                Try
                    _ignoreValueChanged = True

                    'Clear extra controls
                    If subControls.Count > value.Count Then
                        subControls.RemoveRange(value.Count, subControls.Count - value.Count)
                        needLayout = True
                    End If

                    'Fill existing controls, adding new ones as necessary
                    For Each pair In value.AssumeNotNull.ZipWithIndexes
                        Contract.Assume(pair IsNot Nothing)
                        Dim v = pair.Item1.AssumeNotNull
                        Dim i = pair.Item2
                        Contract.Assume(i >= 0)
                        If i < subControls.Count Then
                            Dim c = subControls(i).AssumeNotNull().SubControl
                            If Not v.Equals(c.Value) Then
                                c.Value = v
                                needLayout = True
                            End If
                        Else
                            AddEntry(layout:=False).SubControl.Value = v
                            needLayout = True
                        End If
                    Next pair
                Finally
                    _ignoreValueChanged = False
                End Try

                If needLayout Then RefreshLayout()
            End Set
        End Property
        Private Property ValueSimple As Object Implements ISimpleValueEditor.Value
            Get
                Return Me.Value.AssumeNotNull
            End Get
            Set(value As Object)
                Me.Value = DirectCast(value, IRist(Of T))
            End Set
        End Property

        Protected Overrides Function PerformDispose(finalizing As Boolean) As Task
            If finalizing Then Return Nothing
            For Each subControl In subControls
                Contract.Assume(subControl IsNot Nothing)
                subControl.Dispose()
            Next subControl
            mainControl.Dispose()
            addButton.Dispose()
            Return Nothing
        End Function
    End Class
End Namespace
