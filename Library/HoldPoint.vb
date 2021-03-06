﻿<ContractClass(GetType(IHoldPointContracts(Of )))>
Public Interface IHoldPoint(Of Out TArg)
    Function IncludeTaskHandler(handler As Func(Of TArg, Task)) As IDisposable
    Function IncludeActionHandler(handler As Action(Of TArg)) As IDisposable
End Interface
<ContractClassFor(GetType(IHoldPoint(Of )))>
Public MustInherit Class IHoldPointContracts(Of TArg)
    Implements IHoldPoint(Of TArg)
    Public Function IncludeActionHandler(handler As Action(Of TArg)) As IDisposable Implements IHoldPoint(Of TArg).IncludeActionHandler
        Contract.Requires(handler IsNot Nothing)
        Contract.Ensures(Contract.Result(Of IDisposable)() IsNot Nothing)
        Throw New NotSupportedException
    End Function

    Public Function IncludeFutureHandler(handler As Func(Of TArg, Task)) As IDisposable Implements IHoldPoint(Of TArg).IncludeTaskHandler
        Contract.Requires(handler IsNot Nothing)
        Contract.Ensures(Contract.Result(Of IDisposable)() IsNot Nothing)
        Throw New NotSupportedException
    End Function
End Class

'''<summary>Allows continuing execution only once all attached handlers are finished.</summary>
Public Class HoldPoint(Of TArg)
    Implements IHoldPoint(Of TArg)
    Private ReadOnly _handlers As New List(Of Func(Of TArg, Task))
    Private ReadOnly _lock As New Object

    <ContractInvariantMethod()> Private Sub ObjectInvariant()
        Contract.Invariant(_handlers IsNot Nothing)
        Contract.Invariant(_lock IsNot Nothing)
    End Sub

    ''' <summary>
    ''' Includes a handler which is run before the resulting future from calling 'Hold' will become ready.
    ''' Returns an IDisposable which removes the handler when disposed.
    ''' </summary>
    Public Function IncludeActionHandler(handler As Action(Of TArg)) As IDisposable Implements IHoldPoint(Of TArg).IncludeActionHandler
        Return IncludeTaskHandler(Function(arg)
                                      Dim result = New TaskCompletionSource(Of NoValue)()
                                      result.SetByCalling(Sub() handler(arg))
                                      Return result.Task
                                  End Function)
    End Function
    ''' <summary>
    ''' Includes a handler whose future result must become ready before the resulting future from calling 'Hold' will become ready.
    ''' Returns an IDisposable which removes the handler when disposed.
    ''' </summary>
    Public Function IncludeTaskHandler(handler As Func(Of TArg, Task)) As IDisposable Implements IHoldPoint(Of TArg).IncludeTaskHandler
        Dim safeHandler = Async Function(arg As TArg)
                              Await handler(arg)
                          End Function
        SyncLock _lock
            _handlers.Add(safeHandler)
        End SyncLock
        Return New DelegatedDisposable(Sub()
                                           SyncLock _lock
                                               _handlers.Remove(safeHandler)
                                           End SyncLock
                                       End Sub)
    End Function

    '''<summary>Evaluates all handlers and returns a future which becomes ready once all handlers have finished.</summary>
    Public Function Hold(argument As TArg) As Task
        Contract.Ensures(Contract.Result(Of Task)() IsNot Nothing)
        Dim handlers As IEnumerable(Of Func(Of TArg, Task))
        SyncLock _lock
            handlers = _handlers.ToList
        End SyncLock
        Return Task.WhenAll(From handler In handlers Select handler(argument)).AssumeNotNull()
    End Function
End Class
